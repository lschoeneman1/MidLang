using System;
using System.Collections.Generic;

namespace MidLang.Stage2
{
    /// <summary>
    /// Evaluator (Interpreter) - Stage 2
    /// 
    /// Purpose: Executes the AST to produce program output.
    /// 
    /// Stage 2 Extensions:
    /// - Symbol table now stores object values (int or string)
    /// - String concatenation with + operator
    /// - Type conversion: int + string converts int to string
    /// - Character literals are treated as single-character strings
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// Symbol table: stores variable names and their values.
        /// Values can be int or string (object type).
        /// </summary>
        private readonly Dictionary<string, object> _symbolTable = new Dictionary<string, object>();

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
        /// Evaluates a statement (var declaration, assignment, or print).
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

                default:
                    throw new Exception($"Unknown statement type: {statement.GetType()}");
            }
        }

        /// <summary>
        /// Executes a variable declaration: stores the expression's value in the variable.
        /// </summary>
        private void EvaluateVarDeclaration(VarDeclarationStatement varDecl)
        {
            object value = EvaluateExpression(varDecl.Expression);
            _symbolTable[varDecl.VariableName] = value;
        }

        /// <summary>
        /// Executes an assignment statement: stores the expression's value in the variable.
        /// </summary>
        private void EvaluateAssignment(AssignmentStatement assign)
        {
            object value = EvaluateExpression(assign.Expression);
            _symbolTable[assign.VariableName] = value;
        }

        /// <summary>
        /// Executes a print statement: evaluates the expression and outputs the result.
        /// </summary>
        private void EvaluatePrint(PrintStatement print)
        {
            object value = EvaluateExpression(print.Expression);
            Console.WriteLine(value);
        }

        /// <summary>
        /// Evaluates an expression and returns its value (int or string).
        /// </summary>
        private object EvaluateExpression(Expression expression)
        {
            return expression switch
            {
                IntegerLiteral lit => lit.Value,
                StringLiteral lit => lit.Value,
                CharLiteral lit => lit.Value.ToString(), // Convert char to string
                InputIntExpression input => EvaluateInputInt(),
                InputStringExpression input => EvaluateInputString(),
                VariableReference varRef => EvaluateVariable(varRef),
                BinaryExpression binExpr => EvaluateBinaryExpression(binExpr),
                _ => throw new Exception($"Unknown expression type: {expression.GetType()}")
            };
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
        /// Evaluates an inputString() expression: reads a string from the console.
        /// </summary>
        private string EvaluateInputString()
        {
            return Console.ReadLine() ?? "";
        }

        /// <summary>
        /// Evaluates a variable reference: looks up the variable's value in the symbol table.
        /// </summary>
        private object EvaluateVariable(VariableReference varRef)
        {
            if (!_symbolTable.ContainsKey(varRef.Name))
            {
                throw new Exception($"Undefined variable: {varRef.Name}");
            }
            return _symbolTable[varRef.Name];
        }

        /// <summary>
        /// Evaluates a binary expression: evaluates left and right, then applies the operator.
        /// Handles:
        /// - Integer arithmetic: +, -, *, /
        /// - String concatenation: + (with automatic int-to-string conversion)
        /// </summary>
        private object EvaluateBinaryExpression(BinaryExpression binExpr)
        {
            object leftObj = EvaluateExpression(binExpr.Left);
            object rightObj = EvaluateExpression(binExpr.Right);

            // String concatenation
            if (binExpr.Operator == "+")
            {
                string leftStr = ConvertToString(leftObj);
                string rightStr = ConvertToString(rightObj);
                return leftStr + rightStr;
            }

            // For other operators, both operands must be integers
            if (!(leftObj is int left) || !(rightObj is int right))
            {
                throw new Exception($"Operator '{binExpr.Operator}' requires integer operands");
            }

            return binExpr.Operator switch
            {
                "-" => left - right,
                "*" => left * right,
                "/" => right == 0 ? throw new Exception("Division by zero") : left / right,
                _ => throw new Exception($"Unknown operator: {binExpr.Operator}")
            };
        }

        /// <summary>
        /// Converts a value to a string representation.
        /// Used for string concatenation and type conversion.
        /// </summary>
        private string ConvertToString(object value)
        {
            return value switch
            {
                int i => i.ToString(),
                string s => s,
                _ => value?.ToString() ?? ""
            };
        }
    }
}

