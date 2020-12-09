using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 8)]
    internal class Year2020Day8 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var interpreter = new Interpreter(input);
            interpreter.Execute();
            return $"{interpreter.Accumulator}";
        }

        public string Part2(IList<string> input)
        {
            foreach (var fixedProgram in GeneratePrograms(input.Where(l => l.Length > 0).ToList()))
            {
                var run = TestProgram(fixedProgram);
                if (run.Halted)
                {
                    return $"{run.Accumulator}";
                }

            }

            return "No fix found";
        }

        private IEnumerable<List<string>> GeneratePrograms(IList<string> brokenProgram)
        {
            for (int i = brokenProgram.Count - 1; i >= 0; i--)
            {
                var line = brokenProgram[i];
                var cmd = line.Split(' ')[0];
                var arg = line.Split(' ')[1];
                if (cmd == "jmp")
                {
                    cmd = "nop";
                }
                else if (cmd == "nop")
                {
                    cmd = "jmp";
                }

                var copy = new List<string>(brokenProgram);
                copy[i] = $"{cmd} {arg}";
                yield return copy;
            }
        }

        private Interpreter TestProgram(IList<string> program)
        {
            var interpreter = new Interpreter(program);
            interpreter.Execute();
            return interpreter;
        }
    }

    internal class Interpreter
    {
        private readonly IList<string> program;
        private int programCounter = 0;
        private long accumulator = 0;
        private bool halted = false;

        internal Interpreter(IList<string> program)
        {
            this.program = program;
        }

        internal int ProgramCounter => programCounter;
        internal long Accumulator => accumulator;
        internal bool Halted => halted;

        internal void Step()
        {
            var (cmd, arg) = GetInstruction(this.programCounter);
            switch (cmd)
            {
                case "acc":
                    accumulator += arg;
                    programCounter++;
                    break;
                case "jmp":
                    programCounter += arg;
                    break;
                case "nop":
                    programCounter++;
                    break;
                case "hlt":
                    halted = true;
                    break;
                default:
                    break;
            }
        }

        internal void Execute()
        {
            var executedOperations = new HashSet<int>();
            while (!executedOperations.Contains(programCounter) && !Halted)
            {
                executedOperations.Add(programCounter);
                Step();
            }
        }

        internal void Reset()
        {
            accumulator = 0;
            programCounter = 0;
        }

        private (string cmd, int arg) GetInstruction(int programCounter)
        {
            if (programCounter >= program.Count)
            {
                return ("hlt", 0);
            }

            var segments = this.program[programCounter].Split(' ');
            return (segments[0], int.Parse(segments[1]));
        }
    }
}
