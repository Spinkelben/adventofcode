namespace Year2021

module Day1Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
                "199";
                "200";
                "208";
                "210";
                "200";
                "207";
                "240";
                "269";
                "260";
                "263";
            };
            "7" |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
                "199";
                "200";
                "208";
                "210";
                "200";
                "207";
                "240";
                "269";
                "260";
                "263";
            };
            "5" |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day1.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day1.main input
        part2 |> should equal expected