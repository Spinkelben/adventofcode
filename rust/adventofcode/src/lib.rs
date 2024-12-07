
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
    PuzzleArgs::parse()
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
        (2016, 1) => Some(Box::from(puzzle::solution::year2016_day01::NoTimeForTaxicab::new(input))),
        (2016, 2) => Some(Box::from(puzzle::solution::year2016_day02::BathroomSecurity::new(input))),
        (2016, 3) => Some(Box::from(puzzle::solution::year2016_day03::SquareWithThreeSides::new(input))),
        (2022, 1) => Some(Box::from(puzzle::solution::year2022_day01::CalorieCounter::new(input))),
        (2022, 2) => Some(Box::from(puzzle::solution::year2022_day02::RockPaperScissors::new(input))),
        (2022, 3) => Some(Box::from(puzzle::solution::year2022_day03::RucksackReorganization::new(input))),
        (2022, 4) => Some(Box::from(puzzle::solution::year2022_day04::CampCleanup::new(input))),
        (2022, 5) => Some(Box::from(puzzle::solution::year2022_day05::SupplyStacks::new(input))),
        (2022, 6) => Some(Box::from(puzzle::solution::year2022_day06::TuningTrouble::new(input))),
        (2022, 7) => Some(Box::from(puzzle::solution::year2022_day07::NoSpaceLeftOnDevice::new(input))),
        (2022, 8) => Some(Box::from(puzzle::solution::year2022_day08::TreetopTreeHouse::new(input))),
        (2022, 9) => Some(Box::from(puzzle::solution::year2022_day09::RopeBridge::new(input))),
        (2022, 10) => Some(Box::from(puzzle::solution::year2022_day10::CathodeRayTube::new(input))),
        (2022, 11) => Some(Box::from(puzzle::solution::year2022_day11::MonkeyInTheMiddle::new(input))),
        (2022, 12) => Some(Box::from(puzzle::solution::year2022_day12::HillClimbingAlgorithm::new(input))),
        (2022, 13) => Some(Box::from(puzzle::solution::year2022_day13::DistressSignal::new(input))),
        (2022, 14) => Some(Box::from(puzzle::solution::year2022_day14::RegolithReservoir::new(input))),
        (2022, 15) => Some(Box::from(puzzle::solution::year2022_day15::BeaconExclusionZone::new(input))),
        (2022, 16) => Some(Box::from(puzzle::solution::year2022_day16::ProboscideaVolcanium::new(input))),
        (2023, 1) => Some(Box::from(puzzle::solution::year2023_day01::Trebuchet::new(input))),
        (2024, 1) => Some(Box::from(puzzle::solution::year2024_day01::HistorianHysteria::new(input))),
        (2024, 2) => Some(Box::from(puzzle::solution::year2024_day02::RedNosedReports::new(input))),
        (2024, 3) => Some(Box::from(puzzle::solution::year2024_day03::MullItOver::new(input))),
        (2024, 4) => Some(Box::from(puzzle::solution::year2024_day04::CeresSearch::new(input))),
        _ => None,
    }
}
