# Compilers and flags
CC = gcc
CXX = g++
CFLAGS = -Wall -Wextra -fPIC -shared -O2
CXXFLAGS = -Wall -Wextra -fPIC -shared -O2 -std=c++14
C_TARGET = libmath_operations.so
CPP_TARGET = libcpp_operations.so
SRCDIR_C = src/c
SRCDIR_CPP = src/cpp
BUILDDIR = build

# Source files
C_SOURCES = math_operations.c
C_HEADERS = math_operations.h
C_SOURCES_PATH = $(SRCDIR_C)/$(C_SOURCES)
C_HEADERS_PATH = $(SRCDIR_C)/$(C_HEADERS)

CPP_SOURCES = cpp_operations.cpp
CPP_HEADERS = cpp_operations.hpp
CPP_SOURCES_PATH = $(SRCDIR_CPP)/$(CPP_SOURCES)
CPP_HEADERS_PATH = $(SRCDIR_CPP)/$(CPP_HEADERS)

# Default target - build both libraries
all: $(BUILDDIR)/$(C_TARGET) $(BUILDDIR)/$(CPP_TARGET)

# Create build directory
$(BUILDDIR):
	mkdir -p $(BUILDDIR)

# Build C library
$(BUILDDIR)/$(C_TARGET): $(C_SOURCES_PATH) $(C_HEADERS_PATH) | $(BUILDDIR)
	$(CC) $(CFLAGS) -o $@ $(C_SOURCES_PATH)

# Build C++ library
$(BUILDDIR)/$(CPP_TARGET): $(CPP_SOURCES_PATH) $(CPP_HEADERS_PATH) | $(BUILDDIR)
	$(CXX) $(CXXFLAGS) -o $@ $(CPP_SOURCES_PATH)

# Clean build artifacts
clean:
	rm -rf $(BUILDDIR)

# Install library to system (optional)
install: $(BUILDDIR)/$(C_TARGET) $(BUILDDIR)/$(CPP_TARGET)
	sudo cp $(BUILDDIR)/$(C_TARGET) /usr/local/lib/
	sudo cp $(BUILDDIR)/$(CPP_TARGET) /usr/local/lib/
	sudo cp $(C_HEADERS_PATH) /usr/local/include/
	sudo cp $(CPP_HEADERS_PATH) /usr/local/include/
	sudo ldconfig

# Uninstall library from system
uninstall:
	sudo rm -f /usr/local/lib/$(C_TARGET)
	sudo rm -f /usr/local/lib/$(CPP_TARGET)
	sudo rm -f /usr/local/include/$(C_HEADERS)
	sudo rm -f /usr/local/include/$(CPP_HEADERS)

# Run tests (if available)
test: $(BUILDDIR)/$(C_TARGET) $(BUILDDIR)/$(CPP_TARGET)
	cd tests && dotnet test

.PHONY: all clean install uninstall test