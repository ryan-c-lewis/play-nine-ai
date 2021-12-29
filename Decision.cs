namespace PlayNine
{
    public class Decision
    {
        private char _type;
        public int MyCardIndex { get; private set; }
        
        public bool IsReplaceWithFaceUpKnown => _type == 'K';
        public bool IsReplaceWithFaceDownUnknown => _type == 'U';
        public bool IsFlip => _type == 'F';
        public bool IsSkip => _type == 'S';
        
        public static Decision ReplaceWithFaceUpKnown(int indexToReplace)
        {
            return new Decision {_type = 'K', MyCardIndex = indexToReplace};
        }
        
        public static Decision ReplaceWithFaceDownUnknown(int indexToReplace)
        {
            return new Decision {_type = 'U', MyCardIndex = indexToReplace};
        }
        
        public static Decision Flip(int indexToFlip)
        {
            return new Decision {_type = 'F', MyCardIndex = indexToFlip};
        }
        
        public static Decision Skip()
        {
            return new Decision {_type = 'S', MyCardIndex = -1};
        }
    }
}