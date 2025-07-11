using Sunny.Subdy.Common.API;
using Sunny.Subdy.Common.API.Jobs;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Logs;
using System.Runtime.InteropServices;
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
        public static void LoadFormatFrom(string formattedString, List<ComboBox> cbxs)
        {
            try
            {
                if (string.IsNullOrEmpty(formattedString))
                {
                    return;

                }
                string[] listFormats = formattedString.Split('|');

                for (int i = 0; i < Math.Min(listFormats.Length, cbxs.Count); i++)
                {
                    try
                    {
                        cbxs[i].Text = listFormats[i];
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public static void LoadConfigColums(DataGridView dgv, List<string> listHide)
        {
            var configFile = $"configs\\{dgv.Name}.txt";
            Dictionary<string, bool> configLines = new();

            if (File.Exists(configFile))
            {
                configLines = File.ReadAllLines(configFile)
                                  .Where(line => !string.IsNullOrWhiteSpace(line) && line.Contains("|"))
                                  .Select(line => line.Split('|'))
                                  .ToDictionary(
                                      parts => parts[0].Trim(),
                                      parts => parts[1].Trim().ToLower() == "true"
                                  );
            }

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                // Mặc định là true nếu không có trong config
                bool visible = configLines.TryGetValue(col.HeaderText.Trim(), out bool value) ? value : true;
                col.Visible = visible;
                if (listHide.Contains(col.HeaderText))
                {
                    col.Visible = false;
                }
            }
        }
    }
}
