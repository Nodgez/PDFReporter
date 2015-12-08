using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redback_Report_Generator
{
    class Cover_Page
    {
        XGraphics gfx;
        double centerX;

        public Cover_Page(PdfPage page, ProfileInfo userProfile, ReportType reportType, XGraphics gfx)
        {
            centerX = page.Width * 0.5;
            XRect headingRect = new XRect(page.Width * 0.1, page.Height * 0.05, page.Width * 0.8, page.Height * 0.125);
            string heading = "";
            switch (reportType)
            {
                case ReportType.TMS:
                    heading = "Team Movement Screen Report";
                    break;
                case ReportType.ROM:
                    heading = "Range of Movement Report";
                    break;
                case ReportType.OHS:
                    heading = "Overhead Squat Report";
                    break;
                case ReportType.LNG:
                    heading = "Lunge Report";
                    break;
            }

            gfx.DrawRoundedRectangle(XBrushes.DimGray, headingRect, new XSize(20, 20));
            gfx.DrawString(heading, new XFont("Arial", 26), XBrushes.Black, new XPoint(page.Width * 0.5, page.Height * 0.09), XStringFormats.Center);
            gfx.DrawString(@"""Movement in Motion""", new XFont("Arial", 14), XBrushes.White, new XPoint(page.Width * 0.5, page.Height * 0.15), XStringFormats.Center);

            XRect profileRect = new XRect(page.Width * 0.25, page.Height * 0.375, page.Width * 0.5f, page.Height * 0.1);
            gfx.DrawRoundedRectangle(XBrushes.DimGray, profileRect, new XSize(20, 20));

            XRect tagRect1 = new XRect(profileRect.X + page.Width * 0.015, profileRect.Y + page.Height * 0.015, page.Width * 0.09, page.Height * 0.07);
            gfx.DrawRoundedRectangle(XBrushes.LightGray, tagRect1, new XSize(10, 10));


            XRect tagRect2 = new XRect(profileRect.X + page.Width * 0.25, profileRect.Y + page.Height * 0.015, page.Width * 0.09, page.Height * 0.07);
            gfx.DrawRoundedRectangle(XBrushes.LightGray, tagRect2, new XSize(10, 10));


            XRect infoRect = new XRect(page.Width * 0.1, page.Height * 0.5, page.Width * 0.8, page.Height * 0.45);
            gfx.DrawRoundedRectangle(XBrushes.DimGray, infoRect, new XSize(20, 20));
        }
    }
}
