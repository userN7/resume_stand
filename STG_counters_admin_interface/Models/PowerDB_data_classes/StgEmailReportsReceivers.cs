using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgEmailReportsReceivers
    {
        public int EmailReportsReceiversId { get; set; }
        public int ReportNotificationNameId { get; set; }
        public string EmailName { get; set; }

        public StgEmailReportsNames ReportNotificationName { get; set; }
    }
}
