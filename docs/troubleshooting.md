# Troubleshooting Guide

This guide helps resolve common issues when working with F# and C++ interop.

## Library Loading Issues

### Problem: `DllNotFoundException`

**Symptoms:**
```
System.DllNotFoundException: Unable to load shared library 'libmath_operations.so' or one of its dependencies.
```

**Solutions:**

1. **Check library path:**
   ```bash
   # Add to current session
   export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH
   
   # Or add to ~/.bashrc for permanent effect
   echo 'export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH' >> ~/.bashrc
   ```

2. **Install system-wide:**
   ```bash
   make install
   ```

3. **Use absolute path in code:**
   ```fsharp
   [<Literal>]
   let LibraryName = "/absolute/path/to/libmath_operations.so"
   ```

### Problem: `BadImageFormatException`

**Symptoms:**
```
System.BadImageFormatException: An attempt was made to load a program with an incorrect format.
```

**Causes and Solutions:**

1. **Architecture mismatch (32-bit vs 64-bit):**
   ```bash
   # Check your .NET runtime architecture
   dotnet --info
   
   # Check library architecture
   file build/libmath_operations.so
   
   # Rebuild for correct architecture
   make clean && make CFLAGS="-m64 -fPIC -shared"
   ```

2. **Mixed platforms:**
   - Ensure you're not mixing Windows DLL with Linux/macOS runtime

## Compilation Issues

### Problem: Function not found during compilation

**Symptoms:**
```
error FS0039: The value or constructor 'add_integers' is not defined
```

**Solutions:**

1. **Check function declaration:**
   ```fsharp
   // Ensure extern keyword is present
   [<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
   extern int add_integers(int a, int b)
   ```

2. **Verify calling convention:**
   ```c
   // In C header, ensure extern "C" for C++ files
   #ifdef __cplusplus
   extern "C" {
   #endif
   
   int add_integers(int a, int b);
   
   #ifdef __cplusplus
   }
   #endif
   ```

### Problem: Struct layout mismatch

**Symptoms:**
- Incorrect values in struct fields
- Access violations
- Garbage data

**Solutions:**

1. **Explicit struct layout:**
   ```fsharp
   [<StructLayout(LayoutKind.Sequential, Pack = 1)>]
   type Point = {
       x: int
       y: int
   }
   ```

2. **Check C struct definition:**
   ```c
   // Ensure structs match exactly
   typedef struct {
       int x;  // 4 bytes
       int y;  // 4 bytes
   } Point;   // Total: 8 bytes
   ```

3. **Use size verification:**
   ```fsharp
   let structSize = Marshal.SizeOf<Point>()
   printfn "Point size: %d bytes" structSize
   ```

## Runtime Issues

### Problem: `AccessViolationException`

**Symptoms:**
```
System.AccessViolationException: Attempted to read or write protected memory.
```

**Common Causes:**

1. **Null pointer access:**
   ```fsharp
   // Always check for null
   let safeStringLength str =
       if str = null then -1
       else string_length str
   ```

2. **Buffer overrun:**
   ```fsharp
   // Ensure buffer is large enough
   let buffer = StringBuilder(256)  // Adequate size
   copy_string source buffer 256
   ```

3. **Uninitialized memory:**
   ```c
   // In C code, initialize all variables
   Point p = {0, 0};  // Good
   Point p;           // Dangerous - uninitialized
   ```

### Problem: Memory leaks

**Symptoms:**
- Increasing memory usage over time
- System performance degradation

**Solutions:**

1. **Use RAII pattern:**
   ```fsharp
   let withAllocatedString length action =
       let ptr = allocate_string length
       try
           if ptr <> IntPtr.Zero then action ptr
           else failwith "Allocation failed"
       finally
           if ptr <> IntPtr.Zero then free_string ptr
   ```

2. **Implement IDisposable:**
   ```fsharp
   type NativeResource(allocator: unit -> IntPtr, deallocator: IntPtr -> unit) =
       let ptr = allocator()
       let mutable disposed = false
       
       interface IDisposable with
           member _.Dispose() =
               if not disposed && ptr <> IntPtr.Zero then
                   deallocator ptr
                   disposed <- true
   ```

## String Handling Issues

### Problem: Corrupted or truncated strings

**Solutions:**

1. **Proper encoding:**
   ```fsharp
   [<DllImport(LibraryName, CharSet = CharSet.Ansi)>]
   extern void process_ansi_string(string input)
   
   [<DllImport(LibraryName, CharSet = CharSet.Unicode)>]
   extern void process_unicode_string(string input)
   ```

2. **Buffer management:**
   ```fsharp
   let copyStringSafe source maxLength =
       let buffer = StringBuilder(maxLength)
       copy_string source buffer maxLength
       buffer.ToString().TrimEnd('\0')  // Remove null terminators
   ```

3. **Lifetime management:**
   ```fsharp
   // For returned string pointers
   let getString() =
       let ptr = get_string_from_native()
       if ptr <> IntPtr.Zero then
           let result = Marshal.PtrToStringAnsi(ptr)
           // Note: Don't free if string is static in C
           result
       else
           null
   ```

## Callback Issues

### Problem: Callback not being called

**Solutions:**

1. **Keep delegate alive:**
   ```fsharp
   // Store callback to prevent GC collection
   let private storedCallback = ProgressCallback(fun progress -> 
       printfn "Progress: %d%%" progress)
   
   let startWork() =
       simulate_work 1000 storedCallback
   ```

2. **Thread safety:**
   ```fsharp
   let threadSafeCallback = ProgressCallback(fun progress ->
       // Marshal to UI thread if needed
       Application.Current.Dispatcher.Invoke(fun () ->
           updateProgressBar progress
       )
   )
   ```

## Platform-Specific Issues

### Linux-specific

1. **Library naming:**
   ```bash
   # Ensure library starts with 'lib' prefix
   gcc -shared -fPIC -o libmath_operations.so math_operations.c
   ```

2. **Dependencies:**
   ```bash
   # Check and install missing dependencies
   ldd build/libmath_operations.so
   sudo apt-get install libc6-dev
   ```

### macOS-specific

1. **Library extension:**
   ```fsharp
   let libraryName = 
       if RuntimeInformation.IsOSPlatform(OSPlatform.OSX) then
           "libmath_operations.dylib"
       else
           "libmath_operations.so"
   ```

2. **Code signing (macOS Catalina+):**
   ```bash
   # May need to sign the library
   codesign -s - build/libmath_operations.dylib
   ```

### Windows-specific

1. **Use Windows paths:**
   ```fsharp
   let libraryName = 
       if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then
           "math_operations.dll"
       else
           "libmath_operations.so"
   ```

2. **Visual C++ Runtime:**
   ```bash
   # May need Visual C++ Redistributables
   # Download from Microsoft website
   ```

## Debugging Tips

### Enable Native Debugging

```bash
# Linux: Use GDB
gdb --args dotnet run

# Or with environment variables
DOTNET_EnableCrashReport=1 dotnet run
```

### Logging

```fsharp
// Add logging to P/Invoke calls
let loggedAddIntegers a b =
    printfn "Calling add_integers(%d, %d)" a b
    try
        let result = add_integers a b
        printfn "Result: %d" result
        result
    with
    | ex ->
        printfn "Error: %s" ex.Message
        reraise()
```

### Memory Debugging

```bash
# Use Valgrind on Linux
valgrind --tool=memcheck --leak-check=full dotnet run

# Use AddressSanitizer
export ASAN_OPTIONS=detect_leaks=1
gcc -fsanitize=address -g -o library math_operations.c
```

## Testing Strategies

### Unit Tests with Mocking

```fsharp
// Create testable abstractions
type INativeLibrary =
    abstract member AddIntegers: int -> int -> int

// Test implementation
type MockNativeLibrary() =
    interface INativeLibrary with
        member _.AddIntegers a b = a + b

// Production implementation  
type RealNativeLibrary() =
    interface INativeLibrary with
        member _.AddIntegers a b = add_integers a b
```

### Integration Tests

```fsharp
[<Fact>]
let ``Full integration test`` () =
    // Build and install library first
    let buildResult = Process.Start("make").WaitForExit()
    Assert.Equal(0, buildResult)
    
    // Test actual functionality
    let result = add_integers 10 20
    Assert.Equal(30, result)
```

## Performance Troubleshooting

### Profiling P/Invoke Calls

```fsharp
open System.Diagnostics

let timedOperation operation =
    let sw = Stopwatch.StartNew()
    let result = operation()
    sw.Stop()
    printfn "Operation took: %d ms" sw.ElapsedMilliseconds
    result

// Usage
let result = timedOperation (fun () -> add_integers 1 2)
```

### Reducing P/Invoke Overhead

1. **Batch operations:**
   ```fsharp
   // Instead of multiple single calls
   let processArray arr =
       arr |> Array.map (fun x -> add_integers x 1)
   
   // Use bulk operation
   let processArrayBulk arr =
       process_array_bulk arr arr.Length
   ```

2. **Cache expensive calls:**
   ```fsharp
   let mutable cachedValue = None
   
   let getExpensiveValue() =
       match cachedValue with
       | Some value -> value
       | None ->
           let value = expensive_native_call()
           cachedValue <- Some value
           value
   ```