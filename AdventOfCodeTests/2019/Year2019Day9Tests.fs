module Year2019Day9Tests

open System
open Xunit
open FsUnit.Xunit

let part1Values : obj array seq =
    seq {
        yield [| seq { 
            "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99"; };
             [109L; 1L; 204L; -1L; 1001L; 100L; 1L; 100L; 1008L; 100L; 16L; 101L; 1006L; 101L; 0L; 99L;]; |];
        yield [| seq { 
            "1102,34915192,34915192,7,4,7,99,0"; };
             [1219070632396864L]; |];
        yield [| seq { 
            "104,1125899906842624,99"; };
             [1125899906842624L]; |];
    }

[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, expected) =
    let program =
        (Seq.head input).Split(",")
        |> Array.map int64
    let (output, _, _), _ = IntCodeComputer.executeProgram program [] None None None
    output |> should equal expected
