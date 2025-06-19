using OpenCvSharp;
using Sunny.Subd.Core.Facebook;
using Sunny.Subd.Core.Proxies;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Services;
using Sunny.Subdy.UI.View.Controls;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class pageSetting : UIPage
    {
        public pageSetting()
        {
            InitializeComponent();
            cbb_ListTypeProxy.Items.AddRange(ProxyService.Proxies.ToArray());
            comboBox1.Items.AddRange(FacebookHander.TypeLogin.ToArray());
            this.Symbol = 559576; // Set the symbol for the page, can be used for icons
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, action: new System.Action(() =>
            {
                LoadForm();

            }), exists: false);
        }
        private void LoadForm()
        {
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                textBox1.Text = DeviceServices.Brands;
            }
            panel1.Enabled = checkBox1.Checked;
            panel3.Enabled = checkBox2.Checked;
            panel4.Enabled = checkBox3.Checked;
            panel5.Enabled = checkBox4.Checked;
            panel6.Enabled = checkBox5.Checked;
            panel7.Enabled = checkBox8.Checked;
            if (string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                textBox2.Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup", "Device");
            }
            if (string.IsNullOrEmpty(textBox3.Text.Trim()))
            {
                textBox3.Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup", "Profile");
            }
            if (string.IsNullOrEmpty(textBox4.Text.Trim()))
            {
                textBox4.Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "Facebook.apk");
            }
            checkBox7.Enabled = checkBox6.Checked;
            nud_IndexFailProxy.Enabled = chk_CheckProxy.Checked;
            if (string.IsNullOrEmpty(cbb_ListTypeProxy.Text))
            {
                cbb_ListTypeProxy.SelectedIndex = 0;
            }
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.SelectedIndex = 0;
            }
        }
        private void pageSetting_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            fSelectBrandModel fSelectBrand = new fSelectBrandModel(textBox1.Text.Trim());
            if (fSelectBrand.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = fSelectBrand.Brands;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            int count = textBox1.Text.Split('|').Count();
            if (count < 1)
            {
                count = 1;
            }
            label1.Text = $"Chọn bard device ({count}):";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (f.SelectedPath.Trim() == textBox3.Text.Trim())
                {
                    CommonMethod.ShowConfirmWarning("Vui lòng chọn thư mục khác với thư mục profile");
                    return;
                }
                textBox2.Text = f.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (f.SelectedPath.Trim() == textBox2.Text.Trim())
                {
                    CommonMethod.ShowConfirmWarning("Vui lòng chọn thư mục khác với thư mục device");
                    return;
                }
                textBox3.Text = f.SelectedPath;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            LoadForm();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "LamTool.net Chọn file APK";
            openFileDialog.Filter = "File APK (*.apk)|*.apk";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog.FileName;
            }
        }
    }
}
