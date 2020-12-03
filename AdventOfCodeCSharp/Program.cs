using AdventOfCodeCSharp.Year2020;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeCSharp
{
    class Program
    {
        private static Dictionary<(int, int), IAocPuzzleSolver> puzzleMap = new Dictionary<(int, int), IAocPuzzleSolver>()
        {
            {(2020, 1), new Year2020Day1() },
            {(2020, 2), new Year2020Day2() },
            {(2020, 3), new Year2020Day3() },
        };

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please provide year and data, e.g '2020 1'");
                return;
            }
            
            var year = args[0];
            var day = args[1];
            var forceDownloadInput = args.Length > 2 ? bool.Parse(args[2]) : false;
            var token = (new ConfigurationBuilder())
                .AddUserSecrets<Program>()
                .Build()
                .GetSection("Auth")
                .GetSection("SessionToken").Value;
            

            var input = InputFetcher.getPuzzleInput(day, year, token, forceDownloadInput).ToList();
            Console.WriteLine($"Running Year {year} day {day}");
            var solver = puzzleMap[(int.Parse(year), int.Parse(day))];

            var part1 = solver.Part1(input);
            var part2 = solver.Part2(input);

            Console.WriteLine($"Part 1: {part1}");
            Console.WriteLine($"Part 2: {part2}");
        }
    }
}
