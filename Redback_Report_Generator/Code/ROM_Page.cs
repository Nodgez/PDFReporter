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
    }
}
