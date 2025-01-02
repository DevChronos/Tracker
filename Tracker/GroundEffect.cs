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
        private List<SettingsParameters> GroundEffects => SettingsParameters.GetSettings(Settings);

        /// <summary>
        /// Draw
        /// </summary>
        public void Draw()
        {
            var areaInstance = Core.States.InGameStateObject.CurrentAreaInstance;
            var groundEffects = areaInstance.AwakeEntities
                .Where(entity => GroundEffects.Exists(effect => entity.Value.Path.StartsWith(effect.GroundEffect)))
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

        private sealed class SettingsParameters
        {
            public string GroundEffect { get; set; }

            public SettingsParameters(string settings)
            {
                var split = settings.Split('|');
                GroundEffect = split[0].Trim();
            }

            public static List<SettingsParameters> GetSettings(TrackerSettings settings)
            {
                return settings.GroundEffects
                    .Split('\n')
                    .Where(item => !string.IsNullOrWhiteSpace(item))
                    .Select(item => new SettingsParameters(item)).ToList();
            }
        }
    }
}
