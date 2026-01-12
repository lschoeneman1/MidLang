#ifndef PARSER_H
#define PARSER_H

#include <vector>
#include <memory>
#include "Token.h"
#include "AST.h"

/**
 * Parser (Syntax Analyzer) - Stage 2
 * 
 * Purpose: Builds an Abstract Syntax Tree (AST) from tokens.
 * 
 * Stage 2 Extensions:
 * - Added STRING and CHAR literal parsing in parseFactor
 * 
 * Grammar (recall from EBNF):
 * Program = Statement { Statement }
 * Statement = AssignmentStatement | PrintStatement
 * AssignmentStatement = Identifier ASSIGN Expression SEMICOLON
 * PrintStatement = PRINT Expression SEMICOLON
 * Expression = Term { ("+" | "-") Term }
 * Term = Factor { ("*" | "/") Factor }
 * Factor = INTEGER | STRING | CHAR | Identifier | "(" Expression ")"
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

