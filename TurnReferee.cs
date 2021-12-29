using System;

namespace PlayNine
{
    public class TurnReferee
    {
        private Deck _deck;
        private Player _player;
        private bool _alreadyDiscarded;
        
        public TurnReferee(Deck deck, Player player)
        {
            _deck = deck;
            _player = player;
        }
        
        public Card GetFaceUpFromDiscard()
        {
            if (_alreadyDiscarded)
                throw new Exception("Cannot use the previous discard once you choose to take the face down from the deck");

            return _deck.LookAtTopOfDiscard();
        }

        public Card GetFaceDownFromDeck()
        {
            if (_alreadyDiscarded)
                throw new Exception("Cannot discard twice in one turn");

            _alreadyDiscarded = true;
            return _deck.DiscardTop();
        }

        public void HandleDecision(Decision decision)
        {
            if (decision.IsSkip)
            {
                if (!_player.CanSkip)
                    throw new Exception("Cannot skip");
            }
            else if (decision.IsFlip)
            {
                _player.Hand.Cards[decision.MyCardIndex].Flip();
            }
            else if (decision.IsReplaceWithFaceUpKnown)
            {
                if (_alreadyDiscarded)
                    throw new Exception("Can't use the previous discarded once you've already looked at the next one");
                
                Card cardToAdd = _deck.RemoveTopOfDiscard();
                Card cardToRemove = _player.Hand.Cards[decision.MyCardIndex];
                _player.Hand.Cards[decision.MyCardIndex] = cardToAdd;
                _deck.AddToDiscard(cardToRemove);
            }
            else if (decision.IsReplaceWithFaceDownUnknown)
            {
                if (!_alreadyDiscarded)
                    GetFaceDownFromDeck();
                
                Card cardToAdd = _deck.RemoveTopOfDiscard();
                Card cardToRemove = _player.Hand.Cards[decision.MyCardIndex];
                _player.Hand.Cards[decision.MyCardIndex] = cardToAdd;
                _deck.AddToDiscard(cardToRemove);
            }
            
            if (!_alreadyDiscarded)
                GetFaceDownFromDeck();
        }
    }
}