using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Controls
{
    public partial class fViewDataGridView : Form
    {
        string Tool;
        public fViewDataGridView(List<string> cases, string name)
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            AddCheckBoxesToFlowLayoutPanel(cases, name);
        }
        private void LoadCheckBox()
        {

        }

        private void FormViewDataGridView_Load(object sender, EventArgs e)
        {

        }
        private void AddCheckBoxesToFlowLayoutPanel(List<string> cases, string name)
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;

            var configFile = $"configs\\{name}.txt";
            Dictionary<string, bool> configLines = new();

            // Đọc cấu hình hiện có và loại bỏ key trùng
            if (File.Exists(configFile))
            {
                foreach (var line in File.ReadAllLines(configFile))
                {
                    if (!line.Contains("|")) continue;
                    var parts = line.Split('|');
                    if (parts.Length != 2) continue;

                    var key = parts[0].Trim();
                    var value = parts[1].Trim().ToLower() == "true";

                    if (!configLines.ContainsKey(key))
                        configLines[key] = value;
                }
            }

            foreach (var item in cases)
            {
                bool isChecked = configLines.TryGetValue(item.Trim(), out bool val) ? val : true;

                CheckBox checkBox = new CheckBox
                {
                    Text = item,
                    AutoSize = true,
                    Size = new Size(247, 19),
                    Checked = isChecked
                };

                // Gắn sự kiện thay đổi để ghi file
                checkBox.CheckedChanged += (s, e) => UpdateConfigFile(name);

                flowLayoutPanel1.Controls.Add(checkBox);
            }
        }

        private void UpdateConfigFile(string name)
        {
            var configFile = $"configs\\{name}.txt";
            var config = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            // Ghi cấu hình hiện tại từ checkbox
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                if (ctrl is CheckBox cb)
                {
                    config[cb.Text.Trim()] = cb.Checked;
                }
            }

            var lines = config.Select(kvp => $"{kvp.Key}|{kvp.Value.ToString().ToLower()}");
            File.WriteAllLines(configFile, lines);
        }
        private void btn_Save_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
