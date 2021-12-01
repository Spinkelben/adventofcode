using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Test
{
    [TestClass]
    public class DaySixTests
    {
        private readonly string testInput =
@"1, 1
1, 6
8, 3
3, 4
5, 5
8, 9
";

        [TestMethod]
        public void PartOneTest()
        {
            var solver = new DaySix();
            Assert.AreEqual("17", solver.PartOne(testInput));
        }

        [TestMethod]
        public void HelpersTest()
        {
            var solver = new DaySix();
            Assert.AreEqual((1, 8, 1, 9), solver.CalculateMapDimensions(solver.ToCoordinates(testInput)));
        }

        [TestMethod]
        public void ProduceMapTest()
        {
            var solver = new DaySix();
            var expectedMap = @"aaaaa.cccc
aAaaa.cccc
aaaddecccc
aadddeccCc
..dDdeeccc
bb.deEeecc
bBb.eeee..
bbb.eeefff
bbb.eeffff
bbb.ffffFf
bbb.ffffff
";

            var actualMap = solver.CreateMap(solver.ToCoordinates(testInput));
            Assert.AreEqual(expectedMap, actualMap);
        }

        [TestMethod]
        public void SpotCheckMapTest()
        {
            var solver = new DaySix();

            var coords = solver.ToCoordinates(testInput);

            Assert.AreEqual((false, (5, 5)), solver.GetClosestPoint(coords.ToDictionary(k => k, v => 0), (5, 2)));
        }

        [TestMethod]
        public void BorderTestTest()
        {
            var solver = new DaySix();
            var dimensions = solver.CalculateMapDimensions(solver.ToCoordinates(testInput));
            var testBorder = new List<(int x, int y)>()
            {
                (0, 0),
                (1, 0),
                (2, 0),
                (3, 0),
                (4, 0),
                (5, 0),
                (6, 0),
                (7, 0),
                (8, 0),
                (9, 0),
                (9, 1),
                (9, 2),
                (9, 3),
                (9, 4),
                (9, 5),
                (9, 6),
                (9, 7),
                (9, 8),
                (9, 9),
                (9, 10),
                (8, 10),
                (7, 10),
                (6, 10),
                (5, 10),
                (4, 10),
                (3, 10),
                (2, 10),
                (1, 10),
                (0, 10),
                (0, 9),
                (0, 8),
                (0, 7),
                (0, 6),
                (0, 5),
                (0, 4),
                (0, 3),
                (0, 2),
                (0, 1),
            };
            Assert.IsTrue(testBorder.SequenceEqual(solver.EnumerateBorder(dimensions).ToList()));
            Assert.AreEqual(testBorder[0], solver.EnumerateCoordinates(dimensions).First());
            Assert.AreEqual((9, 10), solver.EnumerateCoordinates(dimensions).Last());
        }

        [TestMethod]
        public void PartTwoTest()
        {
            var solver = new DaySix(32);
            Assert.AreEqual("16", solver.PartTwo(testInput));

        }
    }
}
