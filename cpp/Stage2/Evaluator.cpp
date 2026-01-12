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
    auto value = evaluateExpression(assign->expression.get());
    symbolTable[assign->variableName] = value;
}

void Evaluator::evaluatePrint(PrintStatement* print) {
    auto value = evaluateExpression(print->expression.get());
    std::cout << convertToString(value) << std::endl;
}

std::variant<int, std::string> Evaluator::evaluateExpression(Expression* expression) {
    if (auto* lit = dynamic_cast<IntegerLiteral*>(expression)) {
        return lit->value;
    } else if (auto* strLit = dynamic_cast<StringLiteral*>(expression)) {
        return strLit->value;
    } else if (auto* charLit = dynamic_cast<CharLiteral*>(expression)) {
        return std::string(1, charLit->value); // Convert char to string
    } else if (auto* varRef = dynamic_cast<VariableReference*>(expression)) {
        return evaluateVariable(varRef);
    } else if (auto* binExpr = dynamic_cast<BinaryExpression*>(expression)) {
        return evaluateBinaryExpression(binExpr);
    } else {
        throw std::runtime_error("Unknown expression type");
    }
}

std::variant<int, std::string> Evaluator::evaluateVariable(VariableReference* varRef) {
    auto it = symbolTable.find(varRef->name);
    if (it == symbolTable.end()) {
        std::stringstream ss;
        ss << "Undefined variable: " << varRef->name;
        throw std::runtime_error(ss.str());
    }
    return it->second;
}

std::variant<int, std::string> Evaluator::evaluateBinaryExpression(BinaryExpression* binExpr) {
    auto leftObj = evaluateExpression(binExpr->left.get());
    auto rightObj = evaluateExpression(binExpr->right.get());

    // String concatenation
    if (binExpr->op == "+") {
        std::string leftStr = convertToString(leftObj);
        std::string rightStr = convertToString(rightObj);
        return leftStr + rightStr;
    }

    // For other operators, both operands must be integers
    int left, right;
    try {
        left = std::get<int>(leftObj);
        right = std::get<int>(rightObj);
    } catch (const std::bad_variant_access&) {
        std::stringstream ss;
        ss << "Operator '" << binExpr->op << "' requires integer operands";
        throw std::runtime_error(ss.str());
    }

    if (binExpr->op == "-") {
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

std::string Evaluator::convertToString(const std::variant<int, std::string>& value) {
    if (std::holds_alternative<int>(value)) {
        return std::to_string(std::get<int>(value));
    } else {
        return std::get<std::string>(value);
    }
}

