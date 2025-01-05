using GameHelper;
using GameHelper.RemoteEnums;
using GameHelper.RemoteEnums.Entity;
using GameHelper.RemoteObjects.Components;
using GameHelper.RemoteObjects.States.InGameStateObjects;
using GameHelper.Utils;
using GameOffsets.Objects.States.InGameState;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Tracker
{
    public class StatusEffectLogic(TrackerSettings settings)
    {
        private TrackerSettings Settings { get; } = settings;

        public void Draw()
        {
            foreach (var entity in GetMonsters())
            {
                if (!entity.TryGetComponent<Render>(out var entityRender)) continue;
                if (!entity.TryGetComponent<Buffs>(out var entityBuffs)) continue;

                var effects = Settings.StatusEffects.Where(mse => entityBuffs.StatusEffects.ContainsKey(mse.Name) && mse.IsEnabled);
                if (!effects.Any()) continue;

                var drawList = ImGui.GetBackgroundDrawList();
                var entitylocation = Core.States.InGameStateObject.CurrentWorldInstance.WorldToScreen(entityRender.WorldPosition);

                var shadowOffset = new Vector2(1, 1);
                var shadowColor = ImGuiHelper.Color(new Vector4(0, 0, 0, 1.0f));

                for (var i = 0; i < effects.Count(); i++)
                {
                    var barWidthMultiplier = 1.0f;

                    var effect = effects.ElementAt(i);
                    var statusEffect = entityBuffs.StatusEffects[effect.Name];
                    var displayText = effect.DisplayName;

                    if (statusEffect.TimeLeft > 0 && statusEffect.TimeLeft < 99)
                    {
                        displayText += $"  {statusEffect.TimeLeft:F1}s";

                        if (statusEffect.TotalTime < 99)
                            barWidthMultiplier = statusEffect.TimeLeft / statusEffect.TotalTime;
                    }

                    var textSize = ImGui.CalcTextSize(displayText);
                    var padding = new Vector2(5, 2);
                    var bgPos = new Vector2(entitylocation.X - padding.X, entitylocation.Y - padding.Y);
                    var bgSize = new Vector2(Math.Max(textSize.X + padding.X * 2, Settings.StatusBarMinWidth), textSize.Y + padding.Y * 2);

                    drawList.AddRectFilled(bgPos, bgPos + bgSize, ImGuiHelper.Color(Settings.StatusBarBackgroundColor));

                    var barInset = 1.0f;
                    var barPos = new Vector2(bgPos.X + barInset, bgPos.Y + barInset);
                    var barSize = new Vector2(bgSize.X - barInset * 2, bgSize.Y - barInset * 2);
                    var barWidth = barSize.X * barWidthMultiplier;
                    drawList.AddRectFilled(barPos, new Vector2(barPos.X + barWidth, barPos.Y + barSize.Y), ImGuiHelper.Color(effect.BarColor));

                    drawList.AddText(entitylocation + shadowOffset, shadowColor, displayText);
                    drawList.AddText(entitylocation, ImGuiHelper.Color(effect.TextColor), displayText);

                    entitylocation.Y += textSize.Y + padding.Y * 2 - 1;
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
    }
}
