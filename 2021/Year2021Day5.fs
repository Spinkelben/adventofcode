namespace Year2021

open System

module Day5 =
    let splitString (seperator : string) (s : string) =
        s.Split seperator

    let main (input :string seq) =
        let readCoordinate s =
            let numbers = s 
                        |> String.filter (fun c -> c |> Char.IsWhiteSpace |> not)
                        |> splitString ","
                        |> List.ofArray
                        |> List.map int
            match numbers with
            | n1::n2::[] -> Some (n1, n2)
            | _ -> None

        let readCloudLine s =
            let points = s
                        |> splitString "->"
                        |> List.ofArray
                        |> List.map readCoordinate
            match points with
            | (Some p1)::(Some p2)::[] -> Some (p1, p2)
            | _ -> None

        let getPointsInLine (p1, p2) =
            let getRange x1 x2 =
                if x1 < x2 then
                    [x1 .. x2]
                else 
                    [x2 .. x1]

            match p1, p2 with
            | (x1, y1),(x2, y2) when x1=x2 -> Some [for i in getRange y1 y2 -> (x1, i)]
            | (x1, y1),(x2, y2) when y1=y2 -> Some [for i in getRange x1 x2 -> (i, y1)]
            | _ -> None

        let part1 = input
                    |> List.ofSeq
                    |> List.map readCloudLine // Parse a line into a nested tuple representing the cloud line start and end
                    |> List.map (Option.bind getPointsInLine) // Expand the vertical and horizontal lines into points
                    |> List.choose id // Filter out the obligue lines
                    |> List.concat // Flatten the list of list, we don't care about lines, as we now have the points
                    |> List.groupBy id // Group by coordinates
                    |> List.where (fun (coord, list)-> List.length list > 1) // Filter coordinates with less than 2 lines
                    |> List.length // Count number of coordinates

        let part2 = ""

        string part1, string part2