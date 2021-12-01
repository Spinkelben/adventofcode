using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Test
{
    [TestClass]
    public class DayTwelveTests
    {
        private readonly string input =
@"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #
";

        [TestMethod]
        public void PartOneTest()
        {
            var solver = new DayTwelve();
            Assert.AreEqual("325", solver.PartOne(input));
        }

        [TestMethod]
        public void PartTwoTest()
        {
            var solver = new DayTwelve();
            Assert.AreEqual("999999999374", solver.PartTwo(input));
        }
    }
}
