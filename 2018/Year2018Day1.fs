module Year2018Day1

    let main (input: seq<string>) =
        let part1 = 
            input 
            |> Seq.map int 
            |> Seq.sum
        
        let part2 =
            let rec part2' seenValues frequency currentList completeList =
                if Set.contains frequency seenValues then
                    frequency
                else
                    match currentList with
                    | x::xs -> 
                        part2' (Set.add frequency seenValues) (frequency + x) xs completeList
                    | [] -> part2' seenValues frequency completeList completeList
            part2' (Set.empty) 0 [] (Seq.map int input |> List.ofSeq) 
               
        (part1, part2)
       

    