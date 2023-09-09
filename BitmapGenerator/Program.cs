using System.Drawing;
using System.Drawing.Imaging;

namespace BitmapGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            int seed =
                1597675681;
                //rnd.Next();
            int offsetX =
                0;
                //rnd.Next(-1000,1000);
            int offsetY =
                0;
                //rnd.Next(-1000, 1000);
            byte red = (byte)(seed % 256);
            byte green = (byte)(seed/256 % 256);
            byte blue = (byte)(seed/256/256 % 256);

            Console.WriteLine("{0} {1} {2}\n{3}\n{4}\n{5}\n{6}", red, green, blue, seed, offsetX, offsetY, (int)Math.Log(seed / (red * green * blue + 1)));

            const int chunkSize = 4;

            var arr = new int[chunkSize + 1, chunkSize + 1];

            arr[0, 0] =
                SeederInt(seed, offsetX, offsetY, 8);
            arr[0, chunkSize] =
                SeederInt(seed, offsetX, offsetY + chunkSize, 8);
            arr[chunkSize, 0] =
                SeederInt(seed, offsetX + chunkSize, offsetY, 8);
            arr[chunkSize, chunkSize] =
                SeederInt(seed, offsetX + chunkSize, offsetY + chunkSize, 8);

            // дописать diamond-square алгоритм

            Console.WriteLine(
                "\n{0} {1} {2} {3}\n\n",
                arr[0, 0],
                arr[0, chunkSize],
                arr[chunkSize, 0],
                arr[chunkSize, chunkSize]
                );


            int len = chunkSize;
            while (true)
            {
                DiamondStep(ref arr, ref len);
                Print(arr);
                Console.WriteLine();
                SquareStep(ref arr, ref len);
                Print(arr);
                Console.WriteLine();
                if (len<2)
                break;
            }

            var bitmap = new Bitmap(chunkSize, chunkSize);

            for (int i = 0; i < chunkSize; i++) {
                for (int j = 0; j < chunkSize; j++)
                {
                   
                }
            }

            var fileName = $@"bitmap_{seed}_{offsetX}_{offsetY}.png";

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

        static int SeederInt(int seed, int _x, int _y, int size)
        {
            return (int)Seeder(seed, _x, _y, size);
        }

        static void DiamondStep(ref int[,] arr, ref int len)
        {
            for (int i = 0; i < arr.GetLength(0)-1; i += len)
            {
                for (int j = 0; j < arr.GetLength(1)-1; j += len)
                {
                    int s =
                        arr[i, j] +
                        arr[i + len, j] +
                        arr[i, j + len] +
                        arr[i + len, j + len];
                    arr[i + len / 2, j + len / 2] = s / 4;
                }    
            }
        }

        static void SquareStep(ref int[,] arr, ref int len)
        {
            bool start = true;
            int arrLength = arr.GetLength(0);
            len /= 2;
            for (int i = 0; i < arr.GetLength(0) - 1; i += len)
            {
                for (int j = 0; j < arr.GetLength(1) - 1; j += len)
                {
                    int b = (start) ? 0 : len;
                    int s =
                        ((IsValid(i, arrLength) && IsValid(j, arrLength)) ? arr[i, j] : -40) +
                        ((IsValid(i + len, arrLength) && IsValid(j - len, arrLength)) ? arr[i, j - len] : -400) +
                        ((IsValid(i + len, arrLength) && IsValid(j + len, arrLength)) ? arr[i, j + len] : -4000) +
                        ((IsValid(i + 2 * len, arrLength) && IsValid(j, arrLength)) ? arr[i + 2 * len, j] : 0);
                    arr[i + len, j] = s / 4;
                    Console.WriteLine($"\t{i + len},{j - len}\n{i},{j}\t{arr[i + len, j]}\t{i + 2 * len},{j}\n\t{i + len},{j + len}\n\n");
                }
                start = !start;
            }
            Console.WriteLine();
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
        static bool IsValid(int index, int upperBound)
        {
            return index < upperBound && index >= 0;
        }
    }
}