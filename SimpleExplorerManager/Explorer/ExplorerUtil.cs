using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Peter;

namespace SimpleExplorerManager.Explorer
{

    public class ExplorerUtil
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static List<ExplorerComData> GetExplorerWindows()
        {
            List<ExplorerComData> list = new List<ExplorerComData>();
            try
            {
                Type instanceType = Type.GetTypeFromProgID("Shell.Application");
                dynamic shellApplicaition = Activator.CreateInstance(instanceType);
                dynamic windows = shellApplicaition.Windows;
                foreach (dynamic window in windows)
                {
                    ExplorerComData data = new ExplorerComData();
                    data.LocationURL = window.LocationURL; 
                    data.LocationName = window.LocationName;
                    data.Top = window.Top;
                    data.Left = window.Left;
                    data.Width = window.Width;
                    data.Height = window.Height;
                    data.HWND = window.HWND;
                    data.Kind = "🔘No Group";
                    list.Add(data);
                    Marshal.ReleaseComObject(window);
                }
                foreach(ExplorerComData data in list)
                {
                    data.sibilingList = list;
                }
                Marshal.ReleaseComObject(windows);
                Marshal.ReleaseComObject(shellApplicaition);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return list;
        }

        public static void SetExplorerForegroud(long HWND)
        {

            try { 
                ShowWindow(new IntPtr(HWND), 1);
                SetForegroundWindow(new IntPtr(HWND));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void ShowExplorerWindow(string path)
        {
            try
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                };
                p.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void ShowExplorerContextMenu(string path)
        {
            ShellContextMenu menu = new ShellContextMenu();
            FileInfo[] arrFI = new FileInfo[1];
            arrFI[0] = new FileInfo(path);
            menu.ShowContextMenu(arrFI, Control.MousePosition);
        }
    }
}
