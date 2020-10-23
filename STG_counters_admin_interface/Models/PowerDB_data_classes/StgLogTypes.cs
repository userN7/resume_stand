using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgLogTypes
    {
        public StgLogTypes()
        {
            StgBackgroundTasklogs = new HashSet<StgBackgroundTasklogs>();
        }

        public int Id { get; set; }
        public string Typename { get; set; }

        public ICollection<StgBackgroundTasklogs> StgBackgroundTasklogs { get; set; }
    }
}
