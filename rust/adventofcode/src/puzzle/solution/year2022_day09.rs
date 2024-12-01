use std::{collections::HashSet, iter};

use super::Solution;

pub struct RopeBridge {
    moves: Vec<Move>,
}

struct RopeSolver<'a> {
    tail_positions: HashSet<(i32, i32)>,
    links: Vec<(i32, i32)>,
    moves: &'a Vec<Move>,
}

impl<'a> RopeSolver<'a> {
    fn new(moves: &'a Vec<Move>, num_links: usize) -> Self {
        Self { 
            moves, 
            tail_positions: HashSet::from([(0,0)]),
            links: iter::repeat((0,0)).take(num_links).collect(),
        } 
    }

    fn execute_moves(&mut self) {

        let expanded_moves: Vec<Move> = self.moves
            .iter()
            .flat_map(|m| {
                let c = match m {
                    Move::Up(c) => c,
                    Move::Down(c) => c,
                    Move::Left(c) => c,
                    Move::Right(c) => c,
                };
                
                iter::repeat(*m).take(*c as usize)
        })
        .collect();

        for m in expanded_moves {
            self.move_step(&m);
        }
    }

    fn move_step(&mut self, direction: &Move) {
        let mut new_tail = vec![];
        // Move head
        let head = self.links.first().unwrap();
        new_tail.push(match direction {
            Move::Up(_) => (head.0, head.1 - 1),
            Move::Down(_) => (head.0, head.1 + 1),
            Move::Left(_) => (head.0 - 1, head.1),
            Move::Right(_) => (head.0 + 1, head.1)
        });
        
        for t in self.links.iter().skip(1) {
            let head = new_tail.last().unwrap();
            let new_pos = Self::move_link(head, t);
            new_tail.push(new_pos);
        } 

        self.tail_positions.insert(*new_tail.last().unwrap());
        self.links = new_tail;
    }

    fn move_link(head: &(i32, i32), tail: &(i32, i32)) -> (i32,i32) {
          // Move tail if nessecary
          let delta_x = head.0 - tail.0;
          let delta_y = head.1 - tail.1;
          let x_move = delta_x.signum();
          let y_move = delta_y.signum();
  
          if delta_y.abs() > 1 
              || delta_x.abs() > 1 {
              (tail.0 + x_move, tail.1 + y_move)
          }
          else {
              *tail
          }
    }

    fn num_moves(&self) -> usize {
        self.tail_positions.len()
    }
}

#[derive(Debug, PartialEq, Clone, Copy)]
enum Move {
    Up(i32),
    Down(i32),
    Left(i32),
    Right(i32),
}

impl RopeBridge {
    pub fn new(input: &str) -> Self {
        let moves = input
            .split("\n")
            .filter_map(|m| -> Option<Move> {
                let mut split = m.trim().split(" ");
                let direction = split.next();
                let count: Option<i32> = split.next().and_then(|c| { c.parse().ok() });
                match (direction, count) {
                    (Some("R"), Some(c)) => Some(Move::Right(c)),
                    (Some("L"), Some(c)) => Some(Move::Left(c)),
                    (Some("U"), Some(c)) => Some(Move::Up(c)),
                    (Some("D"), Some(c)) => Some(Move::Down(c)),
                    _ => None
                }
            })
            .collect();
        Self { moves }
    }
}

impl Solution for RopeBridge {
    fn solve_part1(&self) -> String {
        let mut solver = RopeSolver::new(&self.moves, 2);
        solver.execute_moves();
        solver.num_moves().to_string()
    }

    fn solve_part2(&self) -> String {
        let mut solver = RopeSolver::new(&self.moves, 10);
        solver.execute_moves();
        solver.num_moves().to_string()
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    static EXAMPLE: &str = "R 4
    U 4
    L 3
    D 1
    R 4
    D 1
    L 5
    R 2
    ";

    static EXAMPLE_BIG: &str = "R 5
    U 8
    L 8
    D 3
    R 17
    D 10
    L 25
    U 20
    ";

    #[test]
    fn parse_input_test() {
        let expected = vec![
            Move::Right(4),
            Move::Up(4),
            Move::Left(3),
            Move::Down(1),
            Move::Right(4),
            Move::Down(1),
            Move::Left(5),
            Move::Right(2),
        ];

        let solver = RopeBridge::new(EXAMPLE);
        assert_eq!(expected, solver.moves)
    }

    #[test]
    fn single_step_test() {
        let moves = vec![Move::Right(4)];
        let mut solver = RopeSolver::new(&moves, 2);
        assert_eq!((0,0), *solver.links.first().unwrap());
        assert_eq!((0,0), *solver.links.last().unwrap());
        solver.move_step(&Move::Right(4));
        assert_eq!((1,0), *solver.links.first().unwrap());
        assert_eq!((0,0), *solver.links.last().unwrap());
    }

    #[test]
    fn move_right_test() {
        let moves = vec![];
        let mut solver = RopeSolver::new(&moves, 2);
        solver.links[0].0 = 1;
        assert_eq!((1,0), *solver.links.first().unwrap());
        assert_eq!((0,0), *solver.links.last().unwrap());
        solver.move_step(&Move::Right(5));
        assert_eq!((2,0), *solver.links.first().unwrap());
        assert_eq!((1,0), *solver.links.last().unwrap());
    }

    #[test]
    fn move_left_test() {
        let moves = vec![];
        let mut solver = RopeSolver::new(&moves, 2);
        solver.links[0].0 = -1;
        assert_eq!((-1,0), *solver.links.first().unwrap());
        assert_eq!((0,0), *solver.links.last().unwrap());
        solver.move_step(&Move::Left(5));
        assert_eq!((-2,0), *solver.links.first().unwrap());
        assert_eq!((-1,0), *solver.links.last().unwrap());
    }

    #[test]
    fn move_up_test() {
        let moves = vec![];
        let mut solver = RopeSolver::new(&moves, 2);
        solver.links[0].1 = -1;
        assert_eq!((0,-1), *solver.links.first().unwrap());
        assert_eq!((0,0), *solver.links.last().unwrap());
        solver.move_step(&Move::Up(5));
        assert_eq!((0,-2), *solver.links.first().unwrap());
        assert_eq!((0,-1), *solver.links.last().unwrap());
    }

    #[test]
    fn move_down_test() {
        let moves = vec![];
        let mut solver = RopeSolver::new(&moves, 2);
        solver.links[0].1 = 1;
        assert_eq!((0,1), *solver.links.first().unwrap());
        assert_eq!((0,0), *solver.links.last().unwrap());
        solver.move_step(&Move::Down(5));
        assert_eq!((0,2), *solver.links.first().unwrap());
        assert_eq!((0,1), *solver.links.last().unwrap());
    }

    #[test]
    fn move_up_diagonal_test() {
        let moves = vec![];
        let mut solver = RopeSolver::new(&moves, 2);
        solver.links[0].1 = -1;
        solver.links[0].0 = 1;
        assert_eq!((1,-1), *solver.links.first().unwrap());
        assert_eq!((0,0), *solver.links.last().unwrap());
        solver.move_step(&Move::Up(5));
        assert_eq!((1,-2), *solver.links.first().unwrap());
        assert_eq!((1,-1), *solver.links.last().unwrap());
    }

    #[test]
    fn part1_test() {
        let solver = RopeBridge::new(EXAMPLE);
        assert_eq!("13", solver.solve_part1())
    }

    #[test]
    fn part2_test() {
        let solver = RopeBridge::new(EXAMPLE);
        assert_eq!("1", solver.solve_part2())
    }

    #[test]
    fn part2_big_test() {
        let solver = RopeBridge::new(EXAMPLE_BIG);
        assert_eq!("36", solver.solve_part2())
    }

}
