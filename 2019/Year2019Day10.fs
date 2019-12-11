module Year2019Day10

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
            let dx, dy = 
                match gcd x y with 
                | 0 -> x, y
                | d -> x / d, y / d
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
                    asteroidCount, coord
                else 
                    curVisibleAsteroids, kvp.Key
                )
                (0, (-1, -1))

        let headingList = Seq.initInfinite (fun index -> 
            let modIndex = index % (xMax * 2 + (yMax - 2) * 2)
            if modIndex < (xMax - 1) then
                modIndex, 0
            else if modIndex < (xMax - 1) + (yMax - 1) then
                xMax - 1, modIndex - (xMax - 1)
            else if modIndex < (xMax - 1) * 2 + yMax then
                (xMax - 1) - (modIndex - (xMax - 1 + yMax - 1)), (yMax - 1)
            else
                0, (yMax - 1) - (modIndex - ((xMax - 1) * 2) - (yMax - 1)))

        let rec shootAsteroid headingSeq aMap (resultList : list<int * int>) =
            let current = Seq.head headingSeq
            if List.length resultList = 200 then
                List.head resultList
            else 
                match getHitAsteroid laserCoordinate (Seq.head headingSeq) (xMax, yMax) asteroidMap with
                | Some coord -> 
                    shootAsteroid (Seq.tail headingSeq) (Map.remove coord aMap) (coord :: resultList)
                | None -> 
                    shootAsteroid (Seq.tail headingSeq) aMap resultList

        let result = 
            shootAsteroid (headingList |> Seq.skip (fst laserCoordinate)) asteroidMap []
        result

    part1.ToString(), part2.ToString()