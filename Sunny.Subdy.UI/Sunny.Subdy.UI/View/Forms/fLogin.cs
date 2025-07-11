using Sunny.Subdy.Common.API;
using Sunny.Subdy.Common.API.Model;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Models;
using Sunny.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.View.Forms
{
    public partial class fLogin : UIForm
    {
        public fLogin()
        {
            InitializeComponent(); 
            var cached = TempLoginStorage.Load();
            edtUser.Text = cached.Username;
            edtPassword.Text = cached.Password;
            this.Load += FLogin_Load;
        }

        private void FLogin_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(edtUser.Text) && !string.IsNullOrEmpty(edtPassword.Text))
            {
                btnLogin_Click(null, null);
            }
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            OpenLink("https://www.facebook.com/groups/lamtool.net");
        }
        private void OpenLink(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở link: " + ex.Message);
            }
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            OpenLink("https://t.me/lamtool_net");
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            OpenLink("https://www.tiktok.com/@lamtool.net?");
        }

        private void uiSymbolButton4_Click(object sender, EventArgs e)
        {
            OpenLink("https://www.youtube.com/channel/UCJoKRG-V3-QaGGlisVKEscQ");
        }

        private void uiSymbolButton5_Click(object sender, EventArgs e)
        {
            OpenLink("https://zalo.me/g/uubote459");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogin.Enabled = false;
            try
            {
                User user = LamToolClient.Authentication(edtUser.Text.Trim(), edtPassword.Text.Trim());
                Globals.User = user;
                new TempLoginStorage
                {
                    Username = edtUser.Text,
                    Password = edtPassword.Text
                }.Save();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                CommonMethod.ShowMessageError(ex.Message);
                btnLogin.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void uiLinkLabel1_Click(object sender, EventArgs e)
        {
            OpenLink("https://lamtool.net/register");
        }
    }

    public class TempLoginStorage
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        private static string FilePath => Path.Combine(Path.GetTempPath(), "LamTool_LoginCache.json");

        public static TempLoginStorage Load()
        {
            if (!File.Exists(FilePath))
                return new TempLoginStorage();

            try
            {
                var json = File.ReadAllText(FilePath);
                var parts = json.Split('|');
                return new TempLoginStorage
                {
                    Username = parts.ElementAtOrDefault(0) ?? "",
                    Password = parts.ElementAtOrDefault(1) ?? ""
                };
            }
            catch
            {
                return new TempLoginStorage();
            }
        }

        public void Save()
        {
            try
            {
                File.WriteAllText(FilePath, $"{Username}|{Password}");
            }
            catch { }
        }

        public static void Clear()
        {
            try
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
            }
            catch { }
        }
    }
}
