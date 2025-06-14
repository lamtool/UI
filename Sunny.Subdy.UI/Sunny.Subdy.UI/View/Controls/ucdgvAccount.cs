using Sunny.Subdy.Data.Models;
using Sunny.Subdy.UI.View.Forms;

namespace Sunny.Subdy.UI.View.Controls
{
    public partial class ucdgvAccount : UserControl
    {
        private Folder _folder;
        public ucdgvAccount(Folder folder)
        {
            InitializeComponent();
            _folder = folder;
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            fAddAccount fAddAccount = new fAddAccount();
            fAddAccount.ShowDialog();
        }
    }
}
