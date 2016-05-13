using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Docker.PowerShell.Support
{
    internal enum ConsoleDirection
    {
        In,
        Out,
    }

    internal class ConsoleStream : IDisposable
    {
        private IntPtr _handle;
        private int _oldMode;
        private int _currentMode;
        public Stream Stream { get; private set; }
        public ConsoleDirection Direction { get; private set; }

        public ConsoleStream(ConsoleDirection dir)
        {
            Direction = dir;
            _handle = Win32ConsoleInterop.GetStdHandle(dir == ConsoleDirection.In ? Win32ConsoleInterop.STD_INPUT_HANDLE : Win32ConsoleInterop.STD_OUTPUT_HANDLE);
            if (!Win32ConsoleInterop.GetConsoleMode(_handle, out _oldMode))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            _currentMode = _oldMode;

            // Now that it is known that this is a console, reopen the console handle for
            // asynchronous access so that IO to it can be canceled.
            var newHandle = Win32ConsoleInterop.CreateFile(
                dir == ConsoleDirection.In ? "CONIN$" : "CONOUT$",
                dir == ConsoleDirection.In ? Win32ConsoleInterop.GENERIC_READ : Win32ConsoleInterop.GENERIC_WRITE,
                FileShare.ReadWrite,
                new IntPtr(),
                FileMode.Open,
                Win32ConsoleInterop.FILE_FLAG_OVERLAPPED,
                new IntPtr());

            if (newHandle.IsInvalid)
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            Stream = new FileStream(newHandle, dir == ConsoleDirection.In ? FileAccess.Read : FileAccess.Write, 1, true);
        }

        public void EnableRawInputMode()
        {
            if (Direction != ConsoleDirection.In)
            {
                throw new InvalidOperationException("cannot set raw mode on output handle");
            }

            // Put the console in character mode with no input processing.
            var newMode = _currentMode & ~(Win32ConsoleInterop.ENABLE_PROCESSED_INPUT |
                                           Win32ConsoleInterop.ENABLE_LINE_INPUT |
                                           Win32ConsoleInterop.ENABLE_ECHO_INPUT |
                                           Win32ConsoleInterop.ENABLE_MOUSE_INPUT);

            if (!Win32ConsoleInterop.SetConsoleMode(_handle, newMode))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            _currentMode = newMode;
        }

        public void EnableVTMode()
        {
            int newMode;
            if (Direction == ConsoleDirection.Out)
            {
                // Put the console in character mode with no input processing.
                newMode = _currentMode | Win32ConsoleInterop.ENABLE_PROCESSED_OUTPUT | Win32ConsoleInterop.ENABLE_VIRTUAL_TERMINAL_PROCESSING | Win32ConsoleInterop.DISABLE_NEWLINE_AUTO_RETURN;
            }
            else
            {
                newMode = _currentMode | Win32ConsoleInterop.ENABLE_VIRTUAL_TERMINAL_INPUT;
            }

            if (!Win32ConsoleInterop.SetConsoleMode(_handle, newMode))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            _currentMode = newMode;
        }

        public void Dispose()
        {
            Stream.Dispose();
            if (_currentMode != _oldMode)
            {
                if (Win32ConsoleInterop.SetConsoleMode(_handle, _oldMode))
                {
                    _currentMode = _oldMode;
                }
            }
        }
    }
}