use std::{rc::{Rc, Weak}, collections::{HashMap, hash_map::RandomState}, cell::RefCell};

use super::Solution;

pub struct NoSpaceLeftOnDevice {
    
}
impl NoSpaceLeftOnDevice {
    pub fn new(input: &str) -> Self {
        NoSpaceLeftOnDevice {

        }
    }
}

impl Solution for NoSpaceLeftOnDevice {
    fn solve_part1(&self) -> String {
        todo!()
    }

    fn solve_part2(&self) -> String {
        todo!()
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

    fn to_file(&self) -> Option<(&'a str, usize)> {
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

fn parse_tree(commands: &str) -> Tree {
    let root = Directory { name: "/", sub_dirs: RefCell::new(HashMap::new()), parent: Weak::new() };
    let root = Rc::new(Tree::Directory(root));
    let mut current = root.as_ref();
    let mut line_iter = commands.split("\n").skip(1);

    while let Some(cmd) = line_iter.next() {
        if cmd.starts_with("$ cd") {
            let dir_name = &cmd[5..];
           
        }
    }

    Tree::File("dummy", 10)
}

#[cfg(test)]
mod tests {
    use std::borrow::BorrowMut;

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

        assert_eq!(*expected, parse_tree(EXAMPLE));
    }
}