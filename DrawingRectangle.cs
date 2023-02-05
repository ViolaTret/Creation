using System.Drawing;

namespace Сreation
{
    internal class DrawingRectangle : DrawingPen
    {
        public DrawingRectangle(in Color color, in Color backColorPictureBox, int width) : base(color, backColorPictureBox, width) { }

        public override void Draw(in Graphics graphics, Point startPoint, Point endPoint)
        {
            graphics.DrawPolygon(PenDrawing, new Point[]
            {
                new Point(startPoint.X, startPoint.Y),
                new Point(endPoint.X, startPoint.Y),
                new Point(endPoint.X, endPoint.Y),
                new Point(startPoint.X, endPoint.Y)
            });
        }
    }
}
