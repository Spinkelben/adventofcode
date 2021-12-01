module Year2021Day1Tests

open Xunit
open FsUnit.Xunit

let part1Values : obj array seq =
    seq {
        yield [| Seq.empty<string>; "" |];
    }

let part2Values : obj array seq =
    seq {
        yield [| Seq.empty<string>; "" |];
    }

[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, expected) =
    let part1, _ = Year2021Day1.main input
    part1 |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2021Day1.main input
    part2 |> should equal expected