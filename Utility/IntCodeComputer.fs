module IntCodeComputer

type ParamterType =
| Immidiate
| Position
| Relative

let private emitOutput output =
    printfn "Computer Says: %s" output

let private getValue (program : int64 array) memory (index : int64)=
    if index >= int64 program.Length then
        if not (Map.containsKey index memory) then
            0L
        else
            memory.Item index
    else 
        program.[(int index)]

let private setValue (program : int64 array) memory (index : int64) value =
    if index >= int64 program.Length then
        Map.add index value memory
    else
        Array.set program (int index) value
        memory

let private getParamterMode opCode index =
    match (opCode % (pown 10 (3 + index))) / (pown 10 (2 + index)) with 
    | 0 -> Position
    | 1 -> Immidiate
    | 2 -> Relative
    | _ -> failwith "Pramter Mode Parsing Error!"

let private getParameter (program :int64 array) opCode pCounter index baseOffset memory =
    match getParamterMode opCode index with
    | Position -> 
        let idx = program.[pCounter + 1 + index]
        getValue program memory idx
    | Immidiate ->
        program.[pCounter + 1 + index]
    | Relative ->
        let idx = program.[pCounter + 1 + index] + baseOffset
        getValue program memory idx

let private getDestinationIndex (program : int64 array) opCode pCounter index baseOffset =
    match getParamterMode opCode index with
    | Position ->
        program.[pCounter + 1 + index]
    | Relative ->
        program.[pCounter + 1 + index] + baseOffset
    | Immidiate ->
        failwith "Cannot write to literal"

let private executeInstruction ((program : int64 array), memory) pCounter input output baseOffset =
    let opCode = int program.[pCounter]
    let infixOperation op =
        let resultIdx = getDestinationIndex program opCode pCounter 2 baseOffset
        let param1 = getParameter program opCode pCounter 0 baseOffset memory
        let param2 = getParameter program opCode pCounter 1 baseOffset memory
        program, setValue program memory resultIdx (op param1 param2)

    match opCode  % 100 with
    | 1 -> infixOperation (fun x y -> x + y), pCounter + 4, 1, input, output, baseOffset
    | 2 -> infixOperation (fun x y -> x * y), pCounter + 4, 2, input, output, baseOffset
    | 3 -> 
        match input with
        | i :: is ->
            let idx = getDestinationIndex program opCode pCounter 0 baseOffset
            let newMemory = setValue program memory idx i
            (program, newMemory), pCounter + 2, 3, is, output, baseOffset
        | [] ->
            (program, memory), pCounter, 100, input, output, baseOffset
    | 4 -> 
        let out = getParameter program opCode pCounter 0 baseOffset memory
        emitOutput (out.ToString())
        (program, memory), pCounter + 2, 4, input, out :: output, baseOffset
    | 5 ->
        if getParameter program opCode pCounter 0 baseOffset memory <> 0L  then
            let nextAddress = getParameter program opCode pCounter 1 baseOffset memory
            (program, memory), int nextAddress, 5, input, output, baseOffset
        else
            (program, memory), pCounter + 3, 5, input, output, baseOffset
    | 6 ->
        if getParameter program opCode pCounter 0 baseOffset memory = 0L then
            let nextAddress = getParameter program opCode pCounter 1 baseOffset memory
            (program, memory), int nextAddress, 6, input, output, baseOffset
        else
            (program, memory), pCounter + 3, 6, input, output, baseOffset
    | 7 -> infixOperation (fun x y -> if x < y then 1L else 0L), pCounter + 4, 7, input, output, baseOffset
    | 8 -> infixOperation (fun x y -> if x = y then 1L else 0L), pCounter + 4, 8, input, output, baseOffset
    | 9 -> 
        let newBaseOffset = baseOffset + (getParameter program opCode pCounter 0 baseOffset memory)
        (program, memory), pCounter + 2, 9, input, output, newBaseOffset
    | 99 -> (program, memory), pCounter, 99, input, output, baseOffset
    | _ ->  failwith "Program Parsing Error!"

let executeProgram inputProgram (input :list<int64>) pCounter inputMemory baseOffset =
    let program = Array.copy inputProgram
    let pStart = match pCounter with
                 | Some v -> v
                 | None -> 0
    let memory = 
         match inputMemory with
         | Some m -> m
         | None -> Map.empty
    let baseStart = match baseOffset with
                    | Some v -> v
                    | None -> 0L


    let rec executeProgram' (program, memory) pCounter lastOpCode programInput programOutput baseOffset =
        if lastOpCode = 99 then // Terminated
            (List.rev programOutput, pCounter, true), (program, memory)
        else if (lastOpCode) = 100 then // Waiting for input 
            (List.rev programOutput, pCounter, false), (program, memory)
        else
            let (newProgram, newMemory), newpCounter, opCode, newInput, newOutput, newBaseOffset =
                executeInstruction (program, memory) pCounter programInput programOutput baseOffset
            executeProgram' (newProgram, newMemory) newpCounter opCode newInput newOutput newBaseOffset

    executeProgram' (program, memory) pStart 0 input list.Empty baseStart