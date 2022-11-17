use super::Solution;

pub struct Solver<'a> {
    input: &'a str,
}

impl<'a> Solver<'a> {
    pub fn new(input: &'a str) -> Self {
        Solver {
            input 
        }
    }

    fn get_numbers(&self) -> impl Iterator<Item=i32> + '_ {
        self.input
            .split("\n")
            .filter_map(|l| { l.parse().ok() })
    }

    fn solve_part1_internal(&self) -> String {
        let mut numbers = self.get_numbers();

        let first = numbers.next().unwrap();

        let mut increases = 0;
        let mut prev = first;
        for n in numbers {
            if n > prev
            {
                increases += 1;
            }
            prev = n;
            
        }
        

        increases.to_string()
    }

    fn solve_part2_internal(&self) -> String {
        let mut numbers = self.get_numbers();
        let mut increases = 0;
        let mut window = std::collections::VecDeque::<i32>::with_capacity(3);
        window.push_back(numbers.next().unwrap());
        window.push_back(numbers.next().unwrap());
        window.push_back(numbers.next().unwrap());
        let mut prev_sum :i32 = window.iter().sum();

        for n in numbers
        {
            window.pop_front();
            window.push_back(n);
            let sum: i32 = window.iter().sum();
            if sum > prev_sum
            {
                increases += 1;
            }
            prev_sum = sum;
        }

        increases.to_string()
    }
}

impl<'a> Solution for Solver<'a> {
    fn solve_part1(&self) -> String {
        self.solve_part1_internal()
    }

    fn solve_part2(&self) -> String {
        self.solve_part2_internal()
    }
}