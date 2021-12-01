using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Test
{
    [TestClass]
    public class DaySevenTests
    {
        private readonly string testInput =
@"Step C must be finished before step A can begin.
Step C must be finished before step F can begin.
Step A must be finished before step B can begin.
Step A must be finished before step D can begin.
Step B must be finished before step E can begin.
Step D must be finished before step E can begin.
Step F must be finished before step E can begin.
";

        [TestMethod]
        public void PartOneTest()
        {
            var solver = new DaySeven();
            Assert.AreEqual("CABDFE", solver.PartOne(testInput));
        }

        [TestMethod]
        public void PartTwoTest()
        {
            var solver = new DaySeven(2, 0);
            Assert.AreEqual("15", solver.PartTwo(testInput));
        }
    }
}
