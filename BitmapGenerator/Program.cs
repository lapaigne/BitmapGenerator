using System.Drawing;
using System.Drawing.Imaging;

namespace BitmapGenerator
{
    internal class Program
    {
        static Random rnd = new Random();
        static int Chunk = 1024;
        static int seed =
            rnd.Next();
            //755730920;
        static int offsetX =
            rnd.Next(-1000, 1000);
            //357;
        static int offsetY =
            rnd.Next(-1000, 1000);
            //584;
        static void Main(string[] args)
        {
            int scale = 16;

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
            double h = Math.Pow(2, -0.014);
            while (true)
            {
                DiamondStep(ref arr, ref len, ref h);
                h *= h;
                SquareStep(ref arr, ref len, ref h);
                if (len < 2)
                break;
            }

            var bitmap = new Bitmap(Chunk, Chunk);

            for (int i = 0; i < Chunk; i++) {
                for (int j = 0; j < Chunk; j++)
                {
                    int val = (int)arr[i, j];
                    val = (val > 14) ? 14 : val;
                    val = (val < -2) ? -2 : val;
                    int red = 128;
                    int green = 0;
                    int blue = 128;

                    switch (val)
                    {
                        case -2:
                            red = 32;
                            green = 50;
                            blue = 114;
                            break;
                        case -1:
                            red = 70;
                            green = 89;
                            blue = 165;
                            break;
                        case 0:
                            red = 139;
                            green = 154;
                            blue = 94;
                            break;
                        case 1:
                        case 2:
                            red = 131;
                            green = 151;
                            blue = 71;
                            break;
                        case 3:
                        case 4:
                            red = 150;
                            green = 162;
                            blue = 77;
                            break;
                        case 5:
                            red = 169;
                            green = 172;
                            blue = 82;
                            break;
                        case 6:
                        case 7:
                            red = 194;
                            green = 188;
                            blue = 95;
                            break;
                        case 8:
                        case 9:
                            red = 219;
                            green = 204;
                            blue = 108;
                            break;
                        case 10:
                            red = 176;
                            green = 146;
                            blue = 80;
                            break;
                        case 11:
                            red = 132;
                            green = 87;
                            blue = 51;
                            break;
                        case 12:
                            red = 116;
                            green = 48;
                            blue = 37;
                            break;
                        case 13:
                            red = 176;
                            green = 175;
                            blue = 175;
                            break;
                        case 14:
                            red = 226;
                            green = 225;
                            blue = 225;
                            break;
                    }

                    bitmap.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    //bitmap.SetPixel(i, j, Color.FromArgb((val + 2)*255/scale, (val + 2)*255/scale, (val + 2)*255/scale));
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
            for (int i = 0; i < arr.GetLength(0) - 1; i += len) 
            {
                for (int j = 0; j < arr.GetLength(1) - 1; j += len)
                {
                    double s =
                        arr[i, j] +
                        arr[i + len, j] +
                        arr[i, j + len] +
                        arr[i + len, j + len];
                    arr[i + len / 2, j + len / 2] = s / 4 + 7 * Math.Cos(s) * h;
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
                        arr[i, k + len] = s / 4 + 6 * Math.Sin(s) * h;
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
        static void GenerateChunk(ref float[,] arr, int seed )
        {

        }
        static bool IsValid(int index)
        {
            return index < Chunk + 1 && index >= 0;
        }
    }
}