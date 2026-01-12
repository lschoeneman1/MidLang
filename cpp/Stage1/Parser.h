#ifndef PARSER_H
#define PARSER_H

#include <vector>
#include <memory>
#include "Token.h"
#include "AST.h"

/**
 * Parser (Syntax Analyzer)
 * 
 * Purpose: Builds an Abstract Syntax Tree (AST) from tokens.
 * 
 * How it works:
 * 1. Takes a list of tokens from the lexer
 * 2. Uses recursive descent parsing
 * 3. Verifies syntax matches the grammar
 * 4. Builds AST nodes representing the program structure
 * 
 * Grammar (recall from EBNF):
 * Program = Statement { Statement }
 * Statement = AssignmentStatement | PrintStatement
 * AssignmentStatement = Identifier ASSIGN Expression SEMICOLON
 * PrintStatement = PRINT Expression SEMICOLON
 * Expression = Term { ("+" | "-") Term }
 * Term = Factor { ("*" | "/") Factor }
 * Factor = INTEGER | Identifier | "(" Expression ")"
 */
class Parser {
private:
    std::vector<Token> tokens;
    size_t current;

    // Helper methods
    bool match(TokenType type);
    bool match(TokenType type1, TokenType type2);
    bool check(TokenType type);
    Token advance();
    bool isAtEnd();
    Token peek();
    Token previous();
    Token consume(TokenType type, const std::string& message);

    // Parsing methods
    std::unique_ptr<Statement> parseStatement();
    std::unique_ptr<AssignmentStatement> parseAssignmentStatement();
    std::unique_ptr<PrintStatement> parsePrintStatement();
    std::unique_ptr<Expression> parseExpression();
    std::unique_ptr<Expression> parseTerm();
    std::unique_ptr<Expression> parseFactor();

public:
    Parser(const std::vector<Token>& tokens);
    
    /**
     * Parses the token stream and returns a Program AST node.
     */
    std::unique_ptr<ProgramNode> parse();
};

#endif // PARSER_H

