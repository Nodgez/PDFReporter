using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Redback_Report_Generator
{
    class OHS_Page : Report_Page
    {


        public OHS_Page(PdfPage page, ProfileInfo userProfile, List<Parameter> userParameters) :
            base(page, userProfile, userParameters)
        {
        }

        public override void DrawHeader(XGraphics gfx, string reportName)
        {
            base.DrawHeader(gfx, reportName);
        }

        #region pentagon 
        public void DrawPentagon(XGraphics gfx)
        {
            //set up the background grey
            XRect backgroundRect = new XRect(20, page_.Height * 0.11, page_.Width - 40, page_.Height * 0.425);
            gfx.DrawRoundedRectangle(backgroundBrush,
                backgroundRect,
                cornerRadius);
            XPoint center = new XPoint(page_.Width * 0.5, backgroundRect.Y + backgroundRect.Height * 0.6);

            XPoint[] polyPoints = DrawingUtil.Instance.GeneratePoints(center, 130, 5, gfx);
            gfx.DrawPolygon(XBrushes.DimGray, polyPoints, XFillMode.Winding);

            XPen yellowPen = new XPen(XColors.Yellow, 2.5);
            XPen greenPen = new XPen(XColors.Green, 2.5);
            XPen perimeterPen = XPens.LightGray;
            XPen redPen = new XPen(XColors.Red, 2.5);
       
            //center out
            foreach(XPoint p in polyPoints)
                gfx.DrawLine(greenPen, center, p);

            XBrush brush = XBrushes.Black;
            XSize size = new XSize(50, 50);
            XSize ellipseSize = new XSize(10, 10);
            //images
            XImage leftKneeImg = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\Left Knee Stability.png");
            XImage rightKneeImg = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\Right Knee Stability.png");
            XImage tibia_spineImg = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\Tibia Spine Angle.png");
            XImage dosImg = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\Depth of Squat.png");
            XImage pelvicImg = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\Pelvic Stability.png");

            //infoboxes
            DrawPentaInfoBox(gfx, polyPoints[0] + new XPoint(-50, -75), tibia_spineImg, userParameters_["Tibia / Spine Angle"]);
            DrawPentaInfoBox(gfx, polyPoints[1] + new XPoint(25, -35), rightKneeImg, userParameters_["RIGHT Knee Stability"]);
            DrawPentaInfoBox(gfx, polyPoints[2] + new XPoint(25, -60), pelvicImg, userParameters_["Pelvic Stability"]);
            DrawPentaInfoBox(gfx, polyPoints[3] + new XPoint(-125, -60), dosImg, userParameters_["Depth of Squat"]);
            DrawPentaInfoBox(gfx, polyPoints[4] + new XPoint(-100,-35), leftKneeImg, userParameters_["LEFT Knee Stability"]);

            //percentage Lines
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
            }

            XPoint centerTibia = DrawingUtil.Instance.Interpolate(center, polyPoints[0], -userParameters_["Tibia / Spine Angle"].Percentage);
            XPoint centerRightKnee = DrawingUtil.Instance.Interpolate(center, polyPoints[1], -userParameters_["RIGHT Knee Stability"].Percentage);
            XPoint centerPelvicStability = DrawingUtil.Instance.Interpolate(center, polyPoints[2], -userParameters_["Pelvic Stability"].Percentage);
            XPoint centerDos = DrawingUtil.Instance.Interpolate(center, polyPoints[3], -userParameters_["Depth of Squat"].Percentage);
            XPoint centerLeftKnee = DrawingUtil.Instance.Interpolate(center, polyPoints[4], -userParameters_["LEFT Knee Stability"].Percentage);

            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[0], -userParameters_["Tibia / Spine Angle"].RedVal));
            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[1], -userParameters_["RIGHT Knee Stability"].RedVal));
            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[2], -userParameters_["Pelvic Stability"].RedVal));
            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[3], -userParameters_["Depth of Squat"].RedVal));
            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[4], -userParameters_["LEFT Knee Stability"].RedVal));

            gfx.DrawPolygon(new XPen(XColor.FromArgb(1, 0, 255, 255)),
                new XSolidBrush(XColor.FromArgb(40,255,255,0)),
                new XPoint[] { centerTibia, centerRightKnee, centerPelvicStability, centerDos, centerLeftKnee },
                XFillMode.Alternate);

            gfx.DrawLines(new XPen(XColors.Orange, 1), new XPoint[] { centerTibia, centerRightKnee, centerPelvicStability, centerDos, centerLeftKnee, centerTibia });
        }

        void DrawPentaInfoBox(XGraphics gfx, XPoint point, XImage image, Parameter parameter)
        {
            double val = parameter.Value;
            string str = parameter.Name;
            XBrush brush = DrawingUtil.Instance.ChooseBrushColor(parameter.Percentage,
                parameter.RedVal,
                parameter.AmberVal);
            XSize ellipseSize = new XSize(10, 10);

            gfx.DrawRoundedRectangle(brush, new XRect(point.X, point.Y + 10, 55, 30), ellipseSize);
            gfx.DrawString(str, new XFont("Arial", 12), XBrushes.White, point + new XPoint(0,60));
            gfx.DrawString(val.ToString() + " " + parameter.UnitOfMeasure, new XFont("Arial", 10),
                XBrushes.White, new XPoint(point.X + 25,point.Y + 25),XStringFormats.Center);
            gfx.DrawImage(image, new XRect(point + new XPoint(50,0),new XSize(50,50)));
        }
        #endregion

        #region Bar Chart
        public void DrawBarCharts(XGraphics gfx)
        {
            XPoint backgroundPoint = new XPoint(20, page_.Height * 0.55);
            XRect backgroundRect = new XRect(backgroundPoint.X, backgroundPoint.Y, page_.Width - 40, page_.Height * 0.425);
            double quarterRectWidth = backgroundRect.Width * 0.25;
            double offset = quarterRectWidth * 0.25;
            gfx.DrawRoundedRectangle(new XSolidBrush(XColor.FromKnownColor(XKnownColor.Gray)),
                backgroundRect,
                new XSize(40, 40));

            DoubleBar ShoulderFlexionBar = new DoubleBar(userParameters_["LEFT Shoulder Flexion"], userParameters_["RIGHT Shoulder Flexion"], gfx);
            ShoulderFlexionBar.Draw(new XPoint(quarterRectWidth - offset, backgroundPoint.Y + 20),
                backgroundRect,
                XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\Shoulder Flexion.png"));

            DoubleBar hipFlexionBar = new DoubleBar(userParameters_["LEFT Hip Flexion"], userParameters_["RIGHT Hip Flexion"], gfx);
            hipFlexionBar.Draw(new XPoint(quarterRectWidth * 2 - offset, backgroundPoint.Y + 20),
                backgroundRect,
                 XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\Hip Flexion.png"));

            DoubleBar kneeFlexionBar = new DoubleBar(userParameters_["LEFT Knee Flexion"], userParameters_["RIGHT Knee Flexion"], gfx);
            kneeFlexionBar.Draw(new XPoint(quarterRectWidth * 3 - offset, backgroundPoint.Y + 20),
                backgroundRect,
                XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\Knee Flexion.png"));


            DoubleBar ankleFlexionBar = new DoubleBar(userParameters_["LEFT Ankle Flexion"], userParameters_["RIGHT Ankle Flexion"], gfx);
            ankleFlexionBar.Draw(new XPoint(quarterRectWidth * 4 - offset, backgroundPoint.Y + 20),
                backgroundRect,
                XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\Ankle Flexion.png"));

            gfx.DrawString("Degrees :",
                new XFont("Arial", 10),
                XBrushes.Black,
                (backgroundPoint + new XPoint(0, 20)) + new XPoint(backgroundRect.Width * 0.05, backgroundRect.Height * 0.05),
                XStringFormats.Center);

            gfx.DrawString("LSI % :",
                new XFont("Arial", 10),
                XBrushes.Black,
                (backgroundPoint + new XPoint(0, 20)) + new XPoint(backgroundRect.Width * 0.05, backgroundRect.Height * 0.125),
                XStringFormats.Center);

            XPoint top = new XPoint(backgroundPoint.X, backgroundPoint.Y + backgroundRect.Height * 0.2);
            XPoint bottom = new XPoint(backgroundPoint.X, backgroundPoint.Y + backgroundRect.Height * 0.8);
            for (int i = 11; i > 0; i--)
            {
                float increment = -i * 0.1f;
                
                XPoint percentagePoint = DrawingUtil.Instance.Interpolate(top, bottom, increment);
                percentagePoint = new XPoint(percentagePoint.X, Math.Floor(percentagePoint.Y));
                gfx.DrawString(((11 - i) * 10).ToString() + "%", new XFont("Arial", 8), XBrushes.Black, percentagePoint + new XPoint(5, -2));
                gfx.DrawLine(XPens.LightGray, percentagePoint, percentagePoint + new XPoint(backgroundRect.Width, 0));
            }

        }
        #endregion        
    }
}
