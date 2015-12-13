using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using System.IO;

namespace Redback_Report_Generator
{
    class Lunge_Page : Report_Page
    {
        public Lunge_Page(PdfPage page, ProfileInfo profleInfo, List<Parameter> userParameters) : base(page, profleInfo, userParameters)
        { }

        public void DrawStats(XGraphics gfx)
        {
            XFont large = new XFont("Arial", 20);
            XFont small = new XFont("Arial", 12);

            double yOff = page_.Height * 0.11;

            XRect rect = new XRect(20, yOff, page_.Width - 40, page_.Height - (yOff + 20));
            DrawingUtil.DrawOutlineRect(rect, gfx, cornerRadius);
            gfx.DrawRoundedRectangle(backgroundBrush, rect, cornerRadius);
            XPoint center = new XPoint(page_.Width * 0.5, page_.Height * 0.45);

            XImage sideLunge = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\SideLunge.png");
            XImage frontLunge = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\FrontLunge.png");


            gfx.DrawString("Left Lunge", large, XBrushes.Black, new XPoint(center.X - page_.Width * 0.25, page_.Height * 0.25), XStringFormats.Center);
            gfx.DrawString("Right Lunge", large, XBrushes.Black, new XPoint(center.X + page_.Width * 0.25, page_.Height * 0.25), XStringFormats.Center);

            for (int i = 0; i < 4; i++)
            {
                Parameter param1 = userParameters_.ElementAt(i);
                Parameter param2 = userParameters_.ElementAt(i + 4);
                char degree = Convert.ToChar('\u00b0');
                XBrush leftParamCol = XBrushes.Green;
                XBrush rightParamCol = XBrushes.Green;

                double y = (i * page_.Height * 0.15f) + page_.Height * 0.35;
                gfx.DrawString(param1.Name, small, XBrushes.Black, new XPoint(center.X - page_.Width * 0.25, y), XStringFormats.Center);

                if (param1.Color == "Amber")
                    leftParamCol = new XSolidBrush(XColor.FromArgb(199, 171, 14));
                else if (param1.Color == "Red")
                    leftParamCol = XBrushes.Red;
                XRect infoRect = new XRect(center.X - page_.Width * 0.25 - 25, y + 20, 50, 35);
                gfx.DrawRoundedRectangle(leftParamCol, infoRect, new XSize(10, 10));
                gfx.DrawString(param1.Value.ToString() + degree, small, XBrushes.Black, new XPoint(infoRect.X + 25, infoRect.Y + 17.5), XStringFormats.Center);

                gfx.DrawString(param2.Name, small, XBrushes.Black, new XPoint(center.X + page_.Width * 0.25, y), XStringFormats.Center);
                if (param2.Color == "Amber")
                    rightParamCol = new XSolidBrush(XColor.FromArgb(199, 171, 14));
                else if (param2.Color == "Red")
                    rightParamCol = XBrushes.Red;
                infoRect = new XRect(center.X + page_.Width * 0.25 - 25, y + 20, 50, 35);
                gfx.DrawRoundedRectangle(rightParamCol, infoRect, new XSize(10, 10));
                gfx.DrawString(param2.Value.ToString() + degree, small, XBrushes.Black, new XPoint(infoRect.X + 25, infoRect.Y + 17.5), XStringFormats.Center);

                XImage img = i > 1 ? frontLunge : sideLunge;

                double wRatio = (double)img.PixelWidth / (double)sideLunge.PixelHeight;
                XRect imgRect = new XRect(center.X - wRatio * 40, y, wRatio * 80, 80);
                gfx.DrawImage(img, imgRect);
            }
        }
    }
}
