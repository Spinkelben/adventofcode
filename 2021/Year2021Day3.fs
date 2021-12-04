namespace Year2021

open System

module Day3 =
   let main (input :string seq) =
        let stringToInts (s :string) =
            Seq.map (fun c -> Int32.Parse(string c)) s
            |> Seq.toArray

        let arrayOfArrayTo2D arrayOfArray =
            let xSize = Array.length arrayOfArray
            let ySize = Array.length arrayOfArray.[0]
            Array2D.init xSize ySize (fun x y -> arrayOfArray.[x].[y])

        let bitGrid = 
            let listOfList = input 
                            |> Seq.map stringToInts
                            |> Seq.toArray
            arrayOfArrayTo2D listOfList
                      

        let getColumns a =
            seq { for i in 0 .. Array2D.length2 a - 1 -> a.[*, i] }

        let rate selector column =
            let zeroCount = Array.where (fun e -> e = 0) column
            let oneCount = Array.where (fun e -> e = 1) column
            (zeroCount, oneCount)
            |> selector

        let gammaColumnAggregator column = rate (fun (zero, one) -> if one >= zero then 1 else 0) column

        let epsilonColumnAggregator column = rate (fun (zero, one) -> if one >= zero then 0 else 1) column

        let gridAggregator columnAggregator grid =
            getColumns grid
            |> Seq.map columnAggregator
            

        let digitsToDecimal digits =
            let string = digits 
                         |> Seq.map string 
                         |> String.concat ""
            Convert.ToInt32(string, 2)

        let gamma =  bitGrid
                    |> gridAggregator gammaColumnAggregator
                    |> digitsToDecimal

        let epsilon = bitGrid
                      |> gridAggregator epsilonColumnAggregator
                      |> digitsToDecimal

        let part1 = gamma * epsilon
            
        let oxygenRater idx grid = 
            gridAggregator gammaColumnAggregator grid 
            |> Seq.item idx

        let co2ScrubberRater idx grid = 
            gridAggregator epsilonColumnAggregator grid 
            |> Seq.item idx

        let lifeSupportDiagnostic rater grid =
            let rec oxygenFilter' idx grid =
                if Array2D.length1 grid = 1 then 
                    Some grid.[0,*]
                else if idx >= Array2D.length2 grid then
                    None
                else 
                    let bitCriteria = rater idx grid
                    let survivingNumbers =  arrayOfArrayTo2D [|
                        for i in 0 .. Array2D.length1 grid - 1 do 
                            if grid.[i, idx] = bitCriteria then 
                                yield grid.[i, *]
                    |]
                    oxygenFilter' (idx + 1) survivingNumbers

            oxygenFilter' 0 grid
           
        let oxygenRating = lifeSupportDiagnostic oxygenRater bitGrid |> Option.map digitsToDecimal |> Option.get
        let co2ScrubberRating = lifeSupportDiagnostic co2ScrubberRater bitGrid |> Option.map digitsToDecimal |> Option.get

        let part2 = oxygenRating * co2ScrubberRating

        string part1, string part2