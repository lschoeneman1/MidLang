using System;
using System.Collections.Generic;

namespace MidLang.Stage1
{
    /// <summary>
    /// Parser (Syntax Analyzer)
    /// 
    /// Purpose: Builds an Abstract Syntax Tree (AST) from tokens.
    /// 
    /// How it works:
    /// 1. Takes a list of tokens from the lexer
    /// 2. Uses recursive descent parsing
    /// 3. Verifies syntax matches the grammar
    /// 4. Builds AST nodes representing the program structure
    /// 
    /// Grammar (recall from EBNF):
    /// Program = Statement { Statement }
    /// Statement = AssignmentStatement | PrintStatement
    /// AssignmentStatement = Identifier ASSIGN Expression SEMICOLON
    /// PrintStatement = PRINT Expression SEMICOLON
    /// Expression = Term { ("+" | "-") Term }
    /// Term = Factor { ("*" | "/") Factor }
    /// Factor = INTEGER | Identifier | "(" Expression ")"
    /// </summary>
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _current;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _current = 0;
        }

        /// <summary>
        /// Parses the token stream and returns a Program AST node.
        /// </summary>
        public ProgramNode Parse()
        {
            var statements = new List<Statement>();

            while (!IsAtEnd())
            {
                statements.Add(ParseStatement());
            }

            return new ProgramNode(statements);
        }

        /// <summary>
        /// Parses a statement (var declaration, assignment, print, println, or if).
        /// Statement = VarDeclarationStatement | AssignmentStatement | PrintStatement | PrintLineStatement | IfStatement
        /// </summary>
        private Statement ParseStatement()
        {
            if (Match(TokenType.VAR))
            {
                return ParseVarDeclaration();
            }
            else if (Match(TokenType.PRINT))
            {
                return ParsePrintStatement();
            }
            else if (Match(TokenType.PRINTLN))
            {
                return ParsePrintLineStatement();
            }
            else if (Match(TokenType.IF))
            {
                return ParseIfStatement();
            }
            else
            {
                return ParseAssignmentStatement();
            }
        }

        /// <summary>
        /// Parses a variable declaration: var identifier = expression;
        /// VarDeclarationStatement = VAR Identifier ASSIGN Expression SEMICOLON
        /// </summary>
        private VarDeclarationStatement ParseVarDeclaration()
        {
            Token identifier = Consume(TokenType.IDENTIFIER, "Expected variable name after 'var'");
            Consume(TokenType.ASSIGN, "Expected '=' after variable name");
            Expression expression = ParseExpression();
            Consume(TokenType.SEMICOLON, "Expected ';' after expression");

            return new VarDeclarationStatement(identifier.Value, expression);
        }

        /// <summary>
        /// Parses an assignment statement: identifier = expression;
        /// AssignmentStatement = Identifier ASSIGN Expression SEMICOLON
        /// </summary>
        private AssignmentStatement ParseAssignmentStatement()
        {
            Token identifier = Consume(TokenType.IDENTIFIER, "Expected variable name");
            Consume(TokenType.ASSIGN, "Expected '=' after variable name");
            Expression expression = ParseExpression();
            Consume(TokenType.SEMICOLON, "Expected ';' after expression");

            return new AssignmentStatement(identifier.Value, expression);
        }

        /// <summary>
        /// Parses a print statement: print(expression);
        /// PrintStatement = PRINT LEFT_PAREN Expression RIGHT_PAREN SEMICOLON
        /// </summary>
        private PrintStatement ParsePrintStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expected '(' after 'print'");
            Expression expression = ParseExpression();
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression");
            Consume(TokenType.SEMICOLON, "Expected ';' after ')'");

            return new PrintStatement(expression);
        }

        /// <summary>
        /// Parses a print line statement: println(expression);
        /// PrintLineStatement = PRINTLN LEFT_PAREN Expression RIGHT_PAREN SEMICOLON
        /// </summary>
        private PrintLineStatement ParsePrintLineStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expected '(' after 'println'");
            Expression expression = ParseExpression();
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression");
            Consume(TokenType.SEMICOLON, "Expected ';' after ')'");

            return new PrintLineStatement(expression);
        }

        /// <summary>
        /// Parses an if statement: if (condition) { statements } [ else { statements } ]
        /// IfStatement = IF LEFT_PAREN BooleanExpression RIGHT_PAREN LEFT_BRACE Statement { Statement } RIGHT_BRACE [ ELSE LEFT_BRACE Statement { Statement } RIGHT_BRACE ]
        /// </summary>
        private IfStatement ParseIfStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expected '(' after 'if'");
            BooleanExpression condition = ParseBooleanExpression();
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after condition");
            Consume(TokenType.LEFT_BRACE, "Expected '{' after ')'");

            // Parse then block
            var thenStatements = new List<Statement>();
            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                thenStatements.Add(ParseStatement());
            }
            Consume(TokenType.RIGHT_BRACE, "Expected '}' after if block");

            // Parse optional else block
            List<Statement> elseStatements = null;
            if (Match(TokenType.ELSE))
            {
                Consume(TokenType.LEFT_BRACE, "Expected '{' after 'else'");
                elseStatements = new List<Statement>();
                while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
                {
                    elseStatements.Add(ParseStatement());
                }
                Consume(TokenType.RIGHT_BRACE, "Expected '}' after else block");
            }

            return new IfStatement(condition, thenStatements, elseStatements);
        }

        /// <summary>
        /// Parses a boolean expression: expression comparisonOperator expression
        /// BooleanExpression = Expression ComparisonOperator Expression
        /// </summary>
        private BooleanExpression ParseBooleanExpression()
        {
            Expression left = ParseExpression();

            // Check for comparison operator
            if (!Match(TokenType.EQUAL_EQUAL, TokenType.NOT_EQUAL, TokenType.LESS, TokenType.GREATER, TokenType.LESS_EQUAL, TokenType.GREATER_EQUAL))
            {
                throw new Exception($"Expected comparison operator (==, !=, <, >, <=, >=) at line {Peek().Line}, column {Peek().Column}");
            }

            string op = Previous().Value;
            Expression right = ParseExpression();

            return new BooleanExpression(left, op, right);
        }

        /// <summary>
        /// Parses an expression.
        /// Expression = Term { ("+" | "-") Term }
        /// This handles addition and subtraction (lowest precedence).
        /// </summary>
        private Expression ParseExpression()
        {
            Expression expr = ParseTerm();

            while (Match(TokenType.PLUS, TokenType.MINUS))
            {
                string op = Previous().Value;
                Expression right = ParseTerm();
                expr = new BinaryExpression(expr, op, right);
            }

            return expr;
        }

        /// <summary>
        /// Parses a term.
        /// Term = Factor { ("*" | "/") Factor }
        /// This handles multiplication and division (higher precedence).
        /// </summary>
        private Expression ParseTerm()
        {
            Expression expr = ParseFactor();

            while (Match(TokenType.MULTIPLY, TokenType.DIVIDE))
            {
                string op = Previous().Value;
                Expression right = ParseFactor();
                expr = new BinaryExpression(expr, op, right);
            }

            return expr;
        }

        /// <summary>
        /// Parses a factor (lowest level of expression).
        /// Factor = INTEGER | Identifier | "(" Expression ")" | INPUT_INT "()"
        /// </summary>
        private Expression ParseFactor()
        {
            if (Match(TokenType.INTEGER))
            {
                int value = int.Parse(Previous().Value);
                return new IntegerLiteral(value);
            }

            if (Match(TokenType.INPUT_INT))
            {
                Consume(TokenType.LEFT_PAREN, "Expected '(' after 'inputInt'");
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after '('");
                return new InputIntExpression();
            }

            if (Match(TokenType.IDENTIFIER))
            {
                return new VariableReference(Previous().Value);
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                Expression expr = ParseExpression();
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression");
                return expr;
            }

            throw new Exception($"Unexpected token: {Peek().Type} at line {Peek().Line}, column {Peek().Column}");
        }

        /// <summary>
        /// Checks if the current token matches any of the given types.
        /// If it matches, consumes the token.
        /// </summary>
        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the current token is of the given type.
        /// </summary>
        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        /// <summary>
        /// Consumes the current token and returns it.
        /// </summary>
        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        /// <summary>
        /// Returns true if we've consumed all tokens.
        /// </summary>
        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        /// <summary>
        /// Returns the current token without consuming it.
        /// </summary>
        private Token Peek()
        {
            return _tokens[_current];
        }

        /// <summary>
        /// Returns the previous token.
        /// </summary>
        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        /// <summary>
        /// Consumes a token of the expected type, or throws an error.
        /// </summary>
        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();

            Token token = Peek();
            throw new Exception($"{message} at line {token.Line}, column {token.Column}. Found: {token.Type}");
        }
    }
}

