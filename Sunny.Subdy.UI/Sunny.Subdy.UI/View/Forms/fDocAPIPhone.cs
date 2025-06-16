using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Forms
{
    public partial class fDocAPIPhone : Form
    {
        public fDocAPIPhone()
        {
            InitializeComponent();
        }

        private void fDocAPIPhone_Load(object sender, EventArgs e)
        {
            uiTitlePanel1.Size = new Size(424, 420);
            uiTitlePanel1.Collapsed = true;
            uiTitlePanel2.Size = new Size(424, 258);
            uiTitlePanel2.Collapsed = true;

            uiTitlePanel3.Size = new Size(424, 260);
            uiTitlePanel3.Collapsed = true;
            uiTitlePanel4.Size = new Size(424, 260);
            uiTitlePanel4.Collapsed = true;
        }

        private void uiTitlePanel4_Click(object sender, EventArgs e)
        {
           
        }
    }
}
