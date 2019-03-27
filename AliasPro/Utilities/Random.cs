using System;
using System.Collections.Generic;

namespace AliasPro.Utilities
{
    public static class Randomness
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static int RandomNumber(int max)
        {
            return RandomNumber(1, max);
        }

        public static int RandomNumber(int min, int max)
        {
            return rng.Next(min, max);
        }
    }
}
