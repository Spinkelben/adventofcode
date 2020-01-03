module Year2019Day12Test

open System
open Xunit
open FsUnit.Xunit
open Year2019Day12

let part1Values : obj array seq =
    seq {
        yield [| seq { 
           "<x=-1, y=0, z=2>";
           "<x=2, y=-10, z=-7>";
           "<x=4, y=-8, z=8>";
           "<x=3, y=5, z=-1>";};
              10;
              179; |];

        yield [| seq { 
                "<x=-8, y=-10, z=0>  ";
                "<x=5, y=5, z=10>    ";
                "<x=2, y=-7, z=3>    ";
                "<x=9, y=-8, z=-3>   ";};
               100;
               1940; |];

        yield [| seq { 
            "<x=-2, y=9, z=-5>      ";
            "<x=16, y=19, z=9>      ";
            "<x=0, y=3, z=6>        ";
            "<x=11, y=0, z=11>      ";};
           200000;
           126050; |];
    }

let velocityTest : obj array seq =
    seq {
        yield [| seq { 
           "<x=-1, y=0, z=2>";
           "<x=2, y=-10, z=-7>";
           "<x=4, y=-8, z=8>";
           "<x=3, y=5, z=-1>";};
              [
                    { position= (-1, 0,2); velocity= (3,-1,-1) };
                    { position= (2, -10, -7); velocity= (1, 3, 3) };
                    { position= (4, -8, 8); velocity= (-3, 1,-3) };
                    { position= (3, 5, -1); velocity= (-1,-3, 1) };
              ]; |];

    }

let simulationData : obj array seq =
    seq {
        yield [| seq { 
           "<x=-1, y=0, z=2>";
           "<x=2, y=-10, z=-7>";
           "<x=4, y=-8, z=8>";
           "<x=3, y=5, z=-1>";};
              1;
              [
                    { position= (2, -1, 1); velocity= (3,-1,-1) };
                    { position= (3, -7, -4); velocity= (1, 3, 3) };
                    { position= (1, -7, 5); velocity= (-3, 1,-3) };
                    { position= (2, 2, 0); velocity= (-1,-3, 1) };
              ]; |];
        yield [| seq { 
            "<x=-1, y=0, z=2>";
            "<x=2, y=-10, z=-7>";
            "<x=4, y=-8, z=8>";
            "<x=3, y=5, z=-1>";};
               2;
               [
                     { position= (5, -3, -1); velocity= ( 3, -2, -2) };
                     { position= (1, -2,  2); velocity= (-2,  5,  6) };
                     { position= (1, -4, -1); velocity= ( 0,  3, -6) };
                     { position= (1, -4,  2); velocity= (-1, -6,  2) };
               ]; |];
        yield [| seq { 
            "<x=-1, y=0, z=2>";
            "<x=2, y=-10, z=-7>";
            "<x=4, y=-8, z=8>";
            "<x=3, y=5, z=-1>";};
               5;
               [
                     { position= (-1, -9,  2); velocity= (-3, -1,  2) };
                     { position= ( 4,  1,  5); velocity= ( 2,  0, -2) };
                     { position= ( 2,  2, -4); velocity= ( 0, -1,  2) };
                     { position= ( 3, -7, -1); velocity= ( 1,  2, -2) };
               ]; |];
        yield [| seq { 
            "<x=-1, y=0, z=2>";
            "<x=2, y=-10, z=-7>";
            "<x=4, y=-8, z=8>";
            "<x=3, y=5, z=-1>";};
               6;
               [
                     { position= (-1, -7,  3); velocity= ( 0,  2,  1) };
                     { position= ( 3,  0,  0); velocity= (-1, -1, -5) };
                     { position= ( 3, -2,  1); velocity= ( 1, -4,  5) };
                     { position= ( 3, -4, -2); velocity= ( 0,  3, -1) };
               ]; |];
        yield [| seq { 
            "<x=-1, y=0, z=2>";
            "<x=2, y=-10, z=-7>";
            "<x=4, y=-8, z=8>";
            "<x=3, y=5, z=-1>";};
               10;
               [
                     { position= (2,  1, -3); velocity= (-3, -2,  1) };
                     { position= (1, -8,  0); velocity= (-1,  1,  3) };
                     { position= (3, -6,  1); velocity= ( 3,  2, -3) };
                     { position= (2,  0,  4); velocity= ( 1, -1, -1) };
               ]; |];
        yield [| seq { 
                "<x=-8, y=-10, z=0>";
                "<x=5, y=5, z=10>  ";
                "<x=2, y=-7, z=3>  ";
                "<x=9, y=-8, z=-3> "; };
               100;
               [
                     { position= (  8, -12, -9); velocity= (-7,   3,   0) };
                     { position= ( 13,  16, -3); velocity= ( 3, -11,  -5) };
                     { position= (-29, -11, -1); velocity= (-3,   7,   4) };
                     { position= ( 16, -13, 23); velocity= ( 7,   1,   1) };
               ]; |];
    }

let energyCalc : obj array seq =
    seq {
        yield [| seq {
                { position= (2,  1, -3); velocity= (-3, -2,  1) };
                { position= (1, -8,  0); velocity= (-1,  1,  3) };
                { position= (3, -6,  1); velocity= ( 3,  2, -3) };
                { position= (2,  0,  4); velocity= ( 1, -1, -1) };
            };
            179
        |];
        yield [| seq {
            { position= (  8, -12, -9); velocity= (-7,   3,   0) };
            { position= ( 13,  16, -3); velocity= ( 3, -11,  -5) };
            { position= (-29, -11, -1); velocity= (-3,   7,   4) };
            { position= ( 16, -13, 23); velocity= ( 7,   1,   1) };
        };
        1940
        |];
    }

let part2Values : obj array seq =
    seq {
        yield [| seq { 
           "<x=-1, y=0, z=2>";
           "<x=2, y=-10, z=-7>";
           "<x=4, y=-8, z=8>";
           "<x=3, y=5, z=-1>";};
              "2772"; |];

        yield [| seq { 
                "<x=-8, y=-10, z=0>  ";
                "<x=5, y=5, z=10>    ";
                "<x=2, y=-7, z=3>    ";
                "<x=9, y=-8, z=-3>   ";};
               "4686774924"; |];
    }

[<Theory>]
[<MemberData("energyCalc")>]
let ``Energy Calculation Test`` (moons :seq<Moon>, expected) =
    let energy = calculateEnergy moons
    energy |> should equal expected

[<Theory>]
[<MemberData("velocityTest")>]
let ``VelocityUpdate Test`` (input :seq<string>, expected) =
    let moons = List.map parseMoon (List.ofSeq input)
    let updatedMoons = updateVelocities moons
    Seq.toList updatedMoons |> should equal expected

[<Theory>]
[<MemberData("simulationData")>]
let ``SimulationTest Test`` (input :seq<string>, steps, expected) =
    let moons = Seq.map parseMoon input 
    let updatedMoons, _ = runSimulation moons steps
    Seq.toList updatedMoons |> should equal expected

[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, steps, expected) =
    let moons = Seq.map parseMoon input 
    let finalState, _ = runSimulation moons steps
    calculateEnergy finalState  |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2019Day12.main input
    part2 |> should equal expected