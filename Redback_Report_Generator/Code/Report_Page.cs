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
        protected List<Parameter> userParameters_ = new List<Parameter>();
        protected XUnit quarterWidth_;

        protected XSolidBrush backgroundBrush;
        protected XSize cornerRadius;
        protected int dataReadStart = 0;
        protected XFont arialSmall;
        protected XFont arialLarge;
        protected XFont arialSmallBold;

        public Report_Page(PdfPage page, ProfileInfo userProfile, List<Parameter> userParameters, int dataReadStart = 0)
        {
            this.page_ = page;
            this.userProfile_ = userProfile;

            this.userParameters_ = userParameters;
            this.dataReadStart = dataReadStart;

            quarterWidth_ = page_.Width / 4;
            cornerRadius = new XSize(40, 40);
            backgroundBrush = new XSolidBrush(XColor.FromKnownColor(XKnownColor.Gray));

            arialLarge = new XFont("arial" ,20);
            arialSmall = new XFont("arial", 12);
            arialSmallBold = new XFont("arial", 12, XFontStyle.Bold);
        }

        public virtual void DrawHeader(XGraphics gfx, string reportName, int score)
        {
            XRect userDetailsRect = new XRect(20, page_.Height * 0.025, quarterWidth_, page_.Height * 0.075);
            DrawingUtil.DrawOutlineRect(userDetailsRect, gfx, new XSize(20, 20));
            gfx.DrawRoundedRectangle(new XSolidBrush(XColor.FromKnownColor(XKnownColor.Gray)),
                userDetailsRect,
                new XSize(20, 20));

            XRect innerRect = new XRect(userDetailsRect.X + 10, userDetailsRect.Y + 10,
                userDetailsRect.Width * 0.3, userDetailsRect.Height - 20);
            DrawingUtil.DrawOutlineRect(innerRect,gfx, new XSize(10, 10));
            gfx.DrawRoundedRectangle(new XSolidBrush(XColor.FromKnownColor(XKnownColor.LightGray)),
                innerRect,
                new XSize(10, 10));

            gfx.DrawString("Name : ", new XFont("Arial", 10), XBrushes.Black, innerRect.X + 10, innerRect.Y + 10);
            gfx.DrawString(userProfile_.Name, new XFont("Arial", 10), XBrushes.Black, innerRect.X + innerRect.Width + 10, innerRect.Y + 10);

            gfx.DrawString("RB ID : ", new XFont("Arial", 10), XBrushes.Black, innerRect.X + 10, innerRect.Y + 30);
            gfx.DrawString(userProfile_.RBID.Replace(",", ""), new XFont("Arial", 10), XBrushes.Black, innerRect.X + innerRect.Width + 10, innerRect.Y + 30);

            XImage headerImage = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\Redback 3D logo.png");
            gfx.DrawImage(headerImage, new XPoint((quarterWidth_ * 2.5) - (headerImage.PixelWidth / 2), page_.Height * 0.05));

            XBrush brush = new XSolidBrush();

            brush = DrawingUtil.Instance.ChooseBrushColor(score, 49, 74);

            XRect scoreRect = new XRect((quarterWidth_ * 3) - 20, page_.Height * 0.025, quarterWidth_, page_.Height * 0.075);
            DrawingUtil.DrawOutlineRect(scoreRect, gfx, new XSize(20, 20));
            gfx.DrawRoundedRectangle(new XSolidBrush(XColor.FromKnownColor(XKnownColor.Gray)),
                scoreRect,
                new XSize(20, 20));

            XRect innerRectScore = new XRect(scoreRect.X + (scoreRect.Width - 50), scoreRect.Y + 10,
                scoreRect.Height - 20, scoreRect.Height - 20);
            DrawingUtil.DrawOutlineRect(innerRectScore, gfx, new XSize(10, 10));
            gfx.DrawRoundedRectangle(brush,
                innerRectScore,
                new XSize(10, 10));

            //needs to be variable
            gfx.DrawString(reportName + " : ", new XFont("Arial", 10),
                XBrushes.Black, scoreRect.X + 10, scoreRect.Y + 35);

            gfx.DrawString(score+ "%", new XFont("Arial", 10),
                XBrushes.Black, innerRectScore.X + 5, innerRectScore.Y + 25);
        }
    }
}
