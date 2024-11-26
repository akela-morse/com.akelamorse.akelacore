using System;

namespace Akela.Tools
{
    public static class ArrayExtensions
    {
        public static void Deconstruct<T>(this T[] items, out T t0)
        {
            t0 = items.Length > 0 ? items[0] : default;
        }

        public static void Deconstruct<T>(this T[] items, out T t0, out T t1)
        {
            t0 = items.Length > 0 ? items[0] : default;
            t1 = items.Length > 1 ? items[1] : default;
        }

        public static bool ContainsIndex(this Array array, int index, int dimension)
        {
            if (index < 0)
                return false;

            return index < array.GetLength(dimension);
        }
    }
}
