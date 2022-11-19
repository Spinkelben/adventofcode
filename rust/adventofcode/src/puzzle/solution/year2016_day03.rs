use super::Solution;

pub struct SquareWithThreeSides<'a> {
    input: &'a str
}

impl<'a> SquareWithThreeSides<'a> {
    pub fn new(input: &'a str) -> SquareWithThreeSides<'a> {
        SquareWithThreeSides { input }
    }
}

impl Solution for SquareWithThreeSides<'_> {
    fn solve_part1(&self) -> String {
        let triangles = Triangle::parse_triangles(self.input);
        triangles
            .filter(|t| { t.is_possible() })
            .count()
            .to_string()
    }

    fn solve_part2(&self) -> String {
        let triangles = Triangle::parse_triangles_by_column(self.input);
        triangles
            .iter()
            .filter(|t| { t.is_possible() })
            .count()
            .to_string()
    }
}

#[derive(Debug, PartialEq)]
struct Triangle(i32, i32, i32);

impl Triangle {
    fn is_possible(&self) -> bool {
        self.0 + self.1 > self.2 &&
        self.1 + self.2 > self.0 &&
        self.2 + self.0 > self.1
    }

    fn parse_triangles(input: &str) -> impl Iterator<Item = Triangle> + '_ {
        fn parse_triangle(line: &str) -> Option<Triangle> {
            let abc = line
                .split(" ")
                .filter_map(|e| {
                    e.parse::<i32>().ok()
                }).take(3)
                .collect::<Vec<i32>>();

            if abc.len() == 3 {
                Some(Triangle(abc[0], abc[1], abc[2]))
            } else {
                None
            }
        }

        input.split("\n")
            .filter_map(parse_triangle)
    }

    fn parse_triangles_by_column(input: &str) -> Vec<Triangle>  {
        Triangle::parse_triangles(input)
            .collect::<Vec<Triangle>>()
            .chunks_exact(3)
            .flat_map(|chunk| {
                vec![Triangle(chunk[0].0, chunk[1].0, chunk[2].0),
                    Triangle(chunk[0].1, chunk[1].1, chunk[2].1),
                    Triangle(chunk[0].2, chunk[1].2, chunk[2].2) ]
            })
            .collect()
    }
}




#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn single_invalid_triangle_test() {
        let triangle = Triangle(5, 10, 25);
        assert!(!triangle.is_possible())
    }

    #[test]
    fn parse_triangle_test() {
        let input = 
            r"  775  785  361
                622  375  125
                297  839  375
            ";
        let expected = vec![ 
            Triangle(775, 785, 361),
            Triangle(622, 375, 125),
            Triangle(297, 839, 375)];
        
        let actual = Triangle::parse_triangles(input);
        assert_eq!(expected, actual.collect::<Vec<Triangle>>())
    }

    #[test]
    fn parse_triangle_by_column() {
        let input = 
            r"  775  785  361
                622  375  125
                297  839  375
            ";
        let expected = vec![ 
            Triangle(775, 622, 297),
            Triangle(785, 375, 839),
            Triangle(361, 125, 375)];
        
        let actual = Triangle::parse_triangles_by_column(input);
        assert_eq!(expected, actual)
    }
}
