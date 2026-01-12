#ifndef EVALUATOR_H
#define EVALUATOR_H

#include <unordered_map>
#include <string>
#include "AST.h"
#include "Value.h"

/**
 * Evaluator (Interpreter) - Stage 2
 * 
 * Purpose: Executes the AST to produce program output.
 * 
 * Stage 2 Extensions:
 * - Symbol table now stores Value objects (can hold int or string)
 * - String concatenation with + operator
 * - Type conversion: int + string converts int to string
 * - Character literals are treated as single-character strings
 */
class Evaluator {
private:
    // Symbol table: stores variable names and their values
    // Values can be int or string (using simple Value class)
    std::unordered_map<std::string, Value> symbolTable;

    // Helper methods
    void evaluateStatement(Statement* statement);
    void evaluateVarDeclaration(VarDeclarationStatement* varDecl);
    void evaluateAssignment(AssignmentStatement* assign);
    void evaluatePrint(PrintStatement* print);
    Value evaluateExpression(Expression* expression);
    int evaluateInputInt();
    std::string evaluateInputString();
    Value evaluateVariable(VariableReference* varRef);
    Value evaluateBinaryExpression(BinaryExpression* binExpr);

public:
    /**
     * Evaluates a program by executing all its statements.
     */
    void evaluate(ProgramNode* program);
};

#endif // EVALUATOR_H

