namespace Year2021

module Day16Tests =
    open Xunit
    open FsUnit.Xunit

    let part1Values : obj array seq =
        seq {
            yield [| seq {
               "8A004A801A8002F478";
                };
                "16"
            |];
            yield [| seq {
                "620080001611562C8802118E34";
                 };
                 "12"
            |];
            yield [| seq {
                "C0015000016115A2E0802F182340";
                };
                "23"
            |];
            yield [| seq {
                "A0016C880162017C3686B18A3D4780";
                 };
                 "31"
            |];
        }

    let part2Values : obj array seq =
        seq {
            yield [| seq {
               "1163751742";
               "1381373672";
               "2136511328";
               "3694931569";
               "7463417111";
               "1319128137";
               "1359912421";
               "3125421639";
               "1293138521";
               "2311944581";
            };
            ""
            |];
        }

    [<Theory>]
    [<MemberData("part1Values")>]
    let ``Part1 Test`` (input :seq<string>, expected) =
        let part1, _ = Year2021.Day16.main input
        part1 |> should equal expected

    [<Theory>]
    [<MemberData("part2Values")>]
    let ``Part2 Test`` (input :seq<string>, expected) =
        let _, part2 = Year2021.Day16.main input
        part2 |> should equal expected

    [<Fact>]
    let ``Literal Test`` () =
        let input = ["D2FE28"]
        let bitList = Year2021.Day16.getBitList input
        let (packets, rest) = Year2021.Day16.parsePacket bitList
        List.length packets |> should equal 1
        match List.head packets with
        | Year2021.Day16.Literal (header, number) -> number |> should equal 2021L
                                                     header.Version |> should equal 6L
                                                     header.TypeId |> should equal 4L
        | _ -> ()

    [<Fact>]
    let ``Operator bit count Test`` () =
        let input = ["38006F45291200"]
        let bitList = Year2021.Day16.getBitList input
        let (packets, rest) = Year2021.Day16.parsePacket bitList
        List.length packets |> should equal 1
        match List.head packets with
        | Year2021.Day16.Operator (header, subPackets) -> 
            header.Version |> should equal 1L
            header.TypeId |> should equal 6L
            Seq.length subPackets |> should equal 2
            let literals = subPackets |> Seq.map (fun p -> 
                match p with
                | Day16.Literal (header, number) -> number
                | _ -> failwith "Error")
            Seq.item 0 literals |> should equal 10L
            Seq.item 1 literals |> should equal 20L
        | _ -> ()

    [<Fact>]
    let ``Operator packet count Test`` () =
        let input = ["EE00D40C823060"]
        let bitList = Year2021.Day16.getBitList input
        let (packets, rest) = Year2021.Day16.parsePacket bitList
        List.length packets |> should equal 1
        match List.head packets with
        | Year2021.Day16.Operator (header, subPackets) -> 
            header.Version |> should equal 7L
            header.TypeId |> should equal 3L
            Seq.length subPackets |> should equal 3
            let literals = subPackets |> Seq.map (fun p -> 
                match p with
                | Day16.Literal (header, number) -> number
                | _ -> failwith "Error")
            Seq.item 0 literals |> should equal 1L
            Seq.item 1 literals |> should equal 2L
            Seq.item 2 literals |> should equal 3L
        | _ -> ()
