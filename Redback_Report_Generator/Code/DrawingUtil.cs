﻿using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redback_Report_Generator
{
    public class DrawingUtil
    {
        private static DrawingUtil _instance;

        private DrawingUtil()
        { }

        public XBrush ChooseBrushColor(double value, double redVal, double amberVal)
        {
            if (value <= redVal)
                return new XSolidBrush(XColor.FromArgb(255, 0, 0));
            else if (value > redVal && value <= amberVal)
                return new XSolidBrush(XColor.FromArgb(255, 204, 0));

            return new XSolidBrush(XColor.FromArgb(0, 153, 51));

        }

        public XBrush ChooseBrushColor(string value)
        {
            if (value == "Red")
                return new XSolidBrush(XColor.FromArgb(255, 0, 0));
            else if (value ==  "Amber")
                return new XSolidBrush(XColor.FromArgb(255, 204, 0));

            return new XSolidBrush(XColor.FromArgb(0, 153, 51));

        }

        public XPoint[] GeneratePoints(XPoint center, float size, int sides, XGraphics gfx, int angleOffset = 270)
        {
            XPoint[] points = new XPoint[sides + 1];
            float degreeSegments = 360.0f / sides;

            //calculte all of the point in the polygon
            for (int i = 0; i < sides; i++)
            {
                float angle = i * degreeSegments + angleOffset;
                angle *= ((float)Math.PI / 180);
                XPoint location = new XPoint(size * Math.Cos(angle) + center.X,
                    size * Math.Sin(angle) + center.Y);

                points[i] = location;
            }
            points[sides] = points[0];
            return points;
        }

        public XPoint Interpolate(XPoint pt1, XPoint pt2, double amount)
        {
            return pt1 + amount * (pt1 - pt2);
        }

        public double pointLength(XPoint p1)
        {
            return Math.Sqrt((p1.X * p1.X) + (p1.Y * p1.Y));
        }

        public double Distance(XPoint p1, XPoint p2)
        {
            double exes = p1.X - p2.X;
            double whys = p1.Y - p2.Y;
            return Math.Sqrt((exes * exes) + (whys * whys));
        }

        public static DrawingUtil Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DrawingUtil();

                return _instance;
            }
        }

        public static void DrawOutlineRect(XRect rect, XGraphics gfx, XSize rounded)
        {
            rect = new XRect(rect.Left - 1,rect.Top - 1, rect.Width + 2, rect.Height + 2);
            gfx.DrawRoundedRectangle(XBrushes.Black, rect, rounded);
        }
    }
}
