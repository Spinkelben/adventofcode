
module InputFetcher

open System
open System.IO
open System.Net.Http
open System.Net

let private cookieContainer = new CookieContainer()
let private httpClientHandler = new HttpClientHandler()
let private httpClient = new HttpClient(httpClientHandler)
let private baseUrl = new Uri("https://adventofcode.com")
let private cacheDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)

let internal construcUrl year day = new Uri(baseUrl, Path.Combine(year, "day", day, "input"))

let private createOrGetDirectory path =
    if not(Directory.Exists(path)) then
        (Directory.CreateDirectory(path), true)
    else
        (new DirectoryInfo(path), false)

let private getPuzzleFilePath year day token =
    new FileInfo(Path.Combine(cacheDir, token, year, day + ".txt"))

let private setToken token =
    if cookieContainer.Count = 0 then
      let cookie = new Cookie("session", token)
      cookie.Domain <- baseUrl.Host
      cookieContainer.Add(cookie)
      httpClientHandler.CookieContainer <- cookieContainer

let private getInputFileInfo day year token =
    let inputFile = getPuzzleFilePath day year token
    createOrGetDirectory inputFile.Directory.FullName |> ignore
    inputFile

let private getInputFromWeb day year token =
    printfn "Fetching input from website" 
    setToken token |> ignore
    async {
        let! result = httpClient.GetAsync(construcUrl year day) |> Async.AwaitTask
        let! content = result.Content.ReadAsStringAsync() |> Async.AwaitTask
        let file = getInputFileInfo day year token
        let splitLines = content.Split('\n')
        File.WriteAllLinesAsync(file.FullName, splitLines) |> Async.AwaitTask |> ignore
        return splitLines
    }

let private preProcessLines removeEmptyLines lines =
    lines 
    |> Seq.map (fun (s:string) -> s.Trim())
    |> Seq.filter (fun s -> s.Length > 0 || not removeEmptyLines)

let getPuzzleInput day year token forceDownload removeEmptyLines =
    let fileInfo = getInputFileInfo day year token
    if fileInfo.Exists && not forceDownload then
        File.ReadAllLines(fileInfo.FullName)
        |> preProcessLines removeEmptyLines
    else
        getInputFromWeb day year token |> Async.RunSynchronously
        |> preProcessLines removeEmptyLines
