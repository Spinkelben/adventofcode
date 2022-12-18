use std::str::FromStr;

use regex::{Regex, Captures};

use super::Solution;

pub struct RegolithReservoir {
    
}

impl RegolithReservoir {
    pub fn new(input: &str) -> Self { Self {  } }
}

impl Solution for RegolithReservoir {
    fn solve_part1(&self) -> String {
        todo!()
    }

    fn solve_part2(&self) -> String {
        todo!()
    }
}

#[derive(PartialEq, Debug)]
struct RockScan {
    rock_formation: Vec<Vec<(i32, i32)>>
}

impl<'a> FromStr for RockScan {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let re = Regex::new(r"(\d+),(\d+)").unwrap();
        let result : Vec<Vec<(i32, i32)>> = s.lines()
            .map(|l| {
                re.captures_iter(l)
                .map(|s| {
                    (s.get(1).unwrap().as_str().parse::<i32>().ok().unwrap(),
                     s.get(2).unwrap().as_str().parse::<i32>().ok().unwrap())
                }).collect::<Vec<(i32, i32)>>()
            })
            .collect();
        Ok(RockScan { rock_formation: result })
    }
}

#[cfg(test)]
mod tests {
    use super::*;


    static EXAMPLE: &str = "498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";

    #[test]
    fn parse_test() {
        let expected = vec![
            vec![(498, 4), (498,6), (496,6)],
            vec![(503, 4), (502, 4), (502,9), (494,9)],
        ];
        let actual: Result<RockScan, ()> = EXAMPLE.parse();
        assert_eq!(actual.ok().unwrap(), RockScan { rock_formation: expected })

    }
}