using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SeafileOutlookAddIn.Utils
{
    class SecureStringUtils
    {
        public static char[] SecureStringToCharArray(SecureString s)
        {
            IntPtr p = IntPtr.Zero;
            char[] chars = new char[s.Length];

            try
            {
                p = Marshal.SecureStringToBSTR(s);
                Marshal.Copy(p, chars, 0, s.Length);
                return chars;
            }
            finally
            {
                if (p != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(p);
            }

        }
    }
}
