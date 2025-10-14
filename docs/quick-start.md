# Developer Quick Start Guide

This guide will get you up and running with the F# and C++ interop sample project in minutes.

## Prerequisites

Before you begin, ensure you have:

- **.NET 8.0 SDK** or later ([Download](https://dotnet.microsoft.com/download))
- **GCC/G++** compiler (Linux/macOS) or **Visual Studio** (Windows)
- **Make** utility (for building C++ library)
- **Git** (for cloning the repository)

### Platform-Specific Setup

#### Linux (Ubuntu/Debian)
```bash
sudo apt update
sudo apt install build-essential dotnet-sdk-8.0 git
```

#### macOS
```bash
# Install Xcode Command Line Tools
xcode-select --install

# Install .NET using Homebrew (if not installed)
brew install --cask dotnet

# Install make (usually included with Xcode tools)
```

#### Windows
- Install [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/) with C++ development tools
- Install [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- Use Windows Subsystem for Linux (WSL) or adapt Makefile for MSVC

## Quick Start (5 minutes)

### 1. Clone and Build

```bash
# Clone the repository
git clone https://github.com/64J0/fsharp-interop-cpp.git
cd fsharp-interop-cpp

# Build the C++ library
make

# Build the F# projects
dotnet build

# Run the demo
export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH  # Linux/macOS
cd src/fsharp/FSharpCppInterop
dotnet run
```

### 2. Run Tests

```bash
# From the project root
export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH
cd tests/InteropTests
dotnet test
```

### 3. Expected Output

You should see output similar to:
```
F# and C++ Interop Demo
=========================
=== Basic Operations Demo ===
10 + 20 = 30
3.14 * 2.0 = 6.280000
...
=== Demo Complete ===
```

## Project Structure Overview

```
├── src/
│   ├── c/                            # C native library
│   │   ├── math_operations.h         # C function declarations
│   │   └── math_operations.c         # C function implementations
│   ├── cpp/                          # C++ native library
│   │   ├── cpp_operations.hpp        # C++ function declarations
│   │   └── cpp_operations.cpp        # C++ function implementations
│   └── fsharp/FSharpCppInterop/      # F# console application
│       ├── MathOperationsInterop.fs  # P/Invoke declarations
│       └── Program.fs                # Demo application
├── tests/InteropTests/               # Unit tests
├── docs/                             # Detailed documentation
├── Makefile                          # C++ build system
└── README.md                         # Comprehensive documentation
```

## Key Concepts at a Glance

### P/Invoke Declaration
```fsharp
[<DllImport("libmath_operations.so", CallingConvention = CallingConvention.Cdecl)>]
extern int add_integers(int a, int b)
```

### Struct Marshalling
```fsharp
[<Struct; StructLayout(LayoutKind.Sequential)>]
type Point = { x: int; y: int }
```

### Safe String Operations
```fsharp
let copyStringSafe source maxLength =
    let buffer = new StringBuilder(maxLength: int)
    copy_string(source, buffer, maxLength)
    buffer.ToString()
```

### Error Handling
```fsharp
type ResultCode =
    | Success = 0
    | NullPointer = -1
    | InvalidParameter = -2
```

## Common Issues and Solutions

### Library Not Found
**Error**: `DllNotFoundException: Unable to load shared library`

**Solution**:
```bash
# Ensure library path is set
export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH

# Or install system-wide
make install
```

### Build Errors
**Error**: Compilation errors in F# code

**Solution**:
```bash
# Ensure .NET SDK is properly installed
dotnet --version

# Clean and rebuild
dotnet clean
dotnet build
```

## Next Steps

1. **Explore the code**: Start with `src/fsharp/FSharpCppInterop/Program.fs`
2. **Run specific demos**: Modify the main function to run only certain scenarios
3. **Extend functionality**: Add your own C functions and F# bindings
4. **Read detailed docs**: Check the `docs/` folder for advanced topics

## Learning Path

1. **Start here**: Run the demo and explore basic examples
2. **Basic interop**: Study `MathOperationsInterop.fs` for P/Invoke patterns
3. **Advanced topics**: Read `docs/advanced-scenarios.md`
4. **Troubleshooting**: Refer to `docs/troubleshooting.md` when issues arise
5. **Extend**: Create your own interop scenarios

## Example Modifications

### Adding a New Function

**C code** (in `math_operations.c`):
```c
float calculate_circle_area(float radius) {
    return 3.14159f * radius * radius;
}
```

**F# binding** (in `MathOperationsInterop.fs`):
```fsharp
[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern float calculate_circle_area(float radius)
```

**Usage** (in `Program.fs`):
```fsharp
let area = calculate_circle_area(5.0f)
printfn "Circle area: %f" area
```

## Performance Tips

- **Minimize P/Invoke calls**: Batch operations when possible
- **Use appropriate data types**: Match C types exactly
- **Consider memory management**: Use RAII patterns for resources
- **Profile your code**: Measure actual performance impact

## Getting Help

- **Documentation**: Check the `docs/` folder
- **Examples**: All examples are in the demo application
- **Issues**: Common problems are covered in troubleshooting guide
- **Community**: Create issues on GitHub for questions

Happy coding with F# and C++ interop! 🚀