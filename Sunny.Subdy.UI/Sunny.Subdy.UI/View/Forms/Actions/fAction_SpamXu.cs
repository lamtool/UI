using Sunny.Subd.Core.Models;
using Sunny.Subdy.Common.API;
using Sunny.Subdy.Common.Json;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Forms.Actions
{
    public partial class fAction_SpamXu : Form
    {
        Common.Json.ConfigHelper _configHelper;
        public string _Name = "";
        public string _Json = "";
        private JsonHelper _jsonHelper;
        public fAction_SpamXu(string name, string json, string platform)
        {
            InitializeComponent();
            _Json = json;
            _jsonHelper = new JsonHelper(json, true);

            LoadJobService(platform);
            txtNameAction.Text = name;
            if (string.IsNullOrEmpty(txtType.Text))
            {
                txtType.SelectedIndex = 0;
            }
            _configHelper = new Common.Json.ConfigHelper(this, _Json, true, onLoad: new System.Action(() =>
            {
                checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox2.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox3.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox4.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox5.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox6.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox7.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox13.CheckedChanged += CheckBox1_CheckedChanged;
                txtType.SelectedIndexChanged += txtType_SelectedIndexChanged;
                CheckBox1_CheckedChanged(null, null);
                checkBox19_CheckedChanged(null, null);
            }));
            if (platform == PlatformModel.Instagram)
            {
                checkBox8.Visible = false; panel8.Visible = false;
                checkBox9.Visible = false; panel9.Visible = false;
                checkBox11.Visible = false; panel11.Visible = false;
                checkBox10.Visible = false; panel10.Visible = false;
                checkBox12.Visible = false; panel12.Visible = false;
                checkBox15.Visible = false; panel17.Visible = false;
                checkBox18.Visible = false; panel18.Visible = false;
                checkBox17.Visible = false; panel15.Visible = false;
                checkBox14.Visible = false; panel14.Visible = false;
                checkBox16.Visible = false; panel16.Visible = false;
                panel19.Visible = true;
            }
            else if (platform == PlatformModel.Instagram)
            {
                checkBox8.Visible = true; panel8.Visible = true;
                checkBox9.Visible = true; panel9.Visible = true;
                checkBox11.Visible = true; panel11.Visible = true;
                checkBox10.Visible = true; panel10.Visible = true;
                checkBox12.Visible = true; panel12.Visible = true;
                checkBox15.Visible = true; panel17.Visible = true;
                checkBox18.Visible = true; panel18.Visible = true;
                checkBox17.Visible = true; panel15.Visible = true;
                checkBox14.Visible = true; panel14.Visible = true;
                checkBox16.Visible = true; panel16.Visible = true;
                panel19.Visible = false;
            }

        }
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = checkBox1.Checked;
            panel2.Enabled = checkBox2.Checked;
            panel3.Enabled = checkBox3.Checked;
            panel4.Enabled = checkBox4.Checked;
            panel5.Enabled = checkBox5.Checked;
            panel6.Enabled = checkBox6.Checked;
            panel7.Enabled = checkBox7.Checked;
            panel13.Enabled = checkBox13.Checked;
        }
        private void LoadJobService(string platform)
        {
            txtType.Items.Clear();
            if (platform == PlatformModel.Facebook)
            {
                txtType.Items.AddRange(JobServices.TypesFacebook.ToArray());
            }
            else if (platform == PlatformModel.Instagram)
            {
                txtType.Items.AddRange(JobServices.TypesInstagram.ToArray());
            }

        }
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            _Json = _configHelper.GetJsonString();
            _Name = txtNameAction.Text.Trim();
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            _Json = string.Empty;
            Close();
        }

        private void txtType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtType.SelectedIndex == 1)
            {
                check_AddAccount.Visible = true;
            }
            else
            {
                check_AddAccount.Visible = false;
            }
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {

        }

        private void fAction_SpamXu_Load(object sender, EventArgs e)
        {

        }

        private void fAction_SpamXu_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            groupBox1.Text = $"({textBox1.Lines.Count()}) Token";
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = checkBox19.Checked;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
