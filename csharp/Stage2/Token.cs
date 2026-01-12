namespace MidLang.Stage2
{
    /// <summary>
    /// Represents a token in the source code.
    /// Tokens are the smallest meaningful units of the language.
    /// </summary>
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int Line { get; }
        public int Column { get; }

        public Token(TokenType type, string value, int line, int column)
        {
            Type = type;
            Value = value;
            Line = line;
            Column = column;
        }

        public override string ToString() => $"{Type}({Value})";
    }

    /// <summary>
    /// Types of tokens in MidLang Stage 2.
    /// Extends Stage 1 with STRING and CHAR token types.
    /// </summary>
    public enum TokenType
    {
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
        PRINT,          // print

        // Special
        EOF,            // End of file
        UNKNOWN         // Invalid token
    }
}

