using System.Drawing;

namespace Сreation
{
    internal class DrawingPen
    {
        public Pen PenDrawing { get; set; }

        public Pen Eraser { get; set; }

        public DrawingPen(in Color color, in Color backColorPictureBox, int width)
        {
            PenDrawing = new Pen(color, width)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            };
            Eraser = new Pen(backColorPictureBox, width)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Triangle,
                EndCap = System.Drawing.Drawing2D.LineCap.Triangle
            };
        }

        public void DrawPoint(in Graphics graphics, Point startPoint, Point endPoint)
        {
            graphics.DrawLine(PenDrawing, startPoint.X - 0.1F, startPoint.Y - 0.1F, endPoint.X, endPoint.Y);
        }

        public virtual void Draw(in Graphics graphics, Point startPoint, Point endPoint)
        {
            graphics.DrawLine(PenDrawing, startPoint, endPoint);
        }

        public void ErasePoint(in Graphics graphics, Point startPoint, Point endPoint)
        {
            graphics.DrawLine(Eraser, startPoint.X - 0.1F, startPoint.Y - 0.1F, endPoint.X, endPoint.Y);
        }

        public void Erase(in Graphics graphics, Point startPoint, Point endPoint)
        {
            graphics.DrawLine(Eraser, startPoint, endPoint);
        }

        public void Dispose()
        {
            PenDrawing.Dispose();
            Eraser.Dispose();
        }
    }
}
