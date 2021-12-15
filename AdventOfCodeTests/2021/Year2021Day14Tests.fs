namespace Year2021

module Day14Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
               "NNCB";
               "";
               "CH -> B";
               "HH -> N";
               "CB -> H";
               "NH -> C";
               "HB -> C";
               "HC -> B";
               "HN -> C";
               "NN -> C";
               "BH -> H";
               "NC -> B";
               "NB -> B";
               "BN -> B";
               "BB -> N";
               "BC -> B";
               "CC -> N";
               "CN -> C";
            };
            "1588"       
            |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
               "NNCB";
               "";
               "CH -> B";
               "HH -> N";
               "CB -> H";
               "NH -> C";
               "HB -> C";
               "HC -> B";
               "HN -> C";
               "NN -> C";
               "BH -> H";
               "NC -> B";
               "NB -> B";
               "BN -> B";
               "BB -> N";
               "BC -> B";
               "CC -> N";
               "CN -> C";
            };
            "2188189693529"
            |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day14.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day14.main input
        part2 |> should equal expected


