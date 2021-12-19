namespace Year2021

open System.Text.RegularExpressions

module Day17 =


    let main (input :string seq) =
            
        let getTarget s = 
            let regexMatch = Regex.Match(s, "target area: x=(-?\d+)..(-?\d+), y=(-?\d+)..(-?\d+)")
            (int regexMatch.Groups.[1].Value, 
                int regexMatch.Groups.[2].Value),
            (int regexMatch.Groups.[3].Value, 
                int regexMatch.Groups.[4].Value)

        let isInTarget (x1, x2) (y1, y2) (x,y) =
            x >= x1 && x <= x2 && y >= y1 && y <= y2

        let isAbleToHit (x1, x2) (y1,y2) (x, y) (dx, dy) =
            let isAbleToHitX (t1,t2) p v =
                (v > 0 && p <= t2)
                || (v < 0 && p >= t1)
                || (v = 0 && p >= t1 && p <= t2)

            let isAbleToHitY t2 p =
                p >= t2

            isAbleToHitX (x1, x2) x dx && isAbleToHitY y2 y

        let finalX dx = 
            (dx * (dx + 1)) / 2

        let xVelocityCandidates (x1, x2) =
            // All x velocities that at least reach the target range
            // and where first step is still within target range
            [x2.. -1 .. 0] |> List.where (fun dx ->  let destX = finalX dx
                                                     destX >= x1)

        let yVelocityCandidates (y1, y2) =
            // Velocities greater than 1
            // Velocities less tahen y1, as they will shoot straight past
            // the target going down
            [0 .. abs y1]
            

        let getCandidateVelocities xRange yRange =
            [for dx in xVelocityCandidates xRange do 
                for dy in yVelocityCandidates yRange do yield (dx, dy)]

        let calculateTrajectory ((tx1, tx2),(ty1, ty2)) (x0, y0) (dx0, dy0)  =
            let isInTarget =
                isInTarget (tx1, tx2) (ty1, ty2)
            let isAbleToHit = 
                isAbleToHit (tx1, tx2) (ty1, ty2)
            let rec calculateTrajectory' (x, y) (dx, dy) trajectory =
                let p' = x + dx, y + dy
                let dx' = if dx > 0 then 
                            dx - 1
                          elif dx < 0 then
                            dx + 1
                          else 
                            0
                let dy' = dy - 1
                let v' = (dx', dy')
                if isInTarget p' then
                    Some (p' :: trajectory)
                elif isAbleToHit p' v' then 
                    calculateTrajectory' p' v' (p' :: trajectory)
                else 
                    None

            calculateTrajectory' (x0, y0) (dx0, dy0) ((x0, y0) :: [])

        let (x1,x2),(y1,y2) = getTarget (Seq.head input)
        let candidates = getCandidateVelocities (x1, x2) (y1, y2)
        let getTrajectory = calculateTrajectory ((x1, x2),(y1, y2)) (0,0)
        let part1 = candidates 
                    |> List.choose (fun c -> getTrajectory c) // All velocities that hit target
                    |> List.maxBy (fun t -> t |> List.maxBy snd |> snd) // Find trajectory with highest point
                    |> List.maxBy snd // Find highest point in trajectory
                    |> snd // Take y value


        let part2 = ""
                    
        string part1, string part2