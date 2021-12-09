namespace Year2021

open System

module Day9 =
    let splitString (seperator : string) (s : string) =
        s.Split seperator

    let main (input :string seq) =
        let floorMap input =
            let mapLine x line = Seq.mapi (fun y v -> (x,y), string v |> int) line
            Seq.mapi (fun i line -> mapLine i line) input
            |> Seq.concat
            |> Map.ofSeq

        let getSurroundingTiles (x, y) =
            [ 
                (x + 1, y); 
                (x - 1, y); 
                (x, y + 1); 
                (x, y - 1); 
            ]
            

        let floorHeights = floorMap input // Parse input to coordinate to height map

        let part1 = 
            let isLowPoint arg =
                let (c, value) = (|KeyValue|) arg
                getSurroundingTiles c
                |> List.choose (fun c -> Map.tryFind c floorHeights) 
                |> List.forall (fun s -> value < s)

            Seq.where isLowPoint floorHeights // Get low points
            |> Seq.map (fun kvp -> kvp.Value + 1) // Add one to get danger rating
            |> Seq.sum
            
        let rec fillBasins unMapped id frontier basins =
            match frontier with
            | c :: cs when Set.contains c unMapped ->  
                let height = Map.find c floorHeights
                if height = 9 then 
                    fillBasins (Set.remove c unMapped) id cs basins
                else 
                    fillBasins (Set.remove c unMapped) id (List.concat [getSurroundingTiles c; cs]) ((c, id) :: basins)
            | c:: cs -> fillBasins unMapped id cs basins
            | [] when not (Set.isEmpty unMapped) -> fillBasins unMapped (id + 1) [Seq.head unMapped] basins
            | [] -> basins

        let coords = floorHeights |> Seq.map (fun kvp -> kvp.Key) |> Set.ofSeq
        let basinSizeCount = fillBasins coords 0 [] []
                              |> List.countBy (fun (_, basinId) -> basinId)


        let part2 = basinSizeCount
                     |> List.sortByDescending (fun (id, count) -> count)
                     |> List.take 3
                     |> List.map (fun (_, count) -> count)
                     |> List.reduce (*)
            
        string part1, string part2