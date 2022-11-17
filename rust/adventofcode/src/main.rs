use std::time::Instant;

use adventofcode::find_sovler;
use futures::executor::block_on;

#[tokio::main]
async fn main() {
    let a = adventofcode::parse_args();
    println!("Running solution for Year {} Day {}", a.year, a.day);
    let input = block_on(adventofcode::get_input(a.year, a.day, a.reload_input));

    if let Some(solver) = find_sovler(a.year, a.day, &input) {
        let start = Instant::now();
        println!("Solution to part 1 {} in {:?}", solver.solve_part1(), start.elapsed());
        let start2 = Instant::now();
        println!("Solution to part 2 {}, in {:?}. Total time {:?}", solver.solve_part2(), start2.elapsed(), start.elapsed());
    } else {
        panic!("Can't find solver for year {} day {}", a.year, a.day);
    };
}
