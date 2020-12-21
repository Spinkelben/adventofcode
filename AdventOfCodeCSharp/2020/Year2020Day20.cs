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
            var image = AssembleImage(grid);
            var patterns = GetAllVariations(pattern).ToList();
            var variations = GetAllVariations(image).ToList();

            return string.Empty;
        }

        internal List<string> Rotate(List<string> input, int rotation)
        {
            var result = new List<string>();
            if (rotation == 0)
            {
                return new List<string>(input);
            }
            else if (rotation == 1)
            {
                for (int i = 0; i < input[0].Length; i++)
                {
                    result.Add(string.Join(string.Empty, input.Select(l => l[i]).Reverse()));
                }
            }
            else if (rotation == 2)
            {
                for (int i = 1; i <= input.Count; i++)
                {
                    result.Add(string.Join(string.Empty, input[^i].Reverse()));
                }
            }
            else if (rotation == 3)
            {
                for (int i = 1; i <= input[0].Length; i++)
                {
                    result.Add(string.Join(string.Empty, input.Select(l => l[^i])));
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Can only rotate 0 - 3");
            }

            return result;
        }

        private List<string> GetPattern(List<List<Tile>> puzzle)
        {
            var result = new List<string>();
            foreach (var line in puzzle)
            {
                StringBuilder lineBuilder = null;
                for (int patternIdx = 0; patternIdx < line[0].GetPatternColumn(0).Length; patternIdx++)
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

        private IEnumerable<List<string>> GetAllVariations(List<string> image)
        {
            foreach (var (flipX, flipY) in new List<(bool, bool)>() 
            {
                (false, false),
                (false, true),
                (true, false),
                (true, true),
            })
            {
                for (int i = 0; i < 4; i++)
                {
                    var result = Rotate(image, i);

                    if (flipX)
                    {
                        result = FlipX(result);
                    }

                    if (flipY)
                    {
                        result = FlipY(result);
                    }

                    yield return result;
                }
            }
        }

        private List<string> FlipX(List<string> input)
        {
            var result = new List<string>();
            foreach (var line in input)
            {
                result.Add(string.Join(string.Empty, line.Reverse()));
            }

            return result;
        }

        private List<string> FlipY(List<string> input)
        {
            var result = new List<string>(input);
            result.Reverse();
            return result;
        }

        private List<string> AssembleImage(List<List<Tile>> puzzle)
        {
            var result = new List<string>();
            foreach (var line in puzzle)
            {
                for (int patternLineIdx = 1; patternLineIdx < line[0].GetPatternColumn(0).Length - 1; patternLineIdx++)
                {
                    StringBuilder lineBuilder = new StringBuilder();
                    foreach (var tile in line)
                    {
                        lineBuilder.Append(
                            string.Join("", tile.GetPatternLine(patternLineIdx)[1..^1]));
                    }
                    result.Add(lineBuilder.ToString());
                }
            }

            return result;
        }

        internal List<List<Tile>> AssembleTiles(List<Tile> tiles, Dictionary<string, List<(Tile, int edgeNumber, bool flipped)>> edgeDict)
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
                    ConnectPiece(
                        currentLine[0], 
                        2, 
                        nextLineMatches[0].t, 
                        (t) => GetMatchingTile(t.Id, t.GetEdge(1), edgeDict).Count() > 0);
                    currentLine = new List<Tile>();
                    currentTile = nextLineMatches[0].t;
                    
                    continue;
                }

                ConnectPiece(currentTile, 1, matches[0].t);
                currentTile = matches[0].t;
            }

            return result;
        }

        private void ConnectPiece(Tile target, int targetEdge, Tile newPiece, Func<Tile, bool> extraCheck = null)
        {
            var pattern = target.GetEdge(targetEdge);
            var connectingEdge = (targetEdge + 2) % 4;

            foreach (var (flipX, flipY) in new List<(bool, bool)>() 
            { 
                (false, false),
                (true, false),
                (false, true)
            })
            {
                newPiece.FlipX = flipX;
                newPiece.FlipY = flipY;
                for (int rotation = 0; rotation < 4; rotation++)
                {
                    newPiece.Rotation = rotation;
                    if (newPiece.GetEdge(connectingEdge) == pattern)
                    {
                        if (extraCheck is object)
                        {
                            if (extraCheck(newPiece))
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            

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
            var rightPiece = matchRight[0].t;
            var downPiece = matchDown[0].t;
            ConnectPiece(startCorner, 1, rightPiece, t => GetMatchingTile(t.Id, t.GetEdge(1), edgeDict).Count() > 0);
            ConnectPiece(startCorner, 2, downPiece, t => GetMatchingTile(t.Id, t.GetEdge(2), edgeDict).Count() > 0);

            if (GetMatchingTile(rightPiece.Id, rightPiece.GetEdge(1), edgeDict).Count() == 0
                || GetMatchingTile(downPiece.Id, downPiece.GetEdge(2), edgeDict).Count() == 0)
            {
                throw new Exception("Oppsiiie");
            }
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
                    currentTile.AddLine(line.ToCharArray());
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
            internal int Rotation
            {
                get => rotation;
                set {
                    var clamped = value % 4;
                    if (clamped < 0)
                    {
                        clamped += 4;
                    }

                    rotation = clamped; 
                }
            }

            private List<char[]> Pattern { get; } = new List<char[]>();

            internal void AddLine(char[] line)
            {
                Pattern.Add(line);
            }

            internal List<string> Edges
            {
                get
                {
                    var result = new List<string>();
                    result.Add(GetEdge(0)); // Top Edge
                    result.Add(GetEdge(1)); // Right Edge
                    result.Add(GetEdge(2)); // Bottom Edge
                    result.Add(GetEdge(3)); // Left Edge
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
                    return HandleFlipY(Pattern.Select(l => l[lineNum]).Reverse());
                }
                if (Rotation == 2)
                {
                    return HandleFlipX(Pattern[^(lineNum + 1)].Reverse());
                }
                else if (Rotation == 3)
                {
                    return HandleFlipY(Pattern.Select(l => l[^(lineNum + 1)]));
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
                    return HandleFlipX(Pattern[^(columnNum + 1)]);
                }
                if (Rotation == 2)
                {
                    return HandleFlipY(Pattern.Select(l => l[^(columnNum + 1)]).Reverse());
                }
                else if (Rotation == 3)
                {
                    return HandleFlipX(Pattern[columnNum].Reverse());
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

            internal void FlipXRelative(bool v)
            {
                if (rotation % 2 == 0)
                {
                    FlipX = v;
                }
                else
                {
                    FlipY = v;
                }
            }

            internal void FlipYRelative(bool v)
            {
                if (rotation % 2 == 0)
                {
                    FlipY = v;
                }
                else
                {
                    FlipX = v;
                }
            }

            internal bool GetFlipXRelative()
            {
                if (rotation % 2 == 0)
                {
                    return FlipX;
                }
                else
                {
                    return FlipY;
                }
            }

            internal bool GetFlipYRelative()
            {
                if (rotation % 2 == 0)
                {
                    return FlipY;
                }
                else
                {
                    return FlipX;
                }
            }
        }
    }
}
