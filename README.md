# MidLang - A Teaching Interpreter

This project is designed for teaching introductory programming language concepts. It implements a simple interpreter in both C# and C++, built incrementally in stages.

## Project Structure

```
MidLang/
├── docs/              # Documentation (EBNF, architecture, etc.)
├── csharp/            # C# implementation
│   └── Stage1/        # Stage 1: Basic integer expressions
├── cpp/               # C++ implementation
│   └── Stage1/        # Stage 1: Basic integer expressions
└── examples/          # Example MidLang programs
```

## Learning Objectives

Students will learn:
1. **Lexical Analysis** - How source code is broken into tokens
2. **Parsing** - How tokens are organized into an Abstract Syntax Tree (AST)
3. **Evaluation** - How the AST is executed to produce results
4. **Language Design** - How formal grammars (EBNF) define language syntax

## Stage 1: Basic Integer Expressions

**Features:**
- Integer literals (e.g., `42`, `-10`)
- Variables (e.g., `x`, `count`)
- Arithmetic expressions (`+`, `-`, `*`, `/`)
- Assignment statements (`x = 5`)
- Print statements (`print x`)

**Example Program:**
```
x = 10
y = 20
z = x + y
print z
```

## How to Use

### C# Implementation
```bash
cd csharp/Stage1
dotnet run examples/stage1_example1.mid
```

### C++ Implementation
```bash
cd cpp/Stage1
# Compile (instructions in cpp/Stage1/README.md)
./interpreter examples/stage1_example1.mid
```

## Interpreter Architecture

The interpreter consists of three main stages:

1. **Lexer (Tokenizer)**: Converts source code into a stream of tokens
2. **Parser**: Builds an Abstract Syntax Tree (AST) from tokens
3. **Evaluator**: Executes the AST and produces output

See `docs/Architecture.md` for detailed explanations.

