using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Docker.PowerShell.Support
{
    internal static class Win32ConsoleInterop
    {
        [DllImport("api-ms-win-core-file-l1-1-0.dll", EntryPoint = "CreateFileW", SetLastError = true, CharSet = CharSet.Unicode, BestFitMapping = false)]
        public static extern SafeFileHandle CreateFile(
            string lpFileName,
            int dwDesiredAccess,
            System.IO.FileShare dwShareMode,
            IntPtr securityAttrs,
            System.IO.FileMode dwCreationDisposition,
            int dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("api-ms-win-core-console-l1-1-0.dll", SetLastError = true)]
        public extern static bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("api-ms-win-core-console-l1-1-0.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr handle, int mode);

        [DllImport("api-ms-win-core-processenvironment-l1-1-0.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        public const int STD_INPUT_HANDLE = -10;
        public const int STD_OUTPUT_HANDLE = -11;
        public const int STD_ERROR_HANDLE = -12;

        public const int GENERIC_READ = unchecked((int)0x80000000);
        public const int GENERIC_WRITE = 0x40000000;

        public const int FILE_FLAG_OVERLAPPED = 0x40000000;

        // Input flags.
        public const int ENABLE_PROCESSED_INPUT = 0x1;
        public const int ENABLE_LINE_INPUT = 0x2;
        public const int ENABLE_ECHO_INPUT = 0x4;
        public const int ENABLE_MOUSE_INPUT = 0x10;
        public const int ENABLE_VIRTUAL_TERMINAL_INPUT = 0x200;

        // Output flags.
        public const int ENABLE_PROCESSED_OUTPUT = 0x1;
        public const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x4;
        public const int DISABLE_NEWLINE_AUTO_RETURN = 0x8;
    }
}
