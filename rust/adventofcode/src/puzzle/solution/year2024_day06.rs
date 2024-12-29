use std::collections::HashSet;

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
        let rows = self.input.split("\n").collect::<Vec<&str>>();
        let row_length = rows[0].len();
        Grid::from_vec(rows.iter().flat_map(|m| m.trim().chars()).collect::<Vec<char>>(), row_length)
    }
    
}

impl Solution for GuardGallivant<'_> {
    fn solve_part1(&self) -> String {
        let input = self.parse_input();
        let visited_positions = walk_in_grid(&input);
        let set : HashSet<(usize, usize)> = visited_positions.into_iter().collect();
        set.len().to_string()
    }

    fn solve_part2(&self) -> String {
        let input = self.parse_input();
        format!("Part 2: {}", input.rows())
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

fn walk_in_grid(grid: &Grid<char>) -> Vec<(usize, usize)> {
    let mut pos = find_in_grid(grid, '^').unwrap();
    let mut dir = Direction::North;
    let mut visited_positions = vec![];
    loop {
        visited_positions.push(pos);
        if let Some(next) = get_next_step(&dir, pos) {
            let (row, col) = next;
            let next = grid.get(row, col);
            match (next, &dir) {
                (None, _) => break,
                (Some('#'), Direction::North) => dir = Direction::East,
                (Some('#'), Direction::East) => dir = Direction::South,
                (Some('#'), Direction::South) => dir = Direction::West,
                (Some('#'), Direction::West) => dir = Direction::North,
                _ => (),
            }

            if let Some(t) = get_next_step(&dir, pos) {
                pos = t;
            }
            else {
                break;
            }
        }
        else {
            break;
        }
    }

    visited_positions
}

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
        let input = "Test Input";
        let solver = GuardGallivant::new(input);
        assert_eq!(solver.solve_part2(), "Part 2: 1");
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

}