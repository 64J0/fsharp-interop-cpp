module Program

open System.Text
open System.Runtime.InteropServices

// DllImport searches for "libMyLibrary.so" on Linux/macOS or "MyLibrary.dll" on Windows
[<DllImport("MyLibrary", CallingConvention = CallingConvention.Cdecl)>]
extern int Add(int a, int b)

[<DllImport("MyLibrary", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)>]
extern void ReverseString([<MarshalAs(UnmanagedType.LPStr)>] string input, 
                          [<MarshalAs(UnmanagedType.LPStr)>] StringBuilder output, 
                          int buffer_size)

[<EntryPoint>]
let main _argv =
    // Call the C++ function from F#
    let sum = Add(10, 15)
    printfn "The sum of 10 and 15 from C++ is: %d" sum

    let originalString = "Hello, world!"
    let bufferSize = originalString.Length + 1
    let outputBuffer = new StringBuilder(bufferSize)

    ReverseString(originalString, outputBuffer, bufferSize)
    printfn "Original: %s" originalString
    printfn "Reversed: %s" (outputBuffer.ToString())

    0
