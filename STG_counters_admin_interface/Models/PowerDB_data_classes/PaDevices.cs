using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaDevices
    {
        public PaDevices()
        {
            DevicePlaces = new HashSet<DevicePlaces>();
            PaActions = new HashSet<PaActions>();
            PaAdapters = new HashSet<PaAdapters>();
            PaLog = new HashSet<PaLog>();
            StgCriteriaForDataFromDevices = new HashSet<StgCriteriaForDataFromDevices>();
        }

        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int? DeviceType { get; set; }
        public string DeviceTypeName { get; set; }
        public string DeviceClsid { get; set; }
        public string DeviceDescription { get; set; }
        public byte[] DeviceData { get; set; }

        public ICollection<DevicePlaces> DevicePlaces { get; set; }
        public ICollection<PaActions> PaActions { get; set; }
        public ICollection<PaAdapters> PaAdapters { get; set; }
        public ICollection<PaLog> PaLog { get; set; }
        public ICollection<StgCriteriaForDataFromDevices> StgCriteriaForDataFromDevices { get; set; }
    }
}
