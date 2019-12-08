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

let private executeInstruction (program : int array) pCounter input output =
    let opCode = program.[pCounter]
    let infixOperation op =
        let resultIdx = program.[pCounter + 3]
        let param1 = getParameter program opCode pCounter 0
        let param2 = getParameter program opCode pCounter 1
        Array.set program resultIdx (op param1 param2)
        program

    match opCode  % 100 with
    | 1 -> infixOperation (fun x y -> x + y), pCounter + 4, 1, input, output
    | 2 -> infixOperation (fun x y -> x * y), pCounter + 4, 2, input, output
    | 3 -> 
        match input with
        | i :: is ->
            Array.set program (program.[pCounter + 1]) i
            program, pCounter + 2, 3, is, output
        | [] ->
            program, pCounter, 100, input, output
    | 4 -> 
        let out = getParameter program opCode pCounter 0
        emitOutput (out.ToString())
        program, pCounter + 2, 4, input, out :: output
    | 5 ->
        if getParameter program opCode pCounter 0 <> 0 then
            program, getParameter program opCode pCounter 1, 5, input, output
        else
            program, pCounter + 3, 5, input, output
    | 6 ->
        if getParameter program opCode pCounter 0 = 0 then
            program, getParameter program opCode pCounter 1, 6, input, output
        else
            program, pCounter + 3, 6, input, output 
    | 7 -> infixOperation (fun x y -> if x < y then 1 else 0), pCounter + 4, 7, input, output
    | 8 -> infixOperation (fun x y -> if x = y then 1 else 0), pCounter + 4, 8, input, output
    | 99 -> program, pCounter, 99, input, output
    | _ ->  failwith "Program Parsing Error!"

let executeProgram inputProgram (input :list<int>) pCounter =
    let program = Array.copy inputProgram
    let pStart = match pCounter with
                 | Some v -> v
                 | None -> 0

    let rec executeProgram' program pCounter lastOpCode programInput programOutput =
        if lastOpCode = 99 then // Terminated
            (programOutput, pCounter, true), program
        else if (lastOpCode) = 100 then // Waiting for input 
            (programOutput, pCounter, false), program
        else
            let newProgram, newpCounter, opCode, newInput, newOutput = executeInstruction program pCounter programInput programOutput
            executeProgram' newProgram newpCounter opCode newInput newOutput

    executeProgram' program pStart 0 input list.Empty