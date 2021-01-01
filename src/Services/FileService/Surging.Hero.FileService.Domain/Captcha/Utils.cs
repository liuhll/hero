using System;
using System.IO;
using System.Linq;
using SkiaSharp;

namespace Surging.Hero.FileService.Domain.Captcha
{
    internal static class Utils
    {
        public static byte[] CreateCaptcha(string code)
        {
            var charList = code.ToList();
            var bmp = new SKBitmap(80, 30);
            using (var canvas = new SKCanvas(bmp))
            {
                //背景色

                canvas.DrawColor(SKColors.White);

                using (SKPaint sKPaint = new SKPaint())

                {

                    sKPaint.TextSize = 16; //字体大小
                    sKPaint.IsAntialias = true; //开启抗锯齿                  
                    sKPaint.Typeface = SKTypeface.FromFamilyName("微软雅黑");
                    var size = new SKRect();
                    sKPaint.MeasureText(charList[0].ToString(), ref size);
                    var temp = (bmp.Width / 4 - size.Size.Width) / 2;
                    var temp1 = bmp.Height - (bmp.Height - size.Size.Height) / 2;
                    var random = new Random();
                    //画文字
                    for (int i = 0; i < 4; i++)
                    {
                        sKPaint.Color = new SKColor((byte) random.Next(0, 255), (byte) random.Next(0, 255),
                            (byte) random.Next(0, 255));
                        canvas.DrawText(charList[i].ToString(), temp + 20 * i, temp1, sKPaint);

                    }

                    //画干扰线
                    for (int i = 0; i < 5; i++)
                    {
                        sKPaint.Color = new SKColor((byte) random.Next(0, 255), (byte) random.Next(0, 255),
                            (byte) random.Next(0, 255));
                        canvas.DrawLine(random.Next(0, 40), random.Next(1, 29), random.Next(41, 80), random.Next(1, 29),
                            sKPaint);

                    }

                }
            }
            using (SKImage img = SKImage.FromBitmap(bmp))
            {
                using (SKData p = img.Encode())
                {
                    return p.ToArray();
                }

            }
        }
    }
}