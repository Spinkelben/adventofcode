namespace Year2021

module Day6Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
                "3,4,3,1,2";
            };
            "5934" |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
               "3,4,3,1,2";
            };
            "26984457539" |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day6.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day6.main input
        part2 |> should equal expected