using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 11)]
    internal class Year2020Day11 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var stableState = GetStableState(ToCharArray(input.Where(l => l.Length > 0)), false);
            var numOccupiedSeats = stableState.Sum(s => s.Count(c => c == '#'));
            return $"{numOccupiedSeats}";
        }

        public string Part2(IList<string> input)
        {
            var stableState = GetStableState(ToCharArray(input.Where(l => l.Length > 0)), true);
            var numOccupiedSeats = stableState.Sum(s => s.Count(c => c == '#'));
            return $"{numOccupiedSeats}";
        }

        internal List<char[]> GetNextState(List<char[]> currentState, bool useLineOfSight)
        {
            var nextState = InitializeState(currentState);

            for (int y = 0; y < currentState.Count; y++)
            {
                for (int x = 0; x < currentState[y].Length; x++)
                {
                    if (useLineOfSight)
                    {
                        nextState[y][x] = UpdateFieldLineOfSight(currentState, y, x);
                    }
                    else
                    {
                        nextState[y][x] = UpdateField(currentState, y, x);
                    }
                }
            }

            return nextState;
        }

        private List<char[]> GetStableState(List<char[]> start, bool useLineOfSight)
        {
            var next = GetNextState(start, useLineOfSight);
            while (!IsSeatsEqual(start, next))
            {
                start = next;
                next = GetNextState(start, useLineOfSight);
            }

            return next;
        }

        private bool IsSeatsEqual(List<char[]> state, List<char[]> nextState)
        {
            for (int y = 0; y < state.Count; y++)
            {
                for (int x = 0; x < state[y].Length; x++)
                {
                    if (state[y][x] != nextState[y][x])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private char UpdateFieldLineOfSight(List<char[]> currentState, int y, int x)
        {
            return currentState[y][x] switch
            {
                '#' => GetChairsInLineOfSight(currentState, (x, y))
                        .Count(c => c == '#') >= 5 ? 'L' : '#',
                'L' => GetChairsInLineOfSight(currentState, (x, y))
                        .Count(c => c == '#') == 0 ? '#' : 'L',
                '.' => '.',
                char unknown => throw new ArgumentOutOfRangeException(
                        "currentState",
                        $"Unsupported character encountered {unknown}"),
            };
        }

        private IEnumerable<char> GetChairsInLineOfSight(List<char[]> currentState, (int x, int y) origin)
        {
            var directions = new List<(int dx, int dy)>()
            {
                (-1, 0),    // Left
                (-1, -1),   // Up-Left
                (0, -1),    // Up
                (1, -1),    // Up-right
                (1, 0),     // Right
                (1, 1),     // Down right
                (0, 1),     // Down
                (-1, 1),    // Down-left
            };

            return directions.Select(d => Look(currentState, d, origin));
        }

        private char Look(List<char[]> currentState, (int dx, int dy) direction, (int x, int y) origin)
        {
            var x = direction.dx + origin.x;
            var y = direction.dy + origin.y;
            var seen = '.';

            while (IsValidCoordinate((x, y), currentState))
            {
                seen = currentState[y][x];
                if (seen == '#' || seen == 'L')
                {
                    return seen;
                }
                x += direction.dx;
                y += direction.dy;
            }

            return seen;
        }

        private char UpdateField(List<char[]> currentState, int y, int x)
        {
            var numAdjecent = 0;
            switch (currentState[y][x])
            {
                case 'L':
                    numAdjecent = GetAdjecentSpaces(currentState, (x, y))
                        .Where(c => c == '#')
                        .Count();
                    if (numAdjecent == 0)
                    {
                        return '#';
                    }

                    return 'L';
                case '#':
                    numAdjecent = GetAdjecentSpaces(currentState, (x, y))
                        .Where(c => c == '#')
                        .Count();

                    if (numAdjecent >= 4)
                    {
                        return 'L';
                    }

                    return '#';
                case '.':
                    return '.';
                default:
                    throw new ArgumentOutOfRangeException(
                        "currentState",
                        $"Unsupported character encountered {currentState[y][x]}");
            }
        }

        private List<char[]> InitializeState(List<char[]> state)
        {
            var copy = new List<char[]>();
            foreach (var line in state)
            {
                var newLine = new char[line.Length];
                copy.Add(newLine);
            }

            return copy;
        }

        private IEnumerable<char> GetAdjecentSpaces(List<char[]> seatingState, (int x, int y) position)
        {
            var (x, y) = position;
            var coordinates = new List<(int x, int y)>
            {
                (x - 1, y - 1),
                (x, y - 1),
                (x + 1, y - 1),
                (x - 1, y),
                (x + 1, y),
                (x - 1, y + 1),
                (x, y + 1),
                (x + 1, y + 1)
            };

            foreach (var coordinate in coordinates.Where(c => IsValidCoordinate(c, seatingState)))
            {
                yield return seatingState[coordinate.y][coordinate.x];
            }
        }

        private bool IsValidCoordinate((int x, int y) coordinate, List<char[]> seatingState)
        {
            var xValid = coordinate.x >= 0 && coordinate.x < seatingState[0].Length;
            var yValid = coordinate.y >= 0 && coordinate.y < seatingState.Count;
            return xValid && yValid;
        }

        private List<char[]> ToCharArray(IEnumerable<string> input)
        {
            var result = new List<char[]>();
            foreach (var line in input)
            {
                result.Add(line.ToCharArray());
            }

            return result;
        }
    }
}
