using STG_counters_admin_interface.Models.PowerDB_data_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace STG_counters_admin_interface.Models
{
	
	public static class Constants
	{
		public static List<string> list_to_chart_StgPlanTer = new List<string>() {//имена полей содержащие данные для графика
		"ShiftProductivityMin",
		"ShiftProductivityNorm",
		"EnergyPerTonneMin",
		"EnergyPerTonneNorm",
		"SteamPerTonneMin",
		"SteamPerTonneNorm",
		"GasPerTonneMin",
		"GasPerTonneNorm",
		"AvarageSpeedMin",
		"AvarageSpeedNorm"
	};
	public static List<string> ReportListForRebuildForExcel = new List<string>()
		{
			"ConsumptionByCycleByBDM1",
			"ConsumptionByCycleByBDM2"
		};
		public static Dictionary<Type, string> contextNames = new Dictionary<Type, string>() {
			[typeof(StgCacheCycleSpoolProductionByMonth)] = "byMonth",
			[typeof(StgCacheCycleSpoolProductionByDay)] = "byDay",
			[typeof(StgCacheCycleSpoolProductionByShifts)] = "byShifts"
		};
		public static Dictionary<Type, string> sqlUpdateAvgSpeedStr = new Dictionary<Type, string>()
		{
			[typeof(StgCacheCycleSpoolProductionByMonth)] = "Update_avg_speed_Cache_CycleSpoolProduction_byMonth",
			[typeof(StgCacheCycleSpoolProductionByDay)] = "Update_avg_speed_Cache_CycleSpoolProduction_byDay",
			[typeof(StgCacheCycleSpoolProductionByShifts)] = "Update_avg_speed_Cache_CycleSpoolProduction_byShifts"
		};

		

		public static int[] placeIdsForCorrection = new int[] { 1, 2 };
		
		public static List<string> list_to_Chart_SkuDataByShifts = new List<string>()//поля содержащие данные для вывода графика
		{"ShiftProductivity",
		 "EnergyConsumptionPerTonne",
		 "SteamConsumptionPerTonne",
		 "GasConsumptionPerTonne",
		 "AverageSpeed"
		};
		public static List<string> ter_list_shifts = new List<string>(){
						"Сменная производительность",
						"Удельное потребление э./э.",
						"Удельное потребление пара",
						"Удельное потребление газа",
						"Средняя скорость"      };

		public static string[] tesSumList = 
		{	"EnergyConsumption",
			"GasConsumption",
			"SteamConsumption"};

		public static string[] tesAvgList =
		{  
			"AverageSpeed"};

		public static string[] shiftsReportNames = { "SkuDataByShifts_BDM1", "SkuDataByShifts_BDM2" };
		public static Dictionary<string, string> reportsNameProjection = new Dictionary<string, string>() {
			{ "ConsumptionByCycleByBDM1","ConsumptionByCycle" },//БДМ-1 по видам продукции
			{ "ConsumptionByCycleByBDM2","ConsumptionByCycle" },//БДМ-2 по видам продукции
			{ "ConsumptionByBDM1ByDay","ConsumptionByCycle" },//БДМ-1(Суточный)
			{ "ConsumptionByBDM2ByDay","ConsumptionByCycle" },//БДМ-2(Суточный)
			{ "TERperMonthperTonne","TERperMonthperTonne" },//Средневзвешенное ТЭР на тонну по циклам производства, в данном отчете ограничение по датам используется для ограничения SKU по которым выводить годовой отчет, год передается отдельно
			{ "EnergyConsumptionByManufactureByHour","EnergyConsumptionByManufactureByHour" },//Часовой расход электроэнергии по производству
			{ "EnergyConsumptionByDevicesByDay","EnergyConsumptionByDevices"},//Суточный расход электроэнергии по учётам
			{ "EnergyConsumptionByDevicesByHour","EnergyConsumptionByDevices"},//Часовой расход электроэнергии по учётам
			{ "ConsumptionByManufactureByPeriod","ConsumptionByManufactureByPeriod"},//общая по производству
			{ "ConsumptionByBDM1ByHour", "ConsumptionByBDMByHour" },//БДМ-1(Часовой)
			{ "ConsumptionByBDM2ByHour", "ConsumptionByBDMByHour" },//БДМ-2(Часовой)
			{ "SkuDataByShifts_BDM1","SkuDataByShifts"},//По сменный отчет БДМ-1
			{ "SkuDataByShifts_BDM2", "SkuDataByShifts"}//По сменный отчет БДМ-2
		};
		public static string admin_email = "thst@mail.ru";
		public static int shift_duration = 12;
		public static int first_shift_start_hour = 8;
		public static int second_shift_start_hour = 20;
		
		//порядок в TERperMonthperTonne_graph_fields_list важнен, чтобы совпадал с порядком графиков
		public static List<string> TERperMonthperTonne_graph_fields_list = new List<string> {"ValueOfBO",
		"AverageSpeed",
		"EnegryConsumptionperTonne",
		"GasConsumptionperTonne",
		"SteamConsumptionperTonne"
		
	};
		public static List<string> ConsumptionByManufactureByPeriod_header = new List<string>() { "По потребителям", "Ед Изм",  "Период" };

		public static List<string[]> ConsumptionByManufactureByPeriod_additional_data = new List<string[]>(){ new string[]{ "Электроэнергия", "МВт/ч" },
													new string[]{"Теплоэнергия в паре (Пар)","Гкал"},
													new string[]{ "Газ", "т. м. куб." }};

		public static List<string> fields_to_sum_ConsumptionByManufactureByPeriod = new List<string>() { "EnergyConsumption",																										
																										"SteamConsumption",
																										"GasConsumption"};

		public static List<string>  Plan_TER_columns_head_list = new List<string>(){"Месяц",
		"Объем БО",
		"Сменная производительность минимальная",
		"Сменная производительность средняя",
		"Энергоемкость МВт*ч/тонна Мин.",
		"Энергоемкость МВт*ч/тонна Норм.",
		"Потребление пара ГКал/тонна Мин.",
		"Потребление пара ГКал/тонна Норм.",
		"Потребление газа тыс м3 /тонна Мин.",
		"Потребление газа тыс м3 /тонна Норм.",
		"Средняя скорость м/мин Мин.",
		"Средняя скорость м/мин Норм."};
	public static Dictionary<string,bool> EnergyConsumptionByDevices_total_devices = new Dictionary<string,bool>() {
		[ "РП_20  Ячейка 12 (БДМ-2)"]=false,
		["РП_20  Ячейка 13 (Мак.уч)"] = false,
		["РП_20  Ячейка 15 (БДМ-1)"] = false,
		["РП_20  Ячейка 17 (БДМ-2)"] = false,
		["РП_20  Ячейка 18 (БДМ-1)"] = false,
		["РП_20  Ячейка 19 (БДМ-1)"] = false,
		["РП_20  Ячейка 20 (БДМ-1 привод)"] = false,
		["РП_20  Ячейка 21 (БДМ-1)"] = false,
		["РП_20  Ячейка 22 (БДМ-1 привод)"] = false,
		["РП_20  Ячейка 28 (Мак.уч.)"] = false,
		["РП_20  Ячейка 30 (БДМ-1)"] = false,
		["РП_20 Ячейка 16 (СГИ)"] = false,
		["РП_20 Ячейка 24 (Склад, зар-е)"] = false,
		["РП_20_яч. №11"] = false,
		["РП_21_яч. №5"] = false,
		["РП_21_яч. №6"] = false


	};
		public static Dictionary<string, int[]> places = new Dictionary<string, int[]> {
						//в массиве int расположены ID мест входящий в участок
						 { "БДМ1 минус мак. уч.", new int[] { 1, -99 } }
						,{ "Мак. участок", new int[] { 99 } }
						, { "БДМ1", new int[] { 1 } }
						, { "БДМ2",new int[]{ 2 } }
						, { "ПРС-1",new int[]{3 } }
						, { "ПРС-2",new int[]{4 } }
						, { "ПРС-1,2",new int[]{3,4 }}
						, { "СГБ",new int[]{1,2,3,4 }}
						, { "С4", new int[]{ 6} }
						, { "X5",  new int[]{7 } }
						, { "CMG", new int[]{12} }
						, { "SDF", new int[]{13} }
						, { "СГИ", new int[]{13,7,6,12} }
						, { "Производство СГБ+СГИ", new int[]{ 1, 2, 3, 4,13,7,6,12} }
						, { "Вентиляция", new int[]{210} }
						, { "Компрессора", new int[]{211} }
						, { "Заводоуправление", new int[]{0} }
						, { "Столовая", new int[]{212} }
						, { "Склад", new int[]{8} }         };

		public static Dictionary<string, int[]> places_total = new Dictionary<string, int[]> {
						//в массиве int расположены ID мест входящий в участок
						  { "Производство СГБ+СГИ", new int[]{ 1, 2, 3, 4,13,7,6,12} }
						, { "Вентиляция", new int[]{210} }
						, { "Компрессора", new int[]{211} }
						, { "Заводоуправление", new int[]{0} }
						, { "Столовая", new int[]{212} }
						, { "Склад", new int[]{8} }         };

		public static string[] Excel_export_head_ConsumptionByBDMByHour = {"Начало периода"
							,"Конец периода"
							,"Объект"
							,"Вид продукции"
							,"Э/Энергия, МВт*ч"
							,"Газ, тыс.м3"
							,"Пар, ГКал"
							,"Ср. скорость"
							};
	public static string[] Excel_export_head_ConsumptionByCycleByBDM = {
							"Дата начала цикла"
							,"Дата окончания цикла"

							,"Объект"
							, "Продолжительность мин."
							, "Вид продукции"
							,"Объем БО, тонн"
							, "Э/Энергия, МВт*ч"
							,"Энергоемкость, МВт*ч/тонна"
							,"Газ, тыс.м3"
							,"Потребление газа, тыс.м3/тонна"
							,"Пар, ГКал"
							,"Потребление пара, ГКал/тонна"
							,"Ср. скорость"
							};
		public static string[] Excel_export_head_EnergyConsumptionByManufactureByHour = {
							"Дата"
							,"Период"

							,"БДМ1"
							, "БДМ2"
							, "C4"
							,"X5"
							, "SDF"
							,"Всего"
							
							};
		public static IEnumerable<object[]> List_report_fullname_for_tests()
		{

			yield return new object[] { "ConsumptionByCycleByBDM1;БДМ-1 по видам продукции" };
			yield return new object[] { "ConsumptionByCycleByBDM2;БДМ-2 по видам продукции" };
			yield return new object[] { "TERperMonthperTonne;Средневзвешенное ТЭР на тонну по циклам производства" };
			yield return new object[] { "EnergyConsumptionByManufactureByHour;Часовой расход электроэнергии по производству" };
			yield return new object[] { "ConsumptionByBDM1ByDay;БДМ-1(Суточный)" };
			yield return new object[] { "ConsumptionByBDM2ByDay;БДМ-2(Суточный)" };
			yield return new object[] { "EnergyConsumptionByDevicesByDay;Суточный расход электроэнергии по учётам" };
			yield return new object[] { "EnergyConsumptionByDevicesByHour;Часовой расход электроэнергии по учётам" };
			yield return new object[] { "ConsumptionByManufactureByPeriod;Общая по производству" };
			yield return new object[] { "ConsumptionByBDM1ByHour;БДМ-1(Часовой)" };
			yield return new object[] { "ConsumptionByBDM2ByHour;БДМ-2(Часовой)" };
			yield return new object[] { "SkuDataByShifts_BDM1;По сменный отчет БДМ-1" };
			yield return new object[] { "SkuDataByShifts_BDM2;По сменный отчет БДМ-2" };
		}
		public static List<string> BDM1_reports_list = new List<string> {
		"ConsumptionByCycleByBDM1",
		"ConsumptionByBDM1ByDay",
		"ConsumptionByBDM1ByHour",
		"SkuDataByShifts_BDM1"
		};

		public static List<string> IsByDay_report_list = new List<string> {
		"ConsumptionByBDM1ByDay",
		"ConsumptionByBDM2ByDay",
		"EnergyConsumptionByDevicesByDay"
		};
		
	}


	

}
