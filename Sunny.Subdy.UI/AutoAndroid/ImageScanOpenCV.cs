using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Tesseract;
using Point = System.Drawing.Point;
namespace AutoAndroid
{
    public static class ImageScanOpenCV
    {
        public static Bitmap GetImage(string path)
        {
            return new Bitmap(path);
        }

        public static Bitmap Find(string main, string sub, double percent = 0.9)
        {
            Bitmap mainImg = ImageScanOpenCV.GetImage(main);
            Bitmap subImg = ImageScanOpenCV.GetImage(sub);
            return ImageScanOpenCV.Find(mainImg, subImg, percent);
        }

        public static Bitmap Find(Bitmap mainBitmap, Bitmap subBitmap, double percent = 0.9)
        {
            // Convert Bitmaps to Mat
            Mat source = BitmapToMat(mainBitmap);
            Mat template = BitmapToMat(subBitmap);
            Mat imageToShow = source.Clone();

            // Perform template matching
            Mat result = new Mat();
            Cv2.MatchTemplate(source, template, result, TemplateMatchModes.CCoeffNormed);

            // Get the min/max values
            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

            if (maxVal > percent)
            {
                // Draw rectangle around match
                Cv2.Rectangle(imageToShow, maxLoc, new OpenCvSharp.Point(maxLoc.X + template.Width, maxLoc.Y + template.Height), new Scalar(0, 0, 255), 2);
            }
            else
            {
                imageToShow = null;
            }

            return (imageToShow == null) ? null : MatToBitmap(imageToShow);
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static Point? FindOutPoint(Bitmap mainBitmap, Bitmap subBitmap, double percent = 0.9)
        {
            if (subBitmap == null || mainBitmap == null)
                return null;

            if (subBitmap.Width > mainBitmap.Width || subBitmap.Height > mainBitmap.Height)
                return null;

            Mat source = BitmapToMat(mainBitmap);
            Mat template = BitmapToMat(subBitmap);

            Mat result = new Mat();
            Cv2.MatchTemplate(source, template, result, TemplateMatchModes.CCoeffNormed);

            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

            if (maxVal > percent)
            {
                // Convert OpenCvSharp.Point to System.Drawing.Point
                return new System.Drawing.Point(maxLoc.X, maxLoc.Y);
            }

            return null;
        }

        public static List<Point> FindOutPoints(Bitmap mainBitmap, Bitmap subBitmap, double percent = 0.9)
        {
            Mat source = BitmapToMat(mainBitmap);
            Mat template = BitmapToMat(subBitmap);
            List<Point> resPoints = new List<Point>();

            while (true)
            {
                Mat result = new Mat();
                Cv2.MatchTemplate(source, template, result, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

                if (maxVal <= percent)
                    break;

                // Draw rectangle around match and add the location
                Cv2.Rectangle(source, maxLoc, new OpenCvSharp.Point(maxLoc.X + template.Width, maxLoc.Y + template.Height), new Scalar(0, 0, 255), 2);
                resPoints.Add(new System.Drawing.Point(maxLoc.X, maxLoc.Y));
            }

            return resPoints;
        }

        public static List<Point> FindColor(Bitmap mainBitmap, System.Drawing.Color color)
        {
            int searchValue = color.ToArgb();
            List<Point> result = new List<Point>();

            try
            {
                Mat mat = BitmapToMat(mainBitmap);
                for (int y = 0; y < mat.Rows; y++)
                {
                    for (int x = 0; x < mat.Cols; x++)
                    {
                        Vec3b pixel = mat.Get<Vec3b>(y, x);
                        int pixelArgb = Color.FromArgb(pixel.Item2, pixel.Item1, pixel.Item0).ToArgb();

                        if (searchValue == pixelArgb)
                        {
                            result.Add(new Point(x, y));
                        }
                    }
                }
            }
            finally
            {
                if (mainBitmap != null)
                {
                    mainBitmap.Dispose();
                }
            }

            return result;
        }

        public static void TestDilate(Bitmap bmp)
        {
            Mat mat = BitmapToMat(bmp);
            Mat dilated = new Mat();

            // Perform dilation
            Cv2.Dilate(mat, dilated, new Mat(), iterations: 1);

            // Save the result
            MatToBitmap(dilated).Save("dilated.png");
        }

        public static Bitmap ThreshHoldBinary(Bitmap bmp, byte threshold = 190)
        {
            Mat img = BitmapToMat(bmp);
            Mat thresholded = new Mat();

            // Apply binary threshold
            Cv2.Threshold(img, thresholded, threshold, 255, ThresholdTypes.Binary);

            return MatToBitmap(thresholded);
        }

        public static Bitmap NotWhiteToTransparentPixelReplacement(Bitmap bmp)
        {
            return PixelReplacement(bmp, color => color.R > 200 && color.G > 200 && color.B > 200, System.Drawing.Color.Transparent);
        }

        public static Bitmap WhiteToBlackPixelReplacement(Bitmap bmp)
        {
            return PixelReplacement(bmp, color => color.R > 20 && color.G > 230 && color.B > 230, System.Drawing.Color.Black);
        }

        public static Bitmap TransparentToWhitePixelReplacement(Bitmap bmp)
        {
            return PixelReplacement(bmp, color => color.A >= 1, System.Drawing.Color.White);
        }

        private static Bitmap PixelReplacement(Bitmap bmp, Func<System.Drawing.Color, bool> condition, System.Drawing.Color replacementColor)
        {
            Mat mat = BitmapToMat(bmp);
            for (int y = 0; y < mat.Rows; y++)
            {
                for (int x = 0; x < mat.Cols; x++)
                {
                    Vec3b pixel = mat.Get<Vec3b>(y, x);
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(pixel.Item2, pixel.Item1, pixel.Item0);
                    if (condition(color))
                    {
                        mat.Set(y, x, new Vec3b(replacementColor.B, replacementColor.G, replacementColor.R));
                    }
                }
            }
            return MatToBitmap(mat);
        }

        public static Bitmap CreateNonIndexedImage(Image src)
        {
            Bitmap newBmp = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gfx = Graphics.FromImage(newBmp))
            {
                gfx.DrawImage(src, 0, 0);
            }
            return newBmp;
        }

        private static Mat BitmapToMat(Bitmap bmp)
        {
            try
            {
                using (var stream = new System.IO.MemoryStream())
                {
                    bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return Cv2.ImDecode(stream.ToArray(), ImreadModes.Color);
                }
            }
            catch
            {

            }
            return null;
        }

        private static Bitmap MatToBitmap(Mat mat)
        {
            using (var stream = new System.IO.MemoryStream())
            {
                Cv2.ImEncode(".png", mat, out byte[] imageData);
                stream.Write(imageData, 0, imageData.Length);
                return new Bitmap(stream);
            }
        }
        public static string GetTextFromImage(Bitmap bitmap)
        {
            if (bitmap == null)
                return string.Empty;

            try
            {
                // Convert Bitmap to Mat
                Mat image = BitmapToMat(bitmap);

                // Check if image is successfully loaded
                if (image.Empty())
                {
                    Console.WriteLine("Image data is empty after conversion.");
                    return string.Empty;
                }
                // Preprocess image: convert to grayscale and blur
                Cv2.CvtColor(image, image, ColorConversionCodes.BGR2GRAY);
                Cv2.GaussianBlur(image, image, new OpenCvSharp.Size(5, 5), 0);

                // Apply binary thresholding
                Cv2.Threshold(image, image, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                // Optional: Save the processed image for verification
                Cv2.ImWrite("processed_image.png", image);

                // OCR with Tesseract
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var pix = MatToPix(image))
                    {
                        if (pix == null)
                        {
                            Console.WriteLine("Failed to convert Mat to Pix.");
                            return string.Empty;
                        }

                        var result = engine.Process(pix);
                        return result.GetText();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during OCR: " + ex.Message);
                return string.Empty;
            }
        }

        // Chuyển Mat sang Pix (Tesseract)
        private static Pix MatToPix(Mat mat)
        {
            using (var stream = mat.ToMemoryStream())
            {
                return Pix.LoadFromMemory(stream.ToArray());
            }
        }

    }
}
