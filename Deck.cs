using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayNine
{
    public class Deck
    {
        private static Random _random = new Random();
        private readonly Stack<Card> _cards;
        private Stack<Card> _discardedCards;

        public Deck()
        {
            _cards = new Stack<Card>();
            _discardedCards = new Stack<Card>();
            var sortedCards = new List<int> {-5, -5, -5, -5};
            for (int n = 0; n <= 12; n++)
            for (int m = 0; m < 8; m++)
                sortedCards.Add(n);

            while (sortedCards.Any())
            {
                int index = _random.Next(sortedCards.Count);
                _cards.Push(new Card(sortedCards[index]));
                sortedCards.RemoveAt(index);
            }
        }

        public Hand DealOneHand()
        {
            List<Card> cards = new List<Card>();
            for (int n = 0; n < 8; n++)
                cards.Add(_cards.Pop());
            return new Hand(cards);
        }

        public Card DiscardTop()
        {
            if (!_cards.Any())
                ShuffleDiscardIntoDeck();
            Card card = _cards.Pop();
            card.Flip();
            _discardedCards.Push(card);
            return card;
        }

        private void ShuffleDiscardIntoDeck()
        {
            Card topOfDiscard = _discardedCards.Pop();
            List<Card> discardPile = _discardedCards.ToList();
            while (discardPile.Any())
            {
                int index = _random.Next(discardPile.Count);
                Card card = discardPile[index];
                card.UnFlip();
                _cards.Push(card);
                discardPile.RemoveAt(index);
            }
            _discardedCards = new Stack<Card>();
            _discardedCards.Push(topOfDiscard);
        }

        public Card LookAtTopOfDiscard()
        {
            return _discardedCards.Peek();
        }
        
        public Card RemoveTopOfDiscard()
        {
            return _discardedCards.Pop();
        }

        public void AddToDiscard(Card card)
        {
            _discardedCards.Push(card);
        }
    }
}