#!/bin/bash

# Build and run script for F# C++ Interop project
# This script automates the build and test process

set -e  # Exit on any error

echo "F# C++ Interop - Build and Test Script"
echo "======================================"

# Check for required tools
check_tool() {
    if ! command -v $1 &> /dev/null; then
        echo "Error: $1 is not installed or not in PATH"
        exit 1
    fi
}

echo "Checking prerequisites..."
check_tool gcc
check_tool dotnet
check_tool make

# Get script directory
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# Build C++ library
echo
echo "Building C++ library..."
cd "$SCRIPT_DIR"
make clean
make

# Build F# projects
echo
echo "Building F# projects..."
dotnet clean
dotnet build

# Set library path
export LD_LIBRARY_PATH="$SCRIPT_DIR/build:$LD_LIBRARY_PATH"

# Run tests
echo
echo "Running tests..."
cd tests/InteropTests
dotnet test --verbosity minimal

# Run demo
echo
echo "Running demo application..."
cd "$SCRIPT_DIR/src/fsharp/FSharpCppInterop"
dotnet run

echo
echo "Build and test completed successfully!"
echo "Library path set to: $LD_LIBRARY_PATH"
echo
echo "Libraries built:"
echo "  - libmath_operations.so (C library)"
echo "  - libcpp_operations.so (C++ library)"
echo
echo "To run the demo again:"
echo "  export LD_LIBRARY_PATH=$SCRIPT_DIR/build:\$LD_LIBRARY_PATH"
echo "  cd $SCRIPT_DIR/src/fsharp/FSharpCppInterop"
echo "  dotnet run"