use regex::Regex;

use super::Solution;


pub struct PrintQueue<'a> {
    input: &'a str
}

type Rule = (usize, usize);

struct  ManualUpdates {
    pages: Vec<usize>
}

impl<'a> PrintQueue<'a> {
    pub fn new(input : &'a str) -> Self {
        Self { input }
    }

    fn parse_input(&self) -> Option<(Vec<Rule>, Vec<ManualUpdates>)> {
        let mut parsing_rules = true;
        let rule_exp = Regex::new(r"(\d+)|(\d+)").unwrap();
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

                let Some(captures) = rule_exp.captures(line) else { return  None; };
                let (_, [before, after]) = captures.extract();
                let Some(before) = before.parse::<usize>().ok() else { return None; };
                let Some(after) = after.parse::<usize>().ok() else { return None; };
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

        return Some((rules, updates));
    }
}

impl Solution for PrintQueue<'_> {
    fn solve_part1(&self) -> String {
        todo!()
    }

    fn solve_part2(&self) -> String {
        todo!()
    }
}


#[cfg(test)]
mod tests {
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
        assert_eq!(5, updates.len());
        assert_eq!(97, rules[3].0);
        assert_eq!(97, updates[4].pages[0]);
    }
}