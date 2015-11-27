using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;

namespace Redback_Report_Generator
{
    class Report_Page
    {
        protected PdfPage page_;
        protected ProfileInfo userProfile_;
        protected Dictionary<string, Parameter> userParameters_ = new Dictionary<string, Parameter>();
        protected XUnit quarterWidth_;

        protected XSolidBrush backgroundBrush;
        protected XSize cornerRadius;

        public Report_Page(PdfPage page, ProfileInfo userProfile, List<Parameter> userParameters)
        {
            this.page_ = page;
            this.userProfile_ = userProfile;

            foreach (Parameter p in userParameters)
                this.userParameters_.Add(p.Name.TrimEnd(), p);

            quarterWidth_ = page_.Width / 4;
            cornerRadius = new XSize(40, 40);
            backgroundBrush = new XSolidBrush(XColor.FromKnownColor(XKnownColor.Gray));

        }

        public virtual void DrawHeader(XGraphics gfx, string reportName)
        {
            XRect userDetailsRect = new XRect(20, page_.Height * 0.025, quarterWidth_, page_.Height * 0.075);
            gfx.DrawRoundedRectangle(new XSolidBrush(XColor.FromKnownColor(XKnownColor.Gray)),
                userDetailsRect,
                new XSize(20, 20));

            XRect innerRect = new XRect(userDetailsRect.X + 10, userDetailsRect.Y + 10,
                userDetailsRect.Width * 0.3, userDetailsRect.Height - 20);
            gfx.DrawRoundedRectangle(new XSolidBrush(XColor.FromKnownColor(XKnownColor.LightGray)),
                innerRect,
                new XSize(10, 10));

            gfx.DrawString("Name : ", new XFont("Arial", 10), XBrushes.Black, innerRect.X + 10, innerRect.Y + 10);
            gfx.DrawString(userProfile_.Name, new XFont("Arial", 10), XBrushes.Black, innerRect.X + innerRect.Width + 10, innerRect.Y + 10);

            gfx.DrawString("RB ID : ", new XFont("Arial", 10), XBrushes.Black, innerRect.X + 10, innerRect.Y + 30);
            gfx.DrawString(userProfile_.RBID.Replace(",", ""), new XFont("Arial", 10), XBrushes.Black, innerRect.X + innerRect.Width + 10, innerRect.Y + 30);

            XImage headerImage = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\Redback 3D logo.png");
            gfx.DrawImage(headerImage, new XPoint((quarterWidth_ * 2.5) - (headerImage.PixelWidth / 2), page_.Height * 0.05));

            XSolidBrush brush = new XSolidBrush();
            double score = userProfile_.Score;

            //needs to reference userprofile scores
            if (score <= 33)
                brush = new XSolidBrush(XColors.Red);
            else if (score > 33 && score <= 66)
                brush = new XSolidBrush(XColor.FromArgb(255, 255, 190, 0));
            else
                brush = new XSolidBrush(XColors.Green);

            XRect scoreRect = new XRect((quarterWidth_ * 3) - 20, page_.Height * 0.025, quarterWidth_, page_.Height * 0.075);
            gfx.DrawRoundedRectangle(new XSolidBrush(XColor.FromKnownColor(XKnownColor.Gray)),
                scoreRect,
                new XSize(20, 20));

            XRect innerRectScore = new XRect(scoreRect.X + (scoreRect.Width - 50), scoreRect.Y + 10,
                scoreRect.Height - 20, scoreRect.Height - 20);
            gfx.DrawRoundedRectangle(brush,
                innerRectScore,
                new XSize(10, 10));

            //needs to be variable
            gfx.DrawString(reportName + " : ", new XFont("Arial", 10),
                XBrushes.White, scoreRect.X + 10, scoreRect.Y + 35);

            gfx.DrawString(userProfile_.Score.ToString("0") + "%", new XFont("Arial", 10),
                XBrushes.White, innerRectScore.X + 5, innerRectScore.Y + 25);
        }
    }
}
