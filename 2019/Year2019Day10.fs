module Year2019Day10

let parseAsteroidMap input = 
    Seq.mapi (fun yIndex line -> 
        Seq.mapi (fun xIndex char -> ((xIndex, yIndex), char)) line
        |> Seq.filter (fun (_, char) -> char = '#'))
        input
    |> Seq.concat
    |> Map.ofSeq

let getAsteroidsInShadow source obstruction asteroidMap mapSize =
    let getDirection source destination =
        (fst destination) - (fst source), (snd destination) - (snd source)

    let direction = getDirection source obstruction
    let xLimit, xDirection = if (fst direction) > 0 then (fst mapSize),1 else 0,-1
    let yLimit, yDirection = if (snd direction) > 0 then (snd mapSize),1 else 0,-1
    let mapOfOtherAsteroids = 
        asteroidMap
        |> Map.remove source
        |> Map.remove obstruction 
    let shadow =
        match direction with
        | 0, y -> seq {
                for yShadow in (snd source) + y .. yDirection .. yLimit ->
                    (fst source), yShadow
                }
        | x, 0 -> seq {
                for xShadow in (fst source) + x .. xDirection .. xLimit -> 
                    xShadow, snd(source)
                }
        | x, y -> seq {
                for xShadow in (fst source) + x .. x .. xLimit do
                    yield! seq {
                        for yShadow in (snd source) + y .. y .. yLimit do
                            xShadow, yShadow
                    }
                }
    shadow
    |> Seq.filter (fun coord -> Map.containsKey coord mapOfOtherAsteroids)

let getHiddenAsteroids source asteroidMap mapSize =
    Map.remove source asteroidMap
    |> Map.fold (fun state coord _ -> 
            let hiddenAsteroids =
                Set.ofSeq (getAsteroidsInShadow source coord asteroidMap mapSize)
            Set.union hiddenAsteroids state
        )
        Set.empty

let getMapDimensions (input : string seq) =
    (Seq.head input).Length, Seq.length input

let main input =
    let xMax, yMax = getMapDimensions input 

    let part1 =
        let asteroidMap = parseAsteroidMap input
        asteroidMap 
        |> Map.map (fun coord _ -> Set.count (getHiddenAsteroids coord asteroidMap (xMax, yMax)))
        |> Seq.map (fun kvp -> asteroidMap.Count - kvp.Value)
        |> Seq.max


    let part2 =
        ""

    part1.ToString(), part2