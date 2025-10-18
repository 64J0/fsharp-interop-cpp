module MathOperationsInterop

open System
open System.Runtime.InteropServices
open System.Text
open System.Buffers
open Microsoft.Win32.SafeHandles

// Library wrapper class for better naming practices
type LibMathOperations = class end

// P/Invoke library name - will be resolved at runtime
[<Literal>]
let LibraryName = "libmath_operations.so"

// Note: While LibraryImport is preferred for .NET 7+, F# currently has runtime 
// limitations with LibraryImport source generation. Additionally, most operations
// require CallingConvention.Cdecl and CharSet.Ansi which have limited LibraryImport support.
// Using DllImport ensures reliable F# interop while following other P/Invoke best practices.

// Basic primitive type operations - DllImport required for F# runtime compatibility
[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int add_integers(int a, int b)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern float32 multiply_floats(float32 a, float32 b)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern double divide_doubles(double a, double b)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int is_even(int number)

// String operations with proper marshaling
[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)>]
extern int string_length(string str)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)>]
extern void copy_string(string source, StringBuilder destination, int max_length)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)>]
extern IntPtr get_greeting(string name)

// Struct operations
[<Struct>]
[<StructLayout(LayoutKind.Sequential)>]
type Point = {
    x: int
    y: int
}

[<Struct>]
[<StructLayout(LayoutKind.Sequential)>]
type Rectangle = {
    width: float32
    height: float32
}

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern Point create_point(int x, int y)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern float32 calculate_distance(Point p1, Point p2)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern Rectangle create_rectangle(float32 width, float32 height)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern float32 rectangle_area(Rectangle rect)

// Array operations with proper In/Out attributes
[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void fill_array([<In; Out>] int[] array, int size, int value)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int sum_array([<In>] int[] array, int size)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void sort_array([<In; Out>] int[] array, int size)

// Function pointer for callback (preferred over delegate for P/Invoke)
[<UnmanagedFunctionPointer(CallingConvention.Cdecl)>]
type ProgressCallbackDelegate = delegate of int -> unit

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void simulate_work(int duration_ms, ProgressCallbackDelegate callback)

// Error handling
type ResultCode =
    | Success = 0
    | NullPointer = -1
    | InvalidParameter = -2
    | BufferTooSmall = -3

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern ResultCode safe_divide(double a, double b, double& result)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern ResultCode validate_array([<In>] int[] array, int size)

// Memory management
[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr allocate_string(int length)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void free_string(IntPtr str)

// SafeHandle for managed memory lifetime
type SafeStringHandle(length: int) =
    inherit SafeHandleZeroOrMinusOneIsInvalid(true)
    
    do 
        let ptr = allocate_string(length)
        base.SetHandle(ptr)
            
    override this.ReleaseHandle() =
        if not this.IsInvalid then
            free_string(this.handle)
        true

// Helper functions for safer string operations
let getGreeting name =
    let ptr = get_greeting(name)
    if ptr <> IntPtr.Zero then
        Marshal.PtrToStringAnsi(ptr)
    else
        null

let copyStringSafe source maxLength =
    let buffer = new StringBuilder(maxLength: int)
    copy_string(source, buffer, maxLength)
    buffer.ToString()

// Safe wrapper for memory operations using SafeHandle
let withAllocatedString length (action: IntPtr -> 'T) =
    use safeHandle = new SafeStringHandle(length)
    if not safeHandle.IsInvalid then
        action (safeHandle.DangerousGetHandle())
    else
        failwith "Failed to allocate string"

// ArrayPool-based helpers for better array performance
let withPooledArray<'T> minLength (action: 'T[] -> 'U) : 'U =
    let pool = ArrayPool<'T>.Shared
    let array = pool.Rent(minLength)
    try
        action array
    finally
        pool.Return(array)

// Safe array sum using ArrayPool
let sumArraySafe (values: int[]) =
    if values.Length <= 1000 then
        // Use original array for small arrays
        sum_array(values, values.Length)
    else
        // Use pooled array for large arrays to avoid GC pressure
        withPooledArray values.Length (fun pooledArray ->
            Array.Copy(values, pooledArray, values.Length)
            sum_array(pooledArray, values.Length)
        )