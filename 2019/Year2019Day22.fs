module Year2019Day22

open System

type Technique =
    | DealNewStack
    | Cut of int
    | DealWithIncrement of int

let doTechnique cards technique =
    match technique with
    | DealNewStack -> 
        List.rev cards
    | Cut number -> 
        let number = 
            if number > 0 then
                number
            else
                cards.Length + number 
        List.append (List.skip number cards) (List.take number cards)
    | DealWithIncrement inc ->
        let table = Array.create cards.Length -1
        let sequence = seq { for i in 0 .. cards.Length - 1 -> (i * inc) % cards.Length }
        Seq.zip sequence cards
        |> Seq.iter (fun (idx, value) -> Array.set table idx value)
        List.ofArray table



let shuffleDeck commands size =
    let cards = [ for i in 0 .. size -> i ]
    let rec shuffleDeck' commands cards =
        match commands with
        | [] -> cards
        | com :: coms -> 
            let nextCards = doTechnique cards com
            shuffleDeck' coms nextCards

    shuffleDeck' commands cards

let convertCardsTostring cards =
    String.Join(" ", (List.map (fun c -> c.ToString()) cards))

let parseCommand (command : string) =
    match command with
    | c when (c.StartsWith("deal into new stack")) -> DealNewStack
    | c when (c.StartsWith("cut")) -> Cut (int (c.Split(" ").[1]))
    | c when (c.StartsWith("deal with increment")) -> DealWithIncrement (int (c.Replace("deal with increment ", "")))
    | _ -> failwith ("Unknow technique: " + command)

let main input =
    let commands = Seq.map parseCommand input

    let part1 = 
        let result = shuffleDeck (List.ofSeq commands) 10006
        List.findIndex (fun c -> c = 2019 ) result
        

    let part2 = 
        ""

    part1.ToString(), part2