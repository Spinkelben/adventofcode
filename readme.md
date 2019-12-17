# AdventOfCode

This is my repo for AdventOfCode solutions in F#. I am  new to the language so expect weird patterns and dirty hacks.

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
- Day 11: Space Police
- Day 13: Care Package
- Day 14: Space Stoichiometry
- Day 15: Oxygen System
- Day 17: Set and Forget

### 2018

- Day 1: Chronal Calibration
- Day 2: Inventory Management System

## Notes

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
