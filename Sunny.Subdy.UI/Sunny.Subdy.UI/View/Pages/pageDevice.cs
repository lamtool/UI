using Sunny.UI;

namespace Sunny.Subdy.UI.View.Pages
{
    public partial class pageDevice : UIPage
    {
        public ucManagerDevices ManagerDevices;
        public pageDevice()
        {
            InitializeComponent();
            this.Symbol = 558149;
            ManagerDevices = new ucManagerDevices();
            ManagerDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(ManagerDevices);
        }
    }
}
