using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgRecordsLost
    {
        public StgRecordsLost()
        {
            StgDataLost = new HashSet<StgDataLost>();
        }

        public int IdRecord { get; set; }
        public int IdDevice { get; set; }
        public int IdAdapter { get; set; }
        public DateTime RecordTime { get; set; }
        public bool Checked { get; set; }

        public PaDevices IdDeviceNavigation { get; set; }
        public ICollection<StgDataLost> StgDataLost { get; set; }
    }
}
