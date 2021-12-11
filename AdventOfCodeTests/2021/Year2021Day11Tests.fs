namespace Year2021

module Day11Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
               "5483143223";
               "2745854711";
               "5264556173";
               "6141336146";
               "6357385478";
               "4167524645";
               "2176841721";
               "6882881134";
               "4846848554";
               "5283751526";
            };
            "1656" |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
                "5483143223";
                "2745854711";
                "5264556173";
                "6141336146";
                "6357385478";
                "4167524645";
                "2176841721";
                "6882881134";
                "4846848554";
                "5283751526";
             };
             "195" |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day11.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day11.main input
        part2 |> should equal expected
