namespace Utils

module ArrayHelpers =

    let sparseToDense tiles defaultValue =
        let tileList = Map.toList tiles
        let maxX = 
            List.map (fun ((x, _), _) -> x) tileList
            |> List.max
        let minX = 
            List.map (fun ((x, _), _) -> x) tileList
            |> List.min
        let maxY = 
            List.map (fun ((_, y), _) -> y) tileList
            |> List.max
        let minY = 
            List.map (fun ((_, y), _) -> y) tileList
            |> List.min

        let xOffset = if minX < 0 then -minX else 0
        let yOffset = if minY < 0 then -minY else 0
        let ySize = (1 + maxY - minY)
        let xSize = (1 + maxX - minX)
        let picture = Array2D.create ySize xSize defaultValue
        List.iter (fun ((x, y), color) -> 
                Array2D.set picture (y + yOffset) (x + xOffset) color) 
            tileList
        picture

    let printTileMap tiles tileMap =
        [for y in 0 .. Array2D.length1 tiles - 1 do 
            Array.map tileMap tiles.[y,*] 
            |> Array.fold (fun state char -> state + char) ""]
        |> List.fold (fun acc line -> acc + line + "\r\n") "\r\n"