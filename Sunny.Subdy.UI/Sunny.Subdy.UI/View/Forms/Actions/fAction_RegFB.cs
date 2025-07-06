using Sunny.Subd.Core.Proxies;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Forms.Actions
{
    public partial class fAction_RegFB : Form
    {
        private FolderContext _folderContext; [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Folder Folder { get; set; } = new Folder();
        private string _platform;
        public fAction_RegFB(string platform)
        {
            _platform = platform;
            _folderContext = new FolderContext();
            InitializeComponent();
            if (uiComboBox1 == null)
            {
                uiComboBox1 = new UIComboBox();
                uiComboBox1.DataSource = null;
                uiComboBox1.FillColor = Color.White;
                uiComboBox1.Font = new Font("Microsoft Sans Serif", 12F);
                uiComboBox1.ItemHoverColor = Color.FromArgb(155, 200, 255);
                uiComboBox1.ItemSelectForeColor = Color.FromArgb(235, 243, 255);
                uiComboBox1.Location = new Point(31, 15);
                uiComboBox1.Margin = new Padding(4, 5, 4, 5);
                uiComboBox1.MinimumSize = new Size(63, 0);
                uiComboBox1.Name = "uiComboBox1";
                uiComboBox1.Padding = new Padding(0, 0, 30, 2);
                uiComboBox1.Size = new Size(457, 29);
                uiComboBox1.SymbolSize = 24;
                uiComboBox1.TabIndex = 248;
                uiComboBox1.TextAlignment = ContentAlignment.MiddleLeft;
                uiComboBox1.Watermark = "";
                tabPage2.Controls.Add(uiComboBox1);
            }
            uiComboBox1.DropDownStyle = UIDropDownStyle.DropDownList;
            if (uiSymbolButton3 == null)
            {
                uiSymbolButton3 = new UISymbolButton();
                uiSymbolButton3.Anchor = AnchorStyles.Right;
                uiSymbolButton3.FillColor = Color.Green;
                uiSymbolButton3.FillColor2 = Color.FromArgb(4, 60, 44);
                uiSymbolButton3.FillHoverColor = Color.FromArgb(4, 60, 44);
                uiSymbolButton3.FillPressColor = Color.FromArgb(4, 60, 44);
                uiSymbolButton3.FillSelectedColor = Color.FromArgb(4, 60, 44);
                uiSymbolButton3.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
                uiSymbolButton3.Location = new Point(138, 496);
                uiSymbolButton3.Margin = new Padding(3, 3, 10, 3);
                uiSymbolButton3.MinimumSize = new Size(1, 1);
                uiSymbolButton3.Name = "uiSymbolButton3";
                uiSymbolButton3.Radius = 15;
                uiSymbolButton3.RectColor = Color.FromArgb(4, 60, 44);
                uiSymbolButton3.RectHoverColor = Color.FromArgb(4, 60, 44);
                uiSymbolButton3.RectPressColor = Color.FromArgb(4, 60, 44);
                uiSymbolButton3.RectSelectedColor = Color.FromArgb(4, 60, 44);
                uiSymbolButton3.Size = new Size(130, 40);
                uiSymbolButton3.Symbol = 361515;
                uiSymbolButton3.SymbolSize = 18;
                uiSymbolButton3.TabIndex = 98;
                uiSymbolButton3.Text = "Bắt đầu";
                uiSymbolButton3.TipsFont = new Font("Microsoft Sans Serif", 9F);
                Controls.Add(uiSymbolButton3);
            }
            uiSymbolButton3.Click += uiSymbolButton3_Click;
            if (uiSymbolButton4 == null)
            {
                uiSymbolButton4 = new UISymbolButton();
                uiSymbolButton4.Anchor = AnchorStyles.Right;
                uiSymbolButton4.FillColor = Color.Red;
                uiSymbolButton4.FillColor2 = Color.DarkRed;
                uiSymbolButton4.FillHoverColor = Color.DarkRed;
                uiSymbolButton4.FillPressColor = Color.DarkRed;
                uiSymbolButton4.FillSelectedColor = Color.DarkRed;
                uiSymbolButton4.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
                uiSymbolButton4.Location = new Point(286, 496);
                uiSymbolButton4.Margin = new Padding(10, 3, 3, 3);
                uiSymbolButton4.MinimumSize = new Size(1, 1);
                uiSymbolButton4.Name = "uiSymbolButton4";
                uiSymbolButton4.Radius = 15;
                uiSymbolButton4.RectColor = Color.DarkRed;
                uiSymbolButton4.RectHoverColor = Color.DarkRed;
                uiSymbolButton4.RectPressColor = Color.DarkRed;
                uiSymbolButton4.RectSelectedColor = Color.DarkRed;
                uiSymbolButton4.Size = new Size(130, 40);
                uiSymbolButton4.Symbol = 61453;
                uiSymbolButton4.SymbolSize = 17;
                uiSymbolButton4.TabIndex = 97;
                uiSymbolButton4.Text = "Đóng";
                uiSymbolButton4.TipsColor = Color.DarkRed;
                uiSymbolButton4.TipsFont = new Font("Microsoft Sans Serif", 9F);
                Controls.Add(uiSymbolButton4);
            }
            uiSymbolButton4.Click += uiSymbolButton4_Click;
            uiComboBox1.Clear();
            if (_platform == RegistrationType.RegFacebook)
            {
                uiComboBox1.Items.AddRange(RegistrationType.RegFacebook_AllTypes.ToArray());
                checkBox2.Visible = true;
                if (!tabControl1.Controls.Contains(tabPage4))
                {
                    tabControl1.Controls.Add(tabPage4);
                }

            }
            else if (_platform == RegistrationType.RegGmail)
            {
                uiComboBox1.Items.AddRange(RegistrationType.RegGmail_AllTypes.ToArray());
                checkBox2.Visible = false;
                if (tabControl1.Controls.Contains(tabPage4))
                {
                    tabControl1.Controls.Remove(tabPage4);
                }
            }


            if (string.IsNullOrEmpty(uiComboBox1.Text))
            {
                uiComboBox1.SelectedIndex = 0;
            }

            uiComboBox1.SelectedIndexChanged += uiComboBox1_SelectedIndexChanged;
            cbb_Email.Items.Clear();

            cbb_Email.Items.AddRange(RegistrationType.EmailTypes.ToArray());

            if (string.IsNullOrEmpty(cbb_Email.Text.Trim()))
            {
                cbb_Email.SelectedIndex = 0;
            }

            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(RegistrationType.PhoneNumberTypes.ToArray());
            if (string.IsNullOrEmpty(comboBox1.Text.Trim()))
            {
                comboBox1.SelectedIndex = 0;
            }


            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(RegistrationType.EmailTypes.ToArray());
            if (string.IsNullOrEmpty(comboBox2.Text.Trim()))
            {
                comboBox2.SelectedIndex = 0;
            }
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(RegistrationType.EmailTypes.ToArray());
            if (string.IsNullOrEmpty(comboBox2.Text.Trim()))
            {
                comboBox2.SelectedIndex = 0;
            }



            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name + "_" + _platform, action: new System.Action(() =>
            {
                LoadUI();
                panel11.Enabled = check_Bia.Checked;
                panel12.Enabled = check_Avatar.Checked;
                if (uiComboBox1.Text == RegistrationType.Domain || uiComboBox1.Text == RegistrationType.Domain_BaitPhoneNumber)
                {
                    groupBox15.Visible = false;
                    groupBox8.Visible = true;
                    groupBox8.Dock = DockStyle.Fill;
                    groupBox2.Visible = false;
                }
                if (uiComboBox1.Text == RegistrationType.Gmail || uiComboBox1.Text == RegistrationType.Gmail_BaitPhoneNumber)
                {
                    groupBox15.Visible = true;
                    groupBox8.Visible = false;
                    groupBox15.Dock = DockStyle.Fill;
                    groupBox2.Visible = false;
                }
                if (uiComboBox1.Text == RegistrationType.PhoneNumber)
                {
                    groupBox15.Visible = false;
                    groupBox8.Visible = false;
                    groupBox2.Dock = DockStyle.Fill;
                    groupBox2.Visible = true;
                }
                panel5.Enabled = radioButton3.Checked;
                panel4.Enabled = radioButton3.Checked;
                if (cbb_Email.Text == RegistrationType.Domain_Getnada || cbb_Email.Text == RegistrationType.Domain_MailTM || cbb_Email.Text == RegistrationType.Domain_TempMail)
                {
                    textBox2.Enabled = false;
                }
                else
                {
                    textBox2.Enabled = true;
                }
                panel3.Enabled = checkBox1.Checked;
                txtPass.Enabled = !check_PassRandom.Checked;

            }), exists: false);

        }
        private void LoadFolder()
        {
            comboBox3.Items.Clear();
            string type = _platform == RegistrationType.RegFacebook ? "Facebook" : "Gmail";
            var folders = _folderContext.GetByType(type) ?? new List<Folder>();

            if (!folders.Any())
            {
                comboBox3.Items.Add("Tạo nhóm tài khoản trước khi chạy.");
            }
            else
            {
                foreach (var folder in folders)
                    comboBox3.Items.Add(folder.Name);
            }

            if (string.IsNullOrEmpty(comboBox3.Text.Trim()) && comboBox3.Items.Count > 0)
                comboBox3.SelectedIndex = 0;
        }
        private void LoadUI()
        {
            LoadFolder();
        }
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {

            Close();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void fAction_RegFB_Load(object sender, EventArgs e)
        {

        }

        private void uiSymbolButton2_Click_1(object sender, EventArgs e)
        {
            LoadFolder();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "LamTool.net Chọn file Gmail";
            openFileDialog.Filter = "File text (*.txt)|*.txt";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtGmail.Text = openFileDialog.FileName;
            }
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(uiComboBox1.Text))
            {
                return;
            }
            if (uiComboBox1.Text == RegistrationType.Domain || uiComboBox1.Text == RegistrationType.Domain_BaitPhoneNumber)
            {
                groupBox15.Visible = false;
                groupBox8.Visible = true;
                groupBox8.Dock = DockStyle.Fill;
                groupBox2.Visible = false;
            }
            if (uiComboBox1.Text == RegistrationType.Gmail || uiComboBox1.Text == RegistrationType.Gmail_BaitPhoneNumber)
            {
                groupBox15.Visible = true;
                groupBox8.Visible = false;
                groupBox15.Dock = DockStyle.Fill;
                groupBox2.Visible = false;
            }
            if (uiComboBox1.Text == RegistrationType.PhoneNumber)
            {
                groupBox15.Visible = false;
                groupBox8.Visible = false;
                groupBox2.Dock = DockStyle.Fill;
                groupBox2.Visible = true;
            }
            if (uiComboBox1.Text == RegistrationType.NVR)
            {
                groupBox15.Visible = false;
                groupBox8.Visible = false;
                groupBox2.Dock = DockStyle.Fill;
                groupBox2.Visible = false;
            }
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox3.Text.Trim()) || comboBox3.Text.Trim() == "Tạo nhóm tài khoản trước khi chạy.")
            {
                CommonMethod.ShowMessageWarning("Vui lòng chọn nhóm tài khoản cần lưu.");
                return;
            }
            Folder = _folderContext.GetByName(comboBox3.Text.Trim());
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cbb_Email_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbb_Email.Text))
            {
                return;
            }
            if (cbb_Email.Text == RegistrationType.Domain_Getnada || cbb_Email.Text == RegistrationType.Domain_MailTM || cbb_Email.Text == RegistrationType.Domain_TempMail)
            {
                textBox2.Enabled = false;
            }
            else
            {
                textBox2.Enabled = true;
            }

        }

        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void AddFile(System.Windows.Forms.TextBox textBox)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Chọn một tệp văn bản"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox.Text = openFileDialog.FileName;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AddFile(txt_Ho);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddFile(txt_Ten);
        }

        private void check_NameRandom_CheckedChanged(object sender, EventArgs e)
        {
            panel5.Enabled = radioButton3.Checked;
            panel4.Enabled = radioButton3.Checked;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            panel5.Enabled = radioButton3.Checked;
            panel4.Enabled = radioButton3.Checked;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            panel5.Enabled = radioButton3.Checked;
            panel4.Enabled = radioButton3.Checked;
        }

        private void check_PassRandom_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.Enabled = !check_PassRandom.Checked;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.Enabled = !check_PassRandom.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            panel3.Enabled = checkBox1.Checked;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox2.Text))
            {
                return;
            }
            if (comboBox2.Text == RegistrationType.Domain_Getnada || comboBox2.Text == RegistrationType.Domain_MailTM || comboBox2.Text == RegistrationType.Domain_TempMail)
            {
                textBox3.Enabled = false;
            }
            else
            {
                textBox3.Enabled = true;
            }
        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void check_Avatar_CheckedChanged(object sender, EventArgs e)
        {
            panel12.Enabled = check_Avatar.Checked;
        }

        private void check_Bia_CheckedChanged(object sender, EventArgs e)
        {
            panel11.Enabled = check_Bia.Checked;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                txtAvatar.Text = f.SelectedPath;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            if (f.ShowDialog() == DialogResult.OK)
            {
                txtBia.Text = f.SelectedPath;

            }
        }

        private void uiSymbolButton3_Click_1(object sender, EventArgs e)
        {

        }

        private void uiSymbolButton4_Click_1(object sender, EventArgs e)
        {

        }
    }
}
