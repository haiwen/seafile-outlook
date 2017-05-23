
using System;

namespace SeafileOutlookAddIn.AddIns
{
    /// <summary>
    /// Event args for the close event of an OutlookWindow
    /// </summary>
    internal sealed class CloseEventArgs : EventArgs
    {
        #region Private Members
        private Guid _Id;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">a window id</param>
        public CloseEventArgs(Guid id)
        {
            _Id = id;
        }
        #endregion

        #region Property Accessors
        /// <summary>
        /// The id of the closed window
        /// </summary>
        public Guid Id
        {
            get { return _Id; }
        }
        #endregion
    }
}
