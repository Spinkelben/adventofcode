use std::{str::FromStr, collections::{HashSet, BinaryHeap, HashMap}};

use regex::Regex;

use super::Solution;


pub struct ProboscideaVolcanium {
    scan: Scan,
}

impl ProboscideaVolcanium {
    pub fn new(input: &str) -> Self 
    { 
        Self {  
            scan: Scan::new(input, 30)
        } 
    }
}

#[derive(PartialEq, Debug)]
struct Valve {
    name: String,
    flow_rate: i32,
    tunnels: HashSet<String>,
}

struct Scan {
    valves: HashMap<String, Valve>,
    total_time: i32,
}

impl Scan {
    fn new(input: &str, total_time: i32) -> Self {
        let valves = input.split("\n")
            .filter_map(|l| {
                l.parse::<Valve>().ok()
            })
            .map(|v| {
                (v.name.clone(), v)
            })
            .collect();
        Self { valves: valves, total_time }
    }

    fn closed_valves<'a>(&'a self, open_valves: &'a HashSet<&str>) -> impl Iterator<Item= &Valve> +'a {
        self.valves.iter()
            .filter(|(_,v)| { !open_valves.contains(v.name.as_str()) })
            .map(|(_, v)| { v })
    }

    fn potential_pressure_release(&self, state: &SearchState) -> i32 {
        let unopened_flow : i32 = self.closed_valves(&state.open_valves)
            .map(|v| { v.flow_rate })
            .sum();
        
        unopened_flow * (self.total_time - state.time_step)
    }

    fn find_valve(&self, name :  &str) -> &Valve {
        self.valves.get(name).unwrap()
    }

    fn find_optimal_order(&self) -> i32 {
        let mut start = SearchState {
            current_pos : "AA",
            open_valves : HashSet::new(),
            potential_release : 0,
            pressure_release : 0,
            time_step : 1,
        };
        start.potential_release = self.potential_pressure_release(&start);

        let mut heap = BinaryHeap::new();
        heap.push(start);
        let mut best = 0;
        
        while let Some(current) = heap.pop() {
            if current.pressure_release > best {
                best = current.pressure_release;
            }

            // Don't continue pursuing paths that have no chance of beating the best
            if current.score() < best {
                continue;
            }

            // No more time!
            if current.time_step >= self.total_time {
                break;
            }

            // All valves open
            // if current.open_valves.len() == self.valves.len() {
            //     continue;
            // }

            let valve = self.find_valve(current.current_pos);
            // Add open valve move
            if !current.open_valves.contains(current.current_pos) {
                heap.push(current.open_valve(valve, self.total_time));
            
            }

            // Add traverse tunnel move
            for n in current.traverse_tunnel(valve, self.total_time) {
                heap.push(n);
            }
        }

        best
    }
}

#[derive(PartialEq, Eq, Debug)]
struct SearchState<'a> {
    current_pos: &'a str,
    open_valves: HashSet<&'a str>,
    time_step: i32,
    pressure_release: i32,
    potential_release: i32,
}

impl<'a> Ord for SearchState<'a> {
    fn cmp(&self, other: &Self) -> std::cmp::Ordering {
        let self_score = self.score();
        let other_score = other.score();
        self_score.cmp(&other_score)
    }
}

impl<'a> PartialOrd for SearchState<'a> {
    fn partial_cmp(&self, other: &Self) -> Option<std::cmp::Ordering> {
        Some(self.cmp(other))
    }
}

impl<'a> SearchState<'a> {
    fn score(&self) -> i32 {
        self.potential_release + self.pressure_release
    }

    fn open_valve(&self, valve: &Valve, total_time: i32) -> Self {
        let mut open_valves = self.open_valves.clone();
        open_valves.insert(self.current_pos);
        let steps_left = total_time - self.time_step;
        let release = valve.flow_rate * (steps_left);
        // Remove realized potential
        let potential_release = self.potential_release - release;
        // Reduce potential by one time step
        let new_potential = Self::reduce_potential(self.time_step, potential_release, total_time);

        SearchState {
            current_pos : self.current_pos,
            open_valves : open_valves,
            potential_release : new_potential,
            pressure_release : self.pressure_release + release,
            time_step : self.time_step + 1
        }
    }

    fn reduce_potential(time_step : i32, current_potential : i32, total_time: i32) -> i32 {
        let steps_left = total_time - time_step;
        let release_per_step = current_potential / steps_left;
        release_per_step * (steps_left - 1)
    }

    fn traverse_tunnel(&self, valve : &'a Valve, total_time: i32) -> Vec<SearchState<'a>> {
        let mut result = vec![];
        let new_potential = Self::reduce_potential(self.time_step, self.potential_release, total_time);
        for t in &valve.tunnels {
            let open_valves = self.open_valves.clone();
            result.push(SearchState {
                current_pos : t,
                open_valves: open_valves,
                potential_release : new_potential,
                pressure_release : self.pressure_release,
                time_step : self.time_step + 1
            })
        }

        return result;
    }
}

impl FromStr for Valve {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let pattern = Regex::new(r"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? ([\w, ]*)").unwrap();
        if let Some(matches) = pattern.captures_iter(s).next() {
            let matches: Vec<&str> = matches.iter()
                .map(|c| { c.unwrap().as_str() })
                .collect();
            let tunnels = matches[3].split(",")
                .map(|t| {  t.trim().to_owned() })
                .collect();
    
            return Ok(Valve { 
                name: matches[1].to_string(), 
                flow_rate: matches[2].parse().unwrap(), 
                tunnels: tunnels })
        }

        Err(())
    }
}

impl Solution for ProboscideaVolcanium {
    fn solve_part1(&self) -> String {
        let result = self.scan.find_optimal_order();
        result.to_string()
    }

    fn solve_part2(&self) -> String {
        "todo!()".to_string()
    }
}

#[cfg(test)]
mod tests {
    use std::time::Instant;

    use super::*;

    static EXAMPLE: &str = r"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II
";

    #[test]
    fn parse_valve_test() {
        let text = "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB";
        let expected = Valve {
            flow_rate : 0,
            name : "AA".to_string(),
            tunnels : HashSet::from_iter(vec![
                "DD".to_string(),
                "II".to_string(),
                "BB".to_string(),
                ]),
        };
        let actual = text.parse().unwrap();
        assert_eq!(expected, actual);
    }
    #[test]
    fn parse_valve_test2() {
        let text = "Valve HH has flow rate=22; tunnel leads to valve GG";
        let expected = Valve {
            flow_rate : 22,
            name : "HH".to_string(),
            tunnels : HashSet::from_iter(vec![
                "GG".to_string(),
                ]),
        };
        let actual = text.parse().unwrap();
        assert_eq!(expected, actual);
    }

    #[test]
    fn parse_whole_test() {
        let scan = Scan::new(EXAMPLE, 30);
        assert_eq!(10, scan.valves.len());
    }

    #[test]
    fn traverse_tunnel_test() {
        let scan = Scan::new(EXAMPLE, 30);
        let mut init = SearchState {
            current_pos : "AA",
            open_valves : HashSet::new(),
            potential_release : 0,
            pressure_release : 0,
            time_step : 1,
        };
        init.potential_release = scan.potential_pressure_release(&init);
        let valve = scan.find_valve(init.current_pos);

        let states = init.traverse_tunnel(valve, scan.total_time);
        let dd = states.iter().find(|s| { s.current_pos == "DD" }).unwrap();
        let expected = SearchState {
            current_pos: "DD",
            open_valves: HashSet::new(),
            potential_release: (13 + 2 + 20 + 3 + 22 + 21) * 28,
            pressure_release: 0,
            time_step: 2,
        };
        assert_eq!(&expected ,dd);
    }

    #[test]
    fn open_valve_test() {
        let scan = Scan::new(EXAMPLE, 30);
        let mut init = SearchState {
            current_pos : "BB",
            open_valves : HashSet::new(),
            potential_release : 0,
            pressure_release : 0,
            time_step : 2,
        };
        init.potential_release = scan.potential_pressure_release(&init);
        let valve = scan.find_valve(init.current_pos);
        let state = init.open_valve(valve, scan.total_time);
        let expected = SearchState {
            current_pos : "BB",
            open_valves : HashSet::from_iter(vec!["BB"]),
            potential_release : (2 + 20 + 3 + 22 + 21) * 27,
            pressure_release : 364,
            time_step : 3,
        };
        assert_eq!(expected, state);
    }

    #[test]
    fn part1_test() {
        let solver = ProboscideaVolcanium::new(EXAMPLE);
        let start = Instant::now();
        for _ in 0..10 {
            assert_eq!("1651", solver.solve_part1());
        }
        println!("Total {:?}, Avg {:?}", start.elapsed(), start.elapsed() / 10);
    }

    #[test]
    fn open_value_better_than_walking_test() {
        let scan = Scan::new(EXAMPLE, 30);
        let mut init = SearchState {
            current_pos : "DD",
            open_valves : HashSet::new(),
            potential_release : 0,
            pressure_release : 0,
            time_step : 2,
        };
        init.potential_release = scan.potential_pressure_release(&init);
        let valve = scan.find_valve(init.current_pos);
        let open = init.open_valve(valve, scan.total_time);
        let tunnel = &init.traverse_tunnel(valve, scan.total_time)[0];
        assert_eq!(28*20, open.pressure_release);
        assert_eq!(0, tunnel.pressure_release);
        assert!(open.score() > tunnel.score())
    }

    #[test]
    fn potential_decrease_test() {
        let scan = Scan::new(EXAMPLE, 30);
        let mut state = SearchState {
            current_pos : "AA",
            open_valves : HashSet::new(),
            potential_release : 0,
            pressure_release : 0,
            time_step : 1,
        };
        state.potential_release = scan.potential_pressure_release(&state);
        let mut potentials = vec![];
        while state.time_step < 30 {
            let valve = scan.find_valve(state.current_pos);
            potentials.push(state.potential_release);
            for n in state.traverse_tunnel(valve, scan.total_time) {
                state = n;
                break;
            }
        }

        assert_eq!(potentials[..6], vec![
            (13 + 2 + 20 + 3 + 22 + 21) * 29,
            (13 + 2 + 20 + 3 + 22 + 21) * 28,
            (13 + 2 + 20 + 3 + 22 + 21) * 27,
            (13 + 2 + 20 + 3 + 22 + 21) * 26,
            (13 + 2 + 20 + 3 + 22 + 21) * 25,
            (13 + 2 + 20 + 3 + 22 + 21) * 24,
        ]);

        
    }
}
