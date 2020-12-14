using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 14)]
    internal class Year2020Day14 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var instructions = ParseInput(input.Where(i => i.Length > 0).ToList());
            var memory = LoadValues(instructions);
            var sum = memory.Sum(kvp => kvp.Value);
            return $"{sum}";
        }

        public string Part2(IList<string> input)
        {
            var instructions = ParseInput(input.Where(i => i.Length > 0).ToList());
            var memory = LoadValues2(instructions);
            var sum = memory.Sum(kvp => kvp.Value);
            return $"{sum}";
        }

        private Dictionary<long, long> LoadValues2(IEnumerable<(Instruction, string, string)> instructions)
        {
            var result = new Dictionary<long, long>();

            char[] mask = null;

            foreach (var (instruction, arg1, arg2) in instructions)
            {
                switch (instruction)
                {
                    case Instruction.Mask:
                        mask = arg1.ToCharArray();
                        break;
                    case Instruction.Value:
                        var binaryString = Convert.ToString(int.Parse(arg1), 2); 
                        foreach (Queue<char> address in GetAddresses(mask, binaryString.ToCharArray()))
                        {
                            var intAddress = Convert.ToInt64(address.Aggregate("", (acc, c) => acc + c), 2);
                            result[intAddress] = int.Parse(arg2);
                        }

                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        private IEnumerable<Queue<char>> GetAddresses(char[] mask, char[] arg1)
        {
            IEnumerable<Queue<char>> remaining;
            if (arg1.Length > 1)
            {
                remaining = GetAddresses(mask[0..^1], arg1[0..^1]);
            }
            else if (mask.Length > 1)
            {
                remaining = GetAddresses(mask[0..^1], new char[] { });
            }
            else
            {
                remaining = new List<Queue<char>>() { new Queue<char>() };
            }
            

            switch (mask[^1])
            {
                case '1':
                    foreach (var item in remaining)
                    {
                        item.Enqueue('1');
                        yield return item;
                    }
                    break;
                case 'X':
                    foreach (var item in remaining)
                    {
                        var copy = new Queue<char>(item);
                        item.Enqueue('1');
                        yield return item;
                        copy.Enqueue('0');
                        yield return copy;
                    }
                    break;
                case '0':
                    foreach (var item in remaining)
                    {
                        if (arg1.Length > 0)
                        {
                            item.Enqueue(arg1[^1]);
                        }
                        else
                        {
                            item.Enqueue('0');
                        }

                        yield return item;
                    }
                    break;
                default:
                    break;
            }
        }

        internal Dictionary<int, long> LoadValues(IEnumerable<(Instruction, string, string)> instructions)
        {
            var memory = new Dictionary<int, long>();
            long zeroMask = long.MaxValue;
            long oneMask = 0;
            foreach (var (instruction, arg1, arg2) in instructions)
            {
                switch (instruction)
                {
                    case Instruction.Mask:
                        SetMasks(out zeroMask, out oneMask, arg1);
                        break;
                    case Instruction.Value:
                        var resultValue = long.Parse(arg2);
                        resultValue = MaskValue(zeroMask, oneMask, resultValue);
                        memory[int.Parse(arg1)] = resultValue;
                        break;
                    default:
                        break;
                }
            }


            return memory;
        }

        private static long MaskValue(long zeroMask, long oneMask, long resultValue)
        {
            resultValue |= oneMask;
            resultValue &= zeroMask;
            return resultValue;
        }

        private static void SetMasks(out long zeroMask, out long oneMask, string arg1)
        {
            var oneComponents = new List<byte>();
            var zeroComponents = new List<byte>();
            for (byte i = 0; i < arg1.Length; i++)
            {
                switch (arg1[i])
                {
                    case '0':
                        zeroComponents.Add((byte)(35 - i));
                        break;
                    case '1':
                        oneComponents.Add((byte)(35 - i));
                        break;
                    case 'X':
                    default:
                        break;
                }
            }

            oneMask = 0L;
            var oneBits = oneComponents.Select(c => 1L << c).Sum();
            oneMask += oneBits;

            zeroMask = long.MaxValue & 0b1111_1111_1111_1111_1111_1111_1111_1111_1111;
            var zeroBits = zeroComponents.Select(c => 1L << c).Sum();
            zeroMask -= zeroBits;
        }

        internal IEnumerable<(Instruction, string, string)> ParseInput(IList<string> input)
        {
            var memoryMatcher = new Regex(@"mem\[(?<address>\d+)\] = (?<value>\d+)", RegexOptions.Compiled);
            foreach (var line in input)
            {
                if (line.StartsWith("mask = "))
                {
                    var mask = line["mask = ".Length..];
                    yield return (Instruction.Mask, mask, null);
                }
                else
                {
                    var match = memoryMatcher.Match(line);
                    yield return (Instruction.Value, match.Groups["address"].Value, match.Groups["value"].Value);
                }
            }
        }

        internal enum Instruction
        {
            Mask,
            Value
        }

    }
}
