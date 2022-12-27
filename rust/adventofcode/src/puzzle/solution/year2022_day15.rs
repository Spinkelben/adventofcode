use std::{str::FromStr, collections::HashSet};

use regex::Regex;

use super::Solution;

pub struct BeaconExclusionZone {
    sensors: Vec<Sensor>,
    beacons: HashSet<(i32, i32)>,
}

impl BeaconExclusionZone {
    pub fn new(input: &str) -> Self {
        let sensors: Vec<Sensor> = input
            .split("\n")
            .filter_map(|s| { s.parse().ok() })
            .collect();

        let beacons = HashSet::from_iter(
            sensors.iter().map(|b| { b.closest_beacon.clone() }));
        Self { sensors, beacons }
    }

    fn find_overlap(&self, y: i32) -> Vec<(i32, i32)> {
        let mut overlaps: Vec<(i32, i32)> = self.sensors
            .iter()
            .filter_map(|s| {
                s.positions_covered(y)
            })
            .collect();
        overlaps.sort_by(|a,b| { a.0.cmp(&b.0) });

        let mut iter = overlaps.iter();
        let mut current = iter.next().unwrap().clone();
        let mut result = vec![];
        while let Some(next) = iter.next() {
            if current.1 >= next.0 {
                if next.1 > current.1 {
                    current.1 = next.1
                }
            }
            else {
                result.push(current);
                current = *next
            }
        }
        result.push(current);

        result
    }

    fn sum_overlaps(&self, y: i32) -> i32 {
        let binding = self.find_overlap(y);
        let overlaps = binding
            .iter()
            .map(|(start, end)| {
                (end - start) + 1
            });
        let overlaps_sum : i32 = overlaps
            .sum();
        let num_beacons_in_row = self.beacons.iter()
            .filter(|b| {
                binding.iter()
                    .any(|(start, end)| {
                        b.0 >= *start && b.0 <= *end
                    })
                && b.1 == y
             })
            .count();
        overlaps_sum - num_beacons_in_row as i32
    }

    fn find_beacon(&self, max: i32) -> Option<(i32, i32)> {
        for y in 0..=max {
            let overlaps = self.find_overlap(y);
            for (start, end) in overlaps {
                if start > 0 && start <= max {
                    return Some((start - 1, y));
                }
                else if end < max && end >= 0 {
                    return Some((end + 1, y));
                }
            }
        }

        None
    }
}

#[derive(Debug)]
struct Sensor {
    position: (i32, i32),
    closest_beacon: (i32, i32),
}

impl Sensor {
    fn positions_covered(&self, y: i32) -> Option<(i32, i32)> {
        let dist_x = i32::abs(self.closest_beacon.0 - self.position.0);
        let dist_y = i32::abs(self.closest_beacon.1 - self.position.1);
        let manhattan_dist = dist_x + dist_y;

        let y_dist_from_sensor = i32::abs(y - self.position.1);

        let overlap = manhattan_dist - y_dist_from_sensor;

        if overlap >= 0 {
            let start = self.position.0 - overlap;
            let end = self.position.0 + overlap;
            Some((start, end))
        }
        else {
            None
        }
    }

    fn _num_positions_covered(&self, y: i32) -> i32 {
        if let Some((start, end)) = self.positions_covered(y) {
            let cover = end - start + 1;
            if y == self.closest_beacon.1 {
                cover - 1
            }
            else {
                cover
            }
        }
        else {
            0
        }
    }
}

impl FromStr for Sensor {
    type Err = &'static str;

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let re = Regex::new(r"x=(-?\d+), y=(-?\d+)").unwrap();
        let captures = re
            .captures_iter(s);

        let digits : Vec<i32> = captures
            .map(|c| {
                c.iter()
                    .filter_map(|s| {
                        s.and_then(|c| { c.as_str().parse::<i32>().ok() })
                    })
                    .collect::<Vec<i32>>()
                })
            .flatten()
            .collect();

        if digits.len() != 4 {
            return Err("Expected two sets of x y coordinated")
        }

        Ok(Sensor { position: (digits[0], digits[1]), closest_beacon: (digits[2], digits[3]) })
    }
}

impl Solution for BeaconExclusionZone {
    fn solve_part1(&self) -> String {
        self.sum_overlaps(2000000).to_string()
    }

    fn solve_part2(&self) -> String {
        let (x, y) = self.find_beacon(4000000).unwrap();
        (x as i64 * 4000000i64 + y as i64).to_string()
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    static EXAMPLE: &str = r"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";

    #[test]
    fn parse_line() {
        let line = "Sensor at x=2, y=18: closest beacon is at x=-2, y=15";
        let sensor: Sensor = line.parse().ok().unwrap();
        assert_eq!((2, 18), sensor.position);
        assert_eq!((-2, 15), sensor.closest_beacon);
    }

    #[test]
    fn parse_list() {
        let solver = BeaconExclusionZone::new(EXAMPLE);
        assert_eq!(14, solver.sensors.len());
    }

    #[test]
    fn dist_test() {
        let sensor = Sensor::from_str("Sensor at x=8, y=7: closest beacon is at x=2, y=10").unwrap();
        assert_eq!(11, sensor._num_positions_covered(11));
        assert_eq!(15, sensor._num_positions_covered(9));
        assert_eq!(1, sensor._num_positions_covered(16));
        assert_eq!(12, sensor._num_positions_covered(10));
    }

    #[test]
    fn part1_test() {
        let solver = BeaconExclusionZone::new(EXAMPLE);
        assert_eq!(26, solver.sum_overlaps(10));
    }

    #[test]
    fn overlap_test() {
        let solver = BeaconExclusionZone::new(EXAMPLE);
        assert_eq!(26, solver.sum_overlaps(10));
    }

    #[test]
    fn find_beacon_test() {
        let solver = BeaconExclusionZone::new(EXAMPLE);
        assert_eq!(Some((14, 11)), solver.find_beacon(20));
    }
}