using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoAndroid.Stream
{
    public partial class ScrcpyDisplay : UserControl
    {
        Bitmap bmp;
        bool isTouching = false;
        public DeviceModel Device;
        public Scrcpy scrcpy;
        public ScrcpyDisplay(DeviceModel device)
        {
            Device = device;
            InitializeComponent();
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;

            label1.Text = $"{Device.NameDevice} - {Device.Serial}";
            this.Focus();
        }
        public async Task Start()
        {
            this.scrcpy = new Scrcpy(this);
            scrcpy.VideoStreamDecoder.OnFrame += OnFrame;
            View.MouseDown += OnMouseDown;
            View.MouseUp += OnMouseUp;
            View.MouseMove += OnMouseMove;
            this.KeyDown += MainForm_KeyDown;
            await scrcpy.Start();
        }
        public async Task Stop()
        {
            if (scrcpy != null)
            {

                scrcpy.VideoStreamDecoder.OnFrame -= OnFrame;
                View.MouseDown -= OnMouseDown;
                View.MouseUp -= OnMouseUp;
                View.MouseMove -= OnMouseMove;
                this.KeyDown -= MainForm_KeyDown;

                scrcpy.Close();
            }
            View.Image = Properties.Resources.LamTool;
        }

        public void SetSize(System.Drawing.Size size)
        {
            this.Size = size;
        }
        private void OnFrame(object sender, FrameData frameData)
        {
            _ = HandleFrameAsync(frameData); // Fire-and-forget
        }
        private async Task HandleFrameAsync(FrameData frameData)
        {
            try
            {
                await LoadBitmap(frameData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in HandleFrameAsync: {ex.Message}");
            }
        }
       

        private readonly object bitmapLock = new();

        private async Task LoadBitmap(FrameData frameData)
        {
            try
            {
                BitmapData data = null;
                lock (bitmapLock)
                {
                    if (bmp == null || bmp.Width != frameData.Width || bmp.Height != frameData.Height)
                    {
                        bmp?.Dispose();
                        bmp = new Bitmap(frameData.Width, frameData.Height, PixelFormat.Format32bppArgb);
                    }

                    try
                    {
                        data = bmp.LockBits(
                            new Rectangle(0, 0, frameData.Width, frameData.Height),
                            ImageLockMode.WriteOnly,
                            bmp.PixelFormat);
                    }
                    catch { }

                    try
                    {
                        if (data != null && frameData.Data != null && frameData.Data.Length == data.Stride * data.Height)
                        {
                            Marshal.Copy(frameData.Data.ToArray(), 0, data.Scan0, frameData.Data.Length);
                        }
                    }
                    finally
                    {
                        bmp.UnlockBits(data);
                    }
                }

                if (bmp != null && View != null && !View.IsDisposed)
                {
                    if (View.InvokeRequired)
                        View.Invoke(() => View.Image = (Bitmap)bmp.Clone());
                    else
                        View.Image = (Bitmap)bmp.Clone();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in LoadBitmap: {ex.Message}");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            scrcpy.Close();
        }
        public void Close()
        {
            scrcpy.Close();
        }


        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (scrcpy != null && View.Image != null)
            {
                var position = e.Location;

                if (e.Button == MouseButtons.Right)
                {
                    scrcpy.SendControlCommand(new BackOrScreenOnControlMessage
                    {
                        Action = AndroidKeyEventAction.AKEY_EVENT_ACTION_DOWN
                    });
                    scrcpy.SendControlCommand(new BackOrScreenOnControlMessage
                    {
                        Action = AndroidKeyEventAction.AKEY_EVENT_ACTION_UP
                    });
                }
                else if (e.Button == MouseButtons.Left)
                {
                    isTouching = true;
                    SendTouchCommand(AndroidMotionEventAction.AMOTION_EVENT_ACTION_DOWN, position);
                }
            }
        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (scrcpy != null && isTouching)
            {
                isTouching = false;
                SendTouchCommand(AndroidMotionEventAction.AMOTION_EVENT_ACTION_UP, e.Location);
            }
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (scrcpy != null && isTouching)
            {
                SendTouchCommand(AndroidMotionEventAction.AMOTION_EVENT_ACTION_MOVE, e.Location);
            }
        }
        private void SendTouchCommand(AndroidMotionEventAction action, System.Drawing.Point position)
        {
            if (scrcpy != null && View != null)
            {
                var msg = new TouchEventControlMessage();
                msg.Action = action; ;
                double scaleX = (double)scrcpy.Width / View.Bounds.Width;
                double scaleY = (double)scrcpy.Height / View.Bounds.Height;

                // Tọa độ chuyển đổi
                msg.Position.Point.X = (int)(position.X * scaleX);
                msg.Position.Point.Y = (int)(position.Y * scaleY);

                // Kích thước màn hình thực tế của scrcpy
                msg.Position.ScreenSize.Width = (ushort)scrcpy.Width;
                msg.Position.ScreenSize.Height = (ushort)scrcpy.Height;
                TouchHelper.ScaleToScreenSize(msg.Position, scrcpy.Width, scrcpy.Height);
                scrcpy.SendControlCommand(msg); 
                Debug.WriteLine($"📍 Touch {action} tại ({msg.Position.Point.X}, {msg.Position.Point.Y}), " +
                                $"ScreenSize: {msg.Position.ScreenSize.Width}x{msg.Position.ScreenSize.Height}");
            }
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                e.Handled = true;
                string clipboardText = Clipboard.GetText();
                if (!string.IsNullOrEmpty(clipboardText))
                {
                    foreach (char c in clipboardText)
                    {
                        var msg = new KeycodeControlMessage
                        {
                            KeyCode = KeycodeHelper.ConvertKey((Keys)char.ToUpper(c)),
                            Metastate = AndroidMetastate.AMETA_NONE
                        };
                        scrcpy.SendControlCommand(msg);
                    }
                }
            }
            else
            {
                e.Handled = true;
                var msg = new KeycodeControlMessage
                {
                    KeyCode = KeycodeHelper.ConvertKey(e.KeyCode),
                    Metastate = KeycodeHelper.ConvertModifiers(e.Modifiers)
                };
                scrcpy.SendControlCommand(msg);
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            scrcpy.Close();
            RemoveSelf();
        }
        public void RemoveSelf()
        {
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
                this.Dispose();
            }

        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fDebugDevice fDebugDevice = new fDebugDevice(Device);
            fDebugDevice.Show();
        }

        private void uiSymbolButton1_Click(object sender, EventArgs e)
        {
            scrcpy?.Shell("input keyevent 3");
        }

        private void uiSymbolButton3_Click(object sender, EventArgs e)
        {
            scrcpy?.Shell("input keyevent KEYCODE_APP_SWITCH");
        }

        private void uiSymbolButton2_Click(object sender, EventArgs e)
        {
            scrcpy?.Shell("input keyevent 4");
        }
    }
}
