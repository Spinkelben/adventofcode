module Year2019Day5

open IntCodeComputer

let formatInput (input : string seq) =
    let result = 
        (Seq.head input).Split(',')
        |> Array.map int
    result


let main input =
    printfn "Computer part 1:"
    let part1 =
         let program = formatInput input
         let endProgram = executeProgram program 1
         endProgram.[0].ToString()

    printfn "Computer part 2:"
    let part2 = 
        let program = formatInput input
        let endProgram = executeProgram program 5
        endProgram.[0].ToString()

    part1, part2