
using System;

using Outlook = Microsoft.Office.Interop.Outlook;

namespace SeafileOutlookAddIn.AddIns
{
    internal sealed class MessageItem : OutlookItem
    {
        #region OutlookItem Constants
        public const string OlTo = "To";
        public const string OlCc = "Cc";
        public const string OlBcc = "Bcc";
        public const string OlRecipients = "Recipients";
        public const string OlSenderName = "SenderName";
        public const string OlSenderEmailAddress = "SenderEmailAddress";
        public const string OlSenderEmailType = "SenderEmailType";
        public const string OlSendUsingAccount = "SendUsingAccount";
        public const string OlBodyFormat = "BodyFormat";
        public const string OlHTMLBody = "HTMLBody";
        public const string OlSend = "Send";
        public const string OlSent = "Sent";
        #endregion

        #region Private Members
        //
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="item"></param>
        public MessageItem(object item)
            : base(item)
        {
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Event handler for the BeforeAttachmentAdd event
        /// </summary>
        /// <param name="attachment"></param>
        /// <param name="cancel"></param>
        //void _MailItem_BeforeAttachmentAdd(Outlook.Attachment attachment, ref bool cancel)
        //{
        //}
        #endregion

        #region Property Accessors
        /// <summary>
        /// Overridden InnerObject
        /// </summary>
        public override object InnerObject
        {
            get
            {
                System.Diagnostics.Debug.Assert(base.InnerObject is Outlook.MailItem);
                return base.InnerObject as Outlook.MailItem;
            }
        }

        public string To
        {
            get
            {
                return (string)this.GetPropertyValue(OlTo);
            }
            set
            {
                this.SetPropertyValue(OlTo, value);
            }
        }
        public string Cc
        {
            get
            {
                return (string)this.GetPropertyValue(OlCc);
            }
            set
            {
                this.SetPropertyValue(OlCc, value);
            }
        }
        public string Bcc
        {
            get
            {
                return (string)this.GetPropertyValue(OlBcc);
            }
            set
            {
                this.SetPropertyValue(OlBcc, value);
            }
        }
        public Outlook.Recipients Recipients
        {
            get
            {
                return this.GetPropertyValue(OlRecipients) as Outlook.Recipients;
            }
        }
        public string SenderEmailAddress
        {
            get
            {
                return (string)this.GetPropertyValue(OlSenderEmailAddress);
            }
            set
            {
                this.SetPropertyValue(OlSenderEmailAddress, value);
            }
        }
        public string SenderName
        {
            get
            {
                return (string)this.GetPropertyValue(OlSenderName);
            }
            set
            {
                this.SetPropertyValue(OlSenderName, value);
            }
        }
        public string SenderEmailType
        {
            get
            {
                return (string)this.GetPropertyValue(OlSenderEmailType);
            }
            set
            {
                this.SetPropertyValue(OlSenderEmailType, value);
            }
        }
#if OK2007
        public Outlook.Account SendUsingAccount
        {
            get
            {
                return this.GetPropertyValue(OlSendUsingAccount) as Outlook.Account;
            }
            set
            {
                this.SetPropertyValue(OlSendUsingAccount, value);
            }
        }
#endif
        public Outlook.OlBodyFormat BodyFormat
        {
            get
            {
                return (Outlook.OlBodyFormat)this.GetPropertyValue(OlBodyFormat);
            }
            set
            {
                this.SetPropertyValue(OlBodyFormat, value);
            }
        }
        public string HTMLBody
        {
            get
            {
                return (string)this.GetPropertyValue(OlHTMLBody);
            }
            set
            {
                this.SetPropertyValue(OlHTMLBody, value);
            }
        }
        public void Send()
        {
            this.CallMethod(OlSend);
        }
        public bool Sent
        {
            get
            {
                return (bool)this.GetPropertyValue(OlSent);
            }
            //set
            //{
            //    this.SetPropertyValue(OlSent, value);
            //}
        }
        /// <summary>
        /// Gets or sets the transfer id for the transfer package
        /// </summary>
        /// <remarks>
        /// We need this property to store the transfer id in a MessageItem
        /// so that when the MessageItem is 
        /// a PackageEditorForm and PackageEditorController
        /// </remarks>
        public string TransferId
        {
            get
            {
                Outlook.UserProperty objUserProperty = this.UserProperties.Find(Constants.TransferIdUserProp, true);
                if (objUserProperty == null)
                    return null;
                else
                    return (string)this.UserProperties[Constants.TransferIdUserProp].Value;
            }
            set
            {
                Outlook.UserProperty objUserProperty = this.UserProperties.Find(Constants.TransferIdUserProp, true);
                if (objUserProperty == null)
                    objUserProperty = this.UserProperties.Add(
                        Constants.TransferIdUserProp,
                        Outlook.OlUserPropertyType.olText,
                        false,
                        1); //equivalent to Outlook.OlFormatText.olFormatTextText in Outlook 2007
                objUserProperty.Value = value;
                System.Diagnostics.Trace.WriteLine(this.GetType().Name + ": Transfer id set to " + value);
            }
        }
        /// <summary>
        /// Gets or sets the temp directory for the transfer package
        /// </summary>
        /// <remarks>
        /// We need this property to store the TempDirectory used for a MessageItem
        /// especially because we may send a draft item without re-opening
        /// a PackageEditorForm and PackageEditorController
        /// </remarks>
        public string TempDirectory
        {
            get
            {
                Outlook.UserProperty objUserProperty = this.UserProperties.Find(Constants.TempDirUserProp, true);
                if (objUserProperty == null)
                    return null;
                else
                    return (string)this.UserProperties[Constants.TempDirUserProp].Value;
            }
            set
            {
                Outlook.UserProperty objUserProperty = this.UserProperties.Find(Constants.TempDirUserProp, true);
                if(objUserProperty == null)
                    objUserProperty = this.UserProperties.Add(
                        Constants.TempDirUserProp,
                        Outlook.OlUserPropertyType.olText,
                        false,
                        1); //equivalent to Outlook.OlFormatText.olFormatTextText in Outlook 2007
                objUserProperty.Value = value;
                System.Diagnostics.Trace.WriteLine(this.GetType().Name + ": Temp directory set to " + value);
            }
        }
        #endregion
    }
}
