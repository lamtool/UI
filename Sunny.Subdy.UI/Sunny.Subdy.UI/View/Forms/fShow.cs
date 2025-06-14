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
    public partial class fShow : Form
    {
        public fShow(Control control)
        {
            InitializeComponent();
            this.Padding = new Padding(20); // Remove any padding around the form
            control.Dock = DockStyle.Fill; // Set the control to fill the form
            this.Controls.Add(control);
        }

        private void fShow_Load(object sender, EventArgs e)
        {

        }
    }
}
