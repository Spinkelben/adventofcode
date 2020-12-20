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

            var cornerTiles = GetCornerTiles(tiles, edgeDict);
            long product = cornerTiles.Aggregate(1L, (acc, t) => t.Id * acc);

            return $"{product}";
        }

        private static IEnumerable<Tile> GetCornerTiles(List<Tile> tiles, Dictionary<string, List<(Tile, int edgeNumber, bool flipped)>> edgeDict)
        {
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
                    return edgeSet.Count == 4; // 2 free edges with 2 possible rotations eqals 4 "edges"
                });
            return cornerTiles;
        }

        public string Part2(IList<string> input)
        {
            var tiles = ParseInput(input)
                .ToList();
            var edgeDict = GetEdgeDict(tiles);
            var grid = AssembleTiles(tiles, edgeDict);

            var pattern = GetPattern(grid);
            

            return string.Empty;
        }

        private List<string> GetPattern(List<List<Tile>> puzzle)
        {
            var result = new List<string>();
            foreach (var line in puzzle)
            {
                StringBuilder lineBuilder = null;
                for (int patternIdx = 0; patternIdx < line[0].Pattern.Count; patternIdx++)
                {
                    lineBuilder = new StringBuilder();
                    foreach (var tile in line)
                    {
                        lineBuilder.Append(
                            string.Join("", tile.GetPatternLine(patternIdx)));
                        lineBuilder.Append(" ");
                    }
                    result.Add(lineBuilder.ToString());
                }
                result.Add(string.Join("", Enumerable.Repeat(" ", lineBuilder.Length)));
            }

            return result;
        }

        private List<List<Tile>> AssembleTiles(List<Tile> tiles, Dictionary<string, List<(Tile, int edgeNumber, bool flipped)>> edgeDict)
        {
            var idDict = tiles.ToDictionary(t => t.Id);
            var gridSize = Math.Sqrt(tiles.Count);
            var result = new List<List<Tile>>();
            var currentLine = new List<Tile>();
            var startCorner = GetCornerTiles(tiles, edgeDict)
                .First();

            RotateCorner(startCorner, edgeDict);

            var remainingEdgesDict = new Dictionary<string, List<(Tile, int, bool)>>(edgeDict);
            var currentTile = startCorner;
            while (true)
            {
                currentLine.Add(currentTile);
                var edge = currentTile.GetEdge(1);
                var matches = GetMatchingTile(currentTile.Id, edge, remainingEdgesDict).ToList();
                //remainingEdgesDict.Remove(edge);
                if (currentLine.Count == gridSize)
                {
                    result.Add(currentLine);

                    if (result.Count == gridSize)
                    {
                        break;
                    }

                    var nextLineMatches = GetMatchingTile(currentLine[0].Id, currentLine[0].GetEdge(2), remainingEdgesDict).ToList();

                    currentLine = new List<Tile>();
                    currentTile = nextLineMatches[0].t;
                    currentTile.FlipX = nextLineMatches[0].flipped;
                    currentTile.Rotation += nextLineMatches[0].edgeNum;
                    
                    continue;
                }

                currentTile = matches[0].t;
                var edgeNum = matches[0].edgeNum;
                currentTile.FlipY = matches[0].flipped;
                currentTile.Rotation += edgeNum - 3;

            }

            return result;
        }

        private void RotateCorner(Tile startCorner, Dictionary<string, List<(Tile, int edgeNumber, bool flipped)>> edgeDict)
        {
            
            var matchRight = GetMatchingTile(startCorner.Id, startCorner.GetEdge(1), edgeDict).ToList();
            var matchDown = GetMatchingTile(startCorner.Id, startCorner.GetEdge(2), edgeDict).ToList();
            while (matchRight.Count == 0
                || matchDown.Count == 0)
            {
                startCorner.Rotation++;
                matchRight = GetMatchingTile(startCorner.Id, startCorner.GetEdge(1), edgeDict).ToList();
                matchDown = GetMatchingTile(startCorner.Id, startCorner.GetEdge(2), edgeDict).ToList();
            }

            startCorner.FlipY = matchRight[0].flipped;
            startCorner.FlipX = matchDown[0].flipped;
        }

        private IEnumerable<(Tile t, int edgeNum, bool flipped)> GetMatchingTile(int tileId, string edge, Dictionary<string, List<(Tile, int edgeNumber, bool flipped)>> edgeDict)
        {
            var matches = edgeDict[edge].Where(t => t.Item1.Id != tileId);
            return matches;
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

                    // Handle flipped tiles
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
            private int rotation;

            internal int Id { get; set; }

            internal bool FlipX { get; set; }

            internal bool FlipY { get; set; }

            // 1 = one turn clockwise, 4 is a full 360 degrees
            internal int Rotation { get => rotation; set => rotation = value % 4; }

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

            internal string GetPatternLine(int lineNum)
            {
                if (Rotation == 0)
                {
                    return HandleFlipX(Pattern[lineNum]);
                }
                else if (Rotation == 1)
                {
                    return HandleFlipX(Pattern.Select(l => l[lineNum]).Reverse());
                }
                if (Rotation == 2)
                {
                    return HandleFlipX(Pattern[^(lineNum + 1)].Reverse());
                }
                else if (Rotation == 3)
                {
                    return HandleFlipX(Pattern.Select(l => l[^(lineNum + 1)]));
                }
                else
                {
                    throw new IndexOutOfRangeException($"Value {lineNum} is greater than tile size");
                }
            }

            internal string GetPatternColumn(int columnNum)
            {
                if (Rotation == 0)
                {
                    return HandleFlipY(Pattern.Select(l => l[columnNum]));
                }
                else if (Rotation == 1)
                {
                    return HandleFlipY(Pattern[^(columnNum + 1)]);
                }
                if (Rotation == 2)
                {
                    return HandleFlipY(Pattern.Select(l => l[^(columnNum + 1)]).Reverse());
                }
                else if (Rotation == 3)
                {
                    return HandleFlipY(Pattern[columnNum].Reverse());
                }
                else
                {
                    throw new IndexOutOfRangeException($"Value {columnNum} is greater than tile size");
                }
            }

            private string HandleFlipX(IEnumerable<char> line)
            {
                if (FlipX)
                {
                    return string.Join(string.Empty, line.Reverse());
                }
                else
                {
                    return string.Join(string.Empty, line);
                }
            }

            private string HandleFlipY(IEnumerable<char> column)
            {
                if (FlipY)
                {
                    return string.Join(string.Empty, column.Reverse());
                }
                else
                {
                    return string.Join(string.Empty, column);
                }
            }

            internal string GetEdge(int direction)
            {
                if (direction == 0)
                {
                    return GetPatternLine(0);
                }
                else if (direction == 1)
                {
                    return GetPatternColumn(Pattern.Count - 1);
                }
                else if (direction == 2)
                {
                    return GetPatternLine(Pattern.Count - 1);
                }
                else if (direction == 3)
                {
                    return GetPatternColumn(0);
                }
                else
                {
                    throw new IndexOutOfRangeException($"Directions are on 0-3 {direction}");
                }
            }

        }
    }
}
