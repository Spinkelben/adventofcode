module Year2019Day14
open System

type MaterialRequirement =
    { name: string; amount : int64 }

let main input =
    let reactionMap = 
        Seq.map (fun (l : string) -> l.Split("=>")) input
        |> Seq.map (fun (l : string array) -> 
            let materials = 
                l.[0].Split(",", StringSplitOptions.RemoveEmptyEntries)
                |> Array.map(fun mat -> 
                    let splitmat = mat.Trim().Split(" ")
                    { amount = int64 (splitmat.[0]); name = splitmat.[1] } )
                |> List.ofArray

            let splitProduct = l.[1].Trim().Split(" ")
            let product = { amount = int64 (splitProduct.[0]); name = splitProduct.[1] } 
            product.name, (materials, product))
        |> Map.ofSeq

    let rec calculateOreRequirement products (leftovers : Map<string,MaterialRequirement>) oreSpent =
        let productMinusLeftovers reqProduct =
            if leftovers.ContainsKey (reqProduct.name) then
                let t = 
                    leftovers.Item (reqProduct.name)
                { reqProduct with amount = reqProduct.amount - t.amount},
                leftovers.Remove (t.name)
            else
                reqProduct, leftovers
        match products with
        | [] -> 
            oreSpent, leftovers
        | x :: xs ->
            let reaction = reactionMap.Item (x.name)
            let reqProduct, newLeftovers = productMinusLeftovers x
            let numReactionsNeeded = int64 (ceil (float reqProduct.amount / float (snd reaction).amount))
            let leftoverMaterial = { name = reqProduct.name; 
                amount = ((snd reaction).amount * numReactionsNeeded) - reqProduct.amount}
            let newLeftOvers' = 
                if Map.containsKey x.name newLeftovers then
                    let t = newLeftovers.Item reqProduct.name
                    newLeftovers.Add (reqProduct.name,{t with amount = t.amount + leftoverMaterial.amount})
                else if leftoverMaterial.amount > 0L then
                    newLeftovers.Add (leftoverMaterial.name, leftoverMaterial)
                else 
                    newLeftovers

            let requiredProducts = List.map (fun r ->
                                                { r with amount = r.amount * numReactionsNeeded })
                                        (fst reaction)
            let oreRequired = List.fold
                                (fun acc prod -> 
                                    if prod.name = "ORE" then
                                        acc + prod.amount
                                    else
                                        acc)
                                0L
                                requiredProducts
            let nonOreRequirements = requiredProducts |> List.filter (fun p -> p.name <> "ORE")

            calculateOreRequirement (List.append xs nonOreRequirements) newLeftOvers' (oreSpent + oreRequired)

    let rec createFuelUntilOreIsUsed remainingOre leftovers totalFuel batchSize =
        let (oreUsed, leftoverMaterial) = 
            calculateOreRequirement [{ name = "FUEL"; amount = batchSize}] leftovers 0L

        if oreUsed > remainingOre then
            if batchSize = 1L then
                totalFuel
            else 
                createFuelUntilOreIsUsed remainingOre leftoverMaterial totalFuel (batchSize / 2L)
        else 
            createFuelUntilOreIsUsed (remainingOre - oreUsed) leftoverMaterial (totalFuel + batchSize) batchSize

    let part1 =
        let oreUsed, _ = calculateOreRequirement [{ name = "FUEL"; amount = 1L}] Map.empty 0L
        oreUsed

    let part2 =
        createFuelUntilOreIsUsed 1000000000000L Map.empty 0L 1000000000000L

    part1.ToString(), part2.ToString()