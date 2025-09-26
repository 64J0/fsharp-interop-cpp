module Tests

open System
open System.Text
open Xunit
open MathOperationsInterop
open CppOperationsInterop

// Helper function to check if library is available
let skipIfLibraryUnavailable() =
    try
        add_integers(1, 1) |> ignore
    with
    | _ -> () // Skip test if library is not available - we'll handle this in the test runner

// Helper function to check if C++ library is available  
let skipIfCppLibraryUnavailable() =
    try
        use vector = new CppVector()
        vector.Add(1)
    with
    | _ -> () // Skip test if C++ library is not available

[<Fact>]
let ``Basic integer addition works correctly`` () =
    skipIfLibraryUnavailable()
    let result = add_integers(10, 20)
    Assert.Equal(30, result)

[<Fact>]
let ``Float multiplication works correctly`` () =
    skipIfLibraryUnavailable()
    let result = multiply_floats(3.5f, 2.0f)
    Assert.Equal(7.0f, result, 0.01f)

[<Fact>]
let ``Double division works correctly`` () =
    skipIfLibraryUnavailable()
    let result = divide_doubles(10.0, 3.0)
    Assert.Equal(3.333333, result, 5)

[<Fact>]
let ``Even number detection works correctly`` () =
    skipIfLibraryUnavailable()
    let result1 = is_even(4)
    let result2 = is_even(7)
    Assert.Equal(1, result1)
    Assert.Equal(0, result2)

[<Fact>]
let ``String length calculation works correctly`` () =
    skipIfLibraryUnavailable()
    let result = string_length("Hello")
    Assert.Equal(5, result)

[<Fact>]
let ``String length handles null correctly`` () =
    skipIfLibraryUnavailable()
    let result = string_length(null)
    Assert.Equal(-1, result)

[<Fact>]
let ``String copying works correctly`` () =
    skipIfLibraryUnavailable()
    let result = copyStringSafe "Test string" 50
    Assert.Equal("Test string", result)

[<Fact>]
let ``Greeting generation works correctly`` () =
    skipIfLibraryUnavailable()
    let result1 = getGreeting "Alice"
    let result2 = getGreeting null
    Assert.Equal("Hello, Alice!", result1)
    Assert.Equal("Hello, stranger!", result2)

[<Fact>]
let ``Point creation works correctly`` () =
    skipIfLibraryUnavailable()
    let point = create_point(3, 4)
    Assert.Equal(3, point.x)
    Assert.Equal(4, point.y)

[<Fact>]
let ``Distance calculation works correctly`` () =
    skipIfLibraryUnavailable()
    let p1 = create_point(0, 0)
    let p2 = create_point(3, 4)
    let distance = calculate_distance(p1, p2)
    Assert.Equal(5.0f, distance, 0.01f)

[<Fact>]
let ``Rectangle operations work correctly`` () =
    skipIfLibraryUnavailable()
    let rect = create_rectangle(5.0f, 10.0f)
    let area = rectangle_area(rect)
    Assert.Equal(5.0f, rect.width)
    Assert.Equal(10.0f, rect.height)
    Assert.Equal(50.0f, area)

[<Fact>]
let ``Array filling works correctly`` () =
    skipIfLibraryUnavailable()
    let array = Array.zeroCreate<int> 5
    fill_array(array, array.Length, 42)
    Assert.All(array, fun x -> Assert.Equal(42, x))

[<Fact>]
let ``Array sum works correctly`` () =
    skipIfLibraryUnavailable()
    let array = [| 1; 2; 3; 4; 5 |]
    let sum = sum_array(array, array.Length)
    Assert.Equal(15, sum)

[<Fact>]
let ``Array sorting works correctly`` () =
    skipIfLibraryUnavailable()
    let array = [| 5; 2; 8; 1; 9 |]
    sort_array(array, array.Length)
    let expected = [| 1; 2; 5; 8; 9 |]
    Assert.Equal<int[]>(expected, array)

[<Fact>]
let ``Safe division works correctly`` () =
    skipIfLibraryUnavailable()
    let mutable result = 0.0
    let status = safe_divide(10.0, 2.0, &result)
    Assert.Equal(ResultCode.Success, status)
    Assert.Equal(5.0, result)

[<Fact>]
let ``Safe division handles division by zero`` () =
    skipIfLibraryUnavailable()
    let mutable result = 0.0
    let status = safe_divide(10.0, 0.0, &result)
    Assert.Equal(ResultCode.InvalidParameter, status)

[<Fact>]
let ``Array validation works correctly`` () =
    skipIfLibraryUnavailable()
    let array = [| 1; 2; 3 |]
    let status1 = validate_array(array, array.Length)
    let status2 = validate_array(array, 0)
    Assert.Equal(ResultCode.Success, status1)
    Assert.Equal(ResultCode.InvalidParameter, status2)

[<Fact>]
let ``Memory management works correctly`` () =
    skipIfLibraryUnavailable()
    // Test that we can allocate and free without crashing
    let result = 
        withAllocatedString 100 (fun ptr ->
            // Just verify we got a valid pointer
            Assert.NotEqual(IntPtr.Zero, ptr)
            "Success"
        )
    Assert.Equal("Success", result)

// C++ Tests
[<Fact>]
let ``C++ Vector operations work correctly`` () =
    skipIfCppLibraryUnavailable()
    use vector = new CppVector()
    vector.Add(10)
    vector.Add(5)
    vector.Add(20)
    
    Assert.Equal(3, vector.Size)
    Assert.Equal(35, vector.Sum())
    Assert.Equal(20, vector.Get(2))

[<Fact>]
let ``C++ String operations work correctly`` () =
    skipIfCppLibraryUnavailable()
    use str = new CppString("Hello")
    Assert.Equal(5, str.Length)
    
    str.Append(" World")
    Assert.Equal("Hello World", str.Value)
    
    str.ToUpper()
    Assert.Equal("HELLO WORLD", str.Value)

[<Fact>]
let ``C++ Mathematical operations work correctly`` () =
    skipIfCppLibraryUnavailable()
    let values = [| 2.0; 4.0; 6.0; 8.0 |]
    let (mean, variance, stdDev) = calculateStatistics(values)
    
    Assert.Equal(5.0, mean, 1)
    Assert.True(variance > 0.0)
    Assert.True(stdDev > 0.0)

[<Fact>]
let ``C++ Function objects work correctly`` () =
    skipIfCppLibraryUnavailable()
    use addFunc = CppFunction.CreateAdd()
    use multFunc = CppFunction.CreateMultiply()
    
    Assert.Equal(7.0, addFunc.Call(3.0, 4.0))
    Assert.Equal(12.0, multFunc.Call(3.0, 4.0))

[<Fact>]  
let ``C++ Matrix basic operations work`` () =
    skipIfCppLibraryUnavailable()
    use matrix = new CppMatrix(2, 2)
    matrix.Set(0, 0, 1.0)
    matrix.Set(0, 1, 2.0)
    matrix.Set(1, 0, 3.0)
    matrix.Set(1, 1, 4.0)
    
    Assert.Equal(2, matrix.Rows)
    Assert.Equal(2, matrix.Cols)
    Assert.Equal(1.0, matrix.Get(0, 0))
    Assert.Equal(4.0, matrix.Get(1, 1))

[<Fact>]
let ``C++ Matrix multiplication works correctly`` () =
    skipIfCppLibraryUnavailable()
    use matrix1 = new CppMatrix(2, 3)
    matrix1.Set(0, 0, 1.0)
    matrix1.Set(0, 1, 2.0)
    matrix1.Set(0, 2, 3.0)
    matrix1.Set(1, 0, 4.0)
    matrix1.Set(1, 1, 5.0)
    matrix1.Set(1, 2, 6.0)
    
    use matrix2 = new CppMatrix(3, 2)
    matrix2.Set(0, 0, 1.0)
    matrix2.Set(0, 1, 2.0)
    matrix2.Set(1, 0, 3.0)
    matrix2.Set(1, 1, 4.0)
    matrix2.Set(2, 0, 5.0)
    matrix2.Set(2, 1, 6.0)
    
    match matrix1.Multiply(matrix2) with
    | Some result ->
        use resultMatrix = result
        Assert.Equal(2, resultMatrix.Rows)
        Assert.Equal(2, resultMatrix.Cols)
        // Expected result: [[22, 28], [49, 64]]
        Assert.Equal(22.0, resultMatrix.Get(0, 0))
        Assert.Equal(28.0, resultMatrix.Get(0, 1))
        Assert.Equal(49.0, resultMatrix.Get(1, 0))
        Assert.Equal(64.0, resultMatrix.Get(1, 1))
    | None ->
        Assert.True(false, "Matrix multiplication should not fail")

[<Fact>]
let ``C++ Matrix multiplication with square matrices`` () =
    skipIfCppLibraryUnavailable()
    use matrix1 = new CppMatrix(2, 2)
    matrix1.Set(0, 0, 2.0)
    matrix1.Set(0, 1, 3.0)
    matrix1.Set(1, 0, 1.0)
    matrix1.Set(1, 1, 4.0)
    
    use matrix2 = new CppMatrix(2, 2)
    matrix2.Set(0, 0, 1.0)
    matrix2.Set(0, 1, 0.0)
    matrix2.Set(1, 0, 0.0)
    matrix2.Set(1, 1, 1.0)
    
    match matrix1.Multiply(matrix2) with
    | Some result ->
        use resultMatrix = result
        Assert.Equal(2, resultMatrix.Rows)
        Assert.Equal(2, resultMatrix.Cols)
        // Expected result: [[2, 3], [1, 4]] (identity multiplication)
        Assert.Equal(2.0, resultMatrix.Get(0, 0))
        Assert.Equal(3.0, resultMatrix.Get(0, 1))
        Assert.Equal(1.0, resultMatrix.Get(1, 0))
        Assert.Equal(4.0, resultMatrix.Get(1, 1))
    | None ->
        Assert.True(false, "Matrix multiplication should not fail")

[<Fact>]
let ``C++ Matrix multiplication with incompatible dimensions`` () =
    skipIfCppLibraryUnavailable()
    use matrix1 = new CppMatrix(2, 3)
    use matrix2 = new CppMatrix(2, 2) // Incompatible: 2x3 * 2x2 (should be 2x3 * 3x2)
    
    match matrix1.Multiply(matrix2) with
    | Some _ ->
        Assert.True(false, "Matrix multiplication should fail with incompatible dimensions")
    | None ->
        Assert.True(true) // Expected behavior

[<Fact>]
let ``C++ Matrix transpose works correctly`` () =
    skipIfCppLibraryUnavailable()
    use matrix = new CppMatrix(2, 3)
    matrix.Set(0, 0, 1.0)
    matrix.Set(0, 1, 2.0)
    matrix.Set(0, 2, 3.0)
    matrix.Set(1, 0, 4.0)
    matrix.Set(1, 1, 5.0)
    matrix.Set(1, 2, 6.0)
    
    match matrix.Transpose() with
    | Some transposed ->
        use transposeMatrix = transposed
        Assert.Equal(3, transposeMatrix.Rows)
        Assert.Equal(2, transposeMatrix.Cols)
        
        // Expected result: [[1, 4], [2, 5], [3, 6]]
        Assert.Equal(1.0, transposeMatrix.Get(0, 0))
        Assert.Equal(4.0, transposeMatrix.Get(0, 1))
        Assert.Equal(2.0, transposeMatrix.Get(1, 0))
        Assert.Equal(5.0, transposeMatrix.Get(1, 1))
        Assert.Equal(3.0, transposeMatrix.Get(2, 0))
        Assert.Equal(6.0, transposeMatrix.Get(2, 1))
    | None ->
        Assert.True(false, "Matrix transpose should not fail")

[<Fact>]
let ``C++ Matrix square transpose works correctly`` () =
    skipIfCppLibraryUnavailable()
    use matrix = new CppMatrix(2, 2)
    matrix.Set(0, 0, 1.0)
    matrix.Set(0, 1, 2.0)
    matrix.Set(1, 0, 3.0)
    matrix.Set(1, 1, 4.0)
    
    match matrix.Transpose() with
    | Some transposed ->
        use transposeMatrix = transposed
        Assert.Equal(2, transposeMatrix.Rows)
        Assert.Equal(2, transposeMatrix.Cols)
        
        // Expected result: [[1, 3], [2, 4]]
        Assert.Equal(1.0, transposeMatrix.Get(0, 0))
        Assert.Equal(3.0, transposeMatrix.Get(0, 1))
        Assert.Equal(2.0, transposeMatrix.Get(1, 0))
        Assert.Equal(4.0, transposeMatrix.Get(1, 1))
    | None ->
        Assert.True(false, "Matrix transpose should not fail")

[<Fact>]
let ``C++ Vector safe access works correctly`` () =
    skipIfCppLibraryUnavailable()
    use vector = new CppVector()
    vector.Add(10)
    vector.Add(20)
    vector.Add(30)
    
    // Test valid access
    match vector.SafeGet(1) with
    | Some value -> Assert.Equal(20, value)
    | None -> Assert.True(false, "Safe access should return value for valid index")
    
    // Test invalid access
    match vector.SafeGet(10) with
    | Some _ -> Assert.True(false, "Safe access should return None for invalid index")
    | None -> Assert.True(true) // Expected behavior

[<Fact>]
let ``C++ Vector sorting works correctly`` () =
    skipIfCppLibraryUnavailable()
    use vector = new CppVector()
    vector.Add(5)
    vector.Add(2)
    vector.Add(8)
    vector.Add(1)
    
    vector.Sort()
    
    Assert.Equal(1, vector.Get(0))
    Assert.Equal(2, vector.Get(1))
    Assert.Equal(5, vector.Get(2))
    Assert.Equal(8, vector.Get(3))

[<Fact>]
let ``C++ String manipulation operations work correctly`` () =
    skipIfCppLibraryUnavailable()
    use str = new CppString("Hello")
    
    str.Prepend("Hi ")
    Assert.Equal("Hi Hello", str.Value)
    
    str.Reverse()
    Assert.Equal("olleH iH", str.Value)
    
    str.ToLower()
    Assert.Contains("h", str.Value.ToLower())

[<Fact>]
let ``C++ Mathematical operations with edge cases`` () =
    skipIfCppLibraryUnavailable()
    // Test with single value
    let singleValue = [| 5.0 |]
    let (mean, variance, stdDev) = calculateStatistics(singleValue)
    
    Assert.Equal(5.0, mean, 1)
    Assert.Equal(0.0, variance, 1) // Variance of single value is 0
    Assert.Equal(0.0, stdDev, 1)   // Standard deviation of single value is 0

[<Fact>]
let ``C++ Function power operation works correctly`` () =
    skipIfCppLibraryUnavailable()
    use powerFunc = CppFunction.CreatePower()
    
    Assert.Equal(8.0, powerFunc.Call(2.0, 3.0))  // 2^3 = 8
    Assert.Equal(1.0, powerFunc.Call(5.0, 0.0))  // 5^0 = 1
    Assert.Equal(25.0, powerFunc.Call(5.0, 2.0)) // 5^2 = 25

[<Fact>]
let ``C++ Matrix creation with different sizes`` () =
    skipIfCppLibraryUnavailable()
    // Test 1x1 matrix
    use matrix1x1 = new CppMatrix(1, 1)
    matrix1x1.Set(0, 0, 42.0)
    Assert.Equal(1, matrix1x1.Rows)
    Assert.Equal(1, matrix1x1.Cols)
    Assert.Equal(42.0, matrix1x1.Get(0, 0))
    
    // Test larger matrix
    use matrix5x3 = new CppMatrix(5, 3)
    Assert.Equal(5, matrix5x3.Rows)
    Assert.Equal(3, matrix5x3.Cols)
    
    // Test default values are 0
    Assert.Equal(0.0, matrix5x3.Get(2, 1))

[<Fact>]
let ``C++ Matrix identity multiplication`` () =
    skipIfCppLibraryUnavailable()
    use identity = new CppMatrix(3, 3)
    identity.Set(0, 0, 1.0)
    identity.Set(1, 1, 1.0)
    identity.Set(2, 2, 1.0)
    
    use testMatrix = new CppMatrix(3, 3)
    testMatrix.Set(0, 0, 2.0)
    testMatrix.Set(0, 1, 3.0)
    testMatrix.Set(1, 0, 4.0)
    testMatrix.Set(2, 2, 5.0)
    
    match testMatrix.Multiply(identity) with
    | Some result ->
        use resultMatrix = result
        // Multiplying by identity should yield original matrix
        Assert.Equal(2.0, resultMatrix.Get(0, 0))
        Assert.Equal(3.0, resultMatrix.Get(0, 1))
        Assert.Equal(4.0, resultMatrix.Get(1, 0))
        Assert.Equal(5.0, resultMatrix.Get(2, 2))
        Assert.Equal(0.0, resultMatrix.Get(1, 1)) // Should remain 0
    | None ->
        Assert.True(false, "Identity multiplication should not fail")
