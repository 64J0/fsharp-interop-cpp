# Compiler and flags
CC = gcc
CFLAGS = -Wall -Wextra -fPIC -shared -O2
TARGET = libmath_operations.so
SRCDIR = src/cpp
BUILDDIR = build

# Source files
SOURCES = $(SRCDIR)/math_operations.c
HEADERS = $(SRCDIR)/math_operations.h

# Default target
all: $(BUILDDIR)/$(TARGET)

# Create build directory
$(BUILDDIR):
	mkdir -p $(BUILDDIR)

# Build shared library
$(BUILDDIR)/$(TARGET): $(SOURCES) $(HEADERS) | $(BUILDDIR)
	$(CC) $(CFLAGS) -o $@ $(SOURCES)

# Clean build artifacts
clean:
	rm -rf $(BUILDDIR)

# Install library to system (optional)
install: $(BUILDDIR)/$(TARGET)
	sudo cp $(BUILDDIR)/$(TARGET) /usr/local/lib/
	sudo cp $(SRCDIR)/math_operations.h /usr/local/include/
	sudo ldconfig

# Uninstall library from system
uninstall:
	sudo rm -f /usr/local/lib/$(TARGET)
	sudo rm -f /usr/local/include/math_operations.h
	sudo ldconfig

# Run tests (if available)
test: $(BUILDDIR)/$(TARGET)
	cd tests && dotnet test

.PHONY: all clean install uninstall test