﻿namespace Year2021

open System

module Day4 =
    let splitString (split: string) (string : string) =
        string.Split([|split|], StringSplitOptions.RemoveEmptyEntries)

    let main (input :string seq) =
        let numbers = Seq.head input 
                      |> splitString ","
                      |> Array.toList
                      |> List.map int

        let boards = Seq.tail input // Skip first line, as it has the numners
                        |> Seq.where (fun s -> s.Length > 0) // Remove empty the spacing lines
                        |> Seq.chunkBySize 5 // Each bingo plate has 5 rows
                        |> Seq.map (Array.map (fun s -> splitString " " s)) // Convert the lines to array of numbers
                        |> Seq.map array2D // Make each board a 2darray from the jagged array
                        |> Seq.map (Array2D.map (fun s -> int s, false)) // map the string numbers to ints

        

        let markNumber number board =
            Array2D.map (fun (n, isMarked) -> n, n = number || isMarked) board

        let isBingo board =
            let rows = seq { for i in 0 .. Array2D.length1 board - 1 do 
                                yield board.[i, *] 
                        }
            let columns = seq { for i in 0 .. Array2D.length2 board - 1 do
                                yield board.[*, i] 
                          }
            let isAllMarked line = Array.TrueForAll(line,  (fun (_, isMarked) -> isMarked))
            Seq.exists isAllMarked rows 
            || Seq.exists isAllMarked  columns
            
        let rec playBingo boards numbers =
            match numbers with
            | next :: ns -> 
                let marked = Seq.map (markNumber next) boards
                let winners = Seq.choose (fun b -> if isBingo b then Some b else None) marked
                if Seq.length winners > 0 then
                    Some (Seq.head winners, next)
                else 
                    playBingo marked ns
            | [] -> None

        let winner, number = playBingo boards numbers |> Option.get
        // Sum all non marked numbers of winning board
        let boardSum = Seq.sumBy (fun (number, isMarked) -> if not isMarked then number else 0 ) (Seq.cast<int * bool> winner)

        let part1 = boardSum * number

        let part2 = ""

        string part1, string part2