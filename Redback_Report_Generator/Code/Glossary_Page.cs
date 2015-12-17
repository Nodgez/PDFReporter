using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Drawing.Layout;
using System.IO;

namespace Redback_Report_Generator
{
    class Glossary_Page
    {
        public Glossary_Page(PdfPage page, XGraphics gfx)
        {
            XRect headRect = new XRect(page.Width * 0.05, page.Height * 0.05, page.Width * 0.9, page.Height * 0.1);
            gfx.DrawRoundedRectangle(XBrushes.Gray, headRect, new XSize(20, 20));

            gfx.DrawString("Report Glossary", new XFont("Arial", 24), XBrushes.Black, new XPoint(page.Width * 0.5, headRect.Y + headRect.Height * 0.5), XStringFormats.Center);

            //Parameters
            XRect mainRect = new XRect(page.Width * 0.05, page.Height * 0.175, page.Width * 0.9, page.Height * 0.5);
            gfx.DrawRoundedRectangle(XBrushes.Gray, mainRect, new XSize(20, 20));

            XFont parial = new XFont("Arial", 8);
            XTextFormatter frm = new XTextFormatter(gfx);
            double txtOffset = mainRect.X + mainRect.Width * 0.3;

            gfx.DrawString("Parameters :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.05));
            gfx.DrawLine(XPens.White, new XPoint(mainRect.X, mainRect.Y + mainRect.Height * 0.075), new XPoint(mainRect.X + mainRect.Width, mainRect.Y + mainRect.Height * 0.075));

            gfx.DrawString("Hip Flexion :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.255));
            frm.DrawString("is a measurement of the ability of the individual to move their thigh toward the torso when squatting", parial, XBrushes.Black,
                new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.225, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("Hip Abduction :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.31));
            frm.DrawString("Is a measurement of the femur's movement away from the mid-line positon of the body", parial, XBrushes.Black,
                new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.285, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("Hip Internal Rotation :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.365));
            frm.DrawString("Is a measurement of the hip joints ability to rotate in an inward motion towards the mid-line position of the body", parial, XBrushes.Black,
    new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.345, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("Hip External Rotation :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.42));
            frm.DrawString("Is a measurement of the hip joints ability to rotate in an outward motion away from the mid-line position of the body", parial, XBrushes.Black,
    new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.4, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("Shoulder Flexion :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.475));
            frm.DrawString("is a measurement of the ability of the individual to raise their hand over the head in a forward motion", parial, XBrushes.Black,
    new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.455, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("Knee Flexion :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.53));
            frm.DrawString("is a measurement of the range of motion at the knee when squatting", parial, XBrushes.Black,
    new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.51, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("Ankle Flexion :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.585));
            frm.DrawString("is a measurement of the range of motion into dorsiflexion at the ankle joint when squatting", parial, XBrushes.Black,
    new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.565, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("Tibia / Spine Alignment :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.64));
            frm.DrawString("is a measure of the ability of the individual's ability to maintain the spine in neutral during the squat", parial, XBrushes.Black,
    new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.62, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("Pelvic Stability :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.695));
            frm.DrawString("is a measure of the ability of the individual's ability to keep the pelvis stable during the squat", parial, XBrushes.Black,
    new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.675, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("Knee Stability :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.75));
            frm.DrawString("is a measurement of the individuals ability to keep the knees in neutral alignment during the squat", parial, XBrushes.Black,
    new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.73, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("Depth of Squat :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.805));
            frm.DrawString("is a measurement of how deep the individual is able to squat", parial, XBrushes.Black,
    new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.785, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("Spinal Alignment :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.86));
            frm.DrawString("Is a measurement of the ability of the individual to maintain neutral spine angle throughout the squat pattern", parial, XBrushes.Black,
    new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.84, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);

            gfx.DrawString("LSI :", parial, XBrushes.White, new XPoint(mainRect.X + mainRect.Width * 0.1, mainRect.Y + mainRect.Height * 0.915));
            frm.DrawString("Limb Symmetry Index: LSI is a value given to the symmetry between both left and right of the body I.e Left and right knee flexion", parial, XBrushes.Black,
    new XRect(txtOffset, mainRect.Y + mainRect.Height * 0.895, mainRect.Width * 0.7, mainRect.Height), XStringFormats.TopLeft);


            XRect contactRect = new XRect(page.Width * 0.05, page.Height * 0.7, page.Width * 0.9, page.Height * 0.25);
            gfx.DrawRoundedRectangle(XBrushes.Gray, contactRect, new XSize(20, 20));

            XImage img = XImage.FromFile(Directory.GetCurrentDirectory() + @"\Content\logo.png");
            double rat = (double)img.PixelWidth / (double)img.PixelHeight;
            gfx.DrawImage(img, new XRect((page.Width * 0.5) - 50, contactRect.Y + contactRect.Height * 0.05, rat * 50, 50));

            string t = "Redback Biotek is a global leader in the field of 3D biomechanical analysis and is supported by a world -class" +
                " team of clinicians and fitness experts.Our commitment is to provide fast," +
                " accurate and easy to understand reporting that offer you qualitative insight into your movement competency." +
                "\n\nIf you require more details about our 3D bio-mechanical analysis or would like to make another appointment for a follow-up screen please contact Paul Clarke:";

            frm.DrawString(t, parial, XBrushes.Black, new XRect(contactRect.X + 20, contactRect.Y + 75, contactRect.Width - 40, contactRect.Height));

            gfx.DrawString("+353 86 8595155", parial, XBrushes.Black, new XPoint(contactRect.X + contactRect.Width * 0.035, contactRect.Y + contactRect.Height - 40));
            gfx.DrawString("paul@redbackbiotek.com", parial, XBrushes.Black, new XPoint(contactRect.X + contactRect.Width -(contactRect.Width * 0.25), contactRect.Y + contactRect.Height - 40));
            t = "Redback Biotek is the owner of all intellectual property rights in this report and associated material published on it.Those works are protected by copyright laws and treaties around the world.All such rights are reserved.";
            frm.DrawString(t, new XFont("Arial", 5), XBrushes.Black, new XRect(contactRect.X + 20, contactRect.Y + contactRect.Height - 17.5, contactRect.Width - 40, contactRect.Height));
        }
    }
}
