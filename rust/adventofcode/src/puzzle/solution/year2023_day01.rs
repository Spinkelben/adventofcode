use super::Solution;

pub struct Trebuchet<'a> {
    input: &'a str
}

impl<'a> Trebuchet<'a> {
    pub fn new(input: &'a str) -> Self {
        Self { input }
    }
}

impl Solution for Trebuchet<'_> {
    fn solve_part1(&self) -> String {
        read_numbers(self.input)
            .iter()
            .sum::<i32>()
            .to_string()
    }

    fn solve_part2(&self) -> String {
        get_lines(self.input)
            .iter()
            .map(|line| {
                read_number_from_line_part2(line)
            })
            .sum::<i32>()
            .to_string()
    }
}

fn read_number_from_line(line :&str) -> i32 {
    let digits = line.chars()
        .filter(|c| {
            c.is_ascii_digit()
        }).collect::<Vec<char>>();

    String::from_iter([digits[0], digits[digits.len() - 1]])
        .parse()
        .unwrap_or_else(| error | panic!("Failed to parse int from {:#?}: {:#?} ", digits, error))
}

fn read_number_from_line_part2(line :&str) -> i32 {
    let converted = line.replace("one", "on1ne")
        .replace("two", "tw2wo")
        .replace("three", "thre3hree")
        .replace("four", "fou4our")
        .replace("five", "fiv5ive")
        .replace("six", "si6ix")
        .replace("seven", "seve7even")
        .replace("eight", "eigh8ight")
        .replace("nine", "nin9ine");
    
    read_number_from_line(&converted)
}

fn read_numbers(text : &str) -> Vec<i32> {
    get_lines(text)
        .iter()
        .map(|line| { read_number_from_line(line) })
        .collect()
}

fn get_lines(text: &str) -> Vec<&str> {
    text.split("\n")
        .filter_map(|l| {
        let trimmed = l.trim();
        if trimmed.is_empty() {
            None
        }
        else {
            Some(trimmed)
        }
    }).collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    const EXAMPLE : &str = "1abc2
        pqr3stu8vwx
        a1b2c3d4e5f
        treb7uchet
        ";

    const EXAMPLE2 : &str = "
    two1nine
    eightwothree
    abcone2threexyz
    xtwone3four
    4nineeightseven2
    zoneight234
    7pqrstsixteen
    ";

    #[test]
    fn parse_line_test() {
        assert_eq!(12, read_number_from_line("1abc2"));
        assert_eq!(38, read_number_from_line("pqr3stu8vwx"));
        assert_eq!(15, read_number_from_line("a1b2c3d4e5f"));
        assert_eq!(77, read_number_from_line("treb7uchet"));
    }

    #[test]
    fn part1_handle_line_with_single_number() {
        assert_eq!(11, read_number_from_line("1zjgqlz"));
        assert_eq!(77, read_number_from_line("pcfzzjfhqkxhfpztpv7"));
    }

    #[test]
    fn part2_handle_line_with_single_number() {
        assert_eq!(11, read_number_from_line_part2("1zjgqlz"));
        assert_eq!(77, read_number_from_line_part2("pcfzzjfhqkxhfpztpv7"));
    }

    #[test]
    fn part2_handle_overlapping_numbers() {
        assert_eq!(18, read_number_from_line_part2("oneight"))
    }

    #[test]
    fn parse_input_test() {
        assert_eq!(vec![12, 38, 15, 77], read_numbers(EXAMPLE));
    }

    #[test]
    fn part1() {
        let solver = Trebuchet::new(EXAMPLE);
        assert_eq!("142", solver.solve_part1());
    }

    #[test]
    fn parse_line_part2_test()
    {
        assert_eq!(29, read_number_from_line_part2("two1nine"));
        assert_eq!(83, read_number_from_line_part2("eightwothree"));
        assert_eq!(13, read_number_from_line_part2("abcone2threexyz"));
        assert_eq!(24, read_number_from_line_part2("xtwone3four"));
        assert_eq!(42, read_number_from_line_part2("4nineeightseven2"));
        assert_eq!(14, read_number_from_line_part2("zoneight234"));
        assert_eq!(76, read_number_from_line_part2("7pqrstsixteen"));
    }

    #[test]
    fn part2() {
        let solver = Trebuchet::new(EXAMPLE2);
        assert_eq!("281", solver.solve_part2());
    }
}