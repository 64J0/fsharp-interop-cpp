# Example Use Cases and Extensions

This document provides additional examples and ideas for extending the F# C++ interop project with real-world scenarios.

## Real-World Use Cases

### 1. Mathematical Libraries Integration

Integrate with scientific computing libraries like BLAS, LAPACK, or custom mathematical libraries.

**Example: Matrix Operations**

```c
// In C library
typedef struct {
    double* data;
    int rows;
    int cols;
} Matrix;

Matrix* create_matrix(int rows, int cols);
void destroy_matrix(Matrix* m);
Matrix* multiply_matrices(const Matrix* a, const Matrix* b);
double matrix_determinant(const Matrix* m);
```

```fsharp
// F# bindings
[<StructLayout(LayoutKind.Sequential)>]
type Matrix = {
    data: IntPtr
    rows: int
    cols: int
}

[<DllImport(LibraryName)>]
extern IntPtr create_matrix(int rows, int cols)

[<DllImport(LibraryName)>]
extern void destroy_matrix(IntPtr matrix)

// Safe wrapper
type SafeMatrix(rows: int, cols: int) =
    let handle = create_matrix(rows, cols)
    
    member _.Handle = handle
    member _.Rows = rows
    member _.Cols = cols
    
    interface IDisposable with
        member _.Dispose() = destroy_matrix(handle)
```

### 2. Image Processing Integration

Integrate with image processing libraries like OpenCV or custom image manipulation code.

**Example: Basic Image Operations**

```c
// C image processing functions
typedef struct {
    unsigned char* pixels;
    int width;
    int height;
    int channels;
} Image;

Image* load_image(const char* filename);
void save_image(const Image* img, const char* filename);
void apply_blur(Image* img, float radius);
void apply_brightness(Image* img, float factor);
void free_image(Image* img);
```

```fsharp
// F# image processing wrapper
type ImageProcessor() =
    
    [<DllImport(LibraryName, CharSet = CharSet.Ansi)>]
    static extern IntPtr load_image(string filename)
    
    [<DllImport(LibraryName, CharSet = CharSet.Ansi)>]
    static extern void save_image(IntPtr img, string filename)
    
    [<DllImport(LibraryName)>]
    static extern void apply_blur(IntPtr img, float radius)
    
    [<DllImport(LibraryName)>]
    static extern void free_image(IntPtr img)
    
    static member ProcessImage(inputPath: string, outputPath: string, blurRadius: float) =
        let img = load_image(inputPath)
        if img <> IntPtr.Zero then
            try
                apply_blur(img, blurRadius)
                save_image(img, outputPath)
            finally
                free_image(img)
```

### 3. Audio Processing

Integrate with audio libraries for real-time audio processing.

**Example: Audio Effects**

```c
// Audio processing
typedef struct {
    float* samples;
    int sample_count;
    int sample_rate;
} AudioBuffer;

void apply_reverb(AudioBuffer* buffer, float room_size, float damping);
void apply_eq(AudioBuffer* buffer, float* band_gains, int band_count);
float calculate_rms(const AudioBuffer* buffer);
```

```fsharp
// F# audio processing
[<StructLayout(LayoutKind.Sequential)>]
type AudioBuffer = {
    samples: IntPtr
    sample_count: int
    sample_rate: int
}

type AudioProcessor() =
    [<DllImport(LibraryName)>]
    static extern void apply_reverb(AudioBuffer& buffer, float room_size, float damping)
    
    [<DllImport(LibraryName)>]
    static extern float calculate_rms(AudioBuffer& buffer)
    
    static member ProcessAudio(audioData: float[], sampleRate: int) =
        use handle = GCHandle.Alloc(audioData, GCHandleType.Pinned)
        let audioBuffer = {
            samples = handle.AddrOfPinnedObject()
            sample_count = audioData.Length
            sample_rate = sampleRate
        }
        
        apply_reverb(&audioBuffer, 0.5f, 0.3f)
        let rms = calculate_rms(&audioBuffer)
        rms
```

### 4. Database Integration

Integrate with high-performance database libraries or custom storage engines.

**Example: Key-Value Store**

```c
// Simple key-value store
typedef struct KVStore KVStore;

KVStore* kvstore_create(const char* db_path);
void kvstore_destroy(KVStore* store);
int kvstore_put(KVStore* store, const char* key, const void* value, size_t value_size);
int kvstore_get(KVStore* store, const char* key, void* value_buffer, size_t buffer_size);
int kvstore_delete(KVStore* store, const char* key);
```

```fsharp
// F# database wrapper
type KeyValueStore(dbPath: string) =
    let handle = kvstore_create(dbPath)
    
    [<DllImport(LibraryName, CharSet = CharSet.Ansi)>]
    static extern IntPtr kvstore_create(string db_path)
    
    [<DllImport(LibraryName)>]
    static extern void kvstore_destroy(IntPtr store)
    
    [<DllImport(LibraryName, CharSet = CharSet.Ansi)>]
    static extern int kvstore_put(IntPtr store, string key, IntPtr value, UIntPtr value_size)
    
    [<DllImport(LibraryName, CharSet = CharSet.Ansi)>]
    static extern int kvstore_get(IntPtr store, string key, IntPtr value_buffer, UIntPtr buffer_size)
    
    member _.Put(key: string, value: byte[]) =
        use handle = GCHandle.Alloc(value, GCHandleType.Pinned)
        kvstore_put(handle, key, handle.AddrOfPinnedObject(), UIntPtr(uint64 value.Length))
    
    interface IDisposable with
        member _.Dispose() = kvstore_destroy(handle)
```

## Performance Optimization Examples

### 1. Bulk Data Processing

**Inefficient: Multiple P/Invoke calls**
```fsharp
// Don't do this - too many P/Invoke calls
let processArray (data: int[]) =
    data |> Array.map (fun x -> process_single_value(x))
```

**Efficient: Single bulk operation**
```fsharp
// Do this instead - single P/Invoke call
let processArrayBulk (data: int[]) =
    use handle = GCHandle.Alloc(data, GCHandleType.Pinned)
    process_array_bulk(handle.AddrOfPinnedObject(), data.Length)
```

### 2. String Buffer Management

**Safe string buffer operations:**
```fsharp
let processStringsEfficiently (strings: string[]) =
    // Pre-allocate buffer for results
    let bufferSize = 1024
    let buffer = Array.zeroCreate<byte> bufferSize
    
    use handle = GCHandle.Alloc(buffer, GCHandleType.Pinned)
    let bufferPtr = handle.AddrOfPinnedObject()
    
    strings
    |> Array.map (fun str ->
        let result = process_string_to_buffer(str, bufferPtr, bufferSize)
        if result > 0 then
            Encoding.UTF8.GetString(buffer, 0, result)
        else
            ""
    )
```

## Advanced Error Handling Patterns

### 1. Result Type Integration

```fsharp
type NativeResult<'T> =
    | Success of 'T
    | NativeError of string
    | SystemError of exn

let safeNativeCall<'T> (operation: unit -> 'T) : NativeResult<'T> =
    try
        let result = operation()
        Success result
    with
    | :? DllNotFoundException as ex -> 
        NativeError $"Library not found: {ex.Message}"
    | :? EntryPointNotFoundException as ex ->
        NativeError $"Function not found: {ex.Message}"
    | :? AccessViolationException as ex ->
        NativeError $"Memory access violation: {ex.Message}"
    | ex ->
        SystemError ex

// Usage
let addSafely x y =
    safeNativeCall (fun () -> add_integers(x, y))
    |> function
        | Success result -> Some result
        | NativeError msg -> printfn "Native error: %s" msg; None
        | SystemError ex -> printfn "System error: %s" ex.Message; None
```

### 2. Resource Management Patterns

```fsharp
// Generic resource manager for native handles
type NativeResource<'THandle when 'THandle : equality>(
    allocator: unit -> 'THandle, 
    deallocator: 'THandle -> unit,
    nullValue: 'THandle) =
    
    let handle = allocator()
    let mutable disposed = false
    
    do if handle = nullValue then failwith "Failed to allocate native resource"
    
    member _.Handle = 
        if disposed then failwith "Resource has been disposed"
        handle
    
    member _.IsValid = not disposed && handle <> nullValue
    
    interface IDisposable with
        member _.Dispose() =
            if not disposed then
                deallocator handle
                disposed <- true

// Usage example
let createManagedString length =
    new NativeResource<IntPtr>(
        allocator = (fun () -> allocate_string(length)),
        deallocator = free_string,
        nullValue = IntPtr.Zero
    )
```

## Testing Strategies

### 1. Property-Based Testing

```fsharp
open FsCheck

// Property: addition should be commutative
let additionCommutative x y =
    add_integers(x, y) = add_integers(y, x)

// Property: string length should match .NET implementation
let stringLengthCorrect (NonNull str) =
    string_length(str) = str.Length

[<Test>]
let ``Native operations satisfy mathematical properties`` () =
    Check.Quick additionCommutative
    Check.Quick stringLengthCorrect
```

### 2. Integration Testing with Mocks

```fsharp
// Abstract interface for testability
type IMathOperations =
    abstract member Add: int -> int -> int
    abstract member Multiply: float32 -> float32 -> float32

// Native implementation
type NativeMathOperations() =
    interface IMathOperations with
        member _.Add x y = add_integers(x, y)
        member _.Multiply x y = multiply_floats(x, y)

// Mock implementation for testing
type MockMathOperations() =
    interface IMathOperations with
        member _.Add x y = x + y
        member _.Multiply x y = x * y

// Testable business logic
type Calculator(math: IMathOperations) =
    member _.CalculateCompoundInterest principal rate years =
        let rateDecimal = math.Multiply(rate, 0.01f)
        // Complex calculation using math operations
        float principal * (1.0 + float rateDecimal) ** float years
```

## Deployment Considerations

### 1. Cross-Platform Library Loading

```fsharp
module LibraryLoader =
    let private getLibraryName() =
        if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then
            "math_operations.dll"
        elif RuntimeInformation.IsOSPlatform(OSPlatform.Linux) then
            "libmath_operations.so"
        elif RuntimeInformation.IsOSPlatform(OSPlatform.OSX) then
            "libmath_operations.dylib"
        else
            "libmath_operations.so"
    
    [<Literal>]
    let LibraryName = "math_operations"  // Let runtime resolve the extension
```

### 2. NuGet Packaging

Create a NuGet package structure:
```
MyNativeLib.1.0.0.nupkg
├── lib/
│   └── net8.0/
│       └── MyNativeLib.dll
├── runtimes/
│   ├── win-x64/native/math_operations.dll
│   ├── linux-x64/native/libmath_operations.so
│   └── osx-x64/native/libmath_operations.dylib
└── MyNativeLib.nuspec
```

### 3. Docker Deployment

```dockerfile
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# Copy native libraries
COPY build/libmath_operations.so /usr/local/lib/
RUN ldconfig

# Copy application
COPY bin/Release/net8.0/ ./
ENTRYPOINT ["dotnet", "FSharpCppInterop.dll"]
```

These examples demonstrate how the basic interop concepts can be extended to real-world scenarios with proper error handling, resource management, and deployment considerations.