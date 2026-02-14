# 🕵️ Process Investigator Manager

C++ (Backend) ve C# (Frontend) kullanılarak geliştirilmiş, Windows çekirdeği ile haberleşen hibrit bir görev yöneticisi.

## 🚀 Özellikler
- **Hibrit Mimari:** C++ DLL ve C# WPF arayüzü `P/Invoke` ile haberleşir.
- **Derin Analiz:** Windows API (`CreateToolhelp32Snapshot`) kullanarak anlık process takibi.
- **RAM Kullanımı:** PSAPI kütüphanesi ile bellek tüketimi analizi (MB cinsinden).
- **Process Sonlandırma:** Seçili işlemi Kernel seviyesinde sonlandırma (`TerminateProcess`).
- **Filtreleme:** Anlık arama ve filtreleme özelliği.

## 🛠️ Kullanılan Teknolojiler
- **C++** (WinAPI, Memory Management, DLL Export)
- **C# / .NET 8** (WPF, Interop Services, UI Design)
