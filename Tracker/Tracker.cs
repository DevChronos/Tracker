namespace Tracker
{
    using GameHelper.Plugin;
    using ImGuiNET;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Numerics;

    public sealed class Tracker : PCore<TrackerSettings>
    {
        private string SettingPathname => Path.Join(this.DllDirectory, "config", "settings.txt");
        private MonsterLineLogic MonsterLine { get; set; }
        private GroundEffectLogic GroundEffect { get; set; }
        private StatusEffectLogic StatusEffect { get; set; }

        public override void OnDisable()
        {
        }

        public override void OnEnable(bool isGameOpened)
        {
            if (File.Exists(this.SettingPathname))
            {
                var content = File.ReadAllText(this.SettingPathname);
                var serializerSettings = new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace };
                this.Settings = JsonConvert.DeserializeObject<TrackerSettings>(content, serializerSettings);
            }

            MonsterLine = new MonsterLineLogic(this.Settings);
            GroundEffect = new GroundEffectLogic(this.Settings);
            StatusEffect = new StatusEffectLogic(this.Settings);
        }

        public override void SaveSettings()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(this.SettingPathname));
            var settingsData = JsonConvert.SerializeObject(this.Settings, Formatting.Indented);
            File.WriteAllText(this.SettingPathname, settingsData);
        }

        public override void DrawSettings()
        {
            DrawMonsterLineSettigns();
            DrawGroundEffectSettings();
            DrawStatusEffectSettings();
        }

        public override void DrawUI()
        {
            this.MonsterLine.Draw();
            this.GroundEffect.Draw();
            this.StatusEffect.Draw();
        }

        private void DrawMonsterLineSettigns()
        {
            ImGui.Checkbox("##UniqueLine", ref Settings.UniqueLine);
            ImGui.SameLine();
            ColorSwatch("Unique monster line color", ref Settings.UniqueLineColor);

            ImGui.Checkbox("##RareLine", ref Settings.RareLine);
            ImGui.SameLine();
            ColorSwatch("Rare monster line color", ref Settings.RareLineColor);

            ImGui.Checkbox("##MagicLine", ref Settings.MagicLine);
            ImGui.SameLine();
            ColorSwatch("Magic monster line color", ref Settings.MagicLineColor);
        }

        private void DrawGroundEffectSettings()
        {
            if (ImGui.CollapsingHeader("Ground Effects"))
            {
                ImGui.Indent();

                for (int i = 0; i < Settings.GroundEffects.Count; i++)
                {
                    var groundEffect = Settings.GroundEffects[i];

                    ImGui.Checkbox($"##GroundEffectEnabled{i}", ref groundEffect.IsEnabled);

                    ImGui.SameLine();
                    ColorSwatch($"##GroundEffectColor{i}", ref groundEffect.Color);

                    ImGui.SameLine();
                    ImGui.Text($"Fill");
                    ImGui.SameLine();
                    ImGui.Checkbox($"##Fill{i}", ref groundEffect.Filled);

                    ImGui.SameLine();
                    ImGui.Text($"Weight");
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(50);
                    ImGui.SliderInt($"##Weight{i}", ref groundEffect.LineWeight, 1, 5);

                    ImGui.SameLine();
                    ImGui.Text("Radius");
                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(50);
                    ImGui.SliderInt($"##Radius{i}", ref groundEffect.Radius, 50, 300);

                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(GetInputWidth());
                    ImGui.InputText($"##GroundEffectPath{i}", ref groundEffect.Path, 256);

                    ImGui.SameLine();
                    if (ImGui.Button($"Delete##GroundEffectDelete{i}"))
                    {
                        Settings.GroundEffects.RemoveAt(i);
                        break;
                    }
                }

                if (ImGui.Button("Add Ground Effect"))
                    Settings.GroundEffects.Add(new GroundEffectSettings(true, "", new Vector4(1.0f, 0.0f, 0.0f, 0.6f), 100, 1, false));

                ImGui.Unindent();
            }
        }

        private void DrawStatusEffectSettings()
        {
            if (ImGui.CollapsingHeader("Monster Status Effects"))
            {
                ImGui.Indent();

                ImGui.Text($"StatusBar Background");
                ImGui.SameLine();
                ColorSwatch($"##StatusBarBG", ref Settings.StatusBarBackgroundColor);
                Tooltip("Status Effect text color.");

                ImGui.SameLine();
                ImGui.Text($"StatusBar Min Width");
                ImGui.SameLine();
                ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
                ImGui.SliderInt($"##StatusBar Min Width", ref Settings.StatusBarMinWidth, 50, 300);

                for (int i = 0; i < Settings.StatusEffects.Count; i++)
                {
                    var statusEffect = Settings.StatusEffects[i];

                    ImGui.Checkbox($"##StatusEffectEnabled{i}", ref statusEffect.IsEnabled);
                    Tooltip("Enable status Effect.");

                    ImGui.SameLine();
                    ColorSwatch($"##StatusEffectTextColor{i}", ref statusEffect.TextColor);
                    Tooltip("Status Effect text color.");

                    ImGui.SameLine();
                    ColorSwatch($"##StatusEffectBarColor{i}", ref statusEffect.BarColor);
                    Tooltip("Status Effect bar color.");

                    ImGui.SameLine();
                    ImGui.Text($"Display Name");

                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(120);
                    ImGui.InputText($"##DisplayName{i}", ref statusEffect.DisplayName, 256);

                    ImGui.SameLine();
                    ImGui.Text($"Name");

                    ImGui.SameLine();
                    ImGui.SetNextItemWidth(GetInputWidth());
                    ImGui.InputText($"##Name{i}", ref statusEffect.Name, 256);

                    ImGui.SameLine();
                    if (ImGui.Button($"Delete##StatusEffectDelete{i}"))
                    {
                        Settings.StatusEffects.RemoveAt(i);
                        break;
                    }
                }

                if (ImGui.Button("Add Status Effect"))
                    Settings.StatusEffects.Add(new StatusEffectSettings(true, "xxx", "XXX", new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(0.4549f, 0.0314f, 0.0314f, 1.0f)));

                ImGui.Unindent();
            }
        }

        private static void ColorSwatch(string label, ref System.Numerics.Vector4 color)
        {

            if (ImGui.ColorButton(label, color))
                ImGui.OpenPopup(label);

            if (ImGui.BeginPopup(label))
            {
                ImGui.ColorPicker4(label, ref color);
                ImGui.EndPopup();
            }

            if (!label.StartsWith("##"))
            {
                ImGui.SameLine();
                ImGui.Text(label);
            }
        }

        private static void Tooltip(string label)
        {
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Text(label);
                ImGui.EndTooltip();
            }
        }

        private static float GetInputWidth()
        {
            var availableWidth = ImGui.GetContentRegionAvail().X;
            var minWidth = 50.0f;
            var buttonSize = ImGui.CalcTextSize($"Delete");
            buttonSize.X += ImGui.GetStyle().FramePadding.X * 2;
            return Math.Max(availableWidth - buttonSize.X - 10, minWidth);
        }
    }
}
