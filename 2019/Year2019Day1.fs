module Year2019Day1

    let main (input :string seq) =
        let calculateFuel mass = (mass / 3) - 2

        let part1 = 
            input 
            |> Seq.map int 
            |> Seq.map calculateFuel
            |> Seq.sum

        let part2 = 
            let rec part2' total newFuel =
                if newFuel > 0 then
                    part2' (total + newFuel) (calculateFuel newFuel)
                else
                    total

            input 
            |> Seq.map int 
            |> Seq.map (fun x -> part2' 0 (calculateFuel x))
            |> Seq.sum

        part1.ToString(), part2.ToString()