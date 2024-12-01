use std::{str::FromStr, collections::HashMap};

use regex::Regex;

use super::Solution;

pub struct RegolithReservoir {
    scan: RockScan
}

impl RegolithReservoir {
    pub fn new(input: &str) -> Self { Self { scan: input.parse().ok().unwrap() } }
}

impl Solution for RegolithReservoir {
    fn solve_part1(&self) -> String {
        let mut simulation = SandSimulation::new(&self.scan);
        simulation.fill_with_sand((500,0));
        simulation.num_grains_of_sand().to_string()
    }

    fn solve_part2(&self) -> String {
        let mut simulation = SandSimulation::new(&self.scan);
        let max_y = simulation.max_y;
        // Just code the floor width to be wide enough
        for x in -100..1100 {
            simulation.tiles.insert((x, max_y + 2), Material::Rock);
        }
        simulation.update_max_y();
        simulation.fill_with_sand((500,0));
        simulation.num_grains_of_sand().to_string()
    }
}

#[derive(PartialEq, Debug)]
struct RockScan {
    rock_formation: Vec<Vec<(i32, i32)>>
}

#[derive(PartialEq, Eq)]
enum Material {
    Sand,
    Rock,
}

struct SandSimulation {
    tiles:  HashMap<(i32, i32), Material>,
    max_y: i32,
}

impl SandSimulation {
    fn new(scan: &RockScan) -> Self {
        let mut tiles = HashMap::new();
        for line in scan.rock_formation.iter() {
            if line.len() == 1 {
                tiles.insert(line[0], Material::Rock);
                continue;
            }

            for coord in line.windows(2) {
                let dx = i32::signum(coord[1].0 - coord[0].0);
                let dy = i32::signum(coord[1].1 - coord[0].1);
                let mut current = coord[0];
                while current != coord[1] {
                    tiles.insert(current, Material::Rock);
                    current = (current.0 + dx, current.1 + dy);
                }
                tiles.insert(current, Material::Rock);
            }
        }

        let mut temp = Self { tiles, max_y: 0 };
        temp.update_max_y();
        temp
    }

    fn update_max_y(&mut self) {
        self.max_y = self.tiles.keys().map(|n| { n.1 }).max().unwrap();
    }

    fn deposit_grain(&mut self, init: (i32, i32)) -> bool {
        let mut coord = init;
        loop {
            if coord.1 > self.max_y {
                return true;
            }

            let below = self.tiles.get(&(coord.0, coord.1 + 1));
            let below_left = self.tiles.get(&(coord.0 - 1, coord.1 + 1));
            let below_right = self.tiles.get(&(coord.0 + 1, coord.1 + 1));
            match (below, below_left, below_right) {
                (None, _, _) => coord = (coord.0, coord.1 + 1),
                (Some(_), None, _) => coord = (coord.0 - 1, coord.1 + 1),
                (Some(_), Some(_), None) => coord = (coord.0 + 1, coord.1 + 1),
                (Some(_), Some(_), Some(_)) => {
                    self.tiles.insert(coord, Material::Sand);
                    return coord == init
                }
            }
        }
    }

    fn fill_with_sand(&mut self, init: (i32, i32)) {
        loop {
            if self.deposit_grain(init) {
                break;
            }
        }
    }

    fn num_grains_of_sand(&self) -> usize {
        self.tiles.values().filter(|v| { v == &&Material::Sand }).count()
    }
}

impl FromStr for RockScan {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let re = Regex::new(r"(\d+),(\d+)").unwrap();
        let result : Vec<Vec<(i32, i32)>> = s.lines()
            .map(|l| {
                re.captures_iter(l)
                .map(|s| {
                    (s.get(1).unwrap().as_str().parse::<i32>().ok().unwrap(),
                     s.get(2).unwrap().as_str().parse::<i32>().ok().unwrap())
                }).collect::<Vec<(i32, i32)>>()
            })
            .collect();
        Ok(RockScan { rock_formation: result })
    }
}

#[cfg(test)]
mod tests {
    use super::*;


    static EXAMPLE: &str = "498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9";

    #[test]
    fn parse_test() {
        let expected = vec![
            vec![(498, 4), (498,6), (496,6)],
            vec![(503, 4), (502, 4), (502,9), (494,9)],
        ];
        let actual: Result<RockScan, ()> = EXAMPLE.parse();
        assert_eq!(actual.ok().unwrap(), RockScan { rock_formation: expected })

    }

    #[test]
    fn tile_test() {
        let scan = EXAMPLE.parse().ok().unwrap();
        let simulation = SandSimulation::new(&scan);

        assert_eq!(20, simulation.tiles.len());
        assert!(simulation.tiles.contains_key(&(497, 6)));
        assert!(simulation.tiles.contains_key(&(502, 6)));
    }

    #[test]
    fn part1_test() {
        let solver = RegolithReservoir::new(EXAMPLE);
        assert_eq!("24", solver.solve_part1());
    }

    #[test]
    fn part2_test() {
        let solver = RegolithReservoir::new(EXAMPLE);
        assert_eq!("93", solver.solve_part2());
    }
}