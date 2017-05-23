using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SeafileOutlookAddIn.Utils
{
    public class Win32API
    {
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("User32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

    }
}
