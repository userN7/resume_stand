using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaData
    {
        public int IdRecord { get; set; }
        public int IdParameter { get; set; }
        public double ParamValue { get; set; }
        public double MeasureValue { get; set; }

        public PaRecords IdRecordNavigation { get; set; }
    }
}
