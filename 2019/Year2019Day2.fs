module Year2019Day2

let formatInput (input : string seq) =
    let result = 
        (Seq.head input).Split(',')
        |> Array.map int
    result

let executeInstruction (program : int array) pCounter =
    let infixOperation op =
        let idx1 = program.[pCounter + 1]
        let idx2 = program.[pCounter + 2]
        let resultIdx = program.[pCounter + 3]
        Array.set program resultIdx (op (program.[idx1]) (program.[idx2]))
        program

    match program.[pCounter] with
    | 1 -> infixOperation (fun x y -> x + y), pCounter + 4, 1
    | 2 -> infixOperation (fun x y -> x * y), pCounter + 4, 2
    | 99 -> program, pCounter, 99
    | _ ->  failwith "Program Parsing Error!"

let executeProgram program noun verb =
    Array.set program 1 noun
    Array.set program 2 verb
    let rec executeProgram' program pCounter lastOpCode =
        if lastOpCode = 99 then
            program
        else
            let newProgram, newpCounter, opCode = executeInstruction program pCounter
            executeProgram' newProgram newpCounter opCode

    executeProgram' program 0 0

let main (input :string seq) =
    let part1 =
        let program = formatInput input
        let endProgram = executeProgram program 12 2
        endProgram.[0].ToString()

    let part2 =
        let runAndReturnInput program noun verb =
            let pCopy = Array.copy program
            let result = executeProgram pCopy noun verb
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
