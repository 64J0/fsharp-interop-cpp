# Copilot Instructions

This repository uses **F#** as the primary programming language. When generating code or providing suggestions, ensure that the code adheres to F# conventions and idiomatic practices.

Also, keep in mind that we're developing this project to work in a Linux environment. Please ensure that any code snippets, commands, or configurations are compatible with Linux systems primarily.

## Interoperability with C++ using .NET P/Invoke

When working with C++ interop in this repository, follow these guidelines:

1. **Use .NET P/Invoke Best Practices**:
    - Ensure proper marshaling of data types between managed and unmanaged code.
    - DO use `[<LibraryImport>]`, if possible, when targeting .NET 7+.
        - There are cases when using `[DllImport]` is appropriate. A code analyzer with ID `SYSLIB1054` tells you when that's the case.
    - DO use the same naming and capitalization for your methods and parameters as the native method you want to call.
    - CONSIDER using the same naming and capitalization for constant values.
    - DO define P/Invoke and function pointer signatures that match the C function's arguments.
    - DO use .NET types that map closest to the native type. For example, in C#, use `uint` when the native type is `unsigned int`.
    - DO prefer expressing higher level native types using .NET structs rather than classes.
    - DO prefer using function pointers, as opposed to `Delegate` types, when passing callbacks to unmanaged functions in C#.
    - DO use `[<In>]` and `[<Out>]` attributes on array parameters.
    - DO only use `[<In>]` and `[<Out>]` attributes on other types when the behavior you want differs from the default behavior.
    - CONSIDER using `System.Buffers.ArrayPool` to pool your native array buffers.
    - CONSIDER wrapping your P/Invoke declarations in a class with the same name and capitalization as your native library.
    - This allows your `[<LibraryImport>]` or `[<DllImport>]` attributes to use the C# `nameof` language feature to pass in the name of the native library and ensure that you didn't misspell the name of the native library.
    - DO use `SafeHandle` handles to manage lifetime of objects that encapsulate unmanaged resources.
    - AVOID finalizers to manage lifetime of objects that encapsulate unmanaged resources.
    - Avoid memory leaks by properly freeing unmanaged resources when necessary.

2. **F# Specific Interop Guidance**:
    - Use `NativeInterop` modules or helper functions to encapsulate P/Invoke calls.
    - Prefer type-safe wrappers around raw P/Invoke calls for better maintainability.
    - Document all external function imports with comments explaining their purpose.

3. **Error Handling**:
    - Check for and handle errors returned by unmanaged functions.

4. **Testing**:
    - Write unit tests to validate the behavior of interop code.
    - Use mock libraries or test doubles to simulate unmanaged dependencies when possible.

5. **Code Examples**:
    - Provide clear examples of P/Invoke declarations, struct marshalling, string handling, callback functions, error handling, and memory safety patterns.

6. **Documentation**:
    - Maintain up-to-date documentation on how to use the interop features, including any setup required for native libraries.

7. **Review and Refactor**:
    - Regularly review interop code for potential improvements in safety, performance, and readability.
    - Refactor code to follow the latest best practices in .NET and F# interop.

8. **Benchmarks**:
    - Include performance benchmarks for critical interop paths to ensure efficiency.
    - Optimize marshaling strategies based on benchmark results.

By following these instructions, we aim to maintain clean, safe, and efficient interop code in this repository.