use std::str::FromStr;

use super::Solution;


pub struct RedNosedReports<'a>
{
    input: &'a str
}

#[derive(PartialEq, Debug)]
struct Report {
    elements : Vec<i32>
}

enum Direction {
    Increasing,
    Decreasing
}

impl Report {
    fn get_direction(&self) -> Direction {
        match self.elements[0] < self.elements[self.elements.len() - 1] {
            true => Direction::Increasing,
            false => Direction::Decreasing,
        }
    }

    fn is_safe(&self) -> bool {
        let direction = self.get_direction();

        for (i, current) in self.elements[0 .. (self.elements.len() - 1)].iter().enumerate() {
            let next = self.elements[i + 1]; 
            if !Report::is_safe_pair(*current, next, &direction) {
                return false;
            }
        }

        true
    }

    fn is_safe_pair(current: i32, next: i32, direction: &Direction) -> bool {
        let diff = next - current;
        match direction {
            Direction::Decreasing if !(-3 ..= -1).contains(&diff) => false,
            Direction::Increasing if !(1 ..= 3).contains(&diff) => false,
            _ => true,
        }
    }

    fn create_excluding_element(&self, index_to_remove: usize) -> Option<Self> {
        if index_to_remove >= self.elements.len() {
            return None;
        }
        let first = &self.elements[0 .. index_to_remove];
        if index_to_remove < self.elements.len() - 1 {
            Some(Self { elements: [first, &self.elements[index_to_remove + 1..]].concat() })
        }
        else {
            Some(Self { elements: first.to_vec() })
        }
    }

    fn is_safe_part2(&self) -> bool {
        let direction = self.get_direction();

        for (i, current) in self.elements[0 .. self.elements.len() - 1].iter().enumerate() {
            let next = self.elements[i + 1];
            if !Report::is_safe_pair(*current, next, &direction) {
                let alternative_reports = [
                    self.create_excluding_element(i),
                    self.create_excluding_element(i + 1)
                ];
                let safe = alternative_reports.iter().any(|r| {
                    if let Some(report) = r {
                        report.is_safe()
                    }
                    else {
                        false
                    }
                });

                if !safe {
                    return  false;
                }
            }
        }

        true
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
        parse_reports(self.input)
            .iter()
            .filter(|r | { r.is_safe_part2() })
            .count()
            .to_string()
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

    #[test]
    fn safe_part2_test() {
        let parsed = parse_reports(EXAMPLE);
        assert_eq!(true, parsed[0].is_safe_part2());
        assert_eq!(false, parsed[1].is_safe_part2());
        assert_eq!(false, parsed[2].is_safe_part2());
        assert_eq!(true, parsed[3].is_safe_part2());
        assert_eq!(true, parsed[4].is_safe_part2());
        assert_eq!(true, parsed[5].is_safe_part2());
    }

    #[test]
    fn extra_safe_part2() {
        assert_eq!(true, Report::from_str("1 1").unwrap().is_safe_part2())
    }

    #[test]
    fn remove_cur_instead_of_next() {
        assert_eq!(true, Report::from_str("1 4 2 3").unwrap().is_safe_part2())
    }

    #[test]
    fn copy_with_elements_removed_text() {
        let a = Report { elements: vec![1,2,3,4] };
        assert_eq!(vec![2,3,4], a.create_excluding_element(0).unwrap().elements);
        assert_eq!(vec![1,3,4], a.create_excluding_element(1).unwrap().elements);
        assert_eq!(vec![1,2,4], a.create_excluding_element(2).unwrap().elements);
        assert_eq!(vec![1,2,3], a.create_excluding_element(3).unwrap().elements);
        assert_eq!(None, a.create_excluding_element(4));
    }
}