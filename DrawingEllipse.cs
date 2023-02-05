using System.Drawing;

namespace Сreation
{
    internal class DrawingEllipse : DrawingPen
    {
        public DrawingEllipse(in Color color, in Color backColorPictureBox, int width) : base(color, backColorPictureBox, width) { }

        public override void Draw(in Graphics graphics, Point startPoint, Point endPoint)
        {
            graphics.DrawEllipse(PenDrawing, startPoint.X, startPoint.Y, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
        }
    }
}
