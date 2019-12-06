module Year2018Day2Test

open Xunit
open FsUnit.Xunit

let part1Values : obj array seq =
    seq {
        yield [|
            seq { 
                "abcdef";
                "bababc"; 
                "abbcde";
                "abcccd";
                "aabcdd";
                "abcdee";
                "ababab"
                }; 
             "12" |];
    }

let part2Values : obj array seq =
    seq {
        yield [| 
            seq { 
                "abcde"
                "fghij"
                "klmno"
                "pqrst"
                "fguij"
                "axcye"
                "wvxyz" 
            }; 
            "fgij" |];
        
    }

let part1Strings : obj array seq = 
    seq {
        yield [| "abcdef"; 3 ; false|]
        yield [| "abcdef"; 2 ; false|]
        yield [| "bababc"; 2 ; true|]
        yield [| "bababc"; 3 ; true|]
        yield [| "abbcde"; 2 ; true|]
        yield [| "abbcde"; 3 ; false|]
        yield [| "abcccd"; 2 ; false|]
        yield [| "abcccd"; 3 ; true|]
        yield [| "aabcdd"; 2 ; true|]
        yield [| "aabcdd"; 3 ; false|]
        yield [| "abcdee"; 2 ; true|]
        yield [| "abcdee"; 3 ; false|]
        yield [| "ababab"; 2 ; false|]
        yield [| "ababab"; 3 ; true|]
    }

[<Theory>]
[<MemberData("part1Strings")>]
let ``Test element frequency`` string count expected =
    Year2018Day2.countElementFrequency string count 
    |> should equal expected

[<Theory>]
[<MemberData("part1Values")>]
let ``Part1 Test`` (input :seq<string>, expected) =
    let part1, _ = Year2018Day2.main input
    part1 |> should equal expected

[<Theory>]
[<MemberData("part2Values")>]
let ``Part2 Test`` (input :seq<string>, expected) =
    let _, part2 = Year2018Day2.main input
    part2 |> should equal expected