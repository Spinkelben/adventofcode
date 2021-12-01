using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode
{
    public class DayEleven : IDaySolution
    {
        public string PartOne(string input)
        {
            var grid = InitGrid(input);

            var coords = ExamineSubGrid(grid, 3);

            return (coords.x, coords.y).ToString();
        }

        private int[,] InitGrid(string input)
        {
            var grid = new int[300, 300];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = GetPowerLevel((j + 1, i + 1), input);
                }
            }
            return grid;
        }

        private (int x, int y, int power) ExamineSubGrid(int[,] grid, int size)
        {
            var maxSum = int.MinValue;
            var coords = (x: -1, y: -1);

            for (int i = 0; i < grid.GetLength(0) - size; i++)
            {
                for (int j = 0; j < grid.GetLength(1) - size; j++)
                {
                    var subPower = GetSubGridPower((j, i), grid, size);
                    if (subPower > maxSum)
                    {
                        maxSum = subPower;
                        coords = (j + 1, i + 1);
                    }
                }
            }

            return (coords.x, coords.y, maxSum);
        }

        public string PartTwo(string input)
        {
            var grid = InitGrid(input);

            var result = (x: -1, y: -1, subGridSize: -1);
            var maxPower = int.MinValue;
            for (int i = 1; i <= 300; i++)
            {
                var (x, y, power) = ExamineSubGrid(grid, i);

                if (power > maxPower)
                {
                    maxPower = power;
                    result = (x, y, i);
                }

                if (power < -200)
                {
                    break;
                }
            }

            return result.ToString();
        }

        public int GetSubGridPower((int x, int y) coord, int[,] grid, int size)
        {
            var sum = 0;
            for (int i = coord.y; i < coord.y + size; i++)
            {
                for (int j = coord.x; j < coord.x + size; j++)
                {
                    sum += grid[i, j];
                }
            }
            return sum;
        }

        public int GetPowerLevel((int x, int y) cell, string serial)
        {
            var serialNumber = int.Parse(serial.Trim());
            var rackId = cell.x + 10;
            var bigNum = (((rackId * cell.y) + serialNumber) * rackId);
            if (bigNum < 100)
            {
                return -5;
            }
            var str = bigNum.ToString();
            return int.Parse($"{str[str.Length - 3]}") - 5;
            
        }
    }
}
