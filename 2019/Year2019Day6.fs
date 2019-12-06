module Year2019Day6

type OrbitTree =
    | Leaf of name : string
    | Orbits of name : string * sattelites: Map<string, OrbitTree> 


let parsePairAsOrbitTree (pairLine : string) =
    let bodies = pairLine.Split('c')
    let sattelite = 
        Map.empty.Add(bodies.[1], Leaf (name =  bodies.[1]))

    Orbits(name = bodies.[1], sattelites = sattelite)
  

let findParent nodeName tree =
    let rec findParent' nodeName tree childList =
        match childList with
        | c :: cs ->
            match findParent' nodeName c [] with
            | Some parent -> Some parent
            | None -> findParent' nodeName c cs
        | [] -> 
            match tree with
            | Leaf _ -> None
            | Orbits (name, sattelites) ->
                if sattelites.ContainsKey(nodeName) then
                    Some tree
                else
                    let sattliteList = 
                        Map.toList sattelites
                        |> List.map (fun (key, value) -> value)
                    findParent' nodeName tree sattliteList

    findParent' nodeName tree []

let addChild tree child nodeName  =
    let rec addChild' tree nodeName child childName satteliteList =
        match tree with
        | Leaf name -> 
            if name = nodeName then
                Some (Orbits(name, Map.empty.Add(childName, child)))
            else
                None
        | Orbits (name, sattelites) ->
            if name = nodeName then
                Some (Orbits(name, Map.add childName child sattelites))
            else
                match satteliteList with
                | c :: cs -> 
                    match addChild' (snd c) nodeName child childName [] with
                    | Some node -> Some (Orbits (name, Map.add (fst c) (node) sattelites))
                    | None -> addChild' tree nodeName child childName cs
                | [] ->
                    addChild' tree nodeName child childName (Map.toList sattelites)
    
    match child with
    | Leaf name -> 
        addChild' tree nodeName child name []
    | Orbits (name, _) -> 
        addChild' tree nodeName child name []

(*
let assembleTree (input :OrbitTree seq) =
    let rec assembleTree' tree list map =
        match tree with
        | Leaf _ ->
            failwith "tree cannot be a leaf node"
        | Orbits (name, sattelites) ->
            match (snd list) with
            | x :: xs ->
                match x with
                | Leaf xName ->
                    failwith "tree cannot be a leaf node"
                | Orbits (xName, xSattelites) ->
                    match addChild tree x xName with
                    | Some newTree -> assembleTree' newTree xs (Map.remove xName map)
                    | None -> assembleTree' tree xs map
            | [] ->
                if map.Count = 1 then
                    tree
                else
                    assembleTree' tree (Map.toList map) map
                
    let treeMap = 
        input
        |> Seq.map (fun i -> 
            match i with
            | Leaf name -> (name, i)
            | Orbits (name, sattelites) -> (name, i))
        |> Map.ofSeq

    assembleTree' (Seq.head input) (Map.toList treeMap) treeMap
   
*)

let main input =
    let part1 =
        let directOrbits =
            Seq.length input



        ""

    let part2 = 
        ""

    part1, part2