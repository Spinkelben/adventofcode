module Year2019Day6

type OrbitTree =
    | Leaf of name : string
    | Orbits of name : string * satellites: list<OrbitTree>


let parsePairAsOrbitTree (pairLine : string) =
    let bodies = pairLine.Split(')')
    Orbits(name = bodies.[0], satellites = [ Leaf(  bodies.[1] ) ])

let addChild tree child =
    let rec addChild' tree nodeName child = 
        match tree with
        | Leaf name -> 
            if name = nodeName then
                Orbits(name, child), true
            else
                tree, false
        | Orbits (name, satellites) ->
            if name = nodeName then
                Orbits(name, List.append child satellites), true
            else
                let recResults =
                    List.map (fun t ->
                        addChild' t nodeName child)
                        satellites

                Orbits(name, List.map fst recResults), 
                List.fold (fun acc (_, result) -> acc || result) false recResults
    match child with
    | Leaf name -> tree, false
    | Orbits (name, sattelites) -> addChild' tree name sattelites


let assembleTree (input :OrbitTree seq) rootNodeName =
    let rec assembleTree' tree (looseNodes :list<OrbitTree>) =
        let reaminingNodes, resultingTree = 
            looseNodes
            |> Seq.fold 
                (fun (leftOver, accTree) node -> 
                    let (newTree, isDifferent) = addChild accTree node
                    if isDifferent then
                        leftOver, newTree
                    else
                        (node :: leftOver), newTree)
                (list.Empty, tree)

        match reaminingNodes with
        | _ :: _ -> assembleTree' resultingTree reaminingNodes
        | [] -> [], resultingTree

    let root = 
        Seq.find (fun x -> 
            match x with
            | Leaf name -> name = rootNodeName
            | Orbits (name, _) -> name = rootNodeName)
            input

    let _, tree = assembleTree' root (Seq.filter (fun x -> x <> root) input |> List.ofSeq)
    tree
                   
let countIndirectOrbits tree =
    let rec countIndirectOrbits' tree depth =
        match tree with
        | Leaf _ -> max (depth - 1) 0
        | Orbits (_, children) -> 
            let childCount = 
                children 
                |> List.fold (fun acc c -> acc + (countIndirectOrbits' c (depth + 1))) 0
            
            childCount + max (depth - 1) 0

    countIndirectOrbits' tree 0

let findNearestAncestor tree node1Name node2Name =
    let rec findNearestAncestor' tree depth = 
        match tree with
        | Leaf name ->
            name = node1Name, name = node2Name, depth
        | Orbits (name, satellite) ->
            let results = 
                List.map  
                    (fun t -> findNearestAncestor' t (depth + 1))
                    satellite
                |> List.filter (fun r -> 
                    match r with
                    | (false, false, _) -> false
                    | _ -> true)
            if results.Length = 1 then
                results.Head
            else if results.Length = 2 then
                true, true, depth
            else
                false, false, depth
        
    findNearestAncestor' tree 0

let rec getNodeDepth tree nodeName depth =
    match tree with
    | Leaf name -> 
        if name = nodeName then
            Some depth
        else 
            None
    | Orbits (name, satellites) ->
        if name = nodeName then
            Some depth
        else
            List.fold (fun acc v -> match getNodeDepth v nodeName (depth + 1) with
                                    | Some r -> Some r
                                    | None -> acc)
                None
                satellites

let main input =
    let tree = 
        assembleTree (Seq.map parsePairAsOrbitTree input) "COM"

    let part1 =
        let directOrbits =
            Seq.length input
        let indirectOrbits = countIndirectOrbits tree
        directOrbits + indirectOrbits

    let part2 = 
        let (_, _, commonAncestorDepth) =
            findNearestAncestor tree "YOU" "SAN"
        let meDepth =
            match getNodeDepth tree "YOU" 0 with
            | Some v -> v
            | None -> 0

        let santaDepth =
            match getNodeDepth tree "SAN" 0 with
            | Some v -> v
            | None -> 00

        (meDepth - commonAncestorDepth) + (santaDepth - commonAncestorDepth) - 2

    part1.ToString(), part2.ToString()