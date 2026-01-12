# MidLang Stage 1 - C# Implementation

## Overview

This is the C# implementation of MidLang Stage 1: Basic Integer Expressions.

## Files

- **Token.cs**: Defines token types and the Token class
- **Lexer.cs**: Converts source code into tokens
- **AST.cs**: Defines Abstract Syntax Tree node classes
- **Parser.cs**: Builds AST from tokens
- **Evaluator.cs**: Executes the AST
- **Program.cs**: Main entry point

## Building

```bash
cd csharp/Stage1
dotnet build
```

## Running

```bash
# From csharp/Stage1 directory
dotnet run ../../examples/stage1_example1.mid

# Or after building
dotnet bin/Debug/net6.0/MidLang.Stage1.dll ../../examples/stage1_example1.mid
```

## How It Works

1. **Lexer** reads the source file and breaks it into tokens
2. **Parser** builds an AST from the tokens
3. **Evaluator** executes the AST and produces output

See `../../docs/Architecture.md` for detailed explanations.

## Example

**Input file (`program.mid`):**
```
x = 10;
y = 20;
z = x + y;
print z;
```

**Output:**
```
=== Interpreting: program.mid ===

Stage 1: Lexical Analysis (Tokenization)
Generated 13 tokens:
  IDENTIFIER(x)
  ASSIGN(=)
  INTEGER(10)
  SEMICOLON(;)
  IDENTIFIER(y)
  ASSIGN(=)
  INTEGER(20)
  SEMICOLON(;)
  IDENTIFIER(z)
  ASSIGN(=)
  IDENTIFIER(x)
  PLUS(+)
  IDENTIFIER(y)
  SEMICOLON(;)
  PRINT(print)
  IDENTIFIER(z)
  SEMICOLON(;)

Stage 2: Parsing (Building AST)
Parsed 4 statement(s)

Stage 3: Evaluation (Execution)
Output:
30

=== Program completed successfully ===
```

