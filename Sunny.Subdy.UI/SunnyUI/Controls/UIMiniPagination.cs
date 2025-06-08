﻿/******************************************************************************
 * SunnyUI 开源控件库、工具类库、扩展类库、多页面开发框架。
 * CopyRight (C) 2012-2024 ShenYongHua(沈永华). - VIETNAMESE: T.ME/TRANTAIDAKLAK
 * QQ群：56829229 QQ：17612584 EMail：SunnyUI@QQ.Com
 *
 * Blog:   https://www.cnblogs.com/yhuse
 * Gitee:  https://gitee.com/yhuse/SunnyUI
 * GitHub: https://github.com/yhuse/SunnyUI
 *
 * SunnyUI can be used for free under the GPL-3.0 license.
 * If you use this code, please keep this note.
 * 如果您使用此代码，请保留此说明。
 ******************************************************************************
 * 文件名称: UIMiniPagination.cs
 * 文件说明: 分页
 * 当前版本: V3.1
 * 创建日期: 2023-02-19
 *
 * 2023-02-19: V3.3.2 新增迷你分页控件，只有分页按钮，无其他
 * 2023-06-14: V3.3.9 按钮图标位置修正
 * 2023-06-27: V3.3.9 内置按钮关联值由Tag改为TagString
 * 2023-08-30: V3.4.2 左右跳转按钮的文字换成字体图标
 * 2024-05-29: V3.6.6 优化按钮自定义配色逻辑
******************************************************************************/

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sunny.UI
{
    public class UIMiniPagination : UIPanel
    {
        public delegate void OnPageChangeEventHandler(object sender, object pagingSource, int pageIndex, int count);

        private int activePage = 1;
        private UISymbolButton b0;
        private UISymbolButton b1;
        private UISymbolButton b10;
        private UISymbolButton b11;
        private UISymbolButton b12;
        private UISymbolButton b13;
        private UISymbolButton b14;
        private UISymbolButton b15;
        private UISymbolButton b16;
        private UISymbolButton b2;
        private UISymbolButton b3;
        private UISymbolButton b4;
        private UISymbolButton b5;
        private UISymbolButton b6;
        private UISymbolButton b7;
        private UISymbolButton b8;
        private UISymbolButton b9;

        private readonly ConcurrentDictionary<int, UISymbolButton> buttons = new ConcurrentDictionary<int, UISymbolButton>();
        private readonly ConcurrentDictionary<UISymbolButton, int> buttonTags = new ConcurrentDictionary<UISymbolButton, int>();
        private CurrencyManager dataManager;

        private object dataSource;

        private bool inSetDataConnection;

        /// <summary>
        /// 总页数
        /// </summary>
        [Browsable(false)]
        [Description("总页数"), Category("SunnyUI")]
        public int PageCount { get; private set; }

        private int pagerCount = 9;

        private int pageSize = 20;

        private int totalCount = 1000;

        public UIMiniPagination()
        {
            InitializeComponent();

            SetStyleFlags(true, false);

            ShowText = false;
            buttons.TryAdd(0, b0);
            buttons.TryAdd(1, b1);
            buttons.TryAdd(2, b2);
            buttons.TryAdd(3, b3);
            buttons.TryAdd(4, b4);
            buttons.TryAdd(5, b5);
            buttons.TryAdd(6, b6);
            buttons.TryAdd(7, b7);
            buttons.TryAdd(8, b8);
            buttons.TryAdd(9, b9);
            buttons.TryAdd(10, b10);
            buttons.TryAdd(11, b11);
            buttons.TryAdd(12, b12);
            buttons.TryAdd(13, b13);
            buttons.TryAdd(14, b14);
            buttons.TryAdd(15, b15);
            buttons.TryAdd(16, b16);

            for (var i = 0; i < 17; i++)
            {
                buttons[i].MouseEnter += UIDataGridPage_MouseEnter;
                buttons[i].MouseLeave += UIDataGridPage_MouseLeave;
                buttons[i].Click += UIDataGridPage_Click;
                buttons[i].Size = new System.Drawing.Size(32, 32);
            }

            buttonTags.TryAdd(b0, -1);
            buttonTags.TryAdd(b16, 1);
            for (var i = 0; i < 17; i++)
            {
                if (buttonTags.NotContainsKey(buttons[i]))
                    buttonTags.TryAdd(buttons[i], 0);
            }

            TotalCount = 1000;
        }

        /// <summary>
        /// 重载字体变更
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (DefaultFontSize < 0)
            {
                foreach (var item in this.GetControls<UISymbolButton>(true))
                    item.Font = Font;
            }
        }

        public override void SetDPIScale()
        {
            base.SetDPIScale();
            if (DesignMode) return;
            if (!UIDPIScale.NeedSetDPIFont()) return;

            foreach (var item in this.GetControls<UISymbolButton>(true)) item.SetDPIScale();
            foreach (var item in this.GetControls<UITextBox>(true)) item.SetDPIScale();
            foreach (var item in this.GetControls<UIComboBox>(true)) item.SetDPIScale();
            foreach (var item in this.GetControls<UILabel>(true)) item.SetDPIScale();
        }

        private int buttonInterval = 8;

        [DefaultValue(8)]
        [Description("按钮间隔")]
        [Category("SunnyUI")]
        public int ButtonInterval
        {
            get => buttonInterval;
            set
            {
                buttonInterval = Math.Max(0, value);
                buttonInterval = Math.Min(5, value);
                SetShowButtons();
            }
        }

        /// <summary>
        ///     总条目数
        /// </summary>
        [Description("总条目数")]
        [Category("SunnyUI")]
        public int TotalCount
        {
            get => totalCount;
            set
            {
                totalCount = Math.Max(0, value);
                SetShowButtons();
                if (ActivePage > PageCount)
                    ActivePage = 1;
            }
        }

        /// <summary>
        ///     每页显示条目个数
        /// </summary>
        [DefaultValue(20)]
        [Description("每页显示条目个数")]
        [Category("SunnyUI")]
        public int PageSize
        {
            get => pageSize;
            set
            {
                if (pageSize != value)
                {
                    pageSize = Math.Max(1, value);
                    SetShowButtons();
                    DataBind();
                }
            }
        }

        /// <summary>
        ///     页码按钮的数量，当总页数超过该值时会折叠
        ///     大于等于 5 且小于等于 13 的奇数
        /// </summary>
        [DefaultValue(9)]
        [Description("页码按钮的数量，当总页数超过该值时会折叠，大于等于5且小于等于13的奇数")]
        [Category("SunnyUI")]
        public int PagerCount
        {
            get => pagerCount;
            set
            {
                if (pagerCount != value)
                {
                    pagerCount = Math.Max(5, value);
                    pagerCount = Math.Min(13, pagerCount);
                    if (pagerCount.Mod(2) == 0)
                        pagerCount = pagerCount - 1;

                    SetShowButtons();
                    DataBind();
                }
            }
        }

        /// <summary>
        ///     选中页面
        /// </summary>
        [DefaultValue(1)]
        [Description("选中页面")]
        [Category("SunnyUI")]
        public int ActivePage
        {
            get => activePage;
            set
            {
                activePage = Math.Max(1, value);
                //edtPage.IntValue = activePage;
                SetShowButtons();
                DataBind();
            }
        }

        public event EventHandler DataSourceChanged;

        [DefaultValue(null)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [AttributeProvider(typeof(IListSource))]
        [Description("数据源"), Category("SunnyUI")]
        public object DataSource
        {
            get => dataSource;
            set
            {
                if (value != null)
                {
                    if (!(value is DataTable || value is IList))
                    {
                        throw new Exception(UIStyles.CurrentResources.GridDataSourceException);
                    }
                }

                SetDataConnection(value, new BindingMemberInfo(""));
                dataSource = value;
                activePage = 1;
                TotalCount = dataManager?.List.Count ?? 0;
                DataSourceChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Browsable(false)]
        public object PageDataSource { get; private set; }

        private void UIDataGridPage_Click(object sender, EventArgs e)
        {
            var btn = (UISymbolButton)sender;
            btn.BringToFront();
            if (btn.TagString.IsValid())
                ActivePage += buttonTags[btn];
            else
                ActivePage = buttonTags[btn];
        }

        private void UIDataGridPage_MouseLeave(object sender, EventArgs e)
        {
            var btn = (UISymbolButton)sender;
            if (btn.TagString == "<<" || btn.TagString == ">>")
            {
                btn.Symbol = 361761;
                btn.Text = "";
            }
        }

        private void UIDataGridPage_MouseEnter(object sender, EventArgs e)
        {
            var btn = (UISymbolButton)sender;
            if (btn.TagString == "<<")
            {
                btn.Symbol = 361696;
                btn.Text = "";
            }

            if (btn.TagString == ">>")
            {
                btn.Symbol = 361697;
                btn.Text = "";
            }
        }

        #region InitializeComponent

        private void InitializeComponent()
        {
            b0 = new UISymbolButton();
            b1 = new UISymbolButton();
            b3 = new UISymbolButton();
            b2 = new UISymbolButton();
            b7 = new UISymbolButton();
            b6 = new UISymbolButton();
            b5 = new UISymbolButton();
            b4 = new UISymbolButton();
            b15 = new UISymbolButton();
            b14 = new UISymbolButton();
            b13 = new UISymbolButton();
            b12 = new UISymbolButton();
            b11 = new UISymbolButton();
            b10 = new UISymbolButton();
            b9 = new UISymbolButton();
            b8 = new UISymbolButton();
            b16 = new UISymbolButton();
            SuspendLayout();
            // 
            // b0
            // 
            b0.Cursor = Cursors.Hand;
            b0.Font = new Font("宋体", 10.5F);
            b0.ImageAlign = ContentAlignment.MiddleLeft;
            b0.Location = new Point(3, 4);
            b0.MinimumSize = new Size(1, 1);
            b0.Name = "b0";
            b0.Padding = new Padding(5, 0, 5, 0);
            b0.Radius = 2;
            b0.Size = new Size(32, 32);
            b0.Symbol = 61700;
            b0.TabIndex = 0;
            b0.TagString = "<";
            b0.TextAlign = ContentAlignment.MiddleRight;
            b0.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b1
            // 
            b1.Cursor = Cursors.Hand;
            b1.Font = new Font("Segoe UI", 9F);
            b1.Location = new Point(81, 4);
            b1.MinimumSize = new Size(1, 1);
            b1.Name = "b1";
            b1.Radius = 2;
            b1.Size = new Size(29, 29);
            b1.Symbol = 0;
            b1.TabIndex = 1;
            b1.Text = "0";
            b1.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b3
            // 
            b3.Cursor = Cursors.Hand;
            b3.Font = new Font("Segoe UI", 9F);
            b3.Location = new Point(145, 4);
            b3.MinimumSize = new Size(1, 1);
            b3.Name = "b3";
            b3.Radius = 2;
            b3.Size = new Size(29, 29);
            b3.Symbol = 0;
            b3.TabIndex = 3;
            b3.Text = "0";
            b3.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b2
            // 
            b2.Cursor = Cursors.Hand;
            b2.Font = new Font("Segoe UI", 9F);
            b2.Location = new Point(113, 4);
            b2.MinimumSize = new Size(1, 1);
            b2.Name = "b2";
            b2.Radius = 2;
            b2.Size = new Size(29, 29);
            b2.Symbol = 0;
            b2.TabIndex = 2;
            b2.Text = "0";
            b2.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b7
            // 
            b7.Cursor = Cursors.Hand;
            b7.Font = new Font("Segoe UI", 9F);
            b7.Location = new Point(273, 4);
            b7.MinimumSize = new Size(1, 1);
            b7.Name = "b7";
            b7.Radius = 2;
            b7.Size = new Size(29, 29);
            b7.Symbol = 0;
            b7.TabIndex = 7;
            b7.Text = "0";
            b7.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b6
            // 
            b6.Cursor = Cursors.Hand;
            b6.Font = new Font("Segoe UI", 9F);
            b6.Location = new Point(241, 4);
            b6.MinimumSize = new Size(1, 1);
            b6.Name = "b6";
            b6.Radius = 2;
            b6.Size = new Size(29, 29);
            b6.Symbol = 0;
            b6.TabIndex = 6;
            b6.Text = "0";
            b6.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b5
            // 
            b5.Cursor = Cursors.Hand;
            b5.Font = new Font("Segoe UI", 9F);
            b5.Location = new Point(209, 4);
            b5.MinimumSize = new Size(1, 1);
            b5.Name = "b5";
            b5.Radius = 2;
            b5.Size = new Size(29, 29);
            b5.Symbol = 0;
            b5.TabIndex = 5;
            b5.Text = "0";
            b5.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b4
            // 
            b4.Cursor = Cursors.Hand;
            b4.Font = new Font("Segoe UI", 9F);
            b4.Location = new Point(177, 4);
            b4.MinimumSize = new Size(1, 1);
            b4.Name = "b4";
            b4.Radius = 2;
            b4.Size = new Size(29, 29);
            b4.Symbol = 0;
            b4.TabIndex = 4;
            b4.Text = "0";
            b4.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b15
            // 
            b15.Cursor = Cursors.Hand;
            b15.Font = new Font("Segoe UI", 9F);
            b15.Location = new Point(529, 4);
            b15.MinimumSize = new Size(1, 1);
            b15.Name = "b15";
            b15.Radius = 2;
            b15.Size = new Size(29, 29);
            b15.Symbol = 0;
            b15.TabIndex = 15;
            b15.Text = "0";
            b15.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b14
            // 
            b14.Cursor = Cursors.Hand;
            b14.Font = new Font("Segoe UI", 9F);
            b14.Location = new Point(497, 4);
            b14.MinimumSize = new Size(1, 1);
            b14.Name = "b14";
            b14.Radius = 2;
            b14.Size = new Size(29, 29);
            b14.Symbol = 0;
            b14.TabIndex = 14;
            b14.Text = "0";
            b14.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b13
            // 
            b13.Cursor = Cursors.Hand;
            b13.Font = new Font("Segoe UI", 9F);
            b13.Location = new Point(465, 4);
            b13.MinimumSize = new Size(1, 1);
            b13.Name = "b13";
            b13.Radius = 2;
            b13.Size = new Size(29, 29);
            b13.Symbol = 0;
            b13.TabIndex = 13;
            b13.Text = "0";
            b13.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b12
            // 
            b12.Cursor = Cursors.Hand;
            b12.Font = new Font("Segoe UI", 9F);
            b12.Location = new Point(433, 4);
            b12.MinimumSize = new Size(1, 1);
            b12.Name = "b12";
            b12.Radius = 2;
            b12.Size = new Size(29, 29);
            b12.Symbol = 0;
            b12.TabIndex = 12;
            b12.Text = "0";
            b12.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b11
            // 
            b11.Cursor = Cursors.Hand;
            b11.Font = new Font("Segoe UI", 9F);
            b11.Location = new Point(401, 4);
            b11.MinimumSize = new Size(1, 1);
            b11.Name = "b11";
            b11.Radius = 2;
            b11.Size = new Size(29, 29);
            b11.Symbol = 0;
            b11.TabIndex = 11;
            b11.Text = "0";
            b11.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b10
            // 
            b10.Cursor = Cursors.Hand;
            b10.Font = new Font("Segoe UI", 9F);
            b10.Location = new Point(369, 4);
            b10.MinimumSize = new Size(1, 1);
            b10.Name = "b10";
            b10.Radius = 2;
            b10.Size = new Size(29, 29);
            b10.Symbol = 0;
            b10.TabIndex = 10;
            b10.Text = "0";
            b10.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b9
            // 
            b9.Cursor = Cursors.Hand;
            b9.Font = new Font("Segoe UI", 9F);
            b9.Location = new Point(337, 4);
            b9.MinimumSize = new Size(1, 1);
            b9.Name = "b9";
            b9.Radius = 2;
            b9.Size = new Size(29, 29);
            b9.Symbol = 0;
            b9.TabIndex = 9;
            b9.Text = "0";
            b9.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b8
            // 
            b8.Cursor = Cursors.Hand;
            b8.Font = new Font("Segoe UI", 9F);
            b8.Location = new Point(305, 4);
            b8.MinimumSize = new Size(1, 1);
            b8.Name = "b8";
            b8.Radius = 2;
            b8.Size = new Size(29, 29);
            b8.Symbol = 0;
            b8.TabIndex = 8;
            b8.Text = "0";
            b8.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // b16
            // 
            b16.Cursor = Cursors.Hand;
            b16.Font = new Font("宋体", 10.5F);
            b16.ImageAlign = ContentAlignment.MiddleRight;
            b16.Location = new Point(561, 4);
            b16.MinimumSize = new Size(1, 1);
            b16.Name = "b16";
            b16.Padding = new Padding(6, 0, 5, 0);
            b16.Radius = 2;
            b16.Size = new Size(32, 32);
            b16.Symbol = 61701;
            b16.TabIndex = 16;
            b16.TagString = ">";
            b16.TextAlign = ContentAlignment.MiddleLeft;
            b16.TipsFont = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, 134);
            // 
            // UIMiniPagination
            // 
            Controls.Add(b16);
            Controls.Add(b15);
            Controls.Add(b14);
            Controls.Add(b13);
            Controls.Add(b12);
            Controls.Add(b11);
            Controls.Add(b10);
            Controls.Add(b9);
            Controls.Add(b8);
            Controls.Add(b7);
            Controls.Add(b6);
            Controls.Add(b5);
            Controls.Add(b4);
            Controls.Add(b3);
            Controls.Add(b2);
            Controls.Add(b1);
            Controls.Add(b0);
            Name = "UIMiniPagination";
            RadiusSides = UICornerRadiusSides.None;
            RectSides = ToolStripStatusLabelBorderSides.None;
            Size = new Size(641, 40);
            ResumeLayout(false);
        }

        #endregion InitializeComponent

        private void SetShowButton(int buttonIdx, int pageIdx, int activeIdx)
        {
            buttons[buttonIdx].Symbol = 0;
            buttonTags[buttons[buttonIdx]] = pageIdx;
            buttons[buttonIdx].Visible = true;
            buttons[buttonIdx].TagString = "";
            buttons[buttonIdx].Selected = activeIdx == pageIdx;
            SetButtonWidth(buttons[buttonIdx], pageIdx.ToString());
            buttons[buttonIdx].Left = buttons[buttonIdx - 1].Right + buttonInterval - 1;
            if (buttons[buttonIdx].Selected) buttons[buttonIdx].BringToFront();
        }

        private void SetShowButton(int buttonIdx, int addCount, string tagString)
        {
            buttons[buttonIdx].Symbol = 361761;
            buttons[buttonIdx].Text = "";
            buttons[buttonIdx].SymbolOffset = new Point(-1, 1);
            buttonTags[buttons[buttonIdx]] = addCount;
            buttons[buttonIdx].Visible = true;
            buttons[buttonIdx].TagString = tagString;
            buttons[buttonIdx].Selected = false;
            buttons[buttonIdx].Left = buttons[buttonIdx - 1].Right + buttonInterval - 1;
        }

        private void SetButtonWidth(UISymbolButton button, string text)
        {
            if (button.Text != text) button.Text = text;
            int len = 32;
            if (button.Text.Length >= 3) len = 36;
            if (button.Text.Length >= 4) len = 44;
            if (button.Text.Length >= 4) len = 52;
            if (button.Width != len) button.Width = len;
        }

        private void SetHideButton(int beginIdx)
        {
            for (var i = beginIdx; i < 16; i++) buttons[i].Visible = false;
        }

        private void SetShowButtons()
        {
            b0.Visible = true;
            b16.Visible = true;

            PageCount = TotalCount.Mod(PageSize) == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
            //edtPage.HasMaximum = true;
            //edtPage.Maximum = PageCount;

            if (activePage >= PageCount)
            {
                activePage = PageCount;
                b16.Enabled = false;
            }
            else
            {
                b16.Enabled = true;
            }
            if (activePage <= 1)
            {
                activePage = 1;
                b0.Enabled = false;
            }
            else
            {
                b0.Enabled = true;
            }

            b1.Left = b0.Right + buttonInterval - 1;
            //edtPage.IntValue = activePage;

            if (TotalCount == 0)
            {
                PageCount = 1;
                activePage = 1;
                SetShowButton(1, 1, 1);
                SetHideButton(2);
                b16.Left = b1.Right + buttonInterval - 1;
                Width = b16.Right + 8;
                return;
            }

            if (PageCount <= PagerCount + 2)
            {
                for (var i = 1; i <= PageCount; i++)
                    SetShowButton(i, i, activePage);

                b16.Left = buttons[PageCount].Right + buttonInterval - 1;
                Width = b16.Right + 8;
                SetHideButton(PageCount + 1);
            }
            else
            {
                //左
                var leftShow = PagerCount / 2 + 1 + 2;
                if (activePage <= leftShow)
                {
                    for (var i = 1; i <= leftShow; i++) SetShowButton(i, i, activePage);

                    SetShowButton(leftShow + 1, PagerCount - 2, ">>");
                    SetShowButton(leftShow + 2, PageCount, activePage);
                    SetHideButton(leftShow + 3);
                    b16.Left = buttons[leftShow + 2].Right + buttonInterval - 1;
                    Width = b16.Right + 8;
                    return;
                }

                //右
                var rightShow = PageCount - (PagerCount / 2 + 1) - 1;
                if (activePage >= rightShow)
                {
                    SetShowButton(1, 1, activePage);
                    SetShowButton(2, 2 - PagerCount, "<<");

                    var idx = 3;
                    for (var i = rightShow; i <= PageCount; i++)
                    {
                        SetShowButton(idx, i, activePage);
                        idx++;
                    }

                    b16.Left = buttons[idx - 1].Right + buttonInterval - 1;
                    Width = b16.Right + 8;
                    SetHideButton(idx);
                    return;
                }

                //中
                SetShowButton(1, 1, activePage);
                SetShowButton(2, 2 - PagerCount, "<<");
                var cIdx = 3;
                var sIdx = (PagerCount - 2) / 2;
                for (var i = cIdx; i <= PagerCount; i++)
                {
                    SetShowButton(cIdx, activePage - sIdx + (i - 3), activePage);
                    cIdx++;
                }

                SetShowButton(cIdx, PagerCount - 2, ">>");
                SetShowButton(cIdx + 1, PageCount, activePage);
                b16.Left = buttons[cIdx + 1].Right + buttonInterval - 1;
                Width = b16.Right + 8;
                SetHideButton(cIdx + 2);
            }
        }

        private void SetDataConnection(object newDataSource, BindingMemberInfo newDisplayMember)
        {
            var dataSourceChanged = dataSource != newDataSource;

            if (inSetDataConnection) return;

            try
            {
                if (dataSourceChanged)
                {
                    inSetDataConnection = true;
                    CurrencyManager newDataManager = null;
                    if (newDataSource != null && newDataSource != Convert.DBNull)
                        newDataManager = (CurrencyManager)BindingContext[newDataSource, newDisplayMember.BindingPath];

                    dataManager = newDataManager;
                }
            }
            finally
            {
                inSetDataConnection = false;
            }
        }

        public void DataBind()
        {
            if (dataSource == null)
            {
                PageChanged?.Invoke(this, dataSource, activePage, PageSize);
                return;
            }

            var objects = new List<object>();
            var iBegin = PageSize * (ActivePage - 1);
            for (var i = iBegin; i < iBegin + PageSize; i++)
                if (i < TotalCount)
                    objects.Add(dataManager.List[i]);

            PageDataSource = objects;
            PageChanged?.Invoke(this, objects, activePage, objects.Count);
        }

        public event OnPageChangeEventHandler PageChanged;

        /// <summary>
        /// 设置主题样式
        /// </summary>
        /// <param name="uiColor">主题样式</param>
        public override void SetStyleColor(UIBaseStyle uiColor)
        {
            base.SetStyleColor(uiColor);
            foreach (var button in buttons.Values)
            {
                button.SetStyleColor(uiColor);
                button.FillColor = button.RectColor = uiColor.ButtonFillColor;
                button.SymbolColor = button.ForeColor = uiColor.ButtonForeColor;
                b0.RectSelectedColor = button.FillSelectedColor = uiColor.ButtonFillSelectedColor;
            }
        }

        /// <summary>
        /// 填充颜色，当值为背景色或透明色或空值则不填充
        /// </summary>
        [Description("按钮填充颜色"), Category("SunnyUI")]
        [DefaultValue(typeof(Color), "80, 160, 255")]
        public Color ButtonFillColor
        {
            get => b0.FillColor;
            set
            {
                foreach (var button in buttons.Values)
                {
                    button.RectColor = button.FillColor = value;
                    button.Style = UIStyle.Custom;
                }
            }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        [Description("按钮字体颜色"), Category("SunnyUI")]
        [DefaultValue(typeof(Color), "White")]
        public Color ButtonForeColor
        {
            get => b0.ForeColor;
            set
            {
                foreach (var button in buttons.Values)
                {
                    button.SymbolColor = button.ForeColor = value;
                    button.Style = UIStyle.Custom;
                }
            }
        }

        [DefaultValue(typeof(Color), "115, 179, 255"), Category("SunnyUI")]
        [Description("按钮鼠标移上时填充颜色")]
        public Color ButtonFillHoverColor
        {
            get => b0.FillHoverColor;
            set
            {
                foreach (var button in buttons.Values)
                {
                    button.RectHoverColor = button.FillHoverColor = value;
                    button.Style = UIStyle.Custom;
                }
            }
        }

        [DefaultValue(typeof(Color), "White"), Category("SunnyUI")]
        [Description("按钮鼠标移上时字体颜色")]
        public Color ButtonForeHoverColor
        {
            get => b0.ForeHoverColor;
            set
            {
                foreach (var button in buttons.Values)
                {
                    button.SymbolHoverColor = button.ForeHoverColor = value;
                    button.Style = UIStyle.Custom;
                }
            }
        }

        [DefaultValue(typeof(Color), "64, 128, 204"), Category("SunnyUI")]
        [Description("按钮鼠标按下时填充颜色")]
        public Color ButtonFillPressColor
        {
            get => b0.FillPressColor;
            set
            {
                foreach (var button in buttons.Values)
                {
                    button.RectPressColor = button.FillPressColor = value;
                    button.Style = UIStyle.Custom;
                }
            }
        }

        [DefaultValue(typeof(Color), "White"), Category("SunnyUI")]
        [Description("按钮鼠标按下时字体颜色")]
        public Color ButtonForePressColor
        {
            get => b0.ForePressColor;
            set
            {
                foreach (var button in buttons.Values)
                {
                    button.SymbolPressColor = button.ForePressColor = value;
                    button.Style = UIStyle.Custom;
                }
            }
        }

        [DefaultValue(typeof(Color), "White"), Category("SunnyUI")]
        [Description("按钮鼠标按下时字体颜色")]
        public Color ButtonFillSelectedColor
        {
            get => b0.FillSelectedColor;
            set
            {
                foreach (var button in buttons.Values)
                {
                    button.RectSelectedColor = button.FillSelectedColor = value;
                    button.Style = UIStyle.Custom;
                }
            }
        }

        /// <summary>
        /// 滚动条主题样式
        /// </summary>
        [DefaultValue(true), Description("滚动条主题样式"), Category("SunnyUI")]
        public bool ButtonStyleInherited
        {
            get => b0 != null && b0.Style == UIStyle.Inherited;
            set
            {
                if (value && b0 != null)
                {
                    foreach (var button in buttons.Values)
                    {
                        button.Style = UIStyle.Inherited;
                    }
                }
            }
        }
    }
}
