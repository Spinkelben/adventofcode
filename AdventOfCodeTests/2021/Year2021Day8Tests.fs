namespace Year2021

module Day8Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
                "be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe";
                "edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc";
                "fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg";
                "fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb";
                "aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea";
                "fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb";
                "dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe";
                "bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef";
                "egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb";
                "gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce";
            };
            "26" |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
                "be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe";
                "edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc";
                "fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg";
                "fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb";
                "aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea";
                "fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb";
                "dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe";
                "bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef";
                "egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb";
                "gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce";
            };
            "61229" |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day8.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day8.main input
        part2 |> should equal expected

    [<Fact>]
    let ``Part2 Parser Test`` () =
        let input = [|
            Set.ofSeq "acedgfb";
            Set.ofSeq "cdfbe";
            Set.ofSeq "gcdfa";
            Set.ofSeq "fbcad";
            Set.ofSeq "dab";
            Set.ofSeq "cefabd";
            Set.ofSeq "cdfgeb";
            Set.ofSeq "eafb";
            Set.ofSeq "cagedb";
            Set.ofSeq "ab"|]
        let map = Year2021.Day8.deduceSegementConfiguration input
        Map.find (Set.ofSeq "acedgfb") map  |> should equal "8"
        Map.find (Set.ofSeq "cdfbe") map    |> should equal "5"
        Map.find (Set.ofSeq "gcdfa") map    |> should equal "2"
        Map.find (Set.ofSeq "fbcad") map    |> should equal "3"
        Map.find (Set.ofSeq "dab") map      |> should equal "7"
        Map.find (Set.ofSeq "cefabd") map   |> should equal "9"
        Map.find (Set.ofSeq "cdfgeb") map   |> should equal "6"
        Map.find (Set.ofSeq "eafb") map     |> should equal "4"
        Map.find (Set.ofSeq "cagedb") map   |> should equal "0"
        Map.find (Set.ofSeq "ab") map       |> should equal "1"
