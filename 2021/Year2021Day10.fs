namespace Year2021

open System

module Day10 =
    let main (input :string seq) =
        let pointsForCharacter c =
            match c with
            | ')' -> 3
            | ']' -> 57
            | '}' -> 1197
            | '>' -> 25137
            | _   -> 0

        let closingTag c =
            match c with
            | '(' -> ')'
            | '[' -> ']'
            | '{' -> '}'
            | '<' -> '>'
            | _   -> c

        let isOpeningTag c =
            List.contains c ['(';'[';'{';'<']

        let rec getPointForLine openChunks line =
            match line, openChunks with
            | [], _ -> 0 // incomplete ignore
            | c::cs, o::os when closingTag o = c -> getPointForLine os cs  // Matching, chunk closed
            | c::cs, os when isOpeningTag c -> getPointForLine (c :: os) cs // Chunk open
            | c::cs, _ -> pointsForCharacter c // Mismatch close tag, score the points!

        let part1 = input 
                    |> Seq.map (fun line -> getPointForLine [] (List.ofSeq line))
                    |> Seq.sum
            
        let part2 = ""
            
        string part1, string part2