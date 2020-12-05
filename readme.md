# AdventOfCode

This is my repo for AdventOfCode solutions. 2019 is in F#. I am  new to the language so expect weird patterns and dirty hacks. 2020 is in C#, a language I am very familiar withm I have no excuses :P.

## How to build

Open solution file in Visual Studio and build

## How to Run

Add a local secret with your session token to fetch puzzle input.

```
dotnet user-secrets set "Auth:SessionToken" <your session token here>
```

Then just do
```
donet run <year> <day> <optional: force download of input>
```

Example: `dotnet run 2019 4`

## Days Completed

### 2020

- Day 1: Report Repair
- Day 2:  Password Philosophy
- Day 3: Toboggan Trajectory
- Day 4: Passport Processing
- Day 5: Binary Boarding

### 2019

- Day 1: The Tyranny of the Rocket Equation
- Day 2: 1202 Program Alarm
- Day 3: Crossed Wires
- Day 4: Secure Container
- Day 5: Sunny with a Chance of Asteroids
- Day 6: Universal Orbit Map
- Day 7: Amplification Circuit
- Day 8: Space Image Format
- Day 9: Sensor Boost
- Day 10: Monitoring Station
- Day 11: Space Police
- Day 12: The N-Body Problem
- Day 13: Care Package
- Day 14: Space Stoichiometry
- Day 15: Oxygen System
- Day 16: Flawed Frequency Transmission
- Day 17: Set and Forget
- Day 19: Tractor Beam
- Day 21: Springdroid Adventure
- Day 23: Category Six

### 2018

- Day 1: Chronal Calibration
- Day 2: Inventory Management System

## Notes

### 2019 Day 16

For part 2 I had to cheat a bit and read on the subreddit to get some hints. After looking at a few solutions and re-reading the problem description, I first realized that the last digit of the input always is the same. That made me see the pattern that the value of any digit only depends on the digits from previous iteration that where the index is equal or higher. So any digits lower than the offset can safely be ignored. The final piece was realizing that the offset given in the input alway would be so large that the pattern would always be 1. That made calculating the sums even easier, as under that constraint each digit is now only dependent on the digits value in the previous iteration and the next digit in the same iteration! As the last digit is always the same, calculating each iteration is as simple as summing the digits, starting from the end of the list.

### 2019 Day 17
For part 2 I didn't do any coding, to get my program. I simply looked at the printed output, and translated the path to instructions e.g. R6 or L4. Then I used a text editor to make line breaks where the pattern repeated. This made it easy to move the splits one instruction forwards or backwards and I was able to discover the prorgam in two attemps.

This I the breakdown I ended up with:
```
A: R12,L8,L4,L4
B: L8,R6,L6
C: L8,L4,R12,L6,L4


R12,L8,L4,L4,
L8,R6,L6
R12,L8,L4,L4,
L8,R6,L6
L8,L4,R12,L6,L4
R12,L8,L4,L4,
L8,L4,R12,L6,L4
R12,L8,L4,L4,
L8,L4,R12,L6,L4,
L8,R6,L6
```

### 2019 Day 21
For both parts I simply deduced how create a logic expression/ program that would work, by hand.
For part one the strategy was to jump if the landing field has ground (D) and A B or C has a hole.
For part two the strategy is same as for part one, with two addiditions. Go through with a jump if there is ground after the landing site (E) or there is ground at the next landing site (H)

Annotated Part one program:
```
OR A J      # J = A is Ground
AND B J     # J = A and B is Ground
AND C J     # J = A and B and C is Ground
NOT J J     # J = A or B or C has Hole
AND D J     # J = (A or B or C has hole) and D is ground
```

Note that in the code, the part one program is different. That is an earlier version of this program. I kept it because it worked and was good enough for part 1, but needed to be simpler to do part 2

Annoteted Part Two program:
```
OR A J      # J = A is Ground
AND B J     # J = A and B is Ground
AND C J     # J = A and B and C is Ground
NOT J J     # J = A or B or C has Hole
AND D J     # J = (A or B or C has hole) and D is ground
OR H T      # T = H is Ground
OR E T      # T = H is Ground or E is Ground
AND T J     # J =  ((A or B or C has hole) and D is ground) AND (H is Ground or E is Ground)
```

### Year 2019 Day 23

I made the assumption that keeping track of timings didn't matter. Otherwise the Intcode computer had to be changed, which would cause many previous challenges to have to be updated. Instead I run every computer until it requests input. Then I gather the all output packages and feed them back into the computers for the next step. In case multiple computers would write to 255 during one step I would be in trouble. I would have no way of determining wich computer came first, but fortunately that did not happen.

### Year 2020 Day 4

I made too many assumptions on the validations in part one. If I were to redo it, I would not do any validation on part one, maybe even just use a simple dictionary for the passport propreties. For part 2 the validation is specified, which makes it simpler to add. Here I had to update the existing logic I had. In short, I am not proud of this solution, but this I just for fun so I can't be bothered to improve it.