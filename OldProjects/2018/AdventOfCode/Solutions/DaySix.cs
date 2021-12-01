using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class DaySix : IDaySolution
    {
        private readonly int partTwoMinDist;

        public DaySix()
            : this(10000)
        {
        }

        public DaySix(int partTwoMinDist)
        {
            this.partTwoMinDist = partTwoMinDist;
        }

        public string PartOne(string input)
        {
            var dangerPoints = new Dictionary<(int, int), int>();
            foreach (var dangerpoint in ToCoordinates(input))
            {
                dangerPoints[dangerpoint] = 0;
            }

            var dimensions = this.CalculateMapDimensions(dangerPoints.Select(d => d.Key).ToList());

            foreach (var coord in EnumerateCoordinates(dimensions))
            {
                (var tie, var closetPoint) = GetClosestPoint(dangerPoints, coord);

                if (tie == false)
                {
                    dangerPoints[closetPoint]++;
                }
            }

            var excludeList = new HashSet<(int x, int y)>();
            foreach (var borderPoint in EnumerateBorder(dimensions))
            {
                var distances = dangerPoints
                    .Select(d => (coord: d.Key, dist: ManhattenDistance(d.Key, borderPoint))).OrderBy(d => d.dist).Take(2).ToList();
                if (distances[0].dist != distances[1].dist)
                {
                    excludeList.Add(distances[0].coord);
                }
            }

            return dangerPoints.Where(d => excludeList.Contains(d.Key) == false).OrderByDescending(d => d.Value).First().Value.ToString();
        }

        public (bool tie, (int x, int y) coordiante) GetClosestPoint(Dictionary<(int, int), int> dangerPoints, (int x, int y) coord)
        {
            var minDist = int.MaxValue;
            (int, int) closetPoint = default;
            bool tie = false;
            foreach (var dangerpoint in dangerPoints)
            {
                var distance = ManhattenDistance(dangerpoint.Key, coord);
                if (distance < minDist)
                {
                    minDist = ManhattenDistance(dangerpoint.Key, coord);
                    closetPoint = dangerpoint.Key;
                    tie = false;
                }
                else if (distance == minDist)
                {
                    tie = true;
                }
            }

            return (tie, closetPoint);
        }

        public string CreateMap(List<(int x, int y)> list)
        {
            var dangerPoints = new Dictionary<(int, int), int>();
            var symbolMap = list
                .Zip(
                    Enumerable.Range(0, list.Count)
                        .Select(n => (char)(('A' + n))),
                    (coord, letter) => (coord, letter))
                .ToDictionary(c => c.coord, v => v.letter);
            foreach (var dangerpoint in list)
            {
                dangerPoints[dangerpoint] = 0;
            }

            var dimensions = this.CalculateMapDimensions(list);
            char[,] mapArray = new char[dimensions.maxX - dimensions.minX + 3, dimensions.maxY - dimensions.minY + 3];

            var xOffset = dimensions.minX - 1;
            var yOffset = dimensions.minY - 1;

            foreach (var coord in EnumerateCoordinates(dimensions))
            {
                (var tie, var closetPoint) = GetClosestPoint(dangerPoints, coord);

                if (tie)
                {
                    mapArray[coord.x - xOffset, coord.y - yOffset] = '.';
                }
                else if (coord.x == closetPoint.x && coord.y == closetPoint.y)
                {
                    mapArray[coord.x - xOffset, coord.y - yOffset] = symbolMap[closetPoint];
                }
                else
                {
                    mapArray[coord.x - xOffset, coord.y - yOffset] = char.ToLower(symbolMap[closetPoint]);
                }
            }

            var lineSb = new StringBuilder();

            for (int y = 0; y < mapArray.GetLength(1); y++)
            {
                var sb = new StringBuilder();
                for (int x = 0; x < mapArray.GetLength(0); x++)
                {
                    sb.Append(mapArray[x, y]);
                }
                lineSb.Append(sb.ToString());
                lineSb.Append(Environment.NewLine);
            }

            return lineSb.ToString();
        }

        public string PartTwo(string input)
        {
            var points = ToCoordinates(input);
            return EnumerateCoordinates(CalculateMapDimensions(points)).Count(coord => points.Sum(p => ManhattenDistance(p, coord)) < this.partTwoMinDist).ToString();
        }

        public IEnumerable<(int x, int y)> EnumerateBorder((int minX, int maxX, int minY, int maxY) dimensions)
        {
            // Top row
            for (int i = 0; i < (dimensions.maxX - dimensions.minX) + 2; i++)
            {
                yield return (dimensions.minX - 1 + i, dimensions.minY - 1);
            }
            // Right most row
            for (int i = 0; i < (dimensions.maxY - dimensions.minY) + 2; i++)
            {
                yield return (dimensions.maxX + 1, dimensions.minY - 1 + i);
            }
            // Bottom row
            for (int i = 0; i < (dimensions.maxX - dimensions.minX) + 2; i++)
            {
                yield return (dimensions.maxX + 1 - i, dimensions.maxY + 1);
            }
            // Left most row
            for (int i = 0; i < (dimensions.maxY - dimensions.minY) + 2; i++)
            {
                yield return (dimensions.minX - 1, dimensions.maxY + 1 - i);
            }
        }

        public IEnumerable<(int x, int y)> EnumerateCoordinates((int minX, int maxX, int minY, int maxY) dimensions)
        {
            for (int i = 0; i < dimensions.maxY - dimensions.minY + 3; i++)
            {
                for (int j = 0; j < dimensions.maxX - dimensions.minX + 3; j++)
                {
                    yield return (dimensions.minX - 1 + j, dimensions.minY - 1 + i);
                }
            }
        }

        public List<(int x, int y)> ToCoordinates(string input)
        {
            return input.Split('\n').Select(s => s.Replace("\r", "")).Where(s => string.IsNullOrWhiteSpace(s) == false).Select(s =>
            {
                var split = s.Split(',');
                return (int.Parse(split[0]), int.Parse(split[1]));
            }).ToList();
        }

        public int ManhattenDistance((int, int) pointA, (int, int) pointB)
        {
            return Math.Abs(pointA.Item1 - pointB.Item1) + Math.Abs(pointA.Item2 - pointB.Item2);
        }

        public (int minX, int maxX, int minY, int maxY) CalculateMapDimensions(List<(int x, int y)> coordinates)
        {
            var minX = int.MaxValue;
            var maxX = int.MinValue;

            var minY = int.MaxValue;
            var maxY = int.MinValue;

            foreach (var coordinate in coordinates)
            {
                minX = Math.Min(coordinate.x, minX);
                maxX = Math.Max(coordinate.x, maxX);
                minY = Math.Min(coordinate.y, minY);
                maxY = Math.Max(coordinate.y, maxY);
            }

            return (minX, maxX, minY, maxY);
        }

    }
}
