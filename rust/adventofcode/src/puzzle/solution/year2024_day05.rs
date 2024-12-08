use std::collections::{HashMap, HashSet};

use super::Solution;


pub struct PrintQueue<'a> {
    input: &'a str
}

type Rule = (usize, usize);

#[derive(Debug)]
struct  ManualUpdates {
    pages: Vec<usize>
}

impl  ManualUpdates {
    fn find_broken_rules(&self, rules: &HashMap<usize, Vec<usize>>) -> Vec<Rule> {
        // Since both page numbers of the rule has to be present in the manual update for the rule to apply,
        // it is enough to check the first number
        let mut past_pages = HashSet::new();
        let mut result: Vec<(usize, usize)> = vec![];
        for page in &self.pages {
            let default = vec![];
            let broken_rules= rules.get(page)
                .unwrap_or(&default)
                .iter()
                .filter(|after| past_pages.contains(after))
                .map(|r| (*page, *r));
            
            result.extend(broken_rules);
            past_pages.insert(page);
        }

        result
    }

    fn fix_rule(&mut self, rule: Rule) {
        let a = self.pages.iter().position(|p| *p == rule.0).expect("Failed to find before part of rule");
        let b = self.pages.iter().position(|p| *p == rule.1).expect("Failed to find after part of rule");
        self.pages.swap(a, b);
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
            .filter(|u| u.find_broken_rules(&rule_map).is_empty())
            .map(|r| r.get_middle_page())
            .sum::<usize>()
            .to_string()
    }

    fn solve_part2(&self) -> String {
        let (rules, updates) = self.parse_input().expect("Failed to parse input");
        let rule_map = PrintQueue::create_rule_map(&rules);
        updates
            .into_iter()
            .filter(|u| !u.find_broken_rules(&rule_map).is_empty() )
            .map(|mut u| {
                let mut broken_rules = u.find_broken_rules(&rule_map);
                while !broken_rules.is_empty() {
                    for rule in broken_rules.iter() {
                        // println!("Before, Rule: {:#?}, Update {:#?}", rule, u);
                        u.fix_rule(*rule);
                        // println!("After, Rule: {:#?}, Update {:#?}", rule, u);
                    }

                    broken_rules = u.find_broken_rules(&rule_map);
                }
                    
                u.get_middle_page()
            })
            .sum::<usize>()
            .to_string()
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
        assert!(updates[0].find_broken_rules(&rules).is_empty());
        assert!(updates[1].find_broken_rules(&rules).is_empty());
        assert!(updates[2].find_broken_rules(&rules).is_empty());
        assert!(!updates[3].find_broken_rules(&rules).is_empty());
        assert!(!updates[4].find_broken_rules(&rules).is_empty());
        assert!(!updates[5].find_broken_rules(&rules).is_empty());
    }

    #[test]
    fn part1_test() {
        let solver = PrintQueue::new(EXAMPLE);
        assert_eq!("143", solver.solve_part1());
    }

    #[test]
    fn number_of_broken_rules_test()  {
        let solver = PrintQueue::new(EXAMPLE);
        let Some((rules, updates)) = solver.parse_input() else { panic!("Failed to parse input") };
        let rules = &PrintQueue::create_rule_map(&rules);
        assert_eq!(1, updates[3].find_broken_rules(rules).len());
        assert_eq!(1, updates[4].find_broken_rules(rules).len());
        println!("{:#?}", updates[5].find_broken_rules(rules));
        assert_eq!(4, updates[5].find_broken_rules(rules).len());
    }

    #[test]
    fn part2_test() {
        let solver = PrintQueue::new(EXAMPLE);
        assert_eq!("123", solver.solve_part2());
    }
}