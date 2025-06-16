using System.Xml.Linq;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Json;
using Sunny.Subdy.Common.Models;
using Sunny.UI;

namespace Sunny.Subdy.UI.View.Forms.Actions
{
    public partial class fActioc_SpamXu : Form
    {
        Common.Json.ConfigHelper _configHelper;
        public string _Name;
        public string _Json;
        public fActioc_SpamXu(string name, string json)
        {
            InitializeComponent();
            LoadCheckBox();
            txtNameAction.Text = name;
            if (string.IsNullOrEmpty(txtType.Text))
            {
                txtType.SelectedIndex = 0;
            }
            _configHelper = new Common.Json.ConfigHelper(this, _Json);
        }
        private void LoadCheckBox()
        {
            var list = Globals.ListJobFacebook;
            foreach (var item in list)
            {
                var checkBox = new CheckBox
                {
                    Name = item,
                    Text = item,
                    AutoSize = true,
                    Margin = new Padding(5)
                };
                flowLayoutPanel1.Controls.Add(checkBox);
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
            _Name= txtNameAction.Text.Trim();
            Close();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
