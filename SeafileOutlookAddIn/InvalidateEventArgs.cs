
using System;

namespace SeafileOutlookAddIn.AddIns
{
    /// <summary>
    /// Event args for the invalidate event
    /// </summary>
    internal sealed class InvalidateEventArgs : EventArgs
    {
        #region Private Members
        private Guid _Id;
        private string _ControlID;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="controlID"></param>
        public InvalidateEventArgs(Guid id, string controlID)
        {
            _Id = id;
            _ControlID = controlID;
        }
        #endregion

        #region Property Accessor
        /// <summary>
        /// Id of the window raising the event
        /// </summary>
        public Guid Id
        {
            get { return _Id; }
        }
        /// <summary>
        /// TODO
        /// </summary>
        public string ControlID
        {
            get { return _ControlID; }
        }
        #endregion
    }
}
