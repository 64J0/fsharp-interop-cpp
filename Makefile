# Compilers and flags
CC = gcc
CXX = g++
CFLAGS = -Wall -Wextra -fPIC -shared -O2
CXXFLAGS = -Wall -Wextra -fPIC -shared -O2 -std=c++14
C_TARGET = libmath_operations.so
CPP_TARGET = libcpp_operations.so
SRCDIR = src/cpp
BUILDDIR = build

# Source files
C_SOURCES = $(SRCDIR)/math_operations.c
CPP_SOURCES = $(SRCDIR)/cpp_operations.cpp
C_HEADERS = $(SRCDIR)/math_operations.h
CPP_HEADERS = $(SRCDIR)/cpp_operations.h

# Default target - build both libraries
all: $(BUILDDIR)/$(C_TARGET) $(BUILDDIR)/$(CPP_TARGET)

# Create build directory
$(BUILDDIR):
	mkdir -p $(BUILDDIR)

# Build C library
$(BUILDDIR)/$(C_TARGET): $(C_SOURCES) $(C_HEADERS) | $(BUILDDIR)
	$(CC) $(CFLAGS) -o $@ $(C_SOURCES)

# Build C++ library
$(BUILDDIR)/$(CPP_TARGET): $(CPP_SOURCES) $(CPP_HEADERS) | $(BUILDDIR)
	$(CXX) $(CXXFLAGS) -o $@ $(CPP_SOURCES)

# Clean build artifacts
clean:
	rm -rf $(BUILDDIR)

# Install library to system (optional)
install: $(BUILDDIR)/$(C_TARGET) $(BUILDDIR)/$(CPP_TARGET)
	sudo cp $(BUILDDIR)/$(C_TARGET) /usr/local/lib/
	sudo cp $(BUILDDIR)/$(CPP_TARGET) /usr/local/lib/
	sudo cp $(SRCDIR)/math_operations.h /usr/local/include/
	sudo cp $(SRCDIR)/cpp_operations.h /usr/local/include/
	sudo ldconfig

# Uninstall library from system
uninstall:
	sudo rm -f /usr/local/lib/$(C_TARGET)
	sudo rm -f /usr/local/lib/$(CPP_TARGET)
	sudo rm -f /usr/local/include/math_operations.h
	sudo rm -f /usr/local/include/cpp_operations.h
	sudo ldconfig

# Run tests (if available)
test: $(BUILDDIR)/$(C_TARGET) $(BUILDDIR)/$(CPP_TARGET)
	cd tests && dotnet test

.PHONY: all clean install uninstall test