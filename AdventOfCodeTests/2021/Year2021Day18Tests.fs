namespace Year2021

module Day18Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
                    "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]";
                    "[[[5,[2,8]],4],[5,[[9,9],0]]]";
                    "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]";
                    "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]";
                    "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]";
                    "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]";
                    "[[[[5,4],[7,7]],8],[[8,3],8]]";
                    "[[9,3],[[9,9],[6,[4,9]]]]";
                    "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]";
                    "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]";
                };
                "[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]";
                "4140"
            |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
                "[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]";
                "[[[5,[2,8]],4],[5,[[9,9],0]]]";
                "[6,[[[6,2],[5,6]],[[7,6],[4,7]]]]";
                "[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]";
                "[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]";
                "[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]";
                "[[[[5,4],[7,7]],8],[[8,3],8]]";
                "[[9,3],[[9,9],[6,[4,9]]]]";
                "[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]";
                "[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]";
            };
            "3993"
            |];
        }

    let addTestValues : obj array seq = 
        seq {
            yield [| 
                seq {
                    "[1,1]";
                    "[2,2]";
                    "[3,3]";
                    "[4,4]";
                };
                "[[[[1,1],[2,2]],[3,3]],[4,4]]"
            |];
            yield [| 
                seq {
                    "[1,1]";
                    "[2,2]";
                    "[3,3]";
                    "[4,4]";
                    "[5,5]";
                };
                "[[[[3,0],[5,3]],[4,4]],[5,5]]"
            |];
            yield [| 
                seq {
                    "[1,1]";
                    "[2,2]";
                    "[3,3]";
                    "[4,4]";
                    "[5,5]";
                    "[6,6]";
                };
                "[[[[5,0],[7,4]],[5,5]],[6,6]]"
            |];
            yield [| 
                seq {
                    "[[[[4,3],4],4],[7,[[8,4],9]]]";
                    "[1,1]"
                };
                "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]"
            |];
            yield [| 
                seq {
                    "[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]";
                    "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]";
                    "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]";
                    "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]";
                    "[7,[5,[[3,8],[1,4]]]]";
                    "[[2,[2,2]],[8,[8,1]]]";
                    "[2,9]";
                    "[1,[[[9,3],9],[[9,0],[0,7]]]]";
                    "[[[5,[7,4]],7],1]";
                    "[[[[4,2],2],6],[8,7]]";
                };
                "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]"
            |];
        }

    let explodeTestValues : obj array seq = 
        seq {
            yield [|
                "[[[[[9,8],1],2],3],4]"
                "[[[[0,9],2],3],4]"
            |];
            yield [|
                "[7,[6,[5,[4,[3,2]]]]]";
                "[7,[6,[5,[7,0]]]]"
            |];
            yield [|
                "[[6,[5,[4,[3,2]]]],1]";
                "[[6,[5,[7,0]]],3]"
            |];
            yield [|
                "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]";
                "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]"
            |];
            yield [|
                "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]";
                "[[3,[2,[8,0]]],[9,[5,[7,0]]]]"
            |];
        }

    let splitTestValues : obj array seq = 
        seq {
            yield [|
                "[1,10]";
                "[1,[5,5]]"
            |];
            yield [|
                "[1,11]";
                "[1,[5,6]]"
            |];
            yield [|
                "[1,12]";
                "[1,[6,6]]"
            |];
            yield [|
                "[[[[0,7],4],[15,[0,13]]],[1,1]]";
                "[[[[0,7],4],[[7,8],[0,13]]],[1,1]]"
            |];
            yield [|
                "[[[[0,7],4],[[7,8],[0,13]]],[1,1]]"
                "[[[[0,7],4],[[7,8],[0,[6,7]]]],[1,1]]";
            |];
        }

    let magnitudeTestValues : obj array seq = seq {
        yield [|
            "[9,1]";
            29
        |];
        yield [|
            "[1,9]";
            21
        |];
        yield [|
            "[[9,1],[1,9]]";
            129
        |];
        yield [|
            "[[1,2],[[3,4],5]]";
            143
        |];
        yield [|
            "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]";
            1384
        |];
        yield [|
            "[[[[1,1],[2,2]],[3,3]],[4,4]]";
            445
        |];
        yield [|
            "[[[[3,0],[5,3]],[4,4]],[5,5]]";
            791
        |];
        yield [|
            "[[[[5,0],[7,4]],[5,5]],[6,6]]";
            1137
        |];
        yield [|
            "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]";
            3488
        |];
    }

    let parseInput i =
        ParseUtil.run Year2021.Day18.pairParser i 
        |> Year2021.Day18.getTree

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected, magnitude) =
        let part1, _ = Year2021.Day18.main input
        let finalNumber = input 
                          |> Seq.map parseInput
                          |> Seq.reduce Year2021.Day18.addNumbers
        finalNumber |> should equal (parseInput expected)
        part1 |> should equal magnitude

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day18.main input
        part2 |> should equal expected

    [<Theory>]
    [<MemberData("addTestValues")>]
    let ``Add2 Test`` (input, expected) =
        let expected = parseInput expected
        let result = input 
                     |> Seq.map parseInput
                     |> Seq.reduce Year2021.Day18.addNumbers
        result |> should equal expected

    [<Fact>]
    let ``Add1 Test`` () =
        let n1 = ParseUtil.run Year2021.Day18.pairParser "[[[[4,3],4],4],[7,[[8,4],9]]]" 
                 |> Year2021.Day18.getTree
        let n2 = ParseUtil.run Year2021.Day18.pairParser "[1,1]" 
                 |> Year2021.Day18.getTree
        let expected = ParseUtil.run Year2021.Day18.pairParser "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]" 
                       |> Year2021.Day18.getTree
        Year2021.Day18.addNumbers n1 n2 |> should equal expected

    [<Theory>]
    [<MemberData("explodeTestValues")>]
    let ``Explode Test`` input expected  =
        let b = parseInput input 
        let expected = parseInput expected
        Year2021.Day18.explodeNumber b |> should equal (expected, true)

    [<Theory>]
    [<MemberData("splitTestValues")>]
    let ``Split Test`` input expected  =
        let b = parseInput input 
        let expected = parseInput expected
        Year2021.Day18.splitNumber b |> should equal (expected, true)

    [<Theory>]
    [<MemberData("magnitudeTestValues")>]
    let ``Magnitude Test`` input expected  =
        let b = parseInput input 
        Year2021.Day18.getMagnitude b |> should equal expected


