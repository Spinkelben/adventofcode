namespace Year2021

open System

module Day14 =
    let splitString (split: string) (string : string) =
        string.Split([|split|], StringSplitOptions.RemoveEmptyEntries)

    let main (input :string seq) =
        let parseInput input = 
            let template = Seq.head input |> List.ofSeq
            let pairMap = Seq.skip 2 input
                          |> Seq.map (splitString " -> ")
                          |> Seq.choose (fun s -> match s with
                                                  | [|x; y|] -> Some ((x.[0], x.[1]), y.[0])
                                                  | _ -> None)
                          |> Map.ofSeq
            template, pairMap

        let insertPairs map template =
            (template |> List.pairwise
            |> List.collect (fun (x,y) -> [x; Map.find (x, y) map;])
            |> List.append) [ List.last template ]

        let rec polymerize map template step =
            if step <= 0 then
                template
            else 
                polymerize map (insertPairs map template) (step - 1)
                
        let template, map = parseInput input

        let computePolymerScore (elements : ('a * int64) list) =
            (elements |> List.maxBy snd |> snd) - (elements |> List.minBy snd |> snd) 

        let elementCounts = polymerize map template 10
                            |> List.countBy id
                            |> List.map (fun (x,y) -> x, int64 y)

        let part1 = computePolymerScore elementCounts

        let updateCount toAdd existing = 
            match existing with
            | Some c -> c + toAdd |> Some
            | None -> Some toAdd

        let expandPairs map first pairCount =
            let newPairs (x,y) = 
                let pair1 = (x, Map.find (x,y) map)
                let pair2 = (Map.find (x,y) map, y)
                pair1, pair2
            let pairCount' = pairCount |> Map.fold (fun newCounts (x, y) count-> 
                let pair1, pair2 = newPairs (x,y)
                newCounts |> Map.change pair1 (updateCount count)
                |> Map.change pair2 (updateCount count)) Map.empty
            let first', next = newPairs first
            first', pairCount' |> Map.change next (updateCount 1L)

        let polymerize2 map template steps =
            let firstPair = List.pairwise template |> List.head
            let pairCounts  = List.pairwise template 
                                |> List.tail
                                |> List.countBy id 
                                |> List.map (fun (x, y) -> x, int64 y)
                                |> Map.ofList
            let rec polymerize2' firstPair pairCounts steps =
                if steps <= 0 then
                    firstPair, pairCounts
                else
                    let first', pairCounts' = expandPairs map firstPair pairCounts
                    polymerize2' first' pairCounts' (steps - 1)

            polymerize2' firstPair pairCounts steps

        let sumSymbols (f1, f2) pairCounts = 
            pairCounts 
            |> Map.fold (fun acc (_, y) count -> acc |> Map.change y (updateCount count)) Map.empty
            |> Map.change f1 (updateCount 1L) 
            |> Map.change f2 (updateCount 1L)
            |> Map.toList

        let f,c = polymerize2 map template 40 
        let part2 = sumSymbols f c |> computePolymerScore
                    
        string part1, string part2