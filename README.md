# F# and C/C++ Interop Sample Project

This project demonstrates comprehensive interoperability between F# and C/C++ using Platform Invoke (P/Invoke). It showcases various scenarios commonly encountered when integrating F# applications with native C/C++ libraries.

> How does P/Invoke work?
>
> P/Invoke involves a couple of steps. These are the steps you must follow to use a Win32 API in a .NET application:
>
> 1. Find the API you want to use.
> 2. Find the DLL the API resides in.
> 3. Load that DLL in your assembly.
> 4. Declare a stub that tells your application how to call that API.
> 5. Convert the .NET data types into something the Win32 API can understand.
> 6. Use the API.
>
> -- *Systems Programming with C# and .NET. 1st edition.*

## Project Structure

```
├── src/
│   ├── c/                                # C library source code
│   │   ├── math_operations.h             # Header file with function declarations
│   │   └── math_operations.c             # Implementation of C functions
│   ├── cpp/                              # C++ library source code
│   │   ├── cpp_operations.hpp            # Header file with function declarations
│   │   └── cpp_operations.cpp            # Implementation of C++ functions
│   └── fsharp/                           # F# application
│       └── FSharpCppInterop/             # F# console application
│           ├── MathOperationsInterop.fs  # P/Invoke declarations
│           ├── Program.fs                # Demo application
│           └── FSharpCppInterop.fsproj
├── tests/
│   └── InteropTests/                     # Unit tests for interop functionality
├── docs/                                 # Additional documentation
├── Makefile                              # Build system for C++ library
├── README.md                             # This file
└── LICENSE
```

## Features Demonstrated

### 1. Basic Data Types
- Integer operations (addition)
- Floating-point operations (multiplication, division)
- Boolean operations (even number detection)

### 2. String Operations
- String length calculation
- String copying with buffer management
- Dynamic string generation
- Null string handling

### 3. Struct/Record Interop
- Point structures with coordinate data
- Rectangle structures with dimension calculations
- Struct creation and manipulation
- Distance and area calculations

### 4. Array Operations
- Array filling with values
- Array summation
- Array sorting (in-place)
- Safe array bounds checking

### 5. Callback Functions
- Progress reporting callbacks
- Function pointer marshalling
- Event-driven operations

### 6. Error Handling
- Error codes and result types
- Safe operations with validation
- Null pointer checking
- Parameter validation

### 7. Memory Management
- Dynamic memory allocation
- Safe memory deallocation
- RAII-style resource management in F#
- Buffer overflow prevention

## Running Performance Benchmarks

The project includes comprehensive performance benchmarks using BenchmarkDotNet to compare **F# vs C vs C++** implementations of equivalent operations.

### Quick Benchmark Run

```bash
# Build libraries and set environment
make
export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH

# Run benchmarks
cd benchmarks/InteropBenchmarks
dotnet run -c Release
```

### Benchmark Categories

1. **Full Performance Benchmarks**: Compare F# managed code vs C P/Invoke vs C++ P/Invoke across multiple scenarios
2. **Micro Benchmarks**: Array size scaling analysis (10-10,000 elements) for all three approaches
3. **Quick Performance Test**: Fast verification showing relative performance characteristics

### Performance Insights
- **F# Baseline**: Managed code performance without P/Invoke overhead
- **C P/Invoke**: Raw performance gains vs marshalling costs  
- **C++ P/Invoke**: Modern features and safety vs additional object management overhead
- **When to use each**: Data-driven decisions based on actual performance measurements

### Sample Results
- F# array operations: Often fastest due to highly optimized .NET implementations
- C mathematical operations: ~2x faster for compute-intensive algorithms
- C++ template operations: Competitive with C while providing better type safety
- Memory patterns: Trade-offs between managed safety and unmanaged control

See [Benchmarking Guide](docs/benchmarking-guide.md) for detailed instructions, result interpretation, and performance optimization strategies.

## Building and Running

### Prerequisites
- .NET 8.0 SDK or later
- GCC or Clang compiler
- Make (for building C++ library)

### Build Steps

1. **Build the C++ library:**
   ```bash
   make
   ```
   This creates `build/libmath_operations.so`

2. **Build the F# application:**
   ```bash
   cd src/fsharp/FSharpCppInterop
   dotnet build
   ```

3. **Run the demo:**
   ```bash
   # Make sure the library is in the system path or LD_LIBRARY_PATH
   export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH
   cd src/fsharp/FSharpCppInterop
   dotnet run
   ```

4. **Run tests:**
   ```bash
   export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH
   cd tests/InteropTests
   dotnet test
   ```

### Alternative: System Installation
```bash
make install    # Install library system-wide (requires sudo)
```

## Key Concepts Explained

### P/Invoke Declarations
F# uses the `[<DllImport>]` attribute to declare external functions:

```fsharp
[<DllImport("libmath_operations.so", CallingConvention = CallingConvention.Cdecl)>]
extern int add_integers(int a, int b)
```

### Struct Marshalling
Structures must be explicitly laid out for proper interop:

```fsharp
[<Struct>]
[<StructLayout(LayoutKind.Sequential)>]
type Point = {
    x: int
    y: int
}
```

### String Handling
Different approaches for string marshalling:
- `string` for input parameters
- `StringBuilder` for output buffers  
- `IntPtr` for pointer-based operations

### Callback Functions
Delegate types enable callback functionality:

```fsharp
type ProgressCallback = delegate of int -> unit
```

### Error Handling
Enumeration types provide structured error reporting:

```fsharp
type ResultCode =
    | Success = 0
    | NullPointer = -1
    | InvalidParameter = -2
    | BufferTooSmall = -3
```

### Memory Safety
Resource management patterns ensure safe memory usage:

```fsharp
let withAllocatedString length action =
    let ptr = allocate_string length
    if ptr <> IntPtr.Zero then
        try
            action ptr
        finally
            free_string ptr
    else
        failwith "Failed to allocate string"
```

## Best Practices Demonstrated

1. **Always specify calling convention** (`CallingConvention.Cdecl`)
2. **Use proper struct layout** for binary compatibility
3. **Implement safe wrappers** for unsafe operations
4. **Handle null pointers** and validate inputs
5. **Use RAII patterns** for resource management
6. **Provide error handling** with meaningful error codes
7. **Test thoroughly** with comprehensive unit tests

## Common Pitfalls and Solutions

### Library Loading Issues
- Ensure library is in `LD_LIBRARY_PATH` or system library path
- Use `ldd` to check library dependencies
- Consider using absolute paths in development

### Marshalling Problems
- Verify struct layout matches C definitions exactly
- Use correct character encoding for strings
- Be aware of pointer size differences (32-bit vs 64-bit)

### Memory Management
- Always pair allocation with deallocation
- Use try/finally blocks or `using` statements
- Be careful with string lifetime management

## Extending the Examples

This project provides a solid foundation for more complex scenarios:

1. **Complex Data Structures**: Nested structs, unions, bit fields
2. **Advanced Callbacks**: Multi-threaded callbacks, event handling
3. **Performance Optimization**: Bulk operations, streaming data
4. **Cross-Platform Support**: Windows DLL support, macOS dylib support

## Related projects and links

- [simdjson/simdjson](https://github.com/simdjson/simdjson) (simdjson [docs](https://simdjson.github.io/simdjson/md_doc_basics.html)) --> [EgorBo/SimdJsonSharp](https://github.com/EgorBo/SimdJsonSharp). C# bindings for lemire/simdjson (and full C# port).
- [speakeztech/Farscape](https://github.com/speakeztech/Farscape).Farscape is a command-line tool that aims to automatically generate idiomatic F# bindings for C/C++ libraries, preserving F# intrinsics.
- [matthewcrews/OdinPerf](https://github.com/matthewcrews/OdinPerf). With this project, Matthew Crews (the Fast F# guy) makes a performance comparison between F# and Odin programs. There's also this interview where he exposes more about his opinion on this language vs F# considering the scenario he's working on: [Odin Newsletter - Interview with Crews](https://odin-lang.org/news/newsletter-2024-10/#interview-with-crews).
- [[YouTube] dotnet/C++ Interoperability Within .NET](https://youtu.be/udIEiCAn15E)

## Contributing

Feel free to extend this example with additional scenarios or improvements. Some ideas:

- COM interop examples
- Asynchronous operations
- Custom marshalling scenarios
- Performance benchmarks
- Cross-platform build system

## License

This project is licensed under the Apache License 2.0 - see the LICENSE file for details.
