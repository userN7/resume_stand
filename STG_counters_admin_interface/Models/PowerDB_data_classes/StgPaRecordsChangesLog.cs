using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgPaRecordsChangesLog
    {
        public int IdPaRecordsChangesLog { get; set; }
        public int IdRecord { get; set; }
        public int IdChangesStatus { get; set; }
        public double OldMeasureValue { get; set; }
        public double NewMeasureValue { get; set; }
        public double OldParamValue { get; set; }
        public double NewParamValue { get; set; }
        public DateTime? DateChange { get; set; }

        public PaRecords IdRecordNavigation { get; set; }
    }
}
