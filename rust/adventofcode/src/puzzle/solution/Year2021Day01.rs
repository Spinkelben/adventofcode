use super::Solution;

pub struct Solver {
    day1Answer: Option<String>,
    day2Answer: Option<String>,
    input: String,
}

impl Solver {
    
}

impl Solution for Solver {
    fn new(input: String) -> Self {
        Solver {
            day1Answer: None,
            day2Answer: None, 
            input 
        }
    }

    fn solve_part1(&self) -> &str {
        let answer = match &self.day1Answer {
            Some(a) => a.as_str(),
            None => "",
        };

        answer
    }

    fn solve_part2(&self) -> &str {
        let answer = match &self.day2Answer {
            Some(a) => a.as_str(),
            None => "",
        };

        answer
    }
}