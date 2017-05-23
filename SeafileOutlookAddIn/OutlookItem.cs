
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;

using Outlook = Microsoft.Office.Interop.Outlook;

#region Readme and Usage
/// <summary>
/// Helper class to access common properties of Outlook Items.  This class uses
/// reflection to call the common members of all Outlook items.  Because of the use 
/// of reflection, all calls through this class will be significantly slower than
/// executing the same call against a strongly typed object.
/// </summary>
/// <remarks>
/// Uses the IDispatch interface of the Outlook object model 
/// to access the common properties of
/// Outlook items regardless of the actual item type.  
/// This prevents having to test for and cast to 
/// a specific object in order to access common properties.
///
/// Usage example:
/// 
///    OutlookItem myItem = new OutlookItem(olApp.ActiveExplorer.Selection[1]);
///    Debug.WriteLine(myItem.EntryID);
#endregion

namespace SeafileOutlookAddIn.AddIns
{
    //Note: This class is inspired from the following Microsoft sample
    //http://msdn2.microsoft.com/en-us/library/bb226712.aspx

    /// <summary>
    /// OutlookItem represents an Outlook item (message, contact, note, ...)
    /// </summary>
    internal class OutlookItem
    {
        #region OutlookItem Constants
        private const string OlActions = "Actions";
        private const string OlApplication = "Application";
        private const string OlAttachments = "Attachments";
        private const string OlBillingInformation = "BillingInformation";
        private const string OlBody = "Body";
        private const string OlCategories = "Categories";
        private const string OlClass = "Class";
        private const string OlClose = "Close";
        private const string OlCompanies = "Companies";
        private const string OlConversationIndex = "ConversationIndex";
        private const string OlConversationTopic = "ConversationTopic";
        private const string OlCopy = "Copy";
        private const string OlCreationTime = "CreationTime";
        private const string OlDisplay = "Display";
        private const string OlDownloadState = "DownloadState";
        private const string OlEntryID = "EntryID";
        private const string OlFormDescription = "FormDescription";
        private const string OlGetInspector = "GetInspector";
        private const string OlImportance = "Importance";
        private const string OlIsConflict = "IsConflict";
        private const string OlItemProperties = "ItemProperties";
        private const string OlLastModificationTime = "LastModificationTime";
        private const string OlLinks = "Links";
        private const string OlMarkForDownload = "MarkForDownload";
        private const string OlMessageClass = "MessageClass";
        private const string OlMileage = "Mileage";
        private const string OlMove = "Move";
        private const string OlNoAging = "NoAging";
        private const string OlOutlookInternalVersion = "OutlookInternalVersion";
        private const string OlOutlookVersion = "OutlookVersion";
        private const string OlParent = "Parent";
        private const string OlPrintOut = "PrintOut";
        private const string OlPropertyAccessor = "PropertyAccessor";
        private const string OlSave = "Save";
        private const string OlSaveAs = "SaveAs";
        private const string OlSaved = "Saved";
        private const string OlSensitivity = "Sensitivity";
        private const string OlSession = "Session";
        private const string OlShowCategoriesDialog = "ShowCategoriesDialog";
        private const string OlSize = "Size";
        private const string OlSubject = "Subject";
        private const string OlUnRead = "UnRead";
        private const string OlUserProperties = "UserProperties";
        #endregion

        #region Private Members
        private object _Item;  // the wrapped Outlook item
        private Type _Type;  // type for the Outlook item 
        private object[] _Args;  // dummy argument array
        private Type _TypeOlObjectClass;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        public OutlookItem(object item)
        {
            _Item = item;
            _Type = _Item.GetType();
            _Args = new Object[] { };
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
        ~OutlookItem()
        {
            try
            {
                Marshal.ReleaseComObject(_Item);
            }
            catch
            { }
        }
        */
        #endregion

        #region Public Methods and Properties
        public Outlook.Actions Actions
        {
            get
            {
                return this.GetPropertyValue(OlActions) as Outlook.Actions;
            }
        }

        public Outlook.Application Application
        {
            get
            {
                return this.GetPropertyValue(OlApplication) as Outlook.Application;
            }
        }

        public Outlook.Attachments Attachments
        {
            get
            {
                return this.GetPropertyValue(OlAttachments) as Outlook.Attachments;
            }
        }

        public string BillingInformation
        {
            get
            {
                return (string)this.GetPropertyValue(OlBillingInformation); //.ToString();
            }
            set
            {
                this.SetPropertyValue(OlBillingInformation, value);
            }
        }

        public string Body
        {
            get
            {
                return (string)this.GetPropertyValue(OlBody); //.ToString();
            }
            set
            {
                this.SetPropertyValue(OlBody, value);
            }
        }

        public string Categories
        {
            get
            {
                return (string)this.GetPropertyValue(OlCategories); //.ToString();
            }
            set
            {
                this.SetPropertyValue(OlCategories, value);
            }
        }


        public void Close(Outlook.OlInspectorClose SaveMode)
        {
            object[] MyArgs = { SaveMode };
            this.CallMethod(OlClose);
        }

        public string Companies
        {
            get
            {
                return (string)this.GetPropertyValue(OlCompanies); //.ToString();
            }
            set
            {
                this.SetPropertyValue(OlCompanies, value);
            }
        }

        public Outlook.OlObjectClass Class
        {
            get
            {
                if (_TypeOlObjectClass == null)
                {
                    // Note: instantiate dummy ObjectClass enumeration to get type.
                    //       type = System.Type.GetType("Outlook.OlObjectClass") doesn't seem to work
                    Outlook.OlObjectClass objClass = Outlook.OlObjectClass.olAction;
                    _TypeOlObjectClass = objClass.GetType();
                }
                return (Outlook.OlObjectClass)System.Enum.ToObject(_TypeOlObjectClass, this.GetPropertyValue(OlClass));
            }
        }

        public string ConversationIndex
        {
            get
            {
                return (string)this.GetPropertyValue(OlConversationIndex); //.ToString();
            }
        }

        public string ConversationTopic
        {
            get
            {
                return (string)this.GetPropertyValue(OlConversationTopic); //.ToString();
            }
        }

        public object Copy()
        {
            return (this.CallMethod(OlCopy));
        }

        public System.DateTime CreationTime
        {
            get
            {
                return (System.DateTime)this.GetPropertyValue(OlCreationTime);
            }
        }

        public void Display()
        {
            this.CallMethod(OlDisplay);
        }

        public Outlook.OlDownloadState DownloadState
        {
            get
            {
                return (Outlook.OlDownloadState)this.GetPropertyValue(OlDownloadState);
            }
        }

        public string EntryID
        {
            get
            {
                return (string)this.GetPropertyValue(OlEntryID); //.ToString();
            }
        }

        public Outlook.FormDescription FormDescription
        {
            get
            {
                return this.GetPropertyValue(OlFormDescription) as Outlook.FormDescription;
            }
        }

        public virtual Object InnerObject
        {
            get
            {
                return this._Item;
            }
        }

        public Outlook.Inspector GetInspector
        {
            get
            {
                return this.GetPropertyValue(OlGetInspector) as Outlook.Inspector;
            }
        }

        public Outlook.OlImportance Importance
        {
            get
            {
                return (Outlook.OlImportance)this.GetPropertyValue(OlImportance);
            }
            set
            {
                this.SetPropertyValue(OlImportance, value);
            }
        }

        public bool IsConflict
        {
            get
            {
                return (bool)this.GetPropertyValue(OlIsConflict);
            }
        }

        public Outlook.ItemProperties ItemProperties
        {
            get
            {
                return this.GetPropertyValue(OlItemProperties) as Outlook.ItemProperties;
            }
        }

        public System.DateTime LastModificationTime
        {
            get
            {
                return (System.DateTime)this.GetPropertyValue(OlLastModificationTime);
            }
        }

        public Outlook.Links Links
        {
            get
            {
                return this.GetPropertyValue(OlLinks) as Outlook.Links;
            }
        }

        public Outlook.OlRemoteStatus MarkForDownload
        {
            get
            {
                return (Outlook.OlRemoteStatus)this.GetPropertyValue(OlMarkForDownload);
            }
            set
            {
                this.SetPropertyValue(OlMarkForDownload, value);
            }
        }

        public string MessageClass
        {
            get
            {
                return (string)this.GetPropertyValue(OlMessageClass); //.ToString();
            }
            set
            {
                this.SetPropertyValue(OlMessageClass, value);
            }
        }

        public string Mileage
        {
            get
            {
                return (string)this.GetPropertyValue(OlMileage); //.ToString();
            }
            set
            {
                this.SetPropertyValue(OlMileage, value);
            }
        }

        //public object Move(Outlook.Folder DestinationFolder) Changed by JLC
        public object Move(Outlook.MAPIFolder DestinationFolder)
        {
            object[] myArgs = { DestinationFolder };
            return this.CallMethod(OlMove, myArgs);
        }

        public bool NoAging
        {
            get
            {
                return (bool)this.GetPropertyValue(OlNoAging);
            }
            set
            {
                this.SetPropertyValue(OlNoAging, value);
            }
        }

        public long OutlookInternalVersion
        {
            get
            {
                return (long)this.GetPropertyValue(OlOutlookInternalVersion);
            }
        }

        public string OutlookVersion
        {
            get
            {
                return (string)this.GetPropertyValue(OlOutlookVersion); //.ToString();
            }
        }
#if OK2007
        public Outlook.Folder Parent
        {
            get
            {
                return this.GetPropertyValue(OlParent) as Outlook.Folder;
            }
        }

        public Outlook.PropertyAccessor PropertyAccessor
        {
            get
            {
                return this.GetPropertyValue(OlPropertyAccessor) as Outlook.PropertyAccessor;
            }
        }
#endif
        public void PrintOut()
        {
            this.CallMethod(OlPrintOut);
        }

        public void Save()
        {
            this.CallMethod(OlSave);
        }

        public void SaveAs(string path, Outlook.OlSaveAsType type)
        {
            object[] myArgs = { path, type };
            this.CallMethod(OlSaveAs, myArgs);
        }

        public bool Saved
        {
            get
            {
                return (bool)this.GetPropertyValue(OlSaved);
            }
        }

        public Outlook.OlSensitivity Sensitivity
        {
            get
            {
                return (Outlook.OlSensitivity)this.GetPropertyValue(OlSensitivity);
            }
            set
            {
                this.SetPropertyValue(OlSensitivity, value);
            }
        }

        public Outlook.NameSpace Session
        {
            get
            {
                return this.GetPropertyValue(OlSession) as Outlook.NameSpace;
            }
        }

        public void ShowCategoriesDialog()
        {
            this.CallMethod(OlShowCategoriesDialog);
        }

        public long Size
        {
            get
            {
                return (long)this.GetPropertyValue(OlSize);
            }
        }

        public string Subject
        {
            get
            {
                return (string)this.GetPropertyValue(OlSubject); //.ToString();
            }
            set
            {
                this.SetPropertyValue(OlSubject, value);
            }
        }

        public bool UnRead
        {
            get
            {
                return (bool)this.GetPropertyValue(OlUnRead);
            }
            set
            {
                this.SetPropertyValue(OlUnRead, value);
            }
        }

        public Outlook.UserProperties UserProperties
        {
            get
            {
                return this.GetPropertyValue(OlUserProperties) as Outlook.UserProperties;
            }
        }

        #endregion

        #region Protected Helper Functions
        protected object GetPropertyValue(string propertyName)
        {
            try
            {
                // An invalid property name exception is propagated to client
                return _Type.InvokeMember(
                    propertyName,
                    BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty,
                    null,
                    _Item,
                    _Args);
            }
            catch (SystemException ex)
            {
                Trace.WriteLine(
                    string.Format(
                    "OutlookItem: GetPropertyValue for {0} raised exception: {1} ",
                    propertyName, ex.Message));
                throw;
            }
        }

        protected void SetPropertyValue(string propertyName, object propertyValue)
        {
            try
            {
                _Type.InvokeMember(
                    propertyName,
                    BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty,
                    null,
                    _Item,
                    new object[] { propertyValue });
            }
            catch (SystemException ex)
            {
                Trace.WriteLine(
                   string.Format(
                   "OutlookItem: SetPropertyValue for {0} raised exception: {1} ",
                   propertyName, ex.Message));
                throw;
            }
        }

        protected object CallMethod(string methodName)
        {
            try
            {
                // An invalid property name exception is propagated to client
                return _Type.InvokeMember(
                    methodName,
                    BindingFlags.Public | BindingFlags.InvokeMethod,
                    null,
                    _Item,
                    _Args);
            }
            catch (SystemException ex)
            {
                Trace.WriteLine(
                    string.Format(
                    "OutlookItem: CallMethod for {0} raised exception: {1} ",
                    methodName, ex.Message));
                throw;
            }
        }

        protected object CallMethod(string methodName, object[] args)
        {
            try
            {
                // An invalid property name exception is propagated to client
                return _Type.InvokeMember(
                    methodName,
                    BindingFlags.Public | BindingFlags.InvokeMethod,
                    null,
                    _Item,
                    args);
            }
            catch (SystemException ex)
            {
                Debug.WriteLine(
                    string.Format(
                    "OutlookItem: CallMethod for {0} raised exception: {1} ",
                    methodName, ex.Message));
                throw;
            }
        }
        #endregion

    }
}