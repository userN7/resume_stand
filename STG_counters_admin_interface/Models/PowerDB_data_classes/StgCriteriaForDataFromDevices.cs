using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgCriteriaForDataFromDevices
    {
        public int IdCriteria { get; set; }
        public int IdDevice { get; set; }
        public int MinParam { get; set; }
        public int MaxParam { get; set; }

        public PaDevices IdDeviceNavigation { get; set; }
    }
}
