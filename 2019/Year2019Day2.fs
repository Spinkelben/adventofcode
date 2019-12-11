module Year2019Day2

open IntCodeComputer

let formatInput (input : string seq) =
    let result = 
        (Seq.head input).Split(',')
        |> Array.map int64
    result

let main (input :string seq) =
    let part1 =
        let program = formatInput input
        Array.set program 1 12L
        Array.set program 2 2L
        let _, (endProgram, _, _) = executeProgram program [0L] None None None
        endProgram.[0].ToString()

    let part2 =
        let runAndReturnInput program noun verb =
            Array.set program 1 noun
            Array.set program 2 verb
            let _, (result, _, _) = executeProgram program [0L] None None None
            result.[0]

        let originalProgram = formatInput input
        let inputSequence = 
            seq { 
                for noun in { 0L .. 146L } do
                    for verb in { 0L .. 146L } do
                        (noun, verb)
            }
        inputSequence
        |> Seq.filter (fun (noun, verb) -> runAndReturnInput originalProgram noun verb = 19690720L) 
        |> Seq.map (fun (noun, verb) -> 100L * noun + verb)
        |> Seq.head

    part1, part2.ToString()
