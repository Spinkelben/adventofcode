using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class DayThriteen : IDaySolution
    {
        private readonly bool printStages;

        public DayThriteen()
            : this(false)
        {   
        }

        public DayThriteen(bool printStages)
        {
            this.printStages = printStages;
        }

        public string PartOne(string input)
        {
            var (width, height, track, map) = ParseInput(input);
            
            while (true)
            {
                if (printStages)
                {
                    PrintStatus(track, map);
                }
                var crashes = DoTick(track, map);
                if (crashes.Count > 0)
                {
                    return $"{crashes[0].left},{crashes[0].top}";
                }
            }
        }

        public string PartTwo(string input)
        {
            var (width, height, track, map) = ParseInput(input);

            while (true)
            {
                if (printStages)
                {
                    PrintStatus(track, map);
                }
                var crashes = DoTick(track, map);
                if (map.Count == 1)
                {
                    return map.Values.First().Left + "," + map.Values.First().Top;
                }
            }
        }

        public (int width, int height, List<string> track, Dictionary<(int Top, int Left), MineCart> cartMap) ParseInput(string input)
        {
            var lines = input.Split('\n').Select(l => l.Trim('\r')).Where(l => string.IsNullOrWhiteSpace(l) == false).ToList();
            var (width, height) = (lines[0].Length, lines.Count);
            var map = new Dictionary<(int Top, int Left), MineCart>();
            var directions = "^>v<";
            var track = "|-|-";

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ("<>^v".Contains(lines[i][j]))
                    {
                        map[(i, j)] = new MineCart(i, j, lines[i][j]);
                        ReplaceSymbol(lines, track[directions.IndexOf(lines[i][j])], i, j);
                    }
                }
            }

            return (width, height, lines, map);
        }

        public void PrintStatus(List<string> track, Dictionary<(int Top, int Left), MineCart> cartMap)
        {
            var trackCopy = track.ToList();
            foreach (var kvp in cartMap)
            {
                ReplaceSymbol(trackCopy, kvp.Value.Direction, kvp.Value.Top, kvp.Value.Left);
            }
            Console.WriteLine(string.Join('\n', trackCopy));
        }

        private static void ReplaceSymbol(List<string> lines, char symbol, int top, int left)
        {
            lines[top] = lines[top].Substring(0, left) + symbol + lines[top].Substring(left + 1, lines[top].Length - (left + 1));
        }

        public List<(int top, int left)> DoTick(List<string> track, Dictionary<(int Top, int Left), MineCart> map)
        {
            var cartList = map.Keys.OrderBy(key => key.Top).ThenBy(key => key.Left).ToList();
            var crashes = new List<(int top, int left)>();
            foreach (var cartKey in cartList)
            {
                if (map.ContainsKey(cartKey))
                {
                    var cart = map[cartKey];
                    if (cart.Move(track, map))
                    {
                        crashes.Add((cart.Top, cart.Left));
                    }
                }
            }
            return crashes;
        }

        public class MineCart
        {
            private readonly string[] interSectionDirections = new string[] { "left", "straight", "right" };
            private readonly List<char> cartDirections = new List<char>() { '^', '>', 'v', '<' };
            private int nextIntersectionDirection = 0;

            public MineCart(int top, int left, char direction)
            {
                Top = top;
                Left = left;
                Direction = direction;
            }

            public bool Move(List<string> tracks, Dictionary<(int Top, int Left), MineCart> positionMap)
            {
                positionMap.Remove((Top, Left));
                // Do move
                switch (Direction)
                {
                    case '^':
                        this.Top -= 1;
                        break;
                    case '>':
                        this.Left += 1;
                        break;
                    case 'v':
                        this.Top += 1;
                        break;
                    case '<':
                        this.Left -= 1;
                        break;
                    default:
                        throw new InvalidOperationException($"Unknown direction {Direction}");
                }

                switch (tracks[Top][Left])
                {
                    case '/':
                        if ("v^".Contains(Direction))
                        {
                            Direction = cartDirections[(cartDirections.IndexOf(Direction) + 1) % cartDirections.Count];
                        }
                        else
                        {
                            Direction = cartDirections[(cartDirections.IndexOf(Direction) + 3) % cartDirections.Count];
                        }
                        break;
                    case '\\':
                        if ("v^".Contains(Direction))
                        {
                            Direction = cartDirections[(cartDirections.IndexOf(Direction) + 3) % cartDirections.Count];
                        }
                        else
                        {
                            Direction = cartDirections[(cartDirections.IndexOf(Direction) + 1) % cartDirections.Count];
                        }
                        break;
                    case '+':
                        var intersectionDirection = interSectionDirections[nextIntersectionDirection];
                        nextIntersectionDirection = (nextIntersectionDirection + 1) % interSectionDirections.Length;
                        if (intersectionDirection == "left")
                        {
                            Direction = cartDirections[(cartDirections.IndexOf(Direction) + 3) % cartDirections.Count];
                        }
                        else if (intersectionDirection == "right")
                        {
                            Direction = cartDirections[(cartDirections.IndexOf(Direction) + 1) % cartDirections.Count];
                        }
                        break;
                    case '-':
                    case '|':
                        break; // No direction change
                    default:
                        throw new InvalidOperationException($"Unknown track {tracks[Top][Left]}");
                }

                // Check collision and re add to positionMap
                if (positionMap.ContainsKey((Top, Left)))
                {
                    positionMap.Remove((Top, Left));
                    return true;
                }
                positionMap[(Top, Left)] = this;
                return false;
            }

            public int Top { get; private set; }
            public int Left { get; private set; }

            public char Direction { get; private set; } 
        }
    }
}
