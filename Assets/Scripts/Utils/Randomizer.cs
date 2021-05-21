using System;

namespace Utils
{
    public class Randomizer
    {
        private static Random _random = new Random();

        public static int RandInt(int max)
        {
            return _random.Next(max);
        }
        
        public static int RandInt(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}