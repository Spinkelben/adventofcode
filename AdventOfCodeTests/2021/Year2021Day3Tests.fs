namespace Year2021

module Day3Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
                "00100";
                "11110";
                "10110";
                "10111";
                "10101";
                "01111";
                "00111";
                "11100";
                "10000";
                "11001";
                "00010";
                "01010";
            };
            "198" |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
                "00100";
                "11110";
                "10110";
                "10111";
                "10101";
                "01111";
                "00111";
                "11100";
                "10000";
                "11001";
                "00010";
                "01010";
            };
            "" |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day3.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day3.main input
        part2 |> should equal expected