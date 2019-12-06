module Year2019Day3Tests


open System
open Xunit
open FsUnit.Xunit

let part1Values : obj array seq =
        seq {
            yield [| seq { "R8,U5,L5,D3"; 
                            "U7,R6,D4,L4" };                            "6" |];
            yield [| seq { "R75,D30,R83,U83,L12,D49,R71,U7,L72"; 
                            "U62,R66,U55,R34,D71,R55,D58,R83" };        "159" |];
            yield [| seq { "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51";
                            "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" };   "135" |];
        }

let part2Values : obj array seq =
    seq {
        yield [| seq { "R8,U5,L5,D3"; 
                                   "U7,R6,D4,L4" };                            "30" |];
                   yield [| seq { "R75,D30,R83,U83,L12,D49,R71,U7,L72"; 
                                   "U62,R66,U55,R34,D71,R55,D58,R83" };        "610" |];
                   yield [| seq { "R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51";
                                   "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7" };   "410" |];
    }


[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, expected) =
    let part1, _ = Year2019Day3.main input
    part1 |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2019Day3.main input
    part2 |> should equal expected