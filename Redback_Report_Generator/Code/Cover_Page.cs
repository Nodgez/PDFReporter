using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redback_Report_Generator
{
    class Cover_Page
    {
        XGraphics gfx;
        double centerX;

        public Cover_Page(PdfPage page, ProfileInfo userProfile, XGraphics gfx)
        {
            centerX = page.Width * 0.5;
            XRect headingRect = new XRect(page.Width * 0.1, page.Height * 0.05, page.Width * 0.8, page.Height * 0.125);
            string heading = userProfile.reportHeading;

            gfx.DrawRoundedRectangle(XBrushes.DimGray, headingRect, new XSize(20, 20));
            gfx.DrawString(heading, new XFont("Arial", 26), XBrushes.Black, new XPoint(page.Width * 0.5, page.Height * 0.09), XStringFormats.Center);
            gfx.DrawString(@"""Movement in Motion""", new XFont("Arial", 14), XBrushes.White, new XPoint(page.Width * 0.5, page.Height * 0.15), XStringFormats.Center);

            XImage img = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\logo.png");
            gfx.DrawImage(img, new XPoint(centerX - (img.PointWidth / 2), page.Height * 0.2));

            XRect profileRect = new XRect(page.Width * 0.25, page.Height * 0.375, page.Width * 0.5f, page.Height * 0.1);
            gfx.DrawRoundedRectangle(XBrushes.DimGray, profileRect, new XSize(20, 20));

            XRect tagRect1 = new XRect(profileRect.X + page.Width * 0.015, profileRect.Y + page.Height * 0.015, page.Width * 0.09, page.Height * 0.07);
            gfx.DrawRoundedRectangle(XBrushes.LightGray, tagRect1, new XSize(10, 10));
            gfx.DrawString("Name : ", new XFont("Arial", 10), XBrushes.Black, tagRect1.X + 5, tagRect1.Y + 10);
            gfx.DrawString(userProfile.Name, new XFont("Arial", 10), XBrushes.Black, tagRect1.X + tagRect1.Width + 10, tagRect1.Y + 10);
            gfx.DrawString("RB ID : ", new XFont("Arial", 10), XBrushes.Black, tagRect1.X + 5, tagRect1.Y + 30);
            gfx.DrawString(userProfile.RBID.Replace(",", ""), new XFont("Arial", 10), XBrushes.Black, tagRect1.X + tagRect1.Width + 10, tagRect1.Y + 30);
            gfx.DrawString("Gender : ", new XFont("Arial", 10), XBrushes.Black, tagRect1.X + 5, tagRect1.Y + 50);
            gfx.DrawString(userProfile.Gender.Replace(",", ""), new XFont("Arial", 10), XBrushes.Black, tagRect1.X + tagRect1.Width + 10, tagRect1.Y + 50);

            XRect tagRect2 = new XRect(profileRect.X + page.Width * 0.25, profileRect.Y + page.Height * 0.015, page.Width * 0.09, page.Height * 0.07);
            gfx.DrawRoundedRectangle(XBrushes.LightGray, tagRect2, new XSize(10, 10));
            gfx.DrawString("Date : ", new XFont("Arial", 10), XBrushes.Black, tagRect2.X + 5, tagRect2.Y + 10);

            gfx.DrawString(userProfile.Date.ToShortDateString(), new XFont("Arial", 10), XBrushes.Black, tagRect2.X + tagRect2.Width + 10, tagRect2.Y + 10);
            gfx.DrawString("Opperator : ", new XFont("Arial", 10), XBrushes.Black, tagRect2.X + 5, tagRect2.Y + 30);
            gfx.DrawString(userProfile.Opperator.Replace(",", ""), new XFont("Arial", 10), XBrushes.Black, tagRect2.X + tagRect2.Width + 10, tagRect2.Y + 30);
            gfx.DrawString("Sport : ", new XFont("Arial", 10), XBrushes.Black, tagRect2.X + 5, tagRect2.Y + 50);
            gfx.DrawString(userProfile.Sport.Replace(",", ""), new XFont("Arial", 10), XBrushes.Black, tagRect2.X + tagRect2.Width + 10, tagRect2.Y + 50);


            XRect infoRect = new XRect(page.Width * 0.1, page.Height * 0.5, page.Width * 0.8, page.Height * 0.45);
            gfx.DrawRoundedRectangle(XBrushes.DimGray, infoRect, new XSize(20, 20));

            XRect descriptionRect = new XRect(infoRect.X + page.Width * 0.075f, infoRect.Y + page.Height * 0.025, page.Width * 0.65, page.Height * 0.15);
            gfx.DrawRoundedRectangle(XBrushes.LightGray, descriptionRect, new XSize(10, 10));
            string r = userProfile.ReportText;
            XTextFormatter frm = new XTextFormatter(gfx);
            frm.DrawString(r, new XFont("Arial", 12), XBrushes.Black, descriptionRect, XStringFormats.TopLeft);

            XImage img1 = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\CL.png");
            XImage img2 = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\PS.png");
            XImage img3 = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\CR.png");

            double wRatio = (double)img1.PixelWidth / (double)img1.PixelHeight;
            gfx.DrawImage(img1, new XRect(new XPoint(infoRect.X + page.Width * 0.25 - (img1.PointWidth / 2), infoRect.Y + page.Height * 0.2),
                new XSize(wRatio * 100, 100)));

            wRatio = (double)img2.PixelWidth / (double)img2.PixelHeight;
            gfx.DrawImage(img2, new XRect(new XPoint(infoRect.X + page.Width * 0.5 - (img2.PointWidth / 2), infoRect.Y + page.Height * 0.2),
                new XSize(wRatio * 100, 100)));

            wRatio = (double)img3.PixelWidth / (double)img3.PixelHeight;
            gfx.DrawImage(img3, new XRect(new XPoint(infoRect.X + page.Width * 0.75 - (img3.PointWidth / 2), infoRect.Y + page.Height * 0.2),
                new XSize(wRatio * 100, 100)));

            XRect indicatorsRect = new XRect(infoRect.X + page.Width * 0.075f, infoRect.Y + page.Height * 0.34, page.Width * 0.65, page.Height * 0.1);
            gfx.DrawRoundedRectangle(XBrushes.LightGray, indicatorsRect, new XSize(10, 10));

            XRect colorRect = new XRect(indicatorsRect.X + indicatorsRect.Width * 0.01, indicatorsRect.Y + indicatorsRect.Height * 0.01, 20, 12);
            double txtOffset = colorRect.X + colorRect.Width * 1.7;

            gfx.DrawRoundedRectangle(DrawingUtil.Instance.ChooseBrushColor("Red"), colorRect, new XSize(5, 5));
            frm.DrawString("Requires further ongoing 3D screening or immediate intervention from a qualified practitioner", new XFont("Arial", 10), XBrushes.Black,
                new XRect(txtOffset, colorRect.Y, indicatorsRect.Width, indicatorsRect.Height));

            colorRect = new XRect(indicatorsRect.X + indicatorsRect.Width * 0.01, indicatorsRect.Y + indicatorsRect.Height * 0.41, 20, 12);
            txtOffset = colorRect.X + colorRect.Width * 1.7;
            gfx.DrawRoundedRectangle(DrawingUtil.Instance.ChooseBrushColor("Amber"), colorRect, new XSize(5, 5));
            frm.DrawString("Requires follow up intervention in the form of corrective exercises from a qualified health practitioner", new XFont("Arial", 10), XBrushes.Black,
        new XRect(txtOffset, colorRect.Y, indicatorsRect.Width - 20, indicatorsRect.Height));

            colorRect = new XRect(indicatorsRect.X + indicatorsRect.Width * 0.01, indicatorsRect.Y + indicatorsRect.Height * 0.81, 20, 12);
            txtOffset = colorRect.X + colorRect.Width * 1.7;
            gfx.DrawRoundedRectangle(DrawingUtil.Instance.ChooseBrushColor("Green"), colorRect, new XSize(5, 5));
            frm.DrawString("The individual demonstrates optimal movement competency", new XFont("Arial", 10), XBrushes.Black,
    new XRect(txtOffset, colorRect.Y, indicatorsRect.Width, indicatorsRect.Height));

        }

        private static int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }

        string Wrap(string text, int wordCount)
        {
            string tmpText = " ";
            int index = 0;
            int words = 0;
            int marker = 0;
            while (index < text.Length)
            {
                char sample = text[index];
                if (sample == ' ' || sample == ':')
                {
                    words++;

                    if (words >= wordCount || index == text.Length - 1 )
                    {
                        tmpText = tmpText + text.Substring(marker, index - marker) + '\n';
                        marker = index;
                        words = 0;
                    }
                }
                index++;
            }

            return tmpText;
        }

        public static string WordWrap(string text, int width)
        {
            int pos, next;
            StringBuilder sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return text;

            // Parse each line of text
            for (pos = 0; pos < text.Length; pos = next)
            {
                // Find end of line
                int eol = text.IndexOf(Environment.NewLine, pos);
                if (eol == -1)
                    next = eol = text.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > width)
                            len = BreakLine(text, pos, width);
                        sb.Append(text, pos, len);
                        sb.Append(Environment.NewLine);

                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(text[pos]))
                            pos++;
                    } while (eol > pos);
                }
                else sb.Append(Environment.NewLine); // Empty line
            }
            return sb.ToString() + "\n";
        }
    }
}
