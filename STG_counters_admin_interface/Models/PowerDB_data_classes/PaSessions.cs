using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaSessions
    {
        public PaSessions()
        {
            PaLog = new HashSet<PaLog>();
        }

        public int IdSession { get; set; }
        public int IdComputer { get; set; }
        public int? IdUser { get; set; }
        public DateTime StartSession { get; set; }
        public DateTime? FinishSession { get; set; }
        public string ProgTitle { get; set; }
        public byte PossiblyActive { get; set; }
        public DateTime ActiveUpTo { get; set; }

        public PaComputers IdComputerNavigation { get; set; }
        public ICollection<PaLog> PaLog { get; set; }
    }
}
