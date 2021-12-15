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
            let fScore = Map.map (fun k t -> Int32.MaxValue) map
            let gScore = Map.map (fun k t -> Int32.MaxValue) map
            let getNeighbors (x, y) = [ (x + 1, y + 1); (x + 1, y);  (x + 1, y - 1); 
                                        (x, y + 1); (x, y - 1); 
                                        (x - 1, y + 1); (x - 1, y); (x - 1, y-1);]
                                       |> List.where (fun x -> map.ContainsKey x)

            let rec shortestPath' openSet camefrom gScore fScore =
                if Set.count openSet = 0 then failwith "No path"
                let current = openSet 
                              |> Set.toSeq 
                              |> Seq.minBy (fun e -> Map.find e fScore)
                if current = goal then
                    Map.find current gScore
                let openSet' = Set.remove current openSet
                let neighborsToSearch = getNeighbors current
                                        |> List.map (fun n -> (n, Map.find current gScore + Map.find n map))
                                        |> List.where (fun (n, tentativeGScore) -> tentativeGScore < Map.find n gScore)
                let camefrom' = List.fold (fun cf (n, _) -> Map.add n current cf) camefrom neighborsToSearch
                let gScore' = List.fold (fun gS (n, s) -> Map.add n s gS) gScore neighborsToSearch
                let fScore' = List.fold (fun fS (n, s) -> Map.add n (s + 1) fS) gScore neighborsToSearch
                let openSet'' = List.fold (fun oS (n) -> Set.add n oS) openSet' neighborsToSearch
                shortestPath' openSet'' camefrom' gScore' fScore'

            shortestPath' (set [start]) Map.empty gScore fScore

                                
                


        let part1 = ""
     
        let part2 = ""
                    
        string part1, string part2