#pragma once

#include <stdint.h>
#include <string>

extern "C" {
  int32_t Add(int32_t a, int32_t b);
  void ReverseString(const char* input, char* output, int32_t buffer_size);
}
