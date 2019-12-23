module Year2019Day23

type Message = { destination : int; x : int64; y : int64 }

let rec getMessages list output =
    match list with
    | [] -> output
    | dest :: x :: y :: rest -> getMessages rest ( {destination = (int dest); x = x; y = y } :: output)
    | _ -> failwith ("failed to parse messages, remaining list of length: " + list.Length.ToString())

let rec runNetwork computers messages targetDest =
    let nextComputers = 
        computers 
        |> List.mapi (fun index ((_, pCounter, isTerminated), (program, memory, baseOffset)) ->
            let input = 
                if Map.containsKey index messages then
                    let message = Map.find index messages
                    message |> List.map (fun m -> [m.x; m.y]) |> List.concat
                else 
                    [-1L]
            IntCodeComputer.executeProgram program input (Some pCounter) (Some memory) (Some baseOffset))
    let nextMessages = 
        [ for (o, _, _), _ in computers do yield! (List.rev (getMessages o []))] |> List.groupBy (fun t -> t.destination) |> Map.ofList
    if nextMessages.ContainsKey targetDest then
        (nextMessages.Item targetDest).Head.y
    else
        runNetwork nextComputers nextMessages targetDest

let rec runNetworkWithNat computers messages targetDest lastNat =
    let nextComputers = 
        computers 
        |> List.mapi (fun index ((_, pCounter, isTerminated), (program, memory, baseOffset)) ->
            let input = 
                if Map.containsKey index messages then
                    let message = Map.find index messages
                    message |> List.map (fun m -> [m.x; m.y]) |> List.concat
                else 
                    [-1L]
            IntCodeComputer.executeProgram program input (Some pCounter) (Some memory) (Some baseOffset))
    let nextMessages = 
        [ for (o, _, _), _ in computers do yield! List.rev (getMessages o [])] |> List.groupBy (fun t -> t.destination) |> Map.ofList
    if nextMessages.ContainsKey targetDest then
        let natPackage = (List.rev (nextMessages.Item targetDest)).Head
        if (nextMessages.Remove targetDest).Count = 0 then
            if natPackage.y = lastNat.y then
                natPackage.y
            else
                runNetworkWithNat nextComputers (Map.ofList [0, [natPackage]]) targetDest natPackage
        else
            runNetworkWithNat nextComputers nextMessages targetDest lastNat

    else
        runNetworkWithNat nextComputers nextMessages targetDest lastNat
        

let main (input : string seq) =
    let program = (Seq.head input).Split(",") |> Array.map int64
    let computers = [for i in 0L .. 49L do IntCodeComputer.executeProgram program [i] None None None]
    let messages = [ for (o, _, _), _ in computers do yield! (getMessages o [])] |> List.groupBy (fun t -> t.destination) |> Map.ofList
         
    let part1 =
        runNetwork computers Map.empty 255

    let part2 =                      
        runNetworkWithNat computers Map.empty 255 { destination = -1; x = -1L; y = -100L }

    part1.ToString(), part2.ToString()