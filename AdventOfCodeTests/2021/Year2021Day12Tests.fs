namespace Year2021

module Day12Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
               "start-A";
               "start-b";
               "A-c";
               "A-b";
               "b-d";
               "A-end";
               "b-end";
            };
            "10" |];
            yield [| seq {
                "dc-end";
                "HN-start";
                "start-kj";
                "dc-start";
                "dc-HN";
                "LN-dc";
                "HN-end";
                "kj-sa";
                "kj-HN";
                "kj-dc";
             };
             "19" |];
             yield [| seq {
                 "fs-end";
                 "he-DX";
                 "fs-he";
                 "start-DX";
                 "pj-DX";
                 "end-zg";
                 "zg-sl";
                 "zg-pj";
                 "pj-he";
                 "RW-he";
                 "fs-DX";
                 "pj-RW";
                 "zg-RW";
                 "start-pj";
                 "he-WI";
                 "zg-he";
                 "pj-fs";
                 "start-RW";
              };
              "226" |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
                "start-A";
                "start-b";
                "A-c";
                "A-b";
                "b-d";
                "A-end";
                "b-end";
             };
             "36" |];
             yield [| seq {
                 "dc-end";
                 "HN-start";
                 "start-kj";
                 "dc-start";
                 "dc-HN";
                 "LN-dc";
                 "HN-end";
                 "kj-sa";
                 "kj-HN";
                 "kj-dc";
              };
              "103" |];
              yield [| seq {
                  "fs-end";
                  "he-DX";
                  "fs-he";
                  "start-DX";
                  "pj-DX";
                  "end-zg";
                  "zg-sl";
                  "zg-pj";
                  "pj-he";
                  "RW-he";
                  "fs-DX";
                  "pj-RW";
                  "zg-RW";
                  "start-pj";
                  "he-WI";
                  "zg-he";
                  "pj-fs";
                  "start-RW";
               };
               "3509" |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day12.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day12.main input
        part2 |> should equal expected

    [<Fact>]
    let ``Part2 path test`` () =
        let path = [
            "kj";
            "sa";
            "kj";
            "HN";
            "dc";
            "HN";
            "start"]
        let cave = "dc"
        Year2021.Day12.isAllowedPart2 path cave |> should equal false
