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
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
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
            ImageOperations.ConstructGraph(ImageMatrix);
            Colors.Text = ImageOperations.get_numColors().ToString();
            Costs.Text = ImageOperations.generateMstCost().ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            ////////////////////////////////// DEBUG
            /* Console.WriteLine(ImageOperations.GetHeight(ImageMatrix));
            Console.WriteLine(ImageOperations.GetWidth(ImageMatrix));
            Console.WriteLine(ImageMatrix[2, 1].blue);
            Console.WriteLine(ImageMatrix[2, 1].green);
            Console.WriteLine(ImageMatrix[2, 1].red);
            for(int x=0;x<ImageOperations.GetHeight(ImageMatrix);++x)
            {
                for (int y = 0; y < ImageOperations.GetWidth(ImageMatrix); ++y)
                {
                    Console.WriteLine(ImageMatrix[x, y].blue);
                    Console.WriteLine(ImageMatrix[x, y].green);
                    Console.WriteLine(ImageMatrix[x, y].red);
                    Console.Write(' ');
                }
                Console.WriteLine();
            }*/
            ImageOperations.ConstructGraph(ImageMatrix);
           
            ////////////////////////////////// DEBUG
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}