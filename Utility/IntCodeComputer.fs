module IntCodeComputer

let private emitOutput output =
    printfn "Computer Says: %s" output

let private getParameter (program :int array) opCode pCounter index =
    let parameterMode = (opCode % (pown 10 (3 + index))) / (pown 10 (2 + index))
    match parameterMode with
    | 0 -> 
        let idx = program.[pCounter + 1 + index]
        program.[idx]
    | 1 ->
        program.[pCounter + 1 + index]
    | _ -> failwith "Pramter Mode Parsing Error!"

let private executeInstruction (program : int array) pCounter input =
    let opCode = program.[pCounter]
    let infixOperation op =
        let resultIdx = program.[pCounter + 3]
        let param1 = getParameter program opCode pCounter 0
        let param2 = getParameter program opCode pCounter 1
        Array.set program resultIdx (op param1 param2)
        program

    match opCode  % 100 with
    | 1 -> infixOperation (fun x y -> x + y), pCounter + 4, 1
    | 2 -> infixOperation (fun x y -> x * y), pCounter + 4, 2
    | 3 -> 
        Array.set program (program.[pCounter + 1]) input
        program, pCounter + 2, 3
    | 4 -> 
        emitOutput ((getParameter program opCode pCounter 0).ToString())
        program, pCounter + 2, 4
    | 5 ->
        if getParameter program opCode pCounter 0 <> 0 then
            program, getParameter program opCode pCounter 1, 5
        else
            program, pCounter + 3, 5
    | 6 ->
        if getParameter program opCode pCounter 0 = 0 then
            program, getParameter program opCode pCounter 1, 6
        else
            program, pCounter + 3, 6
    | 7 -> infixOperation (fun x y -> if x < y then 1 else 0), pCounter + 4, 7
    | 8 -> infixOperation (fun x y -> if x = y then 1 else 0), pCounter + 4, 8
    | 99 -> program, pCounter, 99
    | _ ->  failwith "Program Parsing Error!"

let executeProgram inputProgram input =
    let program = Array.copy inputProgram
    let rec executeProgram' program pCounter lastOpCode =
        if lastOpCode = 99 then
            program
        else
            let newProgram, newpCounter, opCode = executeInstruction program pCounter input
            executeProgram' newProgram newpCounter opCode

    executeProgram' program 0 0