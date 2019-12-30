using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        private double total = 0;
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            total = 0;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);

            }
            
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
            //////// measuring time  ////////
            var watch = System.Diagnostics.Stopwatch.StartNew();
            double answer = ImageOperations.ConstructGraph(ImageMatrix);
            watch.Stop();
            var elapsedSec = (watch.ElapsedMilliseconds)/1000.0;
            total += elapsedSec;
            ////////////////////////////////
            Colors.Text = ImageOperations.numColors.ToString();
            Costs.Text = answer.ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            int maskSize = 1;
            maskSize = Math.Min((int)nudMaskSize.Value, ImageOperations.numColors);
            var watch = System.Diagnostics.Stopwatch.StartNew(); //////// measuring time  ////////
            ImageOperations.colorPalette(maskSize);
            ImageOperations.assignNewColors(ImageMatrix);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            watch.Stop();
            var elapsedSec = (watch.ElapsedMilliseconds) / 1000.0;
            total += elapsedSec;
            textBox1.Text = total.ToString();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Colors_TextChanged(object sender, EventArgs e)
        {

        }

        private void nudMaskSize_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}