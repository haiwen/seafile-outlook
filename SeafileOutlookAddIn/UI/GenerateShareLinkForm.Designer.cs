namespace SeafileOutlookAddIn.UI
{
    partial class GenerateShareLinkForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateShareLinkForm));
            this.btnGenerateLink = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbExpireDay = new System.Windows.Forms.TextBox();
            this.tbPasswordR = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.cbExpire = new System.Windows.Forms.CheckBox();
            this.cbPassword = new System.Windows.Forms.CheckBox();
            this.lbShareFile = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGenerateLink
            // 
            resources.ApplyResources(this.btnGenerateLink, "btnGenerateLink");
            this.btnGenerateLink.Name = "btnGenerateLink";
            this.btnGenerateLink.UseVisualStyleBackColor = true;
            this.btnGenerateLink.Click += new System.EventHandler(this.btnGenerateLink_Click);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.Name = "btnExit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Name = "label2";
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
            // lbShareFile
            // 
            resources.ApplyResources(this.lbShareFile, "lbShareFile");
            this.lbShareFile.Name = "lbShareFile";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // GenerateShareLinkForm
            // 
            this.AcceptButton = this.btnGenerateLink;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbExpireDay);
            this.Controls.Add(this.tbPasswordR);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.cbExpire);
            this.Controls.Add(this.cbPassword);
            this.Controls.Add(this.lbShareFile);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnGenerateLink);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GenerateShareLinkForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGenerateLink;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbExpireDay;
        private System.Windows.Forms.TextBox tbPasswordR;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.CheckBox cbExpire;
        private System.Windows.Forms.CheckBox cbPassword;
        private System.Windows.Forms.Label lbShareFile;
        private System.Windows.Forms.Label label1;
    }
}