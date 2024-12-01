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

impl<'a> Solution for HistorianHysteria<'a> {
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
        "".to_string()
    }
}

fn parse_line(line: &str) -> (LocationId, LocationId) {
    let split : Vec<LocationId> = line
        .split("   ")
        .filter(|l| { l.len() > 0 })
        .map(|s| { LocationId(s.parse().expect("Invalid input")) })
        .collect();


    (split[0], split[1])
}

fn parse_input(input: &str) -> (Vec<LocationId>, Vec<LocationId>) {
    input.split("\n")
        .map(|line| {
            parse_line(line.trim())
        })
        .unzip()
}

#[cfg(test)]
mod tests {
    use super::{parse_input, parse_line, HistorianHysteria};

    #[test]
    fn parse_line_test() {
        let line = "3   4";
        let (left, right) = parse_line(line);
        assert_eq!(left.0, 3);
        assert_eq!(right.0, 4);
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
}