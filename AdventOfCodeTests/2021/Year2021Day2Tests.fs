namespace Year2021

module Day2Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
                "forward 5";
                "down 5";
                "forward 8";
                "up 3";
                "down 8";
                "forward 2";
            };
            "150" |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
                "forward 5";
                "down 5";
                "forward 8";
                "up 3";
                "down 8";
                "forward 2";
            };
            "900" |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day2.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day2.main input
        part2 |> should equal expected