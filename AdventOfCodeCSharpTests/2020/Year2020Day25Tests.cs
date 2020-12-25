using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day25Tests
    { 
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> directions, int numBlackTiles)
        {
            var puzzleSolver = new Year2020Day25();
            Assert.Equal(numBlackTiles.ToString(), puzzleSolver.Part1(directions));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> directions, string numBlackTiles)
        {
            var puzzleSolver = new Year2020Day25();
            Assert.Equal(numBlackTiles.ToString(), puzzleSolver.Part2(directions));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "5764801",
                    "17807724",
                },
                14897079,
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "sesenwnenenewseeswwswswwnenewsewsw   ",
                    "neeenesenwnwwswnenewnwwsewnenwseswesw",
                    "seswneswswsenwwnwse                  ",
                    "nwnwneseeswswnenewneswwnewseswneseene",
                    "swweswneswnenwsewnwneneseenw         ",
                    "eesenwseswswnenwswnwnwsewwnwsene     ",
                    "sewnenenenesenwsewnenwwwse           ",
                    "wenwwweseeeweswwwnwwe                ",
                    "wsweesenenewnwwnwsenewsenwwsesesenwne",
                    "neeswseenwwswnwswswnw                ",
                    "nenwswwsewswnenenewsenwsenwnesesenew ",
                    "enewnwewneswsewnwswenweswnenwsenwsw  ",
                    "sweneswneswneneenwnewenewwneswswnese ",
                    "swwesenesewenwneswnwwneseswwne       ",
                    "enesenwswwswneneswsenwnewswseenwsese ",
                    "wnwnesenesenenwwnenwsewesewsesesew   ",
                    "nenewswnwewswnenesenwnesewesw        ",
                    "eneswnwswnwsenenwnwnwwseeswneewsenese",
                    "neswnwewnwnwseenwseesewsenwsweewe    ",
                    "wseweeenwnesenwwwswnew               ",
                },
                2208,
            };
        }
    }
}