use std::collections::{ HashSet, VecDeque, hash_map::RandomState};

use super::Solution;


pub struct TuningTrouble<'a> {
    signal: &'a str
}
impl<'a> TuningTrouble<'a> {
    pub fn new(input: &'a str) -> Self {
        TuningTrouble { signal: input }
    }
}

impl Solution for TuningTrouble<'_> {
    fn solve_part1(&self) -> String {
        if let Some(idx) = find_repeat(self.signal, 4) {
            return idx.to_string()
        }
        "None".to_string()
    }

    fn solve_part2(&self) -> String {
        if let Some(idx) = find_repeat(self.signal, 14) {
            return idx.to_string()
        }
        "None".to_string()
    }
}

fn find_repeat(signal: &str, num_unique: usize) -> Option<usize> {
    let mut sample: VecDeque<char> = VecDeque::new();

    for (idx, char) in signal.chars().enumerate() {
        if idx < num_unique {
            sample.push_back(char);
        }
        else {
            let set: HashSet<&char, RandomState> = HashSet::from_iter(&sample);
            if set.len() == num_unique {
                return Some(idx);
            }
            else {
                sample.pop_front();
                sample.push_back(char);
            }
        }
    };

    None
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_test()
    {
        assert_eq!("7", TuningTrouble::new("mjqjpqmgbljsphdztnvjfqwrcgsmlb").solve_part1());
        assert_eq!("5", TuningTrouble::new("bvwbjplbgvbhsrlpgdmjqwftvncz").solve_part1());
        assert_eq!("6", TuningTrouble::new("nppdvjthqldpwncqszvftbrmjlhg").solve_part1());
        assert_eq!("10", TuningTrouble::new("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg").solve_part1());
        assert_eq!("11", TuningTrouble::new("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw").solve_part1());
    }

    #[test]
    fn part2_test()
    {
        assert_eq!("19", TuningTrouble::new("mjqjpqmgbljsphdztnvjfqwrcgsmlb").solve_part2());
        assert_eq!("23", TuningTrouble::new("bvwbjplbgvbhsrlpgdmjqwftvncz").solve_part2());
        assert_eq!("23", TuningTrouble::new("nppdvjthqldpwncqszvftbrmjlhg").solve_part2());
        assert_eq!("29", TuningTrouble::new("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg").solve_part2());
        assert_eq!("26", TuningTrouble::new("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw").solve_part2());
    }
}
