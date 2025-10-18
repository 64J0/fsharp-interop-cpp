# Copilot Instructions

This repository uses **F#** as the primary programming language. When generating code or providing suggestions, ensure that the code adheres to F# conventions and idiomatic practices.

Also, keep in mind that we're developing this project to work in a Linux environment. Please ensure that any code snippets, commands, or configurations are compatible with Linux systems primarily.

## Interoperability with C++ using .NET P/Invoke

When working with C++ interop in this repository, follow these guidelines:

1. **Use .NET P/Invoke Best Practices**:
    - Ensure proper marshaling of data types between managed and unmanaged code.
    - DO use `[<LibraryImport>]`, if possible, when targeting .NET 7+.
        - There are cases when using `[DllImport]` is appropriate. A code analyzer with ID `SYSLIB1054` tells you when that's the case.
    - DO use the same naming and capitalization for your methods and parameters as the native method you want to call.
    - CONSIDER using the same naming and capitalization for constant values.
    - DO define P/Invoke and function pointer signatures that match the C function's arguments.
    - DO use .NET types that map closest to the native type. For example, in C#, use `uint` when the native type is `unsigned int`.
    - DO prefer expressing higher level native types using .NET structs rather than classes.
    - DO prefer using function pointers, as opposed to `Delegate` types, when passing callbacks to unmanaged functions in C#.
    - DO use `[<In>]` and `[<Out>]` attributes on array parameters.
    - DO only use `[<In>]` and `[<Out>]` attributes on other types when the behavior you want differs from the default behavior.
    - CONSIDER using `System.Buffers.ArrayPool` to pool your native array buffers.
    - CONSIDER wrapping your P/Invoke declarations in a class with the same name and capitalization as your native library.
    - This allows your `[<LibraryImport>]` or `[<DllImport>]` attributes to use the C# `nameof` language feature to pass in the name of the native library and ensure that you didn't misspell the name of the native library.
    - DO use `SafeHandle` handles to manage lifetime of objects that encapsulate unmanaged resources.
    - AVOID finalizers to manage lifetime of objects that encapsulate unmanaged resources.
    - Avoid memory leaks by properly freeing unmanaged resources when necessary.

2. **F# Specific Interop Guidance**:
    - Use `NativeInterop` modules or helper functions to encapsulate P/Invoke calls.
    - Prefer type-safe wrappers around raw P/Invoke calls for better maintainability.
    - Document all external function imports with comments explaining their purpose.

3. **Error Handling**:
    - Check for and handle errors returned by unmanaged functions.

4. **Testing**:
    - Write unit tests to validate the behavior of interop code.
    - Use mock libraries or test doubles to simulate unmanaged dependencies when possible.

5. **Code Examples**:
    - Provide clear examples of P/Invoke declarations, struct marshalling, string handling, callback functions, error handling, and memory safety patterns.

6. **Documentation**:
    - Maintain up-to-date documentation on how to use the interop features, including any setup required for native libraries.

7. **Review and Refactor**:
    - Regularly review interop code for potential improvements in safety, performance, and readability.
    - Refactor code to follow the latest best practices in .NET and F# interop.

8. **Benchmarks**:
    - Include performance benchmarks for critical interop paths to ensure efficiency.
    - Optimize marshaling strategies based on benchmark results.

By following these instructions, we aim to maintain clean, safe, and efficient interop code in this repository.

## Repository Structure and Navigation

This repository demonstrates F# and C/C++ interoperability using Platform Invoke (P/Invoke). Key directories:

- **`src/c/`** - C library source code with header files
- **`src/cpp/`** - C++ library source code with header files  
- **`src/fsharp/FSharpCppInterop/`** - Main F# console application with P/Invoke declarations
- **`tests/InteropTests/`** - xUnit tests for interop functionality
- **`benchmarks/InteropBenchmarks/`** - BenchmarkDotNet performance comparisons
- **`build/`** - Generated shared libraries (.so files)
- **`docs/`** - Additional documentation

## Development Environment and Setup

### Prerequisites
- **.NET 8.0 SDK** or later
- **GCC or Clang** compiler for C/C++ libraries
- **Make** for building native libraries
- **Linux environment** (primary target platform)

### Build Process
1. **Build native libraries**: `make` (creates `build/libmath_operations.so` and `build/libcpp_operations.so`)
2. **Build F# projects**: `dotnet build` (builds solution with all projects)
3. **Set library path**: `export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH`

### Testing and Validation
- **Run unit tests**: `cd tests/InteropTests && dotnet test --verbosity minimal`
- **Run benchmarks**: `cd benchmarks/InteropBenchmarks && dotnet run -c Release`
- **Run demo**: `cd src/fsharp/FSharpCppInterop && dotnet run`
- **Quick build/test**: Use `./build-and-run.sh` script for automated build and test

### Linting and Code Quality
- **C/C++ warnings**: Enabled via `-Wall -Wextra` in Makefile
- **F# compiler**: Built-in static analysis and warnings
- **All builds should complete without errors**

## Common Development Tasks

When making changes to this repository:

1. **Adding new P/Invoke functions**:
   - Add C/C++ implementation in appropriate source files
   - Add P/Invoke declaration in F# interop modules
   - Add corresponding unit tests
   - Update documentation and examples

2. **Modifying existing functionality**:
   - Rebuild native libraries with `make clean && make`
   - Rebuild F# projects with `dotnet build`
   - Run full test suite to ensure compatibility
   - Update benchmarks if performance-critical

3. **Cross-platform considerations**:
   - Use `.so` extension for Linux shared libraries
   - Ensure calling convention compatibility (`CallingConvention.Cdecl`)
   - Test library loading paths (`LD_LIBRARY_PATH`)

## Key Files for Code Generation

- **`MathOperationsInterop.fs`** - Primary P/Invoke declarations for C library
- **`CppOperationsInterop.fs`** - P/Invoke declarations for C++ library
- **`src/c/math_operations.h`** - C function signatures and data structures
- **`src/cpp/cpp_operations.hpp`** - C++ function signatures and classes
- **`Makefile`** - Build configuration for native libraries

## Performance and Benchmarking

This repository includes comprehensive performance analysis comparing F# managed code vs C P/Invoke vs C++ P/Invoke. When suggesting optimizations:
- Consider marshalling overhead vs computational gains
- Use `BenchmarkDotNet` attributes for new performance tests
- Focus on memory safety without sacrificing performance
- Document performance trade-offs in code comments