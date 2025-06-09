using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAndroid
{
    public class RectangleArea
    {
        public int Left { get; private set; }
        public int Top { get; private set; }
        public int Right { get; private set; }
        public int Bottom { get; private set; }

        public RectangleArea(string bounds)
        {
            if (string.IsNullOrEmpty(bounds))
            {
                throw new ArgumentException("Bounds string cannot be null or empty.");
            }


            // Remove square brackets and split the two coordinate pairs
            var parts = bounds.Trim('[', ']').Split(new[] { "][" }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                throw new FormatException("Bounds string must be in the format '[left,top][right,bottom]'.");
            }

            // Parse each coordinate pair
            var topLeft = parts[0].Split(',');
            var bottomRight = parts[1].Split(',');

            if (topLeft.Length != 2 || bottomRight.Length != 2)
            {
                throw new FormatException("Each coordinate pair must contain exactly two integers.");
            }

            // Convert to integers
            Left = int.Parse(topLeft[0]);
            Top = int.Parse(topLeft[1]);
            Right = int.Parse(bottomRight[0]);
            Bottom = int.Parse(bottomRight[1]);
        }

        // Calculate the center point of the rectangle


        // Calculate the center point of the rectangle
        public Point GetCenterPoint()
        {
            int centerX = (Left + Right) / 2;
            int centerY = (Top + Bottom) / 2;
            return new Point(centerX, centerY);
        }

        // Optional: ToString override for debugging purposes
        public override string ToString()
        {
            return $"Rectangle [Left={Left}, Top={Top}, Right={Right}, Bottom={Bottom}]";
        }
    }
}
