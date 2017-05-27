using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeafileOutlookAddIn.UI
{
    public partial class AboutForm : Form
    {
        public AboutForm(int lcid)
        {
            CultureInfo c = Thread.CurrentThread.CurrentUICulture;
            if (c.LCID != lcid)
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lcid);
            InitializeComponent();
        }


    }
}
