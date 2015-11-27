using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;

namespace Redback_Report_Generator
{
    class ROM_Page : Report_Page
    {
        public ROM_Page(PdfPage page, ProfileInfo userInfo, List<Parameter> userParameters) :
            base(page, userInfo, userParameters)
        { }

        public override void DrawHeader(XGraphics gfx, string reportName)
        {
            base.DrawHeader(gfx, reportName);
        }

        public void DrawGraph(XGraphics gfx)
        {
            double yOff = page_.Height * 0.11;

            XRect rect = new XRect(20, yOff, page_.Width - 40, page_.Height - (yOff + 20));
            gfx.DrawRoundedRectangle(backgroundBrush, rect, cornerRadius);
            XPoint center = new XPoint(page_.Width * 0.5, page_.Height * 0.45);

            //Left & right boxes
            XRect leftRect = new XRect(center.X - 250, yOff + 5, 160, 25);
            XRect rightRect = new XRect(center.X + 90, yOff + 5, 160, 25);
            gfx.DrawRoundedRectangle(XBrushes.Yellow, leftRect, new XSize(10, 10));
            gfx.DrawRoundedRectangle(XBrushes.CornflowerBlue, rightRect, new XSize(10, 10));
            gfx.DrawString("Left : ", new XFont("Arial", 20), XBrushes.Black, new XPoint(leftRect.X + 80, leftRect.Y + 15), XStringFormats.Center);
            gfx.DrawString("Right : ", new XFont("Arial", 20), XBrushes.Black, new XPoint(rightRect.X + 80, rightRect.Y + 15), XStringFormats.Center);

            float graphSize = (float)page_.Height * 0.175f;
            XPoint[] polyPoints = DrawingUtil.Instance.GeneratePoints(center, graphSize, 7, gfx);
            gfx.DrawPolygon(XBrushes.DimGray, polyPoints, XFillMode.Winding);

            XPen yellowPen = new XPen(XColors.Yellow, 2.5);
            XPen greenPen = new XPen(XColors.Green, 2.5);
            XPen perimeterPen = XPens.LightGray;
            XPen redPen = new XPen(XColors.Red, 2.5);

            GraphIcon hipFlexImg = new GraphIcon(XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\HipFlex.png"),"Hip Flexion");
            GraphIcon hamStringImg = new GraphIcon(XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\HSExt.png"), "Hamstring Extension");
            GraphIcon hipAbdImg = new GraphIcon(XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\HipAbd.png"), "Hip Abduction");
            GraphIcon hipIntImg = new GraphIcon(XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\HipInt.png"), "Hip Internal Rotation");
            GraphIcon hipExtImg = new GraphIcon(XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\HipExt.png"), "Hip External Rotation");
            GraphIcon kneeFlexImg = new GraphIcon(XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\KneeFlex.png"), "Knee Flexion");
            GraphIcon AnkleFlexImg = new GraphIcon(XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\AnkleFlex.png"), "Ankle Flexion");

            GraphIcon[] icons = new GraphIcon[] { hipFlexImg, hamStringImg, hipAbdImg, hipIntImg, hipExtImg, kneeFlexImg, AnkleFlexImg };

            //center out
            foreach (XPoint p in polyPoints)
                gfx.DrawLine(greenPen, center, p);

            //percentage Lines & icons
            gfx.DrawString(0 + "%", new XFont("Arial", 10), XBrushes.Black, center + new XPoint(5, 0));
            for (int j = 0; j < polyPoints.Length - 1; j++)
            {
                XPoint pt1 = polyPoints[j];
                XPoint pt2 = polyPoints[j + 1];

                for (int i = 10; i > 0; i--)
                {
                    float increment = -i * 0.1f;
                    if (j < 1)
                        gfx.DrawString((i * 10).ToString() + '%', new XFont("Arial", 8), XBrushes.Black, DrawingUtil.Instance.Interpolate(center, polyPoints[0], increment) + new XPoint(5, 0));

                    gfx.DrawLine(perimeterPen,
                        DrawingUtil.Instance.Interpolate(center, pt1, increment),
                        DrawingUtil.Instance.Interpolate(center, pt2, increment));
                }

                XVector vec = new XVector(pt1.X, pt1.Y);
                XVector vec2 = new XVector(center.X, center.Y);

                XImage img = icons[j].img;
                double wRatio = (double)img.PixelWidth / (double)img.PixelHeight;

                XVector dir = vec2 - vec;
                dir.Normalize();
                double txtOffset = dir.X * -10;
                XPoint halfmg = new XPoint(-20, -20);
                XPoint imgpos = new XPoint(dir.X * (-graphSize - 50), dir.Y * (-graphSize - 50)) + center + halfmg;
                gfx.DrawImage(img, new XRect(imgpos, new XSize(wRatio * 60, 60)));
                gfx.DrawString(icons[j].txt, new XFont("Arial", 10), XBrushes.Black, imgpos + new XPoint(txtOffset, -10), XStringFormats.Center);
            }

            //leftSide
            XPoint[] percentagePoints = new XPoint[polyPoints.Length];
            for (int k = 0; k < polyPoints.Length; k++)
            {
                KeyValuePair<string, Parameter> kv = userParameters_.ElementAt(k);
                percentagePoints[k] = DrawingUtil.Instance.Interpolate(center, polyPoints[k], -kv.Value.Percentage);
                gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[k], -kv.Value.RedVal));
            }

            gfx.DrawPolygon(new XPen(XColor.FromArgb(1, 0, 255, 255)),
                new XSolidBrush(XColor.FromArgb(40, 255, 255, 0)),
                percentagePoints,
                XFillMode.Alternate);

            //right side
            for (int k = 10; k < polyPoints.Length + 10; k++)
            {
                KeyValuePair<string, Parameter> kv = userParameters_.ElementAt(k);
                percentagePoints[k - 10] = DrawingUtil.Instance.Interpolate(center, polyPoints[k - 10], -kv.Value.Percentage);
                gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[k - 10], -kv.Value.RedVal));
            }

            gfx.DrawPolygon(new XPen(XColor.FromArgb(1, 0, 255, 255)),
                new XSolidBrush(XColor.FromArgb(40, 54, 127, 180)),
                percentagePoints,
                XFillMode.Alternate);

            XRect leftRectLSI = new XRect(center.X - 250, page_.Height * 0.725, 120, 25);
            XRect rightRectLSI = new XRect(center.X + 70, page_.Height * 0.725, 120, 25);
            XRect LSIRect = new XRect(page_.Width - 100, page_.Height * 0.725, 35, 25);
            gfx.DrawRoundedRectangle(XBrushes.Yellow, leftRectLSI, new XSize(10, 10));
            gfx.DrawRoundedRectangle(XBrushes.CornflowerBlue, rightRectLSI, new XSize(10, 10));
            gfx.DrawRoundedRectangle(XBrushes.LightGray, LSIRect, new XSize(10, 10));
            gfx.DrawString("Left", new XFont("Arial", 14), XBrushes.Black, new XPoint(leftRectLSI.X + 60, leftRectLSI.Y + 12.5), XStringFormats.Center);
            gfx.DrawString("Right", new XFont("Arial", 14), XBrushes.Black, new XPoint(rightRectLSI.X + 60, rightRectLSI.Y + 12.5), XStringFormats.Center);
            gfx.DrawString("LSI", new XFont("Arial", 14), XBrushes.Black, new XPoint(LSIRect.X + 17.5, LSIRect.Y + 10), XStringFormats.Center);

            for (int l = 0; l < polyPoints.Length - 1; l++)
            {
                XBrush leftParamCol = XBrushes.Green;
                XBrush rightParamCol = XBrushes.Green;
                XBrush lsiParamCol = XBrushes.Green;
                
                XFont arial = new XFont("Arial", 13);

                Parameter leftParam = userParameters_.ElementAt(l).Value;
                Parameter rightParam = userParameters_.ElementAt(l + 10).Value;

                if (leftParam.Color == "Amber")
                    leftParamCol = new XSolidBrush(XColor.FromArgb(199, 171, 14));
                else if (leftParam.Color == "Red")
                    leftParamCol = XBrushes.Red;

                if (rightParam.Color == "Amber")
                    rightParamCol = new XSolidBrush(XColor.FromArgb(199, 171, 14));
                else if (rightParam.Color == "Red")
                    rightParamCol = XBrushes.Red;

                if (leftParam.LSI < leftParam.RedVal)
                    leftParamCol = XBrushes.Red;
                else if (leftParam.LSI < leftParam.AmberVal)
                    leftParamCol = XBrushes.Yellow;

                char degree = Convert.ToChar('\u00b0');

                double increment = l * page_.Height * 0.025;
                double y = page_.Height * 0.775 + increment;

                XRect rl = new XRect(leftRectLSI.X + (leftRectLSI.Width * 0.5) - 25, y, 50, 15);
                gfx.DrawRoundedRectangle(leftParamCol, rl, new XSize(10, 10));
                gfx.DrawString(leftParam.Value.ToString() + degree, arial, XBrushes.Black, new XPoint(rl.X + 25, rl.Y + 7.5), XStringFormats.Center);

                XRect rr = new XRect(rightRectLSI.X + (rightRectLSI.Width * 0.5) - 25, y, 50, 15);
                gfx.DrawRoundedRectangle(rightParamCol, rr, new XSize(10, 10));
                gfx.DrawString(rightParam.Value.ToString() + degree, arial, XBrushes.Black, new XPoint(rr.X + 25, rr.Y + 7.5), XStringFormats.Center);

                XRect rlsi = new XRect(LSIRect.X + (LSIRect.Width * 0.5) - 17.5, y, 35, 15);
                gfx.DrawRoundedRectangle(lsiParamCol, rlsi, new XSize(10, 10));
                gfx.DrawString(leftParam.LSI.ToString() + degree, arial, XBrushes.Black, new XPoint(rlsi.X + 17.5, rlsi.Y + 7.5), XStringFormats.Center);

                gfx.DrawString(leftParam.Name.Substring(4,leftParam.Name.Length - 4), new XFont("Arial", 10), XBrushes.Black,
                    DrawingUtil.Instance.Interpolate(new XPoint(rl.X + rl.Width, y + 7.5), new XPoint(rr.X, y), -0.5), XStringFormats.TopCenter);

                gfx.DrawLine(XPens.DarkSlateGray, new XPoint(leftRectLSI.X, rl.Y + 17.5), new XPoint(rlsi.X + 35, rl.Y + 17.5));
            }

        }

        XPoint NormalizedPT(XPoint point)
        {
            double length = Math.Sqrt((point.X * point.X) + (point.Y * point.Y));

            XPoint norm = new XPoint(point.X / length, point.Y / length);
            return norm;
        }
    }

    public struct GraphIcon
    {
        public XImage img;
        public string txt;

        public GraphIcon(XImage img, string txt)
        {
            this.img = img;
            this.txt = txt;
        }
    }
}
