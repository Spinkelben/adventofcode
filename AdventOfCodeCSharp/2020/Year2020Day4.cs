using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 4)]
    internal class Year2020Day4 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var passports = ParsePassportBatch(input, false);
            return passports.Count(p => p.IsValid())
                .ToString();
        }

        public string Part2(IList<string> input)
        {
            var passports = ParsePassportBatch(input, true);
            return passports.Count(p => p.IsValid())
                .ToString();
        }

        private List<Passport> ParsePassportBatch(IList<string> passports, bool validateFields)
        {
            var listPassport = new List<Passport>();
            var currentPassport = new Passport()
            {
                InputValidationEnabled = validateFields,
            };
            listPassport.Add(currentPassport);

            foreach (var line in passports)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    currentPassport = new Passport()
                    {
                        InputValidationEnabled = validateFields,
                    };
                    listPassport.Add(currentPassport);
                }
                else
                {
                    currentPassport.AddLineInformation(line);
                }
            }

            return listPassport;
        }
    }

    internal class Passport
    {
        internal int? BirthYear { get; set; }
        internal int? IssueYear { get; set; }
        internal int? ExpirationYear { get; set; }
        internal double? HeightInCm { get; set; }
        internal string HairColor { get; set; }
        internal string EyeColor { get; set; }
        internal long? PassportId { get; set; }
        internal long? CountryId { get; set; }

        internal bool InputValidationEnabled { get; set; }

        internal bool IsValid(bool allowNorthPoleCredentials = true)
        {
            if (BirthYear is null ||
                InputValidationEnabled && (BirthYear < 1920 || BirthYear > 2002))
            {
                return false;
            }

            if (IssueYear is null ||
                InputValidationEnabled && (IssueYear < 2010 || IssueYear > 2020))
            {
                return false;
            }
            
            if (ExpirationYear is null || InputValidationEnabled && (ExpirationYear < 2020 || ExpirationYear > 2030))
            {
                return false;
            }

            if (HeightInCm is null || InputValidationEnabled && (HeightInCm < 149.86 || HeightInCm > 193.04))
            {
                return false;
            }

            if (HairColor is null ||
                InputValidationEnabled && (Regex.IsMatch(HairColor, @"#[0-9a-f]{6}") == false))
            {
                return false;
            }

            if (EyeColor is null
                || InputValidationEnabled && (new string[] 
                { 
                    "amb",
                    "blu",
                    "brn",
                    "gry",
                    "grn",
                    "hzl",
                    "oth",
                }).Contains(EyeColor) == false)
            {
                return false;
            }

            if (PassportId is null)
            {
                return false;
            }

            if (CountryId is null && allowNorthPoleCredentials == false)
            {
                return false;
            }

            return true;
        }

        internal void AddLineInformation(string line)
        {
            foreach (var field in line.Split(' '))
            {
                ParseField(field);
            }
        }

        private void ParseField(string field)
        {
            var segments = field.Split(':');
            var key = segments[0];
            var value = segments[1];

            switch (key)
            {
                case "byr":
                    this.BirthYear = ParseInt(value);
                    break;
                case "iyr":
                    this.IssueYear = ParseInt(value);
                    break;
                case "eyr":
                    this.ExpirationYear = ParseInt(value);
                    break;
                case "hgt":
                    this.HeightInCm = ParseHeightValue(value);
                    break;
                case "hcl":
                    this.HairColor = value;
                    break;
                case "ecl":
                    this.EyeColor = value;
                    break;
                case "pid":
                    this.PassportId = ParseLong(value);
                    if (InputValidationEnabled && value.Length != 9)
                    {
                        this.PassportId = null;
                    }

                    break;
                case "cid":
                    this.CountryId = ParseLong(value);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown field key {field}");
            }

            int? ParseInt(string value)
            {
                if (int.TryParse(value, out int number))
                {
                    return number;
                }

                Console.WriteLine($"Couldn't parse {value} as int. Input; {field}");
                return this.InputValidationEnabled ? (int?)null : -1;
            }

            long? ParseLong(string value)
            {
                if (long.TryParse(value, out long number))
                {
                    return number;
                }

                Console.WriteLine($"Couldn't parse {value} as int. Input; {field}");
                return this.InputValidationEnabled ? (long?)null : -1;
            }
        }

        private double? ParseHeightValue(string value)
        {
            try
            {
                if (value.EndsWith("cm"))
                {
                    return double.Parse(value[0..^2]);
                }
                else if (value.EndsWith("in"))
                {
                    var inches = double.Parse(value[0..^2]);
                    return inches * 2.54;
                }

                Console.WriteLine($"Couldn't parse height value {value}");
                return this.InputValidationEnabled ? (double?)null : -1.0;
            }
            catch (FormatException e)
            {
                Console.WriteLine($"Couldn't parse value {value}");
                throw e;
            }
        }
    }
}
