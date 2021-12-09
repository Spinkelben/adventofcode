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

        let part1 = 
            let floorHeights = floorMap input // Parse input to coordinate to height map
            let isLowPoint arg =
                let ((x,y), value) = (|KeyValue|) arg
                let surroundings = [ 
                    (x + 1, y); 
                    (x - 1, y); 
                    (x, y + 1); 
                    (x, y - 1); ]
                // Get surrounding coordinates that are in bounds
                List.choose (fun c -> Map.tryFind c floorHeights) surroundings 
                |> List.forall (fun s -> value < s)

            Seq.where isLowPoint floorHeights // Get low points
            |> Seq.map (fun kvp -> kvp.Value + 1) // Add one to get danger rating
            |> Seq.sum
            
        let part2 = ""

        string part1, string part2