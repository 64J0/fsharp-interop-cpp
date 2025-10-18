module CppOperationsInterop

open System
open System.Runtime.InteropServices
open System.Text
open System.Buffers
open Microsoft.Win32.SafeHandles

// Library wrapper class for better naming practices
type LibCppOperations = class end

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

// Mathematical operations with proper array marshaling
[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern double calculate_mean_double([<In>] double[] values, int count)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern float32 calculate_mean_float([<In>] float32[] values, int count)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern double calculate_variance([<In>] double[] values, int count)

[<DllImport(CppLibraryName, CallingConvention = CallingConvention.Cdecl)>]
extern double calculate_standard_deviation([<In>] double[] values, int count)

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
extern IntPtr iterator_create([<In>] int[] array, int size)

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

// SafeHandle implementations for better resource management
type SafeVectorHandle() =
    inherit SafeHandleZeroOrMinusOneIsInvalid(true)
    
    do 
        let ptr = vector_create()
        base.SetHandle(ptr)
            
    override this.ReleaseHandle() =
        if not this.IsInvalid then
            vector_destroy(this.handle)
        true

type SafeStringCppHandle(initial: string) =
    inherit SafeHandleZeroOrMinusOneIsInvalid(true)
    
    do 
        let ptr = string_create(initial)
        base.SetHandle(ptr)
            
    override this.ReleaseHandle() =
        if not this.IsInvalid then
            string_destroy(this.handle)
        true

type SafeMatrixHandle(rows: int, cols: int) =
    inherit SafeHandleZeroOrMinusOneIsInvalid(true)
    
    do 
        let ptr = matrix_create(rows, cols)
        base.SetHandle(ptr)
            
    override this.ReleaseHandle() =
        if not this.IsInvalid then
            matrix_destroy(this.handle)
        true

// SafeHandle for adopting existing matrix pointers
type SafeMatrixHandleFromPtr(existingPtr: IntPtr) =
    inherit SafeHandleZeroOrMinusOneIsInvalid(true)
    
    do base.SetHandle(existingPtr)
            
    override this.ReleaseHandle() =
        if not this.IsInvalid then
            matrix_destroy(this.handle)
        true

type SafeFunctionHandle private (ptr: IntPtr) =
    inherit SafeHandleZeroOrMinusOneIsInvalid(true)
    
    do base.SetHandle(ptr)
    
    static member CreateAdd() = new SafeFunctionHandle(function_create_add())
    static member CreateMultiply() = new SafeFunctionHandle(function_create_multiply())
    static member CreatePower() = new SafeFunctionHandle(function_create_power())
            
    override this.ReleaseHandle() =
        if not this.IsInvalid then
            function_destroy(this.handle)
        true

// Safe wrapper types and functions using SafeHandles
type CppVector() =
    let safeHandle = new SafeVectorHandle()
    
    do if safeHandle.IsInvalid then failwith "Failed to create vector"
    
    member _.Handle = 
        if safeHandle.IsInvalid then failwith "Vector has been disposed"
        safeHandle.DangerousGetHandle()
    
    member this.Add(value: int) = vector_add(this.Handle, value)
    member this.Get(index: int) = vector_get(this.Handle, index)
    member this.Size = vector_size(this.Handle)
    member this.Clear() = vector_clear(this.Handle)
    member this.Sum() = vector_sum(this.Handle)
    member this.Sort() = vector_sort(this.Handle)
    
    member this.SafeGet(index: int) =
        let mutable result = 0
        let status = safe_vector_get(this.Handle, index, &result)
        match status with
        | CppResultCode.Success -> Some result
        | _ -> None
    
    interface IDisposable with
        member _.Dispose() = safeHandle.Dispose()

type CppString(initial: string) =
    let safeHandle = new SafeStringCppHandle(initial)
    
    do if safeHandle.IsInvalid then failwith "Failed to create string"
    
    member _.Handle = 
        if safeHandle.IsInvalid then failwith "String has been disposed"
        safeHandle.DangerousGetHandle()
    
    member this.Value =
        let ptr = string_get_cstr(this.Handle)
        if ptr <> IntPtr.Zero then Marshal.PtrToStringAnsi(ptr) else ""
    
    member this.Append(text: string) = string_append(this.Handle, text)
    member this.Prepend(text: string) = string_prepend(this.Handle, text)
    member this.Length = string_length_cpp(this.Handle)
    member this.Reverse() = string_reverse(this.Handle)
    member this.ToUpper() = string_to_upper(this.Handle)
    member this.ToLower() = string_to_lower(this.Handle)
    
    interface IDisposable with
        member _.Dispose() = safeHandle.Dispose()

type CppMatrix private (safeHandle: SafeHandleZeroOrMinusOneIsInvalid) =
    // Public constructor
    new(rows: int, cols: int) = 
        let handle = new SafeMatrixHandle(rows, cols)
        if handle.IsInvalid then failwith "Failed to create matrix"
        new CppMatrix(handle :> SafeHandleZeroOrMinusOneIsInvalid)
    
    // Constructor for adopting existing pointers
    internal new(existingHandle: SafeMatrixHandleFromPtr) = 
        new CppMatrix(existingHandle :> SafeHandleZeroOrMinusOneIsInvalid)
    
    member _.Handle = 
        if safeHandle.IsInvalid then failwith "Matrix has been disposed"
        safeHandle.DangerousGetHandle()
    
    member this.Rows = matrix_rows(this.Handle)
    member this.Cols = matrix_cols(this.Handle)
    member this.Set(row: int, col: int, value: double) = matrix_set(this.Handle, row, col, value)
    member this.Get(row: int, col: int) = matrix_get(this.Handle, row, col)
    member this.Print() = matrix_print(this.Handle)
    
    member this.Multiply(other: CppMatrix) =
        let resultHandle = matrix_multiply(this.Handle, other.Handle)
        if resultHandle <> IntPtr.Zero then
            // Create a SafeHandle that adopts the existing pointer
            let newSafeHandle = new SafeMatrixHandleFromPtr(resultHandle)
            Some (new CppMatrix(newSafeHandle))
        else
            None
    
    member this.Transpose() =
        let resultHandle = matrix_transpose(this.Handle)
        if resultHandle <> IntPtr.Zero then
            // Create a SafeHandle that adopts the existing pointer
            let newSafeHandle = new SafeMatrixHandleFromPtr(resultHandle)
            Some (new CppMatrix(newSafeHandle))
        else
            None
    
    interface IDisposable with
        member _.Dispose() = safeHandle.Dispose()

type CppFunction private(safeHandle: SafeFunctionHandle, name: string) =
    member _.Handle = 
        if safeHandle.IsInvalid then failwith $"{name} function has been disposed"
        safeHandle.DangerousGetHandle()
    
    member _.Name = name
    member this.Call(a: double, b: double) = function_call(this.Handle, a, b)
    
    static member CreateAdd() = new CppFunction(SafeFunctionHandle.CreateAdd(), "Add")
    static member CreateMultiply() = new CppFunction(SafeFunctionHandle.CreateMultiply(), "Multiply") 
    static member CreatePower() = new CppFunction(SafeFunctionHandle.CreatePower(), "Power")
    
    interface IDisposable with
        member _.Dispose() = safeHandle.Dispose()

// Helper functions
let getLastErrorMessage() =
    let ptr = get_last_error_message()
    if ptr <> IntPtr.Zero then Marshal.PtrToStringAnsi(ptr) else ""

let calculateStatistics(values: double[]) =
    let mean = calculate_mean_double(values, values.Length)
    let variance = calculate_variance(values, values.Length)
    let stdDev = calculate_standard_deviation(values, values.Length)
    (mean, variance, stdDev)

// ArrayPool-based helpers for better performance with large arrays
let withPooledDoubleArray minLength (action: double[] -> 'U) : 'U =
    let pool = ArrayPool<double>.Shared
    let array = pool.Rent(minLength)
    try
        action array
    finally
        pool.Return(array)

let withPooledFloatArray minLength (action: float32[] -> 'U) : 'U =
    let pool = ArrayPool<float32>.Shared
    let array = pool.Rent(minLength)
    try
        action array
    finally
        pool.Return(array)

// Safe statistical calculations using ArrayPool for large datasets
let calculateStatisticsSafe(values: double[]) =
    if values.Length <= 1000 then
        // Use original array for small arrays
        calculateStatistics(values)
    else
        // Use pooled array for large arrays to reduce GC pressure
        withPooledDoubleArray values.Length (fun pooledArray ->
            Array.Copy(values, pooledArray, values.Length)
            let mean = calculate_mean_double(pooledArray, values.Length)
            let variance = calculate_variance(pooledArray, values.Length)
            let stdDev = calculate_standard_deviation(pooledArray, values.Length)
            (mean, variance, stdDev)
        )