using System;
using SeafileOutlookAddIn.Utils;
using System.Windows.Forms;
using System.Security;
using SeafileClient;
using SeafileClient.Types;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Globalization;

namespace SeafileOutlookAddIn.UI
{
    public partial class UploadForm : Form
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string strSharelink = string.Empty;

        SeafileSession session;

        CancellationTokenSource tokenSource = null;

        bool bWork = false;
        ConcurrentBag<Task> tasks = new ConcurrentBag<Task>();

        public string ShareLink { get; set; }

        public UploadForm(int lcid)
        {
            CultureInfo c = Thread.CurrentThread.CurrentUICulture;
            if (c.LCID != lcid)
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lcid);

            InitializeComponent();

            tbPassword.Enabled = false;
            tbPasswordR.Enabled = false;
            tbExpireDay.Enabled = false;

            //this.tbFile.AutoSize = false;
            //this.tbFile.Size = new System.Drawing.Size(this.tbFile.Size.Width, this.tbFile.Size.Height + 4);



            //this.tbPassword.AutoSize = false;
            //this.tbPassword.Size = new System.Drawing.Size(this.tbPassword.Size.Width, this.tbPassword.Size.Height + 4);

            //this.tbPasswordR.AutoSize = false;
            //this.tbPasswordR.Size = new System.Drawing.Size(this.tbPasswordR.Size.Width, this.tbPasswordR.Size.Height + 4);

            //this.tbExpireDay.AutoSize = false;
            //this.tbExpireDay.Size = new System.Drawing.Size(this.tbExpireDay.Size.Width, this.tbExpireDay.Size.Height + 4);

        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();

            //fd.InitialDirectory = "C:\\" ;
            fd.Filter = "All files (*.*)|*.*";
            fd.FilterIndex = 1;
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                tbFile.Text = fd.FileName;
            }
        }


        private async void btnUpload_Click(object sender, EventArgs e)
        {
            log.Info("btnUpload_Click");
            if (!bWork)
            {
                this.pbUpload.Value = 0;
                tokenSource = new CancellationTokenSource();
                this.btnCancelUplaod.Enabled = false;
                bWork = true;
                string host = string.Empty;
                string user = string.Empty;
                string strPassWord = string.Empty;
                try
                {
                    if (!File.Exists(Application.UserAppDataPath + "\\seafileaddin.cfg"))
                    {

                        log.Error("seafileaddin.cfg not exits");
                        MessageBox.Show(
                            Properties.Resources.SetAccount,
                            Properties.Resources.MessageBoxErrorTitle,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        //MessageBox.Show("An error occurred while attempting to show the application." +
                        //            "The error is:" + Properties.Resources.SetAccount);
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        this.Close();

                        return;
                    }

                    using (var file = File.OpenText(Application.UserAppDataPath + "\\seafileaddin.cfg"))
                    {
                        using (JsonTextReader reader = new JsonTextReader(file))
                        {
                            JObject o2 = (JObject)JToken.ReadFrom(reader);
                            host = (string)o2["accessurl"];
                            user = (string)o2["account"];
                            strPassWord = DataProtectionExtensions.Unprotect((string)o2["poassword"]);
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

                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.Close();

                    //
                    return;

                }



                if (string.IsNullOrEmpty(tbFile.Text))
                {
                    MessageBox.Show(
                       Properties.Resources.EmptyUploadFile,
                       Properties.Resources.MessageBoxInfoTitle,
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Information);
                    this.btnCancelUplaod.Enabled = true;
                    bWork = false;
                    return;
                }



                string strPassword = string.Empty;
                string strExpire = string.Empty;
                if (cbPassword.Checked)
                {
                    if (string.IsNullOrEmpty(this.tbPassword.Text))
                    {
                        MessageBox.Show(
                      Properties.Resources.EmptyPassword,
                      Properties.Resources.MessageBoxInfoTitle,
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Information);
                        this.btnCancelUplaod.Enabled = true;
                        bWork = false;
                        return;
                    }

                    if (this.tbPassword.Text.Length < 8)
                    {
                        MessageBox.Show(
                     Properties.Resources.PasswordTooShort,
                     Properties.Resources.MessageBoxInfoTitle,
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
                        this.btnCancelUplaod.Enabled = true;
                        bWork = false;
                        return;
                    }

                    if (this.tbPassword.Text != this.tbPasswordR.Text)
                    {
                        MessageBox.Show(
                      Properties.Resources.PasswordNotMatch,
                      Properties.Resources.MessageBoxInfoTitle,
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Information);
                        this.btnCancelUplaod.Enabled = true;
                        bWork = false;
                        return;
                    }
                    strPassword = this.tbPassword.Text;
                }

                if (cbExpire.Checked)
                {
                    if (string.IsNullOrEmpty(this.tbExpireDay.Text))
                    {
                        this.btnCancelUplaod.Enabled = true;
                        bWork = false;
                        return;
                    }
                    strExpire = this.tbExpireDay.Text;
                }



                System.Net.ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, errors) =>
                {
                    return true;
                };


                char[] pChar = strPassWord.ToCharArray();
                SecureString pw = new SecureString();

                foreach (char c in pChar)
                {
                    pw.AppendChar(c);
                }

                char[] pwBuf = SecureStringUtils.SecureStringToCharArray(pw);
                pw.Dispose();

                try
                {
                    session = await SeafileSession.Establish(new Uri(host, UriKind.Absolute), user, pwBuf);

                    // try to connect to the seafile server using the given credentials
                    log.Info("connection success.");


                    this.btnCancelUplaod.Enabled = true;
                    this.btnCancelUplaod.Text = Properties.Resources.Cancel;


                    //TODO loading process...
                    var seafDefaultLibrary = await session.GetDefaultLibrary();


                    try
                    {
                        var exit = await session.ListDirectory(seafDefaultLibrary, "/outlook");
                    }
                    catch (SeafException exListDirectory)
                    {
                        log.Info("outlook folder doesn't exit.");
                        if (exListDirectory.SeafError.HttpStatusCode == System.Net.HttpStatusCode.Forbidden)
                        {

                            bool successCreate = await session.CreateDirectory(seafDefaultLibrary, "/outlook");

                        }
                    }
                    //catch (Exception ex)
                    //{
                    //    log.Error(ex.Message);

                    //    MessageBox.Show(ex.Message, Properties.Resources.MessageBoxErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //    return;

                    //}

                    //check if file exist
                    string fileName = System.IO.Path.GetFileName(tbFile.Text);


                    System.IO.FileStream fs = new System.IO.FileStream(tbFile.Text, System.IO.FileMode.Open);
                    pbUpload.Value = 0;
                    Action<float> ProcessAction = new Action<float>(ProcessCallBack);

                    var resultUploadSingle = await session.UploadSingle(seafDefaultLibrary, "/outlook", fileName, fs, tokenSource.Token, ProcessAction);

                    if (resultUploadSingle != null)
                    {

                        JObject o2 = (JObject)JToken.Parse(resultUploadSingle.Substring(1, resultUploadSingle.Length - 2));

                        string tmpName = (string)o2["name"];

                        SeafDirEntry tmp = new SeafDirEntry();

                        tmp.LibraryId = seafDefaultLibrary.Id;

                        tmp.Path = string.Format("/outlook/{0}", tmpName);

                        var resultCreatShareLink = await session.CreatShareLink(tmp, tokenSource.Token, strPassword, strExpire);
                        if (!string.IsNullOrEmpty(resultCreatShareLink))
                        {
                            this.DialogResult = DialogResult.OK;
                            //this.ShareLink = resultCreatShareLink;
                            JObject oShare = (JObject)JToken.Parse(resultCreatShareLink);
                            this.ShareLink = (string)oShare["link"];
                            if (cbPassword.Checked)
                            {
                                this.ShareLink += string.Format("\r\n {0}:{1}", Properties.Resources.SharePassword, this.tbPassword.Text);
                            }

                        }
                    }
                }
                catch (System.OperationCanceledException ex)
                {
                    log.Info(ex.Message);
                }
                catch (System.IO.IOException ioEx)
                {
                    log.Error(ioEx.Message);
                    MessageBox.Show(
                     Properties.Resources.ReadFileError,
                     Properties.Resources.MessageBoxErrorTitle,
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    MessageBox.Show(ex.Message, Properties.Resources.MessageBoxErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.btnCancelUplaod.Enabled = true;
                    this.btnCancelUplaod.Text = Properties.Resources.UploadBtn;
                    tokenSource.Dispose();
                    bWork = false;
                }
            }
            else
            {
                if (tokenSource != null)
                {
                    tokenSource.Cancel();
                    log.Info("cancel upload.");
                    bWork = false;
                    this.pbUpload.Value = 0;
                }
            }
        }

        void ProcessCallBack(float value)
        {
            this.BeginInvoke((Action)(() =>
            {
                string str = ((int)value).ToString() + "%";
                //Font font = new Font("Times New Roman", (float)11, FontStyle.Regular);
                //PointF pt = new PointF(this.pbUpload.Width / 2 - 10, this.pbUpload.Height / 2 - 10);
                //this.pbUpload.CreateGraphics().DrawString(str, font, Brushes.Red, pt);
                this.pbUpload.Value = (int)(value);
            }));
        }

        private void tbExpireDay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {
                    e.Handled = true;
                }
            }
        }

        private void UploadForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(Application.UserAppDataPath + "\\seafileaddin.cfg"))
                {

                    log.Error("seafileaddin.cfg not exits");
                    MessageBox.Show(
                        Properties.Resources.SetAccount,
                        Properties.Resources.MessageBoxErrorTitle,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    //MessageBox.Show("An error occurred while attempting to show the application." +
                    //            "The error is:" + Properties.Resources.SetAccount);
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.Close();

                    return;
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

                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();

                //
                return;

            }
        }

        private void cbPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPassword.Checked)
            {
                tbPassword.Enabled = true;
                tbPasswordR.Enabled = true;
            }
            else
            {
                tbPassword.Enabled = false;
                tbPasswordR.Enabled = false;
            }
        }

        private void cbExpire_CheckedChanged(object sender, EventArgs e)
        {    if(cbExpire.Checked)
            {
                tbExpireDay.Enabled = true;
            }
            else
            {
                tbExpireDay.Enabled = false;
            }
        }
    }
    
}
