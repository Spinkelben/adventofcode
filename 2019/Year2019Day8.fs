module Year2019Day8

type Pixel =
    | White
    | Black
    | Transparent

let getPixelSequence input = 
    (Seq.head input)
    |> Seq.map (fun c -> 
        match c with
        | '0' -> Black
        | '1' -> White
        | '2' -> Transparent
        | _   -> failwith "Unknown color")
    
let rec splitToLayers inSequence layerLength =
    let rec splitToLayers' inSequence layers =
        if Seq.length inSequence = 0 then
            layers
        else 
            splitToLayers' (Seq.skip layerLength inSequence) (Seq.take layerLength inSequence :: layers)
    splitToLayers' inSequence []
    |> List.rev

let stackLayers baseLayer nextLayer =
    Seq.zip baseLayer nextLayer
        |> Seq.map (fun (b, n) -> 
            match n with
            | Transparent -> b
            | White -> n
            | Black -> n)

let rec makePicture picture remainingLayers =
    match remainingLayers with
    | l :: ls -> makePicture (stackLayers picture l) ls
    | [] -> picture

let printPicture pixels lineWidth =
    let symbols = 
        Seq.map (fun p -> 
            match p with
            | Black -> " "
            | White -> "#"
            | Transparent -> "T")
            pixels

    splitToLayers symbols lineWidth
    |> List.map (fun l -> String.concat "" l)
    |> List.reduce (fun s1 s2 -> s1 + "\n" + s2)    

let main (input: string seq) =
    let width, height = 25, 6
    let pixelSequence = getPixelSequence input
    let layers = splitToLayers pixelSequence (width * height)


    let part1 =
        let least0DigitLayer, _ =
            layers 
            |> List.fold (fun (curLeast, count : int) layer -> 
                let currentCount = 
                    Seq.where (fun p -> p = Black) layer
                    |> Seq.length
                if currentCount < count then
                    layer, currentCount
                else 
                    curLeast, count
                )
                (Seq.empty, Microsoft.FSharp.Core.int.MaxValue)
        (Seq.where (fun i -> i = White) least0DigitLayer |> Seq.length) * (Seq.where (fun i -> i = Transparent) least0DigitLayer |> Seq.length)        

    let part2 = 
        let revLayers = List.rev layers
        printPicture (makePicture (List.head revLayers) (List.tail revLayers)) width

    part1.ToString(), "\n" + part2