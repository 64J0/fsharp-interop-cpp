#ifndef CPP_OPERATIONS_H
#define CPP_OPERATIONS_H

#include <vector>
#include <string>
#include <memory>
#include <functional>

#ifdef __cplusplus
extern "C" {
#endif

// C++ class-based operations (exposed as C functions for P/Invoke)

// Vector operations using C++ std::vector
typedef void* VectorHandle;

VectorHandle vector_create();
void vector_destroy(VectorHandle handle);
void vector_add(VectorHandle handle, int value);
int vector_get(VectorHandle handle, int index);
int vector_size(VectorHandle handle);
void vector_clear(VectorHandle handle);
int vector_sum(VectorHandle handle);
void vector_sort(VectorHandle handle);

// String operations using C++ std::string
typedef void* StringHandle;

StringHandle string_create(const char* initial_value);
void string_destroy(StringHandle handle);
const char* string_get_cstr(StringHandle handle);
void string_append(StringHandle handle, const char* text);
void string_prepend(StringHandle handle, const char* text);
int string_length_cpp(StringHandle handle);
void string_reverse(StringHandle handle);
void string_to_upper(StringHandle handle);
void string_to_lower(StringHandle handle);

// Advanced mathematical operations using templates (instantiated for specific types)
double calculate_mean_double(const double* values, int count);
float calculate_mean_float(const float* values, int count);
double calculate_variance(const double* values, int count);
double calculate_standard_deviation(const double* values, int count);

// Matrix operations using C++ classes
typedef void* MatrixHandle;

MatrixHandle matrix_create(int rows, int cols);
void matrix_destroy(MatrixHandle handle);
void matrix_set(MatrixHandle handle, int row, int col, double value);
double matrix_get(MatrixHandle handle, int row, int col);
int matrix_rows(MatrixHandle handle);
int matrix_cols(MatrixHandle handle);
MatrixHandle matrix_multiply(MatrixHandle a, MatrixHandle b);
MatrixHandle matrix_transpose(MatrixHandle handle);
void matrix_print(MatrixHandle handle);

// Smart pointer operations (demonstrating RAII)
typedef void* SmartResourceHandle;

SmartResourceHandle smart_resource_create(int size);
void smart_resource_use(SmartResourceHandle handle, int index, double value);
double smart_resource_get(SmartResourceHandle handle, int index);
int smart_resource_size(SmartResourceHandle handle);

// Function pointer operations with C++ lambdas and std::function
typedef double (*MathOperation)(double, double);
typedef void* FunctionHandle;

FunctionHandle function_create_add();
FunctionHandle function_create_multiply();
FunctionHandle function_create_power();
void function_destroy(FunctionHandle handle);
double function_call(FunctionHandle handle, double a, double b);

// Iterator-style operations
typedef void* IteratorHandle;

IteratorHandle iterator_create(const int* array, int size);
void iterator_destroy(IteratorHandle handle);
int iterator_has_next(IteratorHandle handle);
int iterator_next(IteratorHandle handle);
void iterator_reset(IteratorHandle handle);
int iterator_find(IteratorHandle handle, int value);

// Exception handling demonstrations
typedef enum {
    CPP_SUCCESS = 0,
    CPP_NULL_POINTER = -1,
    CPP_OUT_OF_BOUNDS = -2,
    CPP_INVALID_OPERATION = -3,
    CPP_MEMORY_ERROR = -4,
    CPP_UNKNOWN_ERROR = -5
} CppResultCode;

CppResultCode safe_vector_get(VectorHandle handle, int index, int* result);
CppResultCode safe_matrix_multiply(MatrixHandle a, MatrixHandle b, MatrixHandle* result);
const char* get_last_error_message();

#ifdef __cplusplus
}
#endif

#endif // CPP_OPERATIONS_H