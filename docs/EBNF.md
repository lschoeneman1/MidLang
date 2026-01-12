# EBNF Grammar for MidLang

This document defines the formal grammar for MidLang using Extended Backus-Naur Form (EBNF).

## Stage 1: Basic Integer Expressions

[See Stage 1 section below for the original grammar]

## Stage 2: Adding Strings and Characters

Stage 2 extends Stage 1 with string and character literal support.

### Additional Tokens for Stage 2

```
STRING      = '"' { ANY_CHAR_EXCEPT_DOUBLE_QUOTE | ESCAPE_SEQUENCE } '"'
CHAR        = "'" ( ANY_CHAR_EXCEPT_SINGLE_QUOTE | ESCAPE_SEQUENCE ) "'"
ESCAPE_SEQUENCE = "\\" ( "n" | "t" | "\\" | '"' | "'" )
```

### Updated Grammar Rules for Stage 2

```
Factor      = INTEGER
            | STRING
            | CHAR
            | Identifier
            | "(" Expression ")"
```

### String Operations

- **Concatenation**: Strings can be concatenated using the `+` operator
- **Type Mixing**: Integer + String converts integer to string and concatenates
- **Character Access**: Characters are single-character strings

### Examples for Stage 2

```
name = "Alice";
print name;

greeting = "Hello, " + name;
print greeting;

letter = 'A';
print letter;

message = "The answer is " + 42;
print message;
```

---

## Stage 1: Basic Integer Expressions

### Tokens (Terminal Symbols)

```
DIGIT       = "0" | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9"
LETTER      = "a" | "b" | ... | "z" | "A" | "B" | ... | "Z"
IDENTIFIER  = LETTER { LETTER | DIGIT | "_" }
INTEGER     = ["-"] DIGIT { DIGIT }
OPERATOR   = "+" | "-" | "*" | "/"
ASSIGN      = "="
PRINT       = "print"
SEMICOLON   = ";"
NEWLINE     = "\n"
WHITESPACE  = " " | "\t"
```

### Grammar Rules (Non-Terminal Symbols)

```
Program     = Statement { Statement }

Statement   = AssignmentStatement
            | PrintStatement

AssignmentStatement = Identifier ASSIGN Expression SEMICOLON

PrintStatement = PRINT Expression SEMICOLON

Expression  = Term { ("+" | "-") Term }

Term        = Factor { ("*" | "/") Factor }

Factor      = INTEGER
            | Identifier
            | "(" Expression ")"

Identifier  = IDENTIFIER
```

### Operator Precedence

The grammar enforces operator precedence:
- **Highest**: Parentheses `()`
- **High**: Multiplication `*`, Division `/`
- **Low**: Addition `+`, Subtraction `-`

### Notes

- **Negative Numbers**: In Stage 1, negative numbers are not handled as literals. Use `0 - 10` to represent `-10`. Unary minus can be added in later stages.
- **Whitespace**: Spaces, tabs, and newlines are ignored except as separators.
- **Semicolons**: Required after each statement.

### Examples

**Valid Programs:**
```
x = 10;
print x;
```

```
a = 5;
b = 3;
c = a + b * 2;
print c;
```

```
x = (10 + 5) * 2;
print x;
```

```
// Negative number workaround
x = 0 - 10;
print x;
```

**Invalid Programs:**
```
x = ;           // Missing expression
print;          // Missing expression
x = 5 +;        // Incomplete expression
x = -10;        // Unary minus not supported in Stage 1
```

