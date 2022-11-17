
mod puzzle;
use clap::Parser;
use puzzle::solution::Solution;

/// Solutions to Advent of Code puzzle in Rust. Remember to set environement varialble AOC_SESSION_KEY
#[derive(Parser, Debug)]
#[command(author, version, about, long_about = None)]
pub struct PuzzleArgs {
    /// Year of Puzzle
    #[arg(short, long)]
    pub year: i32,
    /// Day of Puzzle
    #[arg(short, long)]
    pub day: i32,
    /// Whether to re-load cached input for the day
    #[arg(short, long, default_value_t=false)]
    pub reload_input: bool,
}


pub fn parse_args() -> PuzzleArgs {
    let args = PuzzleArgs::parse();
    args
}

pub async fn get_input(year: i32, day: i32, reload: bool) -> String {
    if let Some(input) = puzzle::puzzle_inputs::get_input(year, day, reload).await
    {
        return input;
    }

    panic!("Failed to get puzzle input!");
}

pub fn find_sovler<'a>(year: i32, day: i32, input: &'a str) -> Option<Box<dyn Solution + 'a>> {
    match (year, day) {
        (2021, 1) => Some(Box::from(puzzle::solution::year2021_day01::Solver::new(input))),
        _ => None,
    }
}
