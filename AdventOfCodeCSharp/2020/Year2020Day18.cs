using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace AdventOfCodeCSharp.Year2020
{
    [AdventOfCodeSolution(2020, 18)]
    internal class Year2020Day18 : IAocPuzzleSolver
    {
        public string Part1(IList<string> input)
        {
            var totalSum = BigInteger.Zero;
            foreach (var expression in input.Where(i => i.Length > 0))
            {
                var tokens = Tokenize(expression);
                var tree = GetExpressionTree(tokens);
                totalSum += tree.Value;
            }

            return $"{totalSum}";
        }

        public string Part2(IList<string> input)
        {
            return string.Empty;
        }

        private IEnumerable<string> Tokenize(string input)
        {
            var tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(t =>
                {
                    var result = new List<string>();
                    var endList = new List<char>();

                    while (t[0] == '(')
                    {
                        result.Add(t[0].ToString());
                        t = t[1..];
                    }

                    while (t.Length > 0 && t[^1] == ')')
                    {
                        endList.Add(t[^1]);
                        t = t[..^1];
                    }

                    result.Add(t);
                    result.AddRange(endList.Select(l => l.ToString()));

                    return result;
                });

            return tokens;
        }

        private ExpressionTree GetExpressionTree(IEnumerable<string> tokens)
        {
            var tokenList = tokens.ToList();
            var currentTokenIdx = 0;
            Stack<ExpressionTree> nodeStack = new Stack<ExpressionTree>();
            while (currentTokenIdx < tokenList.Count)
            {
                var token = tokenList[currentTokenIdx];
                if (token == MultiplicationOp.Symbol)
                {
                    var node = new MultiplicationOp()
                    {
                        Left = nodeStack.Pop(),
                    };

                    nodeStack.Push(node);
                }
                else if (token == AdditionOp.Symbol)
                {
                    var node = new AdditionOp()
                    {
                        Left = nodeStack.Pop(),
                    };

                    nodeStack.Push(node);
                }
                else if (token == Parenthesis.StartSymbol)
                {
                    nodeStack.Push(new Parenthesis());
                }
                else if (token == Parenthesis.EndSymbol)
                {
                    var subExpression = nodeStack.Pop();
                    var parenthesis = new Parenthesis();
                    parenthesis.AddNode(subExpression);
                    if (nodeStack.Count > 0)
                    {
                        nodeStack.Peek().AddNode(parenthesis);
                    }
                    else
                    {
                        nodeStack.Push(parenthesis);
                    }
                }
                else
                {
                    // Must be a number
                    var node = new ValueNode(int.Parse(token));

                    if (nodeStack.Count == 0)
                    {
                        nodeStack.Push(node);
                    }
                    else
                    {
                        var t = nodeStack.Pop();
                        var result = t.AddNode(node);
                        nodeStack.Push(result);
                    }
                }

                currentTokenIdx++;
            }

            if (nodeStack.Count == 1)
            {
                return nodeStack.Pop();
            }

            throw new InvalidOperationException($"Something is up with this expression");
        }

        abstract class ExpressionTree
        {
            internal abstract ExpressionTree AddNode(ExpressionTree node);

            internal abstract long Value { get; }
        }

        abstract class BinaryOperator : ExpressionTree
        {
            internal ExpressionTree Left { get; set; }
            internal ExpressionTree Right { get; set; }

            internal override ExpressionTree AddNode(ExpressionTree node)
            {
                Right = node;
                return this;
            }
        }

        class Parenthesis : ExpressionTree
        {
            internal static string StartSymbol => "(";
            internal static string EndSymbol => ")";
            internal ExpressionTree Child { get; set; }

            internal override long Value => Child.Value;

            internal override ExpressionTree AddNode(ExpressionTree node)
            {
                this.Child = node;
                return node;
            }

            public override string ToString()
            {
                return $"({Child})";
            }
        }

        class ValueNode : ExpressionTree
        {
            private readonly long value;

            public ValueNode(int value)
            {
                this.value = value;
            }

            internal override long Value => value;

            internal override ExpressionTree AddNode(ExpressionTree node)
            {
                throw new InvalidOperationException($"{node} cannot follow {this}");
            }

            public override string ToString()
            {
                return $"{Value}";
            }
        }

        class MultiplicationOp : BinaryOperator
        {
            internal static string Symbol => "*";

            internal override long Value => Left.Value * Right.Value;

            public override string ToString()
            {
                return $"{Left} * {Right}";
            }
        }

        class AdditionOp : BinaryOperator
        {
            internal static string Symbol => "+";

            internal override long Value => Left.Value + Right.Value;

            public override string ToString()
            {
                return $"{Left} + {Right}";
            }
        }
    }
}
