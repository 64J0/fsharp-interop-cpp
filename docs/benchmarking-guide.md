# Performance Benchmarking Guide

This guide explains how to run performance benchmarks to compare C and C++ interop performance in F#.

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
Comprehensive benchmarking suite comparing C and C++ operations:

- **Basic Operations**: Integer addition, float multiplication
- **Array Operations**: Summation, sorting 
- **String Operations**: Length calculation, manipulation
- **Mathematical Operations**: Statistical calculations (mean, variance, standard deviation)
- **Memory Management**: Allocation/deallocation patterns
- **Function Objects**: C++ lambda and function object performance

**Output**: Detailed statistics including mean execution time, standard deviation, memory allocation, and percentile distributions.

#### 2. Micro Benchmarks  
Scaled performance tests with varying array sizes (10, 100, 1000, 10000 elements):

- Array sum operations (C)
- Mathematical mean calculations (C++)
- Variance calculations (C++)

**Output**: Performance scaling analysis showing how operations perform with different data sizes.

#### 3. Quick Performance Test
Fast performance check without statistical analysis - useful for quick verification.

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
| Method                                          | Mean      | Error    | StdDev   | Allocated |
|------------------------------------------------ |----------:|---------:|---------:|----------:|
| C: Integer Addition (1000 calls)               |  12.34 μs | 0.45 μs  | 0.23 μs  |         - |
| C: Float Multiplication (1000 calls)           |  15.67 μs | 0.52 μs  | 0.31 μs  |         - |
| C++: Mathematical Mean (1000 element array)    |  89.12 μs | 2.34 μs  | 1.45 μs  |         - |
| C++: Vector Operations (Add + Sum)              |  45.78 μs | 1.23 μs  | 0.89 μs  |    1024 B |
```

### Interpreting Results

- **Lower Mean = Better Performance**
- **Lower StdDev = More Consistent Performance**  
- **Lower Allocated = Less Memory Usage**
- **Compare similar operations** (e.g., C array sum vs C++ vector sum)

## Expected Performance Characteristics

### C Library Operations
- **Strengths**: Minimal overhead, direct memory access, simple operations
- **Best for**: Basic arithmetic, array processing, low-level operations

### C++ Library Operations  
- **Strengths**: Rich functionality, safe memory management, template optimizations
- **Trade-offs**: Some overhead from object creation, RAII patterns
- **Best for**: Complex operations, mathematical computations, resource management

### General Observations

1. **Simple Operations**: C typically faster due to minimal overhead
2. **Complex Operations**: C++ may be competitive or faster due to optimizations
3. **Memory Management**: C++ shows allocations but provides safety guarantees
4. **Function Objects**: Surprisingly performant due to compiler optimizations

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