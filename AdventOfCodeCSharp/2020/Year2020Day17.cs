using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 17)]
    internal class Year2020Day17 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var cube = new ConwayCube(input);
            for (int i = 0; i < 6; i++)
            {
                cube.RunCycle();
            }

            return $"{cube.NumActiveCells}";
        }

        public string Part2(IList<string> input)
        {
            var cube = new ConwayCube(input, true);
            for (int i = 0; i < 6; i++)
            {
                cube.RunCycle();
            }

            return $"{cube.NumActiveCells}";
        }

        internal class ConwayCube
        {
            private Dictionary<(int x, int y, int z, int w), char> cells;
            private bool use4thDimension = false;

            internal ConwayCube(IList<string> startState, bool use4thDimension = false)
            {
                cells = new Dictionary<(int x, int y, int z, int w), char>();
                this.use4thDimension = use4thDimension;
                for (int y = 0; y < startState.Count; y++)
                {
                    for (int x = 0; x < startState[y].Length; x++)
                    {
                        cells[(x, y, 0, 0)] = startState[y][x];
                    }
                }
            }

            internal void RunCycle()
            {
                var newState = new Dictionary<(int, int, int, int), char>();
                foreach (var coord in new HashSet<(int, int, int, int)>(cells.SelectMany(c => GetNeighbours(c.Key))))
                {
                    var numActiveNeighbours = GetNeighbours(coord)
                        .Select(c => 
                        {
                            if (cells.ContainsKey(c))
                            {
                                return cells[c];
                            }
                            return '.';
                        })
                        .Count(v => v == '#');

                    if (numActiveNeighbours == 0)
                    {
                        // Prune empty areas
                        continue;
                    }

                    var value = cells.ContainsKey(coord) ? cells[coord] : '.';

                    if (value == '#')
                    {
                        if (numActiveNeighbours >= 2 && numActiveNeighbours <= 3)
                        {
                            newState[coord] = '#';
                        }
                        else
                        {
                            newState[coord] = '.';
                        }
                    }
                    else
                    {
                        if (numActiveNeighbours == 3)
                        {
                            newState[coord] = '#';
                        }
                        else
                        {
                            newState[coord] = '.';
                        }
                    }
                }

                cells = newState;
            }

            internal int NumActiveCells => cells.Count(kvp => kvp.Value == '#');

            private IEnumerable<(int x, int y, int z, int w)> GetNeighbours((int x, int y, int z, int w) coordinate)
            {
                for (int z = coordinate.z - 1; z <= coordinate.z + 1; z++)
                {
                    for (int y = coordinate.y - 1; y <= coordinate.y + 1; y++)
                    {
                        for (int x = coordinate.x - 1; x <= coordinate.x + 1; x++)
                        {
                            if (use4thDimension)
                            {
                                for (int w = coordinate.w - 1; w <= coordinate.w + 1; w++)
                                {
                                    if ((x, y, z, w) != coordinate)
                                    {
                                        yield return (x, y, z, w);
                                    }
                                }
                            }
                            else if ((x, y, z, 0) != coordinate)
                            {
                                yield return (x, y, z, 0);
                            }
                        }
                    }
                }
            }
        }
    }
}
