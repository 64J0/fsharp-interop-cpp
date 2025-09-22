open System
open BenchmarkDotNet.Running
open BenchmarkDotNet.Configs
open InteropBenchmarks
open MathOperationsInterop
open CppOperationsInterop

let printHeader() =
    printfn "F# vs C vs C++ Performance Comparison Benchmarks"
    printfn "=============================================="
    printfn ""

let printLibraryStatus() =
    let (cAvailable, cppAvailable) = LibraryChecker.CheckAvailability()
    
    printfn "Library Availability Check:"
    printfn "  F# Implementation: ✅ Always Available (Managed Code)"
    printfn "  C Library (libmath_operations.so): %s" (if cAvailable then "✅ Available" else "❌ Not Available")
    printfn "  C++ Library (libcpp_operations.so): %s" (if cppAvailable then "✅ Available" else "❌ Not Available")
    printfn ""
    
    if not cAvailable && not cppAvailable then
        printfn "❌ Neither native library is available. Only F# benchmarks will run."
        printfn "   To enable C/C++ comparisons:"
        printfn "   1. Run 'make' in the project root"
        printfn "   2. Set LD_LIBRARY_PATH: export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH"
        printfn "   3. Run the benchmarks again"
        true // F# benchmarks can still run
    elif not cAvailable then
        printfn "⚠️  C library not available. F# vs C++ comparisons will run."
        true
    elif not cppAvailable then
        printfn "⚠️  C++ library not available. F# vs C comparisons will run."
        true
    else
        printfn "✅ All libraries available. Running full F# vs C vs C++ benchmark suite."
        true

let printBenchmarkOptions() =
    printfn "Benchmark Options:"
    printfn "  1. Full Performance Benchmarks (F# vs C vs C++ operations)"
    printfn "  2. Micro Benchmarks (scaled array operations with size analysis)"
    printfn "  3. Quick Performance Test (subset of benchmarks)"
    printfn "  4. Exit"
    printfn ""

let runQuickTest() =
    printfn "Running Quick Performance Test (F# vs C vs C++)..."
    printfn "=================================================="
    printfn ""
    
    try
        // F# baseline test
        let startTime1 = DateTime.Now
        let mutable fsharpSum = 0
        for i in 1..1000 do
            fsharpSum <- fsharpSum + (i + (i + 1))
        let fsharpTime = DateTime.Now - startTime1
        printfn "F# Integer Addition (1000 calls): %d ms" fsharpTime.Milliseconds
        
        // Quick C tests
        let startTime2 = DateTime.Now
        let mutable cSum = 0
        for i in 1..1000 do
            cSum <- cSum + add_integers(i, i + 1)
        let cTime = DateTime.Now - startTime2
        printfn "C Integer Addition (1000 calls): %d ms" cTime.Milliseconds
        
        // F# array operation
        let testArray = [| 1..100 |]
        let startTime3 = DateTime.Now
        let fsharpArraySum = Array.sum testArray
        let fsharpArrayTime = DateTime.Now - startTime3
        printfn "F# Array Sum (100 elements): %d ms" fsharpArrayTime.Milliseconds
        
        // Quick C++ tests
        let startTime4 = DateTime.Now
        use vector = new CppVector()
        for i in 1..100 do vector.Add(i)
        let vectorSum = vector.Sum()
        let cppTime = DateTime.Now - startTime4
        printfn "C++ Vector Operations (100 elements): %d ms" cppTime.Milliseconds
        
        printfn ""
        printfn "Performance Comparison Summary:"
        printfn "  F# (managed): Baseline for comparison"
        printfn "  C (P/Invoke): Includes marshalling overhead but benefits from optimized C code"
        printfn "  C++ (P/Invoke): Object management overhead but with modern C++ features"
        printfn ""
        printfn "For detailed benchmarks with statistical analysis, run the full benchmarks."
        
    with
    | ex -> printfn "Error running quick test: %s" ex.Message

[<EntryPoint>]
let main argv =
    printHeader()
    
    if not (printLibraryStatus()) then
        1
    else
        let mutable keepRunning = true
        let mutable result = 0
        
        while keepRunning do
            printBenchmarkOptions()
            printf "Select option (1-4): "
            let input = Console.ReadLine()
            
            match input with
            | "1" ->
                printfn ""
                printfn "Running Full Performance Benchmarks (F# vs C vs C++)..."
                printfn "This may take several minutes. Results will be saved to BenchmarkDotNet.Artifacts/"
                printfn "Comparing managed F# code with native C/C++ libraries via P/Invoke."
                printfn ""
                try
                    BenchmarkRunner.Run<InteropPerformanceBenchmarks>() |> ignore
                    printfn ""
                    printfn "✅ Full benchmarks completed!"
                with
                | ex -> 
                    printfn "❌ Benchmark failed: %s" ex.Message
                    result <- 1
                
            | "2" ->
                printfn ""
                printfn "Running Micro Benchmarks (F# vs C vs C++ Array Size Scaling)..."
                printfn "Analyzing performance characteristics with different data sizes."
                printfn ""
                try
                    BenchmarkRunner.Run<MicroBenchmarks>() |> ignore
                    printfn ""
                    printfn "✅ Micro benchmarks completed!"
                with
                | ex -> 
                    printfn "❌ Benchmark failed: %s" ex.Message
                    result <- 1
                
            | "3" ->
                printfn ""
                runQuickTest()
                printfn ""
                
            | "4" ->
                keepRunning <- false
                printfn "Goodbye!"
                
            | _ ->
                printfn "Invalid option. Please select 1-4."
                printfn ""
        
        result
