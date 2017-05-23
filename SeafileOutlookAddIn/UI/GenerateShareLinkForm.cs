using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SeafileClient;
using System.Threading;
using System.Collections.Concurrent;
using Newtonsoft.Json.Linq;

namespace SeafileOutlookAddIn.UI
{
    public partial class GenerateShareLinkForm : Form
    {
        private static readonly log4net.ILog log =
          log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string ShareLink { set; get; }
        SeafileSession _session;
        String _path;
        String _libraryID;

        CancellationTokenSource tokenSource = null;

        ConcurrentBag<Task> tasks = new ConcurrentBag<Task>();


        public GenerateShareLinkForm(SeafileSession session, String libraryID, String path)
        {
            InitializeComponent();
        

            //this.tbPassword.AutoSize = false;
            //this.tbPassword.Size = new System.Drawing.Size(this.tbPassword.Size.Width, this.tbPassword.Size.Height + 4);

            //this.tbPasswordR.AutoSize = false;
            //this.tbPasswordR.Size = new System.Drawing.Size(this.tbPasswordR.Size.Width, this.tbPasswordR.Size.Height + 4);

            //this.tbExpireDay.AutoSize = false;
            //this.tbExpireDay.Size = new System.Drawing.Size(this.tbExpireDay.Size.Width, this.tbExpireDay.Size.Height + 4);

            _session = session;
            _path = path;
            _libraryID = libraryID;


            this.lbShareFile.Text = string.Format("{0}:{1}", Properties.Resources.ShareFile, path);
        }

        private async void btnGenerateLink_Click(object sender, EventArgs e)
        {
            tokenSource = new CancellationTokenSource();

            this.btnGenerateLink.Enabled = false;

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
                    this.btnGenerateLink.Enabled = true;
                    return;
                }

                if (this.tbPassword.Text.Length < 8)
                {
                    MessageBox.Show(
                 Properties.Resources.PasswordTooShort,
                 Properties.Resources.MessageBoxInfoTitle,
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Information);
                    this.btnGenerateLink.Enabled = true;
                    return;
                }

                if (this.tbPassword.Text != this.tbPasswordR.Text)
                {
                    MessageBox.Show(
                     Properties.Resources.PasswordNotMatch,
                     Properties.Resources.MessageBoxInfoTitle,
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
                    this.btnGenerateLink.Enabled = true;
                    return;
                }
                strPassword = this.tbPassword.Text;
            }

            if (cbExpire.Checked)
            {
                if (string.IsNullOrEmpty(this.tbExpireDay.Text))
                {
                    return;
                }
                strExpire = this.tbExpireDay.Text;
            }


            this.UseWaitCursor = true;
            try
            {
                var resultGetFileDetail = await _session.GetFileDetail(_libraryID, _path, tokenSource.Token);

                if (resultGetFileDetail != null)
                {
                    var resultCreatShareLink = await _session.CreatShareLink(resultGetFileDetail, tokenSource.Token, strPassword, strExpire);
                    if (!string.IsNullOrEmpty(resultCreatShareLink))
                    {
                        this.DialogResult = DialogResult.OK;

                        JObject o2 = (JObject)JToken.Parse(resultCreatShareLink);
                        this.ShareLink = (string)o2["link"];
                        if(cbPassword.Checked)
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
            catch (SeafException se)
            {
                log.Error(se.SeafError.GetErrorMessage());

                MessageBox.Show(
                           se.SeafError.GetErrorMessage(),
                           Properties.Resources.MessageBoxErrorTitle,
                           MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message, Properties.Resources.MessageBoxErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.btnGenerateLink.Enabled = true;
                this.UseWaitCursor = false;
                tokenSource.Dispose();
                tokenSource = null;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (tokenSource != null)
            {

                tokenSource.Cancel();
                tokenSource.Dispose();
            }

            this.DialogResult = DialogResult.Cancel;

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
    }
}
