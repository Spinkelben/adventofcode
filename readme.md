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

### 2018

- Day 1: Chronal Calibration
- Day 2: Inventory Management System

