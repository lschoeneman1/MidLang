#ifndef EVALUATOR_H
#define EVALUATOR_H

#include <unordered_map>
#include <string>
#include "AST.h"

/**
 * Evaluator (Interpreter)
 * 
 * Purpose: Executes the AST to produce program output.
 * 
 * How it works:
 * 1. Traverses the AST nodes
 * 2. Evaluates expressions (computes values)
 * 3. Manages variable storage (symbol table)
 * 4. Executes statements (assignments, prints)
 */
class Evaluator {
private:
    // Symbol table: stores variable names and their values
    // This is like a dictionary: variable name → value
    std::unordered_map<std::string, int> symbolTable;

    // Helper methods
    void evaluateStatement(Statement* statement);
    void evaluateAssignment(AssignmentStatement* assign);
    void evaluatePrint(PrintStatement* print);
    int evaluateExpression(Expression* expression);
    int evaluateVariable(VariableReference* varRef);
    int evaluateBinaryExpression(BinaryExpression* binExpr);

public:
    /**
     * Evaluates a program by executing all its statements.
     */
    void evaluate(ProgramNode* program);
};

#endif // EVALUATOR_H

