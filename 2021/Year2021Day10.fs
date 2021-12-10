namespace Year2021

open System

module Day10 =
    type LinePoints =
        | Incomplete of int64
        | Corrupted of int

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

        let getIncompletePoints openChunks =
            let points = Map.ofList [ (')', 1L); (']', 2L); ('}',3L); ('>', 4L); ]
            let folder total c = let autocomplete = closingTag c 
                                 Map.find autocomplete points + total * 5L
            List.fold folder 0L openChunks

        let rec getPointForLine openChunks line =
            match line, openChunks with
            | [], _ -> getIncompletePoints openChunks |> Incomplete // incomplete ignore
            | c::cs, o::os when closingTag o = c -> getPointForLine os cs  // Matching, chunk closed
            | c::cs, os when isOpeningTag c -> getPointForLine (c :: os) cs // Chunk open
            | c::_, _ -> pointsForCharacter c |> Corrupted // Mismatch close tag, score the points!

        let part1 = input 
                    |> Seq.map (fun line -> getPointForLine [] (List.ofSeq line))
                    |> Seq.choose (fun x -> match x with 
                                            | Corrupted x  -> Some x
                                            | Incomplete _ -> None)
                    |> Seq.sum
            

        let points = input 
                    |> Seq.map (fun line -> getPointForLine [] (List.ofSeq line))
                    |> Seq.choose (fun x -> match x with 
                                            | Corrupted _  -> None
                                            | Incomplete x -> Some x)
                    |> Seq.sort

        let part2 = Seq.item (Seq.length points / 2) points
                    
            
        string part1, string part2