module Year2019Day6Tests

open System
open Xunit
open FsUnit.Xunit

let part1Values : obj array seq =
        seq {
            yield [| seq { 
                "COM)B";
                "B)C";
                "C)D";
                "D)E";
                "E)F";
                "B)G";
                "G)H";
                "D)I";
                "E)J";
                "J)K";
                "K)L"; }; "42" |];
        }

let part2Values : obj array seq =
    seq {
        yield [| seq { 
            "COM)B";
            "B)C";
            "C)D";
            "D)E";
            "E)F";
            "B)G";
            "G)H";
            "D)I";
            "E)J";
            "J)K";
            "K)L";
            "K)YOU"; 
            "I)SAN"; }; "4" |];
    }

[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, expected) =
    let part1, _ = Year2019Day6.main input
    part1 |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2019Day6.main input
    part2 |> should equal expected