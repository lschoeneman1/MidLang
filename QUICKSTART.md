# Quick Start Guide

## For C# Students

### Prerequisites
- .NET 10.0 SDK or later installed
- A text editor or IDE (Visual Studio, VS Code, Rider, etc.)

### Getting Started

1. **Navigate to the C# Stage 1 directory:**
   ```bash
   cd csharp/Stage1
   ```

2. **Build the project:**
   ```bash
   dotnet build
   ```

3. **Run an example:**
   ```bash
   dotnet run ../../examples/stage1_example1.mid
   ```

4. **Create your own program:**
   - Create a file `myprogram.mid` with:
     ```
     x = 10;
     y = 5;
     result = x * y;
     print result;
     ```
   - Run it:
     ```bash
     dotnet run myprogram.mid
     ```

### Understanding the Code

Start by reading these files in order:
1. `Token.cs` - What are tokens?
2. `Lexer.cs` - How do we tokenize?
3. `AST.cs` - What is the AST structure?
4. `Parser.cs` - How do we build the AST?
5. `Evaluator.cs` - How do we execute?
6. `Program.cs` - How does it all fit together?

## For C++ Students

### Prerequisites
- C++17 compatible compiler (g++, clang++, or MSVC)
- A text editor or IDE (Visual Studio, Code::Blocks, CLion, etc.)

### Getting Started

1. **Navigate to the C++ Stage 1 directory:**
   ```bash
   cd cpp/Stage1
   ```

2. **Build the project:**

   **Using g++ (Linux/Mac/Git Bash):**
   ```bash
   g++ -std=c++17 -Wall -o interpreter *.cpp
   ```

   **Using Visual Studio (Windows):**
   ```bash
   cl /EHsc /std:c++17 *.cpp /Fe:interpreter.exe
   ```

   **Using CMake:**
   ```bash
   mkdir build
   cd build
   cmake ..
   make
   ```

3. **Run an example:**
   ```bash
   ./interpreter ../../examples/stage1_example1.mid
   ```

4. **Create your own program:**
   - Create a file `myprogram.mid` with:
     ```
     x = 10;
     y = 5;
     result = x * y;
     print result;
     ```
   - Run it:
     ```bash
     ./interpreter myprogram.mid
     ```

### Understanding the Code

Start by reading these files in order:
1. `Token.h` - What are tokens?
2. `Lexer.h` and `Lexer.cpp` - How do we tokenize?
3. `AST.h` - What is the AST structure?
4. `Parser.h` and `Parser.cpp` - How do we build the AST?
5. `Evaluator.h` and `Evaluator.cpp` - How do we execute?
6. `main.cpp` - How does it all fit together?

## Common Issues

### C#: "dotnet command not found"
- Install .NET SDK from https://dotnet.microsoft.com/download

### C++: "g++ command not found"
- **Windows**: Install MinGW or use Visual Studio
- **Mac**: Install Xcode Command Line Tools: `xcode-select --install`
- **Linux**: Install build-essential: `sudo apt-get install build-essential`

### C++: Compilation errors about C++17
- Make sure your compiler supports C++17
- Check compiler version: `g++ --version` (should be 7+)
- Use the `-std=c++17` flag explicitly

## Next Steps

1. Read `docs/Architecture.md` to understand the three-stage design
2. Read `docs/EBNF.md` to understand the language grammar
3. Read `docs/Stage1_Guide.md` for teaching materials
4. Try modifying the code:
   - Add a new operator (e.g., modulo `%`)
   - Improve error messages
   - Add comments support

## Getting Help

- Check the README files in each Stage directory
- Review the documentation in the `docs/` folder
- Look at the example programs in `examples/`
- Ask your instructor!

