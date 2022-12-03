use super::Solution;

pub struct RockPaperScissors<'a>
{
    input : &'a str
}

#[derive(Debug, PartialEq, Clone, Copy)]
enum Move {
    Rock, Paper, Scissor
}

#[derive(Debug, PartialEq)]
enum GameResult {
    Win, Loss, Tie
}

impl<'a> RockPaperScissors<'a> {
    pub(crate) fn new(input: &'a str) -> Self {
        RockPaperScissors { input }
    }
}

impl Solution for RockPaperScissors<'_> {
    fn solve_part1(&self) -> String {
        let strategy = parse_strategy(self.input);
        strategy
            .iter()
            .map(score_game)
            .sum::<i32>()
            .to_string()
    }

    fn solve_part2(&self) -> String {
        let strategy = parse_strategy_part2(self.input);
        strategy
            .iter()
            .map(fix_game)
            .map(|g| { score_game(&g) })
            .sum::<i32>()
            .to_string()
    }
}

fn parse_strategy(input: &str) -> Vec<(Move, Move)> {
    input.split("\n")
        .filter_map(|x| {
            let split : Vec<&str> = x
                .trim()
                .split(" ")
                .collect();
            if let Some(left) = map_move(split[0]) {
                if let Some(right) = map_move(split[1]) {
                    return Some((left, right))
                }
            }
            None
        })
        .collect()
}

fn parse_strategy_part2(input: &str) -> Vec<(Move, GameResult)>
{
    // Reuse parser from part1 but update to new understanding
    parse_strategy(input)
        .iter()
        .map(|x| {
            (x.0, match x.1 {
                Move::Rock => GameResult::Loss,
                Move::Paper => GameResult::Tie,
                Move::Scissor => GameResult::Win,
            })
        })
        .collect()
}

fn map_move(input: &str) -> Option<Move> {
    match input {
        "X" | "A" => Some(Move::Rock),
        "Y" | "B" => Some(Move::Paper),
        "Z" | "C" => Some(Move::Scissor),
        _ => None
    }
}

fn score_game((left, right): &(Move, Move)) -> i32 {
    let shape_score = match right {
        Move::Rock => 1,
        Move::Paper => 2,
        Move::Scissor => 3,
    };
    let game_score = match determine_outcome((left, right)) {
        GameResult::Win => 6,
        GameResult::Tie => 3,
        GameResult::Loss => 0,
    };
    shape_score + game_score
}

fn fix_game((left, right): &(Move, GameResult)) -> (Move, Move)
{
    match (left, right) {
        (Move::Rock, GameResult::Win)       => (*left, Move::Paper),
        (Move::Rock, GameResult::Loss)      => (*left, Move::Scissor),
        (Move::Rock, GameResult::Tie)       => (*left, Move::Rock),
        (Move::Paper, GameResult::Win)      => (*left, Move::Scissor),
        (Move::Paper, GameResult::Loss)     => (*left, Move::Rock),
        (Move::Paper, GameResult::Tie)      => (*left, Move::Paper),
        (Move::Scissor, GameResult::Win)    => (*left, Move::Rock),
        (Move::Scissor, GameResult::Loss)   => (*left, Move::Paper),
        (Move::Scissor, GameResult::Tie)    => (*left, Move::Scissor),
    }
}

fn determine_outcome((left, right): (&Move, &Move)) -> GameResult {
    match (left, right) {
        (Move::Rock, Move::Rock) => GameResult::Tie,
        (Move::Rock, Move::Paper) => GameResult::Win,
        (Move::Rock, Move::Scissor) => GameResult::Loss,
        (Move::Paper, Move::Rock) => GameResult::Loss,
        (Move::Paper, Move::Paper) => GameResult::Tie,
        (Move::Paper, Move::Scissor) => GameResult::Win,
        (Move::Scissor, Move::Rock) => GameResult::Win,
        (Move::Scissor, Move::Paper) => GameResult::Loss,
        (Move::Scissor, Move::Scissor) => GameResult::Tie,
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn parse_input_test() {
        let input = r"
                            A Y
                            B X
                            C Z
        ";

        let expected = vec!(
            (Move::Rock, Move::Paper),
            (Move::Paper, Move::Rock),
            (Move::Scissor, Move::Scissor)
        );

        let actual = parse_strategy(input);
        assert_eq!(expected[..], actual[..]);
    }

    #[test]
    fn scoring_test() {
        assert_eq!(8, score_game(&(Move::Rock, Move::Paper)));
        assert_eq!(1, score_game(&(Move::Paper, Move::Rock)));
        assert_eq!(6, score_game(&(Move::Scissor, Move::Scissor)));
    }

    #[test]
    fn part1_test() {
        let input = r"
                            A Y
                            B X
                            C Z
        ";
        let solver = RockPaperScissors::new(input);
        assert_eq!("15", solver.solve_part1())
    }

    #[test]
    fn part2_test() {
        let input = r"
                            A Y
                            B X
                            C Z
        ";
        let solver = RockPaperScissors::new(input);
        assert_eq!("12", solver.solve_part2())
    }
}
