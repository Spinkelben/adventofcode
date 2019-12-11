module Year2019Day10
open System

let parseAsteroidMap input = 
    Seq.mapi (fun yIndex line -> 
        Seq.mapi (fun xIndex char -> ((xIndex, yIndex), char)) line
        |> Seq.filter (fun (_, char) -> char = '#'))
        input
    |> Seq.concat
    |> Map.ofSeq

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

let normalizeDirection direction = 
    let x, y = direction
    match gcd x y with 
    | 0 -> x, y
    | d -> x / d, y / d

let getPointsOnLine source direction mapSize =
    let xLimit, xDirection = if (fst direction) > 0 then (fst mapSize),1 else 0,-1
    let yLimit, yDirection = if (snd direction) > 0 then (snd mapSize),1 else 0,-1
    match direction with
    | 0, y -> seq {
            for yShadow in (snd source) .. yDirection .. yLimit ->
                (fst source), yShadow
            }
    | x, 0 -> seq {
            for xShadow in (fst source) .. xDirection .. xLimit -> 
                xShadow, snd(source)
            }
    | x, y -> 
            let dx, dy = normalizeDirection (x, y)
            Seq.zip 
                (seq {
                    for xShadow in (fst source) .. dx .. xLimit -> xShadow
                })
                (seq {
                    for yShadow in (snd source) .. dy .. yLimit -> yShadow
                })

let getDirection source destination =
    (fst destination) - (fst source), (snd destination) - (snd source)

let getAsteroidsInShadow source obstruction asteroidMap mapSize =
    let direction = getDirection source obstruction
    let mapOfOtherAsteroids = 
        asteroidMap
        |> Map.remove source
        |> Map.remove obstruction 
    let shadow =
        getPointsOnLine obstruction direction mapSize

    shadow
    |> Seq.filter (fun coord -> Map.containsKey coord mapOfOtherAsteroids)

let getHitAsteroid source heading mapSize asteroidMap =
    let direction = getDirection source heading
    let targets =
        getPointsOnLine source direction mapSize
    targets
    |> Seq.tryFind (fun t -> Map.containsKey t asteroidMap)

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

let calculateDegrees direction =
    let x, y = direction
    (atan(y / x) / Math.PI) * 180.0

let vectorLength vector =
    let x, y = vector
    sqrt( x ** 2.0 + y ** 2.0)

let main input =
    let xMax, yMax = getMapDimensions input 
    let asteroidMap = parseAsteroidMap input

    let part1 =
        asteroidMap 
        |> Map.map (fun coord _ -> Set.count (getHiddenAsteroids coord asteroidMap (xMax, yMax)))
        |> Seq.map (fun kvp -> asteroidMap.Count - kvp.Value - 1)
        |> Seq.max


    let part2 =
        let _, laserCoordinate =
            asteroidMap 
            |> Map.map (fun coord _ -> Set.count (getHiddenAsteroids coord asteroidMap (xMax, yMax)))
            |> Seq.fold (fun (asteroidCount, coord) kvp ->
                let curVisibleAsteroids = asteroidMap.Count - kvp.Value - 1
                if curVisibleAsteroids > asteroidCount then
                    curVisibleAsteroids, kvp.Key
                else 
                    asteroidCount, coord
                )
                (0, (-1, -1))

        let groupedAsteroids =
            asteroidMap
            |> Map.remove laserCoordinate
            |> Seq.groupBy (fun kvp -> 
                getDirection laserCoordinate kvp.Key
                |> normalizeDirection)
            |> Seq.map (fun ((x, y), asteroids) -> 
                calculateDegrees (double x, double y), asteroids)
            |> Seq.map (fun (coord, asteroids) ->
                let sortedAsteroids = 
                    Seq.sortBy (fun (kvp : Collections.Generic.KeyValuePair<(int * int), char>) -> 
                        let x,y = getDirection laserCoordinate kvp.Key
                        vectorLength (float x, float y)) asteroids
                coord, sortedAsteroids)
            |> List.ofSeq
            |> List.sortBy (fun(degrees, _) -> (degrees + 269.0) % 360.0)

        let rec shootAsteroids (currentList : list<float * seq<Collections.Generic.KeyValuePair<(int * int), char>>>) nextList count =
            match currentList with
            | (degree, asteroids) :: xs -> 
                if count = 200 then
                    (Seq.head asteroids).Key
                else
                    let remainingAsteroids = Seq.tail asteroids
                    shootAsteroids xs ((degree, remainingAsteroids) :: nextList) (count + 1)
            | [] -> 
                let directionsWithAsteroids = 
                    List.filter     
                        (fun (deg, asteroidSeq) ->
                            Seq.length asteroidSeq > 0) 
                        nextList
                match directionsWithAsteroids with
                | [] -> (-1, -1)
                | _ -> shootAsteroids (List.rev directionsWithAsteroids) [] count

        let result = 
            shootAsteroids groupedAsteroids [] 1
        result

    part1.ToString(), part2.ToString()