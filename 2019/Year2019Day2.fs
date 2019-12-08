module Year2019Day2

open IntCodeComputer

let formatInput (input : string seq) =
    let result = 
        (Seq.head input).Split(',')
        |> Array.map int
    result

let main (input :string seq) =
    let part1 =
        let program = formatInput input
        Array.set program 1 12
        Array.set program 2 2
        let _, endProgram = executeProgram program [0] None
        endProgram.[0].ToString()

    let part2 =
        let runAndReturnInput program noun verb =
            Array.set program 1 noun
            Array.set program 2 verb
            let _, result = executeProgram program [0] None
            result.[0]

        let originalProgram = formatInput input
        let inputSequence = 
            seq { 
                for noun in { 0 .. 146 } do
                    for verb in { 0 .. 146 } do
                        (noun, verb)
            }
        inputSequence
        |> Seq.filter (fun (noun, verb) -> runAndReturnInput originalProgram noun verb = 19690720) 
        |> Seq.map (fun (noun, verb) -> 100 * noun + verb)
        |> Seq.head

    part1, part2.ToString()
