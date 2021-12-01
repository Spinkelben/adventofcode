using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Test
{
    [TestClass]
    public class DayEightTests
    {
        private readonly string testInput = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";
        [TestMethod]
        public void PartOneTest()
        {
            var solver = new DayEight();
            Assert.AreEqual("138", solver.PartOne(testInput));
        }

        [TestMethod]
        public void PartTwoTest()
        {
            var solver = new DayEight();
            Assert.AreEqual("66", solver.PartTwo(testInput));
        }
    }
}
