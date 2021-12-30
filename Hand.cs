using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayNine
{
    public class Hand
    {
        // 0 1 2 3
        // 4 5 6 7
        public List<Card> Cards { get; }

        public static Hand Parse(string input)
        {
            List<Card> cards = new List<Card>();
            foreach (string part in input.Split(' ', '\n'))
            {
                string cleanPart = part.Trim();
                if (string.IsNullOrEmpty(cleanPart))
                    continue;
                
                if (cleanPart == "?")
                {
                    Card card = new Card(1);
                    cards.Add(card);
                }
                else
                {
                    Card card = new Card(int.Parse(cleanPart));
                    card.Flip();
                    cards.Add(card);
                }
            }
            
            return new Hand(cards);
        }

        public Hand(List<Card> cards)
        {
            if (cards.Count != 8)
                throw new Exception("Hand must consist of 8 cards, not " + cards.Count);

            Cards = cards;
        }

        public bool IsFinished()
        {
            return Cards.All(x => x.IsFaceUp);
        }

        public List<Card> GetUnmatchedFaceUpCards()
        {
            var results = new List<Card>();

            void ProcessPair(int indexA, int indexB)
            {
                int? valueA = Cards[indexA].IsFaceUp ? Cards[indexA].Value : (int?)null;
                int? valueB = Cards[indexB].IsFaceUp ? Cards[indexB].Value : (int?)null;
                
                if (valueA != null && valueB == null)
                {
                    results.Add(Cards[indexA]);
                }
                else if (valueA == null && valueB != null)
                {
                    results.Add(Cards[indexB]);
                }
                else if (valueA != null && valueB != null && valueA != valueB)
                {
                    results.Add(Cards[indexA]);
                    results.Add(Cards[indexB]);
                }
            }
            
            ProcessPair(0, 4);
            ProcessPair(1, 5);
            ProcessPair(2, 6);
            ProcessPair(3, 7);

            return results;
        }

        public int GetIndexOf(Card card)
        {
            int result = Cards.IndexOf(card);
            if (result < 0)
                throw new Exception("This card isn't in your hand");
            return result;
        }
        
        public int GetIndexOfOtherInPair(Card card)
        {
            int result = GetIndexOf(card);
            if (result < 4)
                return result + 4;
            return result - 4;
        }

        public int GetScore()
        {
            if (Cards.Any(x => !x.IsFaceUp))
                throw new Exception("Can't score a hand until all the cards are face up");

            int points = 0;
            List<int> pairValues = new List<int>();

            void ProcessPair(int indexA, int indexB)
            {
                if (Cards[indexA].Value == Cards[indexB].Value)
                {
                    pairValues.Add(Cards[indexA].Value);

                    if (Cards[indexA].Value == -5)
                        points -= 10;
                }
                else
                {
                    points += Cards[indexA].Value;
                    points += Cards[indexB].Value;
                }
            }
            
            ProcessPair(0, 4);
            ProcessPair(1, 5);
            ProcessPair(2, 6);
            ProcessPair(3, 7);

            if (pairValues.Any())
            {
                foreach (IGrouping<int, int> group in pairValues.GroupBy(x => x))
                {
                    if (group.Count() == 2)
                        points -= 10;
                    if (group.Count() == 3)
                        points -= 15;
                    if (group.Count() == 4)
                        points -= 20;
                }
            }
            
            return points;
        }
    }
}