using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 12)]
    internal class Year2020Day12 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var ship = new Ship();
            foreach (var instruction in input.Where(i => i.Length > 0))
            {
                ship.Part1(instruction);
            }

            var (x, y) = ship.Position;
            return $"{Math.Abs(x) + Math.Abs(y)}";
        }

        public string Part2(IList<string> input)
        {
            var ship = new Ship();
            foreach (var instruction in input.Where(i => i.Length > 0))
            {
                ship.Part2(instruction);
            }

            var (x, y) = ship.Position;
            return $"{Math.Abs(x) + Math.Abs(y)}";
        }

        internal class Ship
        {
            Direction Direction { get; set; } = Direction.East;

            internal (int x, int y) Position { get; set; }
            (int x, int y) Waypoint { get; set; } = (10, -1);

            internal void Part1(string instructionString)
            {
                var (instruction, steps) = ParseInstruction(instructionString);

                switch (instruction)
                {
                    case Instruction.North:
                        MoveShip(Direction.North, steps);
                        break;
                    case Instruction.East:
                        MoveShip(Direction.East, steps);
                        break;
                    case Instruction.South:
                        MoveShip(Direction.South, steps);
                        break;
                    case Instruction.West:
                        MoveShip(Direction.West, steps);
                        break;
                    case Instruction.Left:
                        TurnShip(Instruction.Left, steps);
                        break;
                    case Instruction.Right:
                        TurnShip(Instruction.Right, steps);
                        break;
                    case Instruction.Forward:
                        MoveShip(Direction, steps);
                        break;
                    default:
                        break;
                }
            }

            internal void Part2(string instructionString)
            {
                

                var (instruction, steps) = ParseInstruction(instructionString);

                switch (instruction)
                {
                    case Instruction.North:
                        MoveWaypoint(Direction.North, steps);
                        break;
                    case Instruction.East:
                        MoveWaypoint(Direction.East, steps);
                        break;
                    case Instruction.South:
                        MoveWaypoint(Direction.South, steps);
                        break;
                    case Instruction.West:
                        MoveWaypoint(Direction.West, steps);
                        break;
                    case Instruction.Left:
                        RotateWaypoint(Instruction.Left, steps);
                        break;
                    case Instruction.Right:
                        RotateWaypoint(Instruction.Right, steps);
                        break;
                    case Instruction.Forward:
                        var (x, y) = Position;
                        var (dx, dy) = Waypoint;
                        Position = (x + dx * steps, y + dy * steps);
                        break;
                    default:
                        break;
                }
            }

            private (Instruction instruction, int steps) ParseInstruction(string instruction)
            {
                return instruction[0] switch
                {
                    'N' => (Instruction.North, int.Parse(instruction[1..])),
                    'S' => (Instruction.South, int.Parse(instruction[1..])),
                    'E' => (Instruction.East, int.Parse(instruction[1..])),
                    'W' => (Instruction.West, int.Parse(instruction[1..])),
                    'L' => (Instruction.Left, int.Parse(instruction[1..])),
                    'R' => (Instruction.Right, int.Parse(instruction[1..])),
                    'F' => (Instruction.Forward, int.Parse(instruction[1..])),
                    char wrong => throw new ArgumentOutOfRangeException(
                        nameof(instruction),
                        $"Unknown instruction {wrong}")
                };
            }

            private void MoveWaypoint(Direction direction, int steps)
            {
                Waypoint = MoveCoordinate(Waypoint, direction, steps);
            }

            private void MoveShip(Direction direction, int steps)
            {
                Position = MoveCoordinate(Position, direction, steps);
            }

            private void RotateWaypoint(Instruction direction, int degrees)
            {
                Waypoint = direction switch
                {
                    Instruction.Left => RotateCoordiante(Waypoint, -degrees),
                    Instruction.Right => RotateCoordiante(Waypoint, degrees),
                    Instruction unsupported => throw new ArgumentOutOfRangeException(
                        nameof(direction),
                        $"Only left and right is supported. You providede {unsupported}"),
                };
            }

            private (int x, int y) RotateCoordiante((int x, int y) wayPoint, int degrees)
            {
                var sinMap = new Dictionary<int, int>()
                {
                    [-270] =  1,
                    [-180] =  0,
                     [-90] = -1,
                       [0] =  0,
                      [90] =  1,
                     [180] =  0,
                     [270] = -1,
                };

                var cosMap = new Dictionary<int, int>()
                {
                    [-270] =  0,
                    [-180] = -1,
                     [-90] =  0,
                       [0] =  1,
                      [90] =  0,
                     [180] = -1,
                     [270] =  0,
                };
                var (x, y) = wayPoint;
                var bounded = degrees % 360;

                return (cosMap[bounded] * x - sinMap[bounded] * y,
                    sinMap[bounded] * x + cosMap[bounded] * y);
            }

            private (int x, int y) MoveCoordinate((int x, int y) coordinate, Direction direction, int steps)
            {
                return direction switch
                {
                    Direction.North =>
                        (coordinate.x, coordinate.y - steps),
                    Direction.East =>
                        (coordinate.x + steps, coordinate.y),
                    Direction.South =>
                        (coordinate.x, coordinate.y + steps),
                    Direction.West =>
                        (coordinate.x - steps, coordinate.y),
                    _ => throw new NotImplementedException(),
                };
            }

            private void TurnShip(Instruction direction, int degrees)
            {
                switch (direction)
                {
                    case Instruction.Left:
                        Direction -= (degrees / 90);
                        break;
                    case Instruction.Right:
                        Direction += (degrees / 90);
                        break;
                    default:
                        break;
                }

                var numDirections = Enum.GetValues(typeof(Direction)).Length;
                if (Direction < 0)
                {
                    Direction += numDirections;
                }

                Direction = (Direction)((int)Direction % numDirections);
            }
        }

        enum Direction
        {
            North = 0,
            East = 1, 
            South = 2,
            West = 3,
        }

        enum Instruction
        {
            North,
            East,
            South,
            West,
            Left,
            Right,
            Forward,
        }
    }

    
}
