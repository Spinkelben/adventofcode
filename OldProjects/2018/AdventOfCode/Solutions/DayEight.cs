using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public class DayEight : IDaySolution
    {
        public string PartOne(string input)
        {
            var root = ParseInput(input);
            var queue = new Queue<Node>();
            queue.Enqueue(root);
            var sum = 0;
            while(queue.Count > 0)
            {
                var item = queue.Dequeue();
                foreach (var child in item.Children)
                {
                    queue.Enqueue(child);
                }
                sum += item.Metadata.Sum();
            }

            return sum.ToString();
        }

        public string PartTwo(string input)
        {
            var root = ParseInput(input);
            return CalcNodeValue(root).ToString();
        }

        public int CalcNodeValue(Node node)
        {
            if (node.Children.Length == 0)
            {
                return node.Metadata.Sum();
            }

            var value = 0;
            foreach (var index in node.Metadata)
            {
                if (index > 0 && index <= node.Children.Length)
                {
                    var reference = index - 1;
                    value += CalcNodeValue(node.Children[reference]);
                }
            }
            return value;
        }

        public Node ParseInput(string input)
        {
            var values = input.Replace("\r", "").Replace("\n", "").Split(' ').Select(s => int.Parse(s)).GetEnumerator();
            
            values.MoveNext();
            var numChildren = values.Current;
            values.MoveNext();
            var numMetadata = values.Current;
            var root = new Node(numChildren, numMetadata);

            var stack = new Stack<Node>();
            stack.Push(root);

            while (values.MoveNext())
            {
                var current = stack.Peek();
                var childNumber = current.Children.Count(c => c != null);
                if (childNumber == current.Children.Length)
                {
                    for (int i = 0; i < current.Metadata.Length - 1; i++)
                    {
                        current.Metadata[i] = values.Current;
                        values.MoveNext();
                    }
                    current.Metadata[current.Metadata.Length - 1] = values.Current;
                    stack.Pop();
                }
                else
                {
                    numChildren = values.Current;
                    values.MoveNext();
                    numMetadata = values.Current;
                    var child = new Node(numChildren, numMetadata);
                    current.Children[childNumber] = child;
                    stack.Push(child);
                }
            }
            

            return root;
        }

        public class Node
        {
            public int[] Metadata { get; }

            public Node[] Children { get; }

            public Node(int numChildren, int numMetadata)
            {
                Children = new Node[numChildren];
                Metadata = new int[numMetadata];
            }
        }
    }
}
