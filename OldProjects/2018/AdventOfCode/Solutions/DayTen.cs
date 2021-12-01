using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class DayTen : IDaySolution
    {
        public string PartOne(string input)
        {
            var points = input.Split('\n').Where(s => string.IsNullOrWhiteSpace(s) == false).Select(s => new Point(s.Trim())).ToList();
            var time = 0;
            var lastDist = int.MaxValue;
            for (int i = 0; i < 100000; i++)
            {
                var dist = int.MinValue;
                foreach (var outer in points)
                {
                    var maxDist = int.MinValue;
                    foreach (var inner in points)
                    {
                        if (inner == outer)
                        {
                            continue;
                        }
                        var pairDist = outer.ManhattanDistance(inner);
                        maxDist = maxDist < pairDist ? pairDist : maxDist;
                    }
                    dist = dist < maxDist ? maxDist : dist;
                }

                var stepSize = Math.Max(dist / 1000, 1);

                if (lastDist < dist)
                {
                    foreach (var point in points)
                    {
                        point.AdvanceTime(-1);
                        time--;
                    }
                    
                    Console.WriteLine($"Iteration {i}, Step size: {stepSize} Dist: {dist}, Time: {time}");
                    Console.WriteLine(PrintPosition(points));
                    return PrintPosition(points);
                }


                lastDist = dist;


                foreach (var point in points)
                {
                    point.AdvanceTime(stepSize);
                }
                time += stepSize;
            }

            return time.ToString();
        }

        public string PartTwo(string input)
        {
            return "";
        }

        public string PrintPosition(List<Point> points)
        {
            (int top, int left) topLeft = (points.Min(p => p.Top), points.Min(p => p.Left));
            (int top, int left) bottomRight = (points.Max(p => p.Top), points.Max(p => p.Left));

            (int width, int height) dimension = (Math.Abs(topLeft.left - bottomRight.left) + 1, Math.Abs(topLeft.top - bottomRight.top) + 1);

            var image = new char[dimension.height, dimension.width];

            foreach (var point in points)
            {
                image[point.Top - topLeft.top, point.Left - topLeft.left] = '#';
            }

            var imageBuilder = new StringBuilder();
            for (int i = 0; i < dimension.height; i++)
            {
                var lineBuilder = new StringBuilder();
                for (int j = 0; j < dimension.width; j++)
                {
                    image[i, j] = image[i, j] == '#' ? '#' : '.';
                    lineBuilder.Append(image[i, j]);
                }
                imageBuilder.Append(lineBuilder);
                imageBuilder.Append(Environment.NewLine);
            }
            return imageBuilder.ToString();
        }

        public class Point
        {
            private static readonly Regex parseExpression = new Regex(@"position=<(?<left>.+),(?<top>.+)> velocity=<(?<hVel>.+),(?<vVel>.+)>");
            public Point(string inputLine)
            {
                var match = parseExpression.Match(inputLine);
                Top = int.Parse(match.Groups["top"].Value);
                Left = int.Parse(match.Groups["left"].Value);
                HorizontalVelocity = int.Parse(match.Groups["hVel"].Value);
                VerticalVelocity = int.Parse(match.Groups["vVel"].Value);
            }

            public int ManhattanDistance(Point other)
            {
                return Math.Abs(this.Top - other.Top) + Math.Abs(this.Left - other.Left);
            }

            public int Top { get; private set; }

            public int Left { get; private set; }

            public int HorizontalVelocity { get; }

            public int VerticalVelocity { get; }

            public (int t, int l) Position { get => (Top, Left); }

            public void AdvanceTime(int steps)
            {
                Top += VerticalVelocity * steps;
                Left += HorizontalVelocity * steps;
            }
        }
    }
}
