module Year2019Day13

open System
open Utils

type Tile =
    | Empty
    | Wall
    | Block
    | Paddle
    | Ball

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

    let tileMap char = 
        match char with
        | Block -> "░"
        | Wall -> "▓"
        | Paddle -> "█"
        | Ball -> "*"
        | Empty -> " "

    let printScreen tiles =
        let dense = ArrayHelpers.sparseToDense tiles Empty
        ArrayHelpers.printTileMap dense tileMap

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