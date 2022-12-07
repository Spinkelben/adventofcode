pub mod year2021_day01;
pub mod year2016_day01;
pub mod year2016_day02;
pub mod year2016_day03;
pub mod year2022_day01;
pub mod year2022_day02;
pub mod year2022_day03;
pub mod year2022_day04;
pub mod year2022_day05;
pub mod year2022_day06;


pub trait Solution {
    fn solve_part1(&self) -> String;

    fn solve_part2(&self) -> String;
}