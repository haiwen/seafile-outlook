
using System;
using System.Runtime.InteropServices; //Marshal

using Outlook = Microsoft.Office.Interop.Outlook;

namespace SeafileOutlookAddIn.AddIns
{
    /// <summary>
    /// Represents an inspector or explorer window
    /// </summary>
    internal abstract class OutlookWindow
    {
        #region Private Members
        protected Guid _Id;
        protected object _Window;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window">An Outlook explorer or inspector</param>
        public OutlookWindow(object window)
        {
            if (window == null)
                throw new ArgumentNullException("window");

            if (!((window is Outlook.Inspector) || (window is Outlook.Explorer)))
                throw new InvalidCastException();

            _Id = Guid.NewGuid();
            _Window = window;
        }
        /// <summary>
        /// Destructor (frees resources)
        /// REMOVED: apparently not needed with VSTO
        /// See http://blogs.msdn.com/yvesdolc/archive/2004/04/17/115379.aspx
        /// See http://blogs.msdn.com/eric_carter/archive/2004/10/10/240568.aspx
        /// See http://blogs.msdn.com/geoffda/archive/2007/08/31/the-designer-process-that-would-not-terminate.aspx 
        /// See http://blogs.msdn.com/omars/archive/2004/12/07/276136.aspx
        /// </summary>
        /*
        ~OutlookWindow()
        {
            try
            {
                Marshal.ReleaseComObject(_Window);
            }
            catch
            { }
        }
        */
        #endregion

        #region Property Accessors
        /// <summary>
        /// Gets the window id
        /// </summary>
        public Guid Id
        {
            get { return _Id; }
        }
        /// <summary>
        /// Gets the base inspector or explorer
        /// </summary>
        public object Window
        {
            get { return _Window; }
        }
        #endregion

        #region Events
        /// <summary>
        /// Event raised when closing the window (inspector or explorer)
        /// </summary>
        public event EventHandler<CloseEventArgs> OnClose;
        protected void FireOnClose()
        {
            _Window = null;

            EventHandler<CloseEventArgs> handler = OnClose;
            if (handler != null)
                handler(this, new CloseEventArgs(_Id));
        }
        /// <summary>
        /// Event raised when invalidating the ribbon to make it redrawn
        /// </summary>
        public event EventHandler<InvalidateEventArgs> OnInvalidateControl;
        protected void FireOnInvalidateControl(string controlID)
        {
            EventHandler<InvalidateEventArgs> handler = OnInvalidateControl;
            if (handler != null)
                handler(this, new InvalidateEventArgs(_Id, controlID));
        }
        #endregion
    }
}
