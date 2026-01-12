# MidLang Stage 2 - C++ Implementation

## Overview

This is the C++ implementation of MidLang Stage 2: Adding Strings and Characters.

Stage 2 extends Stage 1 with:
- String literals (e.g., `"hello"`, `"world"`)
- Character literals (e.g., `'a'`, `'Z'`)
- String concatenation with `+` operator
- Automatic type conversion (int + string converts int to string)
- Escape sequences in strings and characters (`\n`, `\t`, `\\`, `\"`, `\'`)

## Files

- **Token.h**: Defines token types (adds STRING and CHAR)
- **Lexer.h/cpp**: Tokenizes source code (handles string/char literals and escape sequences)
- **AST.h**: Defines AST nodes (adds StringLiteral and CharLiteral)
- **Parser.h/cpp**: Builds AST from tokens
- **Evaluator.h/cpp**: Executes AST (uses std::variant for value storage)
- **main.cpp**: Main entry point

## Building

### Using g++ (GCC/Clang)

```bash
cd cpp/Stage2
g++ -std=c++17 -Wall -o interpreter *.cpp
```

### Using CMake

```bash
cd cpp/Stage2
mkdir build
cd build
cmake ..
make
```

## Running

```bash
# From cpp/Stage2 directory
./interpreter ../../examples/stage2_example1.mid

# Or on Windows
interpreter.exe ..\..\examples\stage2_example1.mid
```

## New Features

### String Literals
```
name = "Alice";
print name;
```

### Character Literals
```
letter = 'A';
print letter;
```

### String Concatenation
```
greeting = "Hello, " + name;
print greeting;
```

### Type Conversion
```
message = "The answer is " + 42;  // 42 is converted to string
print message;  // Outputs: The answer is 42
```

### Escape Sequences
```
newline = "Line 1\nLine 2";
tab = "Column1\tColumn2";
quote = "He said \"Hello\"";
```

## Implementation Notes

- Uses `std::variant<int, std::string>` for value storage in the symbol table
- Character literals are converted to single-character strings during evaluation
- String concatenation automatically converts integers to strings

## Example

**Input file (`program.mid`):**
```
name = "Alice";
greeting = "Hello, " + name;
print greeting;
letter = 'A';
print letter;
message = "The answer is " + 42;
print message;
```

**Output:**
```
=== Interpreting: program.mid ===

Stage 1: Lexical Analysis (Tokenization)
Generated 20 tokens:
  IDENTIFIER(name)
  ASSIGN(=)
  STRING(Alice)
  SEMICOLON(;)
  ...

Stage 2: Parsing (Building AST)
Parsed 6 statement(s)

Stage 3: Evaluation (Execution)
Output:
Hello, Alice
A
The answer is 42

=== Program completed successfully ===
```

