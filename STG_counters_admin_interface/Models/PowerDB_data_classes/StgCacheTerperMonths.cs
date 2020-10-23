using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgCacheTerperMonths
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double ValueOfBo { get; set; }
        public double EnegryConsumptionperTonne { get; set; }
        public double SteamConsumptionperTonne { get; set; }
        public double GasConsumptionperTonne { get; set; }
        public double AverageSpeed { get; set; }
    }
}
