
mod puzzle;
use clap::Parser;

/// Solutions to Advent of Code puzzle in Rust
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
