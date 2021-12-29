using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PlayNine
{
    [TestFixture]
    public class Tests
    {
        private Hand MakeHand(params int[] values)
        {
            List<Card> cards = values.Select(x => new Card(x)).ToList();
            cards.ForEach(x => x.Flip());
            return new Hand(cards);
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
            var variableStrategy = new PlayerStrategyConfiguration {InitialFlipIsPair = false};
            Player variablePlayer = new Player("TEST", variableStrategy);
            var game = new Game(new List<Player> {variablePlayer}, false);
            Assert.IsTrue(variablePlayer.Hand.Cards[0].IsFaceUp);
            Assert.IsTrue(variablePlayer.Hand.Cards[1].IsFaceUp);
        }

        [Test]
        public void Strategy_InitialFlipIsPair_true()
        {
            var variableStrategy = new PlayerStrategyConfiguration {InitialFlipIsPair = true};
            Player variablePlayer = new Player("TEST", variableStrategy);
            var game = new Game(new List<Player> {variablePlayer}, false);
            Assert.IsTrue(variablePlayer.Hand.Cards[0].IsFaceUp);
            Assert.IsTrue(variablePlayer.Hand.Cards[4].IsFaceUp);
        }
    }
}