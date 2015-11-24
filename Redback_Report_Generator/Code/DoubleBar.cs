using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redback_Report_Generator
{
    class DoubleBar
    {
        private Parameter parameterLeft;
        private Parameter parameterRight;
        private XGraphics gfx;
        public DoubleBar(Parameter parameterLeft, Parameter parameterRight, XGraphics gfx)
        {
            this.parameterLeft = parameterLeft;
            this.parameterRight = parameterRight;
            this.gfx = gfx;
        }

        public void Draw(XPoint basePoint, XRect baseRect, XImage image)
        {
            XFont headerFont = new XFont("Arial", 16);
            XFont subfont = new XFont("Arial", 10);
            char degree = Convert.ToChar('\u00b0');
            gfx.DrawString(parameterLeft.Name.Substring(5, parameterLeft.Name.Length - 5), headerFont, XBrushes.Black, basePoint, XStringFormats.Center);
            gfx.DrawString(parameterLeft.Value.ToString() + degree, subfont, XBrushes.Black, basePoint + new XPoint(-15, baseRect.Height * 0.05),XStringFormats.Center);
            gfx.DrawString(parameterRight.Value.ToString() + degree, subfont, XBrushes.Black, basePoint + new XPoint(20, baseRect.Height * 0.05), XStringFormats.Center);
            
            XBrush brush = XBrushes.Black;

            if (parameterLeft.LSI >= 75)
                brush = XBrushes.Green;
            else
                brush = XBrushes.Gold;

            gfx.DrawRoundedRectangle(brush, 
                new XRect(basePoint + new XPoint(-baseRect.Width * 0.05, baseRect.Height * 0.075),
                new XSize(baseRect.Width * 0.1, baseRect.Height * 0.075)),
                new XSize(10,10));
            gfx.DrawString(parameterLeft.LSI.ToString("0") + "%",
                subfont,
                XBrushes.Black,
                basePoint + new XPoint(baseRect.Width * -0.025, baseRect.Height * 0.125));

            XSolidBrush blue = new XSolidBrush(XColor.FromArgb(127, 0, 0, 255));
            XSolidBrush yellow = new XSolidBrush(XColor.FromArgb(127, 255, 255, 0));

            XPoint top = new XPoint(basePoint.X - 20,Math.Ceiling( basePoint.Y + baseRect.Height * 0.2));
            XPoint bottom = new XPoint(basePoint.X - 20, Math.Ceiling(basePoint.Y + baseRect.Height * 0.8));

            XPoint leftRectPoint = Interpolate(bottom, top, -parameterLeft.Percentage);
            XRect leftBar = new XRect(leftRectPoint, new XSize(20, bottom.Y - leftRectPoint.Y  ));
            gfx.DrawRectangle(blue, leftBar);

            XPoint rigthRectPoint = Interpolate(bottom, top, -parameterRight.Percentage);
            XRect rightBar = new XRect(rigthRectPoint + new XPoint(30,0), new XSize(20, bottom.Y - rigthRectPoint.Y));
            gfx.DrawRectangle(yellow, rightBar);

            XPoint offset = new XPoint(25, 0);
            gfx.DrawLine(XPens.Green, top + offset, bottom + offset);
            gfx.DrawLine(XPens.Yellow, Interpolate(bottom, top, - parameterLeft.AmberVal) + offset, bottom + offset);
            gfx.DrawLine(XPens.Red, Interpolate(bottom, top, -parameterLeft.RedVal) + offset, bottom + offset);

            gfx.DrawString("L", subfont, XBrushes.Black, bottom + new XPoint(5, -2));
            gfx.DrawString("R", subfont, XBrushes.Black, bottom + new XPoint(35, -2));

            gfx.DrawImage(image, new XRect(bottom.X, bottom.Y, 50, 50));
            image.Dispose();
        }

        XPoint Interpolate(XPoint pt1, XPoint pt2, double amount)
        {
            return pt1 + amount * (pt1 - pt2);
        }
    }
}
