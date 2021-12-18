namespace Year2021

open System

module Day15 =
    let splitString (split: string) (string : string) =
        string.Split([|split|], StringSplitOptions.RemoveEmptyEntries)

    let main (input :string seq) =
        let parseInput input = 
            input |> Seq.map (fun line -> line |> List.ofSeq
                                          |> List.map string
                                          |> List.map int)
                  |> Seq.mapi (fun x line -> line |> List.mapi (fun y v -> (x, y), v))
                  |> Seq.concat
                  |> Map.ofSeq

        let shortestPath map start goal = 
            let getNeighbors (x, y) = [ (x + 1, y);  (x, y + 1); (x, y - 1); (x - 1, y); ]
                                        |> List.where (fun x -> Map.containsKey x map)
            let heuristic (x, y) = 
                let gx, gy = goal
                abs (gx - x) + abs (gy - y)

            let rec computePath cameFrom path current =
                let path' = current :: path
                if current = start then
                    path'
                else
                    computePath cameFrom path' (Map.find current cameFrom)

            let fScore = Map.map (fun k t -> Int32.MaxValue) map
                         |> Map.add start (heuristic start)
            let gScore = Map.map (fun k t -> Int32.MaxValue) map
                         |> Map.add start 0

            let rec shortestPath' openSet camefrom (gScore: Map<(int * int),int>) fScore =
                if Set.count openSet = 0 then failwith "No path"
                let current = openSet 
                              |> Set.toList
                              |> List.minBy (fun e -> Map.find e fScore)
                if current = goal then
                    Map.find current gScore, computePath camefrom [] current
                else
                    let openSet' = Set.remove current openSet
                    let neighborsToSearch = getNeighbors current
                                            |> List.map (fun n -> (n, (Map.find current gScore) + (Map.find n map)))
                                            |> List.where (fun (n, tentativeGScore) -> tentativeGScore < Map.find n gScore)
                    let camefrom' = List.fold (fun cf (n, _) -> Map.add n current cf) camefrom neighborsToSearch
                    let gScore' = List.fold (fun gS (n, s) -> Map.add n s gS) gScore neighborsToSearch
                    let fScore' = List.fold (fun fS (n, s) -> Map.add n (s + heuristic n) fS) fScore neighborsToSearch
                    let openSet'' = List.fold (fun oS (n, _) -> Set.add n oS) openSet' neighborsToSearch
                    shortestPath' openSet'' camefrom' gScore' fScore'

            shortestPath' (set [start]) Map.empty gScore fScore

                                
                

        let map = parseInput input
        let size = Seq.length input
        let score,_ = shortestPath map (0,0) (size - 1, size - 1)
        let part1 = score
     
        let expandedGrid map = 
            map |> Map.fold (fun state (x, y) risk -> 
                seq { for i in [0..4] do 
                        for j in [0..4] do 
                            yield (i * size + x, j * size + y, i + j) }
                |> Seq.fold (fun state' (x', y', increase) -> 
                    Map.add (x', y') (((risk + increase - 1) % 9) + 1) state') state) Map.empty
                    
        let big = expandedGrid map
        let size2 = size * 5
        let score2, _ = shortestPath big (0,0) (size2 - 1, size2 - 1)
        let part2 = score2
                    
        string part1, string part2