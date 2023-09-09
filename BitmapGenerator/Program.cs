using System.Drawing;
using System.Drawing.Imaging;

namespace BitmapGenerator
{
    internal class Program
    {
        static Random rnd = new Random();
        static int seed = rnd.Next();
        static int offsetX = rnd.Next(-1000, 1000);
        static int offsetY = rnd.Next(-1000, 1000);
        static int Chunk = 4;
        static void Main(string[] args)
        {
            int scale = 256;

            var arr = new double[Chunk + 1, Chunk + 1];

            arr[0, 0] =
                Seeder(seed, offsetX, offsetY, scale);
            arr[0, Chunk] =
                Seeder(seed, offsetX, offsetY + Chunk, scale);
            arr[Chunk, 0] =
                Seeder(seed, offsetX + Chunk, offsetY, scale);
            arr[Chunk, Chunk] =
                Seeder(seed, offsetX + Chunk, offsetY + Chunk, scale);

            Console.WriteLine(
                "\n{0} {1} {2} {3}\n\n",
                arr[0, 0],
                arr[0, Chunk],
                arr[Chunk, 0],
                arr[Chunk, Chunk]
                );


            int len = Chunk;
            double h = Math.Pow(2,-0.002);
            while (true)
            {
                DiamondStep(ref arr, ref len, ref h);
                h *= h;
                SquareStep(ref arr, ref len, ref h);
                h *= h;
                if (len<2)
                break;
            }

            var bitmap = new Bitmap(Chunk, Chunk);

            for (int i = 0; i < Chunk; i++) {
                for (int j = 0; j < Chunk; j++)
                {
                    bitmap.SetPixel(i, j, Color.FromArgb((int)arr[i, j] * 256 / scale, (int)arr[i, j] * 256 / scale, (int)arr[i, j] * 256 / scale));
                }
            }

            var fileName = $"generated/bitmap_{seed}_{offsetX}_{offsetY}.png";

            bitmap.Save(fileName, ImageFormat.Png);
        }

        static double Seeder(int seed, int _x, int _y, int size)
        {
            double result = size * Math.Abs(
                Math.Sin(
                    Math.Sin(
                        _x + Math.Sqrt(seed))
                    * Math.Cos(
                        _y + Math.Sqrt(seed))
                ));
            return result;
        }
        static void DiamondStep(ref double[,] arr, ref int len, ref double h)
        {
            for (int i = 0; i < arr.GetLength(0)-1; i += len)
            {
                for (int j = 0; j < arr.GetLength(1)-1; j += len)
                {
                    double s =
                        arr[i, j] +
                        arr[i + len, j] +
                        arr[i, j + len] +
                        arr[i + len, j + len];
                    arr[i + len / 2, j + len / 2] = s / 4 + Math.Sin(Math.Log(seed)) * h;
                }    
            }
        }

        static void SquareStep(ref double[,] arr, ref int len, ref double h)
        {
            bool start = true;
            len /= 2;
            for (int i = 0; i < arr.GetLength(0); i += len)
            {
                for (int j = 0; j < arr.GetLength(1); j += 2*len)
                {
                    int b = (start) ? 0 : len;
                    int k = j - b;
                    double s =
                        ((IsValid(i) && IsValid(k)) ? arr[i, k] : 0) +
                        ((IsValid(i) && IsValid(k + 2 * len)) ? arr[i, k + 2 * len] : 0) +
                        ((IsValid(i - len) && IsValid(k + len)) ? arr[i - len, k + len] : 0) +
                        ((IsValid(i + len) && IsValid(k + len)) ? arr[i + len, k + len] : 0);
                    if (IsValid(i) && IsValid(k + len))
                    {
                        arr[i, k + len] = s / 4 + Math.Cos(Math.Cbrt(seed)) * h;
                    }
                }
                start = !start;
            }
        }

        static void Print(int[,] arr)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write("{0} ", arr[i, j]);
                }
                Console.WriteLine();
            }
        }
        static bool IsValid(int index)
        {
            return index < Chunk + 1 && index >= 0;
        }
    }
}