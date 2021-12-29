using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayNine
{
    public class Player
    {
        public string Name { get; }
        public Hand Hand { get; private set; }
        public PlayerStrategyConfiguration StrategyConfiguration { get; private set; }

        public bool CanSkip => Hand.Cards.Count(x => !x.IsFaceUp) == 1;
        
        public Player(string name, PlayerStrategyConfiguration strategyConfiguration)
        {
            Name = name;
            StrategyConfiguration = strategyConfiguration;
        }

        public void SetHand(Hand hand)
        {
            Hand = hand; // todo this smells
        }

        public Tuple<int, int> GetInitialFlips()
        {
            if (StrategyConfiguration.InitialFlipIsPair)
                return new Tuple<int, int>(0, 4);
            return new Tuple<int, int>(0, 1);
        }

        public Decision TakeTurn(TurnReferee referee, bool isFinalRound)
        {
            List<Card> unmatchedCards = Hand.GetUnmatchedFaceUpCards();
            
            // If the previous discard will make a match, take it
            Card faceUpDiscard = referee.GetFaceUpFromDiscard();
            foreach (Card unmatchedCard in unmatchedCards)
            {
                if (faceUpDiscard.Value == unmatchedCard.Value)
                    return Decision.ReplaceWithFaceUpKnown(Hand.GetIndexOfOtherInPair(unmatchedCard));
            }

            // Take the top of the deck and see if it will make a match
            Card faceDownFromDeck = referee.GetFaceDownFromDeck();
            foreach (Card unmatchedCard in unmatchedCards)
            {
                if (faceDownFromDeck.Value == unmatchedCard.Value)
                    return Decision.ReplaceWithFaceDownUnknown(Hand.GetIndexOfOtherInPair(unmatchedCard));
            }

            Card cardToReplace;
            if (faceDownFromDeck.Value < 6)
            {
                cardToReplace = unmatchedCards.OrderByDescending(x => x.Value).FirstOrDefault();
                if (cardToReplace != null)
                    return Decision.ReplaceWithFaceDownUnknown(Hand.GetIndexOf(cardToReplace));

                cardToReplace = Hand.Cards.FirstOrDefault(x => !x.IsFaceUp);
                if (cardToReplace != null)
                    return Decision.ReplaceWithFaceDownUnknown(Hand.GetIndexOf(cardToReplace));
                
                throw new Exception("How did we get here? No face down cards at all?");
            }
            
            if (CanSkip && !isFinalRound)
                return Decision.Skip();
            
            cardToReplace = unmatchedCards.Where(x => x.Value > faceDownFromDeck.Value)
                .OrderByDescending(x => x.Value).FirstOrDefault();
            if (cardToReplace != null)
                return Decision.ReplaceWithFaceDownUnknown(Hand.GetIndexOf(cardToReplace));
            
            Card cardToFlip = Hand.Cards.FirstOrDefault(x => !x.IsFaceUp);
            if (cardToFlip != null)
                return Decision.Flip(Hand.GetIndexOf(cardToFlip));
            
            throw new Exception("How did we get here? No face down cards at all?");
        }

        public void ChangeStrategyConfiguration(PlayerStrategyConfiguration newConfiguration)
        {
            StrategyConfiguration = newConfiguration;
        }
    }
}