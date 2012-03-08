using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Management;
using System.Security.Principal;
using System.Security.Permissions;

namespace microDrum
{
    class UtilitySDCard
    {
        [Flags]
        public enum DesiredAccess : uint
        {
            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000
        }
        [Flags]
        public enum ShareMode : uint
        {
            FILE_SHARE_NONE = 0x0,
            FILE_SHARE_READ = 0x1,
            FILE_SHARE_WRITE = 0x2,
            FILE_SHARE_DELETE = 0x4,

        }
        public enum MoveMethod : uint
        {
            FILE_BEGIN = 0,
            FILE_CURRENT = 1,
            FILE_END = 2
        }
        public enum CreationDisposition : uint
        {
            CREATE_NEW = 1,
            CREATE_ALWAYS = 2,
            OPEN_EXISTING = 3,
            OPEN_ALWAYS = 4,
            TRUNCATE_EXSTING = 5
        }
        [Flags]
        public enum FlagsAndAttributes : uint
        {
            FILE_ATTRIBUTES_ARCHIVE = 0x20,
            FILE_ATTRIBUTE_HIDDEN = 0x2,
            FILE_ATTRIBUTE_NORMAL = 0x80,
            FILE_ATTRIBUTE_OFFLINE = 0x1000,
            FILE_ATTRIBUTE_READONLY = 0x1,
            FILE_ATTRIBUTE_SYSTEM = 0x4,
            FILE_ATTRIBUTE_TEMPORARY = 0x100,
            FILE_FLAG_WRITE_THROUGH = 0x80000000,
            FILE_FLAG_OVERLAPPED = 0x40000000,
            FILE_FLAG_NO_BUFFERING = 0x20000000,
            FILE_FLAG_RANDOM_ACCESS = 0x10000000,
            FILE_FLAG_SEQUENTIAL_SCAN = 0x8000000,
            FILE_FLAG_DELETE_ON = 0x4000000,
            FILE_FLAG_POSIX_SEMANTICS = 0x1000000,
            FILE_FLAG_OPEN_REPARSE_POINT = 0x200000,
            FILE_FLAG_OPEN_NO_CALL = 0x100000
        }

        public const uint INVALID_HANDLE_VALUE = 0xFFFFFFFF;
        public const uint INVALID_SET_FILE_POINTER = 0xFFFFFFFF;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern SafeFileHandle CreateFile(
         string lpFileName,
         DesiredAccess dwDesiredAccess,
         ShareMode dwShareMode,
         IntPtr lpSecurityAttributes,
         CreationDisposition dwCreationDisposition,
         FlagsAndAttributes dwFlagsAndAttributes,
         IntPtr hTemplateFile);

        [DllImport("kernel32", SetLastError = true)]
        internal static extern Int32 CloseHandle(
         SafeFileHandle hObject);

        [DllImport("kernel32", SetLastError = true)]
        internal static extern bool ReadFile(
         SafeFileHandle hFile,
         byte[] aBuffer,
         int cbToRead,
         ref int cbThatWereRead,
         IntPtr pOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool WriteFile(
         SafeFileHandle hFile,
         Byte[] aBuffer,
         UInt32 cbToWrite,
         ref UInt32 cbThatWereWritten,
         IntPtr pOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern UInt32 SetFilePointer(
         SafeFileHandle hFile,
         Int32 cbDistanceToMove,
         IntPtr pDistanceToMoveHigh,
         MoveMethod fMoveMethod);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetEndOfFile(
         SafeFileHandle hFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern UInt32 GetFileSize(
         SafeFileHandle hFile,
         IntPtr pFileSizeHigh);

        private static SafeFileHandle _hFile = null;

        public static string[] GetSDCards()
        {
            ManagementObjectSearcher mosDisks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            List<string> Devices = new List<string>();
            foreach (ManagementObject moDisk in mosDisks.Get())
            {
                if (moDisk["MediaType"].ToString() == "Removable Media" &&
                    moDisk["Caption"].ToString() == "SD Memory Card")
                    Devices.Add(moDisk["Model"].ToString());
            }
            return Devices.ToArray();
        }
        private static string GetSDCard_Device(string Model)
        {
            ManagementObjectSearcher mosDisks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject moDisk in mosDisks.Get())
            {
                if (moDisk["MediaType"].ToString() == "Removable Media" &&
                    moDisk["Caption"].ToString() == "SD Memory Card" &&
                    moDisk["Model"].ToString() == Model)
                    return moDisk["DeviceID"].ToString();
            }
            return "";
        }
        /* ---------------------------------------------------------
         * Open and Close
         * ------------------------------------------------------ */
        // [PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        private static void Open(string sFileName,
            DesiredAccess fDesiredAccess,
            ShareMode fShareMode,
            CreationDisposition fCreationDisposition,
            FlagsAndAttributes fFlagsAndAttributes)
        {
            //System.Security.Principal.WindowsIdentity.Impersonate( x=new System.Security.Principal.WindowsIdentity(
            if (sFileName.Length == 0)
                throw new ArgumentNullException("FileName");
            _hFile = CreateFile(sFileName, fDesiredAccess, fShareMode,
             IntPtr.Zero, fCreationDisposition, fFlagsAndAttributes,
             IntPtr.Zero);
            if (_hFile.IsInvalid)
            {
                _hFile = null;
                ThrowLastWin32Err();
            }
            //_fDisposed = false;

        }

        private static void ThrowLastWin32Err()
        {
            Marshal.ThrowExceptionForHR(
            Marshal.GetHRForLastWin32Error());
        }

        public void Close()
        {
            if (_hFile == null)
                return;
            _hFile.Close();
            _hFile = null;
            // _fDisposed = true;
        }
        /* ---------------------------------------------------------
         * Read and Write
         * ------------------------------------------------------ */

        private static int Read(byte[] buffer)//, uint cbToRead)
        {
            // returns bytes read
            int cbThatWereRead = 0;
            if (!ReadFile(_hFile, buffer, buffer.Length,
             ref cbThatWereRead, IntPtr.Zero))
                ThrowLastWin32Err();
            return cbThatWereRead;
        }

        private static uint Write(byte[] buffer)
        {
            // returns bytes write
            uint cbThatWereWritten = 0;
            if (!WriteFile(_hFile, buffer, (uint)buffer.Length,
             ref cbThatWereWritten, IntPtr.Zero))
                ThrowLastWin32Err();
            return cbThatWereWritten;
        }

        //PUBLIC============================================

        public static void Open(string Model)
        {
            Open(GetSDCard_Device(Model), DesiredAccess.GENERIC_READ | DesiredAccess.GENERIC_WRITE,
                ShareMode.FILE_SHARE_READ | ShareMode.FILE_SHARE_WRITE,
                CreationDisposition.OPEN_EXISTING, 0);
        }
        public static byte[] ReadSector(int nSector)
        {
            byte[] buffer = new byte[512];
            MoveFilePointer(nSector * 512, MoveMethod.FILE_BEGIN);
            Read(buffer);
            return buffer;
        }
        public static void WriteSector(byte[] buffer, int nSector)
        {
            if (buffer.Length != 512) return;

            MoveFilePointer(nSector * 512, MoveMethod.FILE_BEGIN);
            Write(buffer);
        }
        //====================================================
        /* ---------------------------------------------------------
         * Move file pointer
         * ------------------------------------------------------ */

        private static void MoveFilePointer(int cbToMove)
        {
            MoveFilePointer(cbToMove, MoveMethod.FILE_CURRENT);
        }

        private static void MoveFilePointer(int cbToMove,
         MoveMethod fMoveMethod)
        {
            if (_hFile != null)
                if (SetFilePointer(_hFile, cbToMove, IntPtr.Zero,
                 fMoveMethod) == INVALID_SET_FILE_POINTER)
                    ThrowLastWin32Err();
        }

        private int FilePointer
        {
            get
            {
                return (_hFile != null) ? (int)SetFilePointer(_hFile, 0,
                 IntPtr.Zero, MoveMethod.FILE_CURRENT) : 0;
            }
            set
            {
                MoveFilePointer(value);
            }
        }

    }
}
