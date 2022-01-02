namespace Year2021

open System
open ParseUtil
    

module Day18 =
    type SNumber =    
    | RNumber of int
    | SPair of SNumber * SNumber

    let numberParser = 
        let parser = ParseUtil.pint

        parser |>> RNumber

    let sPair, sPairRef = createParserForwardedToRef<SNumber>()

    let pairParser =
        let left = pchar '['
        let right = pchar ']'
        let sep = pchar ','
        let pair = numberParser <|> sPair

        left >>. pair .>> sep .>>. pair .>> right <?> "pair" |>> SPair

    sPairRef.Value <- pairParser

    let getTree p =
        match p with
        | Success (tree : SNumber, input) -> tree
        | Failure _ -> 
            printResult p 
            failwith "Failure!"

    let rec addLeftMostNumber sn numberToAdd = 
        match sn with
        | RNumber v -> RNumber (v + numberToAdd)
        | SPair (l, r) -> SPair(addLeftMostNumber l numberToAdd, r)

    let rec addRightMostNumber sn numberToAdd =
        match sn with
        | RNumber v -> RNumber (v + numberToAdd)
        | SPair (l, r) -> SPair (l, addRightMostNumber r numberToAdd)

    let explodeNumber n =
        let rec explodeNumber'  (n : SNumber) depth =
            match n with
            | SPair (RNumber l, RNumber r) -> // Only pairs of numbers can be exploded
                if depth > 4 then // If deep enough, explode
                    RNumber 0, (Some l, Some r), true
                else
                    SPair (RNumber l, RNumber r), (None, None), false
            | RNumber n -> RNumber n, (None, None), false
            | SPair (l, r) -> 
                let l', (addLeft, addRight), wasExploded = explodeNumber' l (depth + 1)
                if wasExploded then // If left was exploded, don't do right
                    match addRight with
                    | Some v -> SPair (l', addLeftMostNumber r v), (addLeft, None), true
                    | None -> SPair (l', r), (addLeft, addRight), true
                else // Left was not exploded try right
                    let r', (addLeft, addRight), wasExploded = explodeNumber' r (depth + 1)
                    if wasExploded then
                        match addLeft with
                        | Some v -> SPair (addRightMostNumber l' v, r'), (None, addRight), true
                        | None -> SPair (l', r'), (addLeft, addRight), true
                    else
                        SPair (l', r'), (addLeft, addRight), false
                
        let n', _, wasExploded = explodeNumber' n 1
        n', wasExploded

    let splitNumber n =
        let rec splitNumber' n wasSplit =
            if wasSplit then 
                n, wasSplit
            else
                match n with
                | RNumber v when v >= 10 -> 
                    let split = double v / 2.0
                    SPair (floor split |> int |> RNumber, ceil split |> int |> RNumber), true
                | RNumber _ -> n, wasSplit
                | SPair (l, r) -> 
                    let l', lSplit = splitNumber' l false
                    if lSplit then
                        SPair (l', r), true
                    else
                        let r', rSplit = splitNumber' r false
                        SPair (l', r'), rSplit

        splitNumber' n false

    let rec reduceNumber n = 
        let n', wasExploded = explodeNumber n
        if wasExploded then
            reduceNumber n'
        else 
            let n', wasSplit = splitNumber n
            if wasSplit then
                reduceNumber n'
            else
                n'

    let parseInput i =
        run pairParser i 
        |> getTree

    let addNumbers n1 n2 = 
        SPair (n1, n2) |> reduceNumber

    let rec getMagnitude n = 
        match n with
        | RNumber n -> n
        | SPair (l, r) -> 3 * (getMagnitude l) + 2 * (getMagnitude r)

    let main (input :string seq) =
        let numbers = input 
                      |> Seq.map parseInput
        let part1 = numbers
                    |> Seq.reduce addNumbers
                    |> getMagnitude

        let part2 = Seq.allPairs numbers numbers
                    |> Seq.map (fun (x,y) ->  
                        if x = y then
                            Int32.MinValue
                        else
                            addNumbers x y
                            |> getMagnitude)
                    |> Seq.max
                    
        string part1, string part2