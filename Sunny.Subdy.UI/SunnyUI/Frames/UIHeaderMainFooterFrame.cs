﻿/******************************************************************************
 * SunnyUI 开源控件库、工具类库、扩展类库、多页面开发框架。
 * CopyRight (C) 2012-2024 ShenYongHua(沈永华). - VIETNAMESE: T.ME/TRANTAIDAKLAK
 * QQ群：56829229 QQ：17612584 EMail：SunnyUI@QQ.Com
 *
 * Blog:   https://www.cnblogs.com/yhuse
 * Gitee:  https://gitee.com/yhuse/SunnyUI
 * GitHub: https://github.com/yhuse/SunnyUI
 *
 * SunnyUI.dll can be used for free under the GPL-3.0 license.
 * If you use this code, please keep this note.
 * 如果您使用此代码，请保留此说明。
 ******************************************************************************
 * 文件名称: UIHeaderMainFooterFrame.cs
 * 文件说明: 页面框架(Header-Main-Footer)
 * 当前版本: V3.1
 * 创建日期: 2020-05-05
 *
 * 2020-05-05: V2.2.5 页面框架(Header-Main-Footer)
******************************************************************************/

namespace Sunny.UI
{
    public partial class UIHeaderMainFooterFrame : UIHeaderMainFrame
    {
        public UIHeaderMainFooterFrame()
        {
            InitializeComponent();
            Controls.SetChildIndex(MainTabControl, 0);
            Header.Parent = this;
            Footer.Parent = this;
            MainTabControl.Parent = this;
            MainTabControl.BringToFront();
        }
    }
}