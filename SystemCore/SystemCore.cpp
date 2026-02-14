#include "pch.h"
#include "SystemCore.h"
#include <windows.h>
#include <tlhelp32.h>
#include <psapi.h> 
#include <stdio.h>

#pragma comment(lib, "psapi.lib")

void GetProcessListStr(char* buffer, int bufferSize) {
    memset(buffer, 0, bufferSize);

    HANDLE hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (hSnapshot == INVALID_HANDLE_VALUE) return;

    PROCESSENTRY32 pe;
    pe.dwSize = sizeof(PROCESSENTRY32);

    if (Process32First(hSnapshot, &pe)) {
        do {
            HANDLE hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, pe.th32ProcessID);

            long ramUsageMB = 0;

            if (hProcess != NULL) {
                PROCESS_MEMORY_COUNTERS pmc;
                if (GetProcessMemoryInfo(hProcess, &pmc, sizeof(pmc))) {
                    ramUsageMB = pmc.WorkingSetSize / (1024 * 1024);
                }
                CloseHandle(hProcess);
            }

            
            char entry[260];
            sprintf_s(entry, "%s|%d|%ld;", pe.szExeFile, pe.th32ProcessID, ramUsageMB);

            if (strlen(buffer) + strlen(entry) < bufferSize) {
                strcat_s(buffer, bufferSize, entry);
            }

        } while (Process32Next(hSnapshot, &pe));
    }
    CloseHandle(hSnapshot);
}

bool KillProcess(int pid) {
    HANDLE hProcess = OpenProcess(PROCESS_TERMINATE, FALSE, pid);
    if (hProcess == NULL) return false;
    bool result = TerminateProcess(hProcess, 1);
    CloseHandle(hProcess);
    return result;
}