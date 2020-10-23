using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgDataLost
    {
        public int IdRecord { get; set; }
        public int IdParameter { get; set; }
        public double ParamValue { get; set; }
        public double MeasureValue { get; set; }

        public PaAdapterParameters IdParameterNavigation { get; set; }
        public StgRecordsLost IdRecordNavigation { get; set; }
    }
}
