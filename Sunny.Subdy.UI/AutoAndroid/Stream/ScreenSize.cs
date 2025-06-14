using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAndroid.Stream
{
    public enum ControlMessageType : byte
    {
        InjectKeycode,
        InjectText,
        InjectTouchEvent,
        InjectScrollEvent,
        BackOrScreenOn,
        ExpandNotificationPanel,
        ExpandSettingsPanel,
        CollapsePanels,
        GetClipboard,
        SetClipboard,
        SetScreenPowerMode,
        RotateDevice,
    }

    public record ScreenSize
    {
        public ushort Width;
        public ushort Height;
    }

    public record Point
    {
        public int X;
        public int Y;
    }

    public record Position
    {
        public ScreenSize ScreenSize = new();
        public Point Point = new();

        public Span<byte> ToBytes()
        {
            Span<byte> b = new byte[12];
            BinaryPrimitives.WriteInt32BigEndian(b[0..], Point.X);
            BinaryPrimitives.WriteInt32BigEndian(b[4..], Point.Y);
            BinaryPrimitives.WriteUInt16BigEndian(b[8..], ScreenSize.Width);
            BinaryPrimitives.WriteUInt16BigEndian(b[10..], ScreenSize.Height);
            return b;
        }
    }

    public interface IControlMessage
    {
        public ControlMessageType Type
        {
            get;
        }

        Span<byte> ToBytes();
    }

    public class KeycodeControlMessage : IControlMessage
    {
        public ControlMessageType Type => ControlMessageType.InjectKeycode;
        public AndroidKeyEventAction Action
        {
            get; set;
        }
        public AndroidKeycode KeyCode
        {
            get; set;
        }
        public uint Repeat
        {
            get; set;
        }
        public AndroidMetastate Metastate
        {
            get; set;
        }

        public Span<byte> ToBytes()
        {
            Span<byte> b = new byte[14];
            b[0] = (byte)Type;
            b[1] = (byte)Action;
            BinaryPrimitives.WriteInt32BigEndian(b[2..], (int)KeyCode);
            BinaryPrimitives.WriteInt32BigEndian(b[6..], (int)Repeat);
            BinaryPrimitives.WriteInt32BigEndian(b[10..], (int)Metastate);
            return b;
        }


    }

    public class BackOrScreenOnControlMessage : IControlMessage
    {
        public ControlMessageType Type => ControlMessageType.BackOrScreenOn;
        public AndroidKeyEventAction Action
        {
            get; set;
        }

        public Span<byte> ToBytes()
        {
            Span<byte> b = new byte[2];
            b[0] = (byte)Type;
            b[1] = (byte)Action;
            return b;
        }
    }

    public class TouchEventControlMessage : IControlMessage
    {
        public ControlMessageType Type => ControlMessageType.InjectTouchEvent;
        public AndroidMotionEventAction Action
        {
            get; set;
        }
        public AndroidMotionEventButtons Buttons { get; set; } = AndroidMotionEventButtons.AMOTION_EVENT_BUTTON_PRIMARY;
        public ulong PointerId { get; set; } = 0xFFFFFFFFFFFFFFFF;
        public Position Position { get; set; } = new();
        //public float Pressure { get; set; }

        public Span<byte> ToBytes()
        {
            Span<byte> b = new byte[32];
            b[0] = (byte)Type;
            b[1] = (byte)Action;

            ulong safePointerId = 0;
            BinaryPrimitives.WriteUInt64BigEndian(b.Slice(2, 8), safePointerId);

            BinaryPrimitives.WriteInt32BigEndian(b.Slice(10, 4), Position.Point.X);
            BinaryPrimitives.WriteInt32BigEndian(b.Slice(14, 4), Position.Point.Y);
            BinaryPrimitives.WriteUInt16BigEndian(b.Slice(18, 2), Position.ScreenSize.Width);
            BinaryPrimitives.WriteUInt16BigEndian(b.Slice(20, 2), Position.ScreenSize.Height);

            ushort pressure = (ushort)(1.0f * 0xFFFF);
            BinaryPrimitives.WriteUInt16BigEndian(b.Slice(22, 2), pressure);

            int actionButton = 0;
            BinaryPrimitives.WriteInt32BigEndian(b.Slice(24, 4), actionButton);
            int safeButtons = 0;
            BinaryPrimitives.WriteInt32BigEndian(b.Slice(28, 4), safeButtons);

            //System.Diagnostics.Debug.WriteLine($"🛠️ Debug: Buttons = {safeButtons}, PointerId = {safePointerId}, Position = ({Position.Point.X}, {Position.Point.Y})");

            return b;
        }
    }

    public class ScrollEventControlMessage : IControlMessage
    {
        public ControlMessageType Type => ControlMessageType.InjectScrollEvent;
        public Position Position { get; set; } = new Position();
        public float HorizontalScroll
        {
            get; set;
        }
        public float VerticalScroll
        {
            get; set;
        }
        public int Buttons
        {
            get; set;
        }

        public Span<byte> ToBytes()
        {
            Span<byte> b = new byte[21];

            b[0] = (byte)Type;
            Position.ToBytes().CopyTo(b[1..]);

            short hscroll = FloatToInt16FP(HorizontalScroll);
            short vscroll = FloatToInt16FP(VerticalScroll);

            BinaryPrimitives.WriteInt16BigEndian(b[13..], hscroll);
            BinaryPrimitives.WriteInt16BigEndian(b[15..], vscroll);

            BinaryPrimitives.WriteInt32BigEndian(b[17..], Buttons);

            return b;
        }
        public static short FloatToInt16FP(float value)
        {

            if (value < -1.0f || value > 1.0f)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be between -1.0 and 1.0");

            int intValue = (int)(value * (1 << 15));


            if (intValue >= 0x7FFF)
            {
                intValue = 0x7FFF;
            }
            else if (intValue <= -0x8000)
            {
                intValue = -0x8000;
            }

            return (short)intValue;
        }
    }

    public class InjectTextControlMessage : IControlMessage
    {
        public ControlMessageType Type => ControlMessageType.InjectText;
        public string Text { get; set; } = "";

        public Span<byte> ToBytes()
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(Text);
            Span<byte> buffer = new byte[1 + 4 + textBytes.Length];

            buffer[0] = (byte)Type;
            BinaryPrimitives.WriteInt32BigEndian(buffer[1..], textBytes.Length);
            textBytes.CopyTo(buffer[5..]);

            return buffer;
        }
    }
    public class SetClipboardControlMessage : IControlMessage
    {
        public ControlMessageType Type => ControlMessageType.SetClipboard;
        public ulong Sequence
        {
            get; set;
        }
        public bool Paste
        {
            get; set;
        }
        public string Text { get; set; } = string.Empty;

        public Span<byte> ToBytes()
        {
            if (Text == null)
            {
                Text = string.Empty;
            }

            const int MAX_CLIPBOARD_LENGTH = 4096;
            byte[] utf8TextBytes = Encoding.UTF8.GetBytes(Text);
            if (utf8TextBytes.Length > MAX_CLIPBOARD_LENGTH)
            {
                utf8TextBytes = utf8TextBytes[..MAX_CLIPBOARD_LENGTH];
            }

            int textLength = utf8TextBytes.Length;

            int totalLength = 1 + 8 + 1 + 4 + textLength;
            Span<byte> buffer = new byte[totalLength];

            buffer[0] = (byte)Type;
            BinaryPrimitives.WriteUInt64BigEndian(buffer[1..], Sequence);
            buffer[9] = (byte)(Paste ? 1 : 0);
            BinaryPrimitives.WriteInt32BigEndian(buffer[10..], textLength);

            utf8TextBytes.CopyTo(buffer[14..]);

            System.Diagnostics.Debug.WriteLine($"📋 Đã đóng gói clipboard: {textLength} bytes");

            return buffer;
        }
    }
    public class ExpandNotificationPanelControlMessage : IControlMessage
    {
        public ControlMessageType Type => ControlMessageType.ExpandNotificationPanel;

        public Span<byte> ToBytes()
        {
            Span<byte> buffer = new byte[1];
            buffer[0] = (byte)Type;
            return buffer;
        }
    }
}
