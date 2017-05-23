using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices; //Marshal
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using SeafileOutlookAddIn.AddIns;

namespace SeafileOutlookAddIn
{
    class AddInController
    {
        #region Private Members
        private ThisAddIn _AddIn;
        private Outlook.Inspectors _Inspectors;
        private Dictionary<Guid, MessageInspector> _MessageInspectorDictionary;
        private System.Threading.Timer _OutboxProcessingTimer;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="addin"></param>
        public AddInController(ThisAddIn addin)
        {
            _AddIn = addin;

#if NOTUSED
            _AddIn.Application.ItemLoad += new Outlook.ApplicationEvents_11_ItemLoadEventHandler(Application_ItemLoad);
            _AddIn.Application.ItemSend += new Outlook.ApplicationEvents_11_ItemSendEventHandler(Application_ItemSend);
            //TODO: Maybe we should look at the _AddIn.Application.AttachmentContextMenuDisplay event to add an edit package menu item
#endif

            //Keeping track of inspectors
            _Inspectors = _AddIn.Application.Inspectors;
            _Inspectors.NewInspector += new Outlook.InspectorsEvents_NewInspectorEventHandler(_Inspectors_NewInspector);
            _MessageInspectorDictionary = new Dictionary<Guid, MessageInspector>();

            //Create timer to periodically loop through items in the Outbox and send them
            System.Threading.TimerCallback objTimerCallback = new System.Threading.TimerCallback(this.ProcessOutboxOnTick);
            _OutboxProcessingTimer = new System.Threading.Timer(objTimerCallback, null, Constants.OutboxProcessingTimerDueTime, Constants.OutboxProcessingTimerPeriod);
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~AddInController()
        {
            //Disposing of the timer
            if (_OutboxProcessingTimer != null)
            {
                _OutboxProcessingTimer.Dispose();
                _OutboxProcessingTimer = null;
            }

            //Purging objects used to track inspectors
            _MessageInspectorDictionary.Clear();
            _MessageInspectorDictionary = null;
            try
            {
                //Sometimes, the following line raises an InvalidComObjectException when exiting Outlook
                //TODO: My guess is becuase we have leaking objects.
                //We will need to follow directions at http://blogs.msdn.com/mstehle/archive/2007/12/07/oom-net-part-2-outlook-item-leaks.aspx
                _Inspectors.NewInspector -= new Outlook.InspectorsEvents_NewInspectorEventHandler(_Inspectors_NewInspector);
            }
            catch
            { }
            _Inspectors = null;


#if NOTUSED
            try
            {
                //Sometimes, the following lines raise an InvalidComObjectException when exiting Outlook
                _AddIn.Application.ItemLoad -= new Outlook.ApplicationEvents_11_ItemLoadEventHandler(Application_ItemLoad);
                _AddIn.Application.ItemSend -= new Outlook.ApplicationEvents_11_ItemSendEventHandler(Application_ItemSend);
            }
            catch
            { }
#endif

            _AddIn = null;
        }
        #endregion

        #region Property Accessors
        //
        #endregion

        #region Package Editing
        /// <summary>
        /// Add/Edit Package
        /// </summary>
        /// <param name="inspector"></param>
        internal void AddEditPackage(Outlook.Inspector inspector,string ShareLink)
        {
            MessageInspector objMessageInspector = FindMessageInspector(inspector);
            objMessageInspector.InsertLinksIntoMessageBody(ShareLink);
            //objMessageInspector.AddEditPackage();
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Event handler for the new inspector event
        /// </summary>
        /// <param name="inspector"></param>
        void _Inspectors_NewInspector(Outlook.Inspector inspector)
        {
            try
            {
                OutlookItem objOutlookItem = new OutlookItem(inspector.CurrentItem);

                // Make sure this is a message item
                if ((objOutlookItem.Class == Outlook.OlObjectClass.olMail)
                    && (objOutlookItem.MessageClass.StartsWith(Constants.IPMClass)))
                {
                    // Check to see if this is a new window we don't already track
                    MessageInspector objExistingInspector = this.FindMessageInspector(inspector);
                    // If not found in our dictionary, add it
                    if (objExistingInspector == null)
                    {
                        MessageInspector objMessageInspector = new MessageInspector(inspector);

                        objMessageInspector.OnClose += new EventHandler<CloseEventArgs>(objMessageInspector_OnClose);

                        objMessageInspector.OnInvalidateControl += ObjMessageInspector_OnInvalidateControl;
                          
                        _MessageInspectorDictionary.Add(objMessageInspector.Id, objMessageInspector);

                        //Customize the UI
                        //Office.CommandBars objCommandBars = objMessageInspector.Window.CommandBars;
                    }
                }
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Trace.WriteLine(Ex);
                MessageBox.Show(
                    Ex.Message,
                    Constants.EditorAppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ObjMessageInspector_OnInvalidateControl(object sender, AddIns.InvalidateEventArgs e)
        {
            try
            {
                _AddIn.MessageRibbon.InvalidateControl(e.ControlID);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        /// <summary>
        /// Event handler for the close event of the current inspector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void objMessageInspector_OnClose(object sender, CloseEventArgs e)
        {
            try
            {
                MessageInspector objMessageInspector = (MessageInspector)sender;
                objMessageInspector.OnClose -= new EventHandler<CloseEventArgs>(objMessageInspector_OnClose);

                objMessageInspector.OnInvalidateControl -= ObjMessageInspector_OnInvalidateControl;
                _MessageInspectorDictionary.Remove(objMessageInspector.Id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }
        /// <summary>
        /// Event handler for the invalidate control event of the current inspector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

     
        #endregion

        #region Private Helper Functions
        /// <summary>
        /// Looks up the window wrapper for a given window object
        /// </summary>
        /// <param name="window">A velodoc inspector or explorer window</param>
        /// <returns></returns>
        private MessageInspector FindMessageInspector(object window)
        {
            System.Diagnostics.Debug.Assert((window is Outlook.Inspector) || (window is Outlook.Explorer));
            foreach (MessageInspector objMessageInspector in _MessageInspectorDictionary.Values)
            {
                if (objMessageInspector.Window == window)
                {
                    return objMessageInspector;
                }
            }
            return null;
        }
        /// <summary>
        /// Function periodically called by the timer instantiated here above to send the emails in the outbox once uplaods are successful 
        /// </summary>
        /// <param name="stateInfo"></param>
        private void ProcessOutboxOnTick(object stateInfo)
        {
            Outlook.NameSpace objNS = null;
            Outlook.MAPIFolder objOutboxFolder = null;
            Outlook.Items colItems = null;

            try
            {
                System.Diagnostics.Debug.Assert(stateInfo == null);
                System.Diagnostics.Trace.WriteLine(this.GetType().Name + ": Entering ProcessOutboxOnTick");
                objNS = _AddIn.Application.Session;
                objOutboxFolder = objNS.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderOutbox);
                colItems = (Outlook.Items)objOutboxFolder.Items;
                //colItems.Restrict("[Sent] = False"); //<-- Breaks here without raising an exception

                for (int i = 0; i < colItems.Count; i++)
                {
                    Outlook.MailItem objMailItem = null;
                    try
                    {
                        //IMPORTANT: Index of COM collection starts at 1
                        objMailItem = colItems[i + 1] as Outlook.MailItem;
                        if ((objMailItem != null) && (!objMailItem.Sent))
                        {
                            MessageItem objMessageItem = new MessageItem(objMailItem);
                            string sTransferId = objMessageItem.TransferId;
                            if (!String.IsNullOrEmpty(sTransferId))
                            {

                                //TODO
                                /*
                                Memba.Transfer.PlugIns.ITransferInfo objTransferInfo = Memba.Transfer.Helpers.TransferRegistryHelper.GetTransferInfo(new Guid(sTransferId));
                                if (objTransferInfo.Status == Memba.Transfer.PlugIns.TransferStatus.Completed)
                                {
                                    System.Diagnostics.Trace.WriteLine(this.GetType().Name + "Sending mail item with subject " + objMessageItem.Subject);
                                    objMessageItem.Send();
                                }
                                */
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex);
                    }
                    finally
                    {
                        if (objMailItem != null)
                        {
                            Marshal.ReleaseComObject(objMailItem);
                            objMailItem = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
            finally
            {
                if (colItems != null)
                {
                    Marshal.ReleaseComObject(colItems);
                    colItems = null;
                }
                if (objOutboxFolder != null)
                {
                    Marshal.ReleaseComObject(objOutboxFolder);
                    objOutboxFolder = null;
                }
                if (objNS != null)
                {
                    Marshal.ReleaseComObject(objNS);
                    objNS = null;
                }
            }
        }
        #endregion
    }
}
