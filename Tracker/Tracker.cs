namespace Tracker
{
    using GameHelper.Plugin;
    using ImGuiNET;
    using Newtonsoft.Json;
    using System.IO;

    /// <summary>
    /// Tracker
    /// </summary>
    public sealed class Tracker : PCore<TrackerSettings>
    {
        private string SettingPathname => Path.Join(this.DllDirectory, "config", "settings.txt");
        private MonsterLine MonsterLine { get; set; }
        private GroundEffect GroundEffect { get; set; }
        private StatusEffect StatusEffect { get; set; }

        /// <summary>
        /// OnDisable
        /// </summary>
        public override void OnDisable()
        {
        }

        /// <summary>
        /// OnEnable
        /// </summary>
        /// <param name="isGameOpened"></param>
        public override void OnEnable(bool isGameOpened)
        {
            if (File.Exists(this.SettingPathname))
            {
                var content = File.ReadAllText(this.SettingPathname);
                this.Settings = JsonConvert.DeserializeObject<TrackerSettings>(content);
                MonsterLine = new MonsterLine(this.Settings);
                GroundEffect = new GroundEffect(this.Settings);
                StatusEffect = new StatusEffect(this.Settings);
            }
        }

        /// <summary>
        /// SaveSettings
        /// </summary>
        public override void SaveSettings()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(this.SettingPathname));
            var settingsData = JsonConvert.SerializeObject(this.Settings, Formatting.Indented);
            File.WriteAllText(this.SettingPathname, settingsData);
        }

        /// <summary>
        /// DrawSettings
        /// </summary>
        public override void DrawSettings()
        {
            ImGui.ColorEdit4("Unique monster line color", ref Settings.UniqueLineColor);
            ImGui.ColorEdit4("Rare monster line color", ref Settings.RareLineColor);
            ImGui.ColorEdit4("Magic monster line color", ref Settings.MagicLineColor);
            ImGui.ColorEdit4("Ground effect color", ref Settings.GroundEffectColor);
            ImGui.InputTextMultiline("Ground effects", ref Settings.GroundEffects, 8000, new System.Numerics.Vector2(500, 100));
            ImGui.InputTextMultiline("Monster status effects", ref Settings.MonsterStatusEffects, 8000, new System.Numerics.Vector2(500, 100));
        }

        /// <summary>
        /// DrawUI
        /// </summary>
        public override void DrawUI()
        {
            this.MonsterLine.Draw();
            this.GroundEffect.Draw();
            this.StatusEffect.Draw();
        }
    }
}
