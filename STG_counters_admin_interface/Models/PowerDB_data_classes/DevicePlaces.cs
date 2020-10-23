using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class DevicePlaces
    {
        public int DevicePlaceId { get; set; }
        public int DeviceId { get; set; }
        public int PlaceId { get; set; }
        public double Multiplier { get; set; }
        public DateTime BeginDateOfLocation { get; set; }
        public DateTime EndDateOfLocation { get; set; }
        public string PlaceName { get; set; }

        public PaDevices Device { get; set; }
    }
}
