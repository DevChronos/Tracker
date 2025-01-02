namespace Tracker
{
    using GameHelper.Plugin;
    using System.Numerics;

    /// <summary>
    ///     Preload Tracker settings
    /// </summary>
    public sealed class TrackerSettings : IPSettings
    {
        public Vector4 UniqueLineColor = new Vector4(1.0f, 0.5f, 0.0f, 0.5f);
        public Vector4 RareLineColor = new Vector4(1.0f, 1.0f, 0.0f, 0.5f);
        public Vector4 MagicLineColor = new Vector4(0.0f, 0.1f, 1.0f, 0.3f);
        public Vector4 GroundEffectColor = new Vector4(1.0f, 0.0f, 0.0f, 0.5f);
        public string GroundEffects = string.Empty;
        public string MonsterStatusEffects = "shocked|Shocked|0xFF00FFFF\nproximal_intangibility|Intangible|0xFFFF00FF";

        public TrackerSettings()
        {
            GroundEffects = "Metadata/Effects/Spells/ground_effects/VisibleServerGroundEffect";
            GroundEffects += "\nMetadata/Monsters/MonsterMods/GroundOnDeath/BurningGroundDaemon";
            GroundEffects += "\nMetadata/Monsters/MonsterMods/GroundOnDeath/ColdSnapGroundDaemon";
            GroundEffects += "\nMetadata/Monsters/MonsterMods/OnDeathLightningExplosion";
            GroundEffects += "\nMetadata/Monsters/MonsterMods/OnDeathFireExplosion";
        }
    }
}