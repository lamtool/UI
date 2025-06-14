using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sunny.Subdy.Common.Helper
{
    public class ControlHelper
    {
        public static List<Control> GetControls(Control control)
        {
            List<Control> controls = new List<Control>();
            foreach (Control c in control.Controls)
            {
                controls.Add(c);
                if (c.Controls.Count > 0)
                {
                    controls.AddRange(GetControls(c));
                }
            }
            return controls;
        }
    }
}
