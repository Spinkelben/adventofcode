# AdventOfCode

This is my repo for AdventOfCode solutions in F#. I am  now to the language so expect weird patterns and dirty hacks. 

## How to build

Open solution file in Visual Studio and build

## How to Run

Add a local secret with your session token to fetch puzzle input.

```
dotnet user-secrets set "Auth:SessionToken" <your session token here>
```

Then just do
```
donet run <year> <day>
```