using System;
using System.Diagnostics;
using System.Drawing;

namespace Practical_3_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Func<Bitmap, Bitmap> grayscale = (bitmap) =>
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        Color originalColor = bitmap.GetPixel(x, y);
                        int average = (originalColor.R + originalColor.G + originalColor.B) / 3;
                        Color newColor = Color.FromArgb(originalColor.A, average, average, average);
                        bitmap.SetPixel(x, y, newColor);
                    }
                }
                return bitmap;
            };

            Func<Bitmap, Bitmap> invert = (bitmap) =>
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        Color originalColor = bitmap.GetPixel(x, y);
                        Color newColor = Color.FromArgb(originalColor.A, 255 - originalColor.R, 255 - originalColor.G, 255 - originalColor.B);
                        bitmap.SetPixel(x, y, newColor);
                    }
                }
                return bitmap;
            };

            Action<Bitmap> displayImage = (bitmap) =>
            {
                bitmap.Save("processed_image.png");

                Process p = new Process();
                p.StartInfo = new ProcessStartInfo("processed_image.png")
                {
                    UseShellExecute = true
                };
                p.Start();
            };

            Bitmap originalImage = new Bitmap("image.png");
            Bitmap processed = grayscale(originalImage);
            displayImage(processed);
            Console.ReadLine();
            Bitmap inverted = invert(originalImage);
            displayImage(inverted);
        }
    }
}
