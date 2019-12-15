module Year2019Day15
open System

type Moves =
    | North
    | South
    | West
    | East

type Tile =
    | Wall
    | Ground
    | OxygenSystem
    | Robot
    | Uncharted

let getInput () =
    let key = Console.ReadKey(true);
    match key.Key with
    | ConsoleKey.LeftArrow -> 3L
    | ConsoleKey.RightArrow -> 4L
    | ConsoleKey.UpArrow -> 1L
    | ConsoleKey.DownArrow -> 2L
    | _ -> failwith "Not a valid command"
        
let mapMerge map1 map2 = 
    Map.fold (fun acc key value -> 
                Map.add key value acc) map1 map2

let main (input : string seq) =
    let program = (Seq.head input).Split(",") |> Array.map int64

    let tileMap t =
        match t with
        | Wall -> "▓"
        | Ground -> "."
        | OxygenSystem -> "O"
        | Robot -> "D"
        | Uncharted -> " "

    let getNextCoordinate input coords =
        match input with
        | 1L -> (fst coords), snd coords - 1
        | 2L -> (fst coords), snd coords + 1
        | 3L -> (fst coords) - 1, (snd coords)
        | 4L -> (fst coords) + 1, (snd coords)
        | _ -> failwith "Not a valid direction"

    let intCodeStateFunction state input =
        let program, pCounter, memory, baseOffset = state
        let (output, nextPCounter, isTerminated), (nextProgram, nextMemory, nextBaseOffset) =
            IntCodeComputer.executeProgram program [input] pCounter memory baseOffset
        output, (nextProgram, Some nextPCounter, Some nextMemory, Some nextBaseOffset)

    let rec searchMaze state selectFunction (map : Map<int * int, Tile>) currentCoords count =
        let checkDirection direction =
            let output, nextState = selectFunction state direction
            let nextCoordinate = getNextCoordinate direction currentCoords
            match List.head output with
            | 0L -> None, map.Add (nextCoordinate, Wall), count
            | 1L -> searchMaze 
                        nextState
                        selectFunction
                        (map.Add (currentCoords, Ground))
                        nextCoordinate
                        (count + 1)
            | 2L -> Some nextCoordinate, map.Add(nextCoordinate, OxygenSystem), count
            | _ -> failwith "Unexpected status code"

        if map.ContainsKey currentCoords then
            None, map, count
        else 
            let result = [ (checkDirection 1L); (checkDirection 2L); (checkDirection 3L); (checkDirection 4L) ];
            result |> 
                List.fold (fun (aValue, aMap, aCount) (cValue, cMap, cCount) -> 
                    let mergedMap = mapMerge aMap cMap
                    if cValue.IsSome then 
                        cValue, mergedMap, cCount 
                    else 
                        aValue, mergedMap, aCount) (None, Map.empty, 0)

    let rec oxygenFlow selectFunction state visitedTiles currentCoord acc =
        let checkDirection direction =
            let output, nextState = selectFunction state direction
            let nextCoordinate = getNextCoordinate direction currentCoord
            match output with
            | Wall -> acc, (Set.add nextCoordinate visitedTiles)
            | Ground -> oxygenFlow 
                            selectFunction
                            nextState
                            (Set.add currentCoord visitedTiles)
                            nextCoordinate
                            (acc + 1)
            | OxygenSystem -> oxygenFlow 
                                selectFunction
                                nextState
                                (Set.add currentCoord visitedTiles)
                                nextCoordinate
                                (acc + 1)
            | Robot -> failwith "Unexpected status code"
            | Uncharted -> failwith "Unexpected status code"

        if Set.contains currentCoord visitedTiles then
            acc, visitedTiles
        else
            let result = [ (checkDirection 1L); (checkDirection 2L); (checkDirection 3L); (checkDirection 4L) ];
            (List.fold (fun (aValue, aVisisted) (cValue, cVisited) ->
                let merged = Set.union aVisisted cVisited
                if cValue > aValue then
                    cValue, merged
                else
                    aValue, merged) (0, Set.empty) result)

    let coordinate, map, count = searchMaze (program, None, None, None) intCodeStateFunction Map.empty (0, 0) 1

    let mazeStateFunction (state : Map<int *int, Tile> * (int * int)) direction =
        let (map, coord) = state
        let nextCoordinate = getNextCoordinate direction coord
        if Map.containsKey coord map then
            Map.find coord map, (map, nextCoordinate)
        else
            Wall, (map, nextCoordinate)

    let part1 =
        let fullMap = Map.add (0,0) Robot map |> Map.add (coordinate.Value) OxygenSystem
        let dense = ArrayHelpers.sparseToDense fullMap Uncharted
        printfn "%s" (ArrayHelpers.printTileMap dense tileMap)
        printfn "Coords: %s" (coordinate.Value.ToString())
        count

    let part2 =
        let dense = ArrayHelpers.sparseToDense map Uncharted
        printfn "%s" (ArrayHelpers.printTileMap dense tileMap)
        let countOxygen, _ = oxygenFlow mazeStateFunction (map, coordinate.Value) Set.empty coordinate.Value -1
        countOxygen
        

    part1.ToString(), part2.ToString()