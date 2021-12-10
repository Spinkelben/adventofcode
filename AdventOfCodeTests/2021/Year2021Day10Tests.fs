namespace Year2021

module Day10Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
               "[({(<(())[]>[[{[]{<()<>>";
               "[(()[<>])]({[<{<<[]>>(";
               "{([(<{}[<>[]}>{[]{[(<()>";
               "(((({<>}<{<{<>}{[]{[]{}";
               "[[<[([]))<([[{}[[()]]]";
               "[{[{({}]{}}([{[{{{}}([]";
               "{<[[]]>}<{[{[{[]{()[[[]";
               "[<(<(<(<{}))><([]([]()";
               "<{([([[(<>()){}]>(<<{{";
               "<{([{{}}[<[[[<>{}]]]>[]]";
            };
            "26397" |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
                "2199943210";
                "3987894921";
                "9856789892";
                "8767896789";
                "9899965678";
             };
             "1134" |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day10.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day10.main input
        part2 |> should equal expected
