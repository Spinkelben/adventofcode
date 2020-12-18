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
            BigInteger totalSum = SumExpressions(input, 1);
            return $"{totalSum}";
        }



        public string Part2(IList<string> input)
        {
            BigInteger totalSum = SumExpressions(input, 2);
            return $"{totalSum}";
        }

        private BigInteger SumExpressions(IList<string> input, int part)
        {
            var totalSum = BigInteger.Zero;
            foreach (var expression in input.Where(i => i.Length > 0))
            {
                var tokens = Tokenize(expression);
                var tree = GetExpressionTree(tokens, part);
                totalSum += tree.Value;
            }

            return totalSum;
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

        private ExpressionTree GetExpressionTree(IEnumerable<string> tokens, int part)
        {
            var tokenList = tokens.ToList();
            var currentTokenIdx = 0;

            // Adapted shunting yard
            var endExpression = new Stack<ExpressionTree>();
            var operatorStack = new Stack<ExpressionTree>();
            while (currentTokenIdx < tokenList.Count)
            {
                var token = tokenList[currentTokenIdx];
                var node = GetNode(token, part);

                if (node is ValueNode)
                {
                    endExpression.Push(node);
                }
                else if (node is BinaryOperator binOp)
                {

                    while (StackIsNotEmpty(operatorStack)
                        && !TopNodeIsLeftParenthesis(operatorStack)
                        && TopNodePrecendenceIsGreaterOrEqual(operatorStack, binOp))
                    {
                        AddOperatorToOutput(endExpression, operatorStack);
                    }

                    operatorStack.Push(binOp);
                }
                else if (node is Parenthesis p && p.Orientation == Parenthesis.Type.Left)
                {
                    operatorStack.Push(p);
                }
                else if (node is Parenthesis p1 && p1.Orientation == Parenthesis.Type.Right)
                {
                    while (StackIsNotEmpty(operatorStack) 
                        && !TopNodeIsLeftParenthesis(operatorStack))
                    {
                        var @operator = operatorStack.Pop() as BinaryOperator;
                        @operator.Right = endExpression.Pop();
                        @operator.Left = endExpression.Pop();
                        endExpression.Push(@operator);
                    }

                    if (operatorStack.Peek() is Parenthesis p2 && p2.Orientation == Parenthesis.Type.Left)
                    {
                        operatorStack.Pop();
                    }

                    p1.Child = endExpression.Pop();
                    endExpression.Push(p1);
                }

                currentTokenIdx++;
            }

            while (operatorStack.Count > 0)
            {
                AddOperatorToOutput(endExpression, operatorStack);
            }

            if (endExpression.Count == 1)
            {
                return endExpression.Pop();
            }

            throw new InvalidOperationException($"Something is up with this expression");
        }

        private static void AddOperatorToOutput(Stack<ExpressionTree> endExpression, Stack<ExpressionTree> operatorStack)
        {
            var @operator = operatorStack.Pop();
            if (@operator is BinaryOperator binOp)
            {
                binOp.Right = endExpression.Pop();
                binOp.Left = endExpression.Pop();
            }
            else if (@operator is Parenthesis p)
            {
                p.Child = endExpression.Pop();
            }
            else
            {
                throw new InvalidOperationException("Something wrong in the output set");
            }
            
            endExpression.Push(@operator);
        }

        private bool TopNodePrecendenceIsGreaterOrEqual(Stack<ExpressionTree> operatorStack, BinaryOperator node)
        {
            if (operatorStack.Peek() is BinaryOperator topNode)
            {
                return topNode.Precedence <= node.Precedence;
            }

            return false;
        }

        private static bool TopNodeIsLeftParenthesis(Stack<ExpressionTree> operatorStack)
        {
            if (operatorStack.Peek() is Parenthesis p)
            {
                return p.Orientation == Parenthesis.Type.Left;
            }

            return false; ;
        }

        private static bool StackIsNotEmpty(Stack<ExpressionTree> operatorStack)
        {
            return operatorStack.Count > 0;
        }

        private ExpressionTree GetNode(string token, int part)
        {
            return token switch
            {
                "*" => new MultiplicationOp(part),
                "+" => new AdditionOp(part),
                "(" => new Parenthesis(part, Parenthesis.Type.Left),
                ")" => new Parenthesis(part, Parenthesis.Type.Right),
                _ => new ValueNode(int.Parse(token), part),
            };
        }

        abstract class ExpressionTree
        {
            protected readonly int part;

            internal ExpressionTree(int part)
            {
                this.part = part;
            }

            internal abstract ExpressionTree AddNode(ExpressionTree node);

            internal abstract long Value { get; }
        }

        abstract class BinaryOperator : ExpressionTree
        {
            internal BinaryOperator(int part)
                : base(part)
            { }

            internal ExpressionTree Left { get; set; }
            internal ExpressionTree Right { get; set; }

            internal abstract int Precedence { get; }

            internal override ExpressionTree AddNode(ExpressionTree node)
            {
                Right = node;
                return this;
            }
        }

        class Parenthesis : ExpressionTree
        {
            internal Parenthesis(int part, Type orientation) : base(part)
            {
                Orientation = orientation;
            }

            internal static string StartSymbol => "(";
            internal static string EndSymbol => ")";
            internal ExpressionTree Child { get; set; }

            internal override long Value => Child.Value;

            internal Type Orientation { get; private set; }

            internal override ExpressionTree AddNode(ExpressionTree node)
            {
                this.Child = node;
                return node;
            }

            public override string ToString()
            {
                return $"({Child})";
            }

            internal enum Type
            {
                Left,
                Right
            }
        }

        class ValueNode : ExpressionTree
        {
            private readonly long value;

            public ValueNode(int value, int part) : base(part)
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
            internal MultiplicationOp(int part) : base(part)
            { }

            internal static string Symbol => "*";

            internal override long Value => Left.Value * Right.Value;

            internal override int Precedence => part == 1 ? 1 : 2;

            public override string ToString()
            {
                return $"{Left} * {Right}";
            }
        }

        class AdditionOp : BinaryOperator
        {
            public AdditionOp(int part) : base(part)
            {
            }

            internal static string Symbol => "+";

            internal override long Value => Left.Value + Right.Value;

            internal override int Precedence => 1;

            public override string ToString()
            {
                return $"{Left} + {Right}";
            }
        }
    }
}
