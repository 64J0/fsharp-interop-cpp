#include "cpp_operations.h"
#include <vector>
#include <string>
#include <memory>
#include <functional>
#include <algorithm>
#include <numeric>
#include <cmath>
#include <iostream>
#include <stdexcept>
#include <thread>

// Global error message for exception handling
static std::string last_error_message;

// C++ Vector wrapper class
class VectorWrapper {
private:
    std::vector<int> data;
    
public:
    void add(int value) { data.push_back(value); }
    int get(int index) const { return data.at(index); }
    int size() const { return static_cast<int>(data.size()); }
    void clear() { data.clear(); }
    
    int sum() const {
        return std::accumulate(data.begin(), data.end(), 0);
    }
    
    void sort() {
        std::sort(data.begin(), data.end());
    }
};

// C++ String wrapper class
class StringWrapper {
private:
    std::string data;
    
public:
    StringWrapper(const std::string& initial = "") : data(initial) {}
    
    const char* c_str() const { return data.c_str(); }
    void append(const std::string& text) { data += text; }
    void prepend(const std::string& text) { data = text + data; }
    int length() const { return static_cast<int>(data.length()); }
    
    void reverse() {
        std::reverse(data.begin(), data.end());
    }
    
    void to_upper() {
        std::transform(data.begin(), data.end(), data.begin(), ::toupper);
    }
    
    void to_lower() {
        std::transform(data.begin(), data.end(), data.begin(), ::tolower);
    }
};

// C++ Matrix class
class Matrix {
private:
    std::vector<std::vector<double>> data;
    int rows_, cols_;
    
public:
    Matrix(int rows, int cols) : rows_(rows), cols_(cols) {
        data.resize(rows, std::vector<double>(cols, 0.0));
    }
    
    void set(int row, int col, double value) {
        data.at(row).at(col) = value;
    }
    
    double get(int row, int col) const {
        return data.at(row).at(col);
    }
    
    int rows() const { return rows_; }
    int cols() const { return cols_; }
    
    std::unique_ptr<Matrix> multiply(const Matrix& other) const {
        if (cols_ != other.rows_) {
            throw std::invalid_argument("Matrix dimensions don't match for multiplication");
        }
        
        auto result = std::make_unique<Matrix>(rows_, other.cols_);
        for (int i = 0; i < rows_; i++) {
            for (int j = 0; j < other.cols_; j++) {
                double sum = 0.0;
                for (int k = 0; k < cols_; k++) {
                    sum += data[i][k] * other.data[k][j];
                }
                result->set(i, j, sum);
            }
        }
        return result;
    }
    
    std::unique_ptr<Matrix> transpose() const {
        auto result = std::make_unique<Matrix>(cols_, rows_);
        for (int i = 0; i < rows_; i++) {
            for (int j = 0; j < cols_; j++) {
                result->set(j, i, data[i][j]);
            }
        }
        return result;
    }
    
    void print() const {
        for (int i = 0; i < rows_; i++) {
            for (int j = 0; j < cols_; j++) {
                std::cout << data[i][j] << " ";
            }
            std::cout << std::endl;
        }
    }
};

// Smart Resource class demonstrating RAII
class SmartResource {
private:
    std::unique_ptr<double[]> data;
    int size_;
    
public:
    SmartResource(int size) : size_(size), data(std::make_unique<double[]>(size)) {
        // Initialize with zeros
        for (int i = 0; i < size; i++) {
            data[i] = 0.0;
        }
    }
    
    void set(int index, double value) {
        if (index >= 0 && index < size_) {
            data[index] = value;
        }
    }
    
    double get(int index) const {
        if (index >= 0 && index < size_) {
            return data[index];
        }
        return 0.0;
    }
    
    int size() const { return size_; }
};

// Iterator wrapper class
class IteratorWrapper {
private:
    std::vector<int> data;
    std::vector<int>::iterator current;
    
public:
    IteratorWrapper(const int* array, int size) : data(array, array + size) {
        current = data.begin();
    }
    
    bool has_next() const {
        return current != data.end();
    }
    
    int next() {
        if (has_next()) {
            return *current++;
        }
        return 0;
    }
    
    void reset() {
        current = data.begin();
    }
    
    bool find(int value) {
        auto it = std::find(data.begin(), data.end(), value);
        if (it != data.end()) {
            current = it;
            return true;
        }
        return false;
    }
};

// Template functions for mathematical operations
template<typename T>
T calculate_mean_template(const T* values, int count) {
    if (count <= 0) return T(0);
    T sum = std::accumulate(values, values + count, T(0));
    return sum / count;
}

template<typename T>
T calculate_variance_template(const T* values, int count) {
    if (count <= 0) return T(0);
    T mean = calculate_mean_template(values, count);
    T sum_squared_diff = 0;
    for (int i = 0; i < count; i++) {
        T diff = values[i] - mean;
        sum_squared_diff += diff * diff;
    }
    return sum_squared_diff / count;
}

// C API implementations

// Vector operations
extern "C" {

VectorHandle vector_create() {
    try {
        return new VectorWrapper();
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return nullptr;
    }
}

void vector_destroy(VectorHandle handle) {
    delete static_cast<VectorWrapper*>(handle);
}

void vector_add(VectorHandle handle, int value) {
    if (handle) {
        static_cast<VectorWrapper*>(handle)->add(value);
    }
}

int vector_get(VectorHandle handle, int index) {
    if (handle) {
        try {
            return static_cast<VectorWrapper*>(handle)->get(index);
        } catch (const std::exception& e) {
            last_error_message = e.what();
        }
    }
    return 0;
}

int vector_size(VectorHandle handle) {
    return handle ? static_cast<VectorWrapper*>(handle)->size() : 0;
}

void vector_clear(VectorHandle handle) {
    if (handle) {
        static_cast<VectorWrapper*>(handle)->clear();
    }
}

int vector_sum(VectorHandle handle) {
    return handle ? static_cast<VectorWrapper*>(handle)->sum() : 0;
}

void vector_sort(VectorHandle handle) {
    if (handle) {
        static_cast<VectorWrapper*>(handle)->sort();
    }
}

// String operations
StringHandle string_create(const char* initial_value) {
    try {
        return new StringWrapper(initial_value ? initial_value : "");
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return nullptr;
    }
}

void string_destroy(StringHandle handle) {
    delete static_cast<StringWrapper*>(handle);
}

const char* string_get_cstr(StringHandle handle) {
    return handle ? static_cast<StringWrapper*>(handle)->c_str() : "";
}

void string_append(StringHandle handle, const char* text) {
    if (handle && text) {
        static_cast<StringWrapper*>(handle)->append(text);
    }
}

void string_prepend(StringHandle handle, const char* text) {
    if (handle && text) {
        static_cast<StringWrapper*>(handle)->prepend(text);
    }
}

int string_length_cpp(StringHandle handle) {
    return handle ? static_cast<StringWrapper*>(handle)->length() : 0;
}

void string_reverse(StringHandle handle) {
    if (handle) {
        static_cast<StringWrapper*>(handle)->reverse();
    }
}

void string_to_upper(StringHandle handle) {
    if (handle) {
        static_cast<StringWrapper*>(handle)->to_upper();
    }
}

void string_to_lower(StringHandle handle) {
    if (handle) {
        static_cast<StringWrapper*>(handle)->to_lower();
    }
}

// Mathematical operations
double calculate_mean_double(const double* values, int count) {
    try {
        return calculate_mean_template(values, count);
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return 0.0;
    }
}

float calculate_mean_float(const float* values, int count) {
    try {
        return calculate_mean_template(values, count);
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return 0.0f;
    }
}

double calculate_variance(const double* values, int count) {
    try {
        return calculate_variance_template(values, count);
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return 0.0;
    }
}

double calculate_standard_deviation(const double* values, int count) {
    try {
        double variance = calculate_variance_template(values, count);
        return std::sqrt(variance);
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return 0.0;
    }
}

// Matrix operations
MatrixHandle matrix_create(int rows, int cols) {
    try {
        return new Matrix(rows, cols);
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return nullptr;
    }
}

void matrix_destroy(MatrixHandle handle) {
    delete static_cast<Matrix*>(handle);
}

void matrix_set(MatrixHandle handle, int row, int col, double value) {
    if (handle) {
        try {
            static_cast<Matrix*>(handle)->set(row, col, value);
        } catch (const std::exception& e) {
            last_error_message = e.what();
        }
    }
}

double matrix_get(MatrixHandle handle, int row, int col) {
    if (handle) {
        try {
            return static_cast<Matrix*>(handle)->get(row, col);
        } catch (const std::exception& e) {
            last_error_message = e.what();
        }
    }
    return 0.0;
}

int matrix_rows(MatrixHandle handle) {
    return handle ? static_cast<Matrix*>(handle)->rows() : 0;
}

int matrix_cols(MatrixHandle handle) {
    return handle ? static_cast<Matrix*>(handle)->cols() : 0;
}

MatrixHandle matrix_multiply(MatrixHandle a, MatrixHandle b) {
    if (!a || !b) return nullptr;
    try {
        auto result = static_cast<Matrix*>(a)->multiply(*static_cast<Matrix*>(b));
        return result.release();
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return nullptr;
    }
}

MatrixHandle matrix_transpose(MatrixHandle handle) {
    if (!handle) return nullptr;
    try {
        auto result = static_cast<Matrix*>(handle)->transpose();
        return result.release();
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return nullptr;
    }
}

void matrix_print(MatrixHandle handle) {
    if (handle) {
        static_cast<Matrix*>(handle)->print();
    }
}

// Smart resource operations
SmartResourceHandle smart_resource_create(int size) {
    try {
        return new SmartResource(size);
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return nullptr;
    }
}

void smart_resource_use(SmartResourceHandle handle, int index, double value) {
    if (handle) {
        static_cast<SmartResource*>(handle)->set(index, value);
    }
}

double smart_resource_get(SmartResourceHandle handle, int index) {
    return handle ? static_cast<SmartResource*>(handle)->get(index) : 0.0;
}

int smart_resource_size(SmartResourceHandle handle) {
    return handle ? static_cast<SmartResource*>(handle)->size() : 0;
}

// Function operations
FunctionHandle function_create_add() {
    try {
        auto func = new std::function<double(double, double)>([](double a, double b) { return a + b; });
        return func;
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return nullptr;
    }
}

FunctionHandle function_create_multiply() {
    try {
        auto func = new std::function<double(double, double)>([](double a, double b) { return a * b; });
        return func;
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return nullptr;
    }
}

FunctionHandle function_create_power() {
    try {
        auto func = new std::function<double(double, double)>([](double a, double b) { return std::pow(a, b); });
        return func;
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return nullptr;
    }
}

void function_destroy(FunctionHandle handle) {
    delete static_cast<std::function<double(double, double)>*>(handle);
}

double function_call(FunctionHandle handle, double a, double b) {
    if (handle) {
        try {
            return (*static_cast<std::function<double(double, double)>*>(handle))(a, b);
        } catch (const std::exception& e) {
            last_error_message = e.what();
        }
    }
    return 0.0;
}

// Iterator operations
IteratorHandle iterator_create(const int* array, int size) {
    try {
        return new IteratorWrapper(array, size);
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return nullptr;
    }
}

void iterator_destroy(IteratorHandle handle) {
    delete static_cast<IteratorWrapper*>(handle);
}

int iterator_has_next(IteratorHandle handle) {
    return handle && static_cast<IteratorWrapper*>(handle)->has_next() ? 1 : 0;
}

int iterator_next(IteratorHandle handle) {
    return handle ? static_cast<IteratorWrapper*>(handle)->next() : 0;
}

void iterator_reset(IteratorHandle handle) {
    if (handle) {
        static_cast<IteratorWrapper*>(handle)->reset();
    }
}

int iterator_find(IteratorHandle handle, int value) {
    return handle && static_cast<IteratorWrapper*>(handle)->find(value) ? 1 : 0;
}

// Exception handling
CppResultCode safe_vector_get(VectorHandle handle, int index, int* result) {
    if (!handle) return CPP_NULL_POINTER;
    if (!result) return CPP_NULL_POINTER;
    
    try {
        *result = static_cast<VectorWrapper*>(handle)->get(index);
        return CPP_SUCCESS;
    } catch (const std::out_of_range& e) {
        last_error_message = e.what();
        return CPP_OUT_OF_BOUNDS;
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return CPP_UNKNOWN_ERROR;
    }
}

CppResultCode safe_matrix_multiply(MatrixHandle a, MatrixHandle b, MatrixHandle* result) {
    if (!a || !b) return CPP_NULL_POINTER;
    if (!result) return CPP_NULL_POINTER;
    
    try {
        auto product = static_cast<Matrix*>(a)->multiply(*static_cast<Matrix*>(b));
        *result = product.release();
        return CPP_SUCCESS;
    } catch (const std::invalid_argument& e) {
        last_error_message = e.what();
        return CPP_INVALID_OPERATION;
    } catch (const std::exception& e) {
        last_error_message = e.what();
        return CPP_UNKNOWN_ERROR;
    }
}

const char* get_last_error_message() {
    return last_error_message.c_str();
}

} // extern "C"