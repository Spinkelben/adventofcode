module Year2019Day18

let findString (mazeString : string) (seekChar : string) =
    let width = mazeString.Split("\n").[0].Length
    let height = mazeString.Length / width
    let i = mazeString.IndexOf(seekChar)
    (i % width, i / width)

let findKeys map startPoint =
    let rec findKeys' (map : char[,]) position visitedLocations stepCount foundKeys =
        if Set.contains position visitedLocations then
            foundKeys
        else
            let x, y = position
            match map.[y,x] with
            | '#' -> foundKeys
            | '.' | '@' -> 
                let keys = [ findKeys' map (x - 1, y) (Set.add position visitedLocations) (stepCount + 1) foundKeys;
                             findKeys' map (x + 1, y) (Set.add position visitedLocations) (stepCount + 1) foundKeys;
                             findKeys' map (x, y - 1) (Set.add position visitedLocations) (stepCount + 1) foundKeys;
                             findKeys' map (x, y + 1) (Set.add position visitedLocations) (stepCount + 1) foundKeys; ]
                Set.unionMany keys
            | door when (door >= 'A' && door <= 'Z') -> foundKeys
            | k -> Set.add (position, k, stepCount) foundKeys

    findKeys' map startPoint Set.empty 0 Set.empty

let solveMaze map startPoint =
    let keys = findKeys map startPoint
    // Todo 
    // 1. For each of the keys, pick the key as the next starting point
    // 2. For each of the doors we have a key to, pick it as target and find the path
    // 3. Open the door with the key
    // 4. If there are more doors left, goto 1 else count the total steps taken
    // 5. Select the path with the fewest steps
    ""

let main (input : string seq) =
    let mazeString = Seq.head input
    let width = mazeString.Split("\n").[0].Length
    let height = mazeString.Length / width
    let maze = Array2D.create height width '?'
    Seq.iteri (fun i (c : char) -> 
        let x = i % width
        let y = i / width
        Array2D.set maze y x c) mazeString
    let startPoint = findString "@"
    

    let part1 =
        ""

    let part2 = 
        ""

    part1, part2