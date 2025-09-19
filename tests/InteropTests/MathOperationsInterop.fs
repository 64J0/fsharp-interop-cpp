module MathOperationsInterop

open System
open System.Runtime.InteropServices
open System.Text

// P/Invoke library name - will be resolved at runtime
[<Literal>]
let LibraryName = "libmath_operations.so"

// Basic primitive type operations
[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int add_integers(int a, int b)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern float32 multiply_floats(float32 a, float32 b)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern double divide_doubles(double a, double b)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int is_even(int number)

// String operations
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

// Array operations
[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void fill_array(int[] array, int size, int value)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int sum_array(int[] array, int size)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void sort_array(int[] array, int size)

// Callback function type
type ProgressCallback = delegate of int -> unit

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void simulate_work(int duration_ms, ProgressCallback callback)

// Error handling
type ResultCode =
    | Success = 0
    | NullPointer = -1
    | InvalidParameter = -2
    | BufferTooSmall = -3

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern ResultCode safe_divide(double a, double b, double& result)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern ResultCode validate_array(int[] array, int size)

// Memory management
[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr allocate_string(int length)

[<DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void free_string(IntPtr str)

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

// Safe wrapper for memory operations
let withAllocatedString length (action: IntPtr -> 'T) =
    let ptr = allocate_string(length)
    if ptr <> IntPtr.Zero then
        try
            action ptr
        finally
            free_string(ptr)
    else
        failwith "Failed to allocate string"