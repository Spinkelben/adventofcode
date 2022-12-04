use super::Solution;

pub struct CampCleanup<'a> {
    input: &'a str
}

impl<'a> CampCleanup<'a> {
    pub fn new(input: &'a str) -> Self {
        CampCleanup { input }
    }
}

impl Solution for CampCleanup<'_> {
    fn solve_part1(&self) -> String {
        let pairs = parse_input(self.input);
        pairs
            .iter()
            .filter(|p| { fully_contains(p) })
            .count()
            .to_string()
    }

    fn solve_part2(&self) -> String {
        parse_input(self.input)
            .iter()
            .filter(|p| {
                overlaps(p)
            })
            .count()
            .to_string()
    }
}

fn fully_contains((a , b): &((i32, i32), (i32, i32))) -> bool {
    a.0 <= b.0 && a.1 >= b.1 ||
    a.0 >= b.0 && a.1 <= b.1
}

fn overlaps((a, b): &((i32, i32), (i32, i32))) -> bool {
    a.1 >= b.0 && a.0 <= b.1
}

fn parse_input(input : &str) -> Vec<((i32, i32), (i32, i32))> {
    fn parse_range(range: &str) -> Option<(i32, i32)> {
        let split : Vec<&str> = range.split("-").collect();
        if split.len() == 2
        {
            let start = split[0].parse::<i32>();
            let end = split[1].parse::<i32>();
            return match (start, end) {
                (Ok(x), Ok(y)) => Some((x, y)),
                _ => None,
            }
        }
        None
    }

    input.split("\n")
        .filter_map(|line| {
            let split : Vec<Option<(i32, i32)>> = line
                .trim()
                .split(",")
                .map(|r| { parse_range(r) })
                .collect();
            if split.len() == 2 {
                return match (split[0], split[1]) {
                    (Some(l), Some(r)) => Some((l, r)),
                    _ => None,
                }
            }
            None
        })
        .collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn input_parse_test() {
        let input = 
            r"  2-4,6-8
                2-3,4-5
                5-7,7-9
                2-8,3-7
                6-6,4-6
                2-6,4-8
            ";
        
        let expected = vec![ 
            ((2, 4), (6, 8)),
            ((2, 3), (4, 5)),
            ((5, 7), (7, 9)),
            ((2, 8), (3, 7)),
            ((6, 6), (4, 6)),
            ((2, 6), (4, 8)),
        ];

        assert_eq!(expected, parse_input(input))
    }

    #[test]
    fn fully_contain_test() {
        assert_eq!(true, fully_contains(&((2, 8), (3, 7))));
        assert_eq!(true, fully_contains(&((3, 7), (2, 8))));

        assert_eq!(false, fully_contains(&((2, 4), (6, 8))));
        assert_eq!(false, fully_contains(&((2, 3), (4, 5))));
        assert_eq!(false, fully_contains(&((5, 7), (7, 9))));
        assert_eq!(true, fully_contains(&((2, 8), (3, 7))));
        assert_eq!(true, fully_contains(&((6, 6), (4, 6))));
        assert_eq!(false, fully_contains(&((2, 6), (4, 8))));
    }

    #[test]
    fn part1_test() {
        let input = 
            r"  2-4,6-8
                2-3,4-5
                5-7,7-9
                2-8,3-7
                6-6,4-6
                2-6,4-8
            ";
        assert_eq!("2", CampCleanup::new(input).solve_part1())
    }

    #[test]
    fn overlap_test() {
        assert!(overlaps(&((5,7),(7,9))));
        assert!(overlaps(&((7,9),(5,7))));
        assert!(overlaps(&((2,8),(3,7))));
        assert!(overlaps(&((6,6),(4,6))));
        assert!(overlaps(&((2,6),(4,8))));
        assert!(!overlaps(&((2,4),(6,8))));
        assert!(!overlaps(&((2,3),(4,5))));
        assert!(!overlaps(&((3,5),(1,2))));
    }

    #[test]
    fn part2_test() {
        let input = 
            r"  2-4,6-8
                2-3,4-5
                5-7,7-9
                2-8,3-7
                6-6,4-6
                2-6,4-8
            ";
        assert_eq!("4", CampCleanup::new(input).solve_part2())
    }
}