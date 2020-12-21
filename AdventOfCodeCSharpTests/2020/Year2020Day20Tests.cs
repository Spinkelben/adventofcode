using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day20Tests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(List<string> tiles, long cornerIdProduct)
        {
            var puzzleSolver = new Year2020Day20();
            Assert.Equal(cornerIdProduct.ToString(), puzzleSolver.Part1(tiles));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> expression, long numMatches)
        {
            var puzzleSolver = new Year2020Day20();
            Assert.Equal(numMatches.ToString(), puzzleSolver.Part2(expression));
        }

        [Fact]
        public void TileRotations()
        {
            var lines = new List<string>()
            {
                "Tile 2311:",
                "..##.#..#.",
                "##..#.....",
                "#...##..#.",
                "####.#...#",
                "##.##.###.",
                "##...#.###",
                ".#.#.#..##",
                "..#....#..",
                "###...#.#.",
                "..###..###",
                "          ",
            };

            var puzzleSolver = new Year2020Day20();
            var tiles = puzzleSolver.ParseInput(lines);
            Assert.Single(tiles);
            var tile = tiles.First();
            Assert.Equal("..##.#..#.", tile.GetEdge(0));
            Assert.Equal("...#.##..#", tile.GetEdge(1));
            Assert.Equal("..###..###", tile.GetEdge(2));
            Assert.Equal(".#####..#.", tile.GetEdge(3));

            tile.Rotation++;
            Assert.Equal("..##.#..#.", tile.GetEdge(1));
            Assert.Equal("#..##.#...", tile.GetEdge(2));
            Assert.Equal("..###..###", tile.GetEdge(3));
            Assert.Equal(".#..#####.", tile.GetEdge(0));

            tile.Rotation++;
            Assert.Equal("###..###..", tile.GetEdge(0));
            Assert.Equal(".#..#####.", tile.GetEdge(1));
            Assert.Equal(".#..#.##..", tile.GetEdge(2));
            Assert.Equal("#..##.#...", tile.GetEdge(3));

            tile.Rotation++;
            Assert.Equal("...#.##..#", tile.GetEdge(0));
            Assert.Equal("###..###..", tile.GetEdge(1));
            Assert.Equal(".#####..#.", tile.GetEdge(2));
            Assert.Equal(".#..#.##..", tile.GetEdge(3));

            tile.Rotation++;
            Assert.Equal("..##.#..#.", tile.GetEdge(0));
            Assert.Equal("...#.##..#", tile.GetEdge(1));
            Assert.Equal("..###..###", tile.GetEdge(2));
            Assert.Equal(".#####..#.", tile.GetEdge(3));
        }

        [Fact]
        public void TileFlips()
        {
            var lines = new List<string>()
            {
                "Tile 2311:",
                "..##.#..#.",
                "##..#.....",
                "#...##..#.",
                "####.#...#",
                "##.##.###.",
                "##...#.###",
                ".#.#.#..##",
                "..#....#..",
                "###...#.#.",
                "..###..###",
                "          ",
            };
            var puzzleSolver = new Year2020Day20();
            var tiles = puzzleSolver.ParseInput(lines);
            Assert.Single(tiles);
            var tile = tiles.First();

            tile.FlipX = true;
            Assert.Equal(".#..#.##..", tile.GetEdge(0));
            Assert.Equal("###..###..", tile.GetEdge(2));
            Assert.Equal("...#.##..#", tile.GetEdge(1));
            Assert.Equal(".#####..#.", tile.GetEdge(3));
            Assert.Equal(".....#..##", tile.GetPatternLine(1));

            tile.FlipX = false;
            tile.FlipY = true;

            Assert.Equal("..##.#..#.", tile.GetEdge(0));
            Assert.Equal("..###..###", tile.GetEdge(2));
            Assert.Equal("#..##.#...", tile.GetEdge(1));
            Assert.Equal(".#..#####.", tile.GetEdge(3));
            Assert.Equal(".#.####.#.", tile.GetPatternColumn(1));
        }

        [Fact]
        public void TileFlipAndRotate()
        {
            var lines = new List<string>()
            {
                "Tile 2311:",
                "..##.#..#.",
                "##..#.....",
                "#...##..#.",
                "####.#...#",
                "##.##.###.",
                "##...#.###",
                ".#.#.#..##",
                "..#....#..",
                "###...#.#.",
                "..###..###",
                "          ",
            };
            var puzzleSolver = new Year2020Day20();
            var tiles = puzzleSolver.ParseInput(lines);
            Assert.Single(tiles);
            var tile = tiles.First();

            // Rotate then flip
            tile.Rotation++;
            tile.FlipX = true;
            var topEdge = tile.GetEdge(0);
            var rightEdge = tile.GetEdge(1);
            var bottomEdge = tile.GetEdge(2);
            var leftEdge = tile.GetEdge(3);
            Assert.Equal(".#..#####.", topEdge);
            Assert.Equal(".#..#.##..", rightEdge);
            Assert.Equal("#..##.#...", bottomEdge);
            Assert.Equal("###..###..", leftEdge);

            // Undo
            tile.FlipX = false;
            tile.Rotation--;

            // Flip then rotate
            tile.FlipX = true;
            tile.Rotation++;
            Assert.Equal(topEdge, tile.GetEdge(0));
            Assert.Equal(rightEdge, tile.GetEdge(1));
            Assert.Equal(bottomEdge, tile.GetEdge(2));
            Assert.Equal(leftEdge, tile.GetEdge(3));
        }

        [Fact]
        public void TileRelativeFlipXAndRotate()
        {
            var lines = new List<string>()
            {
                "Tile 2311:",
                "..##.#..#.",
                "##..#.....",
                "#...##..#.",
                "####.#...#",
                "##.##.###.",
                "##...#.###",
                ".#.#.#..##",
                "..#....#..",
                "###...#.#.",
                "..###..###",
                "          ",
            };
            var puzzleSolver = new Year2020Day20();
            var tiles = puzzleSolver.ParseInput(lines);
            Assert.Single(tiles);
            var tile = tiles.First();

            tile.Rotation++;
            tile.FlipXRelative(true);
            tile.Rotation--;
            Assert.Equal("..##.#..#.", tile.GetEdge(0));
            Assert.Equal("#..##.#...", tile.GetEdge(1));
            Assert.Equal("..###..###", tile.GetEdge(2));
            Assert.Equal(".#..#####.", tile.GetEdge(3));
        }

        [Fact]
        public void TileRelativeFlipYAndRotate()
        {
            var lines = new List<string>()
            {
                "Tile 2311:",
                "..##.#..#.",
                "##..#.....",
                "#...##..#.",
                "####.#...#",
                "##.##.###.",
                "##...#.###",
                ".#.#.#..##",
                "..#....#..",
                "###...#.#.",
                "..###..###",
                "          ",
            };
            var puzzleSolver = new Year2020Day20();
            var tiles = puzzleSolver.ParseInput(lines);
            Assert.Single(tiles);
            var tile = tiles.First();

            tile.Rotation++;
            tile.FlipYRelative(true);
            tile.Rotation--;
            Assert.Equal(".#..#.##..", tile.GetEdge(0));
            Assert.Equal("...#.##..#", tile.GetEdge(1));
            Assert.Equal("###..###..", tile.GetEdge(2));
            Assert.Equal(".#####..#.", tile.GetEdge(3));
        }

        [Fact]
        public void RotateImage()
        {
            var puzzleSolver = new Year2020Day20();
            var lines = new List<string>()
            {
                "Tile 2311:",
                "..##.#..#.",
                "##..#.....",
                "#...##..#.",
                "####.#...#",
                "##.##.###.",
                "##...#.###",
                ".#.#.#..##",
                "..#....#..",
                "###...#.#.",
                "..###..###",
                "          ",
            };

            Assert.Equal(new List<string>()
            {
                "Tile 2311:",
                "..##.#..#.",
                "##..#.....",
                "#...##..#.",
                "####.#...#",
                "##.##.###.",
                "##...#.###",
                ".#.#.#..##",
                "..#....#..",
                "###...#.#.",
                "..###..###",
                "          ",
            },
            puzzleSolver.Rotate(lines, 0));

            Assert.Equal(new List<string>()
            {
                " .#..#####.T",
                " .#.####.#.i",
                " ###...#..#l",
                " #..#.##..#e",
                " #....#.##. ",
                " ...##.##.#2",
                " .#...#....3",
                " #.#.##....1",
                " ##.###.#.#1",
                " #..##.#...:",
            },
            puzzleSolver.Rotate(lines, 1));

            var rotate2 = puzzleSolver.Rotate(puzzleSolver.Rotate(lines, 1), 1);
            Assert.Equal(rotate2, puzzleSolver.Rotate(lines, 2));

            var rotate3 = puzzleSolver.Rotate(rotate2, 1);
            Assert.Equal(rotate3, puzzleSolver.Rotate(lines, 3));

        }

            public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "Tile 2311:",
                    "..##.#..#.",
                    "##..#.....",
                    "#...##..#.",
                    "####.#...#",
                    "##.##.###.",
                    "##...#.###",
                    ".#.#.#..##",
                    "..#....#..",
                    "###...#.#.",
                    "..###..###",
                    "          ",
                    "Tile 1951:",
                    "#.##...##.",
                    "#.####...#",
                    ".....#..##",
                    "#...######",
                    ".##.#....#",
                    ".###.#####",
                    "###.##.##.",
                    ".###....#.",
                    "..#.#..#.#",
                    "#...##.#..",
                    "          ",
                    "Tile 1171:",
                    "####...##.",
                    "#..##.#..#",
                    "##.#..#.#.",
                    ".###.####.",
                    "..###.####",
                    ".##....##.",
                    ".#...####.",
                    "#.##.####.",
                    "####..#...",
                    ".....##...",
                    "          ",
                    "Tile 1427:",
                    "###.##.#..",
                    ".#..#.##..",
                    ".#.##.#..#",
                    "#.#.#.##.#",
                    "....#...##",
                    "...##..##.",
                    "...#.#####",
                    ".#.####.#.",
                    "..#..###.#",
                    "..##.#..#.",
                    "          ",
                    "Tile 1489:",
                    "##.#.#....",
                    "..##...#..",
                    ".##..##...",
                    "..#...#...",
                    "#####...#.",
                    "#..#.#.#.#",
                    "...#.#.#..",
                    "##.#...##.",
                    "..##.##.##",
                    "###.##.#..",
                    "          ",
                    "Tile 2473:",
                    "#....####.",
                    "#..#.##...",
                    "#.##..#...",
                    "######.#.#",
                    ".#...#.#.#",
                    ".#########",
                    ".###.#..#.",
                    "########.#",
                    "##...##.#.",
                    "..###.#.#.",
                    "          ",
                    "Tile 2971:",
                    "..#.#....#",
                    "#...###...",
                    "#.#.###...",
                    "##.##..#..",
                    ".#####..##",
                    ".#..####.#",
                    "#..#.#..#.",
                    "..####.###",
                    "..#.#.###.",
                    "...#.#.#.#",
                    "          ",
                    "Tile 2729:",
                    "...#.#.#.#",
                    "####.#....",
                    "..#.#.....",
                    "....#..#.#",
                    ".##..##.#.",
                    ".#.####...",
                    "####.#.#..",
                    "##.####...",
                    "##..#.##..",
                    "#.##...##.",
                    "          ",
                    "Tile 3079:",
                    "#.#.#####.",
                    ".#..######",
                    "..#.......",
                    "######....",
                    "####.#..#.",
                    ".#...#.##.",
                    "#.#####.##",
                    "..#.###...",
                    "..#.......",
                    "..#.###...",
                    "          ",
                },
                20899048083289,
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "Tile 2311:",
                    "..##.#..#.",
                    "##..#.....",
                    "#...##..#.",
                    "####.#...#",
                    "##.##.###.",
                    "##...#.###",
                    ".#.#.#..##",
                    "..#....#..",
                    "###...#.#.",
                    "..###..###",
                    "          ",
                    "Tile 1951:",
                    "#.##...##.",
                    "#.####...#",
                    ".....#..##",
                    "#...######",
                    ".##.#....#",
                    ".###.#####",
                    "###.##.##.",
                    ".###....#.",
                    "..#.#..#.#",
                    "#...##.#..",
                    "          ",
                    "Tile 1171:",
                    "####...##.",
                    "#..##.#..#",
                    "##.#..#.#.",
                    ".###.####.",
                    "..###.####",
                    ".##....##.",
                    ".#...####.",
                    "#.##.####.",
                    "####..#...",
                    ".....##...",
                    "          ",
                    "Tile 1427:",
                    "###.##.#..",
                    ".#..#.##..",
                    ".#.##.#..#",
                    "#.#.#.##.#",
                    "....#...##",
                    "...##..##.",
                    "...#.#####",
                    ".#.####.#.",
                    "..#..###.#",
                    "..##.#..#.",
                    "          ",
                    "Tile 1489:",
                    "##.#.#....",
                    "..##...#..",
                    ".##..##...",
                    "..#...#...",
                    "#####...#.",
                    "#..#.#.#.#",
                    "...#.#.#..",
                    "##.#...##.",
                    "..##.##.##",
                    "###.##.#..",
                    "          ",
                    "Tile 2473:",
                    "#....####.",
                    "#..#.##...",
                    "#.##..#...",
                    "######.#.#",
                    ".#...#.#.#",
                    ".#########",
                    ".###.#..#.",
                    "########.#",
                    "##...##.#.",
                    "..###.#.#.",
                    "          ",
                    "Tile 2971:",
                    "..#.#....#",
                    "#...###...",
                    "#.#.###...",
                    "##.##..#..",
                    ".#####..##",
                    ".#..####.#",
                    "#..#.#..#.",
                    "..####.###",
                    "..#.#.###.",
                    "...#.#.#.#",
                    "          ",
                    "Tile 2729:",
                    "...#.#.#.#",
                    "####.#....",
                    "..#.#.....",
                    "....#..#.#",
                    ".##..##.#.",
                    ".#.####...",
                    "####.#.#..",
                    "##.####...",
                    "##..#.##..",
                    "#.##...##.",
                    "          ",
                    "Tile 3079:",
                    "#.#.#####.",
                    ".#..######",
                    "..#.......",
                    "######....",
                    "####.#..#.",
                    ".#...#.##.",
                    "#.#####.##",
                    "..#.###...",
                    "..#.......",
                    "..#.###...",
                    "          ",
                },
                273,
            };
        }
    }
}