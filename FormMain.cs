using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Сreation
{
    public partial class FormMain : Form
    {
        private bool _isClick;
        private Bitmap _picture;
        private Graphics _graphics;
        private Point _startPoint;
        private Point _endPoint;
        private FunctionDrawing _currentFunction;
        private DrawingPen _drawing;
        private int _indexButtonChangeColor;
        private object _activeButtonFunctionsDrawing;

        public FormMain()
        {
            InitializeComponent();
            _picture = new Bitmap(pictureBoxDraw.Width, pictureBoxDraw.Height);
            _graphics = Graphics.FromImage(_picture);
            _graphics.Clear(pictureBoxDraw.BackColor);
            pictureBoxDraw.Image = _picture;
            _isClick = default;
            _startPoint = default;
            _endPoint = default;
            _currentFunction = FunctionDrawing.Pen;
            _drawing = new DrawingPen(panelCurrentColor.BackColor, pictureBoxDraw.BackColor, trackBarWidthPen.Value);
            colorDialogDrawing.FullOpen = true;
            _indexButtonChangeColor = default;
            _activeButtonFunctionsDrawing = buttonBrush;
            buttonBrush.Select();
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (pictureBoxDraw.Size.IsEmpty) { return; }
            if (_picture != null)
            {
                Bitmap previousPicture = _picture;
                _picture = new Bitmap(pictureBoxDraw.Width, pictureBoxDraw.Height);
                _graphics = Graphics.FromImage(_picture);
                _graphics.Clear(pictureBoxDraw.BackColor);
                GraphicsUnit pageUnit = new GraphicsUnit();
                _graphics.DrawImage(previousPicture, previousPicture.GetBounds(ref pageUnit).X, previousPicture.GetBounds(ref pageUnit).Y);
                pictureBoxDraw.Image = _picture;
                previousPicture.Dispose();
            }
        }

        private void PictureBoxDraw_MouseDown(object sender, MouseEventArgs e)
        {
            _isClick = true;
            _startPoint = e.Location;
            switch (_currentFunction)
            {
                case FunctionDrawing.Pen:
                    _drawing.DrawPoint(_graphics, _startPoint, _startPoint);
                    pictureBoxDraw.Refresh();
                    break;
                case FunctionDrawing.Eraser:
                    _drawing.ErasePoint(_graphics, _startPoint, _startPoint);
                    pictureBoxDraw.Refresh();
                    break;
                case FunctionDrawing.GetColorPixel:
                    _drawing.PenDrawing.Color = _picture.GetPixel(e.X, e.Y);
                    panelCurrentColor.BackColor = _drawing.PenDrawing.Color;
                    break;
                default:
                    break;
            }
        }

        private void PictureBoxDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isClick)
            {
                _endPoint = e.Location;
                switch (_currentFunction)
                {
                    case FunctionDrawing.Pen:
                        _drawing.Draw(_graphics, _startPoint, _endPoint);
                        _startPoint = e.Location;
                        pictureBoxDraw.Refresh();
                        break;
                    case FunctionDrawing.Eraser:
                        _drawing.Erase(_graphics, _startPoint, _endPoint);
                        _startPoint = e.Location;
                        pictureBoxDraw.Refresh();
                        break;
                    case FunctionDrawing.Figure:
                        pictureBoxDraw.Refresh();
                        break;
                    default:
                        break;
                }
            }
        }

        private void PictureBoxDraw_MouseUp(object sender, MouseEventArgs e)
        {
            _isClick = false;
            switch (_currentFunction)
            {
                case FunctionDrawing.Figure:
                    _endPoint = e.Location;
                    _drawing.Draw(_graphics, _startPoint, _endPoint);
                    pictureBoxDraw.Refresh();
                    break;
                default:
                    break;
            }
        }

        private void RefreshPixel(in Bitmap bitmap, in Stack<Point> pixel, int x, int y, Color oldColor, Color newColor)
        {
            if (bitmap.GetPixel(x, y) == oldColor)
            {
                pixel.Push(new Point(x, y));
                bitmap.SetPixel(x, y, newColor);
            }
        }

        private void Fill(in Bitmap bitmap, int x, int y, Color newColor)
        {
            Color oldColor = bitmap.GetPixel(x, y);
            if (oldColor.ToArgb() == newColor.ToArgb()) { return; }
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bitmap.SetPixel(x, y, newColor);
            while (pixel.Count > 0)
            {
                Point point = (Point)pixel.Pop();
                if (point.X > 0 && point.Y > 0 && point.X < bitmap.Width - 1 && point.Y < bitmap.Height - 1)
                {
                    RefreshPixel(in bitmap, in pixel, point.X - 1, point.Y, oldColor, newColor);
                    RefreshPixel(in bitmap, in pixel, point.X, point.Y - 1, oldColor, newColor);
                    RefreshPixel(in bitmap, in pixel, point.X + 1, point.Y, oldColor, newColor);
                    RefreshPixel(in bitmap, in pixel, point.X, point.Y + 1, oldColor, newColor);
                }
            }
        }

        private void PictureBoxDraw_Click(object sender, EventArgs e)
        {
            switch (_currentFunction)
            {
                case FunctionDrawing.Fill:
                    Fill(_picture, _startPoint.X, _startPoint.Y, panelCurrentColor.BackColor);
                    pictureBoxDraw.Refresh();
                    break;
                default:
                    break;
            }
        }

        private void PictureBoxDraw_Paint(object sender, PaintEventArgs e)
        {
            if (_isClick)
            {
                switch (_currentFunction)
                {
                    case FunctionDrawing.Figure:
                        Graphics graphics = e.Graphics;
                        _drawing.Draw(graphics, _startPoint, _endPoint);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ButtonColor_Click(object sender, EventArgs e)
        {
            _drawing.PenDrawing.Color = ((Button)sender).BackColor;
            panelCurrentColor.BackColor = _drawing.PenDrawing.Color;
        }

        private void ChangeBackColorButtonColor(in Color newBackColor, int index)
        {
            Button[] arrayButtonColor = new Button[]
            {
                buttonChangingColor1,
                buttonChangingColor2,
                buttonChangingColor3,
                buttonChangingColor4,
                buttonChangingColor5,
                buttonChangingColor6,
                buttonChangingColor7,
                buttonChangingColor8,
                buttonChangingColor9,
                buttonChangingColor10,
            };
            arrayButtonColor[index].BackColor = newBackColor;
            arrayButtonColor[index].FlatAppearance.CheckedBackColor = newBackColor;
            arrayButtonColor[index].FlatAppearance.MouseDownBackColor = newBackColor;
            arrayButtonColor[index].FlatAppearance.MouseOverBackColor = newBackColor;
        }

        private void ButtonPalette_Click(object sender, EventArgs e)
        {
            if (colorDialogDrawing.ShowDialog() == DialogResult.OK)
            {
                _drawing.PenDrawing.Color = colorDialogDrawing.Color;
                panelCurrentColor.BackColor = _drawing.PenDrawing.Color;
                ChangeBackColorButtonColor(_drawing.PenDrawing.Color, _indexButtonChangeColor);
                _indexButtonChangeColor = _indexButtonChangeColor < 9 ? _indexButtonChangeColor + 1 : 0;
            }
        }

        private void TrackBarWidthPen_ValueChanged(object sender, EventArgs e)
        {
            _drawing.PenDrawing.Width = trackBarWidthPen.Value;
            _drawing.Eraser.Width = trackBarWidthPen.Value;
        }

        private void ButtonBrush_Click(object sender, EventArgs e)
        {
            _currentFunction = FunctionDrawing.Pen;
            _drawing = new DrawingPen(panelCurrentColor.BackColor, pictureBoxDraw.BackColor, trackBarWidthPen.Value);
        }

        private void ButtonGetColorPixel_Click(object sender, EventArgs e)
        {
            _currentFunction = FunctionDrawing.GetColorPixel;
        }

        private void ButtonEraser_Click(object sender, EventArgs e)
        {
            _currentFunction = FunctionDrawing.Eraser;
            _drawing = new DrawingPen(panelCurrentColor.BackColor, pictureBoxDraw.BackColor, trackBarWidthPen.Value);
        }

        private void ButtonBucket_Click(object sender, EventArgs e)
        {
            _currentFunction = FunctionDrawing.Fill;
        }

        private void BbuttonLine_Click(object sender, EventArgs e)
        {
            _currentFunction = FunctionDrawing.Figure;
            _drawing = new DrawingPen(panelCurrentColor.BackColor, pictureBoxDraw.BackColor, trackBarWidthPen.Value);
        }

        private void ButtonTriangle_Click(object sender, EventArgs e)
        {
            _currentFunction = FunctionDrawing.Figure;
            _drawing = new DrawingTriangle(panelCurrentColor.BackColor, pictureBoxDraw.BackColor, trackBarWidthPen.Value);
        }

        private void ButtonRectangle_Click(object sender, EventArgs e)
        {
            _currentFunction = FunctionDrawing.Figure;
            _drawing = new DrawingRectangle(panelCurrentColor.BackColor, pictureBoxDraw.BackColor, trackBarWidthPen.Value);
        }

        private void BbuttonEllipse_Click(object sender, EventArgs e)
        {
            _currentFunction = FunctionDrawing.Figure;
            _drawing = new DrawingEllipse(panelCurrentColor.BackColor, pictureBoxDraw.BackColor, trackBarWidthPen.Value);
        }

        private void ButtonChangeColorCanves_Click(object sender, EventArgs e)
        {
            if (colorDialogDrawing.ShowDialog() == DialogResult.OK)
            {
                pictureBoxDraw.BackColor = colorDialogDrawing.Color;
                _graphics.Clear(pictureBoxDraw.BackColor);
                pictureBoxDraw.Refresh();
                _drawing.Eraser.Color = pictureBoxDraw.BackColor;
            }
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            _graphics.Clear(pictureBoxDraw.BackColor);
            pictureBoxDraw.Refresh();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialogPicture.FileName == "")
            {
                saveFileDialogPicture.Filter = "JPG(*.JPG)|*.jpg";
                if (saveFileDialogPicture.ShowDialog() == DialogResult.OK)
                {
                    pictureBoxDraw.Image.Save(saveFileDialogPicture.FileName);
                }
            }
            else
            {
                pictureBoxDraw.Image.Save(saveFileDialogPicture.FileName);
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialogPicture.Filter = "JPG(*.JPG)|*.jpg";
            if (saveFileDialogPicture.ShowDialog() == DialogResult.OK)
            {
                pictureBoxDraw.Image.Save(saveFileDialogPicture.FileName);
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialogPicture.Filter = "JPG(*.JPG)|*.jpg";
            if (openFileDialogPicture.ShowDialog() == DialogResult.OK)
            {
                Bitmap image = new Bitmap(openFileDialogPicture.FileName);
                _graphics.DrawImage(image, 0, 0);
                pictureBoxDraw.Refresh();
                image.Dispose();
            }
        }

        private void AboutProgramToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string message = "Автор: Данчук Дмитро\n" +
                "Студент групи: 2СП-21Б\n\n" +
                "Призначення:\n" +
                "Ця програма створена для малювання малюнків.";
            _ = MessageBox.Show(message, "Про програму", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ButtonsFunctionsDrawing_Enter(object sender, EventArgs e)
        {
            ((Button)_activeButtonFunctionsDrawing).FlatAppearance.BorderSize = 0;
            _activeButtonFunctionsDrawing = sender;
            ((Button)_activeButtonFunctionsDrawing).FlatAppearance.BorderSize = 1;
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                _picture.Dispose();
                _graphics.Dispose();
                _drawing.Dispose();
            }
        }
    }
}
