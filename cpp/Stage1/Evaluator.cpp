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
    int value = evaluateExpression(assign->expression.get());
    symbolTable[assign->variableName] = value;
}

void Evaluator::evaluatePrint(PrintStatement* print) {
    int value = evaluateExpression(print->expression.get());
    std::cout << value << std::endl;
}

int Evaluator::evaluateExpression(Expression* expression) {
    if (auto* lit = dynamic_cast<IntegerLiteral*>(expression)) {
        return lit->value;
    } else if (auto* varRef = dynamic_cast<VariableReference*>(expression)) {
        return evaluateVariable(varRef);
    } else if (auto* binExpr = dynamic_cast<BinaryExpression*>(expression)) {
        return evaluateBinaryExpression(binExpr);
    } else {
        throw std::runtime_error("Unknown expression type");
    }
}

int Evaluator::evaluateVariable(VariableReference* varRef) {
    auto it = symbolTable.find(varRef->name);
    if (it == symbolTable.end()) {
        std::stringstream ss;
        ss << "Undefined variable: " << varRef->name;
        throw std::runtime_error(ss.str());
    }
    return it->second;
}

int Evaluator::evaluateBinaryExpression(BinaryExpression* binExpr) {
    int left = evaluateExpression(binExpr->left.get());
    int right = evaluateExpression(binExpr->right.get());

    if (binExpr->op == "+") {
        return left + right;
    } else if (binExpr->op == "-") {
        return left - right;
    } else if (binExpr->op == "*") {
        return left * right;
    } else if (binExpr->op == "/") {
        if (right == 0) {
            throw std::runtime_error("Division by zero");
        }
        return left / right;
    } else {
        std::stringstream ss;
        ss << "Unknown operator: " << binExpr->op;
        throw std::runtime_error(ss.str());
    }
}

