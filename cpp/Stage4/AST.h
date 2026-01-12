#ifndef AST_H
#define AST_H

#include <vector>
#include <string>
#include <memory>

/**
 * Abstract Syntax Tree (AST) nodes.
 * The AST represents the structure of the program.
 */

// Forward declarations
class Statement;
class Expression;

/**
 * Root node representing an entire program.
 */
class ProgramNode {
public:
    std::vector<std::unique_ptr<Statement>> statements;

    ProgramNode(std::vector<std::unique_ptr<Statement>> stmts)
        : statements(std::move(stmts)) {}
};

/**
 * Base class for all statements.
 */
class Statement {
public:
    virtual ~Statement() = default;
};

/**
 * Variable declaration statement: var identifier = expression;
 */
class VarDeclarationStatement : public Statement {
public:
    std::string variableName;
    std::unique_ptr<Expression> expression;

    VarDeclarationStatement(const std::string& name, std::unique_ptr<Expression> expr)
        : variableName(name), expression(std::move(expr)) {}
};

/**
 * Assignment statement: identifier = expression;
 */
class AssignmentStatement : public Statement {
public:
    std::string variableName;
    std::unique_ptr<Expression> expression;

    AssignmentStatement(const std::string& name, std::unique_ptr<Expression> expr)
        : variableName(name), expression(std::move(expr)) {}
};

/**
 * Print statement: print(expression);
 */
class PrintStatement : public Statement {
public:
    std::unique_ptr<Expression> expression;

    PrintStatement(std::unique_ptr<Expression> expr)
        : expression(std::move(expr)) {}
};

/**
 * Base class for all expressions.
 */
class Expression {
public:
    virtual ~Expression() = default;
};

/**
 * Integer literal: 42, -10
 */
class IntegerLiteral : public Expression {
public:
    int value;

    IntegerLiteral(int v) : value(v) {}
};

/**
 * Variable reference: x, count
 */
class VariableReference : public Expression {
public:
    std::string name;

    VariableReference(const std::string& n) : name(n) {}
};

/**
 * Binary expression: left operator right
 * Examples: a + b, x * 5, (10 + 5) / 2
 */
class BinaryExpression : public Expression {
public:
    std::unique_ptr<Expression> left;
    std::string op;  // "+", "-", "*", "/"
    std::unique_ptr<Expression> right;

    BinaryExpression(std::unique_ptr<Expression> l, const std::string& o, std::unique_ptr<Expression> r)
        : left(std::move(l)), op(o), right(std::move(r)) {}
};

/**
 * Input expression: inputInt()
 * Reads an integer from the console.
 */
class InputIntExpression : public Expression {
public:
    InputIntExpression() {}
};

#endif // AST_H

