using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayNine
{
    public class Game
    {
        private readonly bool _print;
        private readonly Deck _deck;
        private readonly List<Player> _players;
        
        public Game(List<Player> players, bool print)
        {
            _print = print;
            _deck = new Deck();
            _players = players;
            foreach (Player player in players)
            {
                Hand hand = _deck.DealOneHand();
                player.SetHand(hand);
                
                Tuple<int, int> cardsToFlip = player.GetInitialFlips();
                player.Hand.Cards[cardsToFlip.Item1].Flip();
                player.Hand.Cards[cardsToFlip.Item2].Flip();
            }
            _deck.DiscardTop();
        }

        public void Play(int turnsToPlay = -1)
        {
            int totalTurns = 0;
            int playerIndex = 0;
            int numberOfPlayersFinished = 0;
            while (true)
            {
                TurnReferee referee = new TurnReferee(_deck, _players[playerIndex]);
                Decision decision = _players[playerIndex].TakeTurn(referee, numberOfPlayersFinished > 0);
                referee.HandleDecision(decision);
                
                if (numberOfPlayersFinished > 0)
                    _players[playerIndex].Hand.Cards.ForEach(x => x.Flip());
                
                PrintPlayer(_players[playerIndex]);

                bool thisPlayerIsFinished = _players[playerIndex].Hand.IsFinished();
                if (thisPlayerIsFinished)
                {
                    numberOfPlayersFinished++;
                    if (numberOfPlayersFinished == _players.Count)
                    {
                        if (_players.Any(x => !x.Hand.IsFinished()))
                            throw new Exception("Ending the game but not all players are finished...?");
                        return;
                    }
                }

                playerIndex++;
                playerIndex %= _players.Count;

                if (playerIndex == 0)
                    totalTurns++;
                if (turnsToPlay != -1 && totalTurns >= turnsToPlay)
                    return;
            }
        }

        public Player GetWinner()
        {
            return _players.OrderByDescending(x => x.Hand.GetScore()).First();
        }

        private void PrintPlayer(Player player, bool force = false)
        {
            if (!_print && !force)
                return;
            
            Console.WriteLine(player.Name);
            List<Card> cards = player.Hand.Cards;
            foreach (Card card in cards.Take(4))
                Console.Write(card + "   ");
            Console.Write(Environment.NewLine);
            foreach (Card card in cards.Skip(4).Take(4))
                Console.Write(card + "   ");
            Console.WriteLine(Environment.NewLine);
        }
    }
}