module Year2019Day4

let rec hasDouble digits =
    match digits with
    | d :: ds when digits.Length > 1 && d = List.head ds -> true
    | d :: ds ->  hasDouble ds
    | [] -> false

let hasDoublePart2 digits =
    let rec hasDoublePart2' lastdigit digits =
        match digits with
        | d :: ds when digits.Length > 2 -> 
            let nextD = ds.Item 1
            if d = ds.Head && nextD <> d && lastdigit <> d then
                true
            else
                hasDoublePart2' d ds
        | d :: ds when digits.Length = 2 ->   
            if d = ds.Head && d <> lastdigit then
                true
            else 
                hasDoublePart2' d ds
        | d :: ds ->  hasDoublePart2' d ds
        | [] -> false

    hasDoublePart2' -1 digits

let isIncreasing digits =
    let rec isIncreasing' acc digits =
        match digits with
        | d :: ds ->
            if d >= acc then
                isIncreasing' d ds
            else
                false
        | [] -> true
    isIncreasing' 0 digits

let isValid digits =
    (hasDouble digits) && (isIncreasing digits)

let isValidPart2 digits =
    (isIncreasing digits) && (hasDoublePart2 digits)

let parseInput (input :string seq) =
    let range = 
        (Seq.head input).Split('-')
        |> Array.map int
    range.[0], range.[1]

let main input =
    let range = 
        parseInput input

    let generateSequence =
        seq { for i in (fst range) .. (snd range) -> i}
        |> Seq.map (fun d -> 
            (d.ToString())
            |> Seq.map int
            |> List.ofSeq )

    let part1 =
        generateSequence
        |> Seq.filter isValid
        |> Seq.length

    let part2 =
        generateSequence
        |> Seq.filter isValidPart2
        |> Seq.length

    part1.ToString(), part2.ToString()