using Sunny.Subdy.Common.Models;
using Sunny.Subdy.Data.Context;

namespace Sunny.Subdy.UI.View.Forms.Actions
{
    public partial class fAction_RegFB : Form
    {
        private FolderContext _folderContext;
        public fAction_RegFB()
        {
            InitializeComponent();
            _folderContext = new FolderContext();
            new Sunny.Subdy.Common.Json.ConfigHelper(this, this.Name, action: new System.Action(() =>
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
            cbx_Folders.Items.Clear();
            var folders = _folderContext.GetByType("Facebook");
            if (!folders.Any())
            {
                cbx_Folders.Items.Add("Tạo nhóm tài khoản trước khi chạy.");
            }
            else
            {
                foreach (var folder in folders)
                {
                    cbx_Folders.Items.Add(folder.Name);
                }
            }
            if (string.IsNullOrEmpty(cbx_Folders.Text.Trim()))
            {
                cbx_Folders.SelectedIndex = 0;
            }
        }
        private void LoadUI()
        {
            uiComboBox1.Clear();
            uiComboBox1.Items.AddRange(RegistrationType.AllTypes.ToArray());
            if (string.IsNullOrEmpty(uiComboBox1.Text))
            {
                uiComboBox1.SelectedIndex = 0;
            }
            LoadFolder();
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
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {

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
    }
}
