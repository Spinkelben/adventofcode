using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Test
{
    [TestClass]
    public class DayElevenTests
    {
        [TestMethod]
        public void PowerLevelTest()
        {
            var solver = new DayEleven();
            Assert.AreEqual(4, solver.GetPowerLevel((3, 5), "8"));
            Assert.AreEqual(-5, solver.GetPowerLevel((122, 79), "57"));
            Assert.AreEqual(0, solver.GetPowerLevel((217, 196), "39"));
            Assert.AreEqual(4, solver.GetPowerLevel((101, 153), "71"));
        }

        [TestMethod]
        public void PartOneTest()
        {
            var solver = new DayEleven();
            Assert.AreEqual("(33, 45)", solver.PartOne("18"));
            Assert.AreEqual("(21, 61)", solver.PartOne("42"));
        }
        [TestMethod]
        public void PartTwoTest()
        {
            var solver = new DayEleven();
            Assert.AreEqual("(90, 269, 16)", solver.PartTwo("18"));
            Assert.AreEqual("(232, 251, 12)", solver.PartTwo("42"));
        }


    }
}
