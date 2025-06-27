using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

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

        public struct DesktopIcon
        {
            public System.Drawing.Point Position { get; set; }
            public string Name { get; set; }
            
            public override string ToString()
            {
                return $"{Name} ({Position.X}, {Position.Y})";
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
        const uint LVM_GETITEMRECT = 0x100E;
        const uint LVM_GETITEMSPACING = 0x1035;
        
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
            int spacingX = spacingValue & 0xFFFF;
            int spacingY = (spacingValue >> 16) & 0xFFFF;
            
            return new Point { x = spacingX, y = spacingY };
        }

        /*Assumes the icon count is at least 1
        private static Point GetIconSize(IntPtr explorerProcess, IntPtr listView)
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
            
        }*/

        private static Point GetDesktopSize(IntPtr listView)
        {
            GetClientRect(listView, out RECT rect);

            //The left and top members are zero. The right and bottom members contain the width and height of the window
            return new Point { x = rect.Right, y = rect.Bottom };
        }

        public static Point GetDesktopOffset()
        {
            Point point = new() { x = 0, y = 0 };
            
            string appPath = Path.Combine(Application.streamingAssetsPath, "ScreenHelper.exe");
            string textPath = Path.Combine(Application.persistentDataPath, "offset.txt");
            using (Process pr = new Process())
            {
                pr.StartInfo.FileName = appPath;
                pr.StartInfo.Arguments = textPath;
                pr.StartInfo.UseShellExecute = false;
                pr.StartInfo.CreateNoWindow = true;
                pr.StartInfo.RedirectStandardOutput = true;
                pr.StartInfo.RedirectStandardError = true;

                try
                {
                    pr.Start();
                    pr.WaitForExit();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Failed to start ScreenHelper.exe: {e.Message}");
                    return point;
                }
            }
            
            string offset = File.ReadAllText(textPath);
            string[] parts = offset.Split(' ');
            point.x = int.Parse(parts[0]);
            point.y = int.Parse(parts[1]);
            
            File.Delete(textPath);
            
            return point;
        }
    }
}