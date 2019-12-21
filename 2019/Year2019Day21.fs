module Year2019Day21

open System

let main (input : string seq) =
    let program = (Seq.head input).Split(",") |> Array.map int64
    let part1 = 
        let springBotProgram = 
            "NOT A T\nOR T J\nNOT B T\nOR T J\nNOT C T\nOR T J\nAND D J\nWALK\n" 
            |> Seq.map int64 |> List.ofSeq 
        let (output, _, _), _ =
            IntCodeComputer.executeProgram program springBotProgram None None None

        let outASCII = String.Join("", 
                                        output 
                                        |> List.filter (fun v -> v < 256L) 
                                        |> List.map (fun c -> (char c).ToString())
                                        |> Array.ofList)
        let outRest = output |> List.filter (fun v -> v >= 256L)
        printfn "Output:\n%s\n" outASCII
        List.iter (printfn "Values: %i") outRest
        outASCII

    let part2 =
        let springBotProgram = 
            "OR A J\nAND B J\nAND C J\nNOT J J\nAND D J\nOR H T\nOR E T\nAND T J\nRUN\n" 
            |> Seq.map int64 |> List.ofSeq 
        let (output, _, _), _ =
            IntCodeComputer.executeProgram program springBotProgram None None None

        let outASCII = String.Join("", 
                                        output 
                                        |> List.filter (fun v -> v < 256L) 
                                        |> List.map (fun c -> (char c).ToString())
                                        |> Array.ofList)
        let outRest = output |> List.filter (fun v -> v >= 256L)
        printfn "Output:\n%s\n" outASCII
        List.iter (printfn "Values: %i") outRest
        outASCII

    part1, part2