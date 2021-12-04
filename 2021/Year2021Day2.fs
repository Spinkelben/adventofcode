namespace Year2021

module Day2 =
   type Movement = int * int

    let main (input :string seq) =
        let readLine (line:string) = 
            let split = line.Split(' ')
            match split with
            | [|"forward"; value|]  -> Some (int value, 0)
            | [|"up"; value|]       -> Some (0, -int value) 
            | [|"down"; value|]     -> Some (0, int value) 
            | _ -> None

        let addPosition (position1 :Movement) (position2 :Movement) =
            match position1, position2 with
            | (distance1, depth1), (distance2, depth2) -> distance1 + distance2, depth1 + depth2

        let instructions = input
                           |> Seq.map readLine
                           
        let part1 = instructions 
                    |> Seq.reduce (Option.map2 addPosition)
                    |> Option.map (fun (distance, depth) -> distance * depth)
                    |> Option.get

        let positionAndAimFolder (distance, depth, aim) (movement, aimChange) =
            distance + movement, depth + movement * aim, aim + aimChange  

        let part2 = instructions 
                    |> Seq.fold (Option.map2 positionAndAimFolder) (Some (0, 0, 0))
                    |> Option.map (fun (distance, depth, _) -> distance * depth)
                    |> Option.get

        string part1, string part2