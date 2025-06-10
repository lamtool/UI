using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using AutoAndroid;
using OpenCvSharp;
using Sunny.Subdy.UI.View.DeviceControl;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sunny.Subdy.UI.View.DeviceControl
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
            View.Image = Properties.Resources.LamTool_net;
        }

        public void SetSize(System.Drawing.Size size)
        {
            this.Size = size;
        }
        private async void OnFrame(object sender, FrameData frameData)
        {
            try
            {
                if (View.IsDisposed)
                {
                    return;
                }
                if (View.InvokeRequired)
                {
                    View.Invoke(new Action(() => OnFrame(sender, frameData)));
                    return;
                }
                await LoadBitmap(frameData);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task LoadBitmap(FrameData frameData)
        {
            try
            {
                if (bmp == null || bmp.Width != frameData.Width || bmp.Height != frameData.Height)
                {
                    bmp?.Dispose();
                    bmp = new Bitmap(frameData.Width, frameData.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                }
                BitmapData data = null;
                try
                {
                    data = bmp.LockBits(
                    new Rectangle(0, 0, frameData.Width, frameData.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    bmp.PixelFormat);
                }
                catch
                {

                }
                try
                {
                    if (data != null && frameData.Data != null && frameData.Data.Length == data.Stride * data.Height)
                    {
                        System.Runtime.InteropServices.Marshal.Copy(frameData.Data.ToArray(), 0, data.Scan0, frameData.Data.Length);

                    }
                }
                finally
                {
                    bmp.UnlockBits(data);
                }
                if (bmp != null)
                {
                    await Task.Run(() =>
                    {
                        if (View.InvokeRequired)
                        {
                            View.Invoke(new Action(() => View.Image = bmp));
                        }
                        else
                        {
                            View.Image = bmp;
                        }
                    }).ConfigureAwait(false);
                }

            }
            catch
            {
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            scrcpy.Shell("input keyevent 4");
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

        private async void ScrcpyDisplay_Load(object sender, EventArgs e)
        {

        }

        private void uiHeaderButton2_Click(object sender, EventArgs e)
        {

        }

        private void uiHeaderButton1_Click(object sender, EventArgs e)
        {

        }

        private void uiHeaderButton1_Click_1(object sender, EventArgs e)
        {
            scrcpy.Shell("input keyevent 3");
        }

        private void uiHeaderButton2_Click_1(object sender, EventArgs e)
        {
            scrcpy.Shell("input keyevent KEYCODE_APP_SWITCH");
        }

        private void bellButton_Click(object sender, EventArgs e)
        {
            scrcpy.Shell("input keyevent 4");
        }
    }
}
