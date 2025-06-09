using System.Drawing;
using System.Windows.Forms;

namespace Sunny.Subdy.UI.Helper
{
    public class DragHandler
    {
        private bool dragging = false;
        private Point dragStartPoint;
        private Form targetForm;

        public DragHandler(Control triggerControl, Form formToMove)
        {
            targetForm = formToMove;

            triggerControl.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    dragging = true;
                    dragStartPoint = e.Location;
                }
            };

            triggerControl.MouseMove += (s, e) =>
            {
                if (dragging)
                {
                    Point currentScreenPos = triggerControl.PointToScreen(e.Location);
                    targetForm.Location = new Point(
                        currentScreenPos.X - dragStartPoint.X,
                        currentScreenPos.Y - dragStartPoint.Y
                    );
                }
            };

            triggerControl.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    dragging = false;
            };
        }
    }
}
