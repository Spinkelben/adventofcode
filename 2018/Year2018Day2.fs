module Year2018Day2

let private countUnique s =
    s 
    |> Seq.groupBy (fun c -> c)
    |> Map.ofSeq
    |> Map.map (fun k v -> Seq.length v)

let countElementFrequency inputString count =
    let counts = countUnique inputString
    counts 
    |> Map.exists (fun k v -> v = count)

let main (input :string seq) =
    let countTwo inString =
        if countElementFrequency inString 2 then
            1
        else
            0

    let countThree inString =
        if countElementFrequency inString 3 then
            1
        else
            0

    let commonString a b =
        Seq.zip a b
            |> Seq.filter (fun (c1, c2) -> c1 = c2)
            |> Seq.map (fun (c1, c2) -> c1)

    let part1 =
        let counts = 
            input
            |> Seq.fold (fun (a, b) elem -> 
                (countTwo elem + a, countThree elem + b)) (0, 0)
        ((fst counts) * (snd counts)).ToString()

    let part2 =
        let rec part2' stringA (sequence :string list) = 
            match sequence with
            | x :: xs -> 
                let result = commonString stringA x
                if Seq.length result = String.length stringA - 1 then
                    Some result
                else 
                    part2' stringA xs
            | [] -> None
            
        let rec searchSequences sequenceA sequenceB =
            match sequenceA with
            | x :: xs -> 
                match part2' x sequenceB with 
                | Some s -> Some s
                | None -> searchSequences xs sequenceB
            | [] -> None
        
        match searchSequences (List.ofSeq input) (List.ofSeq input) with
        | Some result -> 
            result 
            |> Seq.map string 
            |> String.concat ""
        | None ->  "FAIL"
        
    part1, part2

