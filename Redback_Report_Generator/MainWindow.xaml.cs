using Excel;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;


namespace Redback_Report_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Process pdfProcess;

        public MainWindow()
        {
            InitializeComponent();

            win_Main.MaxHeight = win_Main.MinHeight = win_Main.Height;
            win_Main.MaxWidth = win_Main.MinWidth = win_Main.Width;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.ShowDialog();

            string filePath = open.FileName;
            if (File.Exists(filePath))
            {
                try
                {
                    Generate(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.TargetSite.Name +  " " + ex.Message + "\n\n Contact developer with the above message");
                }
            }
        }

        void Generate(string filePath)
        {
            List<Parameter> userParameters = new List<Parameter>();
            ProfileInfo userProfile = new ProfileInfo();
            ReportType currentReport = ReportType.ROM;

            //get the excel sheet
            var worksheets = Workbook.Worksheets(filePath);
            var sheets = worksheets.ToArray();
            var profileSheet = sheets[1];
            var dataSheet = sheets[2];

            //read the uer profile data
            userProfile.Name = profileSheet.Rows[0].Cells[0].Text;
            userProfile.Date = profileSheet.Rows[1].Cells[0].Text;
            userProfile.Opperator = profileSheet.Rows[2].Cells[0].Text;
            userProfile.RBID = profileSheet.Rows[3].Cells[0].Text;
            userProfile.Sport = profileSheet.Rows[4].Cells[0].Text;
            userProfile.Gender = profileSheet.Rows[5].Cells[0].Text;

            int rowStart = 0;
            int rowEnd = dataSheet.Rows.Length - 5;

            switch (cmbx_Reports.SelectedIndex)
            {
                case 0:
                    userProfile.Report = ReportType.TMS;
                    break;
                case 1:
                    userProfile.Report = ReportType.ROM;
                    rowEnd = 20;
                    break;
                case 2:
                    userProfile.Report = ReportType.OHS;
                    rowStart = 20;
                    rowEnd = 34;
                    break;
                case 3:
                    userProfile.Report = ReportType.LNG;
                    rowStart = 34;
                    rowEnd = 42;
                    break;
                default:
                    userProfile.Report = ReportType.TMS;
                    MessageBox.Show("Unknown Error : Setting report type to TMS");
                    break;
            }

            //error prevention???
            string prefix = "";
            //look through the rows, and decide what report we are dealing with
            for (int row = rowStart; row < rowEnd; row++)
            {
                if (row < 20)
                    currentReport = ReportType.ROM;
                else if (row >= 20 && row < 33)
                    currentReport = ReportType.OHS;
                else
                    currentReport = ReportType.LNG;

                Row currentRow = dataSheet.Rows[row];
                Parameter userParameter = new Parameter();

                string tmpPrefix = currentRow.Cells[1].Text;

                //error prevention???
                if (tmpPrefix != "")
                    prefix = tmpPrefix + " ";

                //loop through the cells on a row basis
                for (int cell = 0; cell < currentRow.Cells.Length; cell++)
                {
                    //get the current cell iinfor inf null move onwards
                    Cell currentCell = currentRow.Cells[cell];
                    if (currentCell == null)
                        continue;

                    switch (currentCell.ColumnIndex)
                    {
                        case 0:
                            userParameter.Value = Convert.ToDouble(currentCell.Text);
                            break;
                        case 1:
                            userParameter.Percentage = Convert.ToDouble(currentCell.Text) * 0.01;
                            if (userParameter.Percentage < 0.0f)
                                userParameter.Percentage = 0.0f;
                            break;
                        case 2:
                            userParameter.Color = currentCell.Text;
                            break;
                        case 3:
                            if (currentCell.Text != "")
                                userParameter.LSI = Convert.ToDouble(currentCell.Text); break;
                        case 4:
                            userParameter.UnitOfMeasure = currentCell.Text;
                            break;
                        case 5:
                            userParameter.RedVal = Convert.ToDouble(currentCell.Text) * 0.01;
                            break;
                        case 6:
                            if (currentCell.Text != "NA")
                                userParameter.AmberVal = Convert.ToDouble(currentCell.Text) * 0.01;
                            else userParameter.AmberVal = 0.0;
                            break;
                        case 7:
                            userParameter.GreenVal = Convert.ToDouble(currentCell.Text) * 0.01;
                            break;
                        case 8:
                            string side = "";
                            switch (currentReport)
                            {
                                case ReportType.ROM:
                                    side = row < 10 ? "LEFT " : "RIGHT ";
                                    break;
                                case ReportType.OHS:
                                    if (row >= 23 && row < 28)
                                        side = "LEFT ";
                                    else if (row >= 28)
                                        side = "RIGHT ";
                                    break;
                                case ReportType.LNG:
                                    side = row >= 33 ? "LEFT " : "RIGHT ";
                                    break;
                            }
                            userParameter.Name = side + currentCell.Text;
                            break;
                    }
                }
                userParameters.Add(userParameter);
                //Console.ReadKey();
            }
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Redback Report - " + userProfile.Name;

            switch (cmbx_Reports.SelectedIndex)
            {
                case 1:
                    // Create an empty page
                    PdfPage rom_pdf = document.AddPage();
                    // Get an XGraphics object for drawing
                    XGraphics gfx1 = XGraphics.FromPdfPage(rom_pdf);

                    ROM_Page rom_Page = new ROM_Page(rom_pdf, userProfile, userParameters);
                    rom_Page.DrawHeader(gfx1);
                    
                    break;

                case 2:
                    // Create an empty page
                    PdfPage ohs_pdf = document.AddPage();
                    // Get an XGraphics object for drawing
                    XGraphics gfx2 = XGraphics.FromPdfPage(ohs_pdf);

                    OHS_Page squat_page = new OHS_Page(ohs_pdf, userProfile, userParameters);
                    squat_page.DrawHeader(gfx2);
                    squat_page.DrawPentagon(gfx2);
                    squat_page.DrawBarCharts(gfx2);
                    break;
            }

            string rbDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Redback Reports\";
            if (!Directory.Exists(rbDirectory))
                Directory.CreateDirectory(rbDirectory);

            // Save the document...
            string filename = rbDirectory + userProfile.Name + " OHS-Report.pdf";
            document.Save(filename);
            txt_FilePath.Text = "File location :" + filename;
            // ...and start a viewer.
            pdfProcess = Process.Start(filename);
        }


    }
}
