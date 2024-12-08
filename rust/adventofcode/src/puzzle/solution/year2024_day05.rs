use std::collections::{HashMap, HashSet};

use super::Solution;


pub struct PrintQueue<'a> {
    input: &'a str
}

type Rule = (usize, usize);

struct  ManualUpdates {
    pages: Vec<usize>
}

impl  ManualUpdates {
    fn is_in_right_order(&self, rules: &HashMap<usize, Vec<usize>>) -> bool {
        // Since both page numbers of the rule has to be present in the manual update for the rule to apply,
        // it is enough to check the first number
        let mut past_pages = HashSet::new();
        for page in &self.pages {
            let is_rule_broken = rules.get(page)
                .unwrap_or(&vec![])
                .iter()
                .any(|after| past_pages.contains(after));
            if is_rule_broken {
                return false;
            }

            past_pages.insert(page);
        }

        true
    }
    
    fn get_middle_page(&self) -> usize {
        self.pages[self.pages.len() / 2]
    }

}

impl<'a> PrintQueue<'a> {
    pub fn new(input : &'a str) -> Self {
        Self { input }
    }

    fn parse_input(&self) -> Option<(Vec<Rule>, Vec<ManualUpdates>)> {
        let mut parsing_rules = true;
        
        let mut rules = vec![];
        let mut updates = vec![];
        for line in self.input.split("\n")
            .map(|l| l.trim())
            .skip_while(|l| l.is_empty()) {
            if parsing_rules {
                if line.is_empty() {
                    parsing_rules = false;
                    continue;
                }
                
                let mut split = line.split("|");
                let before = split.next()?.parse::<usize>().ok()?;
                let after = split.next()?.parse::<usize>().ok()?;
                rules.push((before, after));
            }
            else {
                if line.is_empty() {
                    continue;
                }

                let numbers: Vec<_> = line.split(",").filter_map(|n| n.parse::<usize>().ok()).collect();
                updates.push(ManualUpdates { pages: numbers });
            }
        }

        Some((rules, updates))
    }

    fn create_rule_map(rule_list: &Vec<Rule>) -> HashMap<usize, Vec<usize>> {
        let mut rule_map: HashMap<usize, Vec<usize>> = HashMap::new();
        for (before, after) in rule_list {
            rule_map.entry(*before).or_default().push(*after);
        }

        rule_map
    }
}

impl Solution for PrintQueue<'_> {
    fn solve_part1(&self) -> String {
        let (rules, updates) = self.parse_input().expect("Failed to parse input");
        let rule_map = PrintQueue::create_rule_map(&rules);
        updates.iter()
            .filter(|u| u.is_in_right_order(&rule_map))
            .map(|r| r.get_middle_page())
            .sum::<usize>()
            .to_string()
    }

    fn solve_part2(&self) -> String {
        todo!()
    }
}


#[cfg(test)]
mod tests {
    use crate::puzzle::solution::Solution;

    use super::PrintQueue;

    const EXAMPLE : &str = "47|53
                            97|13
                            97|61
                            97|47
                            75|29
                            61|13
                            75|53
                            29|13
                            97|29
                            53|29
                            61|53
                            97|53
                            61|29
                            47|13
                            75|47
                            97|75
                            47|61
                            75|61
                            47|29
                            75|13
                            53|13

                            75,47,61,53,29
                            97,61,53,29,13
                            75,29,13
                            75,97,47,61,53
                            61,13,29
                            97,13,75,29,47
                            ";

    #[test]
    fn parse_test() {
        let solver = PrintQueue::new(EXAMPLE);
        let Some((rules, updates)) = solver.parse_input() else { panic!("Failed to parse!")};
        assert_eq!(21, rules.len());
        assert_eq!(6, updates.len());
        assert_eq!(97, rules[3].0);
        assert_eq!(97, updates[5].pages[0]);
    }

    #[test]
    fn valid_update_test() {
        let solver = PrintQueue::new(EXAMPLE);
        let Some((rules, updates)) = solver.parse_input() else { panic!("Failed to parse ")};
        let rules = PrintQueue::create_rule_map(&rules);
        assert!(updates[0].is_in_right_order(&rules));
        assert!(updates[1].is_in_right_order(&rules));
        assert!(updates[2].is_in_right_order(&rules));
        assert!(!updates[3].is_in_right_order(&rules));
        assert!(!updates[4].is_in_right_order(&rules));
        assert!(!updates[5].is_in_right_order(&rules));
    }

    #[test]
    fn part1_test() {
        let solver = PrintQueue::new(EXAMPLE);
        assert_eq!("143", solver.solve_part1());
    }
}