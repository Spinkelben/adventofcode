using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Test
{
    [TestClass]
    public class DayThirteenTests
    {
        private readonly string partOneTestInput =
@"
/->-\        
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/   
";
        private readonly string partTwoTestInput =
@"
/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/  
";

        [TestMethod]
        public void ParseMethodTest()
        {
            var solver = new DayThriteen(true);
            var resultTrack = new List<string>() {
                @"/---\        ",
                @"|   |  /----\",
                @"| /-+--+-\  |",
                @"| | |  | |  |",
                @"\-+-/  \-+--/",
                @"  \------/   ",
            };
            var cartMap = new Dictionary<(int, int), DayThriteen.MineCart>();
            cartMap[(0, 2)] = new DayThriteen.MineCart(0, 2, '>');
            cartMap[(3, 9)] = new DayThriteen.MineCart(3, 9, 'v');
            var result = solver.ParseInput(partOneTestInput);
            Assert.IsTrue(resultTrack.SequenceEqual(result.track));
            Assert.AreEqual(13, result.width);
            Assert.AreEqual(6, result.height);
        }

        [TestMethod]
        public void PartOneTest()
        {
            var solver = new DayThriteen(true);
            Assert.AreEqual("7,3", solver.PartOne(partOneTestInput));
        }

        [TestMethod]
        public void PartTwoTest()
        {
            var solver = new DayThriteen(true);
            Assert.AreEqual("6,4", solver.PartTwo(partTwoTestInput));
        }
    }
}
