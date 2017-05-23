namespace SeafileOutlookAddIn.UI
{
    partial class SeafileForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SeafileForm));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbShare = new System.Windows.Forms.ToolStripButton();
            this.seafiledirData = new SeafileOutlookAddIn.SeafileDir();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.tvDir = new SeafileOutlookAddIn.Control.SeafileDirTreeView();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seafiledirData)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRefresh,
            this.tsbShare});
            this.toolStrip.Name = "toolStrip";
            // 
            // tsbRefresh
            // 
            resources.ApplyResources(this.tsbRefresh, "tsbRefresh");
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // tsbShare
            // 
            resources.ApplyResources(this.tsbShare, "tsbShare");
            this.tsbShare.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbShare.Name = "tsbShare";
            this.tsbShare.Click += new System.EventHandler(this.tsbShare_Click);
            // 
            // seafiledirData
            // 
            this.seafiledirData.DataSetName = "SeafileDir";
            this.seafiledirData.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "lib.png");
            this.imageList.Images.SetKeyName(1, "folder.png");
            this.imageList.Images.SetKeyName(2, "file.png");
            // 
            // tvDir
            // 
            resources.ApplyResources(this.tvDir, "tvDir");
            this.tvDir.DataMember = "SeafileDataTable";
            this.tvDir.DataSource = this.seafiledirData;
            this.tvDir.DirTypeColumn = "Type";
            this.tvDir.FullRowSelect = true;
            this.tvDir.HideSelection = false;
            this.tvDir.HotTracking = true;
            this.tvDir.IDColumn = "ID";
            this.tvDir.ImageList = this.imageList;
            this.tvDir.LibraryIDColumn = "LibraryId";
            this.tvDir.Name = "tvDir";
            this.tvDir.NameColumn = "Name";
            this.tvDir.ParentIDColumn = "ParentId";
            this.tvDir.PathColumn = "Path";
            this.tvDir.ValueColumn = "ID";
            this.tvDir.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDir_AfterSelect);
            this.tvDir.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tvDir_MouseDoubleClick);
            // 
            // SeafileForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvDir);
            this.Controls.Add(this.toolStrip);
            this.MaximizeBox = false;
            this.Name = "SeafileForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.SeafileForm_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seafiledirData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbRefresh;
        private System.Windows.Forms.ToolStripButton tsbShare;
        private Control.SeafileDirTreeView tvDir;
        private SeafileDir seafiledirData;
        private System.Windows.Forms.ImageList imageList;
        //private System.Windows.Forms.TreeView tvDir;
    }
}