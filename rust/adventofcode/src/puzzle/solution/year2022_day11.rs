use super::Solution;
use std::{sync::mpsc, collections::VecDeque, cell::RefCell};

pub struct MonkeyInTheMiddle {

}

impl<'a> MonkeyInTheMiddle {
    pub fn new(input: &'a str) -> Self { Self {  } }
}

struct Monkey<'a> {
    items: VecDeque<i64>,
    operation: Box<dyn Fn(i64) -> i64 + 'a>,
    test_divisor: i64,
    test_true_dest: usize,
    test_false_dest: usize,
    throw_to: mpsc::Sender<i64>,
    receiver: mpsc::Receiver<i64>,
    num_items_handled: usize
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
            num_items_handled: 0
        }
    }

    fn catch_items(&mut self)
    {
        while let Some(a) = self.receiver.try_recv().ok() {
            self.items.push_back(a)
        }
    }

    fn throw_item_to(&self, monkeys: &Vec<RefCell<Monkey>>, monkey_number: usize, item: i64) {
        assert!(monkey_number < monkeys.len());

        let dest_monkey = &monkeys[monkey_number].borrow_mut();
        dest_monkey.throw_to.send(item).expect("Failed to throw item to mokey");
    }

    fn inspect_item(&mut self, item: i64, monkeys: &Vec<RefCell<Monkey>>)
    {
        self.num_items_handled += 1;
        let worry_level = (self.operation)(item);
        let worry_level = worry_level / 3;
        if worry_level % self.test_divisor == 0 {
            self.throw_item_to(monkeys, self.test_true_dest, worry_level)
        }
        else {
            self.throw_item_to(monkeys, self.test_false_dest, worry_level)
        }
    }

    fn do_turn(&mut self, monkeys: &Vec<RefCell<Monkey>>) {
        self.catch_items();
        while let Some(i) = self.items.pop_front() {
            self.inspect_item(i, monkeys);
        }
    }
}

fn do_round(monkeys: &Vec<RefCell<Monkey<'_>>>) {
    for i in 0 .. monkeys.len() {
        let monkey = &monkeys[i];
        monkey.borrow_mut().do_turn(monkeys)
    }
}

impl Solution for MonkeyInTheMiddle {
    fn solve_part1(&self) -> String {
        todo!()
    }

    fn solve_part2(&self) -> String {
        todo!()
    }
}

#[cfg(test)]
mod tests {
    use std::sync::mpsc::{Receiver, Sender};

    use super::*;

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

        for _ in 0 ..20 {
            do_round(&monkeys);
        }

        monkeys.sort_by(|a, b | {
            b.borrow().num_items_handled.cmp(&a.borrow().num_items_handled)
        });
        
        let sum = monkeys[0].borrow().num_items_handled * monkeys[1].borrow().num_items_handled;

        assert_eq!(10605, sum);
    }
}