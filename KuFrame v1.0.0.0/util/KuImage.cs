using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Ku.util
{
    public class KuImage
    {
        //24位图->灰度数据
        private static byte[][] GetData24bGray(Bitmap bmp)
        {
            byte[][] result = null;
            int w = bmp.Width;
            int h = bmp.Height;
            result = new byte[h][];
            for (int i = 0; i < h; i++)
            {
                result[i] = new byte[w];
            }
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte[] bmpDatas = new byte[bmpData.Height * bmpData.Stride];        //为目标数组分配内存
            IntPtr ptr = bmpData.Scan0;                                         //获取bmpData的内存起始位置
            Marshal.Copy(ptr, bmpDatas, 0, bmpDatas.Length);                    //图像数据拷贝
            int offset = bmpData.Stride - w * 3;                                    //显示宽度与扫描线宽度的间隙
            bmp.UnlockBits(bmpData);
            int posScan = 0;
            // 计算灰度数组
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    double temp = bmpDatas[posScan++] * 0.11 +
                        bmpDatas[posScan++] * 0.59 +
                        bmpDatas[posScan++] * 0.3;
                    result[i][j] = (byte)temp;
                }
                // 跳过图像数据每行未用空间的字节，length = stride - width * bytePerPixel
                posScan += offset;
            }
            return result;
        }

        //生成8位灰度图片
        public static Bitmap GetGrayBitmap(Bitmap bmp)
        {
            byte[][] bmpData;
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    bmpData = GetData24bGray(bmp);
                    break;
                default:
                    return bmp;
            }
            int height = bmp.Height;
            int width = bmp.Width;
            // 新建一个8位灰度位图，并锁定内存区域操作
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            // 计算图像参数
            int offset = data.Stride - data.Width;          // 计算每行未用空间字节数
            IntPtr ptr = data.Scan0;                        // 获取首地址
            int scanBytes = data.Stride * data.Height;      // 图像字节数 = 扫描字节数 * 高度
            byte[] datas = new byte[scanBytes];             // 为图像数据分配内存

            // 为图像数据赋值
            int posScan = 0;                                // 图像数据里的地址
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    datas[posScan++] = bmpData[i][j];
                }
                // 跳过图像数据每行未用空间的字节，length = stride - width * bytePerPixel
                posScan += offset;
            }
            // 内存解锁
            Marshal.Copy(datas, 0, ptr, scanBytes);
            bitmap.UnlockBits(data);  // 解锁内存区域
            // 修改生成位图的索引表，从伪彩修改为灰度
            ColorPalette palette;
            palette = bitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            // 修改生成位图的索引表
            bitmap.Palette = palette;
            return bitmap;
        }

        //生成单色位图
        public static Bitmap Get2ValueBitmap(Bitmap bmp, int l)
        {
            byte[][] bmpData;
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format24bppRgb:
                    bmpData = GetData24bGray(bmp);
                    break;
                default:
                    return bmp;
            }

            int height = bmp.Height;
            int width = bmp.Width;
            // 新建一个8位灰度位图，并锁定内存区域操作
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format1bppIndexed);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            IntPtr ptr = data.Scan0;                                                    //获取首地址
            int scanBytes = data.Stride * data.Height;                                            //图像字节数 = 扫描字节数 * 高度 
            byte[] datas = new byte[scanBytes];                                         //为图像数据分配内存                  ??

            // 为图像数据赋值
            int posScan = 0;                                                  //数据在图像里的地址
            for (int i = 0; i < height; i++)
            {
                //posScan = i * data.Stride;                      //移到行首
                int x = 0, value = 0;
                for (int j = 0; j < width; j++)
                {
                    if (bmpData[i][j] > l)
                        value = 2 * value + 1;
                    else
                        value = 2 * value;
                    x++;
                    if (x == 8)
                    {
                        datas[posScan++] = (byte)value;
                        x = 0;
                        value = 0;
                    }
                }
                if (x > 0)
                {                                           //位数不足，在末尾补0
                    for (int t = x; t < 8; t++)
                    {
                        value = value * 2 + 1;
                    }
                    datas[posScan++] = (byte)value;
                }
                while (posScan < (i + 1) * data.Stride)
                {
                    datas[posScan++] = 0xFF;
                }
            }
            Marshal.Copy(datas, 0, data.Scan0, datas.Length);
            bitmap.UnlockBits(data);

            return bitmap;
        }
    }
}
