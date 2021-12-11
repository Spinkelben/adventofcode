namespace Year2021

open System

module Day11 =
    
    let main (input :string seq) =
        let getGrid (input: string seq) =
            Array.ofSeq input
            |> Array.map (fun l ->  l 
                                    |> Seq.map string
                                    |> Seq.map int
                                    |> Array.ofSeq)

        let neighbours maxX maxY (x, y) =
            [ 
              (x + 1, y + 1); 
              (x    , y + 1);
              (x - 1, y + 1);
              (x + 1, y    ); 
              (x - 1, y    );
              (x + 1, y - 1); 
              (x    , y - 1);
              (x - 1, y - 1);
            ] |> List.where (fun (x, y) -> x >= 0 && y >= 0)
            |> List.where (fun (x, y) -> x < maxX && y < maxY)

        let doStep grid =
            let adaptedNeighbours = neighbours (Array.length grid) (Array.length grid.[0]) 
            let incrementNeighbourEnergy (grid :int [][]) (x, y) =
                let n = adaptedNeighbours (x, y)
                n
                |> List.iter (fun (x, y) -> 
                    let e = grid.[x].[y] 
                    if e > 0 then grid.[x].[y] <- (e + 1))

            let newGrid = Array.map (Array.map (fun e -> e + 1)) grid // Add one to all squid energies in new array
            // While there is a squid with more than 9 energy do the flashing
            while Array.exists (Array.exists (fun e -> e > 9)) newGrid  do
                newGrid |> Array.iteri (fun x a -> 
                    Array.iteri (fun y e -> 
                        if e > 9 then
                            newGrid.[x].[y] <- 0  // This squid flashes
                            incrementNeighbourEnergy newGrid (x, y))// Update neighbours
                     a) 
            newGrid

        let rec runSimuation scoreFun acc steps grid =
            if steps >= 0 then
                let result = doStep grid
                runSimuation scoreFun (scoreFun acc grid) (steps - 1) result
            else
                acc

        let part1Score acc input =
            acc + (input 
            |> Array.sumBy (fun a -> 
                a 
                |> Array.choose (fun e -> 
                    if e = 0 then 
                        Some 1
                    else 
                        None)
                |> Array.sum))

        let part1 = getGrid input
                    |> runSimuation part1Score 0 100

        let rec findSyncStep step grid = 
            if part1Score 0 grid = Array.length grid * Array.length grid.[0] then
                step
            else 
                findSyncStep (step + 1) (doStep grid) 

        let part2 = getGrid input
                    |> findSyncStep 0
                    
        string part1, string part2