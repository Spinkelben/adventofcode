namespace Year2021

open System

module Day8 =
    let splitString (seperator : string) (s : string) =
        s.Split(seperator, StringSplitOptions.RemoveEmptyEntries)

    let overlap s1 s2 =
        let set1 = Set.ofSeq s1
        let set2 = Set.ofSeq s2
        Set.intersect set1 set2
        |> Set.count

    let deduceSegementConfiguration combinations =
        let getStringsWithLength length s = 
            Array.where (fun s -> Set.count s = length) s
        let getStringWithOverlap targetOverlap test strings =
            Array.partition (fun s -> overlap test s = targetOverlap) strings
        let one = getStringsWithLength 2 combinations |> Array.head
        let four = getStringsWithLength 4 combinations |> Array.head
        let seven = getStringsWithLength 3 combinations |> Array.head
        let eight = getStringsWithLength 7 combinations |> Array.head
        let zeroSixNine = getStringsWithLength 6 combinations
        let twoThreeFive = getStringsWithLength 5 combinations
        let nine, zeroSix = getStringWithOverlap 4 four zeroSixNine
        let six, zero = getStringWithOverlap 1 one zeroSix
        let five, twoThree = getStringWithOverlap 5 (Array.head six) twoThreeFive
        let three, two = getStringWithOverlap 3 seven twoThree
        Map.ofList [ one, "1"; 
                     Array.head two, "2"; 
                     Array.head three, "3"; 
                     four, "4";
                     Array.head five , "5";
                     Array.head six, "6";
                     seven, "7";
                     eight, "8";
                     Array.head nine, "9";
                     Array.head zero, "0";]

    let main (input :string seq) =
        let readDigits line =
            let split = splitString "|" line 
                        |> Array.map (fun e -> splitString " " e
                                                |> Array.map Set.ofSeq)
            if Array.length split = 2 then
                Some (Array.item 0 split, Array.item 1 split)
            else 
                None

        let mapDigit digit = 
            match Set.count digit with
            | 2 -> Some 1
            | 3 -> Some 7
            | 4 -> Some 4
            | 7 -> Some 8
            | _ -> None
            

        let part1 = input
                    |> Seq.choose readDigits
                    |> Seq.map snd // Only read output values
                    |> Seq.map (Seq.choose mapDigit) // Map string to know 7 segment
                    |> Seq.concat // Flatten list
                    |> Seq.length // Get count of numbers


        

        let mapValues (map :Map<Set<char>, string>) values =
            String.Join("",
                Array.map (fun v -> Map.find v map) values)
            

        let part2 = input
                    |> Seq.choose readDigits
                    |> Seq.map (fun (c, v) -> deduceSegementConfiguration c, v)
                    |> Seq.map (fun (c, v) -> mapValues c v)
                    |> Seq.map int
                    |> Seq.sum

        string part1, string part2