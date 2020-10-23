using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaAdapterParameters
    {
        public PaAdapterParameters()
        {
            PaParamMeasures = new HashSet<PaParamMeasures>();
        }

        public int IdParameter { get; set; }
        public int IdDevice { get; set; }
        public int IdAdapter { get; set; }
        public string ParameterName { get; set; }
        public int ValueType { get; set; }
        public int ParamType { get; set; }
        public int LogicalId { get; set; }
        public int ParameterOrder { get; set; }
        public double MulKoeff { get; set; }
        public int StatusParam { get; set; }
        public int IdMeasure { get; set; }
        public double OverloadValue { get; set; }

        public PaAdapters Id { get; set; }
        public ICollection<PaParamMeasures> PaParamMeasures { get; set; }
    }
}
