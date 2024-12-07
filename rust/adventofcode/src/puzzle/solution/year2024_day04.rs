use std::collections::HashMap;

use super::Solution;

pub struct CeresSearch<'a> {
    input: &'a str,
}

type Point = (usize, usize);

enum Patterns {
    Diagonal1(Point),
    Diagonal2(Point),
    Vertical(()),
    Horizontal(())
}

impl<'a> CeresSearch<'a> {
    pub fn new(input: &'a str) -> Self {
        Self { input }
    }

    fn input_as_2d_array(&self) -> Vec<Vec<char>> {
        self.input
            .split('\n')
            .filter_map(|l| {
                if l.trim().is_empty() {
                    None
                } else {
                    Some(l.trim())
                }
            })
            .map(|l| l.chars().collect::<Vec<char>>())
            .collect()
    }

    fn search_for_string(string: &Vec<char>, area: &[Vec<char>]) -> Vec<Patterns> {
        let mut reversed = string.clone();
        reversed.reverse();
        let mut results = vec![];
        for i in 0..area.len() {
            for j in 0..area[i].len() {
                // println!("Coords {} {}", i, j);
                let in_bounds_right_edge = j + string.len() <= area[i].len();
                let in_bounds_bottom_edge = i + string.len() <= area.len();
                if in_bounds_right_edge {
                    let horisontal = &area[i][j..j + string.len()];
                    // println!("Horizontal {:#?}", horisontal);
                    if horisontal == string || horisontal == reversed {
                        results.push(Patterns::Horizontal(()));
                    }
                }

                if in_bounds_bottom_edge {
                    let mut vertical: Vec<char> = vec![];
                    for line in area.iter().skip(i).take(string.len()) {
                        vertical.push(line[j]);
                    }
                    // println!("vertical {:#?}", vertical);
                    if vertical == *string || vertical == reversed {
                        results.push(Patterns::Vertical(()));
                    }
                }

                if in_bounds_right_edge && in_bounds_bottom_edge {
                    let mut diagonal1: Vec<char> = vec![];
                    for (count, x) in (i..i + string.len()).enumerate() {
                        diagonal1.push(area[x][j + count]);
                    }
                    // println!("Diagonal1 {:#?}", diagonal1);
                    if diagonal1 == *string || diagonal1 == reversed {
                        results.push(Patterns::Diagonal1((i, j)));
                    }
    
                    let mut diagonal2: Vec<char> = vec![];
                    for (count, x) in (i..i + string.len()).rev().enumerate() {
                        diagonal2.push(area[x][j + count]);
                    }
                    // println!("Diagonal2 {:#?}", diagonal2);
                    if diagonal2 == *string || diagonal2 == reversed {
                        results.push(Patterns::Diagonal2((i, j)));
                    }
                }

                // println!(" ###### ")
            }
        }

        results
    }
}

impl Solution for CeresSearch<'_> {
    fn solve_part1(&self) -> String {
        let input_parsed = self.input_as_2d_array();
        let search_string = Vec::from_iter("XMAS".chars());
        let num_matches = CeresSearch::search_for_string(&search_string, &input_parsed);

        num_matches.len().to_string()
    }

    fn solve_part2(&self) -> String {
        let input_parsed = self.input_as_2d_array();
        let seach_string = Vec::from_iter("MAS".chars());
        let matches = CeresSearch::search_for_string(&seach_string, &input_parsed);
        let mut map : HashMap<(usize, usize),  Vec<Patterns>> = HashMap::new();
        for m in matches {
            match m {
                Patterns::Diagonal1(p) | 
                Patterns::Diagonal2(p) => map.entry(p).or_default().push(m),
                _ => ()
            }
        }

        map.iter()
            .filter(|kvp| kvp.1.len() == 2)
            .count()
            .to_string()
    }
}

#[cfg(test)]
mod tests {
    use crate::puzzle::solution::Solution;

    use super::CeresSearch;

    const EXAMPLE: &str = "MMMSXXMASM
    MSAMXMSMSA
    AMXSXMAAMM
    MSAMASMSMX
    XMASAMXAMM
    XXAMMXXAMA
    SMSMSASXSS
    SAXAMASAAA
    MAMMMXMMMM
    MXMXAXMASX";

    const EXAMPLE2: &str = ".M.S......
..A..MSMS.
.M.S.MAA..
..A.ASMSM.
.M.S.M....
..........
S.S.S.S.S.
.A.A.A.A..
M.M.M.M.M.
..........";

    #[test]
    fn part1_test() {
        let solver = CeresSearch::new(EXAMPLE);
        assert_eq!("18", solver.solve_part1());
    }

    #[test]
    fn parse_test() {
        let solver = CeresSearch::new(EXAMPLE);
        let parsed_input = solver.input_as_2d_array();

        assert_eq!(10, parsed_input.len());
        assert_eq!(10, parsed_input[0].len());
        assert!(parsed_input.iter().all(|l| l.len() == 10));

        println!("{:#?}", parsed_input);
    }

    #[test]
    fn simple_pattern_horisontal_test() {
        let input = "....
                           XMAS
                           ....
                           SAMX";
        let solver = CeresSearch::new(input);
        assert_eq!("2", solver.solve_part1());
    }

    #[test]
    fn simple_pattern_vertical_test() {
        let input = "X..S
                           M..A
                           A..M
                           S..X";
        let solver = CeresSearch::new(input);
        assert_eq!("2", solver.solve_part1());
    }

    #[test]
    fn simple_pattern_diagonal1_test() {
        let input = "XS...
                           .MA..
                           ..AM.
                           ...SX
                           .....";
        let solver = CeresSearch::new(input);
        assert_eq!("2", solver.solve_part1());
    }

    #[test]
    fn simple_pattern_diagonal2_test() {
        let input = "...S.
                           ..AX.
                           .MM..
                           XA...
                           S....";
        let solver = CeresSearch::new(input);
        assert_eq!("2", solver.solve_part1());
    }

    #[test]
    fn simple_part2_test() {
        let input = "M.M
                           .A.
                           S.S";
        let solver = CeresSearch::new(input);
        assert_eq!("1", solver.solve_part2());
    }

    #[test]
    fn part_2_test() {
        let solver = CeresSearch::new(EXAMPLE2);
        assert_eq!("9", solver.solve_part2());
    }
}
