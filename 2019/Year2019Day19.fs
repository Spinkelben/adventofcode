module Year2019Day19

let main (input : string seq) =
    let program = (Seq.head input).Split(",") |> Array.map int64

    let part1 =
        let coordinates = seq { for x in 0L .. 49L do
                                yield! seq { for y in 0L ..49L -> [x; y] }
                            }
        Seq.map (fun input -> 
                let (output, _, _), _ = IntCodeComputer.executeProgram program input None None None
                List.head output
            ) coordinates
        |> Seq.sum


    let part2 =

        let rec calcLine xStart xEnd y (xMap : Map<int, int>) =
            let coords = seq { for x in xStart .. xEnd do [int64 x; int64 y] }
            let output = 
                coords 
                |> Seq.map (fun input -> 
                            let (output, _, _), _ = IntCodeComputer.executeProgram program input None None None
                            List.head input, (List.head output)) 
                |> List.ofSeq
            let width = output |> List.map (fun (_, r) -> r) |> List.sum
            let active = output |> List.filter (fun (_, r) -> r = 1L)
            let xs = List.map (fun (x, _) -> int x) active
            if xs.Length = 0 then
                calcLine xStart (xEnd + 2) (y + 1) xMap
            else 
                let minX = List.min xs
                let maxX = List.max xs
                let topY = y - 99
                if (xMap.ContainsKey topY) && (Map.find topY xMap) - minX = 99 then
                        minX, topY
                else
                
                    let nextStartX = minX - 1
                    let nextEndX = maxX + 4
                    calcLine nextStartX nextEndX (y + 1) (xMap.Add (y, maxX))


        let result = calcLine 0 10 0 Map.empty
        (fst result * 10000) + (snd result)

    part1.ToString(), part2.ToString()