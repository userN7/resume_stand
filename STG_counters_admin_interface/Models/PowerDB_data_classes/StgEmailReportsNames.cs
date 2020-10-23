using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgEmailReportsNames
    {
        public StgEmailReportsNames()
        {
            StgEmailReportsReceivers = new HashSet<StgEmailReportsReceivers>();
        }

        public int Id { get; set; }
        public string ReportName { get; set; }
        public string ReportNameEng { get; set; }
        public int DelayTime { get; set; }

        public ICollection<StgEmailReportsReceivers> StgEmailReportsReceivers { get; set; }
    }
}
