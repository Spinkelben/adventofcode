module Year2019Day3

type Direction = 
    | Up 
    | Down 
    | Left 
    | Right

let generateLine startPoint direction steps =
    match direction with
    | Direction.Up ->
        seq { for i in (snd startPoint + 1) .. ((snd startPoint) + steps)
            -> fst startPoint, i }
    | Direction.Down ->
        seq { for i in (snd startPoint - 1) .. -1 .. ((snd startPoint) - steps)
            -> fst startPoint, i }
    | Direction.Left -> 
        seq { for i in (fst startPoint - 1) .. -1 .. ((fst startPoint) - steps) 
            -> i, snd startPoint }
    | Direction.Right ->
        seq { for i in (fst startPoint + 1) .. ((fst startPoint) + steps)
            -> i, snd startPoint }

let parseInstruction instruction =
    let steps = int ((new string (Array.ofSeq instruction)).Substring(1))
    match Seq.head instruction with
        | 'U' -> Some Up, steps
        | 'D' -> Some Down, steps
        | 'L' -> Some Left, steps
        | 'R' -> Some Right, steps
        | _ -> None, 0

let generateCoordinateSequence turnList =
    let rec generatePath startPoint sequenceAcc turns =
            let turn = Seq.head turns
            let line = generateLine startPoint (fst turn) (snd turn)
            if Seq.length turns > 1 then
                let endOfLine = Seq.last line
                generatePath endOfLine (Seq.append sequenceAcc line) (Seq.tail turns)
            else
                Seq.append sequenceAcc line
         
    let parsedTurnList = 
        turnList
        |> Seq.map parseInstruction
        |> Seq.filter (fun (direction, _) -> direction.IsSome)
        |> Seq.map (fun (direction, steps) -> direction.Value, steps)

    generatePath (0, 0) (seq { (0, 0) }) parsedTurnList


let manhattenDistance a b =
    ((abs (fst a - fst b)) + (abs (snd a - snd b)))

let swapElements (a, b) =
    b, a

let main (input : string seq) =
    let part1 =
        let wire1 = (Seq.head input).Split(',')
        let wire2 = (Seq.last input).Split(',')
        let coords1 = generateCoordinateSequence wire1 |> Set.ofSeq
        let coords2 = generateCoordinateSequence wire2 |> Set.ofSeq
        Set.intersect coords1 coords2
        |> Set.remove (0, 0)
        |> Set.map (fun p -> manhattenDistance p (0, 0))
        |> Set.minElement

    let part2 =
        let wire1 = (Seq.head input).Split(',')
        let wire2 = (Seq.last input).Split(',')
        let coords1 = generateCoordinateSequence wire1 
        let coords2 = generateCoordinateSequence wire2 
        let intersections = 
            Set.intersect (Set.ofSeq coords1) (Set.ofSeq coords2)
            |> Set.remove (0, 0)

        let wire1DelayMap =
            coords1
            |> Seq.indexed
            |> Seq.map swapElements
            |> Map.ofSeq

        let wire2DelayMap =
            coords2
            |> Seq.indexed
            |> Seq.map swapElements
            |> Map.ofSeq

        intersections
        |> Seq.map (fun i -> wire1DelayMap.[i] + wire2DelayMap.[i])
        |> Seq.min

    part1.ToString(), part2.ToString()