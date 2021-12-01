using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        private static readonly Dictionary<int, IDaySolution> solutions = new Dictionary<int, IDaySolution>()
        {
            {1, new DayOne() },
            {2, new DayTwo() },
            {3, new DayThree() },
            {4, new DayFour() },
            {5, new DayFive() },
            {6, new DaySix() },
            {7, new DaySeven() },
            {8, new DayEight() },
            {9, new DayNine() },
            {10, new DayTen() },
            {11, new DayEleven() },
            {12, new DayTwelve() },
            {13, new DayThriteen() },
        };

        static async Task Main(string[] args)
        {
            var argDict = ParseArgs(args);
            var inputRetriever = new PuzzleInput(argDict["sessionKey"]);

            if (argDict.ContainsKey("day"))
            {
                var input = await inputRetriever.GetPuzzleInput(argDict["day"]);
                Console.WriteLine($"Day {argDict["day"]} Part 1 {solutions[int.Parse(argDict["day"])].PartOne(input)}");
                Console.WriteLine($"Day {argDict["day"]} Part 2 {solutions[int.Parse(argDict["day"])].PartTwo(input)}");
                return;
            }
            while (true)
            {
                Console.Write($"Please enter day (1 - {solutions.Count}): ");
                var day = int.Parse(Console.ReadLine());
                if (day < 1 || day > solutions.Count)
                {
                    return;
                }
                var input = await inputRetriever.GetPuzzleInput($"{day}");
                Console.WriteLine($"Day {day} Part 1 {solutions[day].PartOne(input)}");
                Console.WriteLine($"Day {day} Part 2 {solutions[day].PartTwo(input)}");
            }

            
        }

        private static Dictionary<string, string> ParseArgs(string[] args)
        {
            var parsedArgs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var listArgs = args.Select(a => a.ToLowerInvariant().TrimStart('-')).ToList();
            if (!parsedArgs.TryAddArgs(listArgs, "sessionkey"))
            {
                Console.Write("Please enter session key: ");
                var sessionKey = Console.ReadLine();
                parsedArgs["sessionkey"] = sessionKey;
            }

            parsedArgs.TryAddArgs(listArgs, "day");

            return parsedArgs;
        }

        
    }
}
