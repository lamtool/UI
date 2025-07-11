using Sunny.Subdy.Common.API.Jobs;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Logs;
using Sunny.Subdy.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Subd.Core.Utils
{
    public class LoadControlModelHelper
    {
        public static ToolStrip ToolStripAccount = null;
        public static void LoadToolStripAccount(string jobservice, string token, ToolStrip toolStripAccount, JobHistoryContext context)
        {
            if (toolStripAccount == null) return;
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(jobservice))
            {
                try
                {
                    (double current_coin, double pending_coin) = JobClient.GetCoin(jobservice, token);
                    var item_Sodu = toolStripAccount.Items["tslBalance"];
                    item_Sodu.Text = current_coin.ToMoneyString();
                    var item_ChoDuyet = toolStripAccount.Items["tslPanding"];
                    item_ChoDuyet.Text = pending_coin.ToMoneyString();
                }
                catch (Exception ex)
                {
                    LogManager.Error(ex);
                    CommonMethod.ShowMessageError(ex.Message);
                }
            }
            var stats = context.GetJobStatisticsSummary(DateTime.Now.ToString("dd/MM/yyyy"));
            string Format(string name)
            {
                var s = stats[name];
                switch (name)
                {
                    case "LikePage":
                        name = "Like Page";
                        break;
                    case "JoinGroup":
                        name = "Join Group";
                        break;
                    case "LikeComment":
                        name = "Like Comment";
                        break;
                    case "Job":
                        name = "Like Total";
                        break;
                }
                return $"{name}: [ Job: {s.JobToday.ToMoneyString()}/{s.JobTotal.ToMoneyString()}] - [ Xu: {s.XuToday.ToMoneyString()}/{s.XuTotal.ToMoneyString()}]";
            }
            var item_SoTienHomNay = toolStripAccount.Items["tslBalanceToday"];
            var item_SoTienBiTru = toolStripAccount.Items["tslDeduct"];
            var jobItems = new (string itemName, string jobType)[]
            {
        ("likeToolStripMenuItem", "Like"),
        ("likeJob00Xu00ToolStripMenuItem", "Love"),
        ("loveJob00Xu00ToolStripMenuItem", "Care"),
        ("hahaJob00Xu00ToolStripMenuItem", "Haha"),
        ("wowJob00Xu00ToolStripMenuItem", "Wow"),
        ("sadJob00Xu00ToolStripMenuItem", "Sad"),
        ("angryJob00Xu00ToolStripMenuItem", "Angry"),
        ("shareJob00Xu00ToolStripMenuItem", "Share"),
        ("followJob00Xu00ToolStripMenuItem", "Follow"),
        ("likePageJob00Xu00ToolStripMenuItem", "LikePage"),
        ("joinGroupJob00Xu00ToolStripMenuItem", "JoinGroup"),
        ("likeCommentJob00Xu00ToolStripMenuItem", "LikeComment"),
        ("jobTotalJob00Xu00ToolStripMenuItem", "Job"),
            };

            if (toolStripAccount.Items["toolStripDropDownButton1"] is ToolStripDropDownButton dropDownButton)
            {
                foreach (var (itemName, jobType) in jobItems)
                {
                    var formattedText = Format(jobType);
                    var menuItem = dropDownButton.DropDownItems
                        .OfType<ToolStripMenuItem>()
                        .FirstOrDefault(m => m.Name == itemName);

                    if (menuItem != null)
                        menuItem.Text = formattedText;
                }
            }
        }


    }
}
