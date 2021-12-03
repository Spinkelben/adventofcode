namespace Year2021

open System

module Day3 =
   type Movement = int * int

    let main (input :string seq) =
        let stringToInts (s :string) =
            Seq.map (fun c -> Int32.Parse(string c)) s
            |> Seq.toArray
            

        let bitGrid = 
            let listOfList = input 
                            |> Seq.map stringToInts
                            |> Seq.toArray
            Array2D.init listOfList.Length listOfList.[0].Length (fun x y -> listOfList.[x].[y])
                      

        let getColumns a =
            seq { for i in 0 .. Array2D.length2 a - 1 -> a.[*, i] }

        let rate selector column =
            Array.groupBy id column
            |> selector

        let gammaColumnAggregator = rate (Array.maxBy (fun (_, elements) -> elements.Length))

        let epsilonColumnAggregator = rate (Array.minBy (fun (_, elements) -> elements.Length))

        let gridAggregator columnAggregator grid =
            getColumns grid
            |> Seq.map columnAggregator
            |> Seq.map (fun (digit, _) -> string digit)

        let digitsToDecimal digits =
            let string = String.concat "" digits
            Convert.ToInt32(string, 2)

        let gamma =  bitGrid
                    |> gridAggregator gammaColumnAggregator
                    |> digitsToDecimal

        let epsilon = bitGrid
                      |> gridAggregator epsilonColumnAggregator
                      |> digitsToDecimal

        let part1 = gamma * epsilon
            

        let part2 = ""

        string part1, string part2