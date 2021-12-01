using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    internal class DayThree : IDaySolution
    {
        public string PartOne(string input)
        {
            var claimList = input.Split('\n').Where(s => string.IsNullOrWhiteSpace(s) == false).Select(s => new Claim(s)).ToList();
            var squares = new Dictionary<string, int>();
            foreach (var claim in claimList)
            {
                foreach (var square in EnumerateSquares(claim))
                {
                    if (squares.ContainsKey(square) == false)
                    {
                        squares[square] = 0;
                    }
                    squares[square] += 1;
                }
            }
            return $"{squares.Count(s => s.Value > 1)}";
        }

        public string PartTwo(string input)
        {
            var claimList = input.Split('\n').Where(s => string.IsNullOrWhiteSpace(s) == false).Select(s => new Claim(s)).ToList();
            var squares = new Dictionary<string, int>();
            foreach (var claim in claimList)
            {
                foreach (var square in EnumerateSquares(claim))
                {
                    if (squares.ContainsKey(square) == false)
                    {
                        squares[square] = 0;
                    }
                    squares[square] += 1;
                }
            }
            return claimList.Where(c => EnumerateSquares(c).All(s => squares[s] == 1)).First().Id.ToString();
        }

        private IEnumerable<string> EnumerateSquares(Claim claim)
        {
            for (int i = 0; i < claim.Width; i++)
            {
                for (int j = 0; j < claim.Height; j++)
                {
                    yield return $"{claim.Left + i},{claim.Top + j}";
                }
            }
        }

        private class Claim
        {
            internal Claim(string input)
            {
                var split = input.Split(' ');
                Id = int.Parse(split[0].Substring(1));
                var coords = split[2].Split(',');
                Left = int.Parse(coords[0]);
                Top = int.Parse(coords[1].Replace(":", string.Empty));
                var dimensions = split[3].Split('x');
                Width = int.Parse(dimensions[0]);
                Height = int.Parse(dimensions[1]);
            }

            internal int Id { get; set; }
            internal int Top { get; set; }
            internal int Left { get; set; }
            internal int Width { get; set; }
            internal int Height { get; set; }

        }
    }
}
