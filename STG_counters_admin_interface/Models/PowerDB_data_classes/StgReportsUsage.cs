using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgReportsUsage
    {
        public int Id { get; set; }
        public string ReportName { get; set; }
        public DateTime? AccessDate { get; set; }
        public string RemoteIp { get; set; }
    }
}
