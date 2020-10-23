using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgBackgroundTasklogs
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Message { get; set; }
        public DateTime LogDateTime { get; set; }
        public string TimerName { get; set; }

        public StgLogTypes Type { get; set; }
    }
}
