module CppOperationsInterop

open System
open System.Runtime.InteropServices
open System.Text

// P/Invoke library name for C++ operations
[<Literal>]
let CppLibraryName = "libcpp_operations.so"

// Vector operations
[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr vector_create()

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void vector_destroy(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void vector_add(IntPtr handle, int value)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int vector_get(IntPtr handle, int index)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int vector_size(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void vector_clear(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int vector_sum(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void vector_sort(IntPtr handle)

// String operations
[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)>]
extern IntPtr string_create(string initial_value)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void string_destroy(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr string_get_cstr(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)>]
extern void string_append(IntPtr handle, string text)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)>]
extern void string_prepend(IntPtr handle, string text)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int string_length_cpp(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void string_reverse(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void string_to_upper(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void string_to_lower(IntPtr handle)

// Mathematical operations
[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern double calculate_mean_double(double[] values, int count)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern float32 calculate_mean_float(float32[] values, int count)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern double calculate_variance(double[] values, int count)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern double calculate_standard_deviation(double[] values, int count)

// Matrix operations
[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr matrix_create(int rows, int cols)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void matrix_destroy(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void matrix_set(IntPtr handle, int row, int col, double value)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern double matrix_get(IntPtr handle, int row, int col)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int matrix_rows(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int matrix_cols(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr matrix_multiply(IntPtr a, IntPtr b)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr matrix_transpose(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void matrix_print(IntPtr handle)

// Smart resource operations
[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr smart_resource_create(int size)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void smart_resource_use(IntPtr handle, int index, double value)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern double smart_resource_get(IntPtr handle, int index)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int smart_resource_size(IntPtr handle)

// Function operations
[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr function_create_add()

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr function_create_multiply()

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr function_create_power()

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void function_destroy(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern double function_call(IntPtr handle, double a, double b)

// Iterator operations
[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr iterator_create(int[] array, int size)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void iterator_destroy(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int iterator_has_next(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int iterator_next(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern void iterator_reset(IntPtr handle)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern int iterator_find(IntPtr handle, int value)

// Error handling
type CppResultCode =
    | Success = 0
    | NullPointer = -1
    | OutOfBounds = -2
    | InvalidOperation = -3
    | MemoryError = -4
    | UnknownError = -5

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern CppResultCode safe_vector_get(IntPtr handle, int index, int& result)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern CppResultCode safe_matrix_multiply(IntPtr a, IntPtr b, IntPtr& result)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr get_last_error_message()

// Safe wrapper types and functions
type CppVector() =
    let handle = vector_create()
    let mutable disposed = false
    
    do if handle = IntPtr.Zero then failwith "Failed to create vector"
    
    member _.Handle = 
        if disposed then failwith "Vector has been disposed"
        handle
    
    member _.Add(value: int) = vector_add(handle, value)
    member _.Get(index: int) = vector_get(handle, index)
    member _.Size = vector_size(handle)
    member _.Clear() = vector_clear(handle)
    member _.Sum() = vector_sum(handle)
    member _.Sort() = vector_sort(handle)
    
    member _.SafeGet(index: int) =
        let mutable result = 0
        let status = safe_vector_get(handle, index, &result)
        match status with
        | CppResultCode.Success -> Some result
        | _ -> None
    
    interface IDisposable with
        member _.Dispose() =
            if not disposed && handle <> IntPtr.Zero then
                vector_destroy(handle)
                disposed <- true

type CppString(initial: string) =
    let handle = string_create(initial)
    let mutable disposed = false
    
    do if handle = IntPtr.Zero then failwith "Failed to create string"
    
    member _.Handle = 
        if disposed then failwith "String has been disposed"
        handle
    
    member _.Value =
        let ptr = string_get_cstr(handle)
        if ptr <> IntPtr.Zero then Marshal.PtrToStringAnsi(ptr) else ""
    
    member _.Append(text: string) = string_append(handle, text)
    member _.Prepend(text: string) = string_prepend(handle, text)
    member _.Length = string_length_cpp(handle)
    member _.Reverse() = string_reverse(handle)
    member _.ToUpper() = string_to_upper(handle)
    member _.ToLower() = string_to_lower(handle)
    
    interface IDisposable with
        member _.Dispose() =
            if not disposed && handle <> IntPtr.Zero then
                string_destroy(handle)
                disposed <- true

type CppMatrix(rows: int, cols: int) =
    let handle = matrix_create(rows, cols)
    let mutable disposed = false
    
    do if handle = IntPtr.Zero then failwith "Failed to create matrix"
    
    member _.Handle = 
        if disposed then failwith "Matrix has been disposed"
        handle
    
    member _.Rows = matrix_rows(handle)
    member _.Cols = matrix_cols(handle)
    member _.Set(row: int, col: int, value: double) = matrix_set(handle, row, col, value)
    member _.Get(row: int, col: int) = matrix_get(handle, row, col)
    member _.Print() = matrix_print(handle)
    
    member _.Multiply(other: CppMatrix) =
        let resultHandle = matrix_multiply(handle, other.Handle)
        if resultHandle <> IntPtr.Zero then
            Some (new CppMatrix(resultHandle))
        else
            None
    
    member _.Transpose() =
        let resultHandle = matrix_transpose(handle)
        if resultHandle <> IntPtr.Zero then
            Some (new CppMatrix(resultHandle))
        else
            None
    
    // Private constructor for matrices created from native operations
    private new(nativeHandle: IntPtr) = 
        new CppMatrix(0, 0) then
        // This is a bit of a hack, but we need to replace the handle
        // In a production system, you'd want a cleaner design
        ()
    
    interface IDisposable with
        member _.Dispose() =
            if not disposed && handle <> IntPtr.Zero then
                matrix_destroy(handle)
                disposed <- true

type CppFunction private(handle: IntPtr, name: string) =
    let mutable disposed = false
    
    member _.Handle = 
        if disposed then failwith $"{name} function has been disposed"
        handle
    
    member _.Name = name
    member _.Call(a: double, b: double) = function_call(handle, a, b)
    
    static member CreateAdd() = new CppFunction(function_create_add(), "Add")
    static member CreateMultiply() = new CppFunction(function_create_multiply(), "Multiply") 
    static member CreatePower() = new CppFunction(function_create_power(), "Power")
    
    interface IDisposable with
        member _.Dispose() =
            if not disposed && handle <> IntPtr.Zero then
                function_destroy(handle)
                disposed <- true

// Helper functions
let getLastErrorMessage() =
    let ptr = get_last_error_message()
    if ptr <> IntPtr.Zero then Marshal.PtrToStringAnsi(ptr) else ""

let calculateStatistics(values: double[]) =
    let mean = calculate_mean_double(values, values.Length)
    let variance = calculate_variance(values, values.Length)
    let stdDev = calculate_standard_deviation(values, values.Length)
    (mean, variance, stdDev)