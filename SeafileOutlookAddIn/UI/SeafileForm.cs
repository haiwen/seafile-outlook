using System;
using SeafileOutlookAddIn.Utils;
using System.Windows.Forms;
using System.Security;
using SeafileClient;
using SeafileClient.Types;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using SeafileOutlookAddIn.AddIns;
using static SeafileOutlookAddIn.SeafileDir;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SeafileOutlookAddIn.UI
{
    
    public enum DirType
    {
        Library = 0,
        Dir,
        File

    }

    public partial class SeafileForm : Form
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public string ShareLink { set; get; }
        SeafileSession session;
        IList<SeafLibrary> seafLibrary = null;
        public SeafileForm()
        {
            InitializeComponent();
            //this.seafiledirData.SeafileDataTable.AddSeafileDataTableRow("1","Test", "", "", "1");


            /*
            TreeNode treeNode = new TreeNode("Windows");
            tvDir.Nodes.Add(treeNode);
            //
            // Another node following the first node.
            //
            treeNode = new TreeNode("Linux");
            tvDir.Nodes.Add(treeNode);
            //
            // Create two child nodes and put them in an array.
            // ... Add the third node, and specify these as its children.
            //
            TreeNode node2 = new TreeNode("C#");
            TreeNode node3 = new TreeNode("VB.NET");
            TreeNode[] array = new TreeNode[] { node2, node3 };
            //
            // Final node.
            //
            treeNode = new TreeNode("Dot Net Perls", array);
            tvDir.Nodes.Add(treeNode);
            */

        }

        private void listViewDir_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //   private async void tscbLibrary_SelectedIndexChanged(object sender, EventArgs e)
        //   {
        //       if (tscbLibrary.SelectedIndex != -1)
        //       {
        //           if (seafLibrary[tscbLibrary.SelectedIndex].Encrypted)
        //           {
        //               //considering we already have the two modes respectively for settings and options
        //               WindowWrapper objActiveWindow = new WindowWrapper(Globals.ThisAddIn.Application.ActiveWindow());
        //               UI.EnterPasswordForm frmBox = new UI.EnterPasswordForm();
        //               frmBox.ShowDialog(objActiveWindow);

        //               if (frmBox.DialogResult == DialogResult.OK)
        //               {
        //                   bool success = await session.DecryptLibrary(seafLibrary[tscbLibrary.SelectedIndex], frmBox.PassWord.ToCharArray());

        //                   if (!success)
        //                   {
        //                       MessageBox.Show("密码错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                   }

        //               }
        //               else
        //               {
        //                   return;
        //               }


        //           }
        //           curSeafDirEntry = await session.ListDirectory(seafLibrary[tscbLibrary.SelectedIndex]);
        //           for (int i = 0; i < curSeafDirEntry.Count; i++)
        //           {
        //               ListViewItem lvi = new ListViewItem(curSeafDirEntry[i].Id);

        //               lvi.SubItems.Add(curSeafDirEntry[i].Name);   //后面添加的Item都为SubItems ，即为子项  
        //               lvi.SubItems.Add(curSeafDirEntry[i].Size.ToString());
        //               lvi.SubItems.Add(curSeafDirEntry[i].Timestamp.ToString());
        //               lvi.ImageIndex = 0;
        //               if (curSeafDirEntry[i].Type == DirEntryType.Dir)
        //               {
        //                   lvi.ImageIndex = 0;
        //               }
        //               else
        //               {
        //                   lvi.ImageIndex = 1;
        //               }
        //               this.listViewDir.Items.Add(lvi);//最后进行添加  
        //           }


        //       }


        //   }

        //private void tsbShare_Click(object sender, EventArgs e)
        //   {
        //       if (listViewDir.SelectedItems.Count > 0)
        //       {
        //           int index = listViewDir.SelectedItems[0].Index;

        //           WindowWrapper objActiveWindow = new WindowWrapper(Globals.ThisAddIn.Application.ActiveWindow());
        //           UI.GenerateShareLinkForm frmBox = new UI.GenerateShareLinkForm(session, curSeafDirEntry[index].Directory);
        //           frmBox.ShowDialog(objActiveWindow);
        //           if (frmBox.DialogResult == DialogResult.OK)
        //           {

        //           }
        //           else
        //           {

        //           }
        //       }
        //   }

        //   private async void listViewDir_MouseDoubleClick(object sender, MouseEventArgs e)
        //   {
        //       if (listViewDir.SelectedItems.Count > 0)
        //       {
        //           int index = listViewDir.SelectedItems[0].Index;

        //           if (curSeafDirEntry != null && curSeafDirEntry[index].Type == DirEntryType.Dir)
        //           {
        //               var temp = await session.ListDirectory(seafLibrary[tscbLibrary.SelectedIndex], curSeafDirEntry[index].Directory);
        //               if (temp != null)
        //               {
        //                   curSeafDirEntry.Clear();
        //                   listViewDir.Clear();
        //                   curSeafDirEntry = temp;

        //                   for (int i = 0; i < curSeafDirEntry.Count; i++)
        //                   {
        //                       if (curSeafDirEntry[i].Type == DirEntryType.Dir)
        //                       {

        //                           ListViewItem lvi = new ListViewItem(curSeafDirEntry[i].Id);

        //                           lvi.SubItems.Add(curSeafDirEntry[i].Name);   //后面添加的Item都为SubItems ，即为子项  
        //                           lvi.SubItems.Add(curSeafDirEntry[i].Size.ToString());
        //                           lvi.SubItems.Add(curSeafDirEntry[i].Timestamp.ToString());
        //                           lvi.ImageIndex = 0;
        //                           this.listViewDir.Items.Add(lvi);//最后进行添加  
        //                       }

        //                   }
        //               }
        //           }

        //       }
        //   }

        private async void SeafileForm_Load(object sender, EventArgs e)
        {

            this.UseWaitCursor = true;//from the Form/Window instance
            this.tsbRefresh.Enabled = false;
            this.tsbShare.Enabled = false;

            System.Net.ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, errors) =>
            {
                return true;
            };

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
                    return;
               
            }
       

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
                log.Info("connection success.");



                //TODO loading process...
                seafLibrary = await session.ListLibraries();
                IList<string> libList = new List<string>();
                int i = 1;
                foreach (var lib in seafLibrary)
                {
                    libList.Add(lib.Name);
                    //System.Windows.Forms.TreeNode treeNode = new System.Windows.Forms.TreeNode(lib.Name);

                    this.seafiledirData.SeafileDataTable.AddSeafileDataTableRow(i.ToString(), lib.Name, "", lib.Id, ((int)(DirType.Library)).ToString(), "");
                    i++;

                }
                this.tvDir.ExpandAll();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message, Properties.Resources.MessageBoxErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.tsbRefresh.Enabled = false;
                this.tsbShare.Enabled = false;
                this.UseWaitCursor = false;
            }

        }

        private void tsbShare_Click(object sender, EventArgs e)
        {
            log.Info("Share link button click.");
            Control.SeafileDirTreeView.DataTreeViewNode node = tvDir.SelectedNode as Control.SeafileDirTreeView.DataTreeViewNode;
            if (node != null)
            {
                WindowWrapper objActiveWindow = new WindowWrapper(Globals.ThisAddIn.Application.ActiveWindow());
                UI.GenerateShareLinkForm frmBox = new UI.GenerateShareLinkForm(session, node.LibraryID.ToString(), node.Path.ToString());
                frmBox.ShowDialog(objActiveWindow);

                if (frmBox.DialogResult == DialogResult.OK)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    ShareLink = frmBox.ShareLink;
                    this.Close();
                }
               
            }
            
        }

        private async void tvDir_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.tsbRefresh.Enabled = false;
            this.tsbShare.Enabled = false;
            Control.SeafileDirTreeView.DataTreeViewNode node = tvDir.SelectedNode as Control.SeafileDirTreeView.DataTreeViewNode;
            if (node != null)
            {
              
                var nodetype = node.Type.ToString();
                //判断结点类型
                if (nodetype =="0" || nodetype == "1")
                {
                    if((node.Nodes == null) || (node.Nodes.Count == 0))
                    {
                        this.UseWaitCursor = true;
                        try
                        {
                            var temp = await session.ListDirectory(node.LibraryID.ToString(), node.Path.ToString());
                            if (temp != null)
                            {

                                for (int i = 0; i < temp.Count; i++)
                                {
                                    DirType type;
                                    if (temp[i].Type == DirEntryType.Dir)
                                    {
                                        type = DirType.Dir;

                                    }
                                    else
                                    {
                                        type = DirType.File;
                                    }

                                    this.seafiledirData.SeafileDataTable.AddSeafileDataTableRow(string.Format("{0}-{1}", node.ID, (i + 1).ToString()), temp[i].Name, node.ID.ToString(), temp[i].LibraryId, ((int)(type)).ToString(), temp[i].Path);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            MessageBox.Show(ex.Message, Properties.Resources.MessageBoxErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            this.UseWaitCursor = false;
                        }
                        node.Expand();
                    }
                }
            }

            this.tsbRefresh.Enabled = true;
            this.tsbShare.Enabled = true;

        }

        private async void tsbRefresh_Click(object sender, EventArgs e)
        {
            this.tsbRefresh.Enabled = false;
            this.tsbShare.Enabled = false;
            Control.SeafileDirTreeView.DataTreeViewNode node = tvDir.SelectedNode as Control.SeafileDirTreeView.DataTreeViewNode;
            if (node != null)
            {

                this.UseWaitCursor = true;
                try
                {
                    //删除已有treenode及其子节点
                    var list = this.seafiledirData.SeafileDataTable.AsEnumerable().Where(
                        r => r.ID.StartsWith(string.Format("{0}-", node.ID)
                        )).OrderByDescending(r => r.ID).ToList();

                    foreach (var row in list)
                    {
                        this.seafiledirData.SeafileDataTable.RemoveSeafileDataTableRow(row);

                    };


                    //重新获取
                    var temp = await session.ListDirectory(node.LibraryID.ToString(), node.Path.ToString());
                    if (temp != null)
                    {

                        for (int i = 0; i < temp.Count; i++)
                        {
                            DirType type;
                            if (temp[i].Type == DirEntryType.Dir)
                            {
                                type = DirType.Dir;

                            }
                            else
                            {
                                type = DirType.File;
                            }

        

                            this.seafiledirData.SeafileDataTable.AddSeafileDataTableRow(string.Format("{0}-{1}", node.ID, (i + 1).ToString()), temp[i].Name, node.ID.ToString(), temp[i].LibraryId, ((int)(type)).ToString(), temp[i].Path);
                        }
                        node.Expand();
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    MessageBox.Show(ex.Message, Properties.Resources.MessageBoxErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                finally
                {
                    this.UseWaitCursor = false;
                }

            }
            this.tsbRefresh.Enabled = true;
            this.tsbShare.Enabled = true;
        }

        private void tvDir_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.SelectedImageIndex = e.Node.ImageIndex;
        }
    }
}
