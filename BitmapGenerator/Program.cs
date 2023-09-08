using System.Drawing;
using System.Drawing.Imaging;

namespace BitmapGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            int seed = rnd.Next();
            int offsetX = rnd.Next(-1000,1000);
            int offsetY = rnd.Next(-1000, 1000);
            byte red = (byte)(seed % 256);
            byte green = (byte)(seed/256 % 256);
            byte blue = (byte)(seed/256/256 % 256);

            Console.WriteLine(seed);
            Console.WriteLine("{0} {1} {2}\n{3}\n{4}\n{5}\n{6}", red, green, blue, seed, offsetX, offsetY, (int)Math.Log(seed / (red * green * blue + 1)));

            const int chunkSize = 64;

            var arr = new int[chunkSize + 1, chunkSize + 1];

            arr[0, 0] = (int)Math.Abs(Math.Sin(offsetX)*Math.Cos(offsetY)*(Math.Sqrt(seed)));
            arr[0, chunkSize - 1] = (int)Math.Abs(Math.Sin(offsetX) * Math.Cos(offsetY) * (Math.Sqrt(seed)));
            arr[chunkSize - 1, 0] = (int)Math.Abs(Math.Sin(offsetX) * Math.Cos(offsetY) * (Math.Sqrt(seed)));
            arr[chunkSize - 1, chunkSize - 1] = (int)Math.Abs(Math.Sin(offsetX) * Math.Cos(offsetY) * (Math.Sqrt(seed)));

            // дописать diamond-square алгоритм

            var bitmap = new Bitmap(chunkSize, chunkSize);

            for (int i = 0; i <chunkSize;i++) {
                for (int j = 0; j < chunkSize; j++) 
                {
                   
                }
            }

            var fileName = $@"bitmap_{seed}_{offsetX}_{offsetY}.png";

            bitmap.Save(fileName, ImageFormat.Png);
        }
    }
}