# P/Invoke Best Practices and Advanced Scenarios

This document covers advanced topics and best practices for F# and C++ interoperability.

## Advanced Marshalling Scenarios

### Custom Marshalling with Attributes

```fsharp
// Custom string marshalling
[<DllImport("library.so", CharSet = CharSet.Ansi)>]
extern void process_ansi_string(string input)

[<DllImport("library.so", CharSet = CharSet.Unicode)>]
extern void process_unicode_string(string input)

// Array marshalling with specific size
[<DllImport("library.so")>]
extern void process_fixed_array([<MarshalAs(UnmanagedType.LPArray, SizeConst = 10)>] int[] array)
```

### Complex Structures

```fsharp
[<StructLayout(LayoutKind.Sequential)>]
type ComplexStruct = {
    [<MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)>]
    name: string
    
    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)>]
    coordinates: float[]
    
    isActive: bool
}
```

### Union Types (Discriminated Unions)

```fsharp
[<StructLayout(LayoutKind.Explicit)>]
type UnionType = {
    [<FieldOffset(0)>]
    intValue: int
    
    [<FieldOffset(0)>]
    floatValue: float32
}
```

## Error Handling Patterns

### Exception-Safe Wrappers

```fsharp
let safeCall (operation: unit -> 'T) : Result<'T, string> =
    try
        Ok(operation())
    with
    | :? System.DllNotFoundException as ex -> Error $"Library not found: {ex.Message}"
    | :? System.EntryPointNotFoundException as ex -> Error $"Function not found: {ex.Message}"
    | :? System.AccessViolationException as ex -> Error $"Memory access violation: {ex.Message}"
    | ex -> Error $"Unexpected error: {ex.Message}"
```

### Resource Management

```fsharp
type NativeResource(ptr: IntPtr, deallocator: IntPtr -> unit) =
    let mutable disposed = false
    
    member _.Pointer = 
        if disposed then failwith "Resource has been disposed"
        ptr
    
    interface System.IDisposable with
        member _.Dispose() =
            if not disposed then
                deallocator ptr
                disposed <- true
```

## Performance Considerations

### Bulk Operations

When transferring large amounts of data, consider:

1. **Pinning Memory**: Use `GCHandle.Alloc` with `GCHandleType.Pinned`
2. **Unsafe Code**: Use `fixed` statements for temporary pinning
3. **Batch Calls**: Minimize the number of P/Invoke calls

```fsharp
// Efficient array processing
let processLargeArray (data: int[]) =
    use handle = GCHandle.Alloc(data, GCHandleType.Pinned)
    let ptr = handle.AddrOfPinnedObject()
    // Call native function with pointer
    process_array_bulk(ptr, data.Length)
```

### Calling Convention Considerations

- **Cdecl**: Standard C calling convention (default for GCC)
- **StdCall**: Windows API calling convention
- **FastCall**: Optimized calling convention (compiler-specific)

## Thread Safety

### Callbacks from Native Code

```fsharp
// Thread-safe callback handling
let private callbackHandler = 
    let syncContext = System.Threading.SynchronizationContext.Current
    ProgressCallback(fun progress ->
        match syncContext with
        | null -> 
            // Handle on current thread
            handleProgress progress
        | context ->
            // Marshal to UI thread
            context.Post((fun _ -> handleProgress progress), null)
    )
```

### Synchronization

```fsharp
// Protect shared resources
let private lockObject = obj()

let threadSafeOperation() =
    lock lockObject (fun () ->
        // Call native function
        critical_native_operation()
    )
```

## Cross-Platform Considerations

### Dynamic Library Loading

```fsharp
module NativeLibrary =
    let private libraryName = 
        if System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then
            "math_operations.dll"
        elif System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux) then
            "libmath_operations.so"
        elif System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX) then
            "libmath_operations.dylib"
        else
            "libmath_operations.so" // Default
```

### Architecture-Specific Considerations

- **Pointer Size**: Use `IntPtr` instead of specific integer types
- **Structure Packing**: Be aware of different alignment rules
- **Endianness**: Consider byte order for binary data

## Debugging Tips

### Common Issues and Solutions

1. **EntryPointNotFoundException**:
   - Check function name spelling
   - Verify calling convention
   - Ensure library is properly linked

2. **AccessViolationException**:
   - Check pointer validity
   - Verify buffer sizes
   - Ensure proper memory management

3. **BadImageFormatException**:
   - Check architecture mismatch (x86 vs x64)
   - Verify library dependencies

### Debugging Tools

```bash
# Check library dependencies (Linux)
ldd build/libmath_operations.so

# Check exported symbols
nm -D build/libmath_operations.so

# Check library loading
LD_DEBUG=libs dotnet run
```

## Testing Strategies

### Mock Native Dependencies

```fsharp
// Interface for testability
type IMathOperations =
    abstract member AddIntegers: int -> int -> int
    abstract member MultiplyFloats: float32 -> float32 -> float32

// Production implementation
type NativeMathOperations() =
    interface IMathOperations with
        member _.AddIntegers a b = add_integers a b
        member _.MultiplyFloats a b = multiply_floats a b

// Test implementation
type MockMathOperations() =
    interface IMathOperations with
        member _.AddIntegers a b = a + b
        member _.MultiplyFloats a b = a * b
```

### Integration Testing

```fsharp
[<Fact>]
let ``Integration test with actual library`` () =
    // Skip test if library is not available
    let skipIfUnavailable() =
        try add_integers 1 1 |> ignore
        with _ -> Skip.If(true, "Native library not available")
    
    skipIfUnavailable()
    
    // Perform comprehensive testing
    let results = [
        add_integers 1 2
        add_integers -1 1
        add_integers 0 0
    ]
    
    Assert.Equal([3; 0; 0], results)
```

## Advanced Use Cases

### Streaming Data

For continuous data processing:

```fsharp
type StreamProcessor() =
    let bufferSize = 4096
    let buffer = Array.zeroCreate<byte> bufferSize
    
    member _.ProcessStream(stream: System.IO.Stream) =
        use handle = GCHandle.Alloc(buffer, GCHandleType.Pinned)
        let ptr = handle.AddrOfPinnedObject()
        
        let rec loop() =
            let bytesRead = stream.Read(buffer, 0, bufferSize)
            if bytesRead > 0 then
                // Process buffer with native function
                process_buffer(ptr, bytesRead)
                loop()
        
        loop()
```

### Event-Driven Architecture

```fsharp
type NativeEventHandler() =
    let event = new Event<int>()
    
    // Native callback that triggers F# event
    let callback = EventCallback(fun data -> event.Trigger(data))
    
    member _.DataReceived = event.Publish
    
    member _.Start() =
        register_event_handler(callback)
    
    member _.Stop() =
        unregister_event_handler(callback)
```