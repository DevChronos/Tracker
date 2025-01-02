namespace Tracker
{
    using GameHelper.Plugin;
    using System.Numerics;

    /// <summary>
    ///     Preload Tracker settings
    /// </summary>
    public sealed class TrackerSettings : IPSettings
    {
        public Vector4 UniqueLineColor;
        public Vector4 RareLineColor;
        public Vector4 MagicLineColor;
        public Vector4 GroundEffectColor;
        public string MonsterStatusEffects = string.Empty;
    }
}