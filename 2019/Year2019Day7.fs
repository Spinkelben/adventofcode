module Year2019Day7
open IntCodeComputer

let permutationGeneratror fullList =
    let rec permutationGeneratror' inputList = 
        if Seq.length inputList = 0 then
            seq { [] }
        else
            seq { 
                for i in inputList do
                    yield! seq { 
                        for res in permutationGeneratror' (Seq.filter (fun x -> x <> i) inputList) ->
                            i :: res
                    }
            }

    permutationGeneratror' fullList

let formatInput (input : string seq) =
    let result = 
        (Seq.head input).Split(',')
        |> Array.map int
    result

let main input =
    let ampifierPipeLine program (settings :seq<int>) =
        let rec ampifierPipeLine' curPrograms lastOutput =
            let newPrograms, output, isTerminated =
                List.fold (fun (nextPrograms, prevOutput, prevIsTerminated) (program, pCounter) ->
                    let (output, resultPCounter, isTerminated), resultProgram =
                        executeProgram program prevOutput (Some pCounter)
                    ((resultProgram, resultPCounter) :: nextPrograms), output, isTerminated)
                    ([], lastOutput, false)
                    curPrograms
            if isTerminated then
                output
            else 
                ampifierPipeLine' (List.rev newPrograms) output

                

        let output, isTerminated, newPrograms =
            (Seq.fold (fun (state : int list * bool * (int array * int) list) i -> 
                let nextInput, _, nextPrograms = state
                let (output, pCounter, isTerminated), currProgram = 
                    executeProgram program [ i; nextInput.[0] ] None
                output, isTerminated, ((currProgram, pCounter) :: nextPrograms))
                ([ 0 ], true, [])
                settings)
        if isTerminated then
            output
        else
            (ampifierPipeLine' (List.rev newPrograms) output)
        

    let part1 =
        let program =
            formatInput input
        let sequences = 
            permutationGeneratror (seq { 0; 1; 2; 3; 4; })
        sequences
        |> Seq.map (fun modeSequence -> 
            ampifierPipeLine program modeSequence)
        |> Seq.max

            

    let part2 =
        let program =
            formatInput input
        let sequences = 
            permutationGeneratror (seq { 5; 6; 7; 8; 9; })
        sequences
        |> Seq.map (fun modeSequence -> 
            ampifierPipeLine program modeSequence)
        |> Seq.max

    part1.[0].ToString(), part2.[0].ToString()


