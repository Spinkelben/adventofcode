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

let part2Values : obj array seq =
    seq {
        yield [| seq { 
            "3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5"; };
             "139629729"; |];
        yield [| seq { 
            "3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10"; };
             "18216"; |];
    }

[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, expected) =
    let program =
        (Seq.head input).Split(",")
        |> Array.map int64
    let (output, _, _), _ = IntCodeComputer.executeProgram program [] None None None
    output |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2019Day7.main input
    part2 |> should equal expected