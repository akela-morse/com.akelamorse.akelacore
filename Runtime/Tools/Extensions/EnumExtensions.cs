using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Akela.Tools
{
    public static class EnumExtensions
    {
        private static readonly ConditionalWeakTable<Type, Array> _cache = new();

        public static T Next<T>(this T @enum) where T : Enum
        {
            if (!_cache.TryGetValue(typeof(T), out var values))
            {
                values = Enum.GetValues(typeof(T));
                _cache.Add(typeof(T), values);
            }

            var index = Array.IndexOf(values, @enum);
            var newIndex = Mathf.Clamp(index + 1, 0, values.Length - 1);

            return (T)values.GetValue(newIndex);
        }

        public static T NextWrap<T>(this T @enum) where T : Enum
        {
            if (!_cache.TryGetValue(typeof(T), out var values))
            {
                values = Enum.GetValues(typeof(T));
                _cache.Add(typeof(T), values);
            }

            var index = Array.IndexOf(values, @enum);
            var newIndex = (int)Mathf.Repeat(index + 1, values.Length);

            return (T)values.GetValue(newIndex);
        }

        public static T Previous<T>(this T @enum) where T : Enum
        {
            if (!_cache.TryGetValue(typeof(T), out var values))
            {
                values = Enum.GetValues(typeof(T));
                _cache.Add(typeof(T), values);
            }

            var index = Array.IndexOf(values, @enum);
            var newIndex = Mathf.Clamp(index - 1, 0, values.Length - 1);

            return (T)values.GetValue(newIndex);
        }

        public static T PreviousWrap<T>(this T @enum) where T : Enum
        {
            if (!_cache.TryGetValue(typeof(T), out var values))
            {
                values = Enum.GetValues(typeof(T));
                _cache.Add(typeof(T), values);
            }

            var index = Array.IndexOf(values, @enum);
            var newIndex = (int)Mathf.Repeat(index - 1, values.Length);

            return (T)values.GetValue(newIndex);
        }
    }
}