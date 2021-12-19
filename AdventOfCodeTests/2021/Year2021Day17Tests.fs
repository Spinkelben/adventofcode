namespace Year2021

module Day17Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
               "target area: x=20..30, y=-10..-5";
                };
                "45"
            |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
                "target area: x=20..30, y=-10..-5";
                 };
                 "112"
             |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day17.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day17.main input
        part2 |> should equal expected
