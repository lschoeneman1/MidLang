#include "Parser.h"
#include <stdexcept>
#include <sstream>

Parser::Parser(const std::vector<Token>& tokens)
    : tokens(tokens), current(0) {}

std::unique_ptr<ProgramNode> Parser::parse() {
    std::vector<std::unique_ptr<Statement>> statements;

    while (!isAtEnd()) {
        statements.push_back(parseStatement());
    }

    return std::make_unique<ProgramNode>(std::move(statements));
}

std::unique_ptr<Statement> Parser::parseStatement() {
    if (match(TokenType::VAR)) {
        return parseVarDeclaration();
    } else if (match(TokenType::PRINT)) {
        return parsePrintStatement();
    } else {
        return parseAssignmentStatement();
    }
}

std::unique_ptr<VarDeclarationStatement> Parser::parseVarDeclaration() {
    Token identifier = consume(TokenType::IDENTIFIER, "Expected variable name after 'var'");
    consume(TokenType::ASSIGN, "Expected '=' after variable name");
    auto expression = parseExpression();
    consume(TokenType::SEMICOLON, "Expected ';' after expression");

    return std::make_unique<VarDeclarationStatement>(identifier.value, std::move(expression));
}

std::unique_ptr<AssignmentStatement> Parser::parseAssignmentStatement() {
    Token identifier = consume(TokenType::IDENTIFIER, "Expected variable name");
    consume(TokenType::ASSIGN, "Expected '=' after variable name");
    auto expression = parseExpression();
    consume(TokenType::SEMICOLON, "Expected ';' after expression");

    return std::make_unique<AssignmentStatement>(identifier.value, std::move(expression));
}

std::unique_ptr<PrintStatement> Parser::parsePrintStatement() {
    consume(TokenType::LEFT_PAREN, "Expected '(' after 'print'");
    auto expression = parseExpression();
    consume(TokenType::RIGHT_PAREN, "Expected ')' after expression");
    consume(TokenType::SEMICOLON, "Expected ';' after ')'");

    return std::make_unique<PrintStatement>(std::move(expression));
}

std::unique_ptr<Expression> Parser::parseExpression() {
    auto expr = parseTerm();

    while (match(TokenType::PLUS) || match(TokenType::MINUS)) {
        std::string op = previous().value;
        auto right = parseTerm();
        expr = std::make_unique<BinaryExpression>(std::move(expr), op, std::move(right));
    }

    return expr;
}

std::unique_ptr<Expression> Parser::parseTerm() {
    auto expr = parseFactor();

    while (match(TokenType::MULTIPLY) || match(TokenType::DIVIDE)) {
        std::string op = previous().value;
        auto right = parseFactor();
        expr = std::make_unique<BinaryExpression>(std::move(expr), op, std::move(right));
    }

    return expr;
}

std::unique_ptr<Expression> Parser::parseFactor() {
    if (match(TokenType::INTEGER)) {
        int value = std::stoi(previous().value);
        return std::make_unique<IntegerLiteral>(value);
    }

    if (match(TokenType::STRING)) {
        return std::make_unique<StringLiteral>(previous().value);
    }

    if (match(TokenType::CHAR)) {
        char value = previous().value[0];
        return std::make_unique<CharLiteral>(value);
    }

    if (match(TokenType::INPUT_INT)) {
        consume(TokenType::LEFT_PAREN, "Expected '(' after 'inputInt'");
        consume(TokenType::RIGHT_PAREN, "Expected ')' after '('");
        return std::make_unique<InputIntExpression>();
    }

    if (match(TokenType::INPUT_STRING)) {
        consume(TokenType::LEFT_PAREN, "Expected '(' after 'inputString'");
        consume(TokenType::RIGHT_PAREN, "Expected ')' after '('");
        return std::make_unique<InputStringExpression>();
    }

    if (match(TokenType::IDENTIFIER)) {
        return std::make_unique<VariableReference>(previous().value);
    }

    if (match(TokenType::LEFT_PAREN)) {
        auto expr = parseExpression();
        consume(TokenType::RIGHT_PAREN, "Expected ')' after expression");
        return expr;
    }

    std::stringstream ss;
    ss << "Unexpected token: " << static_cast<int>(peek().type)
       << " at line " << peek().line << ", column " << peek().column;
    throw std::runtime_error(ss.str());
}

bool Parser::match(TokenType type) {
    if (check(type)) {
        advance();
        return true;
    }
    return false;
}

bool Parser::match(TokenType type1, TokenType type2) {
    return match(type1) || match(type2);
}

bool Parser::check(TokenType type) {
    if (isAtEnd()) return false;
    return peek().type == type;
}

Token Parser::advance() {
    if (!isAtEnd()) current++;
    return previous();
}

bool Parser::isAtEnd() {
    return peek().type == TokenType::EOF_TOKEN;
}

Token Parser::peek() {
    return tokens[current];
}

Token Parser::previous() {
    return tokens[current - 1];
}

Token Parser::consume(TokenType type, const std::string& message) {
    if (check(type)) return advance();

    Token token = peek();
    std::stringstream ss;
    ss << message << " at line " << token.line << ", column " << token.column
       << ". Found: " << static_cast<int>(token.type);
    throw std::runtime_error(ss.str());
}

