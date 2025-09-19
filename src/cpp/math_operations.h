#ifndef MATH_OPERATIONS_H
#define MATH_OPERATIONS_H

#ifdef __cplusplus
extern "C" {
#endif

// Basic primitive type operations
int add_integers(int a, int b);
float multiply_floats(float a, float b);
double divide_doubles(double a, double b);
int is_even(int number);

// String operations
int string_length(const char* str);
void copy_string(const char* source, char* destination, int max_length);
const char* get_greeting(const char* name);

// Struct operations
typedef struct {
    int x;
    int y;
} Point;

typedef struct {
    float width;
    float height;
} Rectangle;

Point create_point(int x, int y);
float calculate_distance(Point p1, Point p2);
Rectangle create_rectangle(float width, float height);
float rectangle_area(Rectangle rect);

// Array operations
void fill_array(int* array, int size, int value);
int sum_array(const int* array, int size);
void sort_array(int* array, int size);

// Callback function type
typedef void (*ProgressCallback)(int progress);
void simulate_work(int duration_ms, ProgressCallback callback);

// Error handling
typedef enum {
    RESULT_SUCCESS = 0,
    RESULT_NULL_POINTER = -1,
    RESULT_INVALID_PARAMETER = -2,
    RESULT_BUFFER_TOO_SMALL = -3
} ResultCode;

ResultCode safe_divide(double a, double b, double* result);
ResultCode validate_array(const int* array, int size);

// Memory management
char* allocate_string(int length);
void free_string(char* str);

#ifdef __cplusplus
}
#endif

#endif // MATH_OPERATIONS_H