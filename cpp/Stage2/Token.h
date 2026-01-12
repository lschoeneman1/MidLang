#ifndef TOKEN_H
#define TOKEN_H

#include <string>

/**
 * TokenType - Types of tokens in MidLang Stage 2
 * Extends Stage 1 with STRING and CHAR token types.
 */
enum class TokenType {
    // Literals
    INTEGER,        // e.g., 42, -10
    STRING,         // e.g., "hello", "world"
    CHAR,           // e.g., 'a', 'Z'
    IDENTIFIER,     // e.g., x, count, myVar

    // Operators
    PLUS,           // +
    MINUS,          // -
    MULTIPLY,       // *
    DIVIDE,         // /

    // Punctuation
    ASSIGN,         // =
    SEMICOLON,      // ;
    LEFT_PAREN,     // (
    RIGHT_PAREN,    // )

    // Keywords
    VAR,            // var
    PRINT,          // print
    INPUT_INT,      // inputInt
    INPUT_STRING,   // inputString

    // Special
    EOF_TOKEN,      // End of file
    UNKNOWN         // Invalid token
};

/**
 * Token - Represents a token in the source code.
 * 
 * Tokens are the smallest meaningful units of the language.
 * Each token has:
 * - A type (what kind of token it is)
 * - A value (the actual text)
 * - Position information (line and column for error reporting)
 */
class Token {
public:
    TokenType type;
    std::string value;
    int line;
    int column;

    Token(TokenType t, const std::string& v, int l, int c)
        : type(t), value(v), line(l), column(c) {}
};

#endif // TOKEN_H

