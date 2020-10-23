using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaUsers
    {
        public int IdUser { get; set; }
        public string UserLogin { get; set; }
        public string UserPassword { get; set; }
        public int UserRole { get; set; }
        public string UserName { get; set; }
        public string UserDescription { get; set; }
    }
}
