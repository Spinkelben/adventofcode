module Year2019Day12

open System.Text.RegularExpressions

type DimensionComponent = {
    position: int;
    velocity: int; }

type Moon = { 
    position: int * int * int;
    velocity: int * int * int; }
    
let private parseLine line =
    let regex = new Regex("<x=(.+), y=(.+), z=(.+)>")
    let stringMatch = regex.Match(line)
    let x = stringMatch.Groups.["1"].Value
    let y = stringMatch.Groups.["2"].Value
    let z = stringMatch.Groups.["3"].Value
    int x, int y, int z

let parseMoon line =
    let x, y, z = parseLine line
    { velocity = 0, 0, 0;
      position = x, y, z }


let applyGravity moonA moonB =
    let applyGravityOnAxis pointA pointB velocityA =
        if pointA > pointB then
            velocityA - 1
        else if pointA < pointB then
            velocityA + 1
        else 
            velocityA

    let aPositionX, aPositionY, aPositionZ = moonA.position
    let bPositionX, bPositionY, bPositionZ = moonB.position
    let aVelocityX, aVelocityY, aVelocityZ = moonA.velocity

    let newAX = applyGravityOnAxis aPositionX bPositionX aVelocityX
    let newAY = applyGravityOnAxis aPositionY bPositionY aVelocityY
    let newAZ = applyGravityOnAxis aPositionZ bPositionZ aVelocityZ
    { moonA with velocity = newAX, newAY, newAZ }

let applySpeed moon =
    let vX, vY, vZ = moon.velocity
    let pX, pY, pZ = moon.position
    { moon with position = pX + vX, pY + vY, pZ + vZ }

let updateVelocities moonList =
    moonList 
    |> List.map (fun moon ->
        moonList 
        |> List.fold (fun acc m -> 
            let next = applyGravity acc m
            next
        ) moon)

let updatePositions moonList =
    moonList |> Seq.map applySpeed

let doSimulationStep moonList = 
    updateVelocities moonList
    |> updatePositions

let covertToDimension (moonList : Moon seq) =
    moonList |> Seq.fold (fun acc moon -> 
        match acc with
        | (x , y, z) -> 
            let posX, posY, posZ = moon.position
            let velX, velY, velZ = moon.velocity
            (
                {DimensionComponent.position = posX; velocity = velX} :: x,
                {DimensionComponent.position = posY; velocity = velY} :: y,
                {DimensionComponent.position = posZ; velocity = velZ} :: z
            ))
        (List.empty, List.empty, List.empty)

let runSimulation moonList numSteps =
    let rec runSimulation' moonList stepCount (xStates, yStates, zStates) (xDup, yDup, zDup) =
        let xList, yList, zList = covertToDimension moonList
        let updateDupStep list state dup =
            if Set.contains list state && dup = -1 then
                stepCount, state
            else
                dup, Set.add list state

        if xDup > -1 && yDup > -1 && zDup > -1 then
            moonList, (xDup, yDup, zDup)
        else if stepCount >= numSteps then
            moonList, (xDup, yDup, zDup)
        else
            let xDup', xStates' = updateDupStep xList xStates xDup
            let yDup', yStates' = updateDupStep yList yStates yDup
            let zDup', zStates' = updateDupStep zList zStates zDup
            runSimulation' (doSimulationStep (List.ofSeq moonList)) (stepCount + 1) (xStates', yStates', zStates') (xDup', yDup', zDup')

    runSimulation' moonList 0 (Set.empty, Set.empty , Set.empty) (-1,-1,-1)

let calculateEnergy moonList=
    let absSum vector =
        let x, y, z = vector
        abs(x) + abs(y) + abs(z)

    moonList 
    |> Seq.map (fun m -> 
        (absSum m.position)  * (absSum m.velocity))
    |> Seq.sum
    
let gcd a b =
    let rec gcd' a b =
        if b = 0L then 
            a
        else
            gcd' b (a % b)
    
    let aabs, babs = abs(a), abs(b)
    if aabs > babs then
        gcd' aabs babs
    else 
        gcd' babs aabs

let lcm a b =
    (a * b) / (gcd a b)

let lcmList list =
    Seq.fold (fun acc v -> lcm acc v) (Seq.head list) (Seq.tail list)

let main input =
    let moonList = 
        input 
        |> Seq.map parseMoon

    let part1 =
        let finalState, duplicateStep = runSimulation moonList 1000
        calculateEnergy finalState

    let part2 = 
        let finalState, (a, b, c) = runSimulation moonList 300000
        lcmList (seq { int64 a; int64 b; int64 c})

    part1.ToString(), part2.ToString()