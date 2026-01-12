#include "Evaluator.h"
#include <iostream>
#include <stdexcept>
#include <sstream>

void Evaluator::evaluate(ProgramNode* program) {
    for (auto& statement : program->statements) {
        evaluateStatement(statement.get());
    }
}

void Evaluator::evaluateStatement(Statement* statement) {
    if (auto* assign = dynamic_cast<AssignmentStatement*>(statement)) {
        evaluateAssignment(assign);
    } else if (auto* print = dynamic_cast<PrintStatement*>(statement)) {
        evaluatePrint(print);
    } else {
        throw std::runtime_error("Unknown statement type");
    }
}

void Evaluator::evaluateAssignment(AssignmentStatement* assign) {
    Value value = evaluateExpression(assign->expression.get());
    symbolTable[assign->variableName] = value;
}

void Evaluator::evaluatePrint(PrintStatement* print) {
    Value value = evaluateExpression(print->expression.get());
    std::cout << value.toString() << std::endl;
}

Value Evaluator::evaluateExpression(Expression* expression) {
    if (auto* lit = dynamic_cast<IntegerLiteral*>(expression)) {
        return Value(lit->value);
    } else if (auto* strLit = dynamic_cast<StringLiteral*>(expression)) {
        return Value(strLit->value);
    } else if (auto* charLit = dynamic_cast<CharLiteral*>(expression)) {
        // Convert char to string
        return Value(std::string(1, charLit->value));
    } else if (auto* varRef = dynamic_cast<VariableReference*>(expression)) {
        return evaluateVariable(varRef);
    } else if (auto* binExpr = dynamic_cast<BinaryExpression*>(expression)) {
        return evaluateBinaryExpression(binExpr);
    } else {
        throw std::runtime_error("Unknown expression type");
    }
}

Value Evaluator::evaluateVariable(VariableReference* varRef) {
    auto it = symbolTable.find(varRef->name);
    if (it == symbolTable.end()) {
        std::stringstream ss;
        ss << "Undefined variable: " << varRef->name;
        throw std::runtime_error(ss.str());
    }
    return it->second;
}

Value Evaluator::evaluateBinaryExpression(BinaryExpression* binExpr) {
    Value left = evaluateExpression(binExpr->left.get());
    Value right = evaluateExpression(binExpr->right.get());

    // String concatenation
    if (binExpr->op == "+") {
        std::string leftStr = left.toString();
        std::string rightStr = right.toString();
        return Value(leftStr + rightStr);
    }

    // For other operators, both operands must be integers
    if (!left.isInt() || !right.isInt()) {
        std::stringstream ss;
        ss << "Operator '" << binExpr->op << "' requires integer operands";
        throw std::runtime_error(ss.str());
    }

    int leftInt = left.getInt();
    int rightInt = right.getInt();

    if (binExpr->op == "-") {
        return Value(leftInt - rightInt);
    } else if (binExpr->op == "*") {
        return Value(leftInt * rightInt);
    } else if (binExpr->op == "/") {
        if (rightInt == 0) {
            throw std::runtime_error("Division by zero");
        }
        return Value(leftInt / rightInt);
    } else {
        std::stringstream ss;
        ss << "Unknown operator: " << binExpr->op;
        throw std::runtime_error(ss.str());
    }
}

