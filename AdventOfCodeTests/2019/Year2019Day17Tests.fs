module Year2019Day17Tests

open Xunit
open FsUnit.Xunit

let part1Values : obj array seq =
    seq { 
        yield [|
            "..#..........
..#..........
#######...###
#.#...#...#.#
#############
..#...#...#..
..#####...^.."
            76;
        |];
    }

let part2Values : obj array seq =
    seq { 
        yield [|
        |];
    }

[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :string, expected) =
    let calculation = Year2019Day17.calculateIntersections input
    calculation |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2019Day17.main input
    part2 |> should equal expected