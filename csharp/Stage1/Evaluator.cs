using System;
using System.Collections.Generic;

namespace MidLang.Stage1
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
        /// Evaluates a statement (assignment or print).
        /// </summary>
        private void EvaluateStatement(Statement statement)
        {
            switch (statement)
            {
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
        /// Executes an assignment statement: stores the expression's value in the variable.
        /// </summary>
        private void EvaluateAssignment(AssignmentStatement assign)
        {
            int value = EvaluateExpression(assign.Expression);
            _symbolTable[assign.VariableName] = value;
        }

        /// <summary>
        /// Executes a print statement: evaluates the expression and outputs the result.
        /// </summary>
        private void EvaluatePrint(PrintStatement print)
        {
            int value = EvaluateExpression(print.Expression);
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
                VariableReference varRef => EvaluateVariable(varRef),
                BinaryExpression binExpr => EvaluateBinaryExpression(binExpr),
                _ => throw new Exception($"Unknown expression type: {expression.GetType()}")
            };
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

