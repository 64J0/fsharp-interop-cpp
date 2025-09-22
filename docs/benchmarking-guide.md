# Performance Benchmarking Guide

This guide explains how to run performance benchmarks comparing **F# vs C vs C++** implementations to understand the performance trade-offs between managed F# code and native libraries accessed via P/Invoke.

## Overview

The benchmark suite compares three approaches:
- **F# (Managed)**: Pure F# implementations using .NET's built-in functions and data structures
- **C (P/Invoke)**: Native C library functions called from F# via Platform Invoke
- **C++ (P/Invoke)**: Modern C++ library functions with STL containers and templates, called from F# via P/Invoke

This comparison helps you understand when P/Invoke overhead is justified and when native performance benefits outweigh the marshalling costs.

## Prerequisites

Before running benchmarks, ensure you have:

1. **Built the native libraries:**
   ```bash
   make
   ```

2. **Set library path:**
   ```bash
   export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH
   ```

3. **Built the benchmark project:**
   ```bash
   dotnet build benchmarks/InteropBenchmarks/
   ```

## Running Benchmarks

### Quick Start

```bash
cd benchmarks/InteropBenchmarks
dotnet run -c Release
```

This will show an interactive menu with benchmark options.

### Benchmark Options

#### 1. Full Performance Benchmarks
Comprehensive benchmarking suite comparing F#, C, and C++ implementations:

- **Basic Operations**: F# arithmetic vs C integer operations vs C++ template operations
- **Array Operations**: F# Array functions vs C array processing vs C++ STL vectors  
- **String Operations**: F# string handling vs C char* operations vs C++ std::string
- **Mathematical Operations**: F# Array.average vs C calculations vs C++ statistical functions
- **Memory Management**: F# managed arrays vs C malloc/free vs C++ smart pointers
- **Function Objects**: F# function values vs C function pointers vs C++ lambdas

**Output**: Detailed comparison showing F# as baseline, with relative performance of C and C++ approaches.

#### 2. Micro Benchmarks  
Scaled performance tests with varying array sizes (10, 100, 1000, 10000 elements):

- F# Array.sum vs C array summation vs C++ vector operations
- F# Array.average vs C++ mathematical mean calculations
- F# variance calculations vs C++ template-based variance

**Output**: Performance scaling analysis showing how F#, C, and C++ perform with different data sizes.

#### 3. Quick Performance Test
Fast performance comparison without statistical analysis - useful for quick verification of F# vs C vs C++ performance characteristics.

### Command Line Options

You can also run specific benchmark types directly:

```bash
# Run full benchmarks
dotnet run -c Release -- --filter "*InteropPerformanceBenchmarks*"

# Run micro benchmarks only
dotnet run -c Release -- --filter "*MicroBenchmarks*"

# Run with specific configuration
dotnet run -c Release -- --job short --warmupCount 3 --iterationCount 5
```

## Understanding Results

### Performance Metrics

- **Mean**: Average execution time across all iterations
- **Error**: Half of 99.9% confidence interval
- **StdDev**: Standard deviation of measurements  
- **Min/Max**: Fastest and slowest execution times
- **P95**: 95th percentile (95% of measurements were faster)
- **Allocated**: Memory allocated per operation

### Sample Output

```
| Method                                          | Mean      | Error    | StdDev   | Ratio | Allocated |
|------------------------------------------------ |----------:|---------:|---------:|------:|----------:|
| F#: Integer Addition (1000 calls)              |  8.45 μs  | 0.12 μs  | 0.08 μs  |  1.00 |         - |
| C: Integer Addition (1000 calls)               | 12.34 μs  | 0.45 μs  | 0.23 μs  |  1.46 |         - |
| F#: Array Sum (100 elements)                   |  0.89 μs  | 0.02 μs  | 0.01 μs  |  1.00 |         - |
| C: Array Sum (100 elements)                    |  2.15 μs  | 0.08 μs  | 0.05 μs  |  2.42 |         - |
| F#: Mathematical Mean (1000 element array)     | 45.67 μs  | 1.12 μs  | 0.67 μs  |  1.00 |         - |
| C++: Mathematical Mean (1000 element array)    | 89.12 μs  | 2.34 μs  | 1.45 μs  |  1.95 |         - |
| F#: List Operations (Add + Sum)                | 23.45 μs  | 0.67 μs  | 0.34 μs  |  1.00 |    1024 B |
| C++: Vector Operations (Add + Sum)              | 45.78 μs  | 1.23 μs  | 0.89 μs  |  1.95 |    1024 B |
```

### Interpreting Results

- **Lower Mean = Better Performance**
- **Lower StdDev = More Consistent Performance**  
- **Ratio Column**: Shows performance relative to F# baseline (F# = 1.00)
- **Lower Allocated = Less Memory Usage**
- **Compare equivalent operations** (e.g., F# Array.sum vs C array_sum vs C++ vector.Sum())

## Expected Performance Characteristics

### F# (Managed Code)
- **Strengths**: No marshalling overhead, JIT optimizations, built-in collection operations
- **Best for**: Most general-purpose operations, when safety and productivity matter more than raw speed
- **Considerations**: GC pressure with large allocations, less control over memory layout

### C Library Operations
- **Strengths**: Minimal overhead, direct memory access, highly optimized algorithms
- **Trade-offs**: P/Invoke marshalling costs, less type safety
- **Best for**: Compute-intensive operations where P/Invoke overhead is amortized

### C++ Library Operations  
- **Strengths**: Modern language features, template optimizations, RAII safety
- **Trade-offs**: Object creation overhead, P/Invoke complexity, larger binary size
- **Best for**: Complex algorithms requiring both performance and modern C++ safety features

### General Performance Patterns

1. **Simple Operations**: F# often competitive or faster due to no marshalling overhead
2. **Array/Collection Processing**: F# built-ins are highly optimized and hard to beat
3. **Mathematical Computations**: C/C++ advantage grows with computation complexity
4. **String Operations**: F# string handling often faster than C char* marshalling
5. **Memory-Intensive**: C/C++ advantages with large datasets and custom memory management

## Advanced Usage

### Custom Benchmark Runs

```bash
# Export results to different formats
dotnet run -c Release -- --exporters html,csv,json

# Filter specific benchmarks
dotnet run -c Release -- --filter "*String*"

# Run with profiler (requires additional setup)
dotnet run -c Release -- --profiler ETW

# Run with memory profiler
dotnet run -c Release -- --profiler NativeMemory
```

### Environment Considerations

- **CPU**: Results vary by processor architecture and clock speed
- **Memory**: Ensure sufficient RAM for large array benchmarks  
- **OS**: Performance characteristics may differ between Linux/Windows/macOS
- **Compiler**: Different C++ compiler versions may affect performance

### Benchmark Reliability

- Run benchmarks multiple times to verify consistency
- Close other applications to reduce noise
- Use Release configuration for production-like performance
- Consider thermal throttling on laptops during long benchmark runs

## Troubleshooting

### Common Issues

1. **Libraries not found**:
   ```bash
   export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH
   ```

2. **Benchmark crashes**:
   - Ensure libraries are built correctly
   - Check for memory issues with `valgrind` (Linux)

3. **Inconsistent results**:
   - Close background applications
   - Run multiple times and average results
   - Check CPU throttling/power management

### Performance Tips

- Use `dotnet run -c Release` for accurate performance measurements
- Avoid running benchmarks on battery power (CPU throttling)
- Run benchmarks when system is idle
- Consider benchmark warmup effects on first runs

## Extending Benchmarks

To add your own benchmarks:

1. Add methods to `InteropPerformanceBenchmarks` class
2. Use `[<Benchmark>]` attribute
3. Follow existing patterns for C/C++ library calls
4. Handle exceptions gracefully for missing libraries
5. Use descriptive names and documentation

Example:
```fsharp
[<Benchmark(Description = "My Custom Operation")>]
member this.MyCustomBenchmark() =
    // Your benchmark code here
    my_custom_operation()
```