using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgConfigParams
    {
        public int Id { get; set; }
        public string ParamName { get; set; }
        public string ParamVal { get; set; }
        public string ParamDescription { get; set; }
    }
}
