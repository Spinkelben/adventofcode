namespace Year2021

module Day13Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
               "6,10";
               "0,14";
               "9,10";
               "0,3";
               "10,4";
               "4,11";
               "6,0";
               "6,12";
               "4,1";
               "0,13";
               "10,12";
               "3,4";
               "3,0";
               "8,4";
               "1,10";
               "2,14";
               "8,10";
               "9,0";
               "";
               "fold along y=7";
               "fold along x=5";
            };
            "17" |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
                "6,10";
                "0,14";
                "9,10";
                "0,3";
                "10,4";
                "4,11";
                "6,0";
                "6,12";
                "4,1";
                "0,13";
                "10,12";
                "3,4";
                "3,0";
                "8,4";
                "1,10";
                "2,14";
                "8,10";
                "9,0";
                "";
                "fold along y=7";
                "fold along x=5";
             };
             "36" |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day13.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day13.main input
        part2 |> should equal expected

