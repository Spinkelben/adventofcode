use std::{collections::HashSet, iter};

use super::Solution;

pub struct NoTimeForTaxicab<'a> {
    input: &'a str

}

impl<'a> NoTimeForTaxicab<'a> {
    pub fn new(input: &'a str) -> NoTimeForTaxicab<'a> {
        NoTimeForTaxicab { input: input }
    }
}

impl Solution for NoTimeForTaxicab<'_> {
    fn solve_part1(&self) -> String {
        let path = parse_instructions(self.input);
        let result = calculate_distance_from_origin(path);
        result.to_string()
    }

    fn solve_part2(&self) -> String {
        let path = parse_instructions(self.input);
        let (x, y) = find_first_location_visited_twice(path).unwrap();

        (x.abs() + y.abs()).to_string()
    }
}

#[derive(Debug)]
#[derive(PartialEq)]
enum Move {
    Left(u32),
    Right(u32)
}

#[derive(Clone, Copy, Debug, PartialEq)]
enum CardinalDirection {
    North,
    East,
    South,
    West,
}

#[derive(Debug, PartialEq)]
struct Position {
    direction: CardinalDirection,
    x: i32,
    y: i32,
}

impl Position {
    fn origin() -> Position {
        Position { direction: CardinalDirection::North, x: 0, y: 0 }
    }
}

fn parse_instructions(instructions: &str) -> Vec<Move> {
    instructions
    .split(", ")
    .map(|i|
    {
        match i.chars().nth(0) {
            Some('R') => Move::Right(i[1..].trim().parse::<u32>().unwrap()),
            Some('L') => Move::Left(i[1..].trim().parse::<u32>().unwrap()),
            _ => panic!("Invalid instruction {i}"),
        }
    })
    .collect()
}

fn calculate_distance_from_origin(path: Vec<Move>) -> i32 {
    let destination = follow_path(path);

    destination.x.abs() + destination.y.abs()
}

fn follow_path(path: Vec<Move>) -> Position {
    let mut position = Position::origin();
    for step in path {
        position = apply_instruction(&position, step);
    }

    position
}



fn apply_instruction(position: &Position, r#move: Move) -> Position
{
    match (position.direction, r#move) {
        (CardinalDirection::North, Move::Left(n)) =>
            new_position_from_direction_and_magnitude(CardinalDirection::West, position, n),
        (CardinalDirection::North, Move::Right(n)) =>
            new_position_from_direction_and_magnitude(CardinalDirection::East, position, n),
        (CardinalDirection::East, Move::Right(n)) =>
            new_position_from_direction_and_magnitude(CardinalDirection::South, position, n),
        (CardinalDirection::East, Move::Left(n)) =>
            new_position_from_direction_and_magnitude(CardinalDirection::North, position, n),
        (CardinalDirection::South, Move::Right(n)) =>
            new_position_from_direction_and_magnitude(CardinalDirection::West, position, n),
        (CardinalDirection::South, Move::Left(n)) =>
            new_position_from_direction_and_magnitude(CardinalDirection::East, position, n),
        (CardinalDirection::West, Move::Right(n)) =>
            new_position_from_direction_and_magnitude(CardinalDirection::North, position, n),
        (CardinalDirection::West, Move::Left(n)) =>
            new_position_from_direction_and_magnitude(CardinalDirection::South, position, n),
    }
}

fn new_position_from_direction_and_magnitude(direction: CardinalDirection, position: &Position, magnitude: u32) -> Position {
    let mut new = Position { direction: direction, x: position.x, y: position.y };
    match direction {
        CardinalDirection::North => new.y += magnitude as i32,
        CardinalDirection::East => new.x += magnitude as i32,
        CardinalDirection::South => new.y -= magnitude as i32,
        CardinalDirection::West => new.x -= magnitude as i32,
    }

    new
}

fn find_first_location_visited_twice(path: Vec<Move>) -> Option<(i32, i32)> {
    let mut visited_positions : HashSet<(i32, i32)> = HashSet::new();
    let mut prev = Position::origin();
    for instruction in path {
        let next = apply_instruction(&prev, instruction);
        let interpolated_positions = interpolate(&prev, &next);
        for coord in interpolated_positions {
            if !visited_positions.insert(coord) {
                return Some(coord);
            }
        }
        prev = next;
    }

    None
}

fn interpolate(prev: &Position, next: &Position) -> impl Iterator<Item=(i32, i32)> + use<> {
    fn get_range(start :i32, end :i32) -> impl Iterator<Item = i32> {
        if start < end {
            Box::new(start .. end)
        }
        else {
            Box::new((end + 1 .. start + 1).rev()) as Box<dyn Iterator<Item = i32>>
        }
    }

    match next.direction {
        CardinalDirection::North | CardinalDirection::South =>
        {
            let xs = iter::repeat(next.x);
            let ys = get_range(prev.y, next.y);
            Box::new(xs.zip(ys))
        }
        CardinalDirection::East | CardinalDirection::West =>
        {
            let xs = get_range(prev.x, next.x);
            let ys = iter::repeat(next.y);
            Box::new(xs.zip(ys)) as Box<dyn Iterator<Item=(i32, i32)>>
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn can_parse_instructions() {
        // Arrange
        let instruction = "R2, L3\n";
        let expected : Vec<Move> = vec![ Move::Right(2), Move::Left(3) ];

        // Act
        let actual = parse_instructions(instruction);

        // Assert
        assert_eq!(expected, actual);
    }

    #[test]
    fn can_calculate_distance() {
        // Arrange
        let inputs = vec![
            (vec![ Move::Right(2), Move::Left(3), ], 5),
            (vec![ Move::Right(2), Move::Right(2), Move::Right(2) ], 2),
            (vec![ Move::Right(5), Move::Left(5), Move::Right(5), Move::Right(3) ], 12),

        ];


        // Act
        for (path, length) in inputs
        {
            let result = calculate_distance_from_origin(path);

            // Assert
            assert_eq!(length, result);
        }
    }

    #[test]
    fn turn_around_test() {
        let start = Position { direction: CardinalDirection::North, x:0, y:0 };
        let next = apply_instruction(&start, Move::Right(2));
        assert_eq!(next, Position { direction: CardinalDirection::East, x:2, y:0 });
        let next = apply_instruction(&next, Move::Right(2));
        assert_eq!(next, Position { direction: CardinalDirection::South, x:2, y:-2 });
        let next = apply_instruction(&next, Move::Right(2));
        assert_eq!(next, Position { direction: CardinalDirection::West, x:0, y:-2 });
        assert_eq!(2, next.x.abs() + next.y.abs());
    }

    #[test]
    fn first_location_visited_twice() {
        let input = "R8, R4, R4, R8";
        let path = parse_instructions(input);
        let (x, y) = find_first_location_visited_twice(path)
            .unwrap();

        assert_eq!(x, 4);
        assert_eq!(y, 0);
    }

    #[test]
    fn range_test() {
        assert_eq!(vec![3,2,1], (1..4).rev().collect::<Vec<i32>>());
    }

    #[test]
    fn interpolate_test() {
        let expected = vec![(1,0), (1,-1), (1,-2), (1,-3)];
        let start = Position {direction: CardinalDirection::North, x:1, y: 0 };
        let end = Position {direction: CardinalDirection::South, x:1, y: -4 };

        let actual = interpolate(&start, &end);
        assert_eq!(expected, actual.collect::<Vec<(i32,i32)>>());
    }

}