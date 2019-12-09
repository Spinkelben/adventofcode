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
        |> Array.map int64
    result

let main input =
    let ampifierPipeLine program (settings :seq<int64>) =
        let rec ampifierPipeLine' curPrograms lastOutput =
            let newPrograms, output, isTerminated =
                List.fold (fun (nextPrograms, prevOutput, prevIsTerminated) (program, pCounter) ->
                    let (output, resultPCounter, isTerminated), (resultProgram, _) =
                        executeProgram program prevOutput (Some pCounter) None None
                    ((resultProgram, resultPCounter) :: nextPrograms), output, isTerminated)
                    ([], lastOutput, false)
                    curPrograms
            if isTerminated then
                output
            else 
                ampifierPipeLine' (List.rev newPrograms) output

                

        let output, isTerminated, newPrograms =
            (Seq.fold (fun (state : int64 list * bool * (int64 array * int) list) i -> 
                let nextInput, _, nextPrograms = state
                let (output, pCounter, isTerminated), (currProgram, _) = 
                    executeProgram program [ i; nextInput.[0] ] None None None
                output, isTerminated, ((currProgram, pCounter) :: nextPrograms))
                ([ 0L ], true, [])
                settings)
        if isTerminated then
            output
        else
            (ampifierPipeLine' (List.rev newPrograms) output)
        

    let part1 =
        let program =
            formatInput input
        let sequences = 
            permutationGeneratror (seq { 0L; 1L; 2L; 3L; 4L; })
        sequences
        |> Seq.map (fun modeSequence -> 
            ampifierPipeLine program modeSequence)
        |> Seq.max

            

    let part2 =
        let program =
            formatInput input
        let sequences = 
            permutationGeneratror (seq { 5L; 6L; 7L; 8L; 9L; })
        sequences
        |> Seq.map (fun modeSequence -> 
            ampifierPipeLine program modeSequence)
        |> Seq.max

    part1.[0].ToString(), part2.[0].ToString()


