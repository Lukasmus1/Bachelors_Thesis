using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DesktopGeneration.IconGeneration
{
    public abstract class WindowsIconPositionUtil
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, out uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, uint nSize, out uint lpNumberOfBytesWritten);

        [DllImport("user32.dll")]
        private static extern uint GetDpiForSystem();
        
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        
        //Structure for Point
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int x;
            public int y;
            
            public override string ToString()
            {
                return $"({x}, {y})";
            }

            public static Point operator -(Point left, Point right)
            {
                return new Point() {x = left.x - right.x, y = left.y - right.y};
            }
        }

        //Structure for ListView item
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct Lvitem
        {
            public uint mask;
            public int iItem;
            public int iSubItem;
            public uint state;
            public uint stateMask;
            public IntPtr pszText;
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
        }

        public struct DesktopIcon : IEquatable<DesktopIcon>
        {
            public System.Drawing.Point Position { get; set; }
            public string Name { get; set; }
            
            public override string ToString()
            {
                return $"{Name} ({Position.X}, {Position.Y})";
            }

            public bool Equals(DesktopIcon other)
            {
                return Position.Equals(other.Position) && Name == other.Name;
            }

            public override bool Equals(object obj)
            {
                return obj is DesktopIcon other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Position, Name);
            }
        }

        const uint LVM_GETITEMCOUNT = 0x1004;
        const uint LVM_GETITEMPOSITION = 0x1010;
        const uint LVM_GETITEMTEXTA = 0x102D;  //ANSI
        const uint LVM_GETITEMTEXTW = 0x1073;  //Unicode
        const uint LVIF_TEXT = 0x0001;
        const uint PROCESS_VM_OPERATION = 0x0008;
        const uint PROCESS_VM_READ = 0x0010;
        const uint PROCESS_VM_WRITE = 0x0020;
        const uint MEM_COMMIT = 0x1000;
        const uint MEM_RELEASE = 0x8000;
        const uint PAGE_READWRITE = 0x04;
        const uint LVM_GETITEMSPACING = 0x1035;
        const uint LVM_GETITEMRECT = 0x100E;
        
        public static Point IconSize { get; private set; } = new Point { x = 0, y = 0 };
        public static Point IconSpacing { get; private set; } = new Point { x = 0, y = 0 };
        
         public static List<DesktopIcon> GetDesktopIconPositions()
        {
            List<DesktopIcon> icons = new();
            IntPtr explorerProcess = IntPtr.Zero;
            IntPtr remotePositionBuffer = IntPtr.Zero;

            try
            {
                //Get the ListView handle for desktop icons
                IntPtr listView = GetDesktopListView();
                if (listView == IntPtr.Zero)
                {
                    //err
                    return icons;
                }
                

                //Getting process ID of the ListView 
                //This is done because the icons are in a different process (explorer.exe) and I need to manage memory there
                uint explorerProcessId;
                GetWindowThreadProcessId(listView, out explorerProcessId);

                //Open process with necessary permissions
                explorerProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, false, explorerProcessId);
                if (explorerProcess == IntPtr.Zero)
                {
                    //err
                    return icons;
                }

                //Allocate memory in the explorer process for position
                uint bufferSize = Math.Max((uint)Marshal.SizeOf<Point>(), (uint)Marshal.SizeOf<Lvitem>() + 512);
                remotePositionBuffer = VirtualAllocEx(explorerProcess, IntPtr.Zero, bufferSize, MEM_COMMIT, PAGE_READWRITE);
                if (remotePositionBuffer == IntPtr.Zero)
                {
                    //err
                    return icons;
                }

                //Allocate memory in the explorer process for text
                IntPtr remoteTextBuffer = VirtualAllocEx(explorerProcess, IntPtr.Zero, 512, MEM_COMMIT, PAGE_READWRITE);
                if (remoteTextBuffer == IntPtr.Zero)
                {
                    //err
                    return icons;
                }

                //Getting the icon count
                IntPtr itemCount = SendMessage(listView, LVM_GETITEMCOUNT, IntPtr.Zero, IntPtr.Zero);
                int count = itemCount.ToInt32();
                
                System.Drawing.Point offset = GetDesktopOffset();

                IconSize = GetIconSize(explorerProcess, listView);
                IconSpacing = GetIconSpacing(listView);
                
                for (int i = 0; i < count; i++)
                {
                    string iconName = "";
                    System.Drawing.Point iconPosition = new();
                    
                    //Get position of the icon
                    IntPtr result = SendMessage(listView, LVM_GETITEMPOSITION, new IntPtr(i), remotePositionBuffer);
                    
                    if (result != IntPtr.Zero)
                    {
                        //Allocate a local buffer to read the position
                        IntPtr localBuffer = Marshal.AllocHGlobal(Marshal.SizeOf<Point>());

                        //Read the position from the remote process memory and save it to localBuffer
                        if (ReadProcessMemory(explorerProcess, remotePositionBuffer, localBuffer, (uint)Marshal.SizeOf<Point>(), out uint _))
                        {
                            //Convert the localBuffer data to POINT structure
                            Point point = Marshal.PtrToStructure<Point>(localBuffer);
                            iconPosition = new System.Drawing.Point(point.x, point.y);
                        }
                        
                        //Free the local buffer
                        Marshal.FreeHGlobal(localBuffer);
                    }

                    //Get the icon text - ANSI
                    iconName = GetIconText(listView, explorerProcess, remotePositionBuffer, remoteTextBuffer, i, true);
                    
                    //If ANSI failed, try Unicode
                    if (string.IsNullOrEmpty(iconName) || iconName.Contains('?'))
                    {
                        iconName = GetIconText(listView, explorerProcess, remotePositionBuffer, remoteTextBuffer, i, false);
                    }
                    
                    //Add the icon to the list
                    if (!string.IsNullOrEmpty(iconName) && iconPosition != System.Drawing.Point.Empty)
                    {
                        //Adjust the position based on the offset of the main screen
                        iconPosition.X -= offset.X;
                        iconPosition.Y -= offset.Y;
                        
                        //Normalize the position to the pre-set 2560x1440 resolution of the canvas
                        iconPosition.X *= 2560 / Screen.width;
                        iconPosition.Y *= 1440 / Screen.height;
                        
                        //Adjust the position to be in the center of the icon
                        iconPosition.X += IconSize.x / 2;
                        iconPosition.Y += IconSize.y / 2;
                        
                        //Add the icon to the list
                        icons.Add(new DesktopIcon 
                        { 
                            Name = iconName,
                            Position = iconPosition
                        });
                    }
                }

                //Free the allocated memory in the remote process
                if (remoteTextBuffer != IntPtr.Zero)
                {
                    VirtualFreeEx(explorerProcess, remoteTextBuffer, 0, MEM_RELEASE);
                }
            }
            catch
            {
                //Same as finally block
            }
            finally
            {
                //Cleanup
                if (remotePositionBuffer != IntPtr.Zero && explorerProcess != IntPtr.Zero)
                {
                    VirtualFreeEx(explorerProcess, remotePositionBuffer, 0, MEM_RELEASE);
                }

                if (explorerProcess != IntPtr.Zero)
                {
                    CloseHandle(explorerProcess);
                }
            }

            return icons;
        }

        private static IntPtr GetDesktopListView()
        {
            //Looking through the Program hierarchy to find the desktop ListView
            IntPtr progman = FindWindow("Progman", "Program Manager");
            IntPtr shellView = FindWindowEx(progman, IntPtr.Zero, "SHELLDLL_DefView", null);
            
            if (shellView != IntPtr.Zero)
            {
                IntPtr listView = FindWindowEx(shellView, IntPtr.Zero, "SysListView32", "FolderView");
                if (listView != IntPtr.Zero) return listView;
            }
            
            //If not found, try to find it through WorkerW
            IntPtr workerw = IntPtr.Zero;
            do
            {
                workerw = FindWindowEx(IntPtr.Zero, workerw, "WorkerW", null);
                if (workerw == IntPtr.Zero) 
                    continue;
                
                shellView = FindWindowEx(workerw, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (shellView == IntPtr.Zero) 
                    continue;
                
                IntPtr listView = FindWindowEx(shellView, IntPtr.Zero, "SysListView32", "FolderView");
                if (listView != IntPtr.Zero) 
                    return listView;
                
            } while (workerw != IntPtr.Zero);

            return IntPtr.Zero;
        }

        private static string GetIconText(IntPtr listView, IntPtr process, IntPtr buffer, IntPtr textBuffer, int index, bool useAnsi)
        {
            IntPtr localItem = IntPtr.Zero;
            try
            {
                Lvitem item = new()
                {
                    mask = LVIF_TEXT,
                    iItem = index,
                    iSubItem = 0,
                    pszText = textBuffer,
                    cchTextMax = 255
                };

                //Creating a pointer to the Lvitem structure in the remote process
                localItem = Marshal.AllocHGlobal(Marshal.SizeOf<Lvitem>());
                Marshal.StructureToPtr(item, localItem, false);

                if (WriteProcessMemory(process, buffer, localItem, (uint)Marshal.SizeOf<Lvitem>(), out uint _))
                {
                    //Checking coding and sending the message to get the text
                    uint message = useAnsi ? LVM_GETITEMTEXTA : LVM_GETITEMTEXTW;
                    IntPtr textResult = SendMessage(listView, message, new IntPtr(index), buffer);
                    
                    if (textResult.ToInt32() > 0)
                    {
                        //Reading the text from the remote process memory
                        byte[] textBytes = new byte[512];

                        if (ReadProcessMemory(process, textBuffer, Marshal.UnsafeAddrOfPinnedArrayElement(textBytes, 0), 512, out uint _))
                        {
                            //Convert the byte array to string based on encoding
                            string res = useAnsi ? System.Text.Encoding.Default.GetString(textBytes).TrimEnd('\0') : System.Text.Encoding.Unicode.GetString(textBytes).TrimEnd('\0');

                            //Reset the remote text buffer to zeros before reading
                            byte[] zeroBuffer = new byte[512];
                            WriteProcessMemory(process, textBuffer, Marshal.UnsafeAddrOfPinnedArrayElement(zeroBuffer, 0), (uint)zeroBuffer.Length, out uint _);
                            
                            //Free the local item buffer
                            Marshal.FreeHGlobal(localItem);
                            return res;
                        }
                    }
                }

                //Free the local item buffer
                Marshal.FreeHGlobal(localItem);
            }
            catch
            {
                if (localItem != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(localItem);
                }
            }
            
            return "";
        }

        private static Point GetIconSpacing(IntPtr listView)
        {
            //IntPtr.Zero -> get both x and y spacing
            IntPtr spacing = SendMessage(listView, LVM_GETITEMSPACING, IntPtr.Zero, IntPtr.Zero);
            int spacingValue = spacing.ToInt32();
            float scaling = GetWindowsScaling();
            int spacingX = (int)((spacingValue & 0xFFFF) * scaling);
            int spacingY = (int)(((spacingValue >> 16) & 0xFFFF) * scaling);

            return new Point { x = spacingX, y = spacingY };
        }

        //Assumes the icon count is at least 1s
        public static Point GetIconSize(IntPtr explorerProcess, IntPtr listView)
        {
            Point res = new Point{x = 0, y = 0};
            
            //Allocate memory for RECT structure in the remote process
            IntPtr remoteRectBuffer = VirtualAllocEx(explorerProcess, IntPtr.Zero, (uint)Marshal.SizeOf<RECT>(), MEM_COMMIT, PAGE_READWRITE);
            if (remoteRectBuffer == IntPtr.Zero)
            {
                return res;
            }
            
            //Write the index of the icon as the first int in the RECT struct
            byte[] indexBytes = BitConverter.GetBytes(0);
            WriteProcessMemory(explorerProcess, remoteRectBuffer, Marshal.UnsafeAddrOfPinnedArrayElement(indexBytes, 0), 4, out _);

            //Send LVM_GETITEMRECT
            SendMessage(listView, LVM_GETITEMRECT, new IntPtr(0), remoteRectBuffer);

            //Read RECT from remote process
            IntPtr localRect = Marshal.AllocHGlobal(Marshal.SizeOf<RECT>());
            if (ReadProcessMemory(explorerProcess, remoteRectBuffer, localRect, (uint)Marshal.SizeOf<RECT>(), out _))
            {
                RECT rect = Marshal.PtrToStructure<RECT>(localRect);
                res.x = rect.Right - rect.Left;
                res.y = rect.Bottom - rect.Top;
            }
            Marshal.FreeHGlobal(localRect);
            VirtualFreeEx(explorerProcess, remoteRectBuffer, 0, MEM_RELEASE);

            return res;
        }

        public static Point GetDesktopSize()
        {
            return new Point { x = Screen.width, y = Screen.height };
        }

        public static System.Drawing.Point GetDesktopOffset()
        {
            System.Drawing.Point point = new(0, 0);

            string command = "Add-Type -AssemblyName System.Windows.Forms; " +
                             "foreach($s in [System.Windows.Forms.Screen]::AllScreens) " +
                             "{ " +
                             "Write-Output \"$($s.Bounds.X) $($s.Bounds.Y)\"" +
                             "}";
            
            //Create a new process to run the PowerShell command
            var psi = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -Command \"{command}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            //Run the powershell command and save the output
            string output;
            using (var process = Process.Start(psi))
            {
                process.WaitForExit();
                output = process.StandardOutput.ReadToEnd();
                Debug.Log(output);
            }

            //Parsing the output into X and Y coordinates
            string[] parts = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            List<int> xBounds = new();
            List<int> yBounds = new();
            for (int i = 0; i < parts.Length; i++)
            {
                if (i % 2 == 0)
                {
                    xBounds.Add(int.Parse(parts[i]));
                }
                else
                {
                    yBounds.Add(int.Parse(parts[i]));
                }
            }
            
            //Getting the offset required to normalize the coordinates to the main screen
            point.X = Math.Max(0, -xBounds.Min());
            point.Y = Math.Max(0, -yBounds.Min());
            
            return point;
        }

        public static float GetWindowsScaling()
        {
            uint dpi = GetDpiForSystem();
            return dpi / 96.0f;
        }
    }
}