using System;
using System.Collections.Generic;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace Redback_Report_Generator
{
    public abstract class Pagel
    {
        public XRect Rectangle { get;private set; }
        public Pagel Parent { get;private set; }

        protected List<Pagel> children = new List<Pagel>();
        protected XPoint minPoint;
        protected XPoint maxPoint;

        public Pagel()
        { }

        public Pagel(Pagel parent, XRect percetileRectangle)
        {
            parent.AddChild(this);
            this.minPoint = new XPoint(parent.Rectangle.X * percetileRectangle.X, parent.Rectangle.Y * percetileRectangle.Y);
            this.maxPoint = minPoint + new XPoint(parent.Rectangle.Width * percetileRectangle.Width,
                parent.Rectangle.Height * percetileRectangle.Height);
            this.Rectangle = new XRect(minPoint, maxPoint);
        }

        public virtual void AddChild(Pagel pagel)
        {
            pagel.Parent = this;
            children.Add(pagel);
        }

        public virtual Pagel GetChild(int index)
        {
            if (index < children.Count)
                return children[index];
            else
                return null;
        }
    }
}
