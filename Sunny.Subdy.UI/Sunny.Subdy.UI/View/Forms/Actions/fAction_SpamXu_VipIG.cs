using Sunny.Subdy.Common.ControlMethod;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Forms.Actions
{
    public partial class fAction_SpamXu_VipIG : Form
    {
        public fAction_SpamXu_VipIG()
        {
            InitializeComponent();
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, onLoad: new System.Action(() =>
            {
                checkBox1_CheckedChanged(null, null);
                checkBox13_CheckedChanged(null, null);
                checkBox2_CheckedChanged(null, null);
                checkBox6_CheckedChanged(null, null);
                checkBox7_CheckedChanged(null, null);
                textBox1_TextChanged(null, null);
                checkBox5_CheckedChanged(null, null);
            }), shouldExit: false);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(checkBox5.Checked && string.IsNullOrEmpty(textBox1.Text))
            {
                CommonMethod.ShowMessageWarning("Vui lòng nhập token job để chạy.");
                return;
            }

            DialogResult = DialogResult.OK;
            
            this.Close();
            return;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = checkBox1.Checked;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            panel2.Enabled = checkBox13.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            panel3.Enabled = checkBox2.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            panel6.Enabled = checkBox6.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            panel7.Enabled = checkBox7.Checked;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            groupBox1.Text = $"({textBox1.Lines.Count()}) Token";
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = checkBox5.Checked;
        }
    }
}
