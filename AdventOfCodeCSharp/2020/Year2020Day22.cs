using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 22)]
    internal class Year2020Day22 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var (player1, player2) = ParseInput(input);
            while (player1.Count > 0
                && player2.Count > 0)
            {
                PlayRound(player1, player2);
            }

            var winDeck = player1.Count > 0 ? player1 : player2;
            int result = CalculateDeckScore(winDeck);
            return $"{result}";
        }

        private static int CalculateDeckScore(Queue<int> winDeck)
        {
            return winDeck
                .Zip(Enumerable.Range(1, winDeck.Count).Reverse())
                .Sum(v => v.First * v.Second);
        }

        public string Part2(IList<string> input)
        {
            var (player1, player2) = ParseInput(input);

            var player1Win = RecursiveCombat(player1, player2);
            var winDeck = player1Win ? player1 : player2;

            var result = CalculateDeckScore(winDeck);
            return $"{result}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        /// <returns>True if Player 1 wins, else false</returns>
        internal bool RecursiveCombat(Queue<int> player1, Queue<int> player2)
        {
            var seenCards = new HashSet<(string, string)>();
            bool? player1Wins = null;
            while (player1.Count > 0
                && player2.Count > 0)
            {
                var player1Sig = string.Join(",", player1);
                var player2Sig = string.Join(",", player2);
                if (seenCards.Contains((player1Sig, player2Sig)))
                {
                    return true;
                }

                seenCards.Add((player1Sig, player2Sig));

                var player1Card = player1.Dequeue();
                var player2Card = player2.Dequeue();
                if (player1.Count >= player1Card
                    && player2.Count >= player2Card)
                {
                    // Recurse
                    player1Wins = RecursiveCombat(new Queue<int>(player1.Take(player1Card)), new Queue<int>(player2.Take(player2Card)));
                }
                else
                {
                    player1Wins = player1Card > player2Card;
                }

                if (player1Wins ?? throw new InvalidOperationException("This should not be null"))
                {
                    player1.Enqueue(player1Card);
                    player1.Enqueue(player2Card);
                }
                else
                {
                    player2.Enqueue(player2Card);
                    player2.Enqueue(player1Card);
                }
            }

            return player1Wins ?? throw new InvalidOperationException("This should not be null");
        }

        internal void PlayRound(Queue<int> player1, Queue<int> player2)
        {
            var player1Card = player1.Dequeue();
            var player2Card = player2.Dequeue();

            if (player1Card > player2Card)
            {
                player1.Enqueue(player1Card);
                player1.Enqueue(player2Card);
            }
            else
            {
                player2.Enqueue(player2Card);
                player2.Enqueue(player1Card);
            }
        }

        private (Queue<int> player1, Queue<int> player2) ParseInput(IList<string> input)
        {
            var player1 = new Queue<int>();
            var player2 = new Queue<int>();
            var currentDeck = player1;
            foreach (var line in input
                .Select(l => l.Trim())
                .Where(l => l.Length > 0)
                .Skip(1))
            {
                if (line == "Player 2:")
                {
                    currentDeck = player2;
                    continue;
                }

                currentDeck.Enqueue(int.Parse(line));
            }

            return (player1, player2);
        }
    }
}
