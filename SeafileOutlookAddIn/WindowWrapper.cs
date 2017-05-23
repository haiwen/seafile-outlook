
using System;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using Word = Microsoft.Office.Interop.Word;
using System.Runtime.InteropServices; //DllImport
using System.Security.Permissions; //SecurityPermission, SecurityAction

namespace SeafileOutlookAddIn.AddIns
{
    /// <summary>
    /// This class wraps an Outlook window in an IWin32Object: see also Memba.Transfer.UI.WindowWrapper
    /// This window wrapper is used with TransferAgent.ShowSettings to display
    /// the window modally with respect to the Outlook window defined by the handle
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
    internal sealed class WindowWrapper : IWin32Window
    {
        [DllImport("user32", CharSet=CharSet.Auto), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        #region Private Members
        private IntPtr _hwnd;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="windowObject"></param>
        public WindowWrapper(object windowObject)
        {
            //The correct class names for FindWindow are given by Ken Slovak at
            //http://www.devnewsgroups.net/group/microsoft.public.office.developer.outlook.vba/topic64945.aspx

            System.Diagnostics.Trace.WriteLine(this.GetType().Name + ": Entering constructor");
            string sCaption = null;
            if (windowObject is Outlook.Explorer)
            {
                sCaption = ((Outlook.Explorer)windowObject).Caption;
                System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(sCaption));
                _hwnd = FindWindow("rctrl_renwnd32\0", sCaption);
            }
            else if (windowObject is Outlook.Inspector)
            {
                sCaption = ((Outlook.Inspector)windowObject).Caption;
                System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(sCaption));
                _hwnd = FindWindow("rctrl_renwnd32\0", sCaption);
            }
            else if (windowObject is Word.Window) //This is for Outlook 2003 + WordMail
            {
                sCaption = ((Word.Window)windowObject).Caption;
                System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(sCaption));
                _hwnd = FindWindow("OpusApp\0", sCaption);
            }
            else
            {
                _hwnd = IntPtr.Zero;
            }
            System.Diagnostics.Trace.WriteLine(this.GetType().Name + ": Window handle is " + _hwnd.ToString());
        }
        #endregion

        #region Property Accessors
        /// <summary>
        /// Gets the window handle
        /// </summary>
        public IntPtr Handle
        {
            get { return _hwnd; }
        }
        #endregion
    }
}
