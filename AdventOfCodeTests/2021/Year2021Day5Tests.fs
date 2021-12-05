namespace Year2021

module Day5Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
                "0,9 -> 5,9";
                "8,0 -> 0,8";
                "9,4 -> 3,4";
                "2,2 -> 2,1";
                "7,0 -> 7,4";
                "6,4 -> 2,0";
                "0,9 -> 2,9";
                "3,4 -> 1,4";
                "0,0 -> 8,8";
                "5,5 -> 8,2";
            };
            "5" |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
               "0,9 -> 5,9";
               "8,0 -> 0,8";
               "9,4 -> 3,4";
               "2,2 -> 2,1";
               "7,0 -> 7,4";
               "6,4 -> 2,0";
               "0,9 -> 2,9";
               "3,4 -> 1,4";
               "0,0 -> 8,8";
               "5,5 -> 8,2";
            };
            "" |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day5.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day5.main input
        part2 |> should equal expected