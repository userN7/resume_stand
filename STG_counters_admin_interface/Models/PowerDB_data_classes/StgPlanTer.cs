using System;
using System.Collections.Generic;

namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
    public partial class StgPlanTer
    {
        public int IdStgPlanTer { get; set; }
        public int PlaceId { get; set; }
        public string Sku { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double BoValue { get; set; }
        public double ShiftProductivityMin { get; set; }
        public double ShiftProductivityNorm { get; set; }
        public double EnergyPerTonneMin { get; set; }
        public double EnergyPerTonneNorm { get; set; }
        public double SteamPerTonneMin { get; set; }
        public double SteamPerTonneNorm { get; set; }
        public double GasPerTonneMin { get; set; }
        public double GasPerTonneNorm { get; set; }
        public double AvarageSpeedMin { get; set; }
        public double AvarageSpeedNorm { get; set; }
    }
}
