use super::Solution;

pub struct CathodeRayTube {
    instructions: Vec<Instruction>,
}

impl CathodeRayTube {
    pub fn new(input: &str) -> Self {
        let program = input
            .split("\n")
            .filter_map(|line| {
                let mut split = line
                    .trim().split(" ");
                let instruction = split.next();
                let arg = split.next().and_then(|n| { n.parse::<i32>().ok() });
                match (instruction, arg) {
                    (Some("noop"), _) => Some(Instruction::Nop),
                    (Some("addx"), Some(n)) => Some(Instruction::Add(n)),
                    _ => None
                }
            })
            .collect();
         Self { instructions: program } 
    }

    fn get_program_states(&self) -> Vec<(i32, i32)> {
        let mut result = vec![];
        let mut x = 1;
        let mut cycle = 0;
        for cmd in &self.instructions {
            match cmd {
                Instruction::Nop =>  {
                    cycle += 1;
                    result.push((cycle, x));
                },
                Instruction::Add(n) => {
                    cycle += 1;
                    result.push((cycle, x));
                    cycle += 1;
                    result.push((cycle, x));
                    x += n;
                },
            }
        }

        result
    }
}

impl Solution for CathodeRayTube {
    fn solve_part1(&self) -> String {
        let states = self.get_program_states();
        states.iter().filter_map(|(cycle, x)| {
            if cycle % 40 == 20 && cycle <= &220 {
                Some(cycle * x)
            }
            else {
                None
            }
        })
        .sum::<i32>()
        .to_string()
    }

    fn solve_part2(&self) -> String {
        let states = self.get_program_states();
        let screen = states.chunks_exact(40)
            .map(|chunk| {
                chunk.iter()
                    .map(|(c, x)| {
                        let pos = (c - 1) % 40;
                        if pos >= x - 1 && pos <= x + 1 {
                            '▓'
                        }
                        else {
                            '░'
                        }
                    })
                    .collect::<String>()
            })
            .collect::<Vec<String>>()
            .join("\n");

        format!("\n{}", screen)
    }
}

#[derive(PartialEq, Debug)]
enum Instruction {
    Nop,
    Add(i32),
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn parse_test() {
        let solver = CathodeRayTube::new(EXAMPLE);
        assert_eq!(Instruction::Add(15), solver.instructions[0]);
        assert_eq!(Instruction::Nop, solver.instructions[solver.instructions.len() - 1])
    }

    #[test]
    fn part1_test() {
        let solver = CathodeRayTube::new(EXAMPLE);
        assert_eq!("13140", solver.solve_part1())
    }

    #[test]
    fn part2_test() {
        let expected = 
          r"
▓▓░░▓▓░░▓▓░░▓▓░░▓▓░░▓▓░░▓▓░░▓▓░░▓▓░░▓▓░░
▓▓▓░░░▓▓▓░░░▓▓▓░░░▓▓▓░░░▓▓▓░░░▓▓▓░░░▓▓▓░
▓▓▓▓░░░░▓▓▓▓░░░░▓▓▓▓░░░░▓▓▓▓░░░░▓▓▓▓░░░░
▓▓▓▓▓░░░░░▓▓▓▓▓░░░░░▓▓▓▓▓░░░░░▓▓▓▓▓░░░░░
▓▓▓▓▓▓░░░░░░▓▓▓▓▓▓░░░░░░▓▓▓▓▓▓░░░░░░▓▓▓▓
▓▓▓▓▓▓▓░░░░░░░▓▓▓▓▓▓▓░░░░░░░▓▓▓▓▓▓▓░░░░░";
        let solver = CathodeRayTube::new(EXAMPLE);
        assert_eq!(expected, solver.solve_part2());
    }

    #[test]
    fn small_program_test() {
        let program = "noop
            addx 3
            addx -5";
        let solver =CathodeRayTube::new(program);
        let expected = vec![
            (1, 1),
            (2, 1),
            (3, 1),
            (4, 4),
            (5, 4),
        ];
        assert_eq!(expected, solver.get_program_states())
    }

    static EXAMPLE: &str = "addx 15
    addx -11
    addx 6
    addx -3
    addx 5
    addx -1
    addx -8
    addx 13
    addx 4
    noop
    addx -1
    addx 5
    addx -1
    addx 5
    addx -1
    addx 5
    addx -1
    addx 5
    addx -1
    addx -35
    addx 1
    addx 24
    addx -19
    addx 1
    addx 16
    addx -11
    noop
    noop
    addx 21
    addx -15
    noop
    noop
    addx -3
    addx 9
    addx 1
    addx -3
    addx 8
    addx 1
    addx 5
    noop
    noop
    noop
    noop
    noop
    addx -36
    noop
    addx 1
    addx 7
    noop
    noop
    noop
    addx 2
    addx 6
    noop
    noop
    noop
    noop
    noop
    addx 1
    noop
    noop
    addx 7
    addx 1
    noop
    addx -13
    addx 13
    addx 7
    noop
    addx 1
    addx -33
    noop
    noop
    noop
    addx 2
    noop
    noop
    noop
    addx 8
    noop
    addx -1
    addx 2
    addx 1
    noop
    addx 17
    addx -9
    addx 1
    addx 1
    addx -3
    addx 11
    noop
    noop
    addx 1
    noop
    addx 1
    noop
    noop
    addx -13
    addx -19
    addx 1
    addx 3
    addx 26
    addx -30
    addx 12
    addx -1
    addx 3
    addx 1
    noop
    noop
    noop
    addx -9
    addx 18
    addx 1
    addx 2
    noop
    noop
    addx 9
    noop
    noop
    noop
    addx -1
    addx 2
    addx -37
    addx 1
    addx 3
    noop
    addx 15
    addx -21
    addx 22
    addx -6
    addx 1
    noop
    addx 2
    addx 1
    noop
    addx -10
    noop
    noop
    addx 20
    addx 1
    addx 2
    addx 2
    addx -6
    addx -11
    noop
    noop
    noop
";


}