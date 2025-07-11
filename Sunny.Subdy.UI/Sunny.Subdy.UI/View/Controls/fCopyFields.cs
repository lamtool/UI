using Sunny.Subdy.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Controls
{
    public partial class fCopyFields : Form
    {
        public bool IsOk = false;
        public List<ComboBox> cbxs = new List<ComboBox>();
        string FormatFileName = "accounts-copy-format.txt";

        public fCopyFields(string jobService = "")
        {
            InitializeComponent(); this.AutoScaleMode = AutoScaleMode.Dpi;
            List<string> listField = Globals.GetFieldsToImportExport();
            for (int i = 0; i < listField.Count - 1; i++)
            {
                ComboBox cbx = new ComboBox();
                cbx.DropDownStyle = ComboBoxStyle.DropDownList;
                cbx.Width = 90;
                cbx.Items.AddRange(listField.ToArray());
                if (i < listField.Count - 1)
                {
                    cbx.SelectedIndex = i + 1;
                }
                cbxs.Add(cbx);
                flowLayoutPanel.Controls.Add(cbx);
            }

            LoadFormatFromFile(FormatFileName, cbxs);
        }
        static string DataFileDir = Path.Join(Directory.GetCurrentDirectory(), "data");
        public void LoadFormatFromFile(string FormatFileName, List<ComboBox> cbxs)
        {
            try
            {
                string FormatFilePath = Path.Join(DataFileDir, FormatFileName);
                if (File.Exists(FormatFilePath))
                {
                    string formattedString = File.ReadAllText(FormatFilePath);
                    string[] listFormats = formattedString.Split('|');

                    for (int i = 0; i < Math.Min(listFormats.Length, cbxs.Count); i++)
                    {
                        try
                        {
                            cbxs[i].Text = listFormats[i];
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            IsOk = true;
            SaveFormatToFile(FormatFileName, cbxs);
            Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (var item in cbxs)
            {
                item.SelectedIndex = 0;
            }
        }
        public void SaveFormatToFile(string FormatFileName, List<ComboBox> cbxs)
        {
            try
            {
                List<string> listFormats = cbxs.Select(cbx => cbx.Text).ToList();
                string formattedString = string.Join("|", listFormats);
                if (!Directory.Exists(DataFileDir))
                {
                    Directory.CreateDirectory(DataFileDir);
                }
                string FormatFilePath = Path.Join(DataFileDir, FormatFileName);
                File.WriteAllText(FormatFilePath, formattedString);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving format: {ex.Message}");
            }
        }

    }
}
