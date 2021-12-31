using System;

namespace PlayNine
{
    public class PlayerStrategyConfiguration
    {
        private static readonly Random _random = new Random();
        
        public double FlipPairToStart { get; set; } // 0.0 - 1.0
        public bool DoFlipPairToStart => _random.NextDouble() < FlipPairToStart;
        public int UpperThresholdForConsideringACardLow { get; set; } // 0 - 12

        public static PlayerStrategyConfiguration Random()
        {
            return new PlayerStrategyConfiguration
            {
                FlipPairToStart = _random.NextDouble()
            };
        }
    }
}