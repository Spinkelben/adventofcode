use regex::Regex;

use super::Solution;

pub struct SupplyStacks<'a> {
    input: &'a str
}
impl<'a> SupplyStacks<'a> {
    pub(crate) fn new(input: &'a str) -> Self {
        SupplyStacks { input }
    }
}

impl Solution for SupplyStacks<'_> {
    fn solve_part1(&self) -> String {
        "todo!()".to_string()
    }

    fn solve_part2(&self) -> String {
        "todo!()".to_string()
    }
}

#[derive(Debug, PartialEq, Eq)]
struct Move {
    amount : i32,
    source_idx: i32,
    dest_idx: i32,
}

fn parse_input(example: &str) -> (Vec<Move>, Vec<Vec<char>>) {
    let lines = example
        .split("\n")
        .collect::<Vec<&str>>();

    let move_stack_divider_line = lines.partition_point(|l| { l.trim().len() == 0});
    let (stack, moves) = lines.split_at(move_stack_divider_line);
    (parse_moves(moves), parse_stacks(stack))
}

fn parse_stacks(stack: &[&str]) -> Vec<Vec<char>> {
    let num_stacks = stack[stack.len() - 2]
        .trim()
        .split(" ")
        .filter_map(|s| { s.parse::<usize>().ok() })
        .max()
        .unwrap();

    let mut result = Vec::with_capacity(num_stacks);
    for _ in 0..num_stacks {
        result.push(Vec::new());
    };

    for i in (0..(stack.len() - 2)).rev() {
        let layer = stack[i];
        for (idx, stack_in_layer) in layer.chars().collect::<Vec<char>>().chunks(4).enumerate() {
            match stack_in_layer {
                ['[', c, ']', ' '] => result[idx].push(c.clone()),
                ['[', c, ']'] => result[idx].push(c.clone()), // handle final stack
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
                c.get(2).and_then(|c| { c.as_str().parse::<i32>().ok() }), 
                c.get(3).and_then(|c| { c.as_str().parse::<i32>().ok() })) {
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
        let example = "
    [D]    
[N] [C]    
[Z] [M] [P]
1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2";
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
}