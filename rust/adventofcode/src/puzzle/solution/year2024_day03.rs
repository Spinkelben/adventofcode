use std::{iter::Peekable, str::Chars};

use super::Solution;

pub struct MullItOver<'a> {
    input: &'a str
}


#[derive(Debug)]
#[derive(PartialEq)]
struct MulExpression {
    left: i32,
    right: i32,
}

impl MulExpression {
    fn execute(&self) -> i32 {
        self.left * self.right
    }
}

impl<'a> MullItOver<'a> {
    pub fn new(input: &'a str) -> Self {
        Self { input }
    }

    fn scan(&self, use_do_dont: bool) -> Vec<MulExpression> {
        let mut result :Vec<MulExpression> = vec![];
        let mut iter = self.input.chars().peekable();
        let mut enabled = true;
        while let Some(curr) = iter.next() {
            match curr {
                'm' if enabled => {
                    let valid = MullItOver::scan_mul(&mut iter);
                    if !valid {
                        continue;
                    }
                    let left = MullItOver::scan_digits(&mut iter);
                    if left.is_none() {
                        continue;
                    }
                    let left = left.unwrap().parse::<i32>();
                    if left.is_err() {
                        continue;
                    }
                    let left = left.unwrap();

                    if iter.peek() != Some(&',') {
                        continue;
                    }

                    iter.next();
                    let right = MullItOver::scan_digits(&mut iter);
                    let right = right.and_then(|r| { r.parse::<i32>().ok() });
                    if right.is_none() {
                        continue;
                    }

                    let right = right.unwrap();

                    if iter.peek() != Some(&')') {
                        continue;
                    }

                    iter.next();
                    result.push(MulExpression { left, right });
                }
                'd' if use_do_dont => { // scan do() or don't()
                    if iter.peek() != Some(&'o') {
                        continue;
                    }

                    iter.next();
                    if MullItOver::scan_parens(&mut iter) {
                        // scanned "do()"
                        enabled = true;
                        continue;
                    }

                    if iter.next_if_eq(&'n').is_none() {
                        continue;
                    }

                    if iter.next_if_eq(&'\'').is_none() {
                        continue;
                    }

                    if iter.next_if_eq(&'t').is_none() {
                        continue;
                    }

                    if MullItOver::scan_parens(&mut iter) {
                        // scanned "don't()"
                        enabled = false;
                    }
                }
                _ => (),
            }
        }

        result
    }

    fn scan_parens(iterator: &mut Peekable<Chars<'_>>) -> bool {
        if iterator.next_if_eq(&'(').is_none() {
            return false;
        }

        if iterator.next_if_eq(&')').is_none() {
            return  false;
        }

        true
    }

    fn scan_mul(iterator: &mut Peekable<Chars<'_>>) -> bool {
        if iterator.peek() != Some(&'u') {
            return false;
        }

        iterator.next();
        if iterator.peek() != Some(&'l') {
            return false;
        }

        iterator.next();
        if iterator.peek() != Some(&'(') {
            return false;
        }

        iterator.next();
        true
    }

    fn scan_digits(iterator: &mut Peekable<Chars<'_>>) -> Option<String> {
        let mut digit = vec![];
        while let Some(c) = iterator.next_if(|next| { next.is_ascii_digit() }) {
            digit.push(c);
        }

        if digit.is_empty() {
            return None;
        }

        Some(String::from_iter(digit))
    }
}

impl Solution for MullItOver<'_> {
    fn solve_part1(&self) -> String {
        let mul_instructions = self.scan(false);
        let result :i32 = mul_instructions
            .iter()
            .map(|e| { e.execute() })
            .sum();
            
        result.to_string()
    }

    fn solve_part2(&self) -> String {
        let mul_instructions = self.scan(true);
        let result :i32 = mul_instructions
            .iter()
            .map(|e| { e.execute() })
            .sum();
            
        result.to_string()
    }
}

#[cfg(test)]
mod test {
    use crate::puzzle::solution::{year2024_day03::MulExpression, Solution};

    use super::MullItOver;

    const EXAMPLE: &str = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";

    #[test]
    fn parse_mul_test() {
        let solver = MullItOver::new(EXAMPLE);
        let expressions = solver.scan(false);
        assert_eq!(vec![
            MulExpression { left: 2, right: 4 },
            MulExpression { left: 5, right: 5 },
            MulExpression { left: 11, right: 8 },
            MulExpression { left: 8, right: 5 },
            ], 
            expressions)
    }

    #[test]
    fn part1_test() {
        let solver = MullItOver::new(EXAMPLE);
        assert_eq!("161", solver.solve_part1());
    }

    #[test]
    fn part2_test() {
        let solver = MullItOver::new("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))");
        assert_eq!("48", solver.solve_part2());
    }
}