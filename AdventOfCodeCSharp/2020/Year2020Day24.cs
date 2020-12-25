using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 24)]
    internal class Year2020Day24 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var tileMap = InitializeFloor(input);

            return $"{tileMap.Count(kvp => kvp.Value)}";
        }

        public string Part2(IList<string> input)
        {
            var tileMap = InitializeFloor(input);
            tileMap = FlipForDays(tileMap, 100);

            return $"{tileMap.Count(kvp => kvp.Value)}";
        }

        private Dictionary<(int x, int y), bool> InitializeFloor(IList<string> input)
        {
            var paths = GetPaths(input);
            // true is black    
            var tileMap = new Dictionary<(int x, int y), bool>();
            foreach (var path in paths)
            {
                var coord = GetCoordinate(path);
                if (tileMap.ContainsKey(coord))
                {
                    tileMap[coord] = !tileMap[coord];
                }
                else
                {
                    tileMap[coord] = true;
                }
            }

            return tileMap;
        }

        private Dictionary<(int x, int y), bool> FlipForDays(Dictionary<(int x, int y), bool> floor, int numDays)
        {
            var result = floor;
            for (int i = 0; i < numDays; i++)
            {
                result = FlipTiles(result);
            }

            return result;
        }

        private Dictionary<(int x, int y), bool> FlipTiles(Dictionary<(int x, int y), bool> floor)
        {
            var result = new Dictionary<(int x, int y), bool>();
            var tilesToCheck = new HashSet<(int x, int y)>(
                floor.Keys.SelectMany(k => GetSurroundingTiles(k)));

            foreach (var tile in tilesToCheck)
            {
                var neighbours = GetSurroundingTiles(tile);
                var blackCount = neighbours
                    .Count(t => IsTileBlack(floor, t));

                if (IsTileBlack(floor, tile))
                {
                    if (!(blackCount == 0 || blackCount > 2))
                    {
                        result[tile] = true;
                    }
                }
                else
                {
                    if (blackCount == 2)
                    {
                        result[tile] = true;
                    }
                }
            }

            return result;
        }

        private bool IsTileBlack(Dictionary<(int x, int y), bool> floor, (int x, int y) tile)
        {
            if (floor.ContainsKey(tile))
            {
                return floor[tile];
            }

            return false;
        }

        private IEnumerable<(int x, int y)> GetSurroundingTiles((int x, int y) coordinate)
        {
            foreach (var direction in new string[] 
            {
                "ne",
                "e",
                "se",
                "sw",
                "w",
                "nw",
            })
            {
                var (dx, dy) = GetMove(direction);
                yield return (coordinate.x + dx, coordinate.y + dy);
            }
        }

        internal IEnumerable<List<string>> GetPaths(IList<string> input)
        {
            var pattern = new Regex("(?<direction>e|se|sw|w|nw|ne)+");
            foreach (var path in input.Select(p => p.Trim()).Where(p => p.Length > 0))
            {
                var match = pattern.Match(path);
                yield return match.Groups["direction"].Captures.Select(c => c.Value).ToList();
            }
        }

        internal (int x, int y) GetCoordinate(IEnumerable<string> path)
        {
            return path
                .Select(GetMove)
                .Aggregate((x: 0, y: 0), (acc, v) => (acc.x + v.x, acc.y + v.y));
        }

        private (int x, int y) GetMove(string direction)
        {
            return direction switch
            {
                "ne" => (1, -1),
                "e"  => (1, 0),
                "se" => (0, 1),
                "sw" => (-1, 1),
                "w"  => (-1, 0),
                "nw" => (0, -1),
                _ => throw new ArgumentException($"Not a valid direction '{direction}'", nameof(direction)),
            };
        }
    }
}
