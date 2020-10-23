using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgDevsApproprMeasuresRange
    {
        public int IdDevApproprM { get; set; }
        public int IdDevice { get; set; }
        public int IdParameter { get; set; }
        public int MinMeasure { get; set; }
        public int MaxMeasure { get; set; }

        public PaDevices IdDeviceNavigation { get; set; }
        public PaAdapterParameters IdParameterNavigation { get; set; }
    }
}
