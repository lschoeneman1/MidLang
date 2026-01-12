using System;
using System.Collections.Generic;

namespace MidLang.Stage3
{
    /// <summary>
    /// Evaluator (Interpreter)
    /// 
    /// Purpose: Executes the AST to produce program output.
    /// 
    /// How it works:
    /// 1. Traverses the AST nodes
    /// 2. Evaluates expressions (computes values)
    /// 3. Manages variable storage (symbol table)
    /// 4. Executes statements (assignments, prints)
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// Symbol table: stores variable names and their values.
        /// This is like a dictionary: variable name → value
        /// </summary>
        private readonly Dictionary<string, int> _symbolTable = new Dictionary<string, int>();

        /// <summary>
        /// Evaluates a program by executing all its statements.
        /// </summary>
        public void Evaluate(ProgramNode program)
        {
            foreach (Statement statement in program.Statements)
            {
                EvaluateStatement(statement);
            }
        }

        /// <summary>
        /// Evaluates a statement (var declaration, assignment, print, println, or if).
        /// </summary>
        private void EvaluateStatement(Statement statement)
        {
            switch (statement)
            {
                case VarDeclarationStatement varDecl:
                    EvaluateVarDeclaration(varDecl);
                    break;

                case AssignmentStatement assign:
                    EvaluateAssignment(assign);
                    break;

                case PrintStatement print:
                    EvaluatePrint(print);
                    break;

                case PrintLineStatement println:
                    EvaluatePrintLine(println);
                    break;

                case IfStatement ifStmt:
                    EvaluateIfStatement(ifStmt);
                    break;

                default:
                    throw new Exception($"Unknown statement type: {statement.GetType()}");
            }
        }

        /// <summary>
        /// Executes a variable declaration: stores the expression's value in the variable.
        /// </summary>
        private void EvaluateVarDeclaration(VarDeclarationStatement varDecl)
        {
            int value = EvaluateExpression(varDecl.Expression);
            _symbolTable[varDecl.VariableName] = value;
        }

        /// <summary>
        /// Executes an assignment statement: stores the expression's value in the variable.
        /// </summary>
        private void EvaluateAssignment(AssignmentStatement assign)
        {
            int value = EvaluateExpression(assign.Expression);
            _symbolTable[assign.VariableName] = value;
        }

        /// <summary>
        /// Executes a print statement: evaluates the expression and outputs the result (no newline).
        /// </summary>
        private void EvaluatePrint(PrintStatement print)
        {
            int value = EvaluateExpression(print.Expression);
            Console.Write(value);
        }

        /// <summary>
        /// Executes a println statement: evaluates the expression and outputs the result with a newline.
        /// </summary>
        private void EvaluatePrintLine(PrintLineStatement println)
        {
            int value = EvaluateExpression(println.Expression);
            Console.WriteLine(value);
        }

        /// <summary>
        /// Evaluates an expression and returns its integer value.
        /// </summary>
        private int EvaluateExpression(Expression expression)
        {
            return expression switch
            {
                IntegerLiteral lit => lit.Value,
                InputIntExpression input => EvaluateInputInt(),
                VariableReference varRef => EvaluateVariable(varRef),
                BinaryExpression binExpr => EvaluateBinaryExpression(binExpr),
                BooleanExpression boolExpr => EvaluateBooleanExpression(boolExpr) ? 1 : 0,
                _ => throw new Exception($"Unknown expression type: {expression.GetType()}")
            };
        }

        /// <summary>
        /// Evaluates a boolean expression and returns true or false.
        /// </summary>
        private bool EvaluateBooleanExpression(BooleanExpression boolExpr)
        {
            int left = EvaluateExpression(boolExpr.Left);
            int right = EvaluateExpression(boolExpr.Right);

            return boolExpr.Operator switch
            {
                "==" => left == right,
                "!=" => left != right,
                "<" => left < right,
                ">" => left > right,
                "<=" => left <= right,
                ">=" => left >= right,
                _ => throw new Exception($"Unknown comparison operator: {boolExpr.Operator}")
            };
        }

        /// <summary>
        /// Executes an if statement: evaluates the condition and executes the appropriate block.
        /// </summary>
        private void EvaluateIfStatement(IfStatement ifStmt)
        {
            bool condition = EvaluateBooleanExpression(ifStmt.Condition);
            
            if (condition)
            {
                // Execute then block
                foreach (Statement stmt in ifStmt.ThenStatements)
                {
                    EvaluateStatement(stmt);
                }
            }
            else if (ifStmt.ElseStatements != null)
            {
                // Execute else block
                foreach (Statement stmt in ifStmt.ElseStatements)
                {
                    EvaluateStatement(stmt);
                }
            }
        }

        /// <summary>
        /// Evaluates an inputInt() expression: reads an integer from the console.
        /// </summary>
        private int EvaluateInputInt()
        {
            string input = Console.ReadLine() ?? "";
            if (int.TryParse(input, out int value))
            {
                return value;
            }
            throw new Exception($"Invalid integer input: '{input}'");
        }

        /// <summary>
        /// Evaluates a variable reference: looks up the variable's value in the symbol table.
        /// </summary>
        private int EvaluateVariable(VariableReference varRef)
        {
            if (!_symbolTable.ContainsKey(varRef.Name))
            {
                throw new Exception($"Undefined variable: {varRef.Name}");
            }
            return _symbolTable[varRef.Name];
        }

        /// <summary>
        /// Evaluates a binary expression: evaluates left and right, then applies the operator.
        /// </summary>
        private int EvaluateBinaryExpression(BinaryExpression binExpr)
        {
            int left = EvaluateExpression(binExpr.Left);
            int right = EvaluateExpression(binExpr.Right);

            return binExpr.Operator switch
            {
                "+" => left + right,
                "-" => left - right,
                "*" => left * right,
                "/" => right == 0 ? throw new Exception("Division by zero") : left / right,
                _ => throw new Exception($"Unknown operator: {binExpr.Operator}")
            };
        }
    }
}

