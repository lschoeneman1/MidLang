using System.Collections.Generic;

namespace MidLang.Stage4
{
    /// <summary>
    /// Abstract Syntax Tree (AST) nodes.
    /// The AST represents the structure of the program.
    /// </summary>

    /// <summary>
    /// Root node representing an entire program.
    /// </summary>
    public class ProgramNode
    {
        public List<Statement> Statements { get; }

        public ProgramNode(List<Statement> statements)
        {
            Statements = statements;
        }
    }

    /// <summary>
    /// Base class for all statements.
    /// </summary>
    public abstract class Statement
    {
    }

    /// <summary>
    /// Variable declaration statement: var identifier = expression;
    /// </summary>
    public class VarDeclarationStatement : Statement
    {
        public string VariableName { get; }
        public Expression Expression { get; }

        public VarDeclarationStatement(string variableName, Expression expression)
        {
            VariableName = variableName;
            Expression = expression;
        }
    }

    /// <summary>
    /// Assignment statement: identifier = expression;
    /// </summary>
    public class AssignmentStatement : Statement
    {
        public string VariableName { get; }
        public Expression Expression { get; }

        public AssignmentStatement(string variableName, Expression expression)
        {
            VariableName = variableName;
            Expression = expression;
        }
    }

    /// <summary>
    /// Print statement: print(expression); (no newline)
    /// </summary>
    public class PrintStatement : Statement
    {
        public Expression Expression { get; }

        public PrintStatement(Expression expression)
        {
            Expression = expression;
        }
    }

    /// <summary>
    /// Print line statement: println(expression); (with newline)
    /// </summary>
    public class PrintLineStatement : Statement
    {
        public Expression Expression { get; }

        public PrintLineStatement(Expression expression)
        {
            Expression = expression;
        }
    }

    /// <summary>
    /// Base class for all expressions.
    /// </summary>
    public abstract class Expression
    {
    }

    /// <summary>
    /// Integer literal: 42, -10
    /// </summary>
    public class IntegerLiteral : Expression
    {
        public int Value { get; }

        public IntegerLiteral(int value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Variable reference: x, count
    /// </summary>
    public class VariableReference : Expression
    {
        public string Name { get; }

        public VariableReference(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Binary expression: left operator right
    /// Examples: a + b, x * 5, (10 + 5) / 2
    /// </summary>
    public class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public string Operator { get; }  // "+", "-", "*", "/"
        public Expression Right { get; }

        public BinaryExpression(Expression left, string op, Expression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }

    /// <summary>
    /// Input expression: inputInt()
    /// Reads an integer from the console.
    /// </summary>
    public class InputIntExpression : Expression
    {
        public InputIntExpression()
        {
        }
    }

    /// <summary>
    /// Boolean expression: left comparisonOperator right
    /// Examples: x == 5, a < b, y >= 10
    /// </summary>
    public class BooleanExpression : Expression
    {
        public Expression Left { get; }
        public string Operator { get; }  // "==", "!=", "<", ">", "<=", ">="
        public Expression Right { get; }

        public BooleanExpression(Expression left, string op, Expression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }

    /// <summary>
    /// If statement: if (condition) { statements } [ else { statements } ]
    /// </summary>
    public class IfStatement : Statement
    {
        public BooleanExpression Condition { get; }
        public List<Statement> ThenStatements { get; }
        public List<Statement>? ElseStatements { get; }  // null if no else clause

        public IfStatement(BooleanExpression condition, List<Statement> thenStatements, List<Statement>? elseStatements = null)
        {
            Condition = condition;
            ThenStatements = thenStatements;
            ElseStatements = elseStatements;
        }
    }

    /// <summary>
    /// While statement: while (condition) { statements }
    /// </summary>
    public class WhileStatement : Statement
    {
        public BooleanExpression Condition { get; }
        public List<Statement> BodyStatements { get; }

        public WhileStatement(BooleanExpression condition, List<Statement> bodyStatements)
        {
            Condition = condition;
            BodyStatements = bodyStatements;
        }
    }
}

