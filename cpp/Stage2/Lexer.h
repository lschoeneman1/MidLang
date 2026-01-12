#ifndef LEXER_H
#define LEXER_H

#include <vector>
#include <string>
#include "Token.h"

/**
 * Lexer (Lexical Analyzer / Tokenizer) - Stage 2
 * 
 * Purpose: Converts source code into a stream of tokens.
 * 
 * Stage 2 Extensions:
 * - Added string literal support (double quotes)
 * - Added character literal support (single quotes)
 * - Added escape sequence handling
 */
class Lexer {
private:
    std::string source;
    size_t position;  // Current position in source
    int line;         // Current line number
    int column;       // Current column number

    // Helper methods
    char peek();
    char advance();
    bool isAtEnd();
    void skipWhitespace();
    Token readNumber();
    Token readString();
    Token readChar();
    Token readIdentifier();
    Token nextToken();
    Token createToken(TokenType type);
    char processEscapeSequence(char escaped);

public:
    Lexer(const std::string& source);
    
    /**
     * Tokenizes the entire source code and returns all tokens.
     */
    std::vector<Token> tokenize();
};

#endif // LEXER_H

