namespace Year2021

module Day4Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
                "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1";
                "";
                "22 13 17 11  0";
                " 8  2 23  4 24";
                "21  9 14 16  7";
                " 6 10  3 18  5";
                " 1 12 20 15 19";
                "";
                " 3 15  0  2 22";
                " 9 18 13 17  5";
                "19  8  7 25 23";
                "20 11 10 24  4";
                "14 21 16 12  6";
                "";
                "14 21 17 24  4";
                "10 16 15  9 19";
                "18  8 23 26 20";
                "22 11 13  6  5";
                " 2  0 12  3  7";
            };
            "4512" |];
        }

    let part2Values : obj array seq =
       seq {
           yield [| seq {
               "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1";
               "";
               "22 13 17 11  0";
               " 8  2 23  4 24";
               "21  9 14 16  7";
               " 6 10  3 18  5";
               " 1 12 20 15 19";
               "";
               " 3 15  0  2 22";
               " 9 18 13 17  5";
               "19  8  7 25 23";
               "20 11 10 24  4";
               "14 21 16 12  6";
               "";
               "14 21 17 24  4";
               "10 16 15  9 19";
               "18  8 23 26 20";
               "22 11 13  6  5";
               " 2  0 12  3  7";
           };
           "1924" |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day4.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day4.main input
        part2 |> should equal expected