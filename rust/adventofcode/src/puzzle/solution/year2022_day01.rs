use super::Solution;

pub struct CalorieCounter<'a> {
    input: &'a str,
}

impl<'a> CalorieCounter<'a> {
    pub fn new(input: &'a str) -> Self {
        CalorieCounter { input }
    }

}

impl Solution for CalorieCounter<'_> {
    fn solve_part1(&self) -> String {
        let elves = read_input(self.input);
        elves.iter()
            .map(|e| { e.iter().sum::<i32>()  })
            .max()
            .unwrap()
            .to_string()
    }

    fn solve_part2(&self) -> String {
        let elves = read_input(self.input);
        let mut snacks : Vec<i32> = elves.iter()
            .map(|e| { e.iter().sum() })
            .collect();
        snacks.sort_unstable();
        snacks[snacks.len() - 3..].iter().sum::<i32>().to_string()
            
    }
}

fn read_input(input: &str) -> Vec<Vec<i32>> {
    let mut line_iter = input
        .split("\n")
        .map(|l| l.trim());

    let mut result: Vec<Vec<i32>> = Vec::new();
    let mut current_elf: Vec<i32> = Vec::new();
    for line in line_iter {
        if let Ok(number) = line.parse::<i32>() {
            current_elf.push(number);
        }
        else if line.is_empty() && !current_elf.is_empty() {
            result.push(current_elf);
            current_elf = Vec::new();
        }
    }

    if !current_elf.is_empty() {
        result.push(current_elf);
    }

    result
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn parse_input() {
        let input = r"
        1000
        2000
        3000
        
        4000
        
        5000
        6000
        
        7000
        8000
        9000
        
        10000
";
        let expected = vec![ 
            vec![1000, 2000, 3000], 
            vec![4000],
            vec![5000, 6000],
            vec![7000, 8000, 9000],
            vec![10000]
        ];

        assert_eq!(read_input(input), expected);

    }
}
