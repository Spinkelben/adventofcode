
module ParseUtil 
    open System

    type Position = {
        line: int
        column: int
    }

    let initialPos = {line=0; column=0}

    let incrCol (pos: Position) =
        {pos with column=pos.column + 1}

    let incrLine pos =
        {line=pos.line + 1; column=0}

    type InputState = {
        lines : string[]
        position : Position
    }

    module InputState =
        let fromStr str =
            if (String.IsNullOrEmpty(str)) then
                {lines=[||]; position=initialPos}
            else
                let seperators = [| "\r\n"; "\n" |]
                let lines = str.Split(seperators, StringSplitOptions.None)
                {lines=lines; position=initialPos}

        let currentLine inputState =
            let linePos = inputState.position.line
            if linePos < inputState.lines.Length then
                inputState.lines.[linePos]
            else 
                "end of file"

        let nextChar input = 
            let linePos = input.position.line
            let colPos = input.position.column
            if linePos >= input.lines.Length then
                input, None
            else 
                let currentLine = currentLine input
                if colPos < currentLine.Length then
                    let char = currentLine.[colPos]
                    let newPos = incrCol input.position
                    let newState = {input with position=newPos}
                    newState, Some char
                else
                    let char = '\n'
                    let newPos = incrLine input.position
                    let newState = {input with position=newPos}
                    newState, Some char

    let rec readAllChars input = 
        [
            let remainingInput, charOpt = InputState.nextChar input
            match charOpt with
            | None -> ()
            | Some ch -> 
                yield ch
                yield! readAllChars remainingInput
        ]

    type ParserPosition = {
        currentLine : string
        line : int
        column : int
    }

    let parserPositionFromInputState (inputState:InputState) = {
        currentLine = InputState.currentLine inputState
        line = inputState.position.line
        column = inputState.position.column
    }

    type ParserLabel = string
    type ParserError = string

    type ParseResult<'a> = 
    | Success of 'a
    | Failure of ParserLabel * ParserError * ParserPosition

    type Parser<'a> = { 
        parseFn:  (InputState -> ParseResult<'a * InputState>) 
        label: ParserLabel
        }

    let printResult result = 
        match result with
        | Success (value, _input) -> printfn "%A" value
        | Failure (label, error, parserPos) -> 
            let errorLine = parserPos.currentLine
            let colPos = parserPos.column
            let linePos = parserPos.line
            let failureCaret = sprintf "%*s^%s" colPos "" error
            printfn "Line:%i Col:%i Error parsing %s\n%s\n%s" linePos colPos label errorLine failureCaret

    let setLabel parser newLabel = 
        let newInnerFn input =
            let result = parser.parseFn input
            match result with
            | Success s -> Success s
            | Failure (oldLabel, err, pos) ->
                Failure (newLabel, err, pos)

        {parseFn=newInnerFn; label=newLabel}

    let ( <?> ) = setLabel

    let getLabel parser = 
        parser.label
            
    let satisfy predicate label = 
        let innerFn input = 
            let remainingInput, charOpt = InputState.nextChar input
            match charOpt with
            | None ->
                let err = "No more input"
                let pos = parserPositionFromInputState input
                Failure (label, err, pos)
            | Some first ->
                if predicate first then
                    Success (first, remainingInput)
                else
                    let err = sprintf "Unexpected '%c'" first
                    let pos = parserPositionFromInputState input
                    Failure (label, err, pos)
        {parseFn=innerFn; label=label}

    let pchar charToMatch  =
        let predicate ch = (ch = charToMatch)
        let label = sprintf "'%c'" charToMatch
        satisfy predicate label

    let runOnInput (parser: Parser<_>) input =
        parser.parseFn input

    let run (parser: Parser<_>) inputStr = 
        runOnInput parser (InputState.fromStr inputStr)

    let orElse parser1 parser2 = 
        let label = sprintf "%s orElse %s" (getLabel parser1) (getLabel parser2)
        let innerFn input = 
            let result1 = runOnInput parser1 input

            match result1 with
            | Success _ -> result1
            | Failure (_,err, pos) -> 
                let result2 = runOnInput parser2 input

                match result2 with
                | Success _ -> result2
                | Failure (_,err, pos) -> Failure (label, err, pos)

        { parseFn=innerFn; label=label }

    let ( <|> ) = orElse

    let choice listOfParsers = 
        List.reduce ( <|> ) listOfParsers

    let anyOf listOfChars =
        let label = sprintf "anyOf %A" listOfChars
        listOfChars
        |> List.map pchar
        |> choice
        <?> label

    let parseLowerCase =
        anyOf ['a' .. 'z']

    let digitChar = 
        let predicate = Char.IsDigit
        let label = "digit"
        satisfy predicate label

    let whitespaceChar =
        let predicate = Char.IsWhiteSpace
        let label = "whitespace"
        satisfy predicate label


    let bindP f p =
        let label = "unknown"
        let innerFn input = 
            let result1 = runOnInput p input
            match result1 with
            | Failure (label, err, pos) -> Failure (label, err, pos)
            | Success (value1, remainingInput) ->
                let p2 = f value1 
                runOnInput p2 remainingInput

        { parseFn=innerFn; label=label }

    let ( >>= ) p f = bindP f p  

    let returnP x = 
        let label = sprintf "%A" x
        let innerFn input = 
            Success (x, input)
        {parseFn= innerFn;label=label}

    let andThen parser1 parser2 = 
        let label = sprintf "%s andThen %s" (getLabel parser1) (getLabel parser2) 
        parser1 >>= (fun p1Result ->
          parser2 >>= (fun p2Result ->
            returnP (p1Result,p2Result) ))
        <?> label

    let ( .>>. ) = andThen

    let mapP f = 
        bindP (f >> returnP)

    let ( <!> ) = mapP
    
    let ( |>> ) x f = mapP f x

    let applyP fP xP = 
        fP >>= (fun f -> 
        xP >>= (fun x -> returnP (f x) ))

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
        let label = str
        str 
        |> List.ofSeq 
        |> List.map pchar
        |> sequence
        |> mapP charListToStr
        <?> label

    let rec parseZeroOrMore parser input = 
        let firstResult = runOnInput parser input
        match firstResult with
        | Failure _ -> ([], input)
        | Success (firstvalue, inputAfterFirstParse) -> 
            let (subsequentValues, remainingInput) = parseZeroOrMore parser inputAfterFirstParse
            (firstvalue::subsequentValues, remainingInput)

    let many parser =
        let innerFn input =
            Success (parseZeroOrMore parser input)

        { parseFn=innerFn; label="unknown" }

    let many1 parser =
        let label = "unknown"
        let innerFn input =
            let firstResult = runOnInput parser input
            match firstResult with
            | Failure (label,err, pos) -> Failure (label, err, pos)
            | Success (firstValue, inputAfterFirstParse) -> 
                let (subsequentValues, remainingInput) = parseZeroOrMore parser inputAfterFirstParse
                Success (firstValue :: subsequentValues, remainingInput)

        { parseFn=innerFn; label=label }

    let opt p = 
        let some = p |>> Some
        let none = returnP None
        some <|> none

    let (.>>) p1 p2 = 
        p1 .>>. p2 
        |> mapP fst

    let (>>.) p1 p2 =
        p1 .>>. p2 
        |> mapP snd
    let sepBy1 p sep = 
        let sepThenp = sep >>. p
        p .>>. many sepThenp
        |>> fun (p, pList) -> p::pList

    let sepBy p sep = 
        sepBy1 p sep <|> returnP []

    let manyChars cp = 
        many cp
        |>> charListToStr

    let manyChars1 cp =
        many1 cp
        |>> charListToStr

    let pint =
        let label = "integer"
        let resultToInt (sign, digits) =
            let i = digits |> int
            match sign with
            | Some ch -> -i
            | None -> i

        let digits = manyChars1 digitChar
        opt (pchar '-') .>>. digits |>> resultToInt <?> label

    let spaces = many whitespaceChar

    let spaces1 = many1 whitespaceChar

    let pfloat = 
        let label = "float"

        let resultToFloat (((sign, digits1),point), digits2) =
            let fl = sprintf "%s.%s" digits1 digits2 |> float
            match sign with
            | Some ch -> -fl
            | None -> fl

        let digits = manyChars1 digitChar
        opt (pchar '-') .>>. digits .>>. (pchar '.') .>>. digits
        |> mapP resultToFloat
        <?> label

    let createParserForwardedToRef<'a>() =
        let dummyParser : Parser<'a> = 
            let innerFn _ = 
                failwith "unfixed forward parser"
            {parseFn=innerFn; label="dummy"}

        let parserRef = ref dummyParser

        let innerFn input = 
            runOnInput parserRef.Value input

        let wrapperParser = {parseFn=innerFn; label="unkown"}

        wrapperParser, parserRef