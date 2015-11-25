using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Redback_Report_Generator
{
    class ROM_Page : Report_Page
    {
        public ROM_Page(PdfPage page, ProfileInfo userInfo, List<Parameter> userParameters) :
            base(page, userInfo, userParameters)
        { }

        public override void DrawHeader(XGraphics gfx)
        {
            base.DrawHeader(gfx);
        }

        public void DrawGraph(XGraphics gfx)
        {
            double yOff = page_.Height * 0.11;

            XRect rect = new XRect(20, yOff, page_.Width - 40, page_.Height - (yOff));
            gfx.DrawRoundedRectangle(backgroundBrush, rect, cornerRadius);

            XPoint[] polyPoints = DrawingUtil.Instance.GeneratePoints(new XPoint(page_.Width * 0.5, page_.Height * 0.5), (float)page_.Height * 0.225f, 7, gfx);
            XPoint center = new XPoint(page_.Width * 0.5, page_.Height * 0.5);
            gfx.DrawPolygon(XBrushes.DimGray, polyPoints, XFillMode.Winding);

            XPen yellowPen = new XPen(XColors.Yellow, 2.5);
            XPen greenPen = new XPen(XColors.Green, 2.5);
            XPen perimeterPen = XPens.LightGray;
            XPen redPen = new XPen(XColors.Red, 2.5);

            //center out
            foreach (XPoint p in polyPoints)
                gfx.DrawLine(greenPen, center, p);

            //percentage Lines
            gfx.DrawString(0 + "%", new XFont("Arial", 10), XBrushes.Black, center + new XPoint(5, 0));
            for (int j = 0; j < polyPoints.Length - 1; j++)
            {
                XPoint pt1 = polyPoints[j];
                XPoint pt2 = polyPoints[j + 1];

                for (int i = 10; i > 0; i--)
                {
                    float increment = -i * 0.1f;
                    if(j < 1)
                        gfx.DrawString((i * 10).ToString() + '%', new XFont("Arial", 8), XBrushes.Black, DrawingUtil.Instance.Interpolate(center, polyPoints[0], increment) + new XPoint(5, 0));

                    gfx.DrawLine(perimeterPen,
                        DrawingUtil.Instance.Interpolate(center, pt1, increment),
                        DrawingUtil.Instance.Interpolate(center, pt2, increment));
                }
            }

            XPoint[] percentagePoints = new XPoint[polyPoints.Length];
            for(int k = polyPoints.Length - 1; k > -1;k--)
            {
                KeyValuePair<string, Parameter> kv = userParameters_.ElementAt(k);
                percentagePoints[k] = DrawingUtil.Instance.Interpolate(center, polyPoints[k], kv.Value.Percentage);
            }
            XPoint leftHipFlex = DrawingUtil.Instance.Interpolate(center, polyPoints[0], -userParameters_["LEFT Hip Flexion"].Percentage);
            XPoint leftHamstringExt = DrawingUtil.Instance.Interpolate(center, polyPoints[1], -userParameters_["LEFT Hamstring Extension"].Percentage);
            XPoint leftHipAbd = DrawingUtil.Instance.Interpolate(center, polyPoints[2], -userParameters_["LEFT Hip Abduction"].Percentage);
            XPoint leftHipInternalRot = DrawingUtil.Instance.Interpolate(center, polyPoints[3], -userParameters_["LEFT Hip Internal Rotation"].Percentage);
            XPoint leftHipExternalRot = DrawingUtil.Instance.Interpolate(center, polyPoints[4], -userParameters_["LEFT Hip External Rotation"].Percentage);
            XPoint leftKneeFlex = DrawingUtil.Instance.Interpolate(center, polyPoints[5], -userParameters_["LEFT Knee Flexion"].Percentage);
            XPoint leftAnkleFlex = DrawingUtil.Instance.Interpolate(center, polyPoints[6], -userParameters_["LEFT Ankle Flexion"].Percentage);
            //XPoint leftShoulderFlex = DrawingUtil.Instance.Interpolate(center, polyPoints[7], -userParameters_["LEFT Shoulder Flexion"].Percentage);
            //XPoint leftShoulderInternalRot = DrawingUtil.Instance.Interpolate(center, polyPoints[8], -userParameters_["LEFT Shoulder Int Rotation"].Percentage);
            //XPoint leftShoulderExternalRot = DrawingUtil.Instance.Interpolate(center, polyPoints[9], -userParameters_["LEFT Shoulder Ext Rotation"].Percentage);

            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[0], -userParameters_["LEFT Hip Flexion"].RedVal));
            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[1], -userParameters_["LEFT Hamstring Extension"].RedVal));
            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[2], -userParameters_["LEFT Hip Abduction"].RedVal));
            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[3], -userParameters_["LEFT Hip Internal Rotation"].RedVal));
            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[4], -userParameters_["LEFT Hip External Rotation"].RedVal));
            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[5], -userParameters_["LEFT Knee Flexion"].RedVal));
            gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[6], -userParameters_["LEFT Ankle Flexion"].RedVal));
            //gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[7], -userParameters_["LEFT Shoulder Flexion"].RedVal));
            //gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[8], -userParameters_["LEFT Shoulder Int Rotation"].RedVal));
            //gfx.DrawLine(redPen, center, DrawingUtil.Instance.Interpolate(center, polyPoints[9], -userParameters_["LEFT Shoulder Ext Rotation"].RedVal));

            //gfx.DrawLines(XPens.Orange, new XPoint[] {leftHipFlex, leftHamstringExt, leftHipAbd, leftHipInternalRot, leftHipExternalRot, leftKneeFlex, leftAnkleFlex });
            gfx.DrawPolygon(new XPen(XColor.FromArgb(1, 0, 255, 255)),
                new XSolidBrush(XColor.FromArgb(40, 255, 255, 0)),
                new XPoint[] { leftHipFlex, leftHamstringExt, leftHipAbd, leftHipInternalRot, leftHipExternalRot, leftKneeFlex, leftAnkleFlex },
                XFillMode.Alternate);
        }
    }
}
