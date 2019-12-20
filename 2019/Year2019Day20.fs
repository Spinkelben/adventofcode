module Year2019Day20
open System

let getPortalSymbols sequence idxFun =
    sequence
    |> Seq.mapi (fun idx (p1, p2) -> (idxFun idx), p1.ToString() + p2.ToString())
    |> Seq.filter (fun (_, s) -> 
        match s with
        | "#." | ".#" | ".." | "##" | "  " | "" | "\r\r" -> false
        | x when x.Trim().Length > 1 -> true
        | _ -> false)

let getColumn (jagged : array<string>) idx =
    [ for line in jagged do
        line.[idx] ]

let main (input : string seq) =
    let maze = Seq.head input
    let mazeLines = maze.Split("\n") |> Array.map (fun s -> s.Replace("\r", ""))
    let width = mazeLines.[0].Length
    let topPortals = 
        getPortalSymbols 
            (Seq.zip mazeLines.[0] mazeLines.[1]) 
            (fun i -> (i, 1))
    let bottomPortals = 
        getPortalSymbols 
            (Seq.zip mazeLines.[mazeLines.Length - 2] mazeLines.[mazeLines.Length - 1]) 
            (fun i -> (i, mazeLines.Length - 2))
    let leftPortals = 
        getPortalSymbols 
            (Seq.zip (getColumn mazeLines 0) (getColumn mazeLines 1))
            (fun i -> (1, i))
    let rightPortals = 
        getPortalSymbols 
            (Seq.zip (getColumn mazeLines (width - 2)) (getColumn mazeLines (width - 1)))
            (fun i -> (width - 2), i)

    let doughnutWidth = 
        (mazeLines.[mazeLines.Length / 2] 
        |> Seq.map (fun c -> 
            match c with
            | '.' | '#' -> 1
            | _ -> 0)
        |> Seq.sum) / 2

    let innerTopPortals =
        getPortalSymbols
            (Seq.zip mazeLines.[doughnutWidth + 2] mazeLines.[doughnutWidth + 3])
            (fun i -> (i, doughnutWidth + 2))
    let innerBottomPortals =
        getPortalSymbols
            (Seq.zip mazeLines.[mazeLines.Length - (doughnutWidth + 4)] mazeLines.[mazeLines.Length - (doughnutWidth + 3)])
            (fun i -> (i, mazeLines.Length - (doughnutWidth + 3)))
    let innerLeftPortals =
        getPortalSymbols 
            (Seq.zip (getColumn mazeLines (doughnutWidth + 2)) (getColumn mazeLines (doughnutWidth + 3)))
            (fun i -> doughnutWidth + 2, i)
    let innerRightPortals =
        getPortalSymbols 
            (Seq.zip (getColumn mazeLines (width - (doughnutWidth + 4))) (getColumn mazeLines (width - (doughnutWidth + 3))))
            (fun i -> width - (doughnutWidth + 3), i)

    let innerCoordToNameMap = Seq.concat [innerBottomPortals; innerLeftPortals; innerRightPortals; innerTopPortals] |> Map.ofSeq
    let innerNameToCoordMap = 
        Seq.concat [innerBottomPortals; innerLeftPortals; innerRightPortals; innerTopPortals] 
        |> Seq.map (fun (c, v) -> v, c)
        |> Map.ofSeq

    let outerCoordToNameMap = Seq.concat [bottomPortals; leftPortals; rightPortals; topPortals] |> Map.ofSeq
    let outerNameToCoordMap = 
        Seq.concat [bottomPortals; leftPortals; rightPortals; topPortals] 
        |> Seq.map (fun (c, v) -> v, c)
        |> Map.ofSeq

    let tryJumpPortal coord =
        if innerCoordToNameMap.ContainsKey coord then
            Some (outerNameToCoordMap.Item (innerCoordToNameMap.Item coord))
        else if outerCoordToNameMap.ContainsKey coord then
            Some (innerNameToCoordMap.Item (outerCoordToNameMap.Item coord))
        else 
            None

    let part1 = 
        ""

    let part2 = 
        ""

    part1, part2