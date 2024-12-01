use super::Solution;

pub struct BathroomSecurity<'a> {
    input: &'a str,
}

impl<'a> BathroomSecurity<'a> {
    pub fn new(input: &'a str) -> BathroomSecurity<'a> {
        BathroomSecurity { input }
    }
}

impl Solution for BathroomSecurity<'_> {
    fn solve_part1(&self) -> String {
        let mut result: Vec<i8> = Vec::new();
        let mut previous_digit: i8 = 5;
        for line in parse_input(self.input) {
            let next = find_digit(previous_digit, line);
            result.push(next);
            previous_digit = next
        }

        String::from_iter(result.iter().map(|c| { c.to_string() }))
    }

    fn solve_part2(&self) -> String {
        let mut result : Vec<char> = Vec::new();
        let mut previous_digit : char = '5';
        for line in parse_input(self.input) {
            let next = find_digit_commitee_keypad(previous_digit, line);
            result.push(next);
            previous_digit = next
        }

        String::from_iter(result)
    }
}

#[derive(Debug, PartialEq)]
enum Direction {
    Up, Down, Left, Right
}

fn parse_input(input: &str) -> impl Iterator<Item = impl Iterator<Item = Direction> + '_> {
    let result = input
        .split("\n")
        .filter_map(|l| {
             if l.trim().len() > 0 {
                 Some(l.trim())
            } else {
                None
            } })
        .map(|line| {
            line
                .chars()
                .map(|c|
                    { match c {
                    'U' => Direction::Up,
                    'D' => Direction::Down,
                    'L' => Direction::Left,
                    'R' => Direction::Right,
                    _ => panic!("Unknown direction {}", c),
                }})
        });

    result
}

fn find_digit(start: i8, moves: impl Iterator<Item = Direction>) -> i8 {
    let mut digit: i8 = start;
    for direction in moves {
        match (direction, digit) {
            // Ignore invalid moves
            (Direction::Up, d) if d <= 3 => (),
            (Direction::Down, d) if d >= 7 => (),
            (Direction::Left, d) if d % 3 == 1 => (),
            (Direction::Right, d) if d % 3 == 0 => (),
            // Handle valid moves
            (Direction::Up, ..) => digit -= 3,
            (Direction::Down, ..) => digit += 3,
            (Direction::Left, ..) => digit -= 1,
            (Direction::Right, ..) => digit += 1,
        }
    }

    digit
}

fn find_digit_commitee_keypad(start: char, moves: impl Iterator<Item = Direction>) -> char {
    let mut digit: char = start;
    for direction in moves {
        match (direction, digit) {
            // Invalid moves
            (Direction::Up, d) if ['1', '2', '4', '5', '9'].contains(&d) => (),
            (Direction::Down, d) if ['A', 'D', 'C', '5', '9'].contains(&d) => (),
            (Direction::Left, d) if ['1', '2', '5', 'A', 'D'].contains(&d) => (),
            (Direction::Right, d) if ['1', '4', '9', 'C', 'D'].contains(&d) => (),
            // Valid moves
            (Direction::Up, d) if d == '3' => digit = '1',
            (Direction::Up, d) if d == 'D' => digit = 'B',
            (Direction::Up, d) => {
                let as_hex = d.to_digit(16).unwrap();
                digit = char::from_digit(as_hex - 4, 16)
                    .unwrap()
                    .to_ascii_uppercase()
            }
            (Direction::Down, d) if d == '1' => digit = '3',
            (Direction::Down, d) if d == 'B' => digit = 'D',
            (Direction::Down, d) => {
                let as_hex = d.to_digit(16).unwrap();
                digit = char::from_digit(as_hex + 4, 16)
                    .unwrap()
                    .to_ascii_uppercase()
            },
            (Direction::Left, d) => {
                let as_hex = d.to_digit(16).unwrap();
                digit = char::from_digit(as_hex - 1, 16)
                    .unwrap()
                    .to_ascii_uppercase()
            },
            (Direction::Right, d) => {
                let as_hex = d.to_digit(16).unwrap();
                digit = char::from_digit(as_hex + 1, 16)
                    .unwrap()
                    .to_ascii_uppercase()
            },
        }
    };

    digit
}

#[cfg(test)]
mod test {
    use super::*;

    fn get_example_input() -> String {
        String::from(r#"
            ULL
            RRDDD
            LURDL
            UUUUD"#)
    }

    #[test]
    fn example() {
        let input = get_example_input();
        let solver = BathroomSecurity::new(&input);
        let result = solver.solve_part1();
        assert_eq!("1985", result);
    }

    #[test]
    fn parse_input_test() {
        let input = get_example_input();
        let expected = vec![
             vec![Direction::Up, Direction::Left, Direction::Left ],
             vec![Direction::Right, Direction::Right, Direction::Down, Direction::Down, Direction::Down],
             vec![Direction::Left, Direction::Up, Direction::Right, Direction::Down, Direction::Left],
             vec![Direction::Up, Direction::Up, Direction::Up, Direction::Up, Direction::Down] ];

        let result = parse_input(&input);
        assert_eq!(expected, result.map(|l| { l.collect::<Vec<Direction>>() }).collect::<Vec<Vec<Direction>>>())
    }

    #[test]
    fn find_digit_test()
    {
        let line: Vec<Direction> = vec![Direction::Up, Direction::Left, Direction::Left ];
        let expected = 1;
        let actual = find_digit(5, line.into_iter());

        assert_eq!(expected, actual);
    }

    #[test]
    fn part_two_find_digit_test()
    {
        let line: Vec<Direction> = vec![Direction::Up, Direction::Left, Direction::Left ];
        let expected = '5';
        let actual = find_digit_commitee_keypad('5', line.into_iter());

        assert_eq!(expected, actual);
    }

    #[test]
    fn example_part2() {
        let input = get_example_input();
        let solver = BathroomSecurity::new(&input);
        let result = solver.solve_part2();
        assert_eq!("5DB3", result);
    }

}