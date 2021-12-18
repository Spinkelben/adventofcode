namespace Year2021

open System
open System.Globalization

module Day16 =
    
    type PacketHeader = {
        Version : int64;
        TypeId : int64;
    } 

    type Packet =
    | Literal of PacketHeader * int64
    | Operator of PacketHeader * Packet seq

    let rec foldPackets (folder : 'a -> Packet -> 'a) (state :'a) p = 
        match p with
        | Packet.Literal (h, n) -> folder state p
        | Packet.Operator (h , ps) -> 
            let state' = folder state p
            ps |> Seq.fold (fun s p -> foldPackets folder s p) state'
           

    let getBitList input = 
        input |> Seq.head 
        |> List.ofSeq
        |> List.map string
        |> List.map (fun s -> 
            let byte = Byte.Parse(s, NumberStyles.HexNumber)
            Convert.ToString(byte, 2).PadLeft(4, '0')
            |> List.ofSeq)
        |> List.concat

    let bitListToNumber (list: char list) =
        Convert.ToInt64(String.Join("", list), 2)

    let parsePacket bitList = 
        let rec parsePacket' bitList packetCount =
            let packetCount', lastPacket = match packetCount with
                                               | None -> None, false
                                               | Some x when x > 1L -> x - 1L |> Some , false
                                               | Some x -> None, true
            if List.forall (fun x -> x = '0') bitList || List.length bitList < 6 then
                [], []
            else
                // Parse header
                let version, rest = List.splitAt 3 bitList
                let typeId, rest' = rest |> List.splitAt 3
                let header = { 
                                Version = bitListToNumber version; 
                                TypeId = bitListToNumber typeId; 
                             }
                match header.TypeId with
                | 4L -> let chunks = rest' // Literal number
                                    |> List.chunkBySize 5 
                        let numberChunks = chunks
                                          |> List.takeWhile (fun c -> List.head c = '1')
                        let chunkCount = List.length numberChunks + 1
                        let lastChunk = [List.item (chunkCount - 1) chunks]
                        let packet = (header, List.append numberChunks lastChunk
                                             |> List.map (fun x -> List.tail x)
                                             |> List.concat 
                                             |> bitListToNumber)
                                             |> Packet.Literal
                        let rest'' = (List.skip (5 * chunkCount) rest')
                        if lastPacket then
                           (packet :: []), rest''
                        else 
                           let packets, rest''' = parsePacket' rest'' packetCount'
                           (packet :: packets, rest''')

                       
                | x when List.head rest' = '0'-> // Subpackets in number of bit
                    let subPackageLength = rest' |> List.skip 1 |> List.take 15 |> bitListToNumber |> int
                    let rest'' = rest' |> List.skip 16
                    let subBits, rest''' = rest'' |> List.splitAt subPackageLength
                    let packets, _ = parsePacket' subBits None
                    let packet = Packet.Operator (header, packets)
                    if lastPacket then 
                        (packet :: []), rest'''
                    else
                        let packets', rest'''' = parsePacket' rest''' packetCount'
                        (packet :: packets', rest'''')
                | x when List.head rest' = '1'-> // Subpackets in number of packages
                    let subPackageLength = rest' |> List.skip 1 |> List.take 11 |> bitListToNumber
                    let rest'' = rest' |> List.skip 12
                    let packets, rest''' = parsePacket' rest'' (Some subPackageLength)
                    let packet = Packet.Operator (header, packets)
                    if lastPacket then
                        (packet :: []), rest'''
                    else 
                        let packets', rest'''' = parsePacket' rest''' packetCount'
                        (packet :: packets', rest'''')
                | _ -> failwith "Ooop!"
                    

        parsePacket' bitList None

    let main (input :string seq) =
        
        let bitlist = getBitList input
        let packages, _ = parsePacket bitlist
        let sumVersion = 
            packages 
            |> List.head 
            |> foldPackets (fun state p ->
                match p with
                | Packet.Operator (h, _) -> h.Version + state
                | Packet.Literal (h, _) -> h.Version + state
             ) 0L

        let part1 = sumVersion

        let rec getValue packet = 
            match packet with 
            | Literal (_ , v) -> v
            | Operator ({ TypeId = 0L }, ps)  -> 
                ps |> Seq.sumBy (getValue)
            | Operator ({TypeId = 1L }, ps) -> 
                ps |> Seq.fold (fun s p -> s * getValue p) 1L
            | Operator ({TypeId = 2L }, ps) -> 
                ps |> Seq.minBy (getValue) |> getValue
            | Operator ({TypeId = 3L }, ps) -> 
                ps |> Seq.maxBy (getValue) |> getValue
            | Operator ({TypeId = 5L }, ps) -> 
                let values = Seq.map getValue ps
                if Seq.item 0 values > Seq.item 1 values then
                    1L
                else 
                    0L
            | Operator ({TypeId = 6L }, ps) -> 
                let values = Seq.map getValue ps
                if Seq.item 0 values < Seq.item 1 values then
                    1L
                else 
                    0L
            | Operator ({TypeId = 7L }, ps) -> 
                let values = Seq.map getValue ps
                if Seq.item 0 values = Seq.item 1 values then
                    1L
                else 
                    0L
            | _ -> failwith "Expression type fail"


     
        let part2 = getValue (List.head packages)
                    
        string part1, string part2