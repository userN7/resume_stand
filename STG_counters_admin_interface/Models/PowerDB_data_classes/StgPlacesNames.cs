using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgPlacesNames
    {
        public int PlacesNamesId { get; set; }
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
    }
}
