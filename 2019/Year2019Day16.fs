module Year2019Day16

let pattern elementNumber =
    let digits = [ 0; 1; 0; -1; ]
    Seq.initInfinite (fun (index) -> 
        let i = index % (elementNumber * 4)
        digits.Item (i / elementNumber))

let oneFftPhase input =
    let oneFftdigit input elementNumber =
        Seq.zip input (Seq.skip 1 (pattern elementNumber))
        |> Seq.map (fun (i, j) -> i * j)
        |> Seq.sum
    let _, result =
        input |> Seq.fold (fun (count, result) _ ->
            let out = oneFftdigit input count 
            count + 1, abs (out % 10) :: result)
            (1, [])
    List.rev result

let fft input numPhases =
    let rec fft' input phaseNumber =
        if phaseNumber > numPhases then
            input
        else
            fft' (oneFftPhase input) (phaseNumber + 1)
    fft' input 1

let parseInput (input : string seq) =
    Seq.head input
    |> Seq.map (fun c -> int (c.ToString()))
    |> List.ofSeq

let take8First input =
    Seq.take 8 input
    |> Seq.map (fun x -> x.ToString())
    |> Seq.fold (fun acc x -> acc + x) ""

let gcd a b =
    let rec gcd' a b =
        if b = 0 then 
            a
        else
            gcd' b (a % b)
    
    let aabs, babs = abs(a), abs(b)
    if aabs > babs then
        gcd' aabs babs
    else 
        gcd' babs aabs

let repeatLength patternLength inputSeqLength =
    patternLength / (gcd patternLength inputSeqLength)

let main input =
    let input = parseInput input

    let part1 =
        fft input 100
        |> take8First

    let part2 =
        ""

    part1, part2