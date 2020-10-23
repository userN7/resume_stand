using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class PaRecords
    {
        public PaRecords()
        {
            PaData = new HashSet<PaData>();
            StgPaRecordsChangesLog = new HashSet<StgPaRecordsChangesLog>();
        }

        public int IdRecord { get; set; }
        public int IdDevice { get; set; }
        public int IdAdapter { get; set; }
        public int MethodType { get; set; }
        public DateTime RecordTime { get; set; }
        public int Status { get; set; }
        public int RecordIndex { get; set; }

        public PaAdapters Id { get; set; }
        public ICollection<PaData> PaData { get; set; }
        public ICollection<StgPaRecordsChangesLog> StgPaRecordsChangesLog { get; set; }
    }
}
