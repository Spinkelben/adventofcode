﻿module Year2019Day10Tests

open System
open Xunit
open FsUnit.Xunit

let part1Values : obj array seq =
    seq {
        yield [| seq { 
            ".#..#";
            ".....";
            "#####";
            "....#";
            "...##"; };
             "8"; |];
        yield [| seq { 
            "......#.#.";
            "#..#.#....";
            "..#######.";
            ".#.#.###..";
            ".#..#.....";
            "..#....#.#";
            "#..#....#.";
            ".##.#..###";
            "##...#..#.";
            ".#....####"; };
             "33"; |];
        yield [| seq { 
            "#.#...#.#.";
            ".###....#.";
            ".#....#...";
            "##.#.#.#.#";
            "....#.#.#.";
            ".##..###.#";
            "..#...##..";
            "..##....##";
            "......#...";
            ".####.###."; }; 
             "35"; |];
        yield [| seq { 
            ".#..#..###";
            "####.###.#";
            "....###.#.";
            "..###.##.#";
            "##.##.#.#.";
            "....###..#";
            "..#.#..#.#";
            "#..#.#.###";
            ".##...##.#";
            ".....#.#.."; }; 
             "41"; |];
        yield [| seq { 
           ".#..##.###...########";
           "##.############..##.#";
           ".#.######.########.##";
           ".###.#######.####.#.#";
           "#####.##.#.##.###.###";
           "..#####..#.##########";
           "#####################";
           "#.####....###.#.#.###";
           "##.##################";
           "#####.##.###..####..#";
           "..######..##.########";
           "####.##.####...##..##";
           ".#####..#.######.####";
           "##...#.##########...#";
           "#.##########.########";
           ".####.#.###.###.#.###";
           "....##.##.###..######";
           ".#.#.###########.####";
           "#.#.#.#####.####.####";
           "###.##.####.##.#..###"; }; 
             "210"; |];
        
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
    let part1, _ = Year2019Day10.main input
    part1 |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2019Day7.main input
    part2 |> should equal expected