using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace Docker.PowerShell.Support
{
    internal sealed class Win32ConsoleStream : ConsoleStream
    {
        public Win32ConsoleStream(ConsoleDirection dir) : base(dir)
        {
            _handle = GetStdHandle(dir == ConsoleDirection.In ? STD_INPUT_HANDLE : STD_OUTPUT_HANDLE);
            if (!GetConsoleMode(_handle, out _oldMode))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            _currentMode = _oldMode;

            // Now that it is known that this is a console, reopen the console handle for
            // asynchronous access so that IO to it can be canceled.
            var newHandle = CreateFile(
                dir == ConsoleDirection.In ? "CONIN$" : "CONOUT$",
                dir == ConsoleDirection.In ? GENERIC_READ : GENERIC_WRITE,
                FileShare.ReadWrite,
                new IntPtr(),
                FileMode.Open,
                FILE_FLAG_OVERLAPPED,
                new IntPtr());

            if (newHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            Stream = new FileStream(newHandle, dir == ConsoleDirection.In ? FileAccess.Read : FileAccess.Write, 1, true);
        }

        override public void EnableRawInputMode()
        {
            if (Direction != ConsoleDirection.In)
            {
                throw new InvalidOperationException("cannot set raw mode on output handle");
            }

            // Put the console in character mode with no input processing.
            var newMode = _currentMode & ~(ENABLE_PROCESSED_INPUT |
                                           ENABLE_LINE_INPUT |
                                           ENABLE_ECHO_INPUT |
                                           ENABLE_MOUSE_INPUT);

            if (!SetConsoleMode(_handle, newMode))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            _currentMode = newMode;
        }

        override public void EnableVTMode()
        {
            int newMode;
            if (Direction == ConsoleDirection.Out)
            {
                // Put the console in character mode with no input processing.
                newMode = _currentMode | ENABLE_PROCESSED_OUTPUT | ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
            }
            else
            {
                newMode = _currentMode | ENABLE_VIRTUAL_TERMINAL_INPUT;
            }

            if (!SetConsoleMode(_handle, newMode))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            _currentMode = newMode;
        }

        override public void Dispose()
        {
            if (_currentMode != _oldMode)
            {
                if (SetConsoleMode(_handle, _oldMode))
                {
                    _currentMode = _oldMode;
                }
            }
            base.Dispose();
        }

        [DllImport("api-ms-win-core-file-l1-1-0.dll", EntryPoint = "CreateFileW", SetLastError = true, CharSet = CharSet.Unicode, BestFitMapping = false)]
        private static extern SafeFileHandle CreateFile(
            string lpFileName,
            int dwDesiredAccess,
            System.IO.FileShare dwShareMode,
            IntPtr securityAttrs,
            System.IO.FileMode dwCreationDisposition,
            int dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("api-ms-win-core-console-l1-1-0.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("api-ms-win-core-console-l1-1-0.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr handle, int mode);

        [DllImport("api-ms-win-core-processenvironment-l1-1-0.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        private const int STD_INPUT_HANDLE = -10;
        private const int STD_OUTPUT_HANDLE = -11;
        private const int STD_ERROR_HANDLE = -12;

        private const int GENERIC_READ = unchecked((int)0x80000000);
        private const int GENERIC_WRITE = 0x40000000;

        private const int FILE_FLAG_OVERLAPPED = 0x40000000;

        // Input flags.
        private const int ENABLE_PROCESSED_INPUT = 0x1;
        private const int ENABLE_LINE_INPUT = 0x2;
        private const int ENABLE_ECHO_INPUT = 0x4;
        private const int ENABLE_MOUSE_INPUT = 0x10;
        private const int ENABLE_VIRTUAL_TERMINAL_INPUT = 0x200;

        // Output flags.
        private const int ENABLE_PROCESSED_OUTPUT = 0x1;
        private const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x4;
        private const int DISABLE_NEWLINE_AUTO_RETURN = 0x8;
    }
}