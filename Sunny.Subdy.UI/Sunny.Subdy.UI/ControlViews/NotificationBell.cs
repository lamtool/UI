using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Sunny.Subdy.UI.ControlViews
{
    public class NotificationBell : UserControl
    {
        internal Button bellButton;
        private Label badgeLabel;
        private NotificationPopupForm popupForm;
        private List<string> _notifications = new List<string>();

        public List<string> Notifications
        {
            get => _notifications;
            set
            {
                _notifications = value ?? new List<string>();
                UpdateBadge();
                if (popupForm != null)
                    popupForm.SetNotifications(_notifications);
            }
        }

        public NotificationBell()
        {
            this.BackColor = Color.Transparent;

            // Chuông thông báo
            bellButton = new Button
            {
                Text = "🔔",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                TabStop = false,
                Cursor = Cursors.Hand,
                Margin = Padding.Empty
            };
            bellButton.FlatAppearance.BorderSize = 0;
            bellButton.MouseEnter += BellButton_MouseEnter;
            bellButton.MouseLeave += BellButton_MouseLeave;
            this.Controls.Add(bellButton);

            // Label hiển thị số thông báo, chỉ là chữ số nhỏ không nền
            badgeLabel = new Label
            {
                AutoSize = true,
                BackColor = Color.Transparent,  // trong suốt, không viền hay nền
                ForeColor = Color.Red,          // màu chữ đỏ (có thể đổi)
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                TextAlign = ContentAlignment.TopRight,
                Visible = false
            };
            this.Controls.Add(badgeLabel);

            // Form popup thông báo (bạn tự định nghĩa NotificationPopupForm)
            popupForm = new NotificationPopupForm();
            popupForm.SetNotifications(_notifications);
            popupForm.MouseLeave += (s, e) => HidePopup();

            // Bắt sự kiện click ngoài để ẩn popup
            Application.AddMessageFilter(new ClickOutsideDetector(popupForm, this));

            // Đặt kích thước mặc định (bạn có thể chỉnh khi thêm vào Form)
            this.Size = new Size(24, 24);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            int padding = 4;

            bellButton.Size = new Size(this.Width - padding, this.Height - padding);
            bellButton.Location = new Point(padding / 2, padding / 2);

            UpdateBadgePosition();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            UpdateBadgePosition();
        }

        private void UpdateBadgePosition()
        {
            if (bellButton != null && badgeLabel != null && badgeLabel.Visible)
            {
                // Đặt badge nhỏ ở góc trên bên phải chuông, cách chuông 1-2 px
                int x = bellButton.Left + bellButton.Width - badgeLabel.Width - 2;
                int y = bellButton.Top + 2;

                badgeLabel.Location = new Point(x, y);
                badgeLabel.BringToFront();  // đảm bảo badge hiển thị trên nút chuông
            }
        }

        private void BellButton_MouseEnter(object sender, EventArgs e) => ShowPopup();

        private void BellButton_MouseLeave(object sender, EventArgs e)
        {
            Timer t = new Timer { Interval = 300 };
            t.Tick += (s, ev) =>
            {
                t.Stop();
                Point cursorPos = Cursor.Position;
                if (!popupForm.Bounds.Contains(cursorPos) &&
                    !bellButton.RectangleToScreen(bellButton.ClientRectangle).Contains(cursorPos))
                {
                    HidePopup();
                }
            };
            t.Start();
        }

        private void ShowPopup()
        {
            if (_notifications.Count == 0)
                return;

            popupForm.SetNotifications(_notifications);

            Point buttonScreenPos = bellButton.PointToScreen(Point.Empty);
            int popupX = buttonScreenPos.X + bellButton.Width - popupForm.Width;
            int popupY = buttonScreenPos.Y + bellButton.Height + 2;

            Rectangle screenBounds = Screen.FromControl(this).WorkingArea;

            if (popupX + popupForm.Width > screenBounds.Right)
                popupX = screenBounds.Right - popupForm.Width;

            if (popupX < screenBounds.Left)
                popupX = screenBounds.Left;

            if (popupY + popupForm.Height > screenBounds.Bottom)
                popupY = buttonScreenPos.Y - popupForm.Height - 2;

            popupForm.Location = new Point(popupX, popupY);
            popupForm.Show();
            popupForm.BringToFront();
        }

        private void HidePopup()
        {
            if (popupForm.Visible)
                popupForm.Hide();
        }

        private void UpdateBadge()
        {
            if (_notifications.Count > 0)
            {
                badgeLabel.Text = _notifications.Count > 99 ? "99+" : _notifications.Count.ToString();
                badgeLabel.Visible = true;
                UpdateBadgePosition();
                badgeLabel.Invalidate();
            }
            else
            {
                badgeLabel.Visible = false;
                HidePopup();
            }
        }

        // Đồng bộ font chuông nếu cần
        public override Font Font
        {
            get => bellButton?.Font ?? base.Font;
            set
            {
                if (bellButton != null)
                    bellButton.Font = value;
                base.Font = value;
            }
        }
    }

    public class NotificationPopupForm : Form
    {
        private ListView listView;
        private ImageList imgList;

        public NotificationPopupForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Size = new Size(260, 180);
            this.BackColor = Color.WhiteSmoke;
            this.ShowInTaskbar = false;
            this.TopMost = true;

            listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                HeaderStyle = ColumnHeaderStyle.None,
                FullRowSelect = true,
                HideSelection = false,
                MultiSelect = false,
            };
            listView.Columns.Add("", 240);

            imgList = new ImageList { ImageSize = new Size(16, 16), ColorDepth = ColorDepth.Depth32Bit };

            Bitmap tickIcon = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(tickIcon))
            {
                g.Clear(Color.Transparent);
                using Pen pen = new Pen(Color.Green, 2);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLines(pen, new Point[] { new Point(2, 9), new Point(6, 13), new Point(14, 3) });
            }

            Bitmap dotIcon = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(dotIcon))
            {
                g.Clear(Color.Transparent);
                Brush brush = Brushes.DarkGray;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillEllipse(brush, 6, 6, 4, 4);
            }

            imgList.Images.Add("read", tickIcon);
            imgList.Images.Add("unread", dotIcon);
            listView.SmallImageList = imgList;

            this.Controls.Add(listView);
            this.Padding = new Padding(1);
            this.Paint += (s, e) =>
            {
                using Pen pen = new Pen(Color.Gray);
                e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, Width - 1, Height - 1));
            };
        }

        public void SetNotifications(List<string> notifications)
        {
            listView.Items.Clear();
            if (notifications == null || notifications.Count == 0)
            {
                listView.Items.Add(new ListViewItem("Hiện tại không có thông báo nào."));
            }
            else
            {
                foreach (var noti in notifications)
                {
                    bool isRead = noti.Contains("[Đã đọc]");
                    var item = new ListViewItem(noti.Replace("[Đã đọc]", "").Trim())
                    {
                        ImageKey = isRead ? "read" : "unread",
                        ToolTipText = isRead ? "Đã đọc" : "Chưa đọc"
                    };
                    listView.Items.Add(item);
                }
            }
        }

        protected override bool ShowWithoutActivation => true;

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE
                return cp;
            }
        }
    }

    public class ClickOutsideDetector : IMessageFilter
    {
        private readonly NotificationPopupForm popupForm;
        private readonly NotificationBell notificationBell;

        public ClickOutsideDetector(NotificationPopupForm popup, NotificationBell bell)
        {
            popupForm = popup;
            notificationBell = bell;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x201) // WM_LBUTTONDOWN
            {
                Point clickPoint = Control.MousePosition;
                if (!popupForm.Bounds.Contains(clickPoint) &&
                    !notificationBell.bellButton.RectangleToScreen(notificationBell.bellButton.ClientRectangle).Contains(clickPoint))
                {
                    popupForm.Hide();
                }
            }
            return false;
        }
    }
}
