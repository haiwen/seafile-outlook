using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SeafileOutlookAddIn.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security;
using SeafileClient;
using SeafileOutlookAddIn.Utils;

namespace SeafileOutlookAddIn.UI
{
    public partial class SettingForm : Form
    {
        private static readonly log4net.ILog log =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public SettingForm()
        {
           
            InitializeComponent();
            //this.tbURL.AutoSize = false;
            //this.tbURL.Size = new System.Drawing.Size(this.tbURL.Size.Width, this.tbURL.Size.Height + 4);

 
            //this.tbPassword.AutoSize = false;
            
            //this.tbPassword.Size = new System.Drawing.Size(this.tbPassword.Size.Width, this.tbPassword.Size.Height + 4);

            //this.tbAccount.AutoSize = false;
            //this.tbAccount.Size = new System.Drawing.Size(this.tbAccount.Size.Width, this.tbAccount.Size.Height + 4);
            //MiscUtils.SetPadding(this.tbURL, new System.Windows.Forms.Padding(5,5,5,5));

        }

        private async void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.UseWaitCursor = true;

                string strPassword = string.Empty;
                string strExpire = string.Empty;


                System.Net.ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, errors) =>
                {
                    return true;
                };


                char[] pChar = this.tbPassword.Text.ToCharArray();
                SecureString pw = new SecureString();

                foreach (char c in pChar)
                {
                    pw.AppendChar(c);
                }

                char[] pwBuf = SecureStringUtils.SecureStringToCharArray(pw);
                pw.Dispose();

                try
                {
                    SeafileSession session = await SeafileSession.Establish(new Uri(this.tbURL.Text, UriKind.Absolute), this.tbAccount.Text, pwBuf);


                }
                catch (SeafException se)
                {
                    log.Error(se.SeafError.GetErrorMessage());
                    //SeafileClient.Types.SeafErrorCode.InvalidCredentials
                    if (se.SeafError.SeafErrorCode == SeafileClient.Types.SeafErrorCode.InvalidCredentials)
                    {
                        MessageBox.Show(
                            Properties.Resources.InvalidCredentials,
                            Properties.Resources.MessageBoxErrorTitle,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                    }
                    else
                    {
                        MessageBox.Show(
                            se.SeafError.GetErrorMessage(),
                            Properties.Resources.MessageBoxErrorTitle,
                            MessageBoxButtons.OK,
                             MessageBoxIcon.Error);
                    }
                    return;
                }

                string encryptedPassword = DataProtectionExtensions.Protect(this.tbPassword.Text);
                string decryptedPassword = DataProtectionExtensions.Unprotect(encryptedPassword);
                // Create a file that the application will store user specific data in.
                using (var file = File.CreateText(Application.UserAppDataPath + "\\seafileaddin.cfg"))
                {
                    JObject objSetting = new JObject(
                               new JProperty("accessurl", this.tbURL.Text),
                               new JProperty("account", this.tbAccount.Text),
                               new JProperty("poassword", encryptedPassword));
                    using (JsonTextWriter writer = new JsonTextWriter(file))
                    {
                        objSetting.WriteTo(writer);
                    }

                    this.DialogResult = DialogResult.OK;
                }

            }
            catch (IOException ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(
                           ex.Message,
                           Properties.Resources.MessageBoxErrorTitle,
                           MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                return;
            }
            finally
            {
                this.UseWaitCursor = false;
            }

        }

        private void SettingForm_Load(object sender, EventArgs e)
        {

            // Create a file that the application will store user specific data in.
            try
            {
                if (!File.Exists(Application.UserAppDataPath + "\\seafileaddin.cfg"))
                {
                    //MessageBox.Show("An error occurred while attempting to show the application." +
                    //             "The error is:" + "文件不存在");
                    return;
                }
                using (var file = File.OpenText(Application.UserAppDataPath + "\\seafileaddin.cfg"))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject o2 = (JObject)JToken.ReadFrom(reader);
                        this.tbURL.Text = (string)o2["accessurl"];
                        this.tbAccount.Text = (string)o2["account"];
                        this.tbPassword.Text = DataProtectionExtensions.Unprotect((string)o2["poassword"]);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(
                           Properties.Resources.ConfigParseError,
                           Properties.Resources.MessageBoxErrorTitle,
                           MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            }
        }
    }
}
