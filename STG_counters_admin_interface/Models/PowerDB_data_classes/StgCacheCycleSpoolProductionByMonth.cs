using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgCacheCycleSpoolProductionByMonth
    {
        public int Id { get; set; }
        public int? Cycle { get; set; }
        public DateTime? CycleDateEnd { get; set; }
        public DateTime? CycleDateBegin { get; set; }
        public string Place { get; set; }
        public double? Weight { get; set; }
        public int? PlaceId { get; set; }
        public string Composition { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public double? AverageSpeed { get; set; }
    }
}
