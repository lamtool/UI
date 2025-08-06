using Sunny.Subdy.Common.ControlMethod;
using System;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Forms.Actions
{
    public partial class fAction_RegVipIG : Form
    {
        public fAction_RegVipIG()
        {
            InitializeComponent();
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, onLoad: new System.Action(() =>
            {
                checkBox1_CheckedChanged(null, null);
                checkBox13_CheckedChanged(null, null);
                checkBox2_CheckedChanged(null, null);
                checkBox6_CheckedChanged(null, null);
                checkBox7_CheckedChanged(null, null);
            }), shouldExit: false);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                CommonMethod.ShowMessageError("Vui lòng nhập keycaptcha");
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                CommonMethod.ShowMessageError("Vui lòng nhập mật khẩu");
                return;
            }
            this.Close();
            return;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
