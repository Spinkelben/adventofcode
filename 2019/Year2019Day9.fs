module Year2019Day9

let main (input :string seq) =
    let part1 =
        let program =
            (Seq.head input).Split(",")
            |> Array.map int64
        let (output, _, _), _ = IntCodeComputer.executeProgram program [1L] None None None
        List.head output

    let part2 =
        let program =
            (Seq.head input).Split(",")
            |> Array.map int64
        let (output, _, _), _ = IntCodeComputer.executeProgram program [2L] None None None
        List.head output

    part1.ToString(), part2.ToString()