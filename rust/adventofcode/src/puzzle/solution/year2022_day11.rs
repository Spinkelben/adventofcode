use regex::Regex;

use super::Solution;
use std::{sync::mpsc, collections::VecDeque, cell::{RefCell}};

pub struct MonkeyInTheMiddle<'a> {
    input: &'a str
}

impl<'a> MonkeyInTheMiddle<'a> {
    pub fn new(input: &'a str) -> Self { 
        Self { input } 
    }
}


struct Monkey<'a> {
    items: VecDeque<i64>,
    operation: Box<dyn Fn(i64) -> i64 + 'a>,
    test_divisor: i64,
    test_true_dest: usize,
    test_false_dest: usize,
    throw_to: mpsc::Sender<i64>,
    receiver: mpsc::Receiver<i64>,
    num_items_handled: usize,
    stress_limit: i64,
}

impl std::fmt::Debug for Monkey<'_> {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        f.debug_struct("Monkey")
        .field("items", &self.items)
        .field("test_divisor", &self.test_divisor)
        .field("test_true_dest", &self.test_true_dest)
        .field("test_false_dest", &self.test_false_dest)
        .field("throw_to", &self.throw_to)
        .field("receiver", &self.receiver)
        .field("num_items_handled", &self.num_items_handled)
        .finish()
    }
}

impl PartialEq for Monkey<'_> {
    fn eq(&self, other: &Self) -> bool {
        self.items == other.items 
        && self.test_divisor == other.test_divisor 
        && self.test_true_dest == other.test_true_dest 
        && self.test_false_dest == other.test_false_dest 
        && self.num_items_handled == other.num_items_handled
    }
}

impl Monkey<'_> {
    fn new(
        items: Vec<i64>, 
        operation: Box<dyn Fn(i64) -> i64>, 
        test_divisor: i64,
        test_true_dest: usize,
        test_false_dest: usize
    ) -> Self {
        let (catcher, receiver) = mpsc::channel::<i64>();
        Self { 
            items: VecDeque::from( items ), 
            operation, 
            test_divisor, 
            test_false_dest, 
            test_true_dest, 
            throw_to: catcher, 
            receiver,
            num_items_handled: 0,
            stress_limit: 1,
        }
    }

    fn catch_items(&mut self)
    {
        while let Ok(a) = self.receiver.try_recv() {
            self.items.push_back(a)
        }
    }

    fn throw_item_to(&self, monkeys: &Vec<RefCell<Monkey>>, monkey_number: usize, item: i64) {
        assert!(monkey_number < monkeys.len());

        let dest_monkey = &monkeys[monkey_number].borrow_mut();
        dest_monkey.throw_to.send(item).expect("Failed to throw item to mokey");
    }

    fn inspect_item(&mut self, item: i64, monkeys: &Vec<RefCell<Monkey>>, worry_level_decrease: bool)
    {
        self.num_items_handled += 1;
        let mut worry_level = (self.operation)(item);
        if worry_level_decrease {
            worry_level /= 3;
        }

        // Stress limit is the LCM of all the test divisors. 
        // We don't need to know the actual stress level,
        // we just need to know if the stress level is dividable by certain numbers
        worry_level %= self.stress_limit;

        if worry_level % self.test_divisor == 0 {
            self.throw_item_to(monkeys, self.test_true_dest, worry_level)
        }
        else {
            self.throw_item_to(monkeys, self.test_false_dest, worry_level)
        }
    }

    fn do_turn(&mut self, monkeys: &Vec<RefCell<Monkey>>, worry_level_decrease: bool) {
        self.catch_items();
        while let Some(i) = self.items.pop_front() {
            self.inspect_item(i, monkeys, worry_level_decrease);
        }
    }
}

fn parse_operation(op: &str) -> Option<Box<dyn Fn(i64) -> i64>> {
    let re = regex::Regex::new(r"Operation: new = old (.) (\d+|old)")
        .unwrap();

    if let Some(m) = re.captures(op.trim()) {
        let op = m.get(1).map(|v| { v.as_str() });
        let value = m.get(2).map(|v| { v.as_str() });
        match (op, value) {
            (Some("*"), Some("old")) => Some(Box::new(|x: i64| { x * x } )),
            (Some("+"), Some("old")) => Some(Box::new(|x: i64| { x + x } )),
            (Some("*"), Some(s)) => {
                if let Ok(n) = s.parse::<i64>() {
                    Some(Box::new(move |x| { x * n })) 
                }
                else {
                    None
                }
            },
            (Some("+"), Some(s)) => {
                if let Ok(n) = s.parse::<i64>() {
                    Some(Box::new(move |x| { x + n })) 
                }
                else {
                    None
                }
            },
            _ => None,
        }
    }
    else {
        None
    }
}

fn do_round(monkeys: &Vec<RefCell<Monkey<'_>>>, worry_level_decrease: bool) {
    for i in 0 .. monkeys.len() {
        let monkey = &monkeys[i];
        monkey.borrow_mut().do_turn(monkeys, worry_level_decrease)
    }
}

fn parse_monkeys(input: &str) -> Vec<RefCell<Monkey>> {
    fn lcm(a: i64, b: i64) -> i64 {
        let mut x;
        let mut y;
        if a > b {
            x = a;
            y = b;
        }
        else {
            x = b;
            y = a;
        }
    
        let mut rem = x % y;
    
        while rem != 0 {
            x = y;
            y = rem;
            rem = x % y;
        }
    
        a * b / y
    }

    let mut result = Vec::new();
    let lines : Vec<&str> = input.split("\n").collect();
    let mut stress_limit = 1;
    for chunk in lines.chunks(7) {
        let items = parse_starting_items(chunk[1]);
        let operation = parse_operation(chunk[2]).unwrap();
        let test_divisor = parse_test_divisor(chunk[3]);
        let true_monkey = parse_monkey_dest(chunk[4]);
        let false_monkey = parse_monkey_dest(chunk[5]);
        stress_limit = lcm(stress_limit, test_divisor);
        result.push(RefCell::new(
            Monkey::new(
                items,
                operation,
                test_divisor,
                true_monkey,
                false_monkey)))
    }

    for m in &result {
        m.borrow_mut().stress_limit = stress_limit;
    }

    result
}

fn parse_monkey_dest(chunk: &str) -> usize {
    let re = Regex::new(r".*throw to monkey (\d+)").unwrap();
    re.captures(chunk.trim())
        .unwrap()
        .get(1)
        .unwrap()
        .as_str()
        .parse()
        .unwrap()
}

fn parse_test_divisor(chunk: &str) -> i64 {
    let re = Regex::new(r"Test: divisible by (\d+)").unwrap();
    re.captures(chunk.trim()).unwrap()
        .get(1).unwrap()
        .as_str()
        .parse().unwrap()
}

fn parse_starting_items(items_str: &str) -> Vec<i64> {
    let re = Regex::new(r"Starting items: (.*)").unwrap();
    let m = re.captures(items_str).unwrap();
    m.get(1)
        .unwrap()
        .as_str()
        .split(",")
        .filter_map(|v| {
            v.trim().parse::<i64>().ok()
        })
        .collect()

}

impl Solution for MonkeyInTheMiddle<'_> {
    fn solve_part1(&self) -> String {
        let monkeys = parse_monkeys(self.input);

        for _ in 0..20 {
            do_round(&monkeys, true);
        }

        two_most_active_multiplied(monkeys)
    }

    fn solve_part2(&self) -> String {
        let monkeys = parse_monkeys(self.input);

        for _ in 0..10000 {
            do_round(&monkeys, false);
        }

        two_most_active_multiplied(monkeys)
    }
}

fn two_most_active_multiplied(mut monkeys: Vec<RefCell<Monkey>>) -> String {
    monkeys.sort_by(|a, b| { b.borrow().num_items_handled.cmp(&a.borrow().num_items_handled) });
    let answer = monkeys[0].borrow().num_items_handled * monkeys[1].borrow().num_items_handled;
    answer.to_string()
}

#[cfg(test)]
mod tests {
    use super::*;

    static EXAMPLE : &str = "Monkey 0:
Starting items: 79, 98
Operation: new = old * 19
Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
Starting items: 54, 65, 75, 74
Operation: new = old + 6
Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
Starting items: 79, 60, 97
Operation: new = old * old
Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
Starting items: 74
Operation: new = old + 3
Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1
";

    #[test]
    fn manual_mokey_construction_test() {
        let mut monkeys = vec![
            RefCell::new(Monkey::new(
                vec![79, 98], 
                Box::new(|x| {x * 19}), 
                23, 
                2, 
                3
            )),
            RefCell::new(Monkey::new(
                vec![54, 65, 75, 74], 
                Box::new(|x| {x + 6}), 
                19, 
                2, 
                0
            )),
            RefCell::new(Monkey::new(
                vec![79, 60, 97], 
                Box::new(|x| {x * x}), 
                13, 
                1, 
                3
            )),
            RefCell::new(Monkey::new(
                vec![74], 
                Box::new(|x| {x + 3}), 
                17, 
                0, 
                1
            )),
        ];

        for m in &monkeys {
            m.borrow_mut().stress_limit = 96577 // LCM of all the stress levels
        }

        for _ in 0 ..20 {
            do_round(&monkeys, true);
        }

        monkeys.sort_by(|a, b | {
            b.borrow().num_items_handled.cmp(&a.borrow().num_items_handled)
        });
        
        let sum = monkeys[0].borrow().num_items_handled * monkeys[1].borrow().num_items_handled;

        assert_eq!(10605, sum);
    }

    #[test]
    fn parse_monkey_test() {
        let parsed_monkey = parse_monkeys(EXAMPLE);
        let expected = vec![
            RefCell::new(Monkey::new(
                vec![79, 98], 
                Box::new(|x| {x * 19}), 
                23, 
                2, 
                3,
            )),
            RefCell::new(Monkey::new(
                vec![54, 65, 75, 74], 
                Box::new(|x| {x + 6}), 
                19, 
                2, 
                0
            )),
            RefCell::new(Monkey::new(
                vec![79, 60, 97], 
                Box::new(|x| {x * x}), 
                13, 
                1, 
                3
            )),
            RefCell::new(Monkey::new(
                vec![74], 
                Box::new(|x| {x + 3}), 
                17, 
                0, 
                1
            )),
        ];
        assert_eq!(expected, parsed_monkey);
    }

    #[test]
    fn part1_test() {
        let solver = MonkeyInTheMiddle::new(EXAMPLE);
        assert_eq!("10605", solver.solve_part1());
    }

    #[test]
    fn part2_test() {
        let solver = MonkeyInTheMiddle::new(EXAMPLE);
        assert_eq!("2713310158", solver.solve_part2());
    }
}