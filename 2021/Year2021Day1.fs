namespace Year2021

module Day1 =

    let main (input :string seq) =
        let isIncreased (a, b) = a < b

        let part1 = input
                    |> Seq.map int
                    |> Seq.pairwise
                    |> Seq.where isIncreased
                    |> Seq.length
                

        let part2 = input 
                    |> Seq.map int
                    |> Seq.windowed 3
                    |> Seq.map Array.sum
                    |> Seq.pairwise
                    |> Seq.where isIncreased
                    |> Seq.length

        string part1, string part2