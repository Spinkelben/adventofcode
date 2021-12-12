namespace Year2021

open System

module Day12 =
    
    let isAllowedPart2 path cave =
        match cave with
        | "start" -> false // Can only visit start once
        | x when x <= "Z" -> true // can always visit big caves
        | x when List.contains x path |> not -> true // Can visit small caves at least once
        | _ -> path // Can visit a single small cave twice
               |> List.where (fun dst -> dst > "Z") 
               |> List.countBy id 
               |> List.exists (fun (_, count) -> count = 2)
               |> not


    let main (input :string seq) =
        let parseCaves input = 
            input 
            |> Seq.map (fun (s: string) -> s.Split('-'))
            |> Seq.collect (fun a -> [ (a.[0], a.[1]); (a.[1], a.[0])])
            |> Seq.groupBy fst
            |> Seq.map (fun (s, dsts) -> (s, Seq.map snd dsts |> List.ofSeq))
            |> Map.ofSeq

        let rec findPaths isAllowedMove caveMap current path =
            let path' = (current :: path)
            if current = "end" then
                [path']
            else
                Map.find current caveMap
                    |> List.where (isAllowedMove path')
                    |> List.collect (fun next -> findPaths isAllowedMove caveMap next path')

        let isAllowedPart1 path cave =
             cave <= "Z" || not (List.contains cave path)

        let caveMap = parseCaves input
        let part1 = findPaths isAllowedPart1 caveMap "start" []
                    |> List.length

        let part2Paths = findPaths isAllowedPart2 caveMap "start" []
        let part2 = part2Paths |> List.length 
        
        string part1, string part2