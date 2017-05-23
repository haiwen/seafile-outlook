using System;
using System.Collections.Generic;
using System.Text;
using System.IO; //Path, Directory, FIleInfo
using System.Windows.Forms; //MessageBox
using System.Drawing; //Bitmap
using System.Drawing.Imaging; //ImageFormat
using System.Text.RegularExpressions;

using Outlook = Microsoft.Office.Interop.Outlook;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;

namespace SeafileOutlookAddIn.AddIns
{
    internal sealed class MessageInspector : OutlookInspector
    {
        private static char[] CRLFTRIM = new char[] { '\r', '\n', ' ' };
        private const string CRLF = "\r\n";
        private const string EMBEDDED_IMAGE_PREFIX = "--#";

        #region Private Members
        private MessageItem _CurrentItem;

        //private PackageEditorController _PackageEditorController = null;
        //private PackageEditorForm _PackageEditorForm = null;

        //The purpose of _LastDialogResult and _DialogResult is to avoid asking the same question
        //again and again when several files are added as attachments at the same time.
        //We have found no easy way to subclass MessageBox in order to add a checkbox "Apply to all items"
        //and we did not want recreating a specific messageBox class at this stage of development
        private DateTime _LastDialogResult;


        //System.Windows.Forms.Timer used to close the inspector upon sending
     
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inspector"></param>
        public MessageInspector(Outlook.Inspector inspector)
            : base(inspector)
        {
            System.Diagnostics.Trace.WriteLine(this.GetType().Name + ": Entering constructor");
            _CurrentItem = new MessageItem(inspector.CurrentItem);
            if ((_CurrentItem != null) && (_CurrentItem.InnerObject != null))
            {

            }
            _LastDialogResult = DateTime.MinValue;

        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~MessageInspector()
        {
            System.Diagnostics.Trace.WriteLine(this.GetType().Name + ": Entering destructor");
          
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Event handler for save event of a package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
    

        public void InsertLinksIntoMessageBody(string insString)
        {
            System.Diagnostics.Debug.WriteLine(new String('-', 80));
            System.Diagnostics.Debug.WriteLine(_CurrentItem.Body);
            System.Diagnostics.Debug.WriteLine(new String('-', 80));
            System.Diagnostics.Debug.WriteLine(_CurrentItem.HTMLBody);
            System.Diagnostics.Debug.WriteLine(new String('-', 80));

            switch (this.Inspector.EditorType)
            {
                case Outlook.OlEditorType.olEditorHTML:
                    System.Diagnostics.Debug.Assert(_CurrentItem.BodyFormat == Outlook.OlBodyFormat.olFormatHTML);
                    //InsertLinksIntoHtmlBody(transferPackage);
                    break;
                case Outlook.OlEditorType.olEditorRTF:
                    System.Diagnostics.Debug.Assert(_CurrentItem.BodyFormat == Outlook.OlBodyFormat.olFormatRichText);
                    //InsertLinksIntoRtfBody(transferPackage);
                    break;
                case Outlook.OlEditorType.olEditorText:
                    System.Diagnostics.Debug.Assert(_CurrentItem.BodyFormat == Outlook.OlBodyFormat.olFormatPlain);
                    InsertLinksIntoTextBody(insString);
                    break;
                case Outlook.OlEditorType.olEditorWord:
                default: //Note that Outlook 2007 will always default to this option
                         /* System.Diagnostics.Debug.Assert(this.Inspector.EditorType == Outlook.OlEditorType.olEditorWord);
                          if (_CurrentItem.BodyFormat == Outlook.OlBodyFormat.olFormatPlain)
                              InsertLinksIntoTextBody(transferPackage); //This is because we cannot add images to a text-only message
                          else if ((_CurrentItem.BodyFormat == Outlook.OlBodyFormat.olFormatRichText)
                              && (_CurrentItem.Application.Version.StartsWith("11")))
                              InsertLinksIntoRtfBody(transferPackage); //This is because we cannot save the changes into the email when using Outlook 2003
                          else
                              InsertLinksIntoWordBody(transferPackage);
                          */
                        InsertLinksIntoWordBody(insString);
                    break;
            }
        }

        /// <summary>
        /// Insert download links into an html message body
        /// </summary>
        /// <param name="transferPackage"></param>
        /// <remarks>This is only used by Outlook 2003</remarks>
  
        /// Insert download links into a text message body
        /// </summary>
        /// <param name="transferPackage"></param>
        private void InsertLinksIntoTextBody(string insString)
        {
            System.Diagnostics.Debug.Assert(_CurrentItem.Application.Version.StartsWith("11"));


        }
        /// <summary>
        /// Insert download links into a Word message body
        /// </summary>
        /// <param name="transferPackage"></param>
        private void InsertLinksIntoWordBody(string insString)
        {
            //See: http://msdn2.microsoft.com/en-us/library/bb386277.aspx (text)
            //See: http://msdn2.microsoft.com/en-us/library/bb157878.aspx (tables)
            //and for an introduction see also...
            //See: http://msdn2.microsoft.com/en-us/library/aa201330(office.11).aspx
            //See: http://msdn2.microsoft.com/en-us/library/aa201332(office.11).aspx
            //See: http://msdn2.microsoft.com/en-us/library/bb407305(VS.80).aspx
            //See: http://support.microsoft.com/kb/316384

            object objMissing = System.Reflection.Missing.Value;
            object objTrue = true;
            object objFalse = false;
            object objStartOfDoc = "\\startofdoc"; /* \startofdoc is a predefined bookmark */
            object objEndOfDoc = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
            //object objParagraphUnit = Word.WdUnits.wdParagraph;
            //object objCountOne = 1;
            object objStart = 0;
            object objEnd = 0;

            System.Diagnostics.Debug.Assert(this.Inspector.EditorType == Outlook.OlEditorType.olEditorWord);

            Word.Document objDocument = this.Inspector.WordEditor as Word.Document;
            if (objDocument == null)
                return; //Not much we can do in this case

#if !DEBUG
            //Improve performances
            objDocument.Application.Options.Pagination = false;
            objDocument.Application.ScreenUpdating = false;
#endif


            #region Insert advertisement at the beginning of the document.
            //Add a paragraph before creating the table
            Word.Range objTopOfMessageRange = objDocument.Bookmarks.get_Item(ref objStartOfDoc).Range;
            System.Diagnostics.Debug.Assert(objTopOfMessageRange.Text == null);

            objTopOfMessageRange.InsertBefore(insString);

            #endregion



        }
 
        #endregion
    }
}
