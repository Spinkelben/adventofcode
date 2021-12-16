namespace Year2021

module Day15Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
               "1163751742";
               "1381373672";
               "2136511328";
               "3694931569";
               "7463417111";
               "1319128137";
               "1359912421";
               "3125421639";
               "1293138521";
               "2311944581";
            };
            "40"
            |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
               "1163751742";
               "1381373672";
               "2136511328";
               "3694931569";
               "7463417111";
               "1319128137";
               "1359912421";
               "3125421639";
               "1293138521";
               "2311944581";
            };
            "315"
            |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day15.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day15.main input
        part2 |> should equal expected


