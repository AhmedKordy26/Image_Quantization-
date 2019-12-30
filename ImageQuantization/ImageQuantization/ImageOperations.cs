using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
///Algorithms Project
///Intelligent Scissors
///

namespace ImageQuantization
{
    /// <summary>
    /// Holds the pixel color in 3 byte values: red, green and blue
    /// </summary>
    public struct RGBPixel
    {
        public byte red, green, blue;
    }
    public struct RGBPixelD
    {
        public double red, green, blue;
    }


    /// <summary>
    /// Library of static functions that deal with images
    /// </summary>
    public class ImageOperations
    {
        static public int numColors;// O(1)
        static private List<RGBPixel> UniquePixelList;// O(1)
        static private List<Tuple<double, int, int>> edges;// O(1)
        static private int currentNumEdges;// O(1)
        static private RGBPixel[,,] repColor;// O(1)
        static private List<List<int>> groubs;// O(1)

        static public List<Tuple<double, int, int>> get_edges()// O(1)
        {
            return edges;// O(1)
        }
        static public void set_edges(List<Tuple<double, int, int>> X) // O(1)
        {
            currentNumEdges = X.Count; // O(1)
            edges = X; // O(1)
        }
        static public List<RGBPixel> get_UniquePixelList()// O(1)
        {
            return UniquePixelList;// O(1)
        }
        static public void set_UniquePixelList(List<RGBPixel> X)// O(1)
        {
            UniquePixelList = X;// O(1)
        }
        /// <summary>
        /// Open an image and load it into 2D array of colors (size: Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>

        /// <returns>2D array of colors</returns>
        private static double euclideanDistance(RGBPixel first, RGBPixel second)//O(1)
        {
            double x = (first.red - second.red) * (first.red - second.red);//O(1)
            double y = (first.green - second.green) * (first.green - second.green);//O(1)
            double z = (first.blue - second.blue) * (first.blue - second.blue);//O(1)
            return Math.Sqrt(x + y + z);  //~O(1)
        }

        public static double generateMstCost() // O(N^2) 
        {

            double[] costs = new double[numColors + 1];// O(1)   cost of unused nodes 
            bool[] Used = new bool[numColors + 1]; // O(1)
            List<Tuple<double, int, int>> myEdges = new List<Tuple<double, int, int>>(); // O(1)
            costs[0] = 0; //O(1)
            double mstCost = 0; //O(1)
            double minValue;  //O(1)
            for (int i = 0; i < numColors; ++i)  //O(N)
            {
                costs[i] = double.MaxValue; // O(1)    fill all nodes with INF 
            }
            int currentNode = 0, minNode = -1;  //O(1)
            int numUsedNodes = 1;  //O(1)
            while (numUsedNodes < numColors) // O(N^2)
            {
                Used[currentNode] = true; minValue = 1e10; //O(1)
                for (int i = 0; i < numColors; ++i) //O(N)
                {
                    if (Used[i] == true) continue; //O(1)
                    if (euclideanDistance(UniquePixelList[currentNode], UniquePixelList[i]) < costs[i]) //O(1)
                    {
                        costs[i] = euclideanDistance(UniquePixelList[currentNode], UniquePixelList[i]); //O(1)
                    }
                    if (costs[i] < minValue)//O(1)
                    {
                        minValue = costs[i]; //O(1)
                        minNode = i; //O(1)
                    }
                }
                mstCost += minValue; //O(1)
                myEdges.Add(Tuple.Create(euclideanDistance(UniquePixelList[minNode], UniquePixelList[currentNode]), currentNode, minNode)); //O(1)
                currentNode = minNode; //O(1)
                numUsedNodes++; //O(1)
            }
            myEdges.Sort();//O(N Log(N))
            set_edges(myEdges); //O(1)
            return mstCost; //O(1)
        }
        public static RGBPixel[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            RGBPixel[,] Buffer = new RGBPixel[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x].red = Buffer[y, x].green = Buffer[y, x].blue = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x].red = p[2];
                            Buffer[y, x].green = p[1];
                            Buffer[y, x].blue = p[0];
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }
        private static List<List<int>> getGroubs(int maskSize) // O(N)
        {
            DSU Uf = new DSU(numColors);// O(N)
            int cur = 0;// O(1)
            while (Uf.numGroubs() > maskSize) //O(N)
            {
                if (Uf.IsSameGroup(edges[cur].Item2, edges[cur].Item3) == false) //O(1)
                {
                    Uf.link(edges[cur].Item2, edges[cur].Item3); //O(1)
                }
                cur++;// O(1)
            }
            return Uf.ConectedComponents(); // O(N)
        }
        public static void colorPalette(int maskSize)
        {
            repColor = new RGBPixel[256, 256, 256]; // O(1)
            groubs = getGroubs(maskSize);  // O(N)
            int curGroub = 0; // O(1)
            RGBPixelD centroid; // O(1)
            for (int i = 0; i < groubs.Count; ++i) // O(N^2)
            {
                curGroub++; // O(1)
                centroid.red = 0; centroid.green = 0; centroid.blue = 0; // O(1)
                for (int j = 0; j < groubs[i].Count; ++j)// O(N)
                {
                    centroid.red += UniquePixelList[groubs[i][j]].red;// O(1)
                    centroid.blue += UniquePixelList[groubs[i][j]].blue;// O(1)
                    centroid.green += UniquePixelList[groubs[i][j]].green;// O(1)
                }
                centroid.red = centroid.red / (groubs[i].Count);// O(1)
                centroid.green = centroid.green / (groubs[i].Count);// O(1)
                centroid.blue = centroid.blue / (groubs[i].Count);// O(1)
                for (int j = 0; j < groubs[i].Count; ++j) // O(N)
                {
                    repColor[UniquePixelList[groubs[i][j]].red, UniquePixelList[groubs[i][j]].green, UniquePixelList[groubs[i][j]].blue].red = (byte)(centroid.red);// O(1)
                    repColor[UniquePixelList[groubs[i][j]].red, UniquePixelList[groubs[i][j]].green, UniquePixelList[groubs[i][j]].blue].green = (byte)(centroid.green);// O(1)
                    repColor[UniquePixelList[groubs[i][j]].red, UniquePixelList[groubs[i][j]].green, UniquePixelList[groubs[i][j]].blue].blue = (byte)(centroid.blue);// O(1)
                }
            }

        }
        public static void assignNewColors(RGBPixel[,] ImageMatrix) //O(W*H)
        {
            int height = GetHeight(ImageMatrix); // O(1)
            int width = GetWidth(ImageMatrix); // O(1)
            for (int x = 0; x < height; ++x)// O( W*H) where N is the width and M is the height
            {
                for (int y = 0; y < width; ++y) //O(W)
                {
                    ImageMatrix[x, y] = repColor[ImageMatrix[x, y].red, ImageMatrix[x, y].green, ImageMatrix[x, y].blue];// O(1)
                }
            }
        }
        
        //////////////////////// Constructing Graph //////
        public static double ConstructGraph(RGBPixel[,] ImageMatrix) // O(N*M) where N is the width and M is the height
        {
            UniquePixelList = new List<RGBPixel>();     //O(1)
            bool[,,] Used = new bool[256, 256, 256];     //O(1)
            int height = GetHeight(ImageMatrix);     //O(1)
            int width = GetWidth(ImageMatrix);     //O(1)
            for (int x = 0; x < height; ++x)    // O(height * width) where N is the width and M is the height
            {
                for (int y = 0; y < width; ++y)  // O(width)
                {
                    if (Used[ImageMatrix[x, y].red, ImageMatrix[x, y].green, ImageMatrix[x, y].blue] == false) //O(1)
                    {
                        Used[ImageMatrix[x, y].red, ImageMatrix[x, y].green, ImageMatrix[x, y].blue] = true; //O(1)
                        UniquePixelList.Add(ImageMatrix[x, y]);     //O(1)
                    }
                }
            }

            numColors = UniquePixelList.Count;//O(1)
            return generateMstCost();// O (N^2)
        }

        //////////////////////////////
        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(RGBPixel[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage(RGBPixel[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[2] = ImageMatrix[i, j].red;
                        p[1] = ImageMatrix[i, j].green;
                        p[0] = ImageMatrix[i, j].blue;
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }


    }
}
