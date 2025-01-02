using GameHelper;
using GameHelper.RemoteEnums;
using GameHelper.RemoteEnums.Entity;
using GameHelper.RemoteObjects.Components;
using GameHelper.RemoteObjects.States.InGameStateObjects;
using GameOffsets.Objects.States.InGameState;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tracker
{
    /// <summary>
    /// StatusEffect
    /// </summary>
    /// <param name="settings"></param>
    public class StatusEffect(TrackerSettings settings)
    {
        private TrackerSettings Settings { get; } = settings;
        private List<SettingsParameters> MonsterStatusEffects => SettingsParameters.GetSettings(Settings);

        /// <summary>
        /// Draw
        /// </summary>
        public void Draw()
        {
            foreach (var entity in GetMonsters())
            {
                if (!entity.TryGetComponent<Render>(out var entityRender)) return;
                if (!entity.TryGetComponent<Buffs>(out var entityBuffs)) return;

                var effects = MonsterStatusEffects.Where(mse => entityBuffs.StatusEffects.ContainsKey(mse.StatusEffect));
                if (!effects.Any()) return;

                var drawList = ImGui.GetBackgroundDrawList();
                var entitylocation = Core.States.InGameStateObject.CurrentWorldInstance.WorldToScreen(entityRender.WorldPosition);

                for (var i = 0; i < effects.Count(); i++)
                {
                    var effect = effects.ElementAt(i);
                    entitylocation.Y += i * 20;
                    drawList.AddText(entitylocation, effect.Color, effect.DisplayName);
                }
            }
        }

        private IEnumerable<Entity> GetMonsters()
        {
            var areaInstance = Core.States.InGameStateObject.CurrentAreaInstance;
            var monsters = areaInstance.AwakeEntities.Where(IsValidMonster).Select(entity => entity.Value);
            return monsters.Where(entity => entity.TryGetComponent<ObjectMagicProperties>(out var comp) && comp.Rarity != Rarity.Normal);
        }

        private bool IsValidMonster(KeyValuePair<EntityNodeKey, Entity> entity)
        {
            return
                entity.Value.IsValid &&
                entity.Value.EntityState == EntityStates.None &&
                entity.Value.EntityType == EntityTypes.Monster;
        }

        private sealed class SettingsParameters
        {
            public string StatusEffect { get; set; }
            public string DisplayName { get; set; }
            public uint Color { get; set; }

            public SettingsParameters(string settings)
            {
                var split = settings.Split('|');
                StatusEffect = split[0].Trim();
                DisplayName = split[1].Trim();
                Color = Convert.ToUInt32(split[2].Trim(), 16);
            }

            public static List<SettingsParameters> GetSettings(TrackerSettings settings)
            {
                return settings.MonsterStatusEffects
                    .Split('\n')
                    .Where(item => !string.IsNullOrWhiteSpace(item))
                    .Select(item => new SettingsParameters(item)).ToList();
            }
        }
    }
}
