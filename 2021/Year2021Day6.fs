namespace Year2021

open System

module Day6 =
    let splitString (seperator : string) (s : string) =
        s.Split seperator

    let main (input :string seq) =
        let updateFish fishList =
            List.collect (fun f -> if f = 0 then [6; 8] else [f - 1]) fishList

        let rec simulateFish updater stepCount fishList = 
            if stepCount = 0 then
                fishList
            else 
                simulateFish updater (stepCount - 1) (updater fishList)

        let readFish input = 
            Seq.head input 
            |> splitString "," 
            |> List.ofArray 
            |> List.map int

        // Keeping part 1 implementatiopn as I really like how neatly it maps to the problem
        let part1 = readFish input 
                    |> simulateFish updateFish 80
                    |> List.length

        // Idea: Keep count of how many fish are at each stage, rather than tracking each fish idividually
        let updateFishPart2 fishMap =
            let fishFolder newMap stage numFish =
                match stage with
                | 0 -> newMap 
                        |> Map.add 8 numFish 
                        |>  if Map.containsKey 6 newMap then
                               Map.add 6 (Map.find 6 newMap + numFish) 
                            else 
                               Map.add 6 numFish 
                | 1 -> Map.add 0 numFish newMap
                | 2 -> Map.add 1 numFish newMap
                | 3 -> Map.add 2 numFish newMap
                | 4 -> Map.add 3 numFish newMap
                | 5 -> Map.add 4 numFish newMap
                | 6 -> Map.add 5 numFish newMap
                | 7 -> if Map.containsKey 6 newMap then 
                           Map.add 6 (Map.find 6 newMap + numFish) newMap
                       else
                           Map.add 6 numFish newMap
                | 8 -> Map.add 7 numFish newMap
                | _ -> newMap
            Map.fold fishFolder Map.empty<int, int64> fishMap

        
        let part2 = readFish input 
                    |> List.countBy id
                    |> List.map (fun (stage, count) -> stage, int64 count)
                    |> Map.ofList
                    |> simulateFish updateFishPart2 256
                    |> Map.fold (fun count _ value-> count + value) 0L
                    

        string part1, string part2