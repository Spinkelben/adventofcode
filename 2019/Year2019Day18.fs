module Year2019Day18

let findString (mazeString : string) (seekChar : string) =
    let width = mazeString.Split("\r\n").[0].Length
    let i = mazeString.Replace("\r\n", "").IndexOf(seekChar)
    (i % width, i / width)

let searchString seekFun (mazeString : string) =
    let width = mazeString.Split("\r\n").[0].Length
    let noLineString = mazeString.Replace("\r\n", "")
    noLineString 
    |> Seq.mapi (fun idx v -> (idx % width, idx / width), v) 
    |> Seq.filter (fun (c, v) -> seekFun v)

(*
let aStar h start goal map =
    let openSet = Set.singleton start
    let cameFrom = Map.empty
    let gScore = Map.empty |> Map.add start 0
    let fscore = Map.empty |> Map.add start (h start)

    let rec aStar' openSet fScore gScore =
        if Set.count openSet = 0 then
            failwith "No path"
        else
            let current = 
                snd (Set.fold 
                        (fun (score, accNode) node ->
                            let cScore = if Map.containsKey node fScore = false then
                                            System.Int32.MaxValue
                                         else
                                            Map.find node fScore
                            if cScore < score then
                                cScore, node
                            else 
                                score, accNode)
                        (System.Int32.MaxValue, (-1, -1))
                        openSet)
            if current = goal then
                Map.find current gScore
            else
                let updateNeighbour neighbour cameFrom gScore fScore openSet =
                    let tempGScore = Map.find current gScore
                    if tempGScore < 

*)
    

let findKeys map startPoint =
    let rec findKeys' (map : char[,]) position visitedLocations stepCount foundKeys =
        if Set.contains position visitedLocations then
            foundKeys
        else
            let x, y = position
            match map.[y,x] with
            | '#' -> foundKeys
            | door when (door >= 'A' && door <= 'Z') -> foundKeys
            | k -> 
                let foundKeys' = 
                    if k = '.' || k ='@' then
                        foundKeys
                    else
                        Set.add (position, k, stepCount) foundKeys
                let visitedLocations' = Set.add position visitedLocations
                let keys = [ findKeys' map (x - 1, y) visitedLocations' (stepCount + 1) foundKeys';
                             findKeys' map (x + 1, y) visitedLocations' (stepCount + 1) foundKeys';
                             findKeys' map (x, y - 1) visitedLocations' (stepCount + 1) foundKeys';
                             findKeys' map (x, y + 1) visitedLocations' (stepCount + 1) foundKeys'; ]
                Set.unionMany keys

    findKeys' map startPoint Set.empty 0 Set.empty

let openDoor doorMap door maze =
    let (dX, dY) = Map.find door doorMap
    Array2D.set maze dY dX '.'

let getDoorFromKey (key : char) =
    char ((int key) - (int 'a' - int 'A'))

//let solveMazeImproved map startPoint doorMap keyCount : (int * char list) =
    //let rec solveMazeImproved' 
    

let solveMaze map startPoint doorMap keyCount : (int * char list) =
    let rec solveMaze' map startPoint doorMap steps keysFound : (int * char list) =
        let keys = findKeys map startPoint
        let results = [ for (pos, key, keySteps) in keys do
                        let door = (getDoorFromKey key)
                        let map' = Array2D.copy map 
                        Array2D.set map' (snd pos) (fst pos) '.' 
                        if Map.containsKey door doorMap then
                            openDoor doorMap door map'
                            solveMaze' map' pos doorMap (steps + keySteps) (key :: keysFound) 
                        else if keysFound.Length + 1 = keyCount then
                            steps + keySteps, key :: keysFound
                        else
                            solveMaze' map' pos doorMap (steps + keySteps) (key :: keysFound) 
                        ]
        List.fold
            (fun acc (v, k) -> 
                        if (fst acc) > v then 
                            (v, k) 
                        else 
                            acc)
            (System.Int32.MaxValue, List.empty)
            results

    let steps, keys = solveMaze' map startPoint doorMap 0 List.empty
    steps, List.rev keys


let main (input : string seq) =
    let mazeString = Seq.head input
    let width = mazeString.Split("\r\n").[0].Length
    let height = mazeString.Length / width
    let maze = Array2D.create height width '?'
    Seq.iteri (fun i (c : char) -> 
        let x = i % width
        let y = i / width
        Array2D.set maze y x c) (mazeString.Replace("\r\n", ""))
    let startPoint = findString mazeString "@"
    let doors = 
        searchString (fun c -> c >= 'A' && c <= 'Z') mazeString
        |> Seq.map (fun (coord, doorId) -> doorId, coord)
        |> Map.ofSeq

    let keyCount = searchString (fun c -> c >= 'a' && c <= 'z') mazeString |> Seq.length

    let part1 =
        let (result, list) = solveMaze maze startPoint doors keyCount
        result
        

    let part2 = 
        ""

    part1.ToString(), part2