#include "MyLibrary.h"
#include <algorithm>
#include <cstring>
#include <iostream>

extern "C" {

int32_t Add(int32_t a, int32_t b) { return a + b; }

void ReverseString(const char *input, char *output, int32_t buffer_size) {
  if (input == nullptr || output == nullptr || buffer_size <= 0) {
    return;
  }

  std::string temp(input);
  std::reverse(temp.begin(), temp.end());

  size_t len = temp.length();
  if (len >= buffer_size) {
    len = buffer_size - 1;
  }

  std::strncpy(output, temp.c_str(), len);
  output[len] = '\0'; // Null-terminate
}

}
