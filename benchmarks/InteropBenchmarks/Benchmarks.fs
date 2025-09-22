module InteropBenchmarks

open System
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Running
open MathOperationsInterop
open CppOperationsInterop

// Benchmark configuration
[<SimpleJob(RuntimeMoniker.Net80)>]
[<MemoryDiagnoser>]
[<Config(typeof<BenchmarkConfig>)>]
type InteropPerformanceBenchmarks() =
    
    let mutable cppVector: CppVector option = None
    let mutable cppString: CppString option = None
    let mutable testData: double[] = [||]
    let mutable intArray: int[] = [||]
    
    [<GlobalSetup>]
    member this.Setup() =
        try
            cppVector <- Some(new CppVector())
            cppString <- Some(new CppString(""))
            testData <- Array.init 1000 (fun i -> float i)
            intArray <- Array.init 100 (fun i -> i)
            
            // Pre-populate C++ vector
            match cppVector with
            | Some v -> for i in 1..100 do v.Add(i)
            | None -> ()
        with
        | _ -> () // Skip setup if libraries not available
    
    [<GlobalCleanup>]
    member this.Cleanup() =
        match cppVector with
        | Some v -> (v :> IDisposable).Dispose()
        | None -> ()
        match cppString with  
        | Some s -> (s :> IDisposable).Dispose()
        | None -> ()

    // Basic arithmetic benchmarks - C vs C++
    [<Benchmark(Description = "C: Integer Addition (1000 calls)")>]
    member this.CIntegerAddition() =
        let mutable sum = 0
        for i in 1..1000 do
            sum <- sum + add_integers(i, i + 1)
        sum

    [<Benchmark(Description = "C: Float Multiplication (1000 calls)")>]  
    member this.CFloatMultiplication() =
        let mutable result = 0.0f
        for i in 1..1000 do
            result <- result + multiply_floats(float32 i, 2.0f)
        result

    [<Benchmark(Description = "C++: Mathematical Mean (1000 element array)")>]
    member this.CppMathematicalMean() =
        try
            calculate_mean_double(testData, testData.Length)
        with
        | _ -> 0.0

    [<Benchmark(Description = "C++: Statistical Operations (variance + std dev)")>]
    member this.CppStatisticalOperations() =
        try
            let variance = calculate_variance(testData, testData.Length)
            let stdDev = calculate_standard_deviation(testData, testData.Length)
            variance + stdDev
        with
        | _ -> 0.0

    // Array operations benchmarks
    [<Benchmark(Description = "C: Array Sum (100 elements)")>]
    member this.CArraySum() =
        sum_array(intArray, intArray.Length)

    [<Benchmark(Description = "C: Array Sort (100 elements)")>]
    member this.CArraySort() =
        let arrayCopy = Array.copy intArray
        sort_array(arrayCopy, arrayCopy.Length)
        arrayCopy.[0] // Return first element to ensure work is done

    [<Benchmark(Description = "C++: Vector Operations (Add + Sum)")>]
    member this.CppVectorOperations() =
        try
            match cppVector with
            | Some v ->
                v.Clear()
                for i in 1..100 do v.Add(i)
                v.Sum()
            | None -> 0
        with
        | _ -> 0

    [<Benchmark(Description = "C++: Vector Sort Operations")>]
    member this.CppVectorSort() =
        try
            match cppVector with
            | Some v ->
                v.Clear()
                // Add elements in reverse order
                for i in [100..-1..1] do v.Add(i)
                v.Sort()
                v.Get(0) // Return first element
            | None -> 0
        with
        | _ -> 0

    // String operations benchmarks
    [<Benchmark(Description = "C: String Operations (1000 calls)")>]
    member this.CStringOperations() =
        let mutable totalLength = 0
        for i in 1..1000 do
            let testStr = sprintf "Test%d" i
            totalLength <- totalLength + string_length(testStr)
        totalLength

    [<Benchmark(Description = "C++: String Operations (Append + Length)")>]
    member this.CppStringOperations() =
        try
            match cppString with
            | Some s ->
                let mutable totalLength = 0
                for i in 1..100 do
                    s.Append(sprintf "%d" i)
                    totalLength <- totalLength + s.Length
                totalLength
            | None -> 0
        with
        | _ -> 0

    // Function call overhead benchmarks
    [<Benchmark(Description = "C++: Function Object Calls (1000 operations)")>]
    member this.CppFunctionObjectCalls() =
        try
            use addFunc = CppFunction.CreateAdd()
            use multFunc = CppFunction.CreateMultiply()
            let mutable result = 0.0
            for i in 1..500 do
                result <- result + addFunc.Call(float i, 1.0)
                result <- result + multFunc.Call(float i, 2.0)
            result
        with
        | _ -> 0.0

    // Memory management benchmarks  
    [<Benchmark(Description = "C: Memory Allocation/Deallocation (100 cycles)")>]
    member this.CMemoryManagement() =
        let mutable totalAllocated = 0
        for i in 1..100 do
            withAllocatedString 256 (fun ptr ->
                totalAllocated <- totalAllocated + 256
            )
        totalAllocated

    [<Benchmark(Description = "C++: Smart Resource Management (100 cycles)")>]
    member this.CppSmartResourceManagement() =
        try
            let mutable totalSize = 0
            for i in 1..100 do
                use resource = new CppMatrix(2, 2)
                resource.Set(0, 0, float i)
                resource.Set(1, 1, float i)
                totalSize <- totalSize + (resource.Rows * resource.Cols)
            totalSize
        with
        | _ -> 0

// Custom benchmark configuration
and BenchmarkConfig() =
    inherit ManualConfig()
    do
        base.AddLogger(BenchmarkDotNet.Loggers.ConsoleLogger.Default) |> ignore
        base.AddColumnProvider(BenchmarkDotNet.Columns.DefaultColumnProviders.Instance) |> ignore
        base.AddExporter(BenchmarkDotNet.Exporters.HtmlExporter.Default) |> ignore

// Micro-benchmarks for detailed performance analysis
[<SimpleJob(RuntimeMoniker.Net80)>]
[<MemoryDiagnoser>]
type MicroBenchmarks() =
    let mutable testArray: int[] = [||]
    let mutable testDoubleArray: double[] = [||]
    
    [<Params(10, 100, 1000, 10000)>]
    member val public ArraySize = 0 with get, set
    
    [<GlobalSetup>]
    member this.Setup() =
        testArray <- Array.init this.ArraySize id
        testDoubleArray <- Array.init this.ArraySize float

    [<Benchmark>]
    member this.CArraySumScaled() =
        sum_array(testArray, testArray.Length)

    [<Benchmark>]
    member this.CppMathMeanScaled() =
        try
            calculate_mean_double(testDoubleArray, testDoubleArray.Length)
        with
        | _ -> 0.0

    [<Benchmark>]
    member this.CppVarianceScaled() =
        try
            calculate_variance(testDoubleArray, testDoubleArray.Length)
        with
        | _ -> 0.0

// Library availability check
type LibraryChecker() =
    static member CheckAvailability() =
        let cAvailable = 
            try
                add_integers(1, 1) |> ignore
                true
            with _ -> false
        
        let cppAvailable =
            try
                use v = new CppVector()
                v.Add(1)
                true  
            with _ -> false
        
        (cAvailable, cppAvailable)