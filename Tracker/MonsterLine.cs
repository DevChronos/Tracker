using GameHelper;
using GameHelper.RemoteEnums;
using GameHelper.RemoteEnums.Entity;
using GameHelper.RemoteObjects.Components;
using GameHelper.RemoteObjects.States.InGameStateObjects;
using GameHelper.Utils;
using GameOffsets.Objects.States.InGameState;
using ImGuiNET;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Tracker
{
    /// <summary>
    /// MonsterLine
    /// </summary>
    /// <param name="settings"></param>
    public class MonsterLine(TrackerSettings settings)
    {
        private TrackerSettings Settings { get; } = settings;

        /// <summary>
        /// Draw
        /// </summary>
        public void Draw()
        {
            var player = Core.States.InGameStateObject.CurrentAreaInstance.Player;

            if (!player.TryGetComponent<Render>(out var playerRender)) return;
            var playerlocation = Core.States.InGameStateObject.CurrentWorldInstance.WorldToScreen(playerRender.WorldPosition);

            if (Settings.ShowUniqueLine)
            {
                foreach (var entity in GetMonsters(Rarity.Unique))
                    _drawMonsterLine(entity, Settings.UniqueLineColor);
            }

            if (Settings.ShowRareLine)
            {
                foreach (var entity in GetMonsters(Rarity.Rare))
                    _drawMonsterLine(entity, Settings.RareLineColor);
            }

            if (Settings.ShowMagicLine)
            {
                foreach (var entity in GetMonsters(Rarity.Magic))
                    _drawMonsterLine(entity, Settings.MagicLineColor);
            }

            void _drawMonsterLine(Entity entity, Vector4 color)
            {
                if (!entity.TryGetComponent<Render>(out var entityRender)) return;

                var drawList = ImGui.GetBackgroundDrawList();
                var entitylocation = Core.States.InGameStateObject.CurrentWorldInstance.WorldToScreen(entityRender.WorldPosition);

                drawList.AddLine(playerlocation, entitylocation, ImGuiHelper.Color(color), 1.0f);
            }
        }

        private IEnumerable<Entity> GetMonsters()
        {
            var areaInstance = Core.States.InGameStateObject.CurrentAreaInstance;
            return areaInstance.AwakeEntities.Where(IsValidMonster).Select(entity => entity.Value);
        }

        private IEnumerable<Entity> GetMonsters(Rarity rarity)
        {
            return GetMonsters().Where(entity => entity.TryGetComponent<ObjectMagicProperties>(out var comp) && comp.Rarity == rarity);
        }

        private bool IsValidMonster(KeyValuePair<EntityNodeKey, Entity> entity)
        {
            return
                entity.Value.IsValid &&
                entity.Value.EntityState == EntityStates.None &&
                entity.Value.EntityType == EntityTypes.Monster;
        }
    }
}
