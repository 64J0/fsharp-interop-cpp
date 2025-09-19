# Define variables
CPP_SOURCE = cpp/MyLibrary.cpp
CPP_HEADER = cpp/MyLibrary.h
CPP_OUTPUT = libMyLibrary.so
FSHARP_PROJECT_DIR = fsharp-interop
FSHARP_OUTPUT_DIR = $(FSHARP_PROJECT_DIR)/bin/Debug/net8.0/

# The default target that builds everything
all: build_cpp build_fsharp run

# Build the C++ shared library
build_cpp:
	@echo "Building C++ shared library..."
	g++ -shared -fPIC -L$(CPP_HEADER) $(CPP_SOURCE) -o $(CPP_OUTPUT)
	@echo "C++ build complete."

# Build the F# project
build_fsharp:
	@echo "Building F# project..."
	dotnet build $(FSHARP_PROJECT_DIR)

# Run the final executable and copy the C++ library to the output directory
run:
	@echo "Copying shared library to F# output directory..."
	cp $(CPP_OUTPUT) $(FSHARP_OUTPUT_DIR)
	@echo "Running F# application..."
	dotnet run --project $(FSHARP_PROJECT_DIR)

# Clean up build files
clean:
	@echo "Cleaning up..."
	rm -f $(CPP_OUTPUT)
	rm -rf $(FSHARP_PROJECT_DIR)/bin $(FSHARP_PROJECT_DIR)/obj
	@echo "Clean complete."

.PHONY: all build_cpp build_fsharp run clean
