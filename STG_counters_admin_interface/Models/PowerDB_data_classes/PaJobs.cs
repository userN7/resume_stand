using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaJobs
    {
        public PaJobs()
        {
            PaActions = new HashSet<PaActions>();
        }

        public int IdJob { get; set; }
        public string JobName { get; set; }
        public string IsEnabled { get; set; }
        public int StartMode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime OffsetTime { get; set; }
        public DateTime MaxExecTime { get; set; }
        public byte[] JobOption { get; set; }

        public ICollection<PaActions> PaActions { get; set; }
    }
}
