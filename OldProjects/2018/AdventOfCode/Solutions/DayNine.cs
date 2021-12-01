using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class DayNine : IDaySolution
    {
        private readonly Regex inputParser = new Regex(@"(?<numPlayers>\d+) players; last marble is worth (?<lastMarble>\d+) points");

        public string PartOne(string input)
        {
            (int numPlayers, int lastMarble) = ParseInput(input);
            long[] players = PlayMarbles(numPlayers, lastMarble);

            return players.Max().ToString();
        }

        private long[] PlayMarbles(long numPlayers, long lastMarble)
        {
            var circle = new LinkedList<long>();
            var players = new long[numPlayers];
            var playerIdx = 0L;
            var current = circle.AddFirst(0);

            for (long marbleValue = 1; marbleValue <= lastMarble; marbleValue++)
            {
                if (marbleValue % 23 == 0)
                {
                    players[playerIdx] += marbleValue;
                    var removed = MoveCounterClockwise(current, 7);
                    current = MoveClockwise(removed, 1);
                    circle.Remove(removed);
                    players[playerIdx] += removed.Value;
                }
                else
                {
                    current = MoveClockwise(current, 1);
                    current = circle.AddAfter(current, marbleValue);
                }

                playerIdx = (playerIdx + 1) % numPlayers;
            }

            return players;
        }

        public string PartTwo(string input)
        {
            (int numPlayers, int lastMarble) = ParseInput(input);
            long[] players = PlayMarbles(numPlayers, lastMarble * 100);

            return players.Max().ToString();
        }

        public LinkedListNode<T> MoveClockwise<T>(LinkedListNode<T> node, int numSteps)
        {
            var current = node;
            for (int i = 0; i < numSteps; i++)
            {
                current = current.Next ?? current.List.First;
            }
            return current;
        }

        public LinkedListNode<T> MoveCounterClockwise<T>(LinkedListNode<T> node, int numSteps)
        {
            var current = node;
            for (int i = 0; i < numSteps; i++)
            {
                current = current.Previous ?? current.List.Last;
            }
            return current;
        }

        private (int numPlayers, int lastMarble) ParseInput(string input)
        {
            var match = inputParser.Match(input.Trim());
            return (int.Parse(match.Groups["numPlayers"].Value), int.Parse(match.Groups["lastMarble"].Value));
        }
    }
}
