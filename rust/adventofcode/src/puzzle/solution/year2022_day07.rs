use std::{rc::{Rc, Weak}, collections::{HashMap, hash_map::RandomState}, cell::RefCell};

use super::Solution;

pub struct NoSpaceLeftOnDevice<'a> {
    input: &'a str
}
impl<'a> NoSpaceLeftOnDevice<'a> {
    pub fn new(input: &'a str) -> Self {
        NoSpaceLeftOnDevice {
            input
        }
    }
}

impl Solution for NoSpaceLeftOnDevice<'_> {
    fn solve_part1(&self) -> String {
        let tree = parse_tree(self.input);
        fn walker(node: &Tree, acc: &Vec<&(usize, usize)>) -> (usize, usize) {
            match node {
                Tree::Directory(_) => {
                    let (sum_total, sum_small) : (usize, usize) = acc
                        .iter()
                        .map(|t| { **t })
                        .reduce(|l, r| {
                            (l.0 + r.0, l.1 + r.1)
                        }).unwrap();
                    if sum_total <= 100000
                    {
                        (sum_total, sum_small + sum_total)
                    }
                    else {
                        (sum_total, sum_small)
                    }
                },
                Tree::File(_, size) => {
                    (*size, 0) 
                },
            }
        }
        
        let (_total_size, small_dirs_size) = walk_depth_first(tree, &walker, &(0,0));
        small_dirs_size.to_string()
    }

    fn solve_part2(&self) -> String {
        let tree = parse_tree(self.input);

        fn dir_size_visitor(node: &Tree, acc: &Vec<&usize>) -> usize {
            match node {
                Tree::Directory(_) => acc.iter().map(|x| {**x }).sum(),
                Tree::File(_, size) => *acc.first().unwrap() + size,
            }
        }

        let space_used = walk_depth_first(Rc::clone(&tree), &dir_size_visitor, &0);
        let total_space = 70000000usize;
        let free_space = total_space - space_used;
        let size_required = 30000000usize;
        let space_to_free = size_required - free_space;

        let delete_candidate_visitor = |node :&Tree, acc: &Vec<&(usize, usize)>| {
            match node {
                Tree::Directory(_) => {
                    let (sum_total, sum_smallest_candidate) : (usize, usize) = acc
                        .iter()
                        .map(|t| { **t })
                        .reduce(|l, r| {
                            (l.0 + r.0, usize::min(r.1, l.1))
                        }).unwrap();
                    if sum_total >= space_to_free && sum_total < sum_smallest_candidate
                    {
                        (sum_total, sum_total)
                    }
                    else {
                        (sum_total, sum_smallest_candidate)
                    }
                },
                Tree::File(_, size) => {
                    (*size, acc.first().unwrap().1) 
                },
            }
        };

        let (_total_size, small_dirs_size) = walk_depth_first(tree, &delete_candidate_visitor, &(0,usize::MAX));
        small_dirs_size.to_string()
    }
}

#[derive(Debug, PartialEq)]
enum Tree<'a> {
    Directory(Directory<'a>),
    File(&'a str, usize),
} 

impl<'a> Tree<'a> {
    fn to_directory(&self) -> Option<&Directory<'a>> {
        match self {
            Tree::Directory(d) => Some(d),
            Tree::File(_, _) => None,
        }
    }

    fn _to_file(&self) -> Option<(&'a str, usize)> {
        match self {
            Tree::Directory(_) => None,
            Tree::File(name, size) => Some((name, *size)),
        }
    }
}

#[derive(Debug)]
struct Directory<'a> {
    name: &'a str,
    sub_dirs: RefCell<HashMap<&'a str, Rc<Tree<'a>>, RandomState>>,
    parent: Weak<Tree<'a>>,
}

impl PartialEq for Directory<'_> {
    fn eq(&self, other: &Self) -> bool {
        let name_subdir_equal = self.name == other.name 
        && self.sub_dirs == other.sub_dirs;
       
        let self_parent = self.parent.upgrade();
        let other_parent = self.parent.upgrade();
        let parent_eq = match (self_parent, other_parent) {
            (Some(s), Some(o)) => Rc::ptr_eq(&s, &o),
            (None, None) => true,
            _ => false,
        };

        name_subdir_equal && parent_eq
    }
}

fn walk_depth_first<F: Fn(&Tree, &Vec<&T>) -> T, T>(root: Rc<Tree>, walker: &F, acc: &T) -> T {
    match root.as_ref() {
        Tree::Directory(d) => {
            let subdirs = d.sub_dirs.borrow();
            let mut state = vec![];
            
            for dir in subdirs.values()
            {
                state.push(walk_depth_first(Rc::clone(dir), walker, acc));
            }

            let temp = state.iter().collect();

            walker(&root, &temp)
        },
        f => walker(f, &vec![acc]),
    }
}

fn new_subdir<'a>(parent: Rc<Tree<'a>>, name: &'a str) -> Rc<Tree<'a>> {
    let parent_dir = parent
        .to_directory()
        .expect("Files cannot have subdirs");
    let mut subdir_map = parent_dir.sub_dirs.borrow_mut();
    let new_dir = Rc::new(Tree::Directory(Directory { 
        name, 
        sub_dirs: RefCell::new(HashMap::new()),
        parent: Rc::downgrade(&parent),
    }));
    subdir_map.insert(name,  Rc::clone(&new_dir));
    Rc::clone(&new_dir)
}

fn new_file<'a>(folder: Rc<Tree<'a>>, name: &'a str, size: usize) {
    let parent_dir = folder
        .to_directory()
        .expect("Files cannot have subdirs");
    let mut dir_map = parent_dir.sub_dirs.borrow_mut();
    let new_folder = Rc::new(Tree::File(name, size));
    dir_map.insert(name, Rc::clone(&new_folder));
}

fn parse_tree(commands: &str) -> Rc<Tree> {
    let root = Directory { name: "/", sub_dirs: RefCell::new(HashMap::new()), parent: Weak::new() };
    let root = Rc::new(Tree::Directory(root));
    let mut current = Rc::clone(&root);
    let mut line_iter = commands
        .split("\n")
        .skip(1)
        .map(|l| {l.trim()})
        .peekable();

    while let Some(cmd) = line_iter.next() {
        if cmd.starts_with("$ cd") {
            // Change dir
            let dir_name = &cmd[5..];
            if dir_name == ".." {
                current = current.to_directory().unwrap().parent.upgrade().unwrap()
            }
            else {
                let temp = Rc::clone(&current);
                let current_dir = temp.to_directory().unwrap();
                let subdirs = current_dir.sub_dirs.borrow();
                let new_dir = subdirs.get(&dir_name).unwrap();
                current = Rc::clone(new_dir);
            }
        }
        else if cmd.starts_with("$ ls") {
            // list folder contents
            // Consumes all lines until next command
            while let Some(item) = line_iter.next_if(|c| { !c.starts_with("$") })
            {   
                if item.starts_with("dir ") {
                    new_subdir(Rc::clone(&current), &item[4..]);
                }
                else if item.len() > 0 {
                    let splits :Vec<&str> = item.split(" ").collect();
                    let name = splits[1];
                    let size = splits[0].parse().unwrap();
                    new_file( Rc::clone(&current), name, size)
                }
            }
        }
    }

    root
}

#[cfg(test)]
mod tests {
    use super::*;

    static EXAMPLE: &str = "$ cd /
    $ ls
    dir a
    14848514 b.txt
    8504156 c.dat
    dir d
    $ cd a
    $ ls
    dir e
    29116 f
    2557 g
    62596 h.lst
    $ cd e
    $ ls
    584 i
    $ cd ..
    $ cd ..
    $ cd d
    $ ls
    4060174 j
    8033020 d.log
    5626152 d.ext
    7214296 k";

    #[test]
    fn parse_example_test() {
        // Create root dir
        let expected = Rc::new(Tree::Directory(Directory { 
            name: "/", 
            sub_dirs: RefCell::new(HashMap::from([
                ("b.txt", Rc::new(Tree::File("b.txt", 14848514))),
                ("c.dat", Rc::new(Tree::File("c.dat", 8504156))),
            ])), 
            parent: Weak::new(), 
        }));
        let root = expected.to_directory().unwrap();

        // Create "a" Dir
        let a_dir = Rc::new(Tree::Directory(Directory { 
            name: "a", 
            sub_dirs: RefCell::new(HashMap::from([
                ("f", Rc::new(Tree::File("f", 29116))),
                ("g", Rc::new(Tree::File("g", 2557))),
                ("h.lst", Rc::new(Tree::File("h.lst", 62596))),
            ])), 
            parent: Rc::downgrade(&expected), 
        }));
        // Insert "a" dir as child of root
        root.sub_dirs.borrow_mut().insert("a", Rc::clone(&a_dir));
        // Create "e" dir
        let e_dir = Rc::new(Tree::Directory(Directory { 
            name: "e", 
            sub_dirs: RefCell::new(HashMap::from([
                ("i", Rc::new(Tree::File("i", 584)))
            ])), 
            parent: Rc::downgrade(&a_dir) 
        }));
        // Insert "e" as child of "a"
        a_dir.to_directory()
            .unwrap()
            .sub_dirs
            .borrow_mut()
            .insert("e", Rc::clone(&e_dir));

        // Create "d" dir
        let d_dir = Rc::new(Tree::Directory(Directory { 
            name: "d", 
            sub_dirs: RefCell::new(HashMap::from([
                ("j", Rc::new(Tree::File("j", 4060174))),
                ("d.log", Rc::new(Tree::File("d.log", 8033020))),
                ("d.ext", Rc::new(Tree::File("d.ext", 5626152))),
                ("k", Rc::new(Tree::File("k", 7214296))),
            ])), 
            parent: Rc::downgrade(&expected) 
        }));
        // Insert "d" dir as child of root dir
        root.sub_dirs
            .borrow_mut()
            .insert("d", Rc::clone(&d_dir));

        assert_eq!(expected, parse_tree(EXAMPLE));
    }

    #[test]
    fn dir_size_test() {
        let tree = parse_tree(EXAMPLE);

        
        fn walker(node : &Tree, acc: &Vec<&usize>) -> usize {
            let expected_sizes: HashMap<&str, usize> = HashMap::from([
                ("a", 94853),
                ("e", 584),
                ("d", 24933642),
                ("/", 48381165),
            ]);
            match node {
                Tree::Directory(d) => { 
                    let sum: usize = acc
                        .iter()
                        .map(|n| { *n })
                        .sum();
                    println!("directory {}, size {}", d.name, sum);
                    assert_eq!(expected_sizes.get(d.name).unwrap(), &sum);
                    sum
                },
                Tree::File(n, size) => {
                    let sum = acc.first().unwrap();
                    println!("file {} size: {}, acc {}", n, size, sum);
                    *sum + size
                },
            }
        }

        walk_depth_first(tree, &walker, &0);
    }

    #[test]
    fn part1_test() {
        let solver = NoSpaceLeftOnDevice::new(EXAMPLE);
        assert_eq!("95437", solver.solve_part1());
    }

    #[test]
    fn part2_test() {
        let solver = NoSpaceLeftOnDevice::new(EXAMPLE);
        assert_eq!("24933642", solver.solve_part2());
    }
}