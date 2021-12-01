using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Test
{
    [TestClass]
    public class DayNineTests
    {
        private readonly List<string> examples = new List<string>()
        {
            "10 players; last marble is worth 1618 points",
            "13 players; last marble is worth 7999 points",
            "17 players; last marble is worth 1104 points",
            "21 players; last marble is worth 6111 points",
            "30 players; last marble is worth 5807 points",
            "9 players; last marble is worth 25 points",
        };

        [TestMethod]
        public void PartOneTest()
        {
            var solver = new DayNine();
            var results = new List<string>()
            {
                "8317",
                "146373",
                "2764",
                "54718",
                "37305",
                "32",
            };


            foreach (var (input, result) in examples.Zip(results, (e, r) => (e, r)))
            {
                Assert.AreEqual(result, solver.PartOne(input));
            }
        }

        //[TestMethod]
        //public void PartTwoTest()
        //{
        //    var solver = new DayNine();
        //    var results = new List<string>()
        //    {
        //        "",
        //        "",
        //        "",
        //        "",
        //        "",
        //        "",
        //    };

        //    foreach (var (input, result) in examples.Zip(results, (e, r) => (e, r)))
        //    {
        //        Assert.AreEqual(result, solver.PartTwo(input));
        //    }
        //}
    }
}
