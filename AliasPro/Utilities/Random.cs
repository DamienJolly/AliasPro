using System;

namespace AliasPro.Utilities
{
    public static class Randomness
    {
        private static Random rng = new Random();
        
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
