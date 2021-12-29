using System;

namespace PlayNine
{
    public class Card
    {
        public int Value { get; }
        public bool IsFaceUp { get; private set; }
        
        public Card(int value)
        {
            if (value >= 0 && value <= 12 || value == -5)
            {
                Value = value;
            }
            else
            {
                throw new Exception("Card cannot have a value of " + value);
            }
        }

        public void Flip()
        {
            IsFaceUp = true;
        }
        
        public void UnFlip()
        {
            IsFaceUp = false;
        }

        public override string ToString()
        {
            if (!IsFaceUp)
                return " ?";
            if (Value < 0 || Value > 9)
                return Value.ToString();
            return " " + Value;
        }
    }
}