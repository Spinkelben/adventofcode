use std::collections::{BinaryHeap, HashMap};

use super::Solution;

pub struct HillClimbingAlgorithm {
    map: Vec<Vec<char>>,
    start: (usize, usize),
    destination: (usize, usize),
}

impl<'a> HillClimbingAlgorithm {
    pub fn new(input: &'a str) -> Self { 
        let (map, start, end) = parse_input(input);
        Self { map, start, destination: end } }
}

impl Solution for HillClimbingAlgorithm {
    fn solve_part1(&self) -> String {
        let steps = find_path_length_astar(&self.map, self.start, self.destination);
        steps.unwrap().to_string()
    }

    fn solve_part2(&self) -> String {
        let mut shortest = usize::MAX;
        for y in 0..self.map.len() {
            for x in 0..self.map[0].len() {
                if self.map[y][x] == 'a' {
                    if let Some(steps) = find_path_length_astar(&self.map, (x,y), self.destination) {
                        shortest = shortest.min(steps);
                    }
                }
            }
        }

        shortest.to_string()
    }
}

#[derive(Eq, Debug)]
struct OpenSetPoint  {
    x: usize,
    y: usize,
    f_score: usize,
}

impl PartialOrd for OpenSetPoint {
    // Only order by fscore because that is what matters for nodes in open set
    // Also order so smallest fscore comes first when placed in BinaryHeap
    fn partial_cmp(&self, other: &Self) -> Option<std::cmp::Ordering> {
        other.f_score.partial_cmp(&self.f_score)
    }
}

impl Ord for OpenSetPoint {
    fn cmp(&self, other: &Self) -> std::cmp::Ordering {
        // Only order by fscore because that is what matters for nodes in open set
        // Also order so smallest fscore comes first when placed in BinaryHeap
        other.f_score.cmp(&self.f_score)
    }
}

impl PartialEq for OpenSetPoint {
    fn eq(&self, other: &Self) -> bool {
        self.x == other.x && self.y == other.y && self.f_score == other.f_score
    }
}

fn parse_input(example: &str) -> (Vec<Vec<char>>, (usize, usize), (usize, usize)) {
    let mut map: Vec<Vec<char>> = example.split("\n")
        .filter_map(|line| {
            let trimmed = line.trim();
            if trimmed.len() > 0 {
                Some(trimmed.chars().collect())
            }
            else {
                None
            }
        })
        .collect::<Vec<Vec<char>>>();
    let (start, end) = find_start_and_end(&map);
    let start = start.unwrap();
    let end = end.unwrap();
    map[start.1][start.0] = 'a';
    map[end.1][end.0] = 'z';
    (map, start, end)
}

fn find_start_and_end(map: &Vec<Vec<char>>) -> (Option<(usize, usize)>, Option<(usize, usize)>) {
    let mut start = None;
    let mut end = None;
    for y in 0..map.len() {
        for x in 0.. map[y].len() {
            if map[y][x] == 'S' {
                start = Some((x, y))
            }
            else if map[y][x] == 'E' {
                end = Some((x, y))
            }
        }
    }

    (start, end)
}

fn get_heuristic_score((x, y): &(usize, usize), (dx, dy): &(usize, usize)) -> usize {
    dx.abs_diff(*x) + dy.abs_diff(*y)
}

fn get_neighbors((x,y): (usize, usize), map: &Vec<Vec<char>>) -> Vec<(usize, usize)> {
    let x_as_int: i32 = x as i32;
    let y_as_int: i32 = y as i32;

    fn get_height_difference(source: (usize, usize), dest: (usize, usize), map: &Vec<Vec<char>>) -> i32 {
        let dest_height = map[dest.1][dest.0] as i32;
        let source_height = map[source.1][source.0] as i32;
        dest_height - source_height
    }

    let candidates: Vec<(i32, i32)> = vec![ 
        (x_as_int  - 1, y_as_int), 
        (x_as_int + 1, y_as_int), 
        (x_as_int, y_as_int -1), 
        (x_as_int, y_as_int + 1) ];
    candidates.iter()
        .filter(| (x,y) | {
            x >= &0 
            && x < &(map[0].len() as i32)
            && y >= &0 
            && y < &(map.len() as i32)
        })
        .map(| (x,y)| {
            (*x as usize, *y as usize)
        })
        .filter(|dest| {
            let height_difference = get_height_difference((x,y), *dest, map);
            height_difference <= 1
        })
        .collect()

}


fn find_path_length_astar(map: &Vec<Vec<char>>, start :(usize, usize), end: (usize, usize)) -> Option<usize> {
    let mut open_set : BinaryHeap<OpenSetPoint> = BinaryHeap::new();
    let mut g_scores : HashMap<(usize, usize), usize> = HashMap::new();
    g_scores.insert(start, 0);

    open_set.push(OpenSetPoint { x: start.0, y: start.1, f_score: get_heuristic_score(&start, &end) });

    while let Some(node) = open_set.pop() {
        let x = node.x;
        let y = node.y;
        // println!("Examining {} {}", x, y);
        if node.x == end.0 && node.y == end.1 {
            // println!("Found it!");
            return Some(*g_scores.get(&(node.x, node.y)).unwrap())
        }

        for neighbor in get_neighbors((x, y), map) {
            let tentative_gscore = g_scores.get(&(x,y)).unwrap() + 1;
            let score =  g_scores.get(&neighbor).unwrap_or(&usize::MAX);
        
            // println!("Neighbour {} {} tentative score {} prev {}", neighbor.0, neighbor.1, tentative_gscore, score);

            if tentative_gscore < *score {
                // println!("Adding nighbour to set");
                g_scores.insert(neighbor, tentative_gscore);
                open_set.push(OpenSetPoint { 
                    x: neighbor.0, 
                    y: neighbor.1, 
                    f_score: tentative_gscore + get_heuristic_score(&neighbor, &end) })
            }
        }
    }
    
    None
}

#[cfg(test)]
mod tests {
    use super::*;
    static EXAMPLE: &str = r"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi";

    #[test]
    fn parse_test() {
        let (map, start, end) = parse_input(EXAMPLE);
        let expected = vec![
            vec!['a','a','b','q','p','o','n','m'],
            vec!['a','b','c','r','y','x','x','l'],
            vec!['a','c','c','s','z','z','x','k'],
            vec!['a','c','c','t','u','v','w','j'],
            vec!['a','b','d','e','f','g','h','i'],
        ];

        assert_eq!(expected, map);
        assert_eq!((0, 0), start);
        assert_eq!((5, 2), end);
    }

    #[test]
    fn openset_test() {
        let mut set: BinaryHeap<OpenSetPoint> = BinaryHeap::new();
        set.push(OpenSetPoint { x: 10, y: 10, f_score: 10 });
        set.push(OpenSetPoint { x: 1, y: 0, f_score: 9 });
        set.push(OpenSetPoint { x: 2, y: 0, f_score: 8 });
        assert_eq!(OpenSetPoint { x:2, y:0, f_score: 8}, set.pop().unwrap())
    }

    #[test]
    fn part1_test() {
        let sovler = HillClimbingAlgorithm::new(EXAMPLE);
        assert_eq!("31", sovler.solve_part1());
    }

    #[test]
    fn part2_test() {
        let sovler = HillClimbingAlgorithm::new(EXAMPLE);
        assert_eq!("29", sovler.solve_part2());
    }
}