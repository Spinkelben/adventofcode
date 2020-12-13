using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 13)]
    internal class Year2020Day13 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var seaPortArriveTime = int.Parse(input[0]);
            var departureTime = seaPortArriveTime;
            var buses = input[1]
                .Split(',')
                .Select(b => 
                { 
                    var success = int.TryParse(b, out int result);
                    return (success, result);
                })
                .Where(r => r.success)
                .Select(r => r.result)
                .ToList();

            var departingBus = -1;
            while (true)
            {
                departingBus = buses.FirstOrDefault(b => departureTime % b == 0);
                if (departingBus != 0)
                {
                    break;
                }

                departureTime++;
            }

            return $"{(departureTime - seaPortArriveTime) * departingBus}";
        }

        public string Part2(IList<string> input)
        {
            var busses = input[1]
                .Split(',')
                .Select(b =>
                {
                    if (int.TryParse(b, out int result))
                    {
                        return result;
                    }
                    else
                    {
                        return (int?)null;
                    }
                })
                .ToList();

            // I owe this solution to redditor "passwordsniffer" https://www.reddit.com/r/adventofcode/comments/kc4njx/2020_day_13_solutions/gfo4b1z/
            // I eventually got frustrated enough that I just looked up other people solutions, this is the 
            // first I understood well enough to try and implement.
            BigInteger timestamp = BigInteger.Zero;
            var jumpSize = new BigInteger(busses.First().Value);
            for (int i = 1; i < busses.Count; i++)
            {
                if (busses[i] is null)
                {
                    continue;
                }

                while ((timestamp + i) % busses[i] != 0)
                {
                    timestamp += jumpSize;
                }

                jumpSize *= busses[i].Value;
            }


            return $"{timestamp}";
        }

        private bool CheckTimeStamp(ulong timestamp, List<int?> constraint)
        {
            for (int i = 0; i < constraint.Count; i++)
            {
                if (constraint[i] is int bus)
                {
                    if ((timestamp + (ulong)i) % (ulong)bus != 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private int GCD(int a, int b)
        {
            int c, d;
            if (a > b)
            {
                c = b;
                d = a;
            }
            else
            {
                c = a;
                d = b;
            }

            while (d != 0)
            {
                var temp = d;
                d = c % d;
                c = temp;
            }
            return c;
        }

        private int LCM(int a, int b)
        {
            return (a / GCD(a, b)) * b;
        }

        private int LCM(IEnumerable<int> numbers)
        {
            var result = 1;
            foreach (var item in numbers)
            {
                result = LCM(result, item);
            }

            return result;
        }

    }

    
}
