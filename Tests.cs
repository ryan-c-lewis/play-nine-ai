using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PlayNine
{
    public class GameForTests : Game
    {
        public GameForTests(List<Player> players) : base(players, false)
        {
        }

        public void SetDiscardValue(int value)
        {
            _deck.RemoveTopOfDiscard();
            var cardToInsert = new Card(value);
            cardToInsert.Flip();
            _deck.AddToDiscard(cardToInsert);
        }
        
        public void SetNextDeckValue(int value)
        {
            _deck.DiscardTop();
            _deck.RemoveTopOfDiscard();
            var cardToInsert = new Card(value);
            _deck.AddToTop(cardToInsert);
        }
    }
    
    [TestFixture]
    public class Tests
    {
        private Hand MakeHand(params int[] values)
        {
            List<Card> cards = values.Select(x => new Card(x)).ToList();
            cards.ForEach(x => x.Flip());
            return new Hand(cards);
        }

        private Decision DecideForScenario(PlayerStrategyConfiguration strategyConfiguration, 
            Hand hand, int discardValue, int nextValue)
        {
            Player player = new Player("TEST", strategyConfiguration);
            var game = new GameForTests(new List<Player> {player});
            player.SetHand(hand);
            game.SetDiscardValue(discardValue);
            game.SetNextDeckValue(nextValue);
            return game.MakeDecision(player, false);
        }
        
        [Test]
        public void ScoreHands()
        {
            Assert.AreEqual(36, MakeHand(
                1, 2, 3, 4, 
                5, 6, 7, 8).GetScore());
            Assert.AreEqual(0, MakeHand(
                1, 2, 3, 4, 
                1, 2, 3, 4).GetScore());
            Assert.AreEqual(7, MakeHand(
                1, 2, 3, 4, 
                1, 5, 3, 4).GetScore());
            Assert.AreEqual(12, MakeHand(
                8, 2, 3, 3, 
                7, 5, 3, 3).GetScore());
            Assert.AreEqual(-10, MakeHand(
                1, 2, 3, 1, 
                1, 2, 3, 1).GetScore());
            Assert.AreEqual(-15, MakeHand(
                1, 1, 3, 1, 
                1, 1, 3, 1).GetScore());
            Assert.AreEqual(-20, MakeHand(
                1, 1, 1, 1, 
                1, 1, 1, 1).GetScore());
            Assert.AreEqual(-20, MakeHand(
                1, 3, 3, 1, 
                1, 3, 3, 1).GetScore());
            Assert.AreEqual(-10, MakeHand(
                -5, 2, 3, 4, 
                -5, 2, 3, 4).GetScore());
            Assert.AreEqual(-30, MakeHand(
                -5, -5, 3, 4, 
                -5, -5, 3, 4).GetScore());
        }
        
        [Test]
        public void Strategy_InitialFlipIsPair_false()
        {
            var variableStrategy = new PlayerStrategyConfiguration {FlipPairToStart = 0.0};
            Player variablePlayer = new Player("TEST", variableStrategy);
            var game = new Game(new List<Player> {variablePlayer}, false);
            Assert.IsTrue(variablePlayer.Hand.Cards[0].IsFaceUp);
            Assert.IsTrue(variablePlayer.Hand.Cards[1].IsFaceUp);
        }

        [Test]
        public void Strategy_InitialFlipIsPair_true()
        {
            var variableStrategy = new PlayerStrategyConfiguration {FlipPairToStart = 1.0};
            Player variablePlayer = new Player("TEST", variableStrategy);
            var game = new Game(new List<Player> {variablePlayer}, false);
            Assert.IsTrue(variablePlayer.Hand.Cards[0].IsFaceUp);
            Assert.IsTrue(variablePlayer.Hand.Cards[4].IsFaceUp);
        }
        
        [Test]
        public void Strategy_UpperThresholdForConsideringACardLow()
        {
            var strategy = new PlayerStrategyConfiguration {UpperThresholdForConsideringACardLow = 3};
            Hand hand = Hand.Parse(
                "1   1   ?   ?" + Environment.NewLine +
                "?   ?   ?   ?");
            Decision result = DecideForScenario(strategy, hand, 2, 0);
            Assert.IsTrue(result.IsReplaceWithFaceUpKnown);
            result = DecideForScenario(strategy, hand, 3, 0);
            Assert.IsTrue(result.IsReplaceWithFaceUpKnown);
            result = DecideForScenario(strategy, hand, 4, 0);
            Assert.IsFalse(result.IsReplaceWithFaceUpKnown);
        }
    }
}