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

        let computePolymerScore elements =
            (elements |> List.maxBy snd |> snd) - (elements |> List.minBy snd |> snd) 

        let elementCounts = polymerize map template 10
                            |> List.countBy id

        let part1 = computePolymerScore elementCounts

        let rec expandPair pairCache pairMap (x,y) steps =
            match Map.tryFind ((x, y), steps) pairCache with
            | Some x -> x, pairCache
            | None ->   if steps = 0 then 
                            [x; y], pairCache 
                        elif steps = 1 then
                            let result = [x; Map.find (x,y) pairMap; y]
                            result, Map.add ((x,y), steps) result pairCache
                        else 
                            let middle = Map.find (x,y) pairMap
                            let subResult1, cache' = expandPair pairCache pairMap (x, middle) (steps - 1)
                            let subResult2, cache'' = expandPair cache' pairMap (middle, y) (steps - 1)
                            let result = List.concat [[x]; subResult1.[1..^1]; [middle]; subResult2.[1..^1]; [y] ]
                            result, Map.add ((x,y), steps) result cache''
                            


        let part2 = ""
                    
        string part1, string part2