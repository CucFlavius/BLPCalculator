using System;

namespace BLPCalculator
{
    class Program
    {
        const int BLP_HEADER_SIZE = 148;
        const int PALLETE_SIZE = 256 * 4;

        static void Main(string[] args)
        {
            Console.Write("Width: ");
            string widthStr = Console.ReadLine();
            if (!int.TryParse(widthStr, out int width)) return;
            Console.Write("Height: ");
            string heightStr = Console.ReadLine();
            if (!int.TryParse(heightStr, out int height)) return;

            Console.WriteLine("Calculating BLP Sizes for a texture with dimensions of " + widthStr + "x" + heightStr + ".");
            Console.WriteLine("--------------------------------------------------------------------------------");

            int[] DXT1Sizes = CalculateDXTSizes(20, width, height, 8);
            int dxt1Total = BLP_HEADER_SIZE;
            for (int i = 0; i < DXT1Sizes.Length; i++)
            {
                dxt1Total += DXT1Sizes[i];
            }
            Console.WriteLine("DXT1 : ".PadRight(20) + GetFileSize(dxt1Total) + "(" + dxt1Total + ")");

            int[] DXT5Sizes = CalculateDXTSizes(20, width, height, 16);
            int dxt5Total = BLP_HEADER_SIZE;
            for (int i = 0; i < DXT5Sizes.Length; i++)
            {
                dxt5Total += DXT5Sizes[i];
            }
            Console.WriteLine("DXT3/DXT5 : ".PadRight(20) + GetFileSize(dxt5Total) + "(" + dxt5Total + ")");

            int palletized = (width * height * 4) + BLP_HEADER_SIZE + PALLETE_SIZE;
            Console.WriteLine("Palletized NoMips : ".PadRight(20) + GetFileSize(palletized) + "(" + palletized + ")");
        }

        public static int[] CalculateDXTSizes(int miplevels, int width, int height, int blockSize)
        {
            int[] DXTSizes = new int[miplevels];
            int increment = 0;
            for (int m = miplevels - 1; m >= 0; m--)
            {
                int w = (int)(width / Math.Pow(2, m));
                int h = (int)(height / Math.Pow(2, m));
                DXTSizes[increment] = (int)(((w + 3) / 4) * ((h + 3) / 4) * blockSize);
                increment++;
            }
            return DXTSizes;
        }

        private static string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";

            return size;
        }
    }
}