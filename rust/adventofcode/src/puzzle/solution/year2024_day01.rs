use std::collections::HashMap;

use super::Solution;

pub struct HistorianHysteria<'a> {
    input: &'a str,
}

#[derive(Clone, Copy, PartialEq, Eq, PartialOrd, Ord)]
struct LocationId(i32);

impl<'a> HistorianHysteria<'a> {
    pub fn new(input: &'a str) -> Self {
        Self { input}
    }
}

impl Solution for HistorianHysteria<'_> {
    fn solve_part1(&self) -> String {
        let (mut left, mut right) = parse_input(self.input);
        left.sort();
        right.sort();
        let sum = left.iter().zip(right.iter())
            .map(|(l,r)| {
                (l.0 - r.0).abs()
            })
            .sum::<i32>();

        sum.to_string()
    }

    fn solve_part2(&self) -> String {
        let (left, right) = parse_input(self.input);
        let scores = right.iter()
            .fold(HashMap::new(), |mut acc, e| {
                acc.entry(&e.0)
                    .and_modify(|count| { *count += 1 })
                    .or_insert(1);

                acc
            });

        left.iter()
            .map(|id | {
                id.0 * scores.get(&id.0).unwrap_or(&0)
            })
            .sum::<i32>()
            .to_string()
    }
}

fn parse_line(line: &str) -> Option<(LocationId, LocationId)> {
    if line.is_empty()
    {
        return None;
    }

    let split : Vec<LocationId> = line
        .split("   ")
        .filter(|l| { !l.is_empty() })
        .map(|s| { LocationId(s.parse().expect("Invalid input")) })
        .collect();

    match split[..] {
        [a , b] => Some((a, b)),
        _ => None
    }
}

fn parse_input(input: &str) -> (Vec<LocationId>, Vec<LocationId>) {
    input.split("\n")
        .filter_map(|line| {
            parse_line(line.trim())
        })
        .unzip()
}

#[cfg(test)]
mod tests {
    use crate::puzzle::solution::Solution;

    use super::{parse_input, parse_line, HistorianHysteria };
    const EXAMPLE : &str = "3   4
4   3
2   5
1   3
3   9
3   3
";


    #[test]
    fn parse_line_test() {
        let line = "3   4";
        if let Some((left, right)) = parse_line(line) {
            assert_eq!(left.0, 3);
            assert_eq!(right.0, 4);
        }
        else {
            assert!(false, "Failed to parse line")
        }
    }

    #[test]
    fn parse_input_test() {
        let input = "77679   19534
67503   19006
22862   87523
68125   36752
32102   85209
57298   32444
44985   10870
";
        let (left, right) = parse_input(input);

        assert_eq!(7, left.len());
        assert_eq!(7, right.len());
        assert_eq!(77679, left[0].0);
        assert_eq!(44985, left[6].0);
        assert_eq!(19534, right[0].0);
        assert_eq!(10870, right[6].0);
    }

    #[test]
    fn example_part1_test() {
        let solver = HistorianHysteria::new(EXAMPLE);
        assert_eq!("11", solver.solve_part1());
    }

    #[test]
    fn example_part2_test() {
        let solver = HistorianHysteria::new(EXAMPLE);
        assert_eq!("31", solver.solve_part2());
    }
}