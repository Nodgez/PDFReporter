using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redback_Report_Generator
{
    class Parameter
    {
        public string Name { get; set; }
        public string UnitOfMeasure { get; set; }
        public double Value { get; set; }
        public double Percentage { get; set; }
        public string Color { get; set; }
        public double LSI { get; set; }
        public double RedVal { get; set; }
        public double AmberVal { get; set; }
        public double GreenVal { get; set; }


        public Parameter()
        { }

        public override string ToString()
        {
            return Name;
        }
    }
}
