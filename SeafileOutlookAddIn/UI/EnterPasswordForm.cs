using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeafileOutlookAddIn.UI
{
    public partial class EnterPasswordForm : Form
    {
        public String PassWord { set; get; }

        public EnterPasswordForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(this.tbPassword.Text))
            {
                return;
            }

            PassWord = this.tbPassword.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
