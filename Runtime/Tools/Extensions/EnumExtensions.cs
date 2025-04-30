using System;
using UnityEngine;

namespace Akela.Tools
{
    public static class EnumExtensions
    {
        public static T Next<T>(this T @enum) where T : Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            var index = Array.IndexOf(values, @enum);
            var newIndex = Mathf.Clamp(index + 1, 0, values.Length - 1);

            return values[newIndex];
        }

        public static T Previous<T>(this T @enum) where T : Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            var index = Array.IndexOf(values, @enum);
            var newIndex = Mathf.Clamp(index - 1, 0, values.Length - 1);

            return values[newIndex];
        }
    }
}