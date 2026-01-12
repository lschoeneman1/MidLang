#include <iostream>
#include <fstream>
#include <sstream>
#include <stdexcept>
#include "Lexer.h"
#include "Parser.h"
#include "Evaluator.h"

/**
 * Main entry point for the MidLang Stage 1 interpreter.
 * 
 * This program demonstrates the three-stage interpreter architecture:
 * 1. Lexer: Converts source code to tokens
 * 2. Parser: Builds AST from tokens
 * 3. Evaluator: Executes AST
 */
int main(int argc, char* argv[]) {
    if (argc < 2) {
        std::cout << "Usage: interpreter <source_file.mid>" << std::endl;
        std::cout << "Example: interpreter examples/program.mid" << std::endl;
        return 1;
    }

    std::string sourceFile = argv[1];

    std::ifstream file(sourceFile);
    if (!file.is_open()) {
        std::cerr << "Error: File not found: " << sourceFile << std::endl;
        return 1;
    }

    try {
        // Read source code
        std::stringstream buffer;
        buffer << file.rdbuf();
        std::string sourceCode = buffer.str();
        file.close();

        std::cout << "=== Interpreting: " << sourceFile << " ===" << std::endl << std::endl;

        // Stage 1: Lexical Analysis
        std::cout << "Stage 1: Lexical Analysis (Tokenization)" << std::endl;
        Lexer lexer(sourceCode);
        auto tokens = lexer.tokenize();
        std::cout << "Generated " << tokens.size() << " tokens:" << std::endl;
        for (const auto& token : tokens) {
            if (token.type != TokenType::EOF_TOKEN) {
                // Convert token type to string for readability
                std::string typeStr;
                switch (token.type) {
                    case TokenType::INTEGER: typeStr = "INTEGER"; break;
                    case TokenType::IDENTIFIER: typeStr = "IDENTIFIER"; break;
                    case TokenType::PLUS: typeStr = "PLUS"; break;
                    case TokenType::MINUS: typeStr = "MINUS"; break;
                    case TokenType::MULTIPLY: typeStr = "MULTIPLY"; break;
                    case TokenType::DIVIDE: typeStr = "DIVIDE"; break;
                    case TokenType::ASSIGN: typeStr = "ASSIGN"; break;
                    case TokenType::SEMICOLON: typeStr = "SEMICOLON"; break;
                    case TokenType::LEFT_PAREN: typeStr = "LEFT_PAREN"; break;
                    case TokenType::RIGHT_PAREN: typeStr = "RIGHT_PAREN"; break;
                    case TokenType::PRINT: typeStr = "PRINT"; break;
                    default: typeStr = "UNKNOWN"; break;
                }
                std::cout << "  " << typeStr << "(" << token.value << ")" << std::endl;
            }
        }
        std::cout << std::endl;

        // Stage 2: Parsing
        std::cout << "Stage 2: Parsing (Building AST)" << std::endl;
        Parser parser(tokens);
        auto ast = parser.parse();
        std::cout << "Parsed " << ast->statements.size() << " statement(s)" << std::endl;
        std::cout << std::endl;

        // Stage 3: Evaluation
        std::cout << "Stage 3: Evaluation (Execution)" << std::endl;
        std::cout << "Output:" << std::endl;
        Evaluator evaluator;
        evaluator.evaluate(ast.get());
        std::cout << std::endl;

        std::cout << "=== Program completed successfully ===" << std::endl;
    } catch (const std::exception& ex) {
        std::cerr << "Error: " << ex.what() << std::endl;
        return 1;
    }

    return 0;
}

