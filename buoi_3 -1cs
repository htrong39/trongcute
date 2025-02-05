using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class InfixToPostfix
    {
        static int GetPrecedence(char op)
        {
            if (op == '+' || op == '-') return 1;
            if (op == '*' || op == '/') return 2;
            if (op == '^') return 3;
            return 0;
        }

        static bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/' || c == '^';
        }

        static string ConvertToPostfix(string infix)
        {
            Stack<char> stack = new Stack<char>();
            string postfix = "";

            foreach (char c in infix)
            {
                if (char.IsLetterOrDigit(c)) // Toán hạng
                    postfix += c;
                else if (c == '(')
                    stack.Push(c);
                else if (c == ')')
                {
                    while (stack.Count > 0 && stack.Peek() != '(')
                        postfix += stack.Pop();
                    stack.Pop(); // Loại bỏ '(' khỏi stack
                }
                else if (IsOperator(c))
                {
                    while (stack.Count > 0 && GetPrecedence(stack.Peek()) >= GetPrecedence(c))
                        postfix += stack.Pop();
                    stack.Push(c);
                }
            }

            while (stack.Count > 0)
                postfix += stack.Pop();

            return postfix;
        }

        static int EvaluatePostfix(string postfix)
        {
            Stack<int> stack = new Stack<int>();

            foreach (char c in postfix)
            {
                if (char.IsDigit(c))
                    stack.Push(c - '0'); // Chuyển ký tự số thành số nguyên
                else if (IsOperator(c))
                {
                    int b = stack.Pop();
                    int a = stack.Pop();
                    int result = 0;
                    switch (c)
                    {
                        case '+': result = a + b; break;
                        case '-': result = a - b; break;
                        case '*': result = a * b; break;
                        case '/': result = a / b; break;
                        case '^': result = (int)Math.Pow(a, b); break;
                    }
                    stack.Push(result);
                }
            }

            return stack.Pop();
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Write("Nhập biểu thức Infix: ");
            string infix = Console.ReadLine();
            string postfix = ConvertToPostfix(infix);
            Console.WriteLine("Biểu thức Postfix: " + postfix);
            Console.WriteLine("Giá trị của biểu thức: " + EvaluatePostfix(postfix));
        }
    }

}

