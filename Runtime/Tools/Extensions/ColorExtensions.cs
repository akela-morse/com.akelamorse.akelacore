using UnityEngine;

namespace Akela.Tools
{
    public static class ColorExtensions
    {
        private const byte k_MaxByteForOverexposedColor = 191; //internal Unity const

        public static float ComputeHdrColorIntensity(this Color color)
        {
            var maxColorComponent = color.maxColorComponent;
            var scaleFactorToGetIntensity = k_MaxByteForOverexposedColor / maxColorComponent;

            return Mathf.Log(255f / scaleFactorToGetIntensity) / Mathf.Log(2f);
        }

        public static Color Hdr2Ldr(this Color hdrColor)
        {
            var intensity = hdrColor.ComputeHdrColorIntensity();
            var currentScaleFactor = Mathf.Pow(2, intensity);

            return hdrColor / currentScaleFactor;
        }

        public static Color Ldr2Hdr(this Color ldrColor, float intensity)
        {
            var newScaleFactor = Mathf.Pow(2, intensity);

            return ldrColor * newScaleFactor;
        }

        public static float Luminance(this Color color)
        {
            return .2126f * color.r + .7152f * color.g + .0722f * color.b;
        }
    }
}
