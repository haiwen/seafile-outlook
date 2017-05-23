using System;
using System.Windows.Forms;
using System.Diagnostics;

using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;

namespace SeafileOutlookAddIn.AddIns
{
    /// <summary>
    /// This class tracks the state of an Outlook inspector window for the 
    /// add-in and ensures that Ribbon and item state are handled correctly.
    /// </summary>
    internal class OutlookInspector : OutlookWindow
    {
        #region Private Variables
        private OutlookItem _CurrentItem;
        #endregion

        #region Constructor
        /// <summary>
        /// Create a new instance of the tracking class for an inspector 
        /// </summary>
        /// <param name="inspector">The new inspector window to monitor</param>
        ///<remarks></remarks>
        public OutlookInspector(Outlook.Inspector inspector)
            : base(inspector)
        {
            _CurrentItem = new OutlookItem(inspector.CurrentItem);

            // Hookup events
            ((Outlook.InspectorEvents_10_Event)this.Window).Close +=
                new Outlook.InspectorEvents_10_CloseEventHandler(OutlookInspector_Close);
        }
        #endregion

        #region Property Accessors
        /// <summary>
        /// The actual Outlook inspector window wrapped by this instance
        /// </summary>
        internal Outlook.Inspector Inspector
        {
            get { return (Outlook.Inspector)this.Window; }
        }
        /// <summary>
        /// The CurrentItem for this inspector window.
        /// </summary>
        internal OutlookItem CurrentItem
        {
            get { return _CurrentItem; }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Event Handler for the inspector close event.
        /// </summary>
        private void OutlookInspector_Close()
        {
            ((Outlook.InspectorEvents_10_Event)this.Window).Close -=
                new Outlook.InspectorEvents_10_CloseEventHandler(OutlookInspector_Close);

            _CurrentItem = null;

            // Raise the window close event
            FireOnClose();
        }
        #endregion
    }
}
