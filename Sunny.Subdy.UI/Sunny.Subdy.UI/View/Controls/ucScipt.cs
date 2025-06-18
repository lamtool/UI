using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;

namespace Sunny.Subdy.UI.View.Controls
{
    public partial class ucScipt : UserControl
    {
        private Script _script;
        public ucScipt(Script script)
        {
            InitializeComponent();
            _script = script;
            LoadScript();
        }
        private void LoadScript()
        {
            this.Name = _script.Name;
            this.Tag = _script.Id; // Store the folder ID in the Tag property for later use
            this.uiLabel1.Text = _script.Name;
            this.uiLabel2.Text = _script.DateCreate;
            this.uiLabel4.Text = _script.Config.Split('|').Count().ToString();
            string type = _script.Type.Trim().ToLower();
            if (type == "facebook")
            {
                this.uiSymbolButton4.Symbol = 161594; // Facebook icon
                uiSymbolButton4.SymbolColor = Color.DodgerBlue;
                uiSymbolButton4.SymbolHoverColor = Color.DodgerBlue;
                uiSymbolButton4.SymbolPressColor = Color.DodgerBlue;
                uiSymbolButton4.SymbolSelectedColor = Color.DodgerBlue;
            }
            else if (type == "instagram")
            {
                this.uiSymbolButton4.Symbol = 61805;
                uiSymbolButton4.SymbolColor = ColorTranslator.FromHtml("#E1306C"); // Màu hồng đặc trưng
                uiSymbolButton4.SymbolHoverColor = ColorTranslator.FromHtml("#FD1D1D"); // Cam
                uiSymbolButton4.SymbolPressColor = ColorTranslator.FromHtml("#833AB4"); // Tím
                uiSymbolButton4.SymbolSelectedColor = ColorTranslator.FromHtml("#FCAF45"); // Vàng
            }
            else if (type == "tiktok")
            {
                this.uiSymbolButton4.Symbol = 157467;
                uiSymbolButton4.SymbolColor = ColorTranslator.FromHtml("#69C9D0");         // Cyan
                uiSymbolButton4.SymbolHoverColor = ColorTranslator.FromHtml("#EE1D52");    // Pink
                uiSymbolButton4.SymbolPressColor = ColorTranslator.FromHtml("#69C9D0");    // Cyan
                uiSymbolButton4.SymbolSelectedColor = ColorTranslator.FromHtml("#EE1D52"); // Pink
            }
            else if (type == "gmail")
            {
                this.uiSymbolButton4.Symbol = 559998;
                uiSymbolButton4.SymbolColor = ColorTranslator.FromHtml("#D93025");         // Đỏ Gmail
                uiSymbolButton4.SymbolHoverColor = ColorTranslator.FromHtml("#1A73E8");     // Xanh dương Google
                uiSymbolButton4.SymbolPressColor = ColorTranslator.FromHtml("#FBBC04");     // Vàng Google
                uiSymbolButton4.SymbolSelectedColor = ColorTranslator.FromHtml("#34A853");  // Xanh lá Google
            }
            else
            {
                this.uiSymbolButton4.Symbol = 559803;
                Color instagramPink = Color.FromArgb(4, 60, 44);
                uiSymbolButton4.SymbolColor = instagramPink;
                uiSymbolButton4.SymbolHoverColor = instagramPink;
                uiSymbolButton4.SymbolPressColor = instagramPink;
                uiSymbolButton4.SymbolSelectedColor = instagramPink;
            }
        }
        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            if (CommonMethod.ShowConfirmWarning($"Bạn có chắc chắn muốn xóa kịch bản [{_script.Name}] ?", "Cảnh báo"))
            {
                new ScriptContext().DeleteById(_script.Id);
                this.Parent.Controls.Remove(this);

            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            fEditScirpt ucFolder = new fEditScirpt(_script);
            ucFolder.ShowDialog();
            _script = new ScriptContext().GetById(_script.Id);
            LoadScript();
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            FEditActionScirpt ucAction = new FEditActionScirpt(_script);
            ucAction.ShowDialog();
            LoadScript();
        }
    }
}
