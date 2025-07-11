using Sunny.Subdy.Common.API;
using Sunny.Subdy.Common.API.Jobs;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Models;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Sunny.Subdy.UI.View.Forms.Actions
{
    public partial class fAction_SpamXu : Form
    {
        Common.Json.ConfigHelper _configHelper;
        public string _Name = "";
        public string _Json = "";
        private JsonHelper _jsonHelper;
        public fAction_SpamXu(string name, string json)
        {
            InitializeComponent();
            _Json = json;
            _jsonHelper = new JsonHelper(json, true);
            flowLayoutPanel1.Visible = true;
            flowLayoutPanel1.BringToFront();
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.WrapContents = false;
            LoadJobService();
            txtNameAction.Text = name;
            if (string.IsNullOrEmpty(txtType.Text))
            {
                txtType.SelectedIndex = 0;
            }
            _configHelper = new Common.Json.ConfigHelper(this, _Json, new System.Action(() =>
            {
                checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox2.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox3.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox4.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox5.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox6.CheckedChanged += CheckBox1_CheckedChanged;
                checkBox7.CheckedChanged += CheckBox1_CheckedChanged;
                CheckBox1_CheckedChanged(null, null);
                LoadCheckBox();
            }));


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
        }




        private void LoadCheckBox()
        {
            try
            {
                flowLayoutPanel1.Controls.Clear();

                flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
                flowLayoutPanel1.WrapContents = false;
                flowLayoutPanel1.AutoScroll = true;
                flowLayoutPanel1.Padding = new Padding(5);

                if (string.IsNullOrEmpty(txtKey.Text) || string.IsNullOrEmpty(txtType.Text))
                    return;
                txtType.Watermark = $"Nhập key {txtType.Text}...";

                var lines = JobClient.GetJobTypes(txtType.Text, txtKey.Text.Trim());
                if (!lines.Any())
                {
                    CommonMethod.ShowMessageError("Đã xảy ra lỗi khi lấy loại job, vui lòng thử lại.");
                    return;
                }

                var leftColumn = new FlowLayoutPanel
                {
                    AutoSize = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    // Add a right margin to the left column to push the right column away
                    Margin = new Padding(0, 0, 80, 0), // (left, top, right, bottom)
                    Padding = new Padding(0)
                };

                var rightColumn = new FlowLayoutPanel
                {
                    AutoSize = true,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    Margin = new Padding(0), // No need to change this if leftColumn has right margin
                    Padding = new Padding(0)
                };

                for (int i = 0; i < lines.Count; i++)
                {
                    var control = CreateControl(lines[i]);

                    if (i % 2 == 0)
                        leftColumn.Controls.Add(control);
                    else
                        rightColumn.Controls.Add(control);
                }

                flowLayoutPanel1.Controls.Add(leftColumn);
                flowLayoutPanel1.Controls.Add(rightColumn);
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
            }
        }
        private void LoadJobService()
        {
            txtType.Items.Clear();
            txtType.Items.AddRange(JobServices.Types.ToArray());
            if (string.IsNullOrEmpty(txtKey.Text))
            {
                txtType.SelectedIndex = 0;
            }
        }
        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtKey.Text))
            {
                CommonMethod.ShowMessageError("Vui lòng nhập key", "Lỗi");
                return;
            }
            _Json = _configHelper.GetJsonString();
            _Name = txtNameAction.Text.Trim();
            this.DialogResult = DialogResult.OK;
            Close();
        }
        private Panel CreateControl(string text)
        {
            string key = text.Split(')').Last().Trim();

            var checkBox = new CheckBox
            {
                AutoSize = true,
                Checked = _jsonHelper.GetBooleanValue(key, true),
                Name = key,
                Text = text,
                Location = new Point(5, 5) // Initial position for the checkbox
            };

            var pMain = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = new Padding(5),
                BackColor = Color.Transparent
            };

            pMain.Controls.Add(checkBox);
            int yOffset = checkBox.Location.Y + (checkBox.Height / 2);

            // Calculate horizontal positions
            int minX = checkBox.Location.X + checkBox.Width + 10;
            var min = new NumericUpDown
            {
                Name = "min_" + key,
                Maximum = 20000,
                Value = _jsonHelper.GetIntType(Name, 500),
                Size = new Size(60, 25),
                Location = new Point(minX, yOffset - (25 / 2))
            };

            int labelDenX = min.Location.X + min.Width + 5;
            var label_den = new Label
            {
                Text = "đến:",
                Size = new Size(35, 20),
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(labelDenX, yOffset - (20 / 2))
            };

            int maxX = label_den.Location.X + label_den.Width + 5;
            var max = new NumericUpDown
            {
                Maximum = 20000,
                Name = "max_" + key,
                Value = _jsonHelper.GetIntType(Name, 1000),
                Size = new Size(60, 25),
                Location = new Point(maxX, yOffset - (25 / 2))
            };

            pMain.Controls.Add(min);
            pMain.Controls.Add(label_den);
            pMain.Controls.Add(max);

            checkBox.CheckedChanged += (s, e) =>
            {
                bool enabled = checkBox.Checked;
                label_den.Enabled = enabled;
                min.Enabled = enabled;
                max.Enabled = enabled;
            };

            return pMain;
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
            LoadCheckBox();
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            LoadCheckBox();
        }

        private void fAction_SpamXu_Load(object sender, EventArgs e)
        {

        }

        private void fAction_SpamXu_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}
