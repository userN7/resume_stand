using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgCriteriaForDataFromElectricDevices
    {
        public int IdCriteria { get; set; }
        public int IdDevice { get; set; }
        public int MinActiveEnergy { get; set; }
        public int MaxActiveEnergy { get; set; }
        public int MinReactiveEnergy { get; set; }
        public int MaxReactiveEnergy { get; set; }

        public PaDevices IdDeviceNavigation { get; set; }
    }
}
