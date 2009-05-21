using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SeamCarving
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var bitmap = new Bitmap(@"C:\Code\DynamicProgramming\SeamCarving\Images\Desert_VerySimpleLandscape.jpg");
            var newImage = ImagerBitmap.LaplaceGreyscale(bitmap);
            var bounds = new Rectangle(Point.Empty, newImage.Size);

            var imageEneryMatrix = new List<List<float>>(bounds.Width);

            foreach(var xIndex in Enumerable.Range(0, bounds.Width))
            {
                var yIndexVector = new List<float>();
                imageEneryMatrix.Add(yIndexVector);
                foreach(var yIndex in Enumerable.Range(0, bounds.Height))
                {
                    yIndexVector.Add(newImage.GetPixel(xIndex, yIndex).GetBrightness());
                }
            }
        }
    }
}
