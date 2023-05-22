using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace GraphicalApp2
{
    public partial class Form1 : Form
    {
        private List<PointF> points;

        public Form1()
        {
            InitializeComponent();
            // Initialize the list of points
            var rand  = new Random();
            points = new List<PointF>();
            points.Add(new PointF(10, 50));
            points.Add(new PointF(20, 70));
            points.Add(new PointF(30, 40));
            points.Add(new PointF(40, 80));
            points.Add(new PointF(50, 60));
            points.Add(new PointF(60, 30));
            points.Add(new PointF(70, 90));
            points.Add(new PointF(80, 20));
            points.Add(new PointF(90, 50));
            points.Add(new PointF(100, 70));
            
                 
        }
        private const int numBins = 10;
        private const float BinWidth = 1000f; // Changed from 1000f to match the x-range of data
        private const float XMax = 100f;
        private const float YMax = 100f;
        private const float XAxisHeight = 0.9f;
        private const float BarHeightScale = 0.8f;
        private void DisplayHistogram()
        {
            // Clear the picture box control
            pictureBox.Invalidate();

            // Count the number of points in each bin
            int[] binCounts = points
                .GroupBy(p => (int)(p.X / BinWidth))
                .Select(gr => gr.Count())
                .ToArray();

            // Determine the maximum bin count
            int maxBinCount = binCounts.Max();

            // Draw a bar chart of the bin counts
            var bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            var btmp = pictureBox.Image = bitmap;
            Graphics g = Graphics.FromImage(btmp);

            Pen pen = new Pen(Color.Black, 2f);
            Brush brush = new SolidBrush(Color.Blue);
            
                for (int i = 0; i < binCounts.Length; i++)
                {
                    float barHeight = binCounts[i] / (float)points.Count * pictureBox.Height * BarHeightScale;
                    float x = i * BinWidth / XMax * pictureBox.Width;
                    float y = pictureBox.Height * XAxisHeight - barHeight;
                    Color color;
                    switch (binCounts[i])
                    {
                        case int n when n <= maxBinCount / 4:
                            color = Color.FromArgb(255, 0, 0);
                            break;
                        case int n when n <= maxBinCount / 2:
                            color = Color.Orange;
                            break;
                        case int n when n <= maxBinCount * 3 / 4:
                            color = Color.Yellow;
                            break;
                        default:
                            color = Color.Green;
                            break;
                    }
                   brush = new SolidBrush(color);
                    g.DrawRectangle(pen, x, y, BinWidth / XMax * pictureBox.Width, barHeight);
                    g.FillRectangle(brush, x, y, BinWidth / XMax * pictureBox.Width, barHeight);
                }

                // Draw the x-axis and y-axis
                g.DrawLine(pen, 0, pictureBox.Height * XAxisHeight, pictureBox.Width, pictureBox.Height * XAxisHeight);
                g.DrawLine(pen, 0, 0, 0, pictureBox.Height * XAxisHeight);

                // Draw the y-axis labels
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Far;
                for (int i = 0; i <= 5; i++)
                {
                    float yLabel = i / 5f * maxBinCount;
                    float yCoord = pictureBox.Height * XAxisHeight - i / 5f * pictureBox.Height * BarHeightScale;
                    g.DrawString(yLabel.ToString(), Font, Brushes.Black, 0, yCoord, format);
                    
                }
            }
        


        private void DisplayGraph()
        {
            // Clear the picture box control
            pictureBox.Invalidate();

            // Draw the lines connecting the points
            var bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            var btmp = pictureBox.Image = bitmap;
            Graphics g = Graphics.FromImage(btmp);
            Pen pen = new Pen(Color.Black, 2f);
            for (int i = 1; i < points.Count; i++)
            {
                float x1 = points[i - 1].X / 100f * pictureBox.Width;
                float y1 = (1 - points[i - 1].Y / 100f) * pictureBox.Height;
                float x2 = points[i].X / 100f * pictureBox.Width;
                float y2 = (1 - points[i].Y / 100f) * pictureBox.Height;
                g.DrawLine(pen, x1, y1, x2, y2);
            }

            // Draw the x-axis and y-axis
            g.DrawLine(pen, 0, pictureBox.Height, pictureBox.Width, pictureBox.Height);
            g.DrawLine(pen, 0, 0, 0, pictureBox.Height);

            pen.Dispose();
        }

        private void DisplayScatterPlot()
        {
            // Clear the picture box control
            pictureBox.Invalidate();

            // Draw a point for each data point
            var bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            var btmp = pictureBox.Image = bitmap;
            Graphics g = Graphics.FromImage(btmp);
            Pen pen = new Pen(Color.Black);
            Brush brush = new SolidBrush(Color.Red);
            foreach (PointF point in points)
            {
                float x = point.X / 100f * pictureBox.Width;
                float y = (1 - point.Y / 100f) * pictureBox.Height;
                g.FillEllipse(brush, x - 2, y - 2, 4, 4);
            }

            // Draw the x-axis and y-axis
            g.DrawLine(pen, 0, pictureBox.Height, pictureBox.Width, pictureBox.Height);
            g.DrawLine(pen, 0, 0, 0, pictureBox.Height);

            pen.Dispose();
            brush.Dispose();
        }

        private void DisplayBubbleDiagram()
        {
            pictureBox.Invalidate();

            // Draw a circle for each data point, where the size of the circle represents a third dimension of the data
            var bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            var btmp = pictureBox.Image = bitmap;
            Graphics g = Graphics.FromImage(btmp);
            Pen pen = new Pen(Color.Black);
            foreach (PointF point in points)
            {
                float x = point.X / 100f * pictureBox.Width;
                float y = (1 - point.Y / 100f) * pictureBox.Height;
                float size = point.Y / 100f * 50; // Use the y-coordinate as the size of the circle
                g.DrawEllipse(pen, x - size / 2, y - size / 2, size, size);
            }

            // Draw the x-axis and y-axis
            g.DrawLine(pen, 0, pictureBox.Height, pictureBox.Width, pictureBox.Height);
            g.DrawLine(pen, 0, 0, 0, pictureBox.Height);

            pen.Dispose();
        }


        private void DrawPoints(Graphics g)
        {
            Pen pen = new Pen(Color.Black);
            Brush brush = new SolidBrush(Color.Red);
            float xMax = 100;
            float yMax = 100;

            foreach (PointF point in points)
            {
                float x = point.X / xMax * pictureBox.Width;
                float y = (1 - point.Y / yMax) * pictureBox.Height;
                g.FillEllipse(brush, x - 2, y - 2, 4, 4);
            }

            pen.Dispose();
            brush.Dispose();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            DrawPoints(e.Graphics);
        }

        private void histogramButton_Click(object sender, EventArgs e)
        {
            DisplayHistogram();
        }

        private void graphButton_Click(object sender, EventArgs e)
        {
            DisplayGraph();
        }

        private void scatterPlotButton_Click(object sender, EventArgs e)
        {
            DisplayScatterPlot();
        }

        private void bubbleDiagramButton_Click(object sender, EventArgs e)
        {
            DisplayBubbleDiagram();
        }
    }
}
