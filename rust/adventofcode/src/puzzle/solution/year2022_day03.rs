use std::collections::{HashSet, hash_map::RandomState};

use super::Solution;

pub struct RucksackReorganization<'a> {
    input: &'a str
}

struct Rucksack {
    left: HashSet<char, RandomState>,
    right: HashSet<char, RandomState>,
}

impl<'a> Rucksack {
    fn new(line : &str) -> Option<Self>{
        let line = line.trim();
        if line.len() % 2 == 0 {
            let midpoint = line.len() / 2;
            Some(Self { 
                left: HashSet::from_iter(line[0..midpoint].chars()), 
                right: HashSet::from_iter(line[midpoint..].chars()),
            })
        }
        else {
            None
        }
    }

    fn duplicate_item(&'a self) -> Option<&'a char> {
        self.left.intersection(&self.right).nth(0)
    }

    fn score_duplicate_item(item : &char) -> Option<u32> {
        match item {
            'a'..='z' => Some((*item as u32) - ('a' as u32) + 1),
            'A'..='Z' => Some((*item as u32) - ('A' as u32) + 27),
            _ => None,
        }
    }

    fn find_badge(&self, other: &Rucksack, and_another: &Rucksack) -> Option<char> {
        let badge = &(&(&self.left | &self.right) & &(&other.left | &other.right)) & &(&and_another.left | &and_another.right);
        
        badge.into_iter().nth(0)
    }
}

impl<'a> RucksackReorganization<'a> {
    pub fn new(input: &'a str) -> Self {
        RucksackReorganization { input }
    }
}

impl Solution for RucksackReorganization<'_> {
    fn solve_part1(&self) -> String {
        self.input
            .split("\n")
            .filter_map(Rucksack::new)
            .filter_map(|r| {
                if let Some(item) = r.duplicate_item() {
                    Rucksack::score_duplicate_item(item)
                } else {
                    None
                }
            })
            .sum::<u32>()
            .to_string()
    }

    fn solve_part2(&self) -> String {
        let rucksacks : Vec<Rucksack> = self.input
            .split("\n")
            .filter_map(|l| {
                let trimmed = l.trim();
                if trimmed.is_empty() {
                    return None;
                }
                Some(trimmed)
             })
            .filter_map(Rucksack::new)
            .collect::<Vec<Rucksack>>();

        let groups: Vec<&[Rucksack]> = 
            rucksacks.chunks_exact(3)
            .collect();

        groups
            .into_iter()
            .filter_map(|w| {
                w[0].find_badge(&w[1], &w[2])
            })
            .filter_map(|b| {
                Rucksack::score_duplicate_item(&b)
            })
            .sum::<u32>()
            .to_string()
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_test() {
        let input = r"
        vJrwpWtwJgWrhcsFMMfFFhFp
        jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
        PmmdzqPrVvPwwTWBwg
        wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
        ttgJtRGJQctTZtZT
        CrZsJsPPZsGzwwsLwLmpwMDw";

        assert_eq!("157", RucksackReorganization::new(input).solve_part1())
    }

    #[test]
    fn duplicate_item_test() {
        let rucksack = Rucksack::new("vJrwpWtwJgWrhcsFMMfFFhFp").unwrap();
        assert_eq!(&'p', rucksack.duplicate_item().unwrap())
    }

    #[test]
    fn score_item_test() {
        let r = Rucksack::new("acab").unwrap();
        let item = r.duplicate_item();
        assert_eq!(1u32, Rucksack::score_duplicate_item(item.unwrap()).unwrap());

        let r = Rucksack::new("zczb").unwrap();
        let item = r.duplicate_item();
        assert_eq!(26u32, Rucksack::score_duplicate_item(item.unwrap()).unwrap());

        let r = Rucksack::new("AcAb").unwrap();
        let item = r.duplicate_item();
        assert_eq!(27u32, Rucksack::score_duplicate_item(item.unwrap()).unwrap());

        let r = Rucksack::new("ZcZb").unwrap();
        let item = r.duplicate_item();
        assert_eq!(52u32, Rucksack::score_duplicate_item(item.unwrap()).unwrap());

        let r = Rucksack::new("vJrwpWtwJgWrhcsFMMfFFhFp").unwrap();
        let item = r.duplicate_item();
        assert_eq!(16u32, Rucksack::score_duplicate_item(item.unwrap()).unwrap())
    }

    #[test]
    fn part2_test() {
        let input = r#"
        vJrwpWtwJgWrhcsFMMfFFhFp
        jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
        PmmdzqPrVvPwwTWBwg
        wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
        ttgJtRGJQctTZtZT
        CrZsJsPPZsGzwwsLwLmpwMDw"#;

        assert_eq!("70", RucksackReorganization::new(input).solve_part2())
    }

    #[test]
    fn group_score_test() {
        let input = vec![
            Rucksack::new("vJrwpWtwJgWrhcsFMMfFFhFp").unwrap(),
            Rucksack::new("jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL").unwrap(),
            Rucksack::new("PmmdzqPrVvPwwTWBwg").unwrap(),
            ];


        assert_eq!('r', input[0].find_badge(&input[1], &input[2]).unwrap());
        assert_eq!(18u32, Rucksack::score_duplicate_item(&'r').unwrap());

        let input = vec![
            Rucksack::new("wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn").unwrap(),
            Rucksack::new("ttgJtRGJQctTZtZT").unwrap(),
            Rucksack::new("CrZsJsPPZsGzwwsLwLmpwMDw").unwrap(),
            ];


        assert_eq!('Z', input[0].find_badge(&input[1], &input[2]).unwrap());
        assert_eq!(52u32, Rucksack::score_duplicate_item(&'Z').unwrap());
    }
}