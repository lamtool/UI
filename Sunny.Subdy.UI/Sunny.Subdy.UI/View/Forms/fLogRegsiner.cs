using Sunny.Subdy.Common.Logs;

namespace Sunny.Subdy.UI.View.Forms
{
    public partial class fLogRegsiner : Form
    {
     
        private HashSet<string> liveSet = new HashSet<string>();
        private HashSet<string> dieSet = new HashSet<string>();
        public fLogRegsiner()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void fLogRegsiner_Load(object sender, EventArgs e)
        {
      
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            uiLabel1.Text = LogManager.LogRegsiner.Count.ToString();
            uiLabel4.Text = liveSet.Count.ToString();
            uiLabel6.Text = dieSet.Count.ToString();
            foreach (var item in LogManager.LogRegsiner)
            {
                if (liveSet.Contains(item) || dieSet.Contains(item))
                    continue;

                if (item.Contains("LIVE"))
                {
                    listView_LIVE.Items.Add(item);
                    liveSet.Add(item);
                }
                else
                {
                    listView_DIE.Items.Add(item);
                    dieSet.Add(item);
                }
            }
        }
    }
}
