using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaLog
    {
        public int IdLog { get; set; }
        public int? IdSession { get; set; }
        public DateTime DateTime { get; set; }
        public int? IdDevice { get; set; }
        public int? IdUser { get; set; }
        public int? Status { get; set; }
        public string Message { get; set; }

        public PaDevices IdDeviceNavigation { get; set; }
        public PaSessions IdSessionNavigation { get; set; }
    }
}
