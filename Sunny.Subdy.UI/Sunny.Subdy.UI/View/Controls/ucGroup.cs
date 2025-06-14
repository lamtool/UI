using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Data.Context;
using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.View.Forms;
using Sunny.UI;

namespace Sunny.Subdy.UI.View.Controls
{
    public partial class ucGroup : UIUserControl
    {
        Folder _folder;
        public ucGroup(Folder folder)
        {
            InitializeComponent();
            _folder = folder;
            LoadFolder();
        }
        private void LoadFolder()
        {
            this.Name = _folder.Name;
            this.Tag = _folder.Id; // Store the folder ID in the Tag property for later use
            this.uiLabel1.Text = _folder.Name;
            this.uiLabel2.Text = _folder.DateCreate;
            this.uiLabel3.Text = _folder.Count;
            string type = _folder.Type.Trim().ToLower();
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
        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            ucdgvAccount uc = new ucdgvAccount(_folder);
            fShow f = new fShow(uc);
            f.ShowDialog();
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            if (CommonMethod.ShowConfirmWarning($"Bạn có chắc chắn muốn xóa nhóm tài khoản [{_folder.Name}] ?", "Cảnh báo"))
            {
                new FolderContext().DeleteById(_folder.Id);
                this.Parent.Controls.Remove(this);

            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            ucFolder ucFolder = new ucFolder(_folder);
            ucFolder.ShowDialog();
            _folder = new FolderContext().GetById(_folder.Id);
            LoadFolder();
        }
    }
}
