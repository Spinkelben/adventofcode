module Year2019Day4Tests

open System
open Xunit
open FsUnit.Xunit

let part1Values : obj array seq =
        seq {
            yield [| seq { "111111-111111"; };                            "1" |];
            yield [| seq { "223450-223450"; };                            "0" |];
            yield [| seq { "123789-123789"; };                            "0" |];
        }

let part2Values : obj array seq =
    seq {
        yield [| seq { "112233-112233"; };                            "1" |];
        yield [| seq { "123444-123444"; };                            "0" |];
        yield [| seq { "111122-111122"; };                            "1" |];
    }



[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, expected) =
    let part1, _ = Year2019Day4.main input
    part1 |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2019Day4.main input
    part2 |> should equal expected