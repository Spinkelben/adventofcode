module Year2019Day16Tests

open System
open Xunit
open FsUnit.Xunit
open Year2019Day16

let part1Values : obj array seq =
    seq {
        yield [|
            seq { "12345678"};
            1;
            "48226158";
        |];
        yield [|
            seq { "12345678"};
            2;
            "34040438";
        |];
        yield [|
            seq { "12345678"};
            3;
            "03415518";
        |];
        yield [|
            seq { "12345678"};
            4;
            "01029498";
        |];
        yield [|
            seq { "80871224585914546619083218645595"};
            100;
            "24176176";
        |];
        yield [|
            seq { "19617804207202209144916044189917"};
            100;
            "73745418";
        |];
        yield [|
            seq { "69317163492948606335995924319873"};
            100;
            "52432133";
        |];
    }

let part2Values : obj array seq =
    seq { 
        yield [|
            seq { "03036732577212944063491565474664"};
            100;
            "84462026";
        |];
        yield [|
            seq { "02935109699940807407585447034323"};
            100;
            "78725270";
        |];
        yield [|
            seq { "03081770884921959731165446850517"};
            100;
            "53553731";
        |];
    }

[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, phases, expected) =
    let pInput = parseInput input
    let output = fft pInput phases
    let result = take8First output
    result |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, phases, expected) =
    let _, part2 = Year2019Day14.main input
    part2 |> should equal expected