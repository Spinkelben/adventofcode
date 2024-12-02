use std::str::FromStr;

use super::Solution;


pub struct RedNosedReports<'a>
{
    input: &'a str
}

struct Report {
    elements : Vec<i32>
}

enum Direction {
    Increasing,
    Decreasing
}

impl Report {
    fn get_direction(&self) -> Direction {
        match self.elements[0] < self.elements[1] {
            true => Direction::Increasing,
            false => Direction::Decreasing,
        }
    }

    fn is_safe(&self) -> bool {
        let direction = self.get_direction();

        for (i, e) in self.elements[0 .. (self.elements.len() - 1)].iter().enumerate() {
            let diff = e - self.elements[i + 1];
            match direction {
                Direction::Decreasing => if diff <= 0 || diff > 3 {
                    return false;
                },
                Direction::Increasing => if diff >= 0 || diff < -3 {
                    return false;
                }
            }
        }

        true
    }

    fn is_safe_part2(&self) -> bool {
        false
    }
}

impl FromStr for Report {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let elements = s.split(" ")
            .map(|e| { e.parse().unwrap() })
            .collect();

        Ok(Self { elements })
    }
}

impl<'a> RedNosedReports<'a> {
    pub fn new(input: &'a str) -> Self {
        Self { input }
    }
}

impl Solution for RedNosedReports<'_> {
    fn solve_part1(&self) -> String {
        parse_reports(self.input)
            .iter()
            .filter(|r| { r.is_safe()} )
            .count()
            .to_string()
    }

    fn solve_part2(&self) -> String {
        "todo!()".to_string()
    }
}

fn parse_reports(input: &str) -> Vec<Report> {
    input.split("\n")
        .filter_map(|l| {
            let trimmed = l.trim();
            if trimmed.is_empty() {
                return  None;
            }

            Report::from_str(trimmed).ok()
        })
        .collect()
}

#[cfg(test)]
mod tests {
    use std::str::FromStr;

    use crate::puzzle::solution::Solution;

    use super::{parse_reports, RedNosedReports, Report};

    #[test]
    fn parse_report() {
        let line = "7 6 4 2 1";
        let report = Report::from_str(line).unwrap();
        assert_eq!(report.elements, [7, 6, 4, 2, 1]);
    }

    const EXAMPLE : &str = "7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
";

    #[test]
    fn parse_example() {
        let parsed = parse_reports(EXAMPLE);
        assert_eq!(vec![7,6,4,2,1], parsed[0].elements);
        assert_eq!(vec![1,2,7,8,9], parsed[1].elements);
        assert_eq!(vec![9,7,6,2,1], parsed[2].elements);
        assert_eq!(vec![1,3,2,4,5], parsed[3].elements);
        assert_eq!(vec![8,6,4,4,1], parsed[4].elements);
        assert_eq!(vec![1,3,6,7,9], parsed[5].elements);
    }

    #[test]
    fn safe_test() {
        
        let parsed = parse_reports(EXAMPLE);
        assert_eq!(true, parsed[0].is_safe());
        assert_eq!(false, parsed[1].is_safe());
        assert_eq!(false, parsed[2].is_safe());
        assert_eq!(false, parsed[3].is_safe());
        assert_eq!(false, parsed[4].is_safe());
        assert_eq!(true, parsed[5].is_safe());
    }

    #[test]
    fn part1_test() {
        let solver = RedNosedReports::new(EXAMPLE);
        assert_eq!("2", solver.solve_part1())
    }

    #[test]
    fn extra_safe() {
        assert_eq!(false, Report::from_str("1 1").unwrap().is_safe())
    }

    #[test]
    fn part2_test() {
        let solver = RedNosedReports::new(EXAMPLE);
        assert_eq!("4", solver.solve_part2());
    }

    fn safe_part2_test() {
        let parsed = parse_reports(EXAMPLE);
        assert_eq!(true, parsed[0].is_safe_part2());
        assert_eq!(false, parsed[1].is_safe_part2());
        assert_eq!(false, parsed[2].is_safe_part2());
        assert_eq!(true, parsed[3].is_safe_part2());
        assert_eq!(true, parsed[4].is_safe_part2());
        assert_eq!(true, parsed[5].is_safe_part2());
    }
}