namespace Year2021

open System
open ParseUtil
    

module Day18 =
    type SNumber =    
    | RNumber of int
    | SPair of SNumber * SNumber

    let numberParser = 
        let parser = ParseUtil.pint

        parser |>> RNumber

    let sPair, sPairRef = createParserForwardedToRef<SNumber>()

    let pairParser =
        let left = pchar '['
        let right = pchar ']'
        let sep = pchar ','
        let pair = numberParser <|> sPair

        left >>. pair .>> sep .>>. pair .>> right <?> "pair" |>> SPair

    sPairRef.Value <- pairParser

    let addNumbers n1 n2 = 
        SPair (n1, n2)

    let 


    let main (input :string seq) =
            


        let part1 = ""

        let part2 = ""
                    
        string part1, string part2