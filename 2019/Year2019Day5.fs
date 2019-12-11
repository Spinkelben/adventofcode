module Year2019Day5

open IntCodeComputer

let formatInput (input : string seq) =
    let result = 
        (Seq.head input).Split(',')
        |> Array.map int64
    result


let main input =
    printfn "Computer part 1:"
    let part1 =
         let program = formatInput input
         let _, (endProgram, _, _) = executeProgram program [1L] None None None
         endProgram.[0].ToString()

    printfn "Computer part 2:"
    let part2 = 
        let program = formatInput input
        let _, (endProgram, _, _) = executeProgram program [5L] None None None
        endProgram.[0].ToString()

    part1, part2