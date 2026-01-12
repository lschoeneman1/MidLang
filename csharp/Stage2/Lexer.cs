using System;
using System.Collections.Generic;
using System.Text;

namespace MidLang.Stage2
{
    /// <summary>
    /// Lexer (Lexical Analyzer / Tokenizer) - Stage 2
    /// 
    /// Purpose: Converts source code into a stream of tokens.
    /// 
    /// Stage 2 Extensions:
    /// - Added string literal support (double quotes)
    /// - Added character literal support (single quotes)
    /// - Added escape sequence handling
    /// </summary>
    public class Lexer
    {
        private readonly string _source;
        private int _position;  // Current position in source
        private int _line;      // Current line number
        private int _column;    // Current column number

        public Lexer(string source)
        {
            _source = source;
            _position = 0;
            _line = 1;
            _column = 1;
        }

        /// <summary>
        /// Tokenizes the entire source code and returns all tokens.
        /// </summary>
        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            while (!IsAtEnd())
            {
                SkipWhitespace();
                if (IsAtEnd()) break;

                Token token = NextToken();
                tokens.Add(token);

                // Stop if we hit an error token
                if (token.Type == TokenType.UNKNOWN)
                {
                    break;
                }
            }

            // Add EOF token at the end
            tokens.Add(new Token(TokenType.EOF, "", _line, _column));
            return tokens;
        }

        /// <summary>
        /// Reads and returns the next token from the source.
        /// </summary>
        private Token NextToken()
        {
            char current = Advance();

            // Single character tokens
            switch (current)
            {
                case '+': return CreateToken(TokenType.PLUS);
                case '-': return CreateToken(TokenType.MINUS);
                case '*': return CreateToken(TokenType.MULTIPLY);
                case '/': return CreateToken(TokenType.DIVIDE);
                case '=': return CreateToken(TokenType.ASSIGN);
                case ';': return CreateToken(TokenType.SEMICOLON);
                case '(': return CreateToken(TokenType.LEFT_PAREN);
                case ')': return CreateToken(TokenType.RIGHT_PAREN);
                case '"': return ReadString();  // String literal
                case '\'': return ReadChar();   // Character literal
            }

            // Numbers (integers)
            if (char.IsDigit(current))
            {
                return ReadNumber();
            }

            // Identifiers and keywords
            if (char.IsLetter(current) || current == '_')
            {
                return ReadIdentifier();
            }

            // Unknown character
            return new Token(TokenType.UNKNOWN, current.ToString(), _line, _column);
        }

        /// <summary>
        /// Reads a string literal: "text"
        /// Handles escape sequences: \n, \t, \\, \", \'
        /// </summary>
        private Token ReadString()
        {
            int startColumn = _column - 1;
            StringBuilder str = new StringBuilder();

            while (!IsAtEnd() && Peek() != '"')
            {
                if (Peek() == '\\')
                {
                    Advance(); // Consume backslash
                    if (IsAtEnd())
                    {
                        return new Token(TokenType.UNKNOWN, "Unterminated string", _line, startColumn);
                    }
                    char escaped = Advance();
                    str.Append(ProcessEscapeSequence(escaped));
                }
                else if (Peek() == '\n')
                {
                    return new Token(TokenType.UNKNOWN, "Unterminated string (newline in string)", _line, startColumn);
                }
                else
                {
                    str.Append(Advance());
                }
            }

            if (IsAtEnd())
            {
                return new Token(TokenType.UNKNOWN, "Unterminated string", _line, startColumn);
            }

            Advance(); // Consume closing quote
            return new Token(TokenType.STRING, str.ToString(), _line, startColumn);
        }

        /// <summary>
        /// Reads a character literal: 'a'
        /// Handles escape sequences: \n, \t, \\, \", \'
        /// </summary>
        private Token ReadChar()
        {
            int startColumn = _column - 1;

            if (IsAtEnd())
            {
                return new Token(TokenType.UNKNOWN, "Unterminated character", _line, startColumn);
            }

            char ch;
            if (Peek() == '\\')
            {
                Advance(); // Consume backslash
                if (IsAtEnd())
                {
                    return new Token(TokenType.UNKNOWN, "Unterminated character", _line, startColumn);
                }
                char escaped = Advance();
                ch = ProcessEscapeSequence(escaped);
            }
            else
            {
                ch = Advance();
            }

            if (IsAtEnd() || Peek() != '\'')
            {
                return new Token(TokenType.UNKNOWN, "Unterminated character", _line, startColumn);
            }

            Advance(); // Consume closing quote
            return new Token(TokenType.CHAR, ch.ToString(), _line, startColumn);
        }

        /// <summary>
        /// Processes escape sequences: \n -> newline, \t -> tab, etc.
        /// </summary>
        private char ProcessEscapeSequence(char escaped)
        {
            return escaped switch
            {
                'n' => '\n',
                't' => '\t',
                '\\' => '\\',
                '"' => '"',
                '\'' => '\'',
                _ => escaped // Unknown escape, return as-is
            };
        }

        /// <summary>
        /// Reads an integer literal (e.g., 42).
        /// </summary>
        private Token ReadNumber()
        {
            int startColumn = _column - 1;
            StringBuilder number = new StringBuilder();

            // Read digits (we've already consumed the first digit)
            number.Append(_source[_position - 1]);

            // Read remaining digits
            while (!IsAtEnd() && char.IsDigit(Peek()))
            {
                number.Append(Advance());
            }

            return new Token(TokenType.INTEGER, number.ToString(), _line, startColumn);
        }

        /// <summary>
        /// Reads an identifier or keyword (e.g., x, print, myVar).
        /// </summary>
        private Token ReadIdentifier()
        {
            int startColumn = _column - 1;
            StringBuilder identifier = new StringBuilder();

            // Read first character (already consumed)
            identifier.Append(_source[_position - 1]);

            // Read remaining letters, digits, and underscores
            while (!IsAtEnd() && (char.IsLetterOrDigit(Peek()) || Peek() == '_'))
            {
                identifier.Append(Advance());
            }

            string value = identifier.ToString();

            // Check if it's a keyword
            TokenType type = value switch
            {
                "var" => TokenType.VAR,
                "print" => TokenType.PRINT,
                "inputInt" => TokenType.INPUT_INT,
                "inputString" => TokenType.INPUT_STRING,
                _ => TokenType.IDENTIFIER
            };

            return new Token(type, value, _line, startColumn);
        }

        /// <summary>
        /// Skips whitespace characters (spaces, tabs, newlines).
        /// </summary>
        private void SkipWhitespace()
        {
            while (!IsAtEnd())
            {
                char c = Peek();
                if (c == ' ' || c == '\t')
                {
                    Advance();
                }
                else if (c == '\n')
                {
                    Advance();
                    _line++;
                    _column = 1;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Returns the current character without consuming it.
        /// </summary>
        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_position];
        }

        /// <summary>
        /// Consumes and returns the current character.
        /// </summary>
        private char Advance()
        {
            if (IsAtEnd()) return '\0';
            _column++;
            return _source[_position++];
        }

        /// <summary>
        /// Checks if we've reached the end of the source.
        /// </summary>
        private bool IsAtEnd()
        {
            return _position >= _source.Length;
        }

        /// <summary>
        /// Creates a token for the current position.
        /// </summary>
        private Token CreateToken(TokenType type)
        {
            string value = _source[_position - 1].ToString();
            return new Token(type, value, _line, _column - 1);
        }
    }
}

