use clap::builder::Str;

mod Year2021Day01;

pub trait Solution {
    fn new(input: String) -> Self;

    fn solve_part1(&self) -> &str;

    fn solve_part2(&self) -> &str;
}