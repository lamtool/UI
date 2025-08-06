using Sunny.Subdy.Common.API;
using Sunny.Subdy.Common.API.Jobs;
using Sunny.Subdy.Common.ControlMethod;
using Sunny.Subdy.Common.Helper;
using Sunny.Subdy.Common.Logs;
using Sunny.Subdy.Data.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunny.Subd.Core.Utils
{
    public class LoadControlModelHelper
    {
        public static ToolStrip ToolStripAccount = null;
        public static void LoadToolStripAccount(string platform, string jobservice, string token, ToolStrip toolStripAccount, JobHistoryContext context)
        {
            try
            {
                if (toolStripAccount == null) return;
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(jobservice) && jobservice == JobServices.GoLike)
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
                var stats = context.GetHistorySummaryToDay(platform);
                string Format(string name)
                {
                    switch (name)
                    {
                        case "likepage":
                            name = "Like Page";
                            break;
                        case "joingroup":
                            name = "Join Group";
                            break;
                        case "likecomment":
                            name = "Like Comment";
                            break;
                        case "total":
                            name = "Job Total";
                            break;
                    }
                    if (!stats.ContainsKey(name))
                    {
                        return $"{name.CapitalizeEachWord()}: [ Job: 0/0] - [ Xu: 0/0]";
                    }
                    var s = stats[name];
                    return $"{name.CapitalizeEachWord()}: [ Job: {s.Split("|").First()}] - [ Xu: {s.Split("|").Last()}]";
                }
                if (toolStripAccount.IsHandleCreated)
                {
                    toolStripAccount.Invoke(new Action(() =>
                    {
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
        ("likePageJob00Xu00ToolStripMenuItem", "likepage"),
        ("joinGroupJob00Xu00ToolStripMenuItem", "joingroup"),
        ("likeCommentJob00Xu00ToolStripMenuItem", "likecomment"),
        ("jobTotalJob00Xu00ToolStripMenuItem", "total"),
                        };
                        if (toolStripAccount.Items["toolStripDropDownButton1"] is ToolStripDropDownButton dropDownButton)
                        {
                            foreach (var (itemName, jobType) in jobItems)
                            {
                                var formattedText = Format(jobType.ToLower());
                                var menuItem = dropDownButton.DropDownItems
                                    .OfType<ToolStripMenuItem>()
                                    .FirstOrDefault(m => m.Name == itemName);

                                if (menuItem != null)
                                    menuItem.Text = formattedText;
                            }
                        }

                    }));
                }
                else
                {
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
        ("likePageJob00Xu00ToolStripMenuItem", "likepage"),
        ("joinGroupJob00Xu00ToolStripMenuItem", "joingroup"),
        ("likeCommentJob00Xu00ToolStripMenuItem", "likecomment"),
        ("jobTotalJob00Xu00ToolStripMenuItem", "total"),
                    };
                    if (toolStripAccount.Items["toolStripDropDownButton1"] is ToolStripDropDownButton dropDownButton)
                    {
                        foreach (var (itemName, jobType) in jobItems)
                        {
                            var formattedText = Format(jobType.ToLower());
                            var menuItem = dropDownButton.DropDownItems
                                .OfType<ToolStripMenuItem>()
                                .FirstOrDefault(m => m.Name == itemName);

                            if (menuItem != null)
                                menuItem.Text = formattedText;
                        }
                    }
                }

            }
            catch 
            {

            }
           
            
        }


    }
}
