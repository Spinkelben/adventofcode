module Year2019Day13

open System

type Tile =
    | Empty
    | Wall
    | Block
    | Paddle
    | Ball

let printScreen (tiles : Map<int * int, Tile>)=
    let tileList = Map.toList tiles
    let maxX = 
        List.map (fun ((x, _), _) -> x) tileList
        |> List.max
    let minX = 
        List.map (fun ((x, _), _) -> x) tileList
        |> List.min
    let maxY = 
        List.map (fun ((_, y), _) -> y) tileList
        |> List.max
    let minY = 
        List.map (fun ((_, y), _) -> y) tileList
        |> List.min

    let xOffset = if minX < 0 then -minX else 0
    let yOffset = if minY < 0 then -minY else 0
    let ySize = (1 + maxY - minY)
    let xSize = (1 + maxX - minX)
    let picture = Array2D.create ySize xSize Empty
    List.iter (fun ((x, y), color) -> 
            Array2D.set picture (y + yOffset) (x + xOffset) color) 
        tileList

    [for y in 0 .. ySize - 1 do 
        Array.map (fun char -> 
            match char with
            | Block -> "░"
            | Wall -> "▓"
            | Paddle -> "█"
            | Ball -> "*"
            | Empty -> " "
        ) picture.[y,*] 
        |> Array.fold (fun state char -> state + char) ""]
    |> List.fold (fun acc line -> acc + line + "\n") "\n"

let parseOutput output existingScreen =
    let rec parseOutput' outputList acc =
        let screen, curScore = acc
        match outputList with
        | x :: xs when (x = -1L) ->
            parseOutput' (List.skip 2 xs) (screen, List.item 1 xs)
        | x :: xs ->
            if xs.Length > 1 then
                let y = List.item 0 xs
                let block = match List.item 1 xs with
                            | 0L -> Empty
                            | 1L -> Wall
                            | 2L -> Block
                            | 3L -> Paddle
                            | 4L -> Ball
                            | _ -> failwith "Unexpected block type"
                let newSceen = Map.add (int x, int y) block screen
                parseOutput' (List.skip 2 xs) (newSceen, curScore)
            else
                failwith "Unexpected output length"
        | [] -> acc

    parseOutput' output (existingScreen, 0L)

let getInput () =
    let key = Console.ReadKey(true);
    match key.Key with
    | ConsoleKey.LeftArrow -> -1
    | ConsoleKey.RightArrow -> 1
    | _ -> 0

let autoplay (screen : Map<int*int,Tile>)  =
    let ball = Map.tryPick (fun key value -> 
                            if value = Ball then
                                Some key
                            else 
                                None) screen

    let paddle = Map.tryPick (fun key value -> 
                            if value = Paddle then
                                Some key
                            else 
                                None) screen
    match ball, paddle with
    | Some b, Some p -> ((fst b) - (fst p)) |> max -1 |> min 1
    | _ -> 0


let main (input : seq<string>) =
    let program = (Seq.head input).Split(",") |> Array.map int64

    let part1 =
        let (output, _, _), _ = IntCodeComputer.executeProgram program [] None None None
        let screen, score = parseOutput output Map.empty
        printfn "%s" (printScreen screen)
        printfn "Score: %i" score 
        Map.fold (fun count key value -> 
                if value = Block then
                   count + 1
                else 
                    count) 
            0
            screen

    let part2 =
        Array.set program 0 2L
        let rec runGame pCounter program memory baseOffset screen score =
            let input = int64 (autoplay screen)
            let (output, nextPCounter, isTerminated), (nextProgram, nextMemory, nextBaseOffset) = 
                IntCodeComputer.executeProgram program [input] (Some pCounter) (Some memory) (Some baseOffset)
            let screen', tscore = parseOutput output screen
            let score' = if tscore <> 0L then tscore else score
            if isTerminated = true then
                printfn "You WON"
                score'
            else 
                Console.Clear()
                printfn "%s" (printScreen screen')
                printfn "Score: %i" score'
                runGame nextPCounter nextProgram nextMemory nextBaseOffset screen' score'

        runGame 0 program Map.empty 0L Map.empty 0L

    part1.ToString(), part2.ToString()