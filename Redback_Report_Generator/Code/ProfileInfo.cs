using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ReportType
{
    TMS = 0,
    ROM = 1,
    OHS = 2,
    LNG = 3
}

namespace Redback_Report_Generator
{
    class ProfileInfo
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string RBID { get; set; }
        public double Score { get; set; }
        public string Color { get; set; }
        public string Opperator { get; set; }
        public string Sport { get; set; }
        public string Gender { get; set; }
        public ReportType Report { get; set; }
        public string ROMScore { get; set; }
        public string LNGScore { get; set; }
        public string OHSScire { get; set; }
        public string ReportText { get; set; }
        public string reportHeading { get; set; }

        public ProfileInfo()
        { }
    }
}
