using System.Numerics;

namespace Tracker
{
    public class StatusEffectSettings(bool isEnabled, string name, string displayName, Vector4 textcolor, Vector4 barColor)
    {
        public string Name = name;
        public string DisplayName = displayName;
        public Vector4 TextColor = textcolor;
        public Vector4 BarColor = barColor;
        public bool IsEnabled = isEnabled;
    }
}
