module Year2019Day16

let patternAtIndex elementNumber index =
    let digits = [ 0; 1; 0; -1; ]
    let i = index % (elementNumber * 4)
    digits.Item (i / elementNumber)

let oneFftPhase input =
    let oneFftdigit input elementNumber =
        input
        |> Seq.mapi (fun idx value -> (patternAtIndex elementNumber (idx + 1)) * value)
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

let takeNFirst n input =
    Seq.take n input
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


let doIterationsBottomUp numIterations input =
    let rec doIterationsBottomUp' iterationNum currentInput =
        if iterationNum = numIterations then
            currentInput
        else
            let nextInput = 
                List.scanBack (fun v state ->
                     (v + state) % 10)
                    currentInput
                    0
            doIterationsBottomUp' (iterationNum + 1) nextInput

    doIterationsBottomUp' 0 input



let main input =
    let input = parseInput input

    let part1 =
        fft input 100
        |> takeNFirst 8

    let part2 =
        let fullInputLength = (input.Length * 10000)
        let offset = int (takeNFirst 7 input)
        let inputLength = fullInputLength - offset
        let skipLength = offset % input.Length
        let repeatCount = inputLength / input.Length
        let offsetInput = [ yield! (List.skip skipLength input);
                            for i in 1 .. repeatCount do yield! input]
        if not (patternAtIndex offset offset = 1) then
            failwith "Offset not big enough"

        let result = doIterationsBottomUp 100 offsetInput
        takeNFirst 8 result

    part1, part2