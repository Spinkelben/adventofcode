use futures::executor::block_on;

fn main() {
    let a = adventofcode::parse_args();
    println!("Running solution for Year {} Day {}", a.year, a.day);
    let input = block_on(adventofcode::get_input(a.year, a.day, a.reload_input));
    println!("Input: {}", input);

}
