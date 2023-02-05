using System.Drawing;

namespace Сreation
{
    internal class DrawingTriangle : DrawingPen
    {
        public DrawingTriangle(in Color color, in Color backColorPictureBox, int width) : base(color, backColorPictureBox, width) { }

        public override void Draw(in Graphics graphics, Point startPoint, Point endPoint)
        {
            graphics.DrawPolygon(PenDrawing, new Point[]
            {
                new Point(startPoint.X, endPoint.Y),
                new Point(startPoint.X + ((endPoint.X - startPoint.X) / 2), startPoint.Y),
                new Point(endPoint.X, endPoint.Y)
            });
        }
    }
}
