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

        let getRange x1 x2 =
            if x1 < x2 then
                [x1 .. 1 .. x2]
            else 
                [x1 .. -1 ..  x2]

        let getPointsInLine (p1, p2) =
            match p1, p2 with
            | (x1, y1),(x2, y2) when x1=x2 -> Some [for i in getRange y1 y2 -> (x1, i)]
            | (x1, y1),(x2, y2) when y1=y2 -> Some [for i in getRange x1 x2 -> (i, y1)]
            | _ -> None

        let getPointsInLinePart2 ((x1, y1), (x2, y2)) =
            let straightLines = getPointsInLine ((x1,y1), (x2, y2))
            if Option.isSome straightLines then
                straightLines
            else 
                Some (List.zip (getRange x1 x2) (getRange y1 y2))

        let ventGasFinder lineResolver input =
            input
            |> List.ofSeq
            |> List.map readCloudLine // Parse a line into a nested tuple representing the cloud line start and end
            |> List.map (Option.bind lineResolver) // Expand the vertical and horizontal lines into points
            |> List.choose id // Filter out the obligue lines
            |> List.concat // Flatten the list of list, we don't care about lines, as we now have the points
            |> List.groupBy id // Group by coordinates
            |> List.where (fun (coord, list)-> List.length list > 1) // Filter coordinates with less than 2 lines
            |> List.length // Count number of coordinates

        let part1 = ventGasFinder getPointsInLine input

        let part2 = ventGasFinder getPointsInLinePart2 input

        string part1, string part2