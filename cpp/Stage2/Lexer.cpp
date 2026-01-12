#include "Lexer.h"
#include <cctype>
#include <sstream>

Lexer::Lexer(const std::string& source)
    : source(source), position(0), line(1), column(1) {}

std::vector<Token> Lexer::tokenize() {
    std::vector<Token> tokens;

    while (!isAtEnd()) {
        skipWhitespace();
        if (isAtEnd()) break;

        Token token = nextToken();
        tokens.push_back(token);

        // Stop if we hit an error token
        if (token.type == TokenType::UNKNOWN) {
            break;
        }
    }

    // Add EOF token at the end
    tokens.push_back(Token(TokenType::EOF_TOKEN, "", line, column));
    return tokens;
}

Token Lexer::nextToken() {
    char current = advance();

    // Single character tokens
    switch (current) {
        case '+': return createToken(TokenType::PLUS);
        case '-': return createToken(TokenType::MINUS);
        case '*': return createToken(TokenType::MULTIPLY);
        case '/': return createToken(TokenType::DIVIDE);
        case '=': return createToken(TokenType::ASSIGN);
        case ';': return createToken(TokenType::SEMICOLON);
        case '(': return createToken(TokenType::LEFT_PAREN);
        case ')': return createToken(TokenType::RIGHT_PAREN);
        case '"': return readString();  // String literal
        case '\'': return readChar();   // Character literal
    }

    // Numbers (integers)
    if (std::isdigit(current)) {
        return readNumber();
    }

    // Identifiers and keywords
    if (std::isalpha(current) || current == '_') {
        return readIdentifier();
    }

    // Unknown character
    return Token(TokenType::UNKNOWN, std::string(1, current), line, column);
}

Token Lexer::readString() {
    int startColumn = column - 1;
    std::stringstream str;

    while (!isAtEnd() && peek() != '"') {
        if (peek() == '\\') {
            advance(); // Consume backslash
            if (isAtEnd()) {
                return Token(TokenType::UNKNOWN, "Unterminated string", line, startColumn);
            }
            char escaped = advance();
            str << processEscapeSequence(escaped);
        } else if (peek() == '\n') {
            return Token(TokenType::UNKNOWN, "Unterminated string (newline in string)", line, startColumn);
        } else {
            str << advance();
        }
    }

    if (isAtEnd()) {
        return Token(TokenType::UNKNOWN, "Unterminated string", line, startColumn);
    }

    advance(); // Consume closing quote
    return Token(TokenType::STRING, str.str(), line, startColumn);
}

Token Lexer::readChar() {
    int startColumn = column - 1;

    if (isAtEnd()) {
        return Token(TokenType::UNKNOWN, "Unterminated character", line, startColumn);
    }

    char ch;
    if (peek() == '\\') {
        advance(); // Consume backslash
        if (isAtEnd()) {
            return Token(TokenType::UNKNOWN, "Unterminated character", line, startColumn);
        }
        char escaped = advance();
        ch = processEscapeSequence(escaped);
    } else {
        ch = advance();
    }

    if (isAtEnd() || peek() != '\'') {
        return Token(TokenType::UNKNOWN, "Unterminated character", line, startColumn);
    }

    advance(); // Consume closing quote
    return Token(TokenType::CHAR, std::string(1, ch), line, startColumn);
}

char Lexer::processEscapeSequence(char escaped) {
    switch (escaped) {
        case 'n': return '\n';
        case 't': return '\t';
        case '\\': return '\\';
        case '"': return '"';
        case '\'': return '\'';
        default: return escaped; // Unknown escape, return as-is
    }
}

Token Lexer::readNumber() {
    int startColumn = column - 1;
    std::stringstream number;

    // Read digits (we've already consumed the first digit)
    number << source[position - 1];

    // Read remaining digits
    while (!isAtEnd() && std::isdigit(peek())) {
        number << advance();
    }

    return Token(TokenType::INTEGER, number.str(), line, startColumn);
}

Token Lexer::readIdentifier() {
    int startColumn = column - 1;
    std::stringstream identifier;

    // Read first character (already consumed)
    identifier << source[position - 1];

    // Read remaining letters, digits, and underscores
    while (!isAtEnd() && (std::isalnum(peek()) || peek() == '_')) {
        identifier << advance();
    }

    std::string value = identifier.str();

    // Check if it's a keyword
    TokenType type;
    if (value == "var") {
        type = TokenType::VAR;
    } else if (value == "print") {
        type = TokenType::PRINT;
    } else if (value == "println") {
        type = TokenType::PRINTLN;
    } else if (value == "inputInt") {
        type = TokenType::INPUT_INT;
    } else if (value == "inputString") {
        type = TokenType::INPUT_STRING;
    } else {
        type = TokenType::IDENTIFIER;
    }

    return Token(type, value, line, startColumn);
}

void Lexer::skipWhitespace() {
    while (!isAtEnd()) {
        char c = peek();
        if (c == ' ' || c == '\t') {
            advance();
        } else if (c == '\r') {
            // Handle Windows line endings (\r\n)
            advance();
            if (!isAtEnd() && peek() == '\n') {
                advance(); // Skip the \n after \r
            }
            line++;
            column = 1;
        } else if (c == '\n') {
            // Handle Unix line endings (\n)
            advance();
            line++;
            column = 1;
        } else {
            break;
        }
    }
}

char Lexer::peek() {
    if (isAtEnd()) return '\0';
    return source[position];
}

char Lexer::advance() {
    if (isAtEnd()) return '\0';
    column++;
    return source[position++];
}

bool Lexer::isAtEnd() {
    return position >= source.length();
}

Token Lexer::createToken(TokenType type) {
    std::string value(1, source[position - 1]);
    return Token(type, value, line, column - 1);
}

