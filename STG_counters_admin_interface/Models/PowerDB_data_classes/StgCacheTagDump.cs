using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgCacheTagDump
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public int Period { get; set; }
    }
}
