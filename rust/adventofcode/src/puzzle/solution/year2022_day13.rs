use std::{rc::Rc, cell::RefCell};

use super::Solution;


pub struct DistressSignal {
    packets: Vec<PacketRef>
}

impl DistressSignal {
    pub fn new(input: &str) -> Self { 
        Self { packets: parse_input(input) } 
    }
}

impl Solution for DistressSignal {
    fn solve_part1(&self) -> String {
        let answer = self.packets
            .chunks(2)
            .enumerate()
            .filter(|(_, n)| {
                is_packets_in_right_order(
                    Rc::clone(&n[0]), 
                    Rc::clone(&n[1]))
                    .unwrap()
            })
            .map(|m| {
                m.0 + 1 // Pairs are 1 indexed
            })
            .reduce(| a,b | {
                a + b
            });
            
            
        format!("{}", answer.unwrap())
    }

    fn solve_part2(&self) -> String {
        todo!()
    }
}

#[derive(Debug, PartialEq)]
enum Packet {
    Integer(i32),
    List(PacketList),
}

impl Packet {
    fn new_int(int: i32) -> PacketRef {
        Rc::new(RefCell::new(Packet::Integer(int)))
    }

    fn to_list(&mut self) -> Option<&mut PacketList> {
        match self {
            Packet::Integer(_) => None,
            Packet::List(p) => Some(p),
        }
    }
}

#[derive(Debug, PartialEq)]
struct PacketList {
    subpackets: Vec<PacketRef>
}

impl PacketList {
    fn add_new_subpacket(&mut self, packet: &PacketRef) {
        let s = &mut self.subpackets;
        s.push(
            Rc::clone(packet)
        )
    }

    fn new(list: Vec<PacketRef> ) -> PacketRef {
        Rc::new(RefCell::new(Packet::List(Self { subpackets: list })))
    }
}

type PacketRef = Rc<RefCell<Packet>>;

fn parse_input(input: &str) -> Vec<PacketRef> {
    let tokenized_lines = input
        .split("\n")
        .filter_map(|line| {
            let trimmed = line.trim();
            if trimmed.len() > 0 {
                Some(trimmed)
            }
            else {
                None
            }
        })
        .map(|line| {
            let mut tokens = Vec::new();
            let mut current = String::new();
            for c in line.chars() {
                match c {
                    '[' => {
                        if current.len() > 0 {
                            tokens.push(current);
                            current = String::new();
                        }
                        tokens.push("[".to_string());
                    }
                    ']' => {
                        if current.len() > 0 {
                            tokens.push(current);
                            current = String::new();
                        }
                        tokens.push("]".to_string())
                    }
                    ',' => {
                        if current.len() > 0 {
                            tokens.push(current);
                            current = String::new();
                        }
                    }
                    n => current.push(n),
                }
            }
            tokens
        });

    let packet_list = tokenized_lines.
        map(|line| {
            let packet = PacketList::new(vec![]);
            let mut packet_stack = vec![packet];
            for t in line {
                match t.as_str() {
                    "[" => {
                        let new = PacketList::new(vec![]);
                        let current = Rc::clone(packet_stack.last().unwrap());
                        let mut temp = current.borrow_mut();
                        let list = temp.to_list().unwrap();
                        list.add_new_subpacket(&new);
                        packet_stack.push(Rc::clone(&new));

                    },
                    "]" => {
                        packet_stack.pop();
                    }
                    m => {
                        let num :PacketRef = Packet::new_int(m.parse().unwrap());
                        let current = Rc::clone(packet_stack.last().unwrap());
                        let mut temp = current.borrow_mut();
                        let list = temp.to_list().unwrap();
                        list.add_new_subpacket(&num);
                    }
                }
            }


            assert_eq!(1, packet_stack.len());
            // Whole packet is wrapped in a root packet, to avoid special handling of the root node
            // We now need to unwrap it
            let mut top = packet_stack.first().unwrap().borrow_mut();
            let temp = top.to_list().unwrap();
            assert_eq!(1, temp.subpackets.len());
            Rc::clone(&temp.subpackets[0])
        })
        .collect();

    packet_list
}

fn is_packets_in_right_order(left: PacketRef, right: PacketRef) -> Option<bool> {
    let left_borrow = &*left.borrow();
    let right_borrow = &*right.borrow();
    match (left_borrow, right_borrow) {
        (Packet::Integer(l), Packet::Integer(r)) => {
            if l < r {
                return Some(true);
            }
            else if l > r {
                return Some(false);
            }
            else {
                return None
            }
        },
        (Packet::List(l), Packet::List(r)) => {
            let mut left_iter = l.subpackets.iter();
            let mut right_iter = r.subpackets.iter();

            loop {
                match (left_iter.next(), right_iter.next()) {
                    (None, None) => return None,
                    (None, Some(_)) => return Some(true),
                    (Some(_), None) => return Some(false),
                    (Some(nested_l), Some(nested_r)) => {
                        let l_clone = Rc::clone(nested_l);
                        let r_clone = Rc::clone(nested_r);
                        if let Some(result) = is_packets_in_right_order(l_clone, r_clone) {
                            return Some(result);
                        }
                    },
                }
            }
        },
        (Packet::Integer(_), Packet::List(_)) => {
            let wrapped_in_list = PacketList::new(vec![Rc::clone(&left)]);
            return is_packets_in_right_order(wrapped_in_list, Rc::clone(&right));
        },
        (Packet::List(_), Packet::Integer(_)) => {
            let wrapped_in_list = PacketList::new(vec![Rc::clone(&right)]);
            return is_packets_in_right_order(Rc::clone(&left), wrapped_in_list);
        },
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn comparison_test1() {
        let left = Rc::clone(&parse_input("[1,1,3,1,1]")[0]);
        let right = Rc::clone(&parse_input("[1,1,5,1,1]")[0]);

        assert_eq!(Some(true), is_packets_in_right_order(left, right));
    }

    #[test]
    fn comparison_test2() {
        let left = Rc::clone(&parse_input("[[1],[2,3,4]]")[0]);
        let right = Rc::clone(&parse_input("[[1],4]")[0]);

        assert_eq!(Some(true), is_packets_in_right_order(left, right));
    }

    #[test]
    fn comparison_test3() {
        let left = Rc::clone(&parse_input("[9]")[0]);
        let right = Rc::clone(&parse_input("[[8,7,6]]")[0]);

        assert_eq!(Some(false), is_packets_in_right_order(left, right));
    }

    #[test]
    fn comparison_test4() {
        let left = Rc::clone(&parse_input("[[4,4],4,4]")[0]);
        let right = Rc::clone(&parse_input("[[4,4],4,4,4]")[0]);

        assert_eq!(Some(true), is_packets_in_right_order(left, right));
    }

    #[test]
    fn comparison_test5() {
        let left = Rc::clone(&parse_input("[7,7,7,7]")[0]);
        let right = Rc::clone(&parse_input("[7,7,7]")[0]);

        assert_eq!(Some(false), is_packets_in_right_order(left, right));
    }

    #[test]
    fn comparison_test6() {
        let left = Rc::clone(&parse_input("[]")[0]);
        let right = Rc::clone(&parse_input("[3]")[0]);

        assert_eq!(Some(true), is_packets_in_right_order(left, right));
    }

    #[test]
    fn comparison_test7() {
        let left = Rc::clone(&parse_input("[[[]]]")[0]);
        let right = Rc::clone(&parse_input("[[]]")[0]);

        assert_eq!(Some(false), is_packets_in_right_order(left, right));
    }

    #[test]
    fn comparison_test8() {
        let left = Rc::clone(&parse_input("[1,[2,[3,[4,[5,6,7]]]],8,9]")[0]);
        let right = Rc::clone(&parse_input("[1,[2,[3,[4,[5,6,0]]]],8,9]")[0]);

        assert_eq!(Some(false), is_packets_in_right_order(left, right));
    }

    #[test]
    fn part1_test() {
        let solver = DistressSignal::new(EXAMPLE);
        assert_eq!("13", solver.solve_part1());
    }

    static EXAMPLE: &str = "[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]
";

    #[test]
    fn parse_test() {
        let expected = vec![
            PacketList::new(vec![Packet::new_int(1), Packet::new_int(1), Packet::new_int(3), Packet::new_int(1), Packet::new_int(1)]),
            PacketList::new(vec![Packet::new_int(1), Packet::new_int(1), Packet::new_int(5), Packet::new_int(1), Packet::new_int(1)]),
            PacketList::new(vec![
                PacketList::new(vec![
                    Packet::new_int(1)]), 
                PacketList::new(vec![
                    Packet::new_int(2), Packet::new_int(3), Packet::new_int(4) ])]),
            PacketList::new(vec![
                PacketList::new(vec![Packet::new_int(1)]), 
                Packet::new_int(4) ]),
            PacketList::new(vec![Packet::new_int(9)]),
            PacketList::new(vec![
                PacketList::new( vec![Packet::new_int(8), Packet::new_int(7), Packet::new_int(6)])
            ]),
            PacketList::new(vec![
                PacketList::new(vec![
                    Packet::new_int(4), Packet::new_int(4) 
                ]),
                Packet::new_int(4),
                Packet::new_int(4),
            ]),
            PacketList::new(vec![
                PacketList::new(vec![
                    Packet::new_int(4), Packet::new_int(4) 
                ]),
                Packet::new_int(4),
                Packet::new_int(4),
                Packet::new_int(4),
            ]),
            PacketList::new(vec![
                Packet::new_int(7), Packet::new_int(7), Packet::new_int(7), Packet::new_int(7),
            ]),
            PacketList::new(vec![
                Packet::new_int(7), Packet::new_int(7), Packet::new_int(7),
            ]),
            PacketList::new(vec![]),
            PacketList::new(vec![Packet::new_int(3)]),
            PacketList::new(vec![PacketList::
                new(vec![
                    PacketList::new(vec![]),
                ]),
            ]),
            PacketList::new(vec![
                PacketList::new(vec![]),
            ]),
            PacketList::new(vec![
                Packet::new_int(1),
                PacketList::new(vec![
                    Packet::new_int(2),
                    PacketList::new(vec![
                        Packet::new_int(3),
                        PacketList::new(vec![
                            Packet::new_int(4),
                            PacketList::new(vec![
                                Packet::new_int(5), Packet::new_int(6), Packet::new_int(7),
                            ]),
                        ]),
                    ]),
                ]),
                Packet::new_int(8), Packet::new_int(9),
            ]),
            PacketList::new(vec![
                Packet::new_int(1),
                PacketList::new(vec![
                    Packet::new_int(2),
                    PacketList::new(vec![
                        Packet::new_int(3),
                        PacketList::new(vec![
                            Packet::new_int(4),
                            PacketList::new(vec![
                                Packet::new_int(5), Packet::new_int(6), Packet::new_int(0),
                            ]),
                        ]),
                    ]),
                ]),
                Packet::new_int(8), Packet::new_int(9),
            ]),
        ];
        let actual = parse_input(EXAMPLE);
        assert_eq!(expected, actual);
    }
}