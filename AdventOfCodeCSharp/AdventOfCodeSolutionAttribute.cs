using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeCSharp
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal class AdventOfCodeSolutionAttribute : Attribute
    {
        internal AdventOfCodeSolutionAttribute(int year, int day)
        {
            Year = year;
            Day = day;
        }

        public int Year { get; }
        public int Day { get; }
    }
}
