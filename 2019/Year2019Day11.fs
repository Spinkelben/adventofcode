module Year2019Day11

type PanelColor =
| White
| Black

type Direction =
| Right
| Left

type Heading =
| Up
| Down
| Left
| Right


let turnRobot heading direction =
    match heading with
    | Up ->
        match direction with
        | Direction.Right -> Right
        | Direction.Left -> Left
    | Down -> 
        match direction with
        | Direction.Right -> Left
        | Direction.Left -> Right
    | Left -> 
        match direction with
        | Direction.Right -> Up
        | Direction.Left -> Down
    | Right ->
        match direction with
        | Direction.Right -> Down
        | Direction.Left -> Up

let moveRobot heading position steps =
    let x,y = position
    match heading with
    | Up -> x, y  - steps
    | Down -> x, y + steps
    | Left -> x - steps, y
    | Right -> x + steps, y
        

let rec runRobot position heading panels computerState =
    let ((program, memory, baseOffset), pCounter) = computerState
    let input =
        if Map.containsKey position panels then
            match Map.find position panels with
            | White -> 1L
            | Black -> 0L
        else 
            0L
    let (output, nextPCounter, isTerminated), nextComputerState = 
         IntCodeComputer.executeProgram program [input] (Some pCounter) (Some memory) (Some baseOffset)
    let color = 
        match List.item 0 output with
        | 0L -> Black
        | 1L -> White
        | _ -> failwith "Invalid color specified"
    let newPanels = Map.add position color panels
    if isTerminated then
        newPanels
    else
        let turnDirection =
            match List.item 1 output with
            | 0L -> Direction.Left
            | 1L -> Direction.Right
            | _ -> failwith "Invalid direction specified"
        let newHeading = turnRobot heading turnDirection
        let newPosition = moveRobot newHeading position 1
        runRobot newPosition newHeading newPanels (nextComputerState, nextPCounter)

let drawPanels (panels : Map<int*int, PanelColor>)=
    let maxX = 
        Seq.map (fun ((x, _), _) -> x) (Map.toSeq panels)
        |> Seq.max
    let minX = 
        Seq.map (fun ((x, _), _) -> x) (Map.toSeq panels)
        |> Seq.min
    let maxY = 
        Seq.map (fun ((_, y), _) -> y) (Map.toSeq panels)
        |> Seq.max
    let minY = 
        Seq.map (fun ((_, y), _) -> y) (Map.toSeq panels)
        |> Seq.min

    let xOffset = if minX < 0 then -minX else 0
    let yOffset = if minY < 0 then -minY else 0
    let ySize = (1 + maxY - minY)
    let xSize = (1 + maxX - minX)
    let picture = Array2D.create ySize xSize Black
    Seq.iter (fun ((x, y), color) -> 
            Array2D.set picture (y + yOffset) (x + xOffset) color) 
        (Map.toSeq panels)

    [for y in 0 .. ySize - 1 do 
        Array.map (fun char -> 
            match char with
            | Black -> "#"
            | White -> "."
        ) picture.[y,*] 
        |> Array.fold (fun state char -> state + char) ""]
    |> List.fold (fun acc line -> acc + line + "\n") "\n"
        

let main (input : seq<string>) =
    let program =
        (Seq.head input).Split(",")
        |> Array.map int64
    let part1 = 
        let finalPaint = runRobot (0,0) Heading.Up Map.empty ((program, Map.empty, 0L), 0)
        finalPaint.Count

    let part2 = 
        let startPanel = Map.empty |> Map.add (0,0) White
        let finalPaint = runRobot (0,0) Heading.Up startPanel ((program, Map.empty, 0L), 0)
        drawPanels finalPaint

    part1.ToString(), part2