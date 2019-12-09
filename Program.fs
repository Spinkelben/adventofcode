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
                | _   -> None
    | _      -> None

type Dummy() = class end
let getAuthToken =
    (new ConfigurationBuilder()).AddUserSecrets<Dummy>().Build().["Auth:SessionToken"]


let printResult result =
    printfn "Solution Part1: %s. \nSolution Part2: %s." (fst result) (snd result)

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

    let inputLines = getPuzzleInput day year getAuthToken forceDownloadInput
    
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


