open System
open System.IO
open System.Text
open MathOperationsInterop

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
    
    let progressCallback = ProgressCallback(fun progress ->
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

let checkLibraryAvailability() =
    try
        // Try a simple operation to check if the library is loaded
        let _ = add_integers(1, 1)
        true
    with
    | ex -> 
        printfn "Library not available: %s" ex.Message
        printfn "Please make sure libmath_operations.so is built and available."
        printfn "Run 'make' in the project root to build the library."
        false

[<EntryPoint>]
let main argv =
    printfn "F# and C++ Interop Demo"
    printfn "========================="
    
    if checkLibraryAvailability() then
        runBasicOperationsDemo()
        runStringOperationsDemo()
        runStructOperationsDemo()
        runArrayOperationsDemo()
        runCallbackDemo()
        runErrorHandlingDemo()
        runMemoryManagementDemo()
        
        printfn "\n=== Demo Complete ==="
        0 // Success
    else
        1 // Error
