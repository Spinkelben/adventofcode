namespace Year2021

open System
open System.Text.RegularExpressions;

module Day13 =
    type Fold = 
    | X of int
    | Y of int

    let main (input :string seq) =
        let parseInput input =
            let isNotSectionDevider s = String.IsNullOrWhiteSpace(s) |> not
            let dots = input |> Seq.takeWhile isNotSectionDevider
                       |> Seq.map (fun s -> let split = Regex.Match(s, "(\d+),(\d+)")
                                            (int split.Groups.[1].Value, int split.Groups.[2].Value))
                       |> Set.ofSeq
            let folds = input |> Seq.skipWhile isNotSectionDevider 
                        |> Seq.tail
                        |> Seq.map (fun l -> Regex.Match(l, "fold along (\w+)=(\d+)"))
                        |> Seq.map (fun x -> (x.Groups.[1].Value, int x.Groups.[2].Value))
                        |> Seq.choose (fun (x, v) -> match x with 
                                                     | "x" -> Fold.X v |> Some
                                                     | "y" -> Fold.Y v |> Some
                                                     | _ -> None)
                        |> List.ofSeq

            (dots, folds)


        let foldx idx (x, y) = (idx - (x - idx), y)
        let foldy idx (x, y) = (x, idx - (y - idx))
            
        let foldTransparent fold dots =
            let folder, isUnmoved = match fold with
                                    | Fold.X i -> (foldx i, fun (x, _) -> x < i)
                                    | Fold.Y i -> (foldy i, fun (_, y) -> y < i)
            let unmovedDots = dots |> Set.filter isUnmoved
            let foldedDots = dots |> Set.filter  (fun dot -> dot |> isUnmoved |> not)
                                  |> Set.map folder
            unmovedDots |> Set.union foldedDots

        let (dots, folds) = parseInput input
        let part1 = foldTransparent (List.head folds) dots
                    |> Set.count

        let part2 = ""
        
        string part1, string part2