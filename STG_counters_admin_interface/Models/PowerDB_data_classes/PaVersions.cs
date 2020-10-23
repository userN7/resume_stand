using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaVersions
    {
        public int IdVersion { get; set; }
        public double? Ver { get; set; }
        public DateTime? VerDate { get; set; }
        public string Changes { get; set; }
    }
}
