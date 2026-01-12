cmake_minimum_required(VERSION 3.10)
project(MidLangStage3)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_executable(midlang_stage3
    main.cpp
    Lexer.cpp
    Parser.cpp
    Evaluator.cpp
)

# Set output directory
set_target_properties(interpreter PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}
)

