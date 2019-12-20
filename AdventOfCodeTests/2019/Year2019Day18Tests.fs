module Year2019Day18Tests

open Xunit
open FsUnit.Xunit

let part1Values : obj array seq =
    seq { 
        yield [|
            seq  { "#########
#b.A.@.a#
#########"
        };
            "8";
        |];
        yield [| seq {
            "########################
#f.D.E.e.C.b.A.@.a.B.c.#
######################.#
#d.....................#
########################" };
            "86";
        |];
        yield [| seq {
            "########################
#...............b.C.D.f#
#.######################
#.....@.a.B.c.d.A.e.F.g#
########################" };
            "132";
        |];
        yield [| seq {
            "#################
#i.G..c...e..H.p#
########.########
#j.A..b...f..D.o#
########@########
#k.E..a...g..B.n#
########.########
#l.F..d...h..C.m#
#################" };
            "136";
        |];
        yield [|  seq {
            "########################
#@..............ac.GI.b#
###d#e#f################
###A#B#C################
###g#h#i################
########################" };
            "81";
        |];
    }

let part2Values : obj array seq =
    seq { 
        yield [|
        |];
    }

[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, expected) =
    let part1, _ = Year2019Day18.main input
    part1 |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2019Day17.main input
    part2 |> should equal expected