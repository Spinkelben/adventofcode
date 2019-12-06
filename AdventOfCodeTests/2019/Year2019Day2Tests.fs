module Year2019Day2Tests

open System
open Xunit
open FsUnit.Xunit
open Microsoft.FSharp.Control
open IntCodeComputer

let part1Values : obj array seq =
        seq {
            yield [| seq { "1,0,0,0,99" };          [| 2; 0; 0; 0; 99               |] |];
            yield [| seq { "2,3,0,3,99" };          [| 2; 3; 0; 6; 99               |] |];
            yield [| seq { "2,4,4,5,99,0" };        [| 2; 4; 4; 5; 99; 9801         |] |];
            yield [| seq { "1,1,1,4,99,5,6,0,99" }; [|30; 1; 1; 4; 2; 5; 6; 0; 99   |] |];
        }

let part2Values : obj array seq =
    Seq.empty


[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, expected) =
    let program = (Year2019Day2.formatInput input)
    let result = executeProgram program 0
    result |> should equal expected

[<Theory(Skip="No examples provided")>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2019Day2.main input
    part2 |> should equal expected
