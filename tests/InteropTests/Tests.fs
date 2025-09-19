module Tests

open System
open System.Text
open Xunit
open MathOperationsInterop

// Helper function to check if library is available
let skipIfLibraryUnavailable() =
    try
        add_integers(1, 1) |> ignore
    with
    | _ -> () // Skip test if library is not available - we'll handle this in the test runner

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
