namespace SeafileOutlookAddIn.UI
{
    partial class UploadForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadForm));
            this.pbUpload = new System.Windows.Forms.ProgressBar();
            this.btnCancelUplaod = new System.Windows.Forms.Button();
            this.seafileDir1 = new SeafileOutlookAddIn.SeafileDir();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbExpireDay = new System.Windows.Forms.TextBox();
            this.tbPasswordR = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.cbExpire = new System.Windows.Forms.CheckBox();
            this.cbPassword = new System.Windows.Forms.CheckBox();
            this.tbFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.seafileDir1)).BeginInit();
            this.SuspendLayout();
            // 
            // pbUpload
            // 
            resources.ApplyResources(this.pbUpload, "pbUpload");
            this.pbUpload.Name = "pbUpload";
            // 
            // btnCancelUplaod
            // 
            resources.ApplyResources(this.btnCancelUplaod, "btnCancelUplaod");
            this.btnCancelUplaod.Name = "btnCancelUplaod";
            this.btnCancelUplaod.UseVisualStyleBackColor = true;
            this.btnCancelUplaod.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // seafileDir1
            // 
            this.seafileDir1.DataSetName = "SeafileDir";
            this.seafileDir1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tbExpireDay
            // 
            resources.ApplyResources(this.tbExpireDay, "tbExpireDay");
            this.tbExpireDay.Name = "tbExpireDay";
            // 
            // tbPasswordR
            // 
            resources.ApplyResources(this.tbPasswordR, "tbPasswordR");
            this.tbPasswordR.Name = "tbPasswordR";
            // 
            // tbPassword
            // 
            resources.ApplyResources(this.tbPassword, "tbPassword");
            this.tbPassword.Name = "tbPassword";
            // 
            // cbExpire
            // 
            resources.ApplyResources(this.cbExpire, "cbExpire");
            this.cbExpire.Name = "cbExpire";
            this.cbExpire.UseVisualStyleBackColor = true;
            this.cbExpire.CheckedChanged += new System.EventHandler(this.cbExpire_CheckedChanged);
            // 
            // cbPassword
            // 
            resources.ApplyResources(this.cbPassword, "cbPassword");
            this.cbPassword.Name = "cbPassword";
            this.cbPassword.UseVisualStyleBackColor = true;
            this.cbPassword.CheckedChanged += new System.EventHandler(this.cbPassword_CheckedChanged);
            // 
            // tbFile
            // 
            resources.ApplyResources(this.tbFile, "tbFile");
            this.tbFile.Name = "tbFile";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnSelectFile
            // 
            resources.ApplyResources(this.btnSelectFile, "btnSelectFile");
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Name = "label2";
            // 
            // UploadForm
            // 
            this.AcceptButton = this.btnCancelUplaod;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbExpireDay);
            this.Controls.Add(this.tbPasswordR);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.cbExpire);
            this.Controls.Add(this.cbPassword);
            this.Controls.Add(this.tbFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.pbUpload);
            this.Controls.Add(this.btnCancelUplaod);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "UploadForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.UploadForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.seafileDir1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar pbUpload;
        private System.Windows.Forms.Button btnCancelUplaod;
        private SeafileDir seafileDir1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbExpireDay;
        private System.Windows.Forms.TextBox tbPasswordR;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.CheckBox cbExpire;
        private System.Windows.Forms.CheckBox cbPassword;
        private System.Windows.Forms.TextBox tbFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
    }
}