use regex::Regex;

use super::Solution;

pub struct SupplyStacks {
    stacks: Vec<Vec<char>>,
    moves: Vec<Move>,
}

impl SupplyStacks {
    pub(crate) fn new(input: &str) -> Self {
        let (moves, stacks) = parse_input(input);
        SupplyStacks { moves, stacks }
    }

    fn execute_moves(&self) -> Vec<Vec<char>> {
        let mut result = self.stacks
            .iter()
            .map(|s| { s.to_owned() })
            .collect::<Vec<Vec<char>>>();

        for crane_move in &self.moves {
            for _ in 0..crane_move.amount {
                let source_stack = result.get_mut(crane_move.source_idx - 1).unwrap();
                let cargo_crate = source_stack.pop().unwrap();
                let destination_stack = result.get_mut(crane_move.dest_idx - 1).unwrap();
                destination_stack.push(cargo_crate);
            }
        }

        result
    }

    fn execute_moves2(&self) -> Vec<Vec<char>> {
        let mut result = self.stacks.to_owned();
        for crane_move in &self.moves {
            let mut crates = Vec::new();
            for _ in 0 .. crane_move.amount {
                let source_stack = result.get_mut(crane_move.source_idx - 1).unwrap();
                crates.push(source_stack.pop().unwrap());
            }
            let dest_stack = result.get_mut(crane_move.dest_idx - 1).unwrap();
            for _ in 0 .. crane_move.amount {
                let value = crates.pop().unwrap();
                dest_stack.push(value)
            }
        }

        result
    }

    fn get_top_stack_element(stacks: &[Vec<char>]) -> String {
        stacks
            .iter()
            .filter_map(|s| 
                {
                    if !s.is_empty() 
                    {
                        return Some(s[s.len() - 1])
                    }
                    None
                })
            .collect::<String>()
    }
}

impl Solution for SupplyStacks {
    fn solve_part1(&self) -> String {
        let result = self.execute_moves();
        SupplyStacks::get_top_stack_element(&result)
    }

    fn solve_part2(&self) -> String {
        let result = self.execute_moves2();
        SupplyStacks::get_top_stack_element(&result)
    }
}

#[derive(Debug, PartialEq, Eq)]
struct Move {
    amount : i32,
    source_idx: usize,
    dest_idx: usize,
}

fn parse_input(example: &str) -> (Vec<Move>, Vec<Vec<char>>) {
    let lines = example
        .split("\n")
        .collect::<Vec<&str>>();

    let move_stack_divider_line = lines.iter().position(|l| 
        { 
            l.trim().is_empty()
        }).unwrap();

    let (stack, moves) = lines.split_at(move_stack_divider_line);
    (parse_moves(moves), parse_stacks(stack))
}

fn parse_stacks(stack: &[&str]) -> Vec<Vec<char>> {
    let num_stacks = stack[stack.len() - 1]
        .trim()
        .split(" ")
        .filter_map(|s| { s.parse::<usize>().ok() })
        .max()
        .unwrap();

    let mut result = Vec::with_capacity(num_stacks);
    for _ in 0..num_stacks {
        result.push(Vec::new());
    };

    for i in (0..(stack.len() - 1)).rev() {
        let layer = stack[i];
        for (idx, stack_in_layer) in layer.chars().collect::<Vec<char>>().chunks(4).enumerate() {
            match stack_in_layer {
                ['[', c, ']', ' '] => result[idx].push(*c),
                ['[', c, ']'] => result[idx].push(*c), // handle final stack
                _ => (),
            }
        }
    };

    result
}

fn parse_moves(moves: &[&str] ) -> Vec<Move> {
    let move_expression = Regex::new(r"move (\d+) from (\d+) to (\d+)").unwrap();
    moves.iter()
        .filter_map(|m| {
            move_expression.captures(m)
        })
        .filter_map(|c| {
            match 
                (c.get(1).and_then(|c| { c.as_str().parse::<i32>().ok() }), 
                c.get(2).and_then(|c| { c.as_str().parse::<usize>().ok() }), 
                c.get(3).and_then(|c| { c.as_str().parse::<usize>().ok() })) {
                (Some(amount), Some(source_idx), Some(dest_idx)) 
                    => Some(Move { amount, source_idx, dest_idx}),
                _ => None
            }
        })
        .collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn parse_test() {
        let example = 
"    [D]    
[N] [C]    
[Z] [M] [P]
1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2
";
        let expected_moves = vec![
            Move { amount: 1, source_idx: 2, dest_idx: 1 },
            Move { amount: 3, source_idx: 1, dest_idx: 3 },
            Move { amount: 2, source_idx: 2, dest_idx: 1 },
            Move { amount: 1, source_idx: 1, dest_idx: 2 },
        ];

        let expected_stacks = vec![ 
            vec![ 'Z', 'N' ],
            vec![ 'M', 'C', 'D' ],
            vec![ 'P' ],
        ];

        let (moves, stacks) = parse_input(example);
        assert_eq!(4, moves.len());
        assert_eq!(expected_moves, moves);
        assert_eq!(3, stacks.len());
        assert_eq!(expected_stacks, stacks);

    }

    #[test]
    fn part1_test() {
        let example = 
"    [D]    
[N] [C]    
[Z] [M] [P]
1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2
";

        let expected = "CMZ";
        let solver = SupplyStacks::new(example);
        assert_eq!(expected, solver.solve_part1())
    }

    #[test]
    fn part2_test() {
        let example = 
"    [D]    
[N] [C]    
[Z] [M] [P]
1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2
";

        let expected = "MCD";
        let solver = SupplyStacks::new(example);
        assert_eq!(expected, solver.solve_part2())
    }
}