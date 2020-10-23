using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace STG_counters_admin_interface.Models.PowerDB_data_classes
{
	public partial class GammaContext : DbContext
	{
		public GammaContext()
		{
		}

		public GammaContext(DbContextOptions<GammaContext> options)
			: base(options)
		{
		}
	}
	public partial class ScaffoldContext : DbContext
	{
		public virtual DbSet<ElectricEnergyPlaces> ElectricEnergyPlaces { get; set; }
		public virtual DbSet<DevicePlacesT> DevicePlacesT { get; set; }
		public virtual DbSet<GammaPlaces> GammaPlaces { get; set; }
		public virtual DbSet<ConsumptionByCycleOfProduction> ConsumptionByCycleOfProduction { get; set; }
		public virtual DbSet<EnergyConsumptionByDevices> EnergyConsumptionByDevices { get; set; }

		public virtual DbSet<vPlaces> vPlaces { get; set; }
		public virtual DbSet<EnergyConsumptionByManufactureByHour> EnergyConsumptionByManufactureByHour { get; set; }
		public virtual DbSet<ConsumptionByManufactureByPeriod> EnergyConsumptionByManufactureByPeriod { get; set; }
		public virtual DbSet<ConsumptionByBDMbyHour> ConsumptionByBDMbyHour { get; set; }
		public virtual DbSet<PaRecordsByFilter> PaRecordsByFilter { get; set; }
		public virtual DbSet<TERperMonthperTonne> TERperMonthperTonne { get; set; }
		public virtual DbSet<TERperCycleperTonne> TERperCycleperTonne { get; set; }
		public virtual DbSet<SkuDataByShifts> SkuDataByShifts { get; set; }
		





	}
	public partial class StgCacheCycleSpoolProductionByMonth : IStgCacheCycleSpoolProduction
	{
	}
	public partial class StgCacheCycleSpoolProductionByDay : IStgCacheCycleSpoolProduction
	{
	}
	class Period
	{ 
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	}
	 class Shift
	{
		public DateTime shift_start  { get; set; }
		public DateTime shift_end { get; set; }
		public int shift_index { get; set; }
		//public Shift()
		//{
		//	shift_start = new DateTime();
		//	shift_end = new DateTime();
		//}
		
	}
	public partial class StgPlanTer: Isku_Identify
	{
	}
	public partial class StgSkuPlanTer : Isku_Identify
	{
	}
	
		public interface Isku_Identify
	{
	int PlaceId { get; set; }
	string Sku { get; set; }
	int Year { get; set; }
	int Month { get; set; }
	}
		public interface IStgCacheCycleSpoolProduction
	{
		int Id { get; set; }
		int? Cycle { get; set; }
		DateTime? CycleDateEnd { get; set; }
		DateTime? CycleDateBegin { get; set; }
		string Place { get; set; }
		double? Weight { get; set; }
		int? PlaceId { get; set; }
		string Composition { get; set; }
		int Month { get; set; }
		int Year { get; set; }
		double? AverageSpeed { get; set; }
	}

	public class TER_SKU_cells: ITER_SKU_cells
	{


		public string PlaceName { get; set; }

		public double ValueOfBO { get; set; }
		public double AverageSpeed { get; set; }
		public double EnegryConsumptionperTonne { get; set; }
		public double SteamConsumptionperTonne { get; set; }
		public double GasConsumptionperTonne { get; set; }
	}
	public interface ITER_SKU_cells
	{
		
		
		 string PlaceName { get; set; }
		
		 double ValueOfBO { get; set; }
		 double AverageSpeed { get; set; }
		 double EnegryConsumptionperTonne { get; set; }
		 double SteamConsumptionperTonne { get; set; }
		 double GasConsumptionperTonne { get; set; }
	}

	public class TERperMonthperTonne: ITER_SKU_cells
	{
		public int ID { get; set; }
		public int PlaceID { get; set; }
		public string PlaceName { get; set; }
		public string SKU { get; set; }
		public int Month { get; set; }
		//public DateTime CycleDateBegin { get; set; }
		//public DateTime CycleDateEnd { get; set; }
		public double ValueOfBO { get; set; }
		public double AverageSpeed { get; set; }
		public double EnegryConsumptionperTonne { get; set; }		
		public double SteamConsumptionperTonne { get; set; }
		public double GasConsumptionperTonne { get; set; }

	}

	public class Data_EnergyConsumptionByManufactureByHour
	{
		public string Date { get; set; }
		public string Time { get; set; }
		public List<double> Values { get; set; }
	}

	public class TERperCycleperTonne
	{
		public long ID { get; set; }
		public int PlaceID { get; set; }
		public string PlaceName { get; set; }
		public string SKU { get; set; }
		public int Month { get; set; }
		public DateTime CycleDateBegin { get; set; }
		public DateTime CycleDateEnd { get; set; }
		public double? ValueOfBO { get; set; }
		public double? AverageSpeed { get; set; }
		public double? EnegryConsumptionperTonne { get; set; }
		public double? SteamConsumptionperTonne { get; set; }
		public double? GasConsumptionperTonne { get; set; }
	}
	//Добавляем связь с таблицей Devices

	//26.04.2019 Временный класс для того чтобы содержать DevicePlaces, 
	//потому что когда публикуешь в в 32 бит, откуда то берется колонка deviceid1 entity, и падает с экспшеном, причем вначале все норально работало
	//подозреваю решение завести норальный ключ и ИД в deviceplace
	public partial class DevicePlacesT
	{
		public long ID { get; set; }
		public int DeviceId { get; set; }
		public int PlaceId { get; set; }
		public double Multiplier { get; set; }
		public PaDevices Device { get; set; }
	}

	//public partial class DevicePlaces
	//{

	//    public PaDevices Device { get; set; }
	//}
	//Класс для представления dbo.vPlaces
	public class GammaPlaces
	{
		public long ID { get; set; }
		public int PlaceID { get; set; }
		public string PlaceName { get; set; }
	}

	//Класс для отчета ElectricEnergyPlaces
	public class ElectricEnergyPlaces
	{
		public long ID { get; set; }
		public double? GasValue { get; set; }
		public double? SteamValue { get; set; }
		public double? SteamMassValue { get; set; }
		public double? Value { get; set; }
		public int PlaceID { get; set; }
		public DateTime Date { get; set; }
		public string DEVICE_NAME { get; set; }
	}

	public interface IHaveSku {
		string Sku { get; set; }
	}
	public class SkuDataByShifts:IHaveSku
	{
		public long? ID { get; set; }		
		public string Sku { get; set; }
		public int ShiftId { get; set; }
		public DateTime CycleDateBegin { get; set; }
		public DateTime CycleDateEnd { get; set; }
		public string Machinist_name { get; set; }
		public double FluidStream { get; set; }
		public double Wetness { get; set; }

		public double ShiftProductivity { get; set; }
		public double EnergyConsumptionPerTonne { get; set; }
		
		public double SteamConsumptionPerTonne { get; set; }
		public double GasConsumptionPerTonne { get; set; }
		public double AverageSpeed { get; set; }


	}
	public class ConsumptionByCycleOfProduction:  IListSortOfProd, ISums_CycleOfProduction, ICycleDate
	{
		public long? ID { get; set; }
		public DateTime CycleDateBegin { get; set; }
		public DateTime CycleDateEnd { get; set; }
		
		public string Place { get; set; }
		public string SortofProduction { get; set; }

		public double EnergyConsumption { get; set; }
		public double CapacityOfBO { get; set; }
		public double EnergyConsumptionPerTonne { get; set; }
		public double GasConsumption { get; set; }
		public double SteamConsumption { get; set; }


		public double GasConsumptionPerTonne { get; set; }
		public double SteamConsumptionPerTonne { get; set; }
		public double AverageSpeed { get; set; }


	}



	public class ConsumptionByBDMbyHour: IHaveComposition
	{
		public long? ID { get; set; }
		
		public DateTime StartPeriod { get; set; }
		public DateTime EndPeriod { get; set; }
		public string Place { get; set; }
		public string Composition { get; set; }
		public double? EnergyConsumption { get; set; }	
		public double? GasConsumption { get; set; }
		public double? SteamConsumption { get; set; }
		public double? AverageSpeed { get; set; }


	}

	public class EnergyConsumptionByDevices
	{
		public long ID { get; set; }
		public DateTime Measure_Date { get; set; }
		public string Device_Name { get; set; }
		public double? EnergyConsumption { get; set; }
	}
	
	////Из-за того что имена мест располгаются в view vPlaces,
	////это свойство добавляем отдельно
	//public partial class DevicePlaces
	//{
	//	public string PlaceName { get; set; }
	//}
	
		//класс для представления vPlaces
	public class vPlaces
	{
		public long ID { get; set; }
		public int PlaceID { get; set; }
		public string PlaceName { get; set; }
	}
	public class EnergyConsumptionByManufactureByHour_t //Количество полей здесь зависит от количество мест в запросе
	{
		public long ID { get; set; }
		public DateTime Measure_Date { get; set; }
		//public double[] EnergyConsumptions { get; set; }
		public double EnergyConsumption_byT0 { get; set; }
		public double EnergyConsumption_byT1 { get; set; }
		public double EnergyConsumption_byT2 { get; set; }
		public double EnergyConsumption_byT3 { get; set; }
		public double EnergyConsumption_byT4 { get; set; }
		public double EnergyConsumption_byT5 { get; set; }
		public double EnergyConsumption_byT6 { get; set; }
		public double EnergyConsumption_byT7 { get; set; }
		public double EnergyConsumption_byT8 { get; set; }
		public double EnergyConsumption_byT9 { get; set; }
		public double EnergyConsumption_byT10 { get; set; }
		public double EnergyConsumption_byT11 { get; set; }
		public double EnergyConsumption_byT12 { get; set; }
		public double EnergyConsumption_byT13 { get; set; }
		public double EnergyConsumption_byT14 { get; set; }
		public double EnergyConsumption_byT15 { get; set; }
		public double EnergyConsumption_byT16 { get; set; }
		public double EnergyConsumption_byT17 { get; set; }
		

		public double EnergyConsumption_Total { get; set; }
	}

	public class ConsumptionByManufactureByPeriod: IHavePlaceId
	{
		public long ID { get; set; }
		public int PlaceID { get; set; }
		public double? EnergyConsumption { get; set; }
		public double? GasConsumption { get; set; }
		public double? SteamConsumption { get; set; }
		
	}
	public interface IHavePlaceId
	{
		int PlaceID { get; set; }
	}

	public class EnergyConsumptionByManufactureByHour: IHavePlaceId,IHaveMeasureDate
	{
		public long ID { get; set; }
		public int PlaceID { get; set; }
		public double? EnergyConsumption { get; set; }		
		public DateTime Measure_Date { get; set; }

	}
	//Класс данных для вывода записей не подходящих под критерий из таблицы STG_CriteriaForDataFromDevices
	//Данные фильтруются по этому критерию либо выделяются красным
	public class PaRecordsByFilter
	{
		public long ID { get; set; }
		public string PlaceName { get; set; }
		public string DEVICE_NAME { get; set; }
		public int ID_RECORD { get; set; }
		public int DEVICE_ID { get; set; }
		public DateTime RECORD_TIME { get; set; }
		public double PARAM_VALUE { get; set; }
		public double MEASURE_VALUE { get; set; }
		public string RecordStatus { get; set; }
		//public int ID_Criteria { get; set; }
	}

	}