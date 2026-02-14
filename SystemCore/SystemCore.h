#pragma once

extern "C" {
    __declspec(dllexport) void GetProcessListStr(char* buffer, int bufferSize);

    __declspec(dllexport) bool KillProcess(int pid);
}