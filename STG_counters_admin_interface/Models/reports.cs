using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STG_counters_admin_interface.Models.PowerDB_data_classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static STG_counters_admin_interface.Models.Constants;
using static STG_counters_admin_interface.Models.StaticMethods;

namespace STG_counters_admin_interface.Models
{
	
	public interface IListSortOfProd {
		string SortofProduction { get; set; }
	}
	public interface ICycleDate {
		 DateTime CycleDateBegin { get; set; }
		DateTime CycleDateEnd { get; set; }
	}
	public interface IHaveMeasureDate
	{ 
		DateTime Measure_Date { get; set; }
	}
	public interface IHaveComposition
	{
		string Composition { get; set; }
	}
	public interface ISums_CycleOfProduction
	{
		string Place { get; set; }
		double EnergyConsumption { get; set; }
		double CapacityOfBO { get; set; }		
		double GasConsumption { get; set; }
		double SteamConsumption { get; set; }
		double AverageSpeed { get; set; }
	}

	public class ReportDates
	{
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime CurrShiftStart { get; set; }
		public DateTime CurrShiftEnd { get; set; }
		public int ShiftLength { get; set; } = 12;
		public int TesYear { get; set; }
		public ReportDates(DateTime startDate, DateTime endDate
			, int selectedPeriod=0 
			, int lastDay=0
			, int tesYear=0//для отчёта по ТЭС нужен отдельно год отчёта
			, int selectedMonth=0
			, int selectedYear=0)
		{
			
			//Проверяем выставлен ли период и прочие проверки на время
			StaticMethods.Check_dates(ref startDate, ref endDate, selectedPeriod, selectedMonth: selectedMonth, selectedYear: selectedYear, lastDay: lastDay);
			this.StartDate = startDate;
			this.EndDate = endDate;
			this.TesYear = tesYear;
			// Добиваем дату окончания если раньше не сделано
			this.EndDate = this.EndDate.Date.AddDays(1).AddSeconds(-1);			
			this.CurrShiftStart = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, first_shift_start_hour, 0, 0);
			this.CurrShiftEnd = CurrShiftStart.AddHours(ShiftLength).AddSeconds(-1);
		}
		public void RecalcShiftsDate(ReportInfo reportInfo)
		{
			var curShift = reportInfo.Records_List["Shifts_dates_list"].UnPack<string>().ElementAtOrDefault(reportInfo.CurrShiftIndex==-1?0: reportInfo.CurrShiftIndex);
			CurrShiftStart = DateTime.Parse(curShift.Split('-')[0]);
			CurrShiftEnd = DateTime.Parse(curShift.Split('-')[1]);
			CurrShiftEnd = CurrShiftEnd.AddSeconds(59);//время отчётов выводится без секунд
		}
	}
	public class ReportName
	{
		public string Eng { get; }
		public string Rus { get; }
		public string Full { get; }
		public string ViewName { get; set; }
		public ReportName(string reportName)
		{
			this.Full = reportName;
			this.Eng = reportName.Split(';')[0];
			this.Rus = reportName.Split(';')[1];
			ViewName = Constants.reportsNameProjection[this.Eng];



		}
	}
	public class ReportInfo
		
	{

		public ReportName ReportName { get; set; }
		public List<object> Records { get; set; }
		public Dictionary<string, List<object>> Records_List { get; set; } = new Dictionary<string, List<object>>();
		public string[] ListSortOfProd { get; set; }//фильтр для отчётов по продукуии
		public string SelectedSortOfProduction { get; set; } = null;//ограничение по продукции		
		public Dictionary<string, double[]> Sums { get; set; } = new Dictionary<string, double[]>();//Нужно для вывода сумму в отчётах
		public ScaffoldContext Dbcontext { get; set; }
		public Controller Curr_controller { get; set; }
		public int CurrShiftIndex { get; set; } = -1;
		public string PlaceName;
		public bool IsByDay { get; set; } //Признак что отчёт выводится в разрезе дня
		public string[] BaseStrings { get; set; } = null;
		public Stopwatch stopwatch = new Stopwatch();// для замера производительности	
		public bool Changed { get; set; } //Если изменения касаются не имени и дат формирования отчёта, то говорим об этом отдельно		
		public bool Successfull { get; set; }//Проверяем формированные ли были записи		
		public ReportDates ReportDates { get; set; }

		private bool isBDM1;
		private readonly string[] propsMustBeRounded = { "Sums", "Records", "Records_List" };
		private int placeId = 0;
		private int roundNum; //Количество знаков для округляние записей и итоговых сумм
							  //формируем записи для отчетов
		public int GetCurShiftIndex(int currShiftIndex, int shifts_listCount) => (currShiftIndex == -1 ? (shifts_listCount >= 2 ? shifts_listCount - 2 : 0) : currShiftIndex);
#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
		private List<T> Form_records<T>(
			string opt_sql=null,//иногда нужно задать свой sql
			DateTime startDate=default(DateTime), DateTime endDate = default(DateTime))
			where T:class
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
		{
			string sql_cmd = Generate_sql_string(opt_sql??ReportName.Eng, 
				//если даты не указаны, что иногда нужно, то берём из стандартного источника
				startDate == default(DateTime)?ReportDates.StartDate:startDate,
				endDate == default(DateTime)?ReportDates.EndDate:endDate,
				Dbcontext, placeId, IsByDay, tesYear: ReportDates.TesYear, sku: SelectedSortOfProduction);
			return  Dbcontext.Set<T>().FromSql(sql_cmd).ToList();// генерируем sql строку для вывода данных для отчета
		}
		public void RoundProps(int roundNumer=0)
		{//округляем поля
			
				this.roundNum = roundNumer > 0 ? roundNumer : this.roundNum;
				//работает с Dictionary<string,List<object>>, List<object>, Dictionary<string,double[]> и double[]
				foreach (var prop in propsMustBeRounded)
				{
					var curr_prop = this.GetType().GetProperty(prop);
					switch (curr_prop.PropertyType)
					{
						case var prop_type when prop_type == typeof(Dictionary<string, List<object>>)://dict.values не изменяемы, так что на основе ключей и содержимого, строим новый dict
							var dictionary = (Dictionary<string, List<object>>)curr_prop.GetValue(this);
							if (dictionary == null)
							{
								continue;
							}
							var rounded_dict = new Dictionary<string, List<object>>(dictionary.Count);
							foreach (var key in dictionary.Keys)
							{
								rounded_dict.Add(key, RoundList(dictionary[key], roundNum));
							}
							curr_prop.SetValue(this, rounded_dict);
							//dictionary
							break;
						case var prop_type when prop_type == typeof(List<object>):

							curr_prop.SetValue(this, RoundList((List<object>)curr_prop.GetValue(this), roundNum));
							break;
						case var prop_type when prop_type == typeof(double[]):
							curr_prop.SetValue(this, ((double[])curr_prop.GetValue(this))
																.Select(d => Math.Round(d, roundNum)).ToArray());
							break;
						case var prop_type when prop_type == typeof(Dictionary<string, double[]>):
							var ds = (Dictionary<string, double[]>)curr_prop.GetValue(this);
							if (ds == null)
							{
								continue;
							}
							var rounded_ds = new Dictionary<string, double[]>(ds.Count);
							foreach (var key in ds.Keys)
							{
								rounded_ds.Add(key, ds[key].Select(d => Math.Round(d, roundNum)).ToArray());
							}
							curr_prop.SetValue(this, rounded_ds);
							break;
						default:
							break;
					}
				}
				
			
		}
		public void EvalReportInfo(ReportName reportName, ScaffoldContext context, ReportDates reportDates,
			int currShiftIndex=-1,
			int export_excel = 0,			
			string selectedSortOfProduction=null,
			string[] baseStrings=null,//хранится выгрузки в base64 рисунки графиков			
			Controller curr_controller = null
			)
		{
			//В некоторых случаях, вроде экспорта в эксель, можно использовать уже вычисленные данные
			if (
			#region IsNeedCachedDateForReport
				(this.Successfull
				&&(export_excel==1
					&&!ReportListForRebuildForExcel.Contains(reportName.Eng))//Некоторые отчёты требуют не округлённые данные для экселя, так что формируем их заново
					|| shiftsReportNames.Contains(reportName.Eng))//Или для смен
				&& this.ReportName != null
				&& reportName.Eng == this.ReportName.Eng
				&& this.ReportDates.StartDate == reportDates.StartDate
				&& this.ReportDates.EndDate == reportDates.EndDate
			#endregion
				)
			{
				//Данные по сменам формируются из списка смен в периоде, но выводится только одна, так что обновляем эти данные
				if (shiftsReportNames.Contains(reportName.Eng))
				{
					this.CurrShiftIndex = currShiftIndex;
					ReportDates.RecalcShiftsDate(this);//Пересчитываем края текущей смены					
				}
				if (baseStrings != null)
				{
					this.BaseStrings = baseStrings;
				}
				this.SelectedSortOfProduction = selectedSortOfProduction;
				return;
			}
			#region init var
			this.Successfull = false;
			this.stopwatch.Restart();


			this.CurrShiftIndex = currShiftIndex;
			this.ReportDates = reportDates;
			this.SelectedSortOfProduction = selectedSortOfProduction;
			this.BaseStrings = baseStrings;
			this.ReportName = reportName;
			
			this.Dbcontext = context;			
			this.Curr_controller = curr_controller;
			
			this.IsByDay = IsByDay_report_list.Contains(reportName.Eng);
			isBDM1 = BDM1_reports_list.Contains(reportName.Eng);
			this.PlaceName = isBDM1 ? "БДМ-1" : "БДМ-2";			
			placeId = isBDM1 ? 1:2;
			
			

			this.roundNum = 3;
			this.Sums = new Dictionary<string, double[]>();			
			this.Records_List = new Dictionary<string, List<object>>();
			this.Records = new List<object>();
			#endregion
			switch (reportName.Eng)//на основе названия отчёта дополнительные операции над записями
			{
				case "ConsumptionByCycleByBDM1"://БДМ-1 по видам продукции
				case "ConsumptionByCycleByBDM2"://БДМ-2 по видам продукции
				case "ConsumptionByBDM1ByDay"://БДМ-1(Суточный)
				case "ConsumptionByBDM2ByDay"://БДМ-2(Суточный)
											  //делаем итого для нужных категорий
											  //double[] sums = StaticMethods.Generate_sums(RecordsToShow_ConsumptionByCycleByBDM);
					Records = Form_records<ConsumptionByCycleOfProduction>("ConsumptionByCycle").Select(r=>(object)r).ToList();
					var t_sum = Sums_CycleOfProduction(Records.Select(r => (ISums_CycleOfProduction)r).ToList());
					if (t_sum!=null)
					{
						Sums.Add("ConsumptionByCycleByBDM", t_sum);
					}
					
					break;
				case "TERperMonthperTonne"://Средневзвешенное ТЭР на тонну по циклам производства
										   //В данном отчете ограничение по датам используется для ограничения SKU по которым выводить годовой отчет, год передается отдельно
					Records_List = new Dictionary<string, List<object>>();
					Records_List.Add("TERperMonthperTonne",Form_records<TERperMonthperTonne>("TERperMonthperTonne").Select(r => (object)r).ToList());
					Records_List.Add("TERperCycleperTonne", Form_records<TERperCycleperTonne>("TERperCycleperTonne").Select(r => (object)r).ToList());
					Records_List.Add("TER_Plan", Form_records<TERperMonthperTonne>("TER_Plan").Select(r => (object)r).ToList());
					break;
				case "EnergyConsumptionByManufactureByHour"://Часовой расход электроэнергии по производству
					#region case "EnergyConsumptionByManufactureByHour"

					//Задача для каждого поля для указного для подсчёта, посчитать данные в разрезе списка индификаторов в Constants.places
					//Довольно интересный алгоритм, позволяющий делать группировку по динамическому Id закреплённому за местами Constants.places
					// причем за одно место может быть закреленно несколько ID
					//Получаем в результате массив double, где индекс массива это индекс места в Constants.places
					// А сам массив содержит информацию в разрезе часа, по всем местам.
					var precompute_data = Form_records<EnergyConsumptionByManufactureByHour>();
					if (precompute_data == null || precompute_data.Count == 0)
					{
						break;
					}
					//Собираем id по которым будем считать итоговую сумму по производству(колонка итого:)
					List<int> total_sums_place_ids_EnergyConsumptionByManufactureByHour = new List<int>();

					Constants.places_total.Values.ToList()
						.ForEach(r =>
						{ total_sums_place_ids_EnergyConsumptionByManufactureByHour.AddRange(r); }
							);

					////строка итого, храним Dictionary<string, double> для поддержки и других видов ТЭР, газа, пара и т.д.
					//List<Dictionary<string, double>> sums_EnergyConsumptionByManufactureByHour = new List<Dictionary<string, double>>();
					//подсчитываем данные по местам
					//var data_EnergyConsumptionByManufactureByHour; //= new Dictionary<string, List<double>>();
					var field_name_EnergyConsumptionByManufactureByHour = "EnergyConsumption";
					
					Records= precompute_data
							.Select(r => ((IHaveMeasureDate)r).Measure_Date)
							.Distinct()
							.Select(Measure_Date => {

								var values = SumField_byPlaceId(field_name_EnergyConsumptionByManufactureByHour, precompute_data.Where(r => ((IHaveMeasureDate)r).Measure_Date == Measure_Date).Select(r=>(EnergyConsumptionByManufactureByHour)r) .ToList(), roundNum, total_sums_place_ids_EnergyConsumptionByManufactureByHour);

								return new Data_EnergyConsumptionByManufactureByHour
								{
									Date = Measure_Date.ToShortDateString(),
									Time = Measure_Date.ToShortTimeString(),
									Values = values//массив из double с показаниями за текущий слайс на время и дату
								};

							}).ToList<object>();

					//Добавляем строковые итоги
					//Нужно пробежатся по срезам по часам и посчитать итоговые данные по каждому месту
					//напомню что индекс в массиве data это индекс места массиве Constants.places_total
					var sums_row_EnergyConsumptionByManufactureByHour = new double[places.Count + 1];//разменость +1 потому что ещё считаем последную колонку итого
					var param_values = typeof(Data_EnergyConsumptionByManufactureByHour).GetProperty("Values");
					foreach (var data in Records)
					{

						var curr_data = (List<double>)param_values.GetValue(data);
						for (int i = 0; i < curr_data.Count; i++)
						{
							sums_row_EnergyConsumptionByManufactureByHour[i] += curr_data[i];
						}

					}
					Sums.Add("row_EnergyConsumptionByManufactureByHour", sums_row_EnergyConsumptionByManufactureByHour);
					break;
				#endregion
				case "EnergyConsumptionByDevicesByDay"://Суточный расход электроэнергии по учётам
				case "EnergyConsumptionByDevicesByHour"://Часовой расход электроэнергии по учётам
					#region EnergyConsumptionByDevicesBy
					Stopwatch tempWatch = new Stopwatch();
					tempWatch.Start();
					//отличия только в группировках между этими отчетами, так что формируем признак
					//EnergyConsumptionByDevices
					//Собираем записи для формирования вывода
					//Т.к список устройств динамический, то под него нельзя создать класс, так что делаем группировки по ходу
					Records  = Form_records<EnergyConsumptionByDevices>("EnergyConsumptionByDevicesByDay").Select(r => (object)r).ToList();
					tempWatch.Stop();
					//Для строки Итого:
					Sums.Add("EnergyConsumptionByDevices", 
							Records
							.Select(r=>(EnergyConsumptionByDevices)r)
							.GroupBy(r => r.Device_Name,
							(g, r) => Math.Round(r.Sum(e => e.EnergyConsumption ?? 0), 3))
							.Append(0)
							.ToArray());



					//Для колонки Итого по производству:
					Sums.Add("EnergyConsumptionByDevices_col_production",
							Records
							.Select(r => (EnergyConsumptionByDevices)r)					
							.Where(r => Constants.EnergyConsumptionByDevices_total_devices.ContainsKey(r.Device_Name))//итого для строк считаем только для определенных колонок
							.GroupBy(r => r.Measure_Date, (g, rs) => rs.Sum(r => r.EnergyConsumption) ?? 0).ToArray());
					//Добавляем сумму по производству
					Sums["EnergyConsumptionByDevices"][Sums["EnergyConsumptionByDevices"].Length-1] = (Sums["EnergyConsumptionByDevices_col_production"].Sum(r => r));

										
					break;
				#endregion
				case "ConsumptionByManufactureByPeriod"://общая по производству
					#region "ConsumptionByManufactureByPeriod"
					Records = Form_records<ConsumptionByManufactureByPeriod>().Select(r=>(object)r).ToList();
					
					//Подготавливаем лист id мест по которым будем считать данные для итогов
					List<int> total_sums_place_ids = new List<int>();					
					Constants.places_total.Values.ToList()
						.ForEach(r =>
						{ total_sums_place_ids.AddRange(r); }
							);

					//Задача для каждого поля для указного для подсчёта,
					//посчитать данные в разрезе списка индификаторов в Constants.places
					var fInfo_ConsumptionByManufactureByPeriod = typeof(ConsumptionByManufactureByPeriod).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
					Records_List = new Dictionary<string, List<object>>(Constants.fields_to_sum_ConsumptionByManufactureByPeriod.Count);
					foreach (var field_name in Constants.fields_to_sum_ConsumptionByManufactureByPeriod)
					{
						//подсчитываем данные по местам
						Records_List.Add(field_name, 
							SumField_byPlaceId(
								field_name,
								Records
								 .Select(r=>(ConsumptionByManufactureByPeriod)r)
								 .ToList(),
								roundNum,
								total_sums_place_ids)
							.Select(r=>(object)r)
							.ToList());
					}
					break;
				#endregion
				case "ConsumptionByBDM1ByHour"://БДМ-1(Часовой)
				case "ConsumptionByBDM2ByHour"://БДМ-2(Часовой)
					#region ConsumptionByBDMByHour
					Records = Form_records<ConsumptionByBDMbyHour>("ConsumptionByBDMByHour").Select(r=>(object)r).ToList();
					if (Records==null||Records.Count==0)
					{
						break;
					}
					//Из-за того что для вывода по часового мы берем сетку по часам,
					//а циклы могот заканчиватся и начинатся внутри часа, то на эти цикла происходит дублирование по параметрам, 
					//так что пробегаемся по записям и ищем такие случаи, и объединяем их в одну запись 

					//берем элементы по count-2 потому что сравниваем два элемента и последний не счем сравнивать
					var temp_recs = new List<ConsumptionByBDMbyHour>();
					for (int i = 0; i < Records.Count - 1; i++)
					{
						var curr = (IHaveComposition)Records.ElementAt(i);
						var next = (IHaveComposition)Records.ElementAt(i + 1);
						var excludeProp = new Dictionary<string, bool>() {["ID"]=false,["Composition"]=false};

						if (IsEqualPropValue(firstObject: curr, secondObject: next, excludePropList: excludeProp))
						{
							curr.Composition += ", " + next.Composition;
							i++;
						}
						temp_recs.Add((ConsumptionByBDMbyHour)curr);
					}
					Records = temp_recs.Select(r=>(object)r).ToList();
					//Для строки Итого:
					Sums.Add("ConsumptionByBDMByHour",

						value:
						//чтобы получить итого по записям, выделяем нужные столбцы и добаляем столбец по кторому будем вести группировку
						GetSumsAndAvgProp(
							Records.Select(r => (ConsumptionByBDMbyHour)r).ToList(), 
							propForSumList: tesSumList.ToDictionary(r => r, r => false),
							propForAvgList: tesAvgList.ToDictionary(r => r, r => false))
						 .Select(d => (double)(d??0.0)).ToArray());//распаковываем в дабл
					break;
				#endregion
				case "SkuDataByShifts_BDM1":
				case "SkuDataByShifts_BDM2":
					#region SkuDataByShifts
					var ter_list_shifts = Constants.ter_list_shifts;
					//нарезаем указный период на смены
					var shifts_list = new List<Shift>();
					int shift_index = 0;

					reportDates.CurrShiftStart = reportDates.CurrShiftStart.AddHours(-reportDates.ShiftLength);
					reportDates.CurrShiftEnd = reportDates.CurrShiftEnd.AddHours(-reportDates.ShiftLength);
					while (true)
					{
						if (reportDates.CurrShiftStart > DateTime.Now || reportDates.CurrShiftStart > reportDates.EndDate)//поледняя смена не должна выходить за текущюю дату или конец преиода
						{
							break;
						}
						else
						{
							shifts_list.Add(new Shift() { shift_start = reportDates.CurrShiftStart, shift_end = reportDates.CurrShiftEnd, shift_index = shift_index });
						}
						reportDates.CurrShiftStart = reportDates.CurrShiftStart.AddHours(reportDates.ShiftLength);
						reportDates.CurrShiftEnd = reportDates.CurrShiftEnd.AddHours(reportDates.ShiftLength);
						shift_index++;
					}

					var shiftStart = DateTime.Now;
					var shiftEnd = DateTime.Now;
					//если смена не выбрана(-1) то выводим последнию смену в нарезке, иначе выбраную смену
					if (shifts_list.Count > 0)
					{
						shiftStart = shifts_list[GetCurShiftIndex(currShiftIndex, shifts_list.Count)].shift_start;
						shiftEnd = shifts_list[GetCurShiftIndex(currShiftIndex, shifts_list.Count)].shift_end;
					}
					else
					{
						shiftStart = reportDates.CurrShiftStart;
						shiftEnd = reportDates.CurrShiftEnd;
					}

					reportDates.CurrShiftStart= shiftStart;
					reportDates.CurrShiftEnd = shiftEnd;

					Records_List.Add("SkuDataByShifts",
						Form_records<SkuDataByShifts>("SkuDataByShifts")						
						.OrderBy(r => r.CycleDateBegin)
						.Select(r=>(object)r)
						.ToList());
					var sku_list = Records_List["SkuDataByShifts"]
									.Select(r => ((IHaveSku)r).Sku)
									.ToList();


					Records_List.Add("Shifts_plan",  
						context.StgPlanTer
						 .Where(r => r.Year == shiftStart.Year
									&& r.Month == shiftStart.Month
									&& sku_list.Contains(r.Sku)
									&& r.PlaceId == (isBDM1 ? 1 : 2))
						 .Select(r=>(object)r)
						 .ToList());



					Records_List.Add("Shifts_index_list", shifts_list.Select(r => (object)r.shift_index.ToString()).ToList());
					Records_List.Add("Shifts_dates_list", shifts_list
															.Select(r => $"{r.shift_start.ToShortDateString()} {r.shift_start.ToShortTimeString()}-{r.shift_end.ToShortDateString()} {r.shift_end.ToShortTimeString()}")
															.Select(r=>(object)r)
															.ToList());
				
					//if (export_excel == 1)
					//{ return Excel_methods.Export_to_Excel_SkuDataByShifts(new ExcelExport(), report_name, shiftStart, shiftEnd, placeName: place_name, this, baseStrings, RecordsToShow_SkuDataByShifts, ter_list_shifts, shiftId: RecordsToShow_SkuDataByShifts.Count > 0 ? RecordsToShow_SkuDataByShifts.FirstOrDefault().ShiftId : 0, machinist_name: RecordsToShow_SkuDataByShifts.Count > 0 ? RecordsToShow_SkuDataByShifts.FirstOrDefault().Machinist_name : ""); }
					break;
				#endregion
				default:
					break;
			}
			//Если нужен список продуктов
			if (Records?.Count>0? Records?.FirstOrDefault().GetType().GetInterface("IListSortOfProd") != null:false)
			{
				ListSortOfProd = Records.OrderBy(m => ((IListSortOfProd)m).SortofProduction).Select(m => ((IListSortOfProd)m).SortofProduction).Distinct().ToArray();
				if (SelectedSortOfProduction != null && SelectedSortOfProduction != "All")
					Records = Records.Where(r => ((IListSortOfProd)r).SortofProduction == SelectedSortOfProduction).ToList();
			}
			//Округляем все поля
			RoundProps();
			this.Successfull = true;			
		}
	}
	
}
