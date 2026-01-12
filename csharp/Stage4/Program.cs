using System;
using System.IO;

namespace MidLang.Stage4
{
    /// <summary>
    /// Main entry point for the MidLang Stage 4 interpreter.
    /// 
    /// This program demonstrates the three-stage interpreter architecture:
    /// 1. Lexer: Converts source code to tokens
    /// 2. Parser: Builds AST from tokens
    /// 3. Evaluator: Executes AST
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: MidLang.Stage4 <source_file.mid>");
                Console.WriteLine("Example: MidLang.Stage4 examples/program.mid");
                args = new string[] { @"c:\temp\stage3_example4.mid" };        
            }

            string sourceFile = args[0];

            if (!File.Exists(sourceFile))
            {
                Console.WriteLine($"Error: File not found: {sourceFile}");
                return;
            }

            try
            {
                // Read source code
                string sourceCode = File.ReadAllText(sourceFile);
                Console.WriteLine($"=== Interpreting: {sourceFile} ===\n");

                // Stage 1: Lexical Analysis
                Console.WriteLine("Stage 1: Lexical Analysis (Tokenization)");
                Lexer lexer = new Lexer(sourceCode);
                var tokens = lexer.Tokenize();
                Console.WriteLine($"Generated {tokens.Count} tokens:");
                foreach (var token in tokens)
                {
                    if (token.Type != TokenType.EOF)
                    {
                        Console.WriteLine($"  {token}");
                    }
                }
                Console.WriteLine();

                // Stage 2: Parsing
                Console.WriteLine("Stage 2: Parsing (Building AST)");
                Parser parser = new Parser(tokens);
                ProgramNode ast = parser.Parse();
                Console.WriteLine($"Parsed {ast.Statements.Count} statement(s)");
                Console.WriteLine();

                // Stage 3: Evaluation
                Console.WriteLine("Stage 3: Evaluation (Execution)");
                Console.WriteLine("Output:");
                Evaluator evaluator = new Evaluator();
                evaluator.Evaluate(ast);
                Console.WriteLine();

                Console.WriteLine("=== Program completed successfully ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}

