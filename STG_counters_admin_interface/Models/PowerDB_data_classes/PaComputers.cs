using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaComputers
    {
        public PaComputers()
        {
            PaSessions = new HashSet<PaSessions>();
        }

        public int IdComputer { get; set; }
        public string NameComputer { get; set; }
        public string DescComputer { get; set; }

        public ICollection<PaSessions> PaSessions { get; set; }
    }
}
