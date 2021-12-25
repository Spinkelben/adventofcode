namespace Year2021

open System

module ParseUtil = 
    type ParseResult<'a> = 
    | Success of 'a
    | Failure of string

    type Parser<'T> = Parser of (string -> ParseResult<'T * string>)

    let pchar charToMatch  = 
        let innerFn str =
            if String.IsNullOrEmpty(str) then
                Failure "No more input"
            else
                let first = str.[0]
                if first = charToMatch then
                    let remaining = str.[1..]
                    Success (charToMatch, remaining)
                else 
                    let msg = sprintf "Expecting '%c'. Got '%c'" charToMatch first
                    Failure msg
        Parser innerFn

    let run parser input = 
        let (Parser innerFun) = parser
        innerFun input

    let andThen parser1 parser2 = 
        let innerFn input = 
            let result1 = run parser1 input

            match result1 with
            | Failure err -> 
                Failure err
            | Success (value1, remainig1) ->
                let result2 = run parser2 remainig1

                match result2 with
                | Failure err -> 
                    Failure err
                | Success (value2, remainig2) -> 
                    let newValue = (value1, value2)
                    Success (newValue, remainig2)

        Parser innerFn

    let ( .>>. ) = andThen

    let orElse parser1 parser2 = 
        let innerFn input = 
            let result1 = run parser1 input

            match result1 with
            | Success _ -> result1
            | Failure _ -> 
                run parser2 input

        Parser innerFn

    let ( <|> ) = orElse

    let choice listOfParsers = 
        List.reduce ( <|> ) listOfParsers

    let anyOf listOfChars =
        listOfChars
        |> List.map pchar
        |> choice

    let parseLowerCase =
        anyOf ['a' .. 'z']

    let parseDigit = 
        anyOf ['0' .. '9']

    let mapP f parser = 
        let innerFn input =
            let result = run parser input

            match result with 
            | Success (value, remaining) -> 
                let newValue = f value
                Success (newValue, remaining)
            | Failure err -> Failure err

        Parser innerFn

    let ( <!> ) = mapP

    let ( |>> ) x f = mapP f x  

    let returnP x = 
        let innerFn input = 
            Success (x, input)

        Parser innerFn

    let applyP fP xP = 
        (fP .>>. xP)
        |> mapP (fun (f,x) -> f x)

    let ( <*> ) = applyP

    let lift2 f xP yP =
        returnP f <*> xP <*> yP

    let addP = 
        lift2 (+)

    let startsWith (str :string) (prefix :string) = 
        str.StartsWith(prefix)

    let startsWithP =
        lift2 startsWith

    let rec sequence parserList = 
        let cons head tail = head :: tail
        let consP = lift2 cons

        match parserList with
        | [] -> 
            returnP []
        | head :: tail ->
            consP head (sequence tail)

    let charListToStr charList =
        charList |> List.toArray |> String

    let pstring str = 
        str 
        |> List.ofSeq 
        |> List.map pchar
        |> sequence
        |> mapP charListToStr

    let rec parseZeroOrMore parser input = 
        let firstResult = run parser input
        match firstResult with
        | Failure _ -> ([], input)
        | Success (firstvalue, inputAfterFirstParse) -> 
            let (subsequentValues, remainingInput) = parseZeroOrMore parser inputAfterFirstParse
            (firstvalue::subsequentValues, remainingInput)

    let many parser =
        let innerFn input =
            Success (parseZeroOrMore parser input)

        Parser innerFn

    let many1 parser =
        let innerFn input =
            let firstResult = run parser input
            match firstResult with
            | Failure err -> Failure err
            | Success (firstValue, inputAfterFirstParse) -> 
                let (subsequentValues, remainingInput) = parseZeroOrMore parser inputAfterFirstParse
                Success (firstValue :: subsequentValues, remainingInput)

        Parser innerFn

module Day18 =
    let main (input :string seq) =
            
        let part1 = ""

        let part2 = ""
                    
        string part1, string part2