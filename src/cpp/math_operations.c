#include "math_operations.h"
#include <string.h>
#include <stdlib.h>
#include <math.h>
#include <stdio.h>
#include <unistd.h>

// Global buffer for string operations (simple approach for demo)
static char greeting_buffer[256];

// Basic primitive type operations
int add_integers(int a, int b) {
    return a + b;
}

float multiply_floats(float a, float b) {
    return a * b;
}

double divide_doubles(double a, double b) {
    return a / b;
}

int is_even(int number) {
    return (number % 2) == 0 ? 1 : 0;
}

// String operations
int string_length(const char* str) {
    if (str == NULL) {
        return -1;
    }
    return (int)strlen(str);
}

void copy_string(const char* source, char* destination, int max_length) {
    if (source == NULL || destination == NULL || max_length <= 0) {
        return;
    }
    
    strncpy(destination, source, max_length - 1);
    destination[max_length - 1] = '\0';
}

const char* get_greeting(const char* name) {
    if (name == NULL) {
        strcpy(greeting_buffer, "Hello, stranger!");
    } else {
        snprintf(greeting_buffer, sizeof(greeting_buffer), "Hello, %s!", name);
    }
    return greeting_buffer;
}

// Struct operations
Point create_point(int x, int y) {
    Point p;
    p.x = x;
    p.y = y;
    return p;
}

float calculate_distance(Point p1, Point p2) {
    int dx = p2.x - p1.x;
    int dy = p2.y - p1.y;
    return sqrt(dx * dx + dy * dy);
}

Rectangle create_rectangle(float width, float height) {
    Rectangle r;
    r.width = width;
    r.height = height;
    return r;
}

float rectangle_area(Rectangle rect) {
    return rect.width * rect.height;
}

// Array operations
void fill_array(int* array, int size, int value) {
    if (array == NULL || size <= 0) {
        return;
    }
    
    for (int i = 0; i < size; i++) {
        array[i] = value;
    }
}

int sum_array(const int* array, int size) {
    if (array == NULL || size <= 0) {
        return 0;
    }
    
    int sum = 0;
    for (int i = 0; i < size; i++) {
        sum += array[i];
    }
    return sum;
}

// Simple bubble sort for demo
void sort_array(int* array, int size) {
    if (array == NULL || size <= 1) {
        return;
    }
    
    for (int i = 0; i < size - 1; i++) {
        for (int j = 0; j < size - i - 1; j++) {
            if (array[j] > array[j + 1]) {
                int temp = array[j];
                array[j] = array[j + 1];
                array[j + 1] = temp;
            }
        }
    }
}

// Callback function
void simulate_work(int duration_ms, ProgressCallback callback) {
    if (callback == NULL) {
        return;
    }
    
    const int steps = 10;
    const int step_duration = duration_ms / steps;
    
    for (int i = 0; i <= steps; i++) {
        callback((i * 100) / steps);
        usleep(step_duration * 1000); // Convert ms to microseconds
    }
}

// Error handling
ResultCode safe_divide(double a, double b, double* result) {
    if (result == NULL) {
        return RESULT_NULL_POINTER;
    }
    
    if (b == 0.0) {
        return RESULT_INVALID_PARAMETER;
    }
    
    *result = a / b;
    return RESULT_SUCCESS;
}

ResultCode validate_array(const int* array, int size) {
    if (array == NULL) {
        return RESULT_NULL_POINTER;
    }
    
    if (size <= 0) {
        return RESULT_INVALID_PARAMETER;
    }
    
    return RESULT_SUCCESS;
}

// Memory management
char* allocate_string(int length) {
    if (length <= 0) {
        return NULL;
    }
    
    char* str = (char*)malloc(length);
    if (str != NULL) {
        memset(str, 0, length);
    }
    return str;
}

void free_string(char* str) {
    if (str != NULL) {
        free(str);
    }
}