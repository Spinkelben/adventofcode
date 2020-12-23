using AdventOfCodeCSharp.Year2020;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCodeCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please provide year and date, e.g '2020 1'");
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
            

            var input = InputFetcher.getPuzzleInput(day, year, token, forceDownloadInput, false).ToList();
            var puzzleMap = GetPuzzleMap();
            Console.WriteLine($"Running Year {year} day {day}");
            var solver = puzzleMap[(int.Parse(year), int.Parse(day))];

            var stopWatch = Stopwatch.StartNew();
            var part1 = solver.Part1(input);
            var part1Time = stopWatch.Elapsed;
            var part2 = solver.Part2(input);
            stopWatch.Stop();

            Console.WriteLine($"Part 1: {part1}");
            Console.WriteLine($"Part 2: {part2}");
            Console.WriteLine($"Part 1 time: {part1Time.TotalMilliseconds} ms");
            Console.WriteLine($"Part 2 time: {(stopWatch.Elapsed - part1Time).TotalMilliseconds} ms");
        }

        private static Dictionary<(int, int), IAocPuzzleSolver> GetPuzzleMap()
        {
            var assembly = typeof(Program).Assembly;
            var map = new Dictionary<(int, int), IAocPuzzleSolver>();
            foreach (var type in assembly
                .GetTypes()
                .Where(t => typeof(IAocPuzzleSolver).IsAssignableFrom(t)))
            {
                var attributes = type.GetCustomAttributes(typeof(AdventOfCodeSolutionAttribute), false)
                    .Cast<AdventOfCodeSolutionAttribute>();
                if (attributes.FirstOrDefault() is AdventOfCodeSolutionAttribute attribute)
                {
                    map[(attribute.Year, attribute.Day)] = (IAocPuzzleSolver)Activator.CreateInstance(type);
                }
            }
            return map;
        }
    }
}
