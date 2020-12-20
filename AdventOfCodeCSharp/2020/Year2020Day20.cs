using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 20)]
    internal class Year2020Day20 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var tiles = ParseInput(input)
                .ToList();
            var edgeDict = GetEdgeDict(tiles);

            // I looked at the edge dict for my input. All tile edges have at most one other matching edge
            // That makes the puzzle easier, as you don't have to try multiple pieces at each location
            // Furthermore finding cornsers is then as easy as finding the pecies that have two unique edges

            var cornerEdges = new HashSet<string>(
                edgeDict
                    .Where(e => e.Value.Count == 1)
                    .Select(e => e.Key));

            var cornerTiles = tiles
                .Where(t =>
                {
                    var edgeSet = new HashSet<string>(t.Edges);
                    foreach (var edge in t.Edges)
                    {
                        edgeSet.Add(string.Join("", edge.Reverse()));
                    }

                    edgeSet.IntersectWith(cornerEdges);
                    return edgeSet.Count == 4;
                });
            long product = cornerTiles.Aggregate(1L, (acc, t) => t.Id * acc);

            return $"{product}";
        }

        public string Part2(IList<string> input)
        {
            return string.Empty;
        }

        internal Dictionary<string, List<(Tile, int edgeNumber, bool flipped)>> GetEdgeDict(IEnumerable<Tile> tiles)
        {
            var result = new Dictionary<string, List<(Tile, int, bool)>>();
            foreach (var tile in tiles)
            {
                var edges = tile.Edges;
                for (int i = 0; i < edges.Count; i++)
                {
                    var edge = edges[i];
                    if (!result.ContainsKey(edge))
                    {
                        result[edge] = new List<(Tile, int, bool)>();
                    }
                    result[edge].Add((tile, i, false));

                    var reverse = string.Join("", edges[i].Reverse());
                    if (!result.ContainsKey(reverse))
                    {
                        result[reverse] = new List<(Tile, int, bool)>();
                    }
                    result[reverse].Add((tile, i, true));
                }
            }

            return result;
        }

        internal IEnumerable<Tile> ParseInput(IList<string> input)
        {
            var currentTile = new Tile();
            var headerPattern = new Regex(@"^Tile (?<id>\d+):$");
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (currentTile.Id != 0)
                    {
                        yield return currentTile;
                        currentTile = new Tile();
                    }
                    continue;
                }

                var headerMatch = headerPattern.Match(line);
                if (headerMatch.Success)
                {
                    currentTile.Id = int.Parse(headerMatch.Groups["id"].Value);
                }
                else
                {
                    currentTile.Pattern.Add(line.ToCharArray());
                }
            }

            if (currentTile.Id != 0)
            {
                yield return currentTile;
            }
        }

        internal class Tile
        {
            internal int Id { get; set; }

            internal List<char[]> Pattern { get; } = new List<char[]>();

            internal List<string> Edges
            {
                get
                {
                    var result = new List<string>();
                    result.Add(string.Join("", Pattern[0])); // Top Edge
                    result.Add(string.Join("", Pattern.Select(l => l[^1]))); // Right Edge
                    result.Add(string.Join("", Pattern[^1])); // Bottom Edge
                    result.Add(string.Join("", Pattern.Select(l => l[0]))); // Left Edge
                    return result;
                }
            }

        }
    }
}
