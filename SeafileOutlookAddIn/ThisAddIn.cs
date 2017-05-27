using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using log4net.Config;
using SeafileOutlookAddIn.Utils;
using System.Threading;
using System.Globalization;

namespace SeafileOutlookAddIn
{
    public partial class ThisAddIn
    {
        private static readonly log4net.ILog log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int LCID { set; get; }


        private MessageRibbon _MessageRibbon;
        
        private AddInController _AddInController;

        internal MessageRibbon MessageRibbon
        {
            get { return _MessageRibbon; }
        }

        internal AddInController AddInController
        {
            get { return _AddInController; }
        }

        protected override object RequestService(Guid serviceGuid)
        {
           if (serviceGuid == typeof(Office.IRibbonExtensibility).GUID)
            {
                if (_MessageRibbon == null)
                {
                    _MessageRibbon = new MessageRibbon();
                    _MessageRibbon.LCID = this.LCID;
                }
                return _MessageRibbon;
            }

            return base.RequestService(serviceGuid);
        }


        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            Logger.ConfigureFileAppender(System.Windows.Forms.Application.UserAppDataPath + "\\seafile-outlook.log");
            log.Info("ThisAddIn_Startup");

            //ThreadLocalizer.Localize(this.Application);

            //Create controller which manages the creation of new inspectors
            _AddInController = new AddInController(this);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see http://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        protected override Office.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            Outlook.Application app = this.GetHostItem<Outlook.Application>(typeof(Outlook.Application), "Application");
            this.LCID = app.LanguageSettings.get_LanguageID(Office.MsoAppLanguageID.msoLanguageIDUI);
           // CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(lcid);
           ;
            return base.CreateRibbonExtensibilityObject();

        }
        #endregion
    }
}
