namespace Tracker
{
    using GameHelper.Plugin;
    using System.Collections.Generic;
    using System.Numerics;

    public sealed class TrackerSettings : IPSettings
    {
        public bool UniqueLine = true;
        public bool RareLine = true;
        public bool MagicLine = false;

        public Vector4 UniqueLineColor = new(1.0f, 0.580f, 0.0f, 0.564f);
        public Vector4 RareLineColor = new(1.0f, 0.988f, 0.0f, 0.490f);
        public Vector4 MagicLineColor = new(0.0f, 0.117f, 1.0f, 0.294f);

        public List<GroundEffectSettings> GroundEffects;
        public List<StatusEffectSettings> StatusEffects;

        public Vector4 StatusBarBackgroundColor = new(0, 0, 0, 0.750f);
        public int StatusBarMinWidth = 120;

        public TrackerSettings()
        {
            GroundEffects = [
                new GroundEffectSettings(true, "Metadata/Effects/Spells/ground_effects/VisibleServerGroundEffect", new Vector4(1.0f, 0.0f, 0.0f, 0.6f), 100, 1, false),
                new GroundEffectSettings(true, "Metadata/Monsters/MonsterMods/GroundOnDeath/BurningGroundDaemon", new Vector4(1.0f, 0.0f, 0.0f, 0.6f), 100, 1, false),
                new GroundEffectSettings(true, "Metadata/Monsters/MonsterMods/GroundOnDeath/ColdSnapGroundDaemon", new Vector4(1.0f, 0.0f, 0.0f, 0.6f), 100, 1, false),
                new GroundEffectSettings(true, "Metadata/Monsters/MonsterMods/OnDeathLightningExplosion", new Vector4(1.0f, 0.0f, 0.0f, 0.6f), 100, 1, false),
                new GroundEffectSettings(true, "Metadata/Monsters/MonsterMods/OnDeathFireExplosion", new Vector4(1.0f, 0.0f, 0.0f, 0.6f), 100, 1, false)
            ];

            StatusEffects = [
                new StatusEffectSettings(true, "shocked", "Shocked", new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(0.6549f, 0.6039f, 0.0431f, 1.0f) ),
                new StatusEffectSettings(true, "proximal_intangibility", "Intangible", new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(0.4549f, 0.0314f, 0.0314f, 1.0f) ),
                new StatusEffectSettings(true, "frozen", "Frozen", new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(0.0f,    0.6314f, 0.8118f, 1.0f) )
            ];
        }
    }
}
