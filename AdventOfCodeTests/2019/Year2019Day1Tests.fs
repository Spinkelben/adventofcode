module Year2019Day1Tests

open System
open Xunit
open FsUnit.Xunit

let part1Values : obj array seq =
        seq {
            yield [| seq { "12" };      "2" |];
            yield [| seq { "14" };      "2" |];
            yield [| seq { "1969" };    "654" |];
            yield [| seq { "100756" };  "33583" |];
        }

let part2Values : obj array seq =
    seq {
        yield [| seq { "12" };      "2" |];
        yield [| seq { "14" };      "2" |];
        yield [| seq { "1969" };    "966" |];
        yield [| seq { "100756" };  "50346" |];
    }


[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, expected) =
    let part1, _ = Year2019Day1.main input
    part1 |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2019Day1.main input
    part2 |> should equal expected
