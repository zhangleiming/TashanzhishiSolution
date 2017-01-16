using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhixing.Tashanzhishi.Web.Imaging
{
    public static class ImageCommon
    {
        /// <summary>
        /// 为图片生成水印
        /// </summary>
        /// <param name="originalBmp">原始图片</param>
        /// <param name="text">文字</param>
        /// <param name="point">写入位置</param>
        /// <returns></returns>
        public static Bitmap GenerateWatermark(Bitmap originalBmp, string text, PointF point, Font font, Brush brush)
        {
            Bitmap bmpOut = new Bitmap(originalBmp);
            using (Graphics graphics = Graphics.FromImage(bmpOut))
            {
                graphics.DrawString(text, font, brush, point);
            }

            return bmpOut;
        }

        /// <summary>
        /// 向图片中添加图片
        /// </summary>
        /// <param name="originalImg"></param>
        /// <param name="addImg"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static Bitmap AddImage(Bitmap originalImg, Image addImg, PointF point)
        {
            Bitmap outBitmap = new Bitmap(originalImg);
            using (Graphics graphics = Graphics.FromImage(outBitmap))
            {
                graphics.DrawImage(addImg, point);
            }

            return outBitmap;
        }

        /// <summary>
        /// 生成图片的缩略图
        /// </summary>
        /// <param name="originalBmp">原始图片</param>
        /// <param name="desiredWidth">缩放后宽</param>
        /// <param name="desiredHeight">缩放后高</param>
        /// <returns></returns>
        public static Bitmap CreateThumbnail(Bitmap originalBmp, int desiredWidth, int desiredHeight)
        {
            if (originalBmp.Width <= desiredWidth && originalBmp.Height <= desiredHeight)
            {
                return originalBmp;
            }

            int newWidth, newHeight;

            // scale down the smaller dimension
            if (desiredWidth * originalBmp.Height < desiredHeight * originalBmp.Width)
            {
                newWidth = desiredWidth;
                newHeight = (int)Math.Round((decimal)originalBmp.Height * desiredWidth / originalBmp.Width);
            }
            else
            {
                newHeight = desiredHeight;
                newWidth = (int)Math.Round((decimal)originalBmp.Width * desiredHeight / originalBmp.Height);
            }

            // This code creates cleaner (though bigger) thumbnails and properly
            // and handles GIF files better by generating a white background for
            // transparent images (as opposed to black)
            // This is preferred to calling Bitmap.GetThumbnailImage()
            Bitmap bmpOut = new Bitmap(newWidth, newHeight);
            using (Graphics graphics = Graphics.FromImage(bmpOut))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                graphics.DrawImage(originalBmp, 0, 0, newWidth, newHeight);
            }

            return bmpOut;
        }

        /// <summary>
        /// 生成透明图片
        /// The input is the color which you want to make transparent.
        /// </summary>
        /// <param name="color">The color to make transparent.</param>
        /// <param name="bitmap">The bitmap to make transparent.</param>
        /// <returns>New memory stream containing transparent background gif.</returns>
        public static Bitmap MakeTransparentGif(Bitmap bitmap, Color color)
        {
            byte R = color.R;
            byte G = color.G;
            byte B = color.B;

            MemoryStream fin = new MemoryStream();
            bitmap.Save(fin, System.Drawing.Imaging.ImageFormat.Gif);

            MemoryStream fout = new MemoryStream((int)fin.Length);
            int count = 0;
            byte[] buf = new byte[256];
            byte transparentIdx = 0;
            fin.Seek(0, SeekOrigin.Begin);
            //header
            count = fin.Read(buf, 0, 13);
            if ((buf[0] != 71) || (buf[1] != 73) || (buf[2] != 70)) return null; //GIF

            fout.Write(buf, 0, 13);

            int i = 0;
            if ((buf[10] & 0x80) > 0)
            {
                i = 1 << ((buf[10] & 7) + 1) == 256 ? 256 : 0;
            }

            for (; i != 0; i--)
            {
                fin.Read(buf, 0, 3);
                if ((buf[0] == R) && (buf[1] == G) && (buf[2] == B))
                {
                    transparentIdx = (byte)(256 - i);
                }
                fout.Write(buf, 0, 3);
            }

            bool gcePresent = false;
            while (true)
            {
                fin.Read(buf, 0, 1);
                fout.Write(buf, 0, 1);
                if (buf[0] != 0x21) break;
                fin.Read(buf, 0, 1);
                fout.Write(buf, 0, 1);
                gcePresent = (buf[0] == 0xf9);
                while (true)
                {
                    fin.Read(buf, 0, 1);
                    fout.Write(buf, 0, 1);
                    if (buf[0] == 0) break;
                    count = buf[0];
                    if (fin.Read(buf, 0, count) != count) return null;
                    if (gcePresent)
                    {
                        if (count == 4)
                        {
                            buf[0] |= 0x01;
                            buf[3] = transparentIdx;
                        }
                    }
                    fout.Write(buf, 0, count);
                }
            }
            while (count > 0)
            {
                count = fin.Read(buf, 0, 1);
                fout.Write(buf, 0, 1);
            }
            fin.Close();
            fout.Flush();

            return new Bitmap(fout);
        }
    }
}
