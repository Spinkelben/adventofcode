namespace Year2021

module Day17Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
               "target area: x=20..30, y=-10..-5";
                };
                "45"
            |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
                "C200B40A82";
                 };
                 "3"
             |];
             yield [| seq {
                 "04005AC33890";
                  };
                  "54"
             |];
             yield [| seq {
                 "880086C3E88112";
                 };
                 "7"
             |];
             yield [| seq {
                 "CE00C43D881120";
                  };
                  "9"
             |];
             yield [| seq {
                 "D8005AC2A8F0";
                  };
                  "1"
             |];
             yield [| seq {
                 "F600BC2D8F";
                  };
                  "0"
             |];
             yield [| seq {
                 "9C005AC2F8F0";
                  };
                  "0"
             |];
             yield [| seq {
                 "9C0141080250320F1802104A08";
                  };
                  "1"
             |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day17.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day17.main input
        part2 |> should equal expected
