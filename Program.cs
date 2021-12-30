using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayNine
{
    public class Program
    {
        public static void Main()
        {
            int numberOfPlayers = 4;
            var variableStrategy = new PlayerStrategyConfiguration {FlipPairToStart = 1.0};
            
            Player variablePlayer = new Player("TEST", variableStrategy);
            Queue<Player> players = new Queue<Player>();
            players.Enqueue(variablePlayer);
            for (int n = 0; n < numberOfPlayers - 1; n++)
            {
                players.Enqueue(new Player("NPC" + (n + 1), PlayerStrategyConfiguration.Random()));
            }
            
            Dictionary<string, int> winners = new Dictionary<string, int>();
            for (int x = 0; x < 10000; x++)
            {
                foreach (Player player in players)
                {
                    if (player != variablePlayer)
                        player.ChangeStrategyConfiguration(PlayerStrategyConfiguration.Random());
                }
                
                var game = new Game(players.ToList(), false);
                game.Play();
                string winner = game.GetWinner().Name;
                if (!winners.ContainsKey(winner))
                    winners[winner] = 0;
                winners[winner]++;
                
                players.Enqueue(players.Dequeue());
            }
            foreach (string winner in winners.Keys.OrderByDescending(x => winners[x]))
            {
                Console.WriteLine($"{winner} won {winners[winner]} times");
            }
        }
    }
}