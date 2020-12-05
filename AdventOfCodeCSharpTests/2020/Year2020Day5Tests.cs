using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AdventOfCodeCSharp.Year2020;

namespace AdventOfCodeCSharpTests
{
    public class Year2020Day5Tests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public void Part1(string boardingPass, int row, int column, int id)
        {
            var puzzleSolver = new Year2020Day5();
            (int r, int c, int i) = puzzleSolver.ParseBoardingPass(boardingPass);
            Assert.Equal(row, r);
            Assert.Equal(column, c);
            Assert.Equal(id, i);
        }

        [Theory]
        [MemberData(nameof(TestData2))]
        public void Part2(List<string> passports, int numberOfValidPassports)
        {
            var puzzleSolver = new Year2020Day5();
            Assert.Equal(numberOfValidPassports.ToString(), puzzleSolver.Part2(passports));
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[]
            {
                "BFFFBBFRRR",
                70,
                7,
                567
            };

            yield return new object[]
            {
                "FFFBBBFRRR",
                14,
                7,
                119
            };

            yield return new object[]
            {
                "BBFFBBFRLL",
                102,
                4,
                820
            };
        }

        public static IEnumerable<object[]> TestData2()
        {
            yield return new object[]
            {
                new List<string>()
                {
                    "eyr:1972 cid:100",
                    "hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926",
                    "",
                    "iyr:2019",
                    "hcl:#602927 eyr:1967 hgt:170cm",
                    "ecl:grn pid:012533040 byr:1946",
                    "",
                    "hcl:dab227 iyr:2012",
                    "ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277",
                    "",
                    "hgt:59cm ecl:zzz",
                    "eyr:2038 hcl:74454a iyr:2023",
                    "pid:3556412378 byr:2007",
                    "",
                    "pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980",
                    "hcl:#623a2f",
                    "",
                    "eyr:2029 ecl:blu cid:129 byr:1989",
                    "iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm",
                    "",
                    "hcl:#888785",
                    "hgt:164cm byr:2001 iyr:2015 cid:88",
                    "pid:545766238 ecl:hzl",
                    "eyr:2022",
                    "",
                    "iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719",
                },
                4
            };
        }
    }
}