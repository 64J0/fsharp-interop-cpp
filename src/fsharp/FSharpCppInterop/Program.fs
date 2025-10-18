open System
open System.IO
open System.Text
open MathOperationsInterop
open CppOperationsInterop

let runBasicOperationsDemo() =
    printfn "=== Basic Operations Demo ==="
    
    // Integer operations
    let sum = add_integers(10, 20)
    printfn "10 + 20 = %d" sum
    
    // Float operations
    let product = multiply_floats(3.14f, 2.0f)
    printfn "3.14 * 2.0 = %f" product
    
    // Double operations
    let quotient = divide_doubles(10.0, 3.0)
    printfn "10.0 / 3.0 = %f" quotient
    
    // Boolean operations
    let evenCheck1 = is_even(4)
    let evenCheck2 = is_even(7)
    printfn "Is 4 even? %s" (if evenCheck1 = 1 then "Yes" else "No")
    printfn "Is 7 even? %s" (if evenCheck2 = 1 then "Yes" else "No")

let runStringOperationsDemo() =
    printfn "\n=== String Operations Demo ==="
    
    // String length
    let testString = "Hello, World!"
    let length = string_length(testString)
    printfn "Length of '%s': %d" testString length
    
    // String copying
    let copied = copyStringSafe "Original text" 50
    printfn "Copied string: '%s'" copied
    
    // Greeting generation
    let greeting1 = getGreeting "Alice"
    let greeting2 = getGreeting null
    printfn "Greeting: %s" greeting1
    printfn "Default greeting: %s" greeting2

let runStructOperationsDemo() =
    printfn "\n=== Struct Operations Demo ==="
    
    // Create points
    let point1 = create_point(0, 0)
    let point2 = create_point(3, 4)
    printfn "Point 1: (%d, %d)" point1.x point1.y
    printfn "Point 2: (%d, %d)" point2.x point2.y
    
    // Calculate distance
    let distance = calculate_distance(point1, point2)
    printfn "Distance between points: %f" distance
    
    // Rectangle operations
    let rect = create_rectangle(5.0f, 10.0f)
    let area = rectangle_area(rect)
    printfn "Rectangle: %f x %f, Area: %f" rect.width rect.height area

let runArrayOperationsDemo() =
    printfn "\n=== Array Operations Demo ==="
    
    // Fill array
    let array = Array.zeroCreate<int> 5
    fill_array(array, array.Length, 42)
    printfn "Filled array: [%s]" (String.Join(", ", array))
    
    // Sum array
    let numbers = [| 1; 2; 3; 4; 5 |]
    let sum = sum_array(numbers, numbers.Length)
    printfn "Sum of [%s]: %d" (String.Join(", ", numbers)) sum
    
    // Sort array
    let unsorted = [| 5; 2; 8; 1; 9 |]
    printfn "Before sorting: [%s]" (String.Join(", ", unsorted))
    sort_array(unsorted, unsorted.Length)
    printfn "After sorting: [%s]" (String.Join(", ", unsorted))

let runCallbackDemo() =
    printfn "\n=== Callback Demo ==="
    printfn "Simulating work with progress callbacks..."
    
    let progressCallback = ProgressCallbackDelegate(fun progress ->
        if progress % 20 = 0 then // Show every 20%
            printfn "Progress: %d%%" progress
    )
    
    simulate_work(2000, progressCallback) // 2 seconds
    printfn "Work completed!"

let runErrorHandlingDemo() =
    printfn "\n=== Error Handling Demo ==="
    
    // Safe division
    let mutable result = 0.0
    let status1 = safe_divide(10.0, 2.0, &result)
    match status1 with
    | ResultCode.Success -> printfn "10.0 / 2.0 = %f" result
    | _ -> printfn "Division failed: %A" status1
    
    let status2 = safe_divide(10.0, 0.0, &result)
    match status2 with
    | ResultCode.Success -> printfn "10.0 / 0.0 = %f" result
    | ResultCode.InvalidParameter -> printfn "Error: Division by zero!"
    | _ -> printfn "Division failed: %A" status2
    
    // Array validation
    let validArray = [| 1; 2; 3 |]
    let validStatus = validate_array(validArray, validArray.Length)
    printfn "Valid array validation: %A" validStatus
    
    let emptyStatus = validate_array(validArray, 0)
    printfn "Empty array validation: %A" emptyStatus

let runMemoryManagementDemo() =
    printfn "\n=== Memory Management Demo ==="
    
    // Safe string allocation example
    try
        withAllocatedString 100 (fun ptr ->
            printfn "Allocated string at: %A" ptr
            // In a real scenario, you would write to this memory
            // For demo purposes, we'll just show the pointer
            "Success"
        ) |> printfn "Operation result: %s"
    with
    | ex -> printfn "Memory operation failed: %s" ex.Message

let runCppVectorDemo() =
    printfn "\n=== C++ Vector Demo ==="
    
    use vector = new CppVector()
    vector.Add(10)
    vector.Add(5)
    vector.Add(20)
    vector.Add(1)
    
    printfn "Vector size: %d" vector.Size
    printfn "Vector sum: %d" (vector.Sum())
    printfn "Element at index 2: %d" (vector.Get(2))
    
    // Safe access
    match vector.SafeGet(1) with
    | Some value -> printfn "Safe access at index 1: %d" value
    | None -> printfn "Safe access failed"
    
    match vector.SafeGet(10) with
    | Some value -> printfn "Safe access at index 10: %d" value
    | None -> printfn "Safe access at index 10: Out of bounds"
    
    vector.Sort()
    printfn "After sorting:"
    for i in 0 .. vector.Size - 1 do
        printfn "  [%d] = %d" i (vector.Get(i))

let runCppStringDemo() =
    printfn "\n=== C++ String Demo ==="
    
    use str = new CppString("Hello")
    printfn "Initial string: '%s'" str.Value
    printfn "Length: %d" str.Length
    
    str.Append(" World")
    printfn "After append: '%s'" str.Value
    
    str.Prepend("C++ ")
    printfn "After prepend: '%s'" str.Value
    
    str.ToUpper()
    printfn "Upper case: '%s'" str.Value
    
    str.ToLower()
    printfn "Lower case: '%s'" str.Value
    
    str.Reverse()
    printfn "Reversed: '%s'" str.Value

let runCppMathDemo() =
    printfn "\n=== C++ Mathematical Operations Demo ==="
    
    let values = [| 1.0; 2.0; 3.0; 4.0; 5.0; 6.0; 7.0; 8.0; 9.0; 10.0 |]
    let (mean, variance, stdDev) = calculateStatistics(values)
    
    printfn "Data: [%s]" (String.Join(", ", values))
    printfn "Mean: %f" mean
    printfn "Variance: %f" variance
    printfn "Standard Deviation: %f" stdDev
    
    let floatValues = [| 2.5f; 4.5f; 6.5f; 8.5f |]
    let floatMean = calculate_mean_float(floatValues, floatValues.Length)
    printfn "Float mean of [%s]: %f" (String.Join(", ", floatValues)) (float floatMean)

let runCppMatrixDemo() =
    printfn "\n=== C++ Matrix Demo ==="
    
    use matrix1 = new CppMatrix(2, 3)
    matrix1.Set(0, 0, 1.0)
    matrix1.Set(0, 1, 2.0)
    matrix1.Set(0, 2, 3.0)
    matrix1.Set(1, 0, 4.0)
    matrix1.Set(1, 1, 5.0)
    matrix1.Set(1, 2, 6.0)
    
    printfn "Matrix 1 (%dx%d):" matrix1.Rows matrix1.Cols
    matrix1.Print()
    
    use matrix2 = new CppMatrix(3, 2)
    matrix2.Set(0, 0, 1.0)
    matrix2.Set(0, 1, 2.0)
    matrix2.Set(1, 0, 3.0)
    matrix2.Set(1, 1, 4.0)
    matrix2.Set(2, 0, 5.0)
    matrix2.Set(2, 1, 6.0)
    
    printfn "\nMatrix 2 (%dx%d):" matrix2.Rows matrix2.Cols
    matrix2.Print()
    
    match matrix1.Multiply(matrix2) with
    | Some result ->
        use resultMatrix = result
        printfn "\nMatrix multiplication result (%dx%d):" resultMatrix.Rows resultMatrix.Cols
        resultMatrix.Print()
    | None ->
        printfn "Matrix multiplication failed: %s" (getLastErrorMessage())

let runCppFunctionDemo() =
    printfn "\n=== C++ Function Objects Demo ==="
    
    use addFunc = CppFunction.CreateAdd()
    use multiplyFunc = CppFunction.CreateMultiply()
    use powerFunc = CppFunction.CreatePower()
    
    let a, b = 3.0, 4.0
    printfn "Using function objects with values a=%.1f, b=%.1f:" a b
    printfn "%s: %.1f" addFunc.Name (addFunc.Call(a, b))
    printfn "%s: %.1f" multiplyFunc.Name (multiplyFunc.Call(a, b))
    printfn "%s: %.1f" powerFunc.Name (powerFunc.Call(a, b))

let checkLibraryAvailability() =
    try
        // Try a simple operation to check if the library is loaded
        let _ = add_integers(1, 1)
        true
    with
    | ex -> 
        printfn "C library not available: %s" ex.Message
        printfn "Please make sure libmath_operations.so is built and available."
        printfn "Run 'make' in the project root to build the library."
        false

let checkCppLibraryAvailability() =
    try
        // Try a simple operation to check if the C++ library is loaded
        use vector = new CppVector()
        vector.Add(1)
        true
    with
    | ex -> 
        printfn "C++ library not available: %s" ex.Message
        printfn "Please make sure libcpp_operations.so is built and available."
        false

[<EntryPoint>]
let main argv =
    printfn "F# and C++ Interop Demo"
    printfn "========================="
    
    let cAvailable = checkLibraryAvailability()
    let cppAvailable = checkCppLibraryAvailability()
    
    if cAvailable then
        printfn "\n### C LIBRARY DEMONSTRATIONS ###"
        runBasicOperationsDemo()
        runStringOperationsDemo()
        runStructOperationsDemo()
        runArrayOperationsDemo()
        runCallbackDemo()
        runErrorHandlingDemo()
        runMemoryManagementDemo()
    
    if cppAvailable then
        printfn "\n### C++ LIBRARY DEMONSTRATIONS ###"
        runCppVectorDemo()
        runCppStringDemo()
        runCppMathDemo()
        runCppMatrixDemo()
        runCppFunctionDemo()
    
    if cAvailable && cppAvailable then
        printfn "\n=== Demo Complete ==="
        printfn "Both C and C++ interop demonstrations completed successfully!"
        0 // Success
    elif cAvailable then
        printfn "\n=== C Demo Complete ==="
        printfn "C interop demonstration completed. C++ library not available."
        0 // Success
    elif cppAvailable then
        printfn "\n=== C++ Demo Complete ==="
        printfn "C++ interop demonstration completed. C library not available."
        0 // Success  
    else
        printfn "\nNeither C nor C++ libraries are available."
        1 // Error
