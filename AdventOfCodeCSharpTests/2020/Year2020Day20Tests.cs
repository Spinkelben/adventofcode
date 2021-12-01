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
        public void Part2(List<string> tiles, long numMatches)
        {
            var puzzleSolver = new Year2020Day20();
            Assert.Equal(numMatches.ToString(), puzzleSolver.Part2(tiles));
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void TileBordersMatch(List<string> tilesStr, long numMatches)
        {
            var puzzleSolver = new Year2020Day20();
            var tiles = puzzleSolver.ParseInput(tilesStr).ToList();
            var edgeDict = puzzleSolver.GetEdgeDict(tiles);
            var grid = puzzleSolver.AssembleTiles(tiles, edgeDict);
            var waringRemove = numMatches;
            for (int i = 0; i < grid.Count - 1; i++)
            {
                for (int j = 0; j < grid[i].Count - 1; j++)
                {
                    Assert.Equal(grid[i][j].GetEdge(1), grid[i][j + 1].GetEdge(3));
                    Assert.Equal(grid[i][j].GetEdge(2), grid[i + 1][j].GetEdge(0));
                }
            }
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void AssembledPictureTest(List<string> tilesStr, long numMatches)
        {
            var puzzleSolver = new Year2020Day20();
            var tiles = puzzleSolver.ParseInput(tilesStr).ToList();
            var edgeDict = puzzleSolver.GetEdgeDict(tiles);
            var grid = puzzleSolver.AssembleTiles(tiles, edgeDict);
            var image = puzzleSolver.AssembleImage(grid);
            var variations = puzzleSolver.GetAllVariations(image);
            var waringRemove = numMatches;

            var sampleImage = new List<string>()
            {
                ".#.#..#.##...#.##..#####",
                "###....#.#....#..#......",
                "##.##.###.#.#..######...",
                "###.#####...#.#####.#..#",
                "##.#....#.##.####...#.##",
                "...########.#....#####.#",
                "....#..#...##..#.#.###..",
                ".####...#..#.....#......",
                "#..#.##..#..###.#.##....",
                "#.####..#.####.#.#.###..",
                "###.#.#...#.######.#..##",
                "#.####....##..########.#",
                "##..##.#...#...#.#.#.#..",
                "...#..#..#.#.##..###.###",
                ".#.#....#.##.#...###.##.",
                "###.#...#..#.##.######..",
                ".#.#.###.##.##.#..#.##..",
                ".####.###.#...###.#..#.#",
                "..#.#..#..#.#.#.####.###",
                "#..####...#.#.#.###.###.",
                "#####..#####...###....##",
                "#.##..#..#...#..####...#",
                ".#.###..##..##..####.##.",
                "...###...##...#...#..###",
            };

            Assert.Contains(variations, v => listCompare(v, sampleImage));

            bool listCompare(List<string> a, List<string> b)
            {
                if (a.Count != b.Count)
                {
                    return false;
                }

                for (int i = 0; i < a.Count; i++)
                {
                    if (a[i].CompareTo(b[i]) != 0)
                    {
                        return false;
                    }
                }

                return true;
            }
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
            Assert.Equal(".#####..#.", tile.GetEdge(1));
            Assert.Equal("...#.##..#", tile.GetEdge(3));
            Assert.Equal(".....#..##", tile.GetPatternLine(1));
            var patternXFlipped = new List<string>()
            {
                ".#..#.##.. ",
                ".....#..## ",
                ".#..##...# ",
                "#...#.#### ",
                ".###.##.## ",
                "###.#...## ",
                "##..#.#.#. ",
                "..#....#.. ",
                ".#.#...### ",
                "###..###.. ",
                "           ",
            };
            Assert.Equal(
                patternXFlipped,
                puzzleSolver.GetPattern(new List<List<Year2020Day20.Tile>>()
                { 
                    new List<Year2020Day20.Tile>()
                    { 
                        tile
                    }
                }));

            tile.FlipX = false;
            tile.FlipY = true;

            Assert.Equal("..###..###", tile.GetEdge(0));
            Assert.Equal("..##.#..#.", tile.GetEdge(2));
            Assert.Equal("#..##.#...", tile.GetEdge(1));
            Assert.Equal(".#..#####.", tile.GetEdge(3));
            Assert.Equal("###...#..#", tile.GetPatternColumn(2));

            var patternYFlipped = new List<string>()
            {
                "..###..### ",
                "###...#.#. ",
                "..#....#.. ",
                ".#.#.#..## ",
                "##...#.### ",
                "##.##.###. ",
                "####.#...# ",
                "#...##..#. ",
                "##..#..... ",
                "..##.#..#. ",
                "           ",
            };
            Assert.Equal(
                patternYFlipped,
                puzzleSolver.GetPattern(new List<List<Year2020Day20.Tile>>()
                {
                    new List<Year2020Day20.Tile>()
                    {
                        tile
                    }
                }));
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
            Assert.Equal("#..##.#...", topEdge);
            Assert.Equal(".#..#.##..", rightEdge);
            Assert.Equal(".#..#####.", bottomEdge);
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
            Assert.Equal("..###..###", tile.GetEdge(0));
            Assert.Equal("#..##.#...", tile.GetEdge(1));
            Assert.Equal("..##.#..#.", tile.GetEdge(2));
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
            Assert.Equal(".#####..#.", tile.GetEdge(1));
            Assert.Equal("###..###..", tile.GetEdge(2));
            Assert.Equal("...#.##..#", tile.GetEdge(3));
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

        [Fact]
        public void Edge()
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
            var tile = puzzleSolver.ParseInput(lines).First();

            Assert.Equal("..##.#..#.", tile.GetEdge(0));
            Assert.Equal("...#.##..#", tile.GetEdge(1));
            Assert.Equal("..###..###", tile.GetEdge(2));
            Assert.Equal(".#####..#.", tile.GetEdge(3));

            tile.FlipXRelative(true);
            tile.Rotation = 3;
            Assert.Equal(".#####..#.", tile.GetEdge(0));
            Assert.Equal("..###..###", tile.GetEdge(1));
            Assert.Equal("...#.##..#", tile.GetEdge(2));
            Assert.Equal("..##.#..#.", tile.GetEdge(3));

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