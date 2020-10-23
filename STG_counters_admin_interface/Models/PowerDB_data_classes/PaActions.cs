using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaActions
    {
        public int IdAction { get; set; }
        public int IdJob { get; set; }
        public int? IdDevice { get; set; }
        public string ActionName { get; set; }
        public int? ActionTypeId { get; set; }
        public string IsEnabled { get; set; }
        public byte[] ActionData { get; set; }
        public int ActionOrder { get; set; }

        public PaDevices IdDeviceNavigation { get; set; }
        public PaJobs IdJobNavigation { get; set; }
    }
}
