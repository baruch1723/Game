using UnityEngine;

namespace Runtime.Extensions
{
    public static class MathExtensions
    {
        public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
        }

        public static Color RemapColor(float value)
        {
            return value * (Color.red - Color.blue) + Color.blue;
        }
    }
}