using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaParamMeasures
    {
        public int IdParameter { get; set; }
        public int IdMeasure { get; set; }
        public string MeasureName { get; set; }
        public double Koeff { get; set; }
        public string IsMultiplier { get; set; }
        public string IsDefault { get; set; }
        public int Identifier { get; set; }

        public PaAdapterParameters IdParameterNavigation { get; set; }
    }
}
