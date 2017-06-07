using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;
using SeafileOutlookAddIn.AddIns;

// TODO:   按照以下步骤启用功能区(XML)项: 

// 1. 将以下代码块复制到 ThisAddin、ThisWorkbook 或 ThisDocument 类中。

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new MessageRibbon();
//  }

// 2. 在此类的“功能区回调”区域中创建回调方法，以处理用户
//    操作(如单击某个按钮)。注意: 如果已经从功能区设计器中导出此功能区，
//    则将事件处理程序中的代码移动到回调方法并修改该代码以用于
//    功能区扩展性(RibbonX)编程模型。

// 3. 向功能区 XML 文件中的控制标记分配特性，以标识代码中的相应回调方法。  

// 有关详细信息，请参见 Visual Studio Tools for Office 帮助中的功能区 XML 文档。


namespace SeafileOutlookAddIn
{
    [ComVisible(true)]
    public class MessageRibbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;
        public int LCID { set; get; }

        public MessageRibbon()
        {
        }

        #region IRibbonExtensibility 成员

        public string GetCustomUI(string ribbonID)
        {
            //if ((ribbonID == "Microsoft.Outlook.Mail.Compose")
            //    || (ribbonID == "Microsoft.Outlook.Mail.Read"))
            if ((ribbonID == "Microsoft.Outlook.Mail.Compose"))
                return GetResourceText("SeafileOutlookAddIn.MessageRibbon.xml");

            //return Properties.Resources.MessageRibbon; //<-- Consider adding as resource
            else
                return String.Empty;
        }
        #endregion

        #region 功能区回调
        //在此创建回调方法。有关添加回调方法的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #endregion

        #region 帮助器

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }
        internal void InvalidateControl(string controlID)
        {
            if (ribbon != null)
            {
                ribbon.InvalidateControl(controlID);
            }
        }
        public stdole.IPictureDisp GetImage(Office.IRibbonControl control)
        {
            //About handling images in the Ribbon, please check:
            //http://blogs.msdn.com/jensenh/archive/2006/11/27/ribbonx-image-faq.aspx
            //http://blogs.msdn.com/andreww/archive/2007/10/10/preserving-the-alpha-channel-when-converting-images.aspx

            //Icon objIcon = null;
            Image objIcon = null;
            switch (control.Id)
            {
                case "uploadFile":

                    objIcon = Properties.Resources.uploadpng;
                    break;
                case "addLink":

                    objIcon = Properties.Resources.sharepng;
                    break;
                case "setting":

                    objIcon = Properties.Resources.settingpng;
                    break;
                case "about":
                    objIcon = Properties.Resources.aboutpng;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    objIcon = Properties.Resources.uploadpng;
                    break;
            }

            //objIcon == null is OK, means no icon
            return SeafileOutlookAddIn.AddIns.ImageConverter.Convert(objIcon);
        }


        public string GetLabel(Office.IRibbonControl control)
        {
            switch (control.Id)
            {
                case "uploadFile":

                    return Properties.Resources.GetBtnText("UploadBtnText", this.LCID);
                    //return Properties.Resources.UploadBtnText + "\r\n";

                case "addLink":

                    return Properties.Resources.GetBtnText("ShareBtnText", this.LCID);
                    //return Properties.Resources.ShareBtnText + "\r\n";

                case "setting":
                    return Properties.Resources.GetBtnText("SettingBtnText", this.LCID);
                    //return Properties.Resources.SettingBtnText + "\r\n";
                case "about":
                    return Properties.Resources.GetBtnText("AboutBtnText", this.LCID);
                    //return Properties.Resources.AboutBtnText + "\r\n";
                default:
                    return "";
            }
        }
        #endregion
        public void UploadFile_OnAction(Office.IRibbonControl control)
        {
            try
            {
                System.Diagnostics.Trace.WriteLine(this.GetType().Name + ": UploadButton clicked");
                WindowWrapper objActiveWindow = new WindowWrapper(Globals.ThisAddIn.Application.ActiveWindow());
                //We would have preferred a consistent design through remoting, but
                //the about dialog is so simple that there is no reason to open it through remoting
                //considering we already have the two modes respectively for settings and options
                UI.UploadForm frmUploadForm = new UI.UploadForm(this.LCID);
                frmUploadForm.ShowDialog(objActiveWindow);
                if (frmUploadForm.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    Outlook.Inspector objInspector = (Outlook.Inspector)control.Context;
                    Globals.ThisAddIn.AddInController.AddEditPackage(objInspector, frmUploadForm.ShareLink);

                }
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Trace.WriteLine(Ex);
                System.Windows.Forms.MessageBox.Show(
                    Ex.Message,
                    Constants.EditorAppName,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public void AddLink_OnAction(Office.IRibbonControl control)
        {
            try
            {
                System.Diagnostics.Trace.WriteLine(this.GetType().Name + ": UploadButton clicked");
                WindowWrapper objActiveWindow = new WindowWrapper(Globals.ThisAddIn.Application.ActiveWindow());
                //We would have preferred a consistent design through remoting, but
                //the about dialog is so simple that there is no reason to open it through remoting
                //considering we already have the two modes respectively for settings and options
                UI.SeafileForm frmSeafileForm = new UI.SeafileForm(this.LCID);
                frmSeafileForm.ShowDialog(objActiveWindow);
                if (frmSeafileForm.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    Outlook.Inspector objInspector = (Outlook.Inspector)control.Context;
                    Globals.ThisAddIn.AddInController.AddEditPackage(objInspector, frmSeafileForm.ShareLink);

                }
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Trace.WriteLine(Ex);
                System.Windows.Forms.MessageBox.Show(
                    Ex.Message,
                    Constants.EditorAppName,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public void Setting_OnAction(Office.IRibbonControl control)
        {
            try
            {
                System.Diagnostics.Trace.WriteLine(this.GetType().Name + ": AboutButton clicked");
                WindowWrapper objActiveWindow = new WindowWrapper(Globals.ThisAddIn.Application.ActiveWindow());
                //We would have preferred a consistent design through remoting, but
                //the about dialog is so simple that there is no reason to open it through remoting
                //considering we already have the two modes respectively for settings and options
                UI.SettingForm frmSetting = new UI.SettingForm(this.LCID);
                frmSetting.ShowDialog(objActiveWindow);
            }
            catch (Exception Ex)
            {
                System.Diagnostics.Trace.WriteLine(Ex);
                System.Windows.Forms.MessageBox.Show(
                    Ex.Message,
                    Constants.EditorAppName,
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public void About_OnAction(Office.IRibbonControl control)
        {

            System.Diagnostics.Trace.WriteLine(this.GetType().Name + ": AboutButton clicked");
            WindowWrapper objActiveWindow = new WindowWrapper(Globals.ThisAddIn.Application.ActiveWindow());
            //We would have preferred a consistent design through remoting, but
            //the about dialog is so simple that there is no reason to open it through remoting
            //considering we already have the two modes respectively for settings and options
            UI.AboutForm frmSetting = new UI.AboutForm(this.LCID);
            frmSetting.ShowDialog(objActiveWindow);


        }
    }
}
