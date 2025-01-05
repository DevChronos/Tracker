using System.Numerics;

namespace Tracker
{
    public class GroundEffectSettings(bool isEnabled, string path, Vector4 color, int radius, int lineWeight, bool filled)
    {
        public string Path = path;
        public Vector4 Color = color;
        public bool IsEnabled = isEnabled;
        public int LineWeight = lineWeight;
        public bool Filled = filled;
        public int Radius = radius;
    }
}
