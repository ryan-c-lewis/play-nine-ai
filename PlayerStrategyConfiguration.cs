using System;

namespace PlayNine
{
    public class PlayerStrategyConfiguration
    {
        private static Random _random = new Random();
        
        public bool InitialFlipIsPair { get; set; }

        public static PlayerStrategyConfiguration Random()
        {
            return new PlayerStrategyConfiguration
            {
                InitialFlipIsPair = _random.Next(2) == 1
            };
        }
    }
}