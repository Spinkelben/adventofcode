module Year2019Day12

open System.Text.RegularExpressions

type Moon = { 
    position: int * int * int;
    velocity: int * int * int; }
    
let parseMoon line =
    let regex = new Regex("<x=(.+), y=(.+), z=(.+)>")
    let stringMatch = regex.Match(line)
    let x = stringMatch.Groups.["1"].Value
    let y = stringMatch.Groups.["2"].Value
    let z = stringMatch.Groups.["3"].Value
    { velocity = 0, 0, 0;
      position = int x, int y, int z }

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
    let bVelocityX, bVelocityY, bVelocityZ = moonB.velocity

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

let runSimulation moonList numSteps =
    let rec runSimulation' moonList stepCount =
        if stepCount >= numSteps then
            moonList
        else
            runSimulation' (doSimulationStep (List.ofSeq moonList)) (stepCount + 1)

    runSimulation' moonList 0

let calculateEnergy moonList=
    let absSum vector =
        let x, y, z = vector
        abs(x) + abs(y) + abs(z)

    moonList 
    |> Seq.map (fun m -> 
        (absSum m.position)  * (absSum m.velocity))
    |> Seq.sum

let main input =
    let moonList = 
        input 
        |> Seq.map parseMoon

    let part1 =
        let finalState = runSimulation moonList 200000
        calculateEnergy finalState

    let part2 = 
        ""

    part1.ToString(), part2