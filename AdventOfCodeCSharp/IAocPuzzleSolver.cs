using System.Collections.Generic;

namespace AdventOfCodeCSharp
{
    internal interface IAocPuzzleSolver
    {
        string Part1(IList<string> input);

        string Part2(IList<string> input);
    }
}