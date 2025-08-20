using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace CraftingInterpreters.Lox
{
    public class Lox
    {
        private static bool hadError = false;
        public static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: slox [script]");
                // In C#, we use Environment.Exit for termination codes.
                Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                // The implementation for RunFile was not included in your snippet.
                // This is where you would call its C# equivalent.
                RunFile(args[0]); 
            }
            else
            {
                // The implementation for RunPrompt was not included in your snippet.
                // This is where you would call its C# equivalent.
                RunPrompt();
            }
        }
        private static void RunFile(string path)
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    string content = Encoding.Default.GetString(bytes);
                    Run(content);

                    if (hadError) { Environment.Exit(65); }
                }
            }
        }
        private static void RunPrompt()
        {
            while (true)
            {
                Console.Write("> ");
                string? line = Console.ReadLine();

                if (line == null)
                {
                    break;
                }
                Run(line);
                hadError = false;
            }
        }
        private static void Run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.ScanTokens();

            foreach (Token token in tokens)
            {
                Console.WriteLine(token);
            }
        }
        internal static void Error(int line, string message)
        {
            Report(line, "", message);
        }
        private static void Report(int line, string where, string message)
        {
            Console.Error.WriteLine($"[line {line}] Error {where} : {message}");
            hadError = true;
        }
    }
}