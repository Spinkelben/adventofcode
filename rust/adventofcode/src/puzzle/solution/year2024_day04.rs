use super::Solution;

pub struct CeresSearch<'a> {
    input: &'a str,
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

    fn search_for_string(string: &Vec<char>, area: &Vec<Vec<char>>) -> usize {
        let mut reversed = string.clone();
        reversed.reverse();
        let mut num_matches: usize = 0;
        for i in 0..area.len() {
            for j in 0..area[i].len() {
                // println!("Coords {} {}", i, j);
                let in_bounds_right_edge = j + string.len() <= area[i].len();
                let in_bounds_bottom_edge = i + string.len() <= area.len();
                if in_bounds_right_edge {
                    let horisontal = &area[i][j..j + string.len()];
                    // println!("Horizontal {:#?}", horisontal);
                    if horisontal == string || horisontal == reversed {
                        num_matches += 1;
                    }
                }

                if in_bounds_bottom_edge {
                    let mut vertical: Vec<char> = vec![];
                    for x in i..i + string.len() {
                        vertical.push(area[x][j]);
                    }
                    // println!("vertical {:#?}", vertical);
                    if vertical == *string || vertical == reversed {
                        num_matches += 1;
                    }
                }

                if in_bounds_right_edge && in_bounds_bottom_edge {
                    let mut diagonal1: Vec<char> = vec![];
                    for (count, x) in (i..i + string.len()).enumerate() {
                        diagonal1.push(area[x][j + count]);
                    }
                    // println!("Diagonal1 {:#?}", diagonal1);
                    if diagonal1 == *string || diagonal1 == reversed {
                        num_matches += 1;
                    }
    
                    let mut diagonal2: Vec<char> = vec![];
                    for (count, x) in (i..i + string.len()).rev().enumerate() {
                        diagonal2.push(area[x][j + count]);
                    }
                    // println!("Diagonal2 {:#?}", diagonal2);
                    if diagonal2 == *string || diagonal2 == reversed {
                        num_matches += 1;
                    }
                }

                // println!(" ###### ")
            }
        }

        num_matches
    }
}

impl Solution for CeresSearch<'_> {
    fn solve_part1(&self) -> String {
        let input_parsed = self.input_as_2d_array();
        let search_string = Vec::from_iter("XMAS".chars());
        let num_matches = CeresSearch::search_for_string(&search_string, &input_parsed);

        num_matches.to_string()
    }

    fn solve_part2(&self) -> String {
        "".to_string()
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
}
