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
            var stableState = GetStableState(ToCharArray(input.Where(l => l.Length > 0)));
            var numOccupiedSeats = stableState.Sum(s => s.Count(c => c == '#'));
            return $"{numOccupiedSeats}";
        }

        internal List<char[]> GetNextState(List<char[]> currentState)
        {
            var nextState = InitializeState(currentState);

            for (int y = 0; y < currentState.Count; y++)
            {
                for (int x = 0; x < currentState[y].Length; x++)
                {
                    nextState[y][x] = UpdateField(currentState, y, x);
                }
            }

            return nextState;
        }

        private List<char[]> GetStableState(List<char[]> start)
        {
            var next = GetNextState(start);
            while (!IsSeatsEqual(start, next))
            {
                start = next;
                next = GetNextState(start);
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

        public string Part2(IList<string> input)
        {
            return "";
        }
    }
}
