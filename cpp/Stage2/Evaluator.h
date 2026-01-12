#ifndef EVALUATOR_H
#define EVALUATOR_H

#include <unordered_map>
#include <string>
#include <variant>
#include "AST.h"

/**
 * Evaluator (Interpreter) - Stage 2
 * 
 * Purpose: Executes the AST to produce program output.
 * 
 * Stage 2 Extensions:
 * - Symbol table now stores variant values (int or string)
 * - String concatenation with + operator
 * - Type conversion: int + string converts int to string
 * - Character literals are treated as single-character strings
 */
class Evaluator {
private:
    // Symbol table: stores variable names and their values
    // Values can be int or string (using std::variant)
    std::unordered_map<std::string, std::variant<int, std::string>> symbolTable;

    // Helper methods
    void evaluateStatement(Statement* statement);
    void evaluateAssignment(AssignmentStatement* assign);
    void evaluatePrint(PrintStatement* print);
    std::variant<int, std::string> evaluateExpression(Expression* expression);
    std::variant<int, std::string> evaluateVariable(VariableReference* varRef);
    std::variant<int, std::string> evaluateBinaryExpression(BinaryExpression* binExpr);
    std::string convertToString(const std::variant<int, std::string>& value);

public:
    /**
     * Evaluates a program by executing all its statements.
     */
    void evaluate(ProgramNode* program);
};

#endif // EVALUATOR_H

