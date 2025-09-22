open System
open BenchmarkDotNet.Running
open BenchmarkDotNet.Configs
open InteropBenchmarks
open MathOperationsInterop
open CppOperationsInterop

let printHeader() =
    printfn "F# C/C++ Interop Performance Benchmarks"
    printfn "======================================="
    printfn ""

let printLibraryStatus() =
    let (cAvailable, cppAvailable) = LibraryChecker.CheckAvailability()
    
    printfn "Library Availability Check:"
    printfn "  C Library (libmath_operations.so): %s" (if cAvailable then "✅ Available" else "❌ Not Available")
    printfn "  C++ Library (libcpp_operations.so): %s" (if cppAvailable then "✅ Available" else "❌ Not Available")
    printfn ""
    
    if not cAvailable && not cppAvailable then
        printfn "❌ Neither library is available. Please build the libraries first:"
        printfn "   1. Run 'make' in the project root"
        printfn "   2. Set LD_LIBRARY_PATH: export LD_LIBRARY_PATH=$PWD/build:$LD_LIBRARY_PATH"
        printfn "   3. Run the benchmarks again"
        false
    elif not cAvailable then
        printfn "⚠️  C library not available. Only C++ benchmarks will run."
        true
    elif not cppAvailable then
        printfn "⚠️  C++ library not available. Only C library benchmarks will run."
        true
    else
        printfn "✅ All libraries available. Running full benchmark suite."
        true

let printBenchmarkOptions() =
    printfn "Benchmark Options:"
    printfn "  1. Full Performance Benchmarks (all operations)"
    printfn "  2. Micro Benchmarks (scaled array operations)"
    printfn "  3. Quick Performance Test (subset of benchmarks)"
    printfn "  4. Exit"
    printfn ""

let runQuickTest() =
    printfn "Running Quick Performance Test..."
    printfn "================================="
    printfn ""
    
    try
        // Quick C tests
        let startTime = DateTime.Now
        let mutable cSum = 0
        for i in 1..1000 do
            cSum <- cSum + add_integers(i, i + 1)
        let cTime = DateTime.Now - startTime
        printfn "C Integer Addition (1000 calls): %d ms" cTime.Milliseconds
        
        // Quick C++ tests
        let startTime2 = DateTime.Now
        use vector = new CppVector()
        for i in 1..100 do vector.Add(i)
        let vectorSum = vector.Sum()
        let cppTime = DateTime.Now - startTime2
        printfn "C++ Vector Operations (100 elements): %d ms" cppTime.Milliseconds
        
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
                printfn "Running Full Performance Benchmarks..."
                printfn "This may take several minutes. Results will be saved to BenchmarkDotNet.Artifacts/"
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
                printfn "Running Micro Benchmarks (Array Size Scaling)..."
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
