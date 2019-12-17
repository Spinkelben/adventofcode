module Year2019Day17

let generateWindows (inString : string) = 
    let lines = inString.Split("\n")
    seq {
        for i in 0 .. 1 .. lines.Length - 3 do
            i, Seq.zip3 
                (Seq.windowed 3 (lines.[i]))
                (Seq.windowed 3 (lines.[i + 1]))
                (Seq.windowed 3 (lines.[i + 2]))
    }


let detectIntersection (window : int * seq<char[] * char[] * char[]>) =
    let yIndex, window = window
    Seq.mapi (fun xIndex w -> 
        match w with
        | [|_;'#';_|], [|'#';'#';'#'|],[|_;'#';_|] ->
            (xIndex + 1, yIndex + 1), true
        | _ -> (xIndex + 1, yIndex + 1), false
        ) window
    |> Seq.filter (fun (index, isIntersection) -> 
        if isIntersection then
            true
        else
            false)

let calculateIntersections stringInput =
    let windows = generateWindows stringInput
    let result = 
        Seq.map detectIntersection windows 
        |> Seq.concat
        |> Seq.map (fun ((x, y), _) -> x * y)
        |> Seq.sum
    result

let main (input : string seq) =
    let program = 
        (Seq.head input).Split(",")
        |> Array.map int64

    let part1 = 
        let (output, _, _), _ = IntCodeComputer.executeProgram program [] None None None
        let outputString =
            output
            |> List.map char 
            |> List.map (fun v -> v.ToString())
            |> List.fold (fun acc v -> acc + v) ""

        printfn "\n%s" outputString
        calculateIntersections outputString


    let part2 = 
        let programString = "A,B,A,B,C,A,C,A,C,B\nR,12,L,8,L,4,L,4\nL,8,R,6,L,6\nL,8,L,4,R,12,L,6,L,4\nn\n"
        let programInput = Seq.map int64 programString |> List.ofSeq
        Array.set program 0 2L
        let (output, _, _), _ = IntCodeComputer.executeProgram program programInput None None None
        (List.rev output).Head

    part1.ToString(), part2.ToString()