use std::{collections::HashSet, hash::Hash};

use super::Solution;
use grid::Grid;

pub struct GuardGallivant<'a> {
    input: &'a str,
}

impl<'a> GuardGallivant<'a> {
    pub fn new(input : &'a str) -> Self {
        Self { input }
    }

    fn parse_input(&self) -> Grid<char> {
        let rows = self.input.split("\n").map(|r| r.trim()).collect::<Vec<&str>>();
        let row_length = rows[0].len();
        Grid::from_vec(rows.iter().flat_map(|m| m.chars()).collect::<Vec<char>>(), row_length)
    }
    
}

impl Solution for GuardGallivant<'_> {
    fn solve_part1(&self) -> String {
        let input = self.parse_input();
        let start = find_in_grid(&input, '^').unwrap();
        let (visited_positions, _) = walk_in_grid(&input, start);
        let set : HashSet<(usize, usize)> = visited_positions.into_iter().map(|p| (p.row, p.col)).collect();
        set.len().to_string()
    }

    fn solve_part2(&self) -> String {
        let input = self.parse_input();
        let start = find_in_grid(&input, '^').unwrap();
        let (visited_positions, _) = walk_in_grid(&input, start);
        let set : HashSet<(usize, usize)> = visited_positions.into_iter().map(|p| (p.row, p.col)).collect();
        let mut potential_loops = 0;
        let mut new_obstacles = HashSet::new();
        for (row, col) in set.iter() {
            if (*row, *col) == start {
                continue;
            }

            let mut copy = input.clone();
            if let Some(a) = copy.get_mut(*row, *col) {
                *a = 'O';
                let (_, r#loop) = walk_in_grid(&copy, start);
                if r#loop {
                    // println!("Loop at {}, {}", row, col);
                    // println!("{:#?}", copy);
                    potential_loops += 1;
                    new_obstacles.insert((*row, *col));
                }
            }

        }   

        new_obstacles.len().to_string()
    }
}

fn find_in_grid(grid: &Grid<char>, target: char) -> Option<(usize, usize)> {
    grid.indexed_iter()
        .find(|(_, e)| **e == target)
        .map(|(coord, _)| coord)
}

fn get_next_step(direction: &Direction, position: (usize, usize)) -> Option<(usize, usize)> {
    let (row, col) = position;
    match direction {
        Direction::North => 
            row.checked_sub(1).map(|r | (r, col)),
        Direction::South => Some((row + 1, col)),
        Direction::East => Some((row, col + 1)),
        Direction::West => col.checked_sub(1).map(|c| (row, c)),
    }
}

#[derive(Debug, PartialEq, Clone, Copy, Eq, Hash)]
struct Place {
    row: usize,
    col: usize,
    direction: Direction,
}

fn turn_on_grid(grid: &Grid<char>, place: Place) -> Option<Place> {
    // Keep turning right until a new unblocked path is found
    let mut dir = place.direction;
    
    loop {
        let next_step = get_next_step(&dir, (place.row, place.col));
        let field = next_step.and_then(|(row, col) | grid.get(row, col));
        match (field, &dir) {
            (None, _) => return None,
            (Some('.'), _) => return Some(Place { row: next_step.unwrap().0, col: next_step.unwrap().1, direction: dir }),
            (Some('^'), _) => return Some(Place { row: next_step.unwrap().0, col: next_step.unwrap().1, direction: dir }),
            (_, Direction::North) => dir = Direction::East,
            (_, Direction::East) => dir = Direction::South,
            (_, Direction::South) => dir = Direction::West,
            (_, Direction::West) => dir = Direction::North,
        }
    }

}

fn walk_in_grid(grid: &Grid<char>, start: (usize, usize)) -> (Vec<Place>, bool) {
    let mut pos = start;
    let mut dir = Direction::North;
    let mut path = vec![];
    let mut visited_positions: HashSet<Place> = HashSet::new();
    let mut is_loop = false;
    loop {
        let place = Place{ row: pos.0, col: pos.1, direction: dir };
        if visited_positions.contains(&place) {
            is_loop = true;
            break;
        }

        path.push(place);
        visited_positions.insert(place);
        if let Some(next) = turn_on_grid(grid, place)  {
            pos = (next.row, next.col);
            dir = next.direction;   
        }
        else {
            break;
        }
    }

    (path, is_loop)
}


#[derive(Debug, PartialEq, Clone, Copy, Eq, Hash)]
enum Direction {
    North,
    South,
    East,
    West,
}



#[cfg(test)]
mod test {
    use super::*;

    #[test]
    fn test_part1() {
        let input =  "....#.....
                            .........#
                            ..........
                            ..#.......
                            .......#..
                            ..........
                            .#..^.....
                            ........#.
                            #.........
                            ......#...";
        let solver = GuardGallivant::new(input);
        assert_eq!(solver.solve_part1(), "41");
    }

    #[test]
    fn test_part2() {
        let input = "....#.....
                            .........#
                            ..........
                            ..#.......
                            .......#..
                            ..........
                            .#..^.....
                            ........#.
                            #.........
                            ......#...";
        let solver = GuardGallivant::new(input);
        assert_eq!(solver.solve_part2(), "6");
    }

    #[test]
    fn test_parse_input() {
        let input = "....#.....
        .........#
        ..........
        ..#.......
        .......#..
        ..........
        .#..^.....
        ........#.
        #.........
        ......#...";
        let solver = GuardGallivant::new(input);
        let grid = solver.parse_input();
        assert_eq!(grid.rows(), 10);
        assert_eq!(grid.cols(), 10);
        assert_eq!("....#.....", grid.iter_row(0).collect::<String>());
    }

    #[test]
    fn test_find_in_grid() {
        let input = "....#.....
        .........#
        ..........
        ..#.......
        .......#..
        ..........
        .#..^.....
        ........#.
        #.........
        ......#...";
        let solver = GuardGallivant::new(input);
        let grid = solver.parse_input();
        assert_eq!(find_in_grid(&grid, '^'), Some((6, 4)));
        assert_eq!(find_in_grid(&grid, '#'), Some((0, 4)));
        assert_eq!(find_in_grid(&grid, 'X'), None);
    }

    #[test]
    fn loop_detection_test() {
        let input = "....
                           ....
                           .#..
                           .^#.";
        let solver = GuardGallivant::new(input);
        let grid = solver.parse_input();
        let start = find_in_grid(&grid, '^').unwrap();
        let (visited_positions, is_loop) = walk_in_grid(&grid, start);
        assert_eq!(is_loop, false);
        assert_eq!(visited_positions.len(), 1);
    }

}