using GameHelper;
using GameHelper.RemoteObjects.Components;
using GameHelper.Utils;
using ImGuiNET;
using System.Collections.Generic;
using System.Linq;

namespace Tracker
{
    /// <summary>
    /// GroundEffect
    /// </summary>
    /// <param name="settings"></param>
    public class GroundEffect(TrackerSettings settings)
    {
        private TrackerSettings Settings { get; } = settings;

        /// <summary>
        /// Draw
        /// </summary>
        public void Draw()
        {
            var areaInstance = Core.States.InGameStateObject.CurrentAreaInstance;
            var effects = new List<string> {
                "Metadata/Effects/Spells/ground_effects/VisibleServerGroundEffect",
                "Metadata/Monsters/MonsterMods/GroundOnDeath/BurningGroundDaemon",
                "Metadata/Monsters/MonsterMods/GroundOnDeath/ColdSnapGroundDaemon",
                "Metadata/Monsters/MonsterMods/OnDeathLightningExplosion",
                "Metadata/Monsters/MonsterMods/OnDeathFireExplosion"
            };

            var groundEffects = areaInstance.AwakeEntities
                .Where(entity => effects.Exists(effect => entity.Value.Path.StartsWith(effect)))
                .Select(entity => entity.Value);

            foreach (var entity in groundEffects)
            {
                if (!entity.TryGetComponent<Render>(out var entityRender)) continue;
                if (!entity.TryGetComponent<Positioned>(out var entityPositioned) || entityPositioned.IsFriendly) continue;

                var drawList = ImGui.GetBackgroundDrawList();
                var entitylocation = Core.States.InGameStateObject.CurrentWorldInstance.WorldToScreen(entityRender.WorldPosition);
                drawList.AddCircle(entitylocation, 100.0f, ImGuiHelper.Color(Settings.GroundEffectColor));
            }
        }
    }
}
