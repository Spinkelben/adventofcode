module Year2019Day22Tests

open Xunit
open FsUnit.Xunit

let part1Values : obj array seq = 
    seq {
        yield [| 
            seq {
                "deal with increment 7";
                "deal into new stack  ";
                "deal into new stack  ";
            };
            "0 3 6 9 2 5 8 1 4 7"
        |];
        yield [| 
            seq {
                "cut 6                ";
                "deal with increment 7";
                "deal into new stack  ";
            };
            "3 0 7 4 1 8 5 2 9 6"
        |];
        yield [| 
            seq {
                "deal with increment 7";
                "deal with increment 9";
                "cut -2               ";
            };
            "6 3 0 7 4 1 8 5 2 9"
        |];
        yield [| 
            seq {
                "deal into new stack  ";
                "cut -2               ";
                "deal with increment 7";
                "cut 8                ";
                "cut -4               ";
                "deal with increment 7";
                "cut 3                ";
                "deal with increment 9";
                "deal with increment 3";
                "cut -1               ";
            };
            "9 2 5 8 1 4 7 0 3 6"
        |];
    }

[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, expected) =
    let commands = Seq.map Year2019Day22.parseCommand input
    let result = Year2019Day22.shuffleDeck (List.ofSeq commands) 9
    Year2019Day22.convertCardsTostring result |> should equal expected