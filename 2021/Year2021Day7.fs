namespace Year2021

open System

module Day7 =
    let splitString (seperator : string) (s : string) =
        s.Split seperator

    let main (input :string seq) =
        let parseCrabPositions string =
             splitString "," string
             |> Array.map int64
             |> Array.countBy id
             |> Array.map (fun (key, count) -> key, int64 count)
             |> Map.ofArray

        let part1FuelUsage nMoves = 
            nMoves

        let calculateFuelNeed fuelUsage position crabs =
            Map.fold (fun acc (cPosition :int64) count -> acc + (fuelUsage (Math.Abs (position - cPosition))) * count) 0L crabs

        let getMinimunConsumption fuelUsageFun crabs =
            let positions = Map.toList crabs // Will be ordered by keys
                            |> List.map (fun (pos, _) -> pos)
            let minPosition = List.min positions
            let maxPosition = List.max positions
            let optimalPos = [minPosition .. 1L .. maxPosition] 
                             |> List.minBy (fun posToTest -> calculateFuelNeed fuelUsageFun posToTest crabs)
            calculateFuelNeed fuelUsageFun optimalPos crabs


        let part1 = parseCrabPositions (Seq.head input)
                    |> getMinimunConsumption part1FuelUsage

        // Partial sum of series 1+2+3+4... for step n
        let part2fuelUsage n = 
            n * (n + 1L) / 2L

        let part2 = parseCrabPositions (Seq.head input)
                    |> getMinimunConsumption part2fuelUsage
                    

        string part1, string part2