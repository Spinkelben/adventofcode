using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode;

namespace AdventOfCode.Test
{
    [TestClass]
    public class DayFiveTests
    {
        private readonly string sampleInput = "dabAcCaCBAcCcaDA";

        [TestMethod]
        public void PartOneTest()
        {
            var solver = new DayFive();
            var result = solver.PartOne(sampleInput);
            Assert.AreEqual("Polymer: dabCBAcaDA, Result: 10", result);
        }

        [TestMethod]
        public void PartTwoTest()
        {
            var solver = new DayFive();
            var result = solver.PartTwo(sampleInput);
            Assert.AreEqual("Removed c. Polymer: daDA, Result: 4", result);
        }

        [TestMethod]
        public void PartOneSimpleExamplesTest()
        {
            var solver = new DayFive();
            Assert.AreEqual("", solver.ReduceOnce("aA"));
            Assert.AreEqual("aA", solver.ReduceOnce("abBA"));
            Assert.AreEqual("abAB", solver.ReduceOnce("abAB"));
            Assert.AreEqual("aabAAB", solver.ReduceOnce("aabAAB"));
        }

        [TestMethod]
        public void PartTwoSimpleExamplesTest()
        {
            var solver = new DayFive();
            Assert.AreEqual("dbcCCBcCcD", solver.RemoveElement("dabAcCaCBAcCcaDA", 'a'));
            Assert.AreEqual("daAcCaCAcCcaDA", solver.RemoveElement("dabAcCaCBAcCcaDA", 'b'));
            Assert.AreEqual("dabAaBAaDA", solver.RemoveElement("dabAcCaCBAcCcaDA", 'c'));
            Assert.AreEqual("abAcCaCBAcCcaA", solver.RemoveElement("dabAcCaCBAcCcaDA", 'd'));
        }

        [TestMethod]
        public void MoreExamplesTest()
        {
            var solver = new DayFive();
            Assert.AreEqual("dabAaCBAcaDA", solver.ReduceOnce("dabAcCaCBAcCcaDA"));
            Assert.AreEqual("dabCBAcaDA", solver.ReduceOnce("dabAaCBAcaDA"));
            Assert.AreEqual("dabCBAcaDA", solver.ReduceOnce("dabCBAcaDA"));
            Assert.AreEqual("dabCBAcaDA", solver.ReduceOnce("dabCBAcaDA"));
        }
    }
}
