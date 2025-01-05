using GameHelper;
using GameHelper.RemoteObjects.Components;
using GameHelper.RemoteObjects.States.InGameStateObjects;
using GameHelper.Utils;
using ImGuiNET;
using System.Linq;

namespace Tracker
{
    public class EntityWithSettings
    {
        public Entity Entity { get; set; }
        public GroundEffectSettings GroundEffect { get; set; }
    }

    public class GroundEffectLogic(TrackerSettings settings)
    {
        private TrackerSettings Settings { get; } = settings;

        public void Draw()
        {
            var areaInstance = Core.States.InGameStateObject.CurrentAreaInstance;
            var matchingEntities = areaInstance.AwakeEntities
                .Select(entity => new
                {
                    Entity = entity.Value,
                    GroundEffect = Settings.GroundEffects.Find(effect => entity.Value.Path.StartsWith(effect.Path) && effect.IsEnabled)
                })
                .Where(entity => entity.GroundEffect is not null)
                .Select(entity => new EntityWithSettings
                {
                    Entity = entity.Entity,
                    GroundEffect = entity.GroundEffect
                });

            foreach (var item in matchingEntities)
            {
                var entity = item.Entity;
                var groundEffect = item.GroundEffect;

                if (!entity.TryGetComponent<Render>(out var entityRender)) continue;
                if (!entity.TryGetComponent<Positioned>(out var entityPositioned) || entityPositioned.IsFriendly) continue;

                var drawList = ImGui.GetBackgroundDrawList();
                var entityLocation = Core.States.InGameStateObject.CurrentWorldInstance.WorldToScreen(entityRender.WorldPosition);
                var color = ImGuiHelper.Color(groundEffect.Color);

                if (groundEffect.Filled) drawList.AddCircleFilled(entityLocation, groundEffect.Radius, color);
                else drawList.AddCircle(entityLocation, groundEffect.Radius, color, 0, groundEffect.LineWeight);
            }
        }
    }
}
