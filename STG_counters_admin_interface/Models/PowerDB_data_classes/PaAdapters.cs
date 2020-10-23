using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaAdapters
    {
        public PaAdapters()
        {
            PaAdapterParameters = new HashSet<PaAdapterParameters>();
            PaRecords = new HashSet<PaRecords>();
        }

        public int IdDevice { get; set; }
        public int IdAdapter { get; set; }
        public string AdapterName { get; set; }
        public string AdapterDescription { get; set; }
        public int AdapterOrder { get; set; }
        public int AdapterLogicalId { get; set; }
        public int AdapterTypeId { get; set; }
        public string AdapterTypeName { get; set; }

        public PaDevices IdDeviceNavigation { get; set; }
        public ICollection<PaAdapterParameters> PaAdapterParameters { get; set; }
        public ICollection<PaRecords> PaRecords { get; set; }
    }
}
