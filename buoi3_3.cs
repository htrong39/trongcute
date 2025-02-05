using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

    class HTMLValidator
    {
        static bool ValidateHTML(string htmlContent)
        {
            Stack<string> stack = new Stack<string>();
            Regex tagRegex = new Regex("<(/?\\w+)>", RegexOptions.Compiled);

            MatchCollection matches = tagRegex.Matches(htmlContent);

            foreach (Match match in matches)
            {
                string tag = match.Groups[1].Value;

                if (!tag.StartsWith("/")) // Thẻ mở
                {
                    stack.Push(tag);
                }
                else // Thẻ đóng
                {
                    if (stack.Count == 0 || stack.Peek() != tag.Substring(1))
                    {
                        return false; // Thẻ đóng không khớp
                    }
                    stack.Pop();
                }
            }

            return stack.Count == 0; // Nếu stack rỗng thì HTML hợp lệ
        }

        static void Main()
        {
        Console.OutputEncoding = Encoding.UTF8;
            Console.Write("Nhập đường dẫn đến tệp HTML: ");
            string filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("Tệp không tồn tại.");
                return;
            }

            string htmlContent = File.ReadAllText(filePath);
            bool isValid = ValidateHTML(htmlContent);

            if (isValid)
            {
                Console.WriteLine("Tệp HTML hợp lệ.");
            }
            else
            {
                Console.WriteLine("Tệp HTML không hợp lệ.");
            }
        }
    }

