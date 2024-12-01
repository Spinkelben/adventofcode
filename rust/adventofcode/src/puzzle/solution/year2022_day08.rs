use super::Solution;

pub struct  TreetopTreeHouse {
    trees: Vec<Vec<i8>>
}
impl TreetopTreeHouse {
    pub(crate) fn new(input: &str) -> Self {
        let result = input
            .split("\n")
            .filter_map(|line| {
                let tree_line = line
                    .chars()
                    .filter_map(|c|  { char::to_digit(c, 10) } )
                    .map(|i| { i as i8 })
                    .collect::<Vec<i8>>();
                if !tree_line.is_empty() {
                    Some(tree_line)
                }
                else {
                    None
                }
            })
            .collect();
        TreetopTreeHouse { trees: result }
    }
}

impl Solution for TreetopTreeHouse {
    fn solve_part1(&self) -> String {
        let edge_tree_count = self.trees.len() * 4 - 4;
        let mut visiable_trees = edge_tree_count;
        for y in 1..self.trees.len() - 1 {
            for x in 1..self.trees.len() - 1 {
                if is_tree_visible(&self.trees, (x, y)) {
                    visiable_trees +=  1
                }
            }
        }

        visiable_trees.to_string()
    }

    fn solve_part2(&self) -> String {
        let mut max_score = 0usize;
        for y in 1..self.trees.len() {
            for x in 1..self.trees.len() {
                max_score = usize::max(max_score, calculate_scenic_score(&self.trees, (x,y)));
            }
        }

        max_score.to_string()
    }
}

fn get_trees_around(trees: &Vec<Vec<i8>>, pos: (usize, usize)) 
    -> (impl Iterator<Item = i8> + '_,
        impl Iterator<Item = i8> + '_,
        impl Iterator<Item = i8> + '_,
        impl Iterator<Item = i8> + '_) {
    let map_coord_to_tree =  |(x, y)| {
            let row: &Vec<i8> = &trees[y];
            
            row[x]
        };

    (
        std::iter::repeat(pos.0)
            .zip(0..pos.1)
            .map(map_coord_to_tree),
        (0..pos.0)
            .zip(std::iter::repeat(pos.1))
            .map(map_coord_to_tree),
        std::iter::repeat(pos.0)
            .zip(pos.1 + 1..trees.len())
            .map(map_coord_to_tree),
        (pos.0 + 1..trees.len())
            .zip(std::iter::repeat(pos.1))
            .map(map_coord_to_tree)
    )
}

fn is_tree_visible(trees: &Vec<Vec<i8>>, pos: (usize, usize)) -> bool {
    let (north, west, south, east) = get_trees_around(trees, pos);
    let largest_tree_north = north
        .max()
        .unwrap();
    let largest_tree_west = west
        .max()
        .unwrap();
    let largest_tree_south = south
        .max()
        .unwrap();
    let largest_tree_east = east
        .max()
        .unwrap();

    let height = trees[pos.1][pos.0];

    height > largest_tree_east ||
    height > largest_tree_north ||
    height > largest_tree_south ||
    height > largest_tree_west
}

fn calculate_scenic_score(trees: &Vec<Vec<i8>>, (x, y): (usize, usize)) -> usize {
    let (north, 
        west, 
        south, 
        east) = get_trees_around(trees, (x, y));
    
    fn get_score(line: impl Iterator<Item = i8>, height: i8) -> usize {
        let mut distance = 0;
        for h in line {
            distance += 1;
            if h >= height {
                return distance;
            }
        }
        
        distance
    }

    let height = trees[y][x];
    let north : Vec<i8> = north.collect();
    let west : Vec<i8> = west.collect();

    get_score(north.into_iter().rev(), height) *
    get_score(west.into_iter().rev(), height) *
    get_score(east, height) *
    get_score(south, height)

}

#[cfg(test)]
mod tests {
    use super::*;

    static EXAMPLE: &str = "30373
    25512
    65332
    33549
    35390
";

    #[test]
    fn test_parsing() {
        let expected: Vec<Vec<i8>> = vec![
            vec![ 3,0,3,7,3 ],
            vec![ 2,5,5,1,2 ],
            vec![ 6,5,3,3,2 ],
            vec![ 3,3,5,4,9 ],
            vec![ 3,5,3,9,0 ],
        ];

        let solver = TreetopTreeHouse::new(EXAMPLE);
        assert_eq!(expected, solver.trees)
    }

    #[test]
    fn height_tester_test() {
        let solver = TreetopTreeHouse::new(EXAMPLE);
        assert_eq!(true, is_tree_visible(&solver.trees, (1,1))); // Top left 5
        assert_eq!(true, is_tree_visible(&solver.trees, (2,1))); // top middle 5
        assert_eq!(false, is_tree_visible(&solver.trees, (3,1))); // top right 1
        assert_eq!(true, is_tree_visible(&solver.trees, (1,2))); // left middle 5
        assert_eq!(false, is_tree_visible(&solver.trees, (2,2))); // center 3
        assert_eq!(true, is_tree_visible(&solver.trees, (3,2))); // right middle 3
        assert_eq!(false, is_tree_visible(&solver.trees, (1,3))); // left bottom 3
        assert_eq!(true, is_tree_visible(&solver.trees, (2,3))); // center bottom 5
        assert_eq!(false, is_tree_visible(&solver.trees, (3,3))); // right bottom 4
    }

    #[test]
    fn part1_test() {
        let solver = TreetopTreeHouse::new(EXAMPLE);
        assert_eq!("21", solver.solve_part1());
    }

    #[test]
    fn scenic_score_calculator_test() {
        let solver = TreetopTreeHouse::new(EXAMPLE);
        assert_eq!(4, calculate_scenic_score(&solver.trees, (2, 1)));
        assert_eq!(8, calculate_scenic_score(&solver.trees, (2, 3)));
    }

    #[test]
    fn part2_test() {
        let solver = TreetopTreeHouse::new(EXAMPLE);
        assert_eq!("8", solver.solve_part2());
    }
}