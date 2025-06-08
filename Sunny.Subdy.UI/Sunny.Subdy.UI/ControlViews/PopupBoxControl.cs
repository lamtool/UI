using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.ControlViews
{
    public partial class PopupBoxControl : UserControl
    {
        public PopupBoxControl()
        {
            InitializeComponent();
        }
        public void RegisterButtonEvents(EventHandler minimizeClick, EventHandler maximizeClick, EventHandler closeClick)
        {
            btnMinimize.Click += minimizeClick;
            btnMaximize.Click += maximizeClick;
            btnClose.Click += closeClick;
        }
        
        private void PopupBoxControl_Load(object sender, EventArgs e)
        {

        }
    }
}
