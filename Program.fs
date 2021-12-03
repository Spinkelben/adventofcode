// Learn more about F# at http://fsharp.org

open System
open InputFetcher
open Microsoft.Extensions.Configuration
open System.Diagnostics

let private puzzleMap year day =
    match year with
    | "2018" -> match day with
                | "1" -> Some Year2018Day1.main
                | "2" -> Some Year2018Day2.main
                | _   -> None
    | "2019" -> match day with
                | "1" -> Some Year2019Day1.main
                | "2" -> Some Year2019Day2.main
                | "3" -> Some Year2019Day3.main
                | "4" -> Some Year2019Day4.main
                | "5" -> Some Year2019Day5.main
                | "6" -> Some Year2019Day6.main
                | "7" -> Some Year2019Day7.main
                | "8" -> Some Year2019Day8.main
                | "9" -> Some Year2019Day9.main
                | "10" -> Some Year2019Day10.main
                | "11" -> Some Year2019Day11.main
                | "12" -> Some Year2019Day12.main
                | "13" -> Some Year2019Day13.main
                | "14" -> Some Year2019Day14.main
                | "15" -> Some Year2019Day15.main
                | "16" -> Some Year2019Day16.main
                | "17" -> Some Year2019Day17.main
                | "18" -> Some Year2019Day18.main
                | "19" -> Some Year2019Day19.main
                | "21" -> Some Year2019Day21.main
                | "22" -> Some Year2019Day22.main
                | "23" -> Some Year2019Day23.main
                | _   -> None
    | "2021" -> match day with
                | "1" -> Some Year2021.Day1.main
                | "2" -> Some Year2021.Day2.main
                | "3" -> Some Year2021.Day3.main
                | _ -> None
    | _      -> None

type Dummy() = class end
let getAuthToken =
    (new ConfigurationBuilder()).AddUserSecrets<Dummy>().Build().["Auth:SessionToken"]


let printResult result =
    printfn "Solution Part1: %s \nSolution Part2: %s" (fst result) (snd result)

[<EntryPoint>]
let main argv =
    let year = argv.[0]
    let day = argv.[1]
    printfn "Running Puzzle Year %s Day %s" year day
    let forceDownloadInput =
        if argv.Length >= 3 then
            bool.Parse(argv.[2])
        else
            false

    let inputLines = getPuzzleInput day year getAuthToken forceDownloadInput false
    
    let puzzleSolver = puzzleMap year day

    let stopWatch = new Stopwatch();
    stopWatch.Start()
    match puzzleSolver with
    | Some solver -> 
        inputLines 
        |> solver 
        |> printResult
    | None -> 
        printfn "No solver configured for Year %s Day %s" year day
    stopWatch.Stop()
    printf "Runtime was %s" (stopWatch.Elapsed.ToString())
    0


