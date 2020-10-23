using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STG_counters_admin_interface.Models.PowerDB_data_classes;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Drawing;
using Microsoft.AspNetCore.Mvc;
using STG_counters_admin_interface.Controllers;
using System.Threading;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using static STG_counters_admin_interface.Models.Constants;
using System.Collections;
using Microsoft.Extensions.Hosting;
using System.Security;
using System.Diagnostics;
using STG_counters_admin_interface.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;


namespace STG_counters_admin_interface.Models
{


	public static class StaticMethods
	{

		//public static void CheckResults( Dictionary<string,int> results)
		//{
		//	foreach (var result in results)//проверяем были ли пустые загрузки
		//	{
		//		if (result.Value == 0)//если ничего не выгрузилось, то уведовляем админа
		//		{
		//			SendMail(admin_email, "warning", $"Выгрузка {result.Key} выгрузила ноль записей!");
		//		}
		//	}
		//}
		public static object ChangeOutput(object prop, string propName)
		{

			switch (propName)
			{
				case "StartPeriod":
				case "EndPeriod":
					return ((DateTime)prop).ToString("dd.MM.yyyy HH:mm:ss");
				case "ID":
					return null;
				default:					
					return null;
			}


		}
		public static object[] GetSumsAndAvgProp<T>(List<T> records, Dictionary<string, bool> propForSumList, Dictionary<string, bool> propForAvgList)
		 
		{
			var result = new object[propForSumList.Count+propForAvgList.Count];
			int curr_sum_ind = 0;
			var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			//Берем указаные свойства и считаем по ним сумму

			foreach (var prop in props.Where(p=>propForSumList.ContainsKey(p.Name)||propForAvgList.ContainsKey(p.Name)))
			{
				switch (prop.PropertyType)
				{
					case var _ when prop.PropertyType == typeof(double?):
					case var _ when prop.PropertyType == typeof(double):
						var temp = records.Select(r => (double)(prop.GetValue(r) ?? 0.0));
						if (propForAvgList.ContainsKey(prop.Name))
						{
							result[curr_sum_ind] = temp.Average();
							curr_sum_ind++;
						}
						if (propForSumList.ContainsKey(prop.Name))
						{
							result[curr_sum_ind] = temp.Sum(d => d);
							curr_sum_ind++;
						}
						
						
						break;
					default:
						throw new Exception($"Type {prop.PropertyType} is not supported!");
						
				}
				
				
			}

			return result;
		}

		public static bool IsEqualPropValue<T>(T firstObject,T secondObject, Dictionary<string,bool> excludePropList)
		{
			var props = firstObject.GetType().GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance| BindingFlags.NonPublic);
			
			
			foreach (var prop in props)
			{
				
					if (!excludePropList.ContainsKey(prop.Name))
					{
						//Сравниваем значения
						if (prop.GetValue(firstObject) == prop.GetValue(secondObject))
						{ 						
							continue; }//значения совпадают, продолжаем
						else
						{
						return false;
						}
					}
				
			}
			return true;

		}
		public static List<double> SumField_byPlaceId<T>(string field_name, List<T> Records, int roundNum, List<int> total_sums_place_ids)
		where T : IHavePlaceId
		{
			var data = new List<double>();
			var fInfo_ConsumptionByManufactureByPeriod = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			foreach (var field in fInfo_ConsumptionByManufactureByPeriod)
			{
				if (StaticMethods.GetFieldName(field) == field_name)
				{
					foreach (var place in Constants.places)
					{
						data.Add(Math.Round(Records
									.Where(r => place.Value.Contains(r.PlaceID)).ToList()
									.Sum(r => (double?)field.GetValue(r) ?? 0), roundNum)//положительные индификаторы плюсуются
							- Math.Round(Records
									.Where(r => place.Value.Contains(-r.PlaceID) && r.PlaceID != 0).ToList()
									.Sum(r => (double?)field.GetValue(r) ?? 0), roundNum)//отрицательные индификаторы минусуются 0 и -0 оно и тоже, так что его нельзя вычитать
							);

					}

					//Для строки Итого:
					data.Add(
						Math.Round(Records
						.Where(r => total_sums_place_ids.Contains(r.PlaceID))
						.Sum(r => (double?)field.GetValue(r) ?? 0), roundNum));


				}



			}
			return data;
		}

		public static List<string> GetFieldsName(FieldInfo[] myInfo)
		{
			var result = new List<string>();
			var pattern = @"<\S*>";
			foreach (var item in myInfo)
			{
				result.Add(Regex.Match(item.Name, pattern).Value.Trim(new char[] { '<', '>' }));
			}
			return result;

		}
		public static string GetFieldName(FieldInfo myInfo) => Regex.Match(myInfo.Name, @"<\S*>").Value.Trim(new char[] { '<', '>' });

		public static void Copy_StgPlanTer(ScaffoldContext context, List<StgPlanTer> records, List<StgPlanTer> records_ToCopy, int year_CopyTo, string sku_CopyTo, int PlaceId_CopyTo)
		{

			//Берём структуру класса
			var myInfo_StgPlanTer = typeof(StgPlanTer).GetTypeInfo().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

			records
			.ForEach(r => {
				var item = new StgPlanTer();
				for (int i = 1; i < myInfo_StgPlanTer.Count(); i++) //копируем всё кроме ID, и полей для переписования
				{
					string field_to_copy = myInfo_StgPlanTer[i].Name;

					if (field_to_copy.Contains("Year")) { myInfo_StgPlanTer[i].SetValue(item, year_CopyTo); continue; }
					if (field_to_copy.Contains("Sku")) { myInfo_StgPlanTer[i].SetValue(item, sku_CopyTo); continue; }
					if (field_to_copy.Contains("PlaceId")) { myInfo_StgPlanTer[i].SetValue(item, PlaceId_CopyTo); continue; }

					myInfo_StgPlanTer[i].SetValue(item, myInfo_StgPlanTer[i].GetValue(r));
				}
				records_ToCopy.Add(item); //{ AvarageSpeedNorm = r.AvarageSpeedNorm,Year=year_CopyTo,Sku=sku_CopyTo,PlaceId=PlaceId_CopyTo,EnegryConsumptionperTonne=r.EnegryConsumptionperTonne,GasConsumptionperTonne=r.GasConsumptionperTonne,Month=r.Month,PlaceName=(PlaceId_CopyTo==1?"БДМ-1": "БДМ-2"), SteamConsumptionperTonne=r.SteamConsumptionperTonne,ValueOfBo=r.ValueOfBo} ); 
			});


		}
		public static void Copy_StgPlanTer_full_Year(ScaffoldContext context, List<StgPlanTer> records, List<StgPlanTer> records_ToCopy, int year_CopyTo, FieldInfo[] myInfo_StgPlanTer)
		{




			records
			.ForEach(r => {
				var item = new StgPlanTer();
				for (int i = 1; i < myInfo_StgPlanTer.Count(); i++) //копируем всё кроме ID, и полей для переписования
				{
					string field_to_copy = myInfo_StgPlanTer[i].Name;

					if (field_to_copy.Contains("Year")) { myInfo_StgPlanTer[i].SetValue(item, year_CopyTo); continue; }

					myInfo_StgPlanTer[i].SetValue(item, myInfo_StgPlanTer[i].GetValue(r));
				}
				records_ToCopy.Add(item); //{ AvarageSpeedNorm = r.AvarageSpeedNorm,Year=year_CopyTo,Sku=sku_CopyTo,PlaceId=PlaceId_CopyTo,EnegryConsumptionperTonne=r.EnegryConsumptionperTonne,GasConsumptionperTonne=r.GasConsumptionperTonne,Month=r.Month,PlaceName=(PlaceId_CopyTo==1?"БДМ-1": "БДМ-2"), SteamConsumptionperTonne=r.SteamConsumptionperTonne,ValueOfBo=r.ValueOfBo} ); 
			});


		}

		public static List<string> DeviceTypesListwithHourlyIndication = new List<string> { "Elster A1800 (Электросчетчик)", "СПГ761 (газовый корректор)", "СПТ961.1(.2) (тепловычислитель)" };
		public static List<string> DeviceTypesListwithHalfHourlyIndication = new List<string> { "Меркурий 230ART (Электросчетчик)", "СЭТ-4TM.03 (Электросчетчик)" };
		public static string SqlInList(List<string> list)
		{
			return $"({String.Join(",", list.Select(r => $"'{r}'"))})";
		}
		public static string WriteSelect(List<string> list, string nameSel, string selected)
		{
			string outputString = "";
			outputString += $@"<select class=""custom-select w-auto"" name=""{ nameSel}"" onchange=""fireSubmit(event)"">""";
			foreach (var item in list)
			{
				outputString += $@"<option value=""{item.ToString()}""" + (selected == item ? "selected" : "") + @">";
				outputString += item.ToString();
				outputString += "</option>";

			}
			outputString += "</select>";
			return outputString;


		}
		public static string WriteSelect(List<string> list_options, List<string> list_values, string nameSel, string idSel, string selected, string funcName = null)
		{
			string outputString = "";
			outputString += $@"<select class=""custom-select w-auto"" id =""{idSel}"" name=""{ nameSel}"" onchange=""{funcName}(event)"" >""";
			for (int i = 0; i < list_options.Count; i++)
			{
				outputString += $@"<option value=""{list_options[i]}""" + (selected == list_options[i] ? "selected" : "") + @">";
				outputString += list_values[i].ToString();
				outputString += "</option>";

			}
			outputString += "</select>";
			return outputString;


		}
		public static DateTime GetStartofMonth(DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}
		public static DateTime GetEndOfMonth(DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddSeconds(-1);
		}
		public static void ExecSql(ScaffoldContext context, string sqlString, DateTime start_Date, DateTime end_Date)
		{
			context.Database.ExecuteSqlCommand(StaticMethods.Generate_sql_string(sqlString
																  , start_Date
																  , end_Date
																  ));
		}
		public static async Task TER_avg_permonth(ScaffoldContext context, DateTime start_Date, DateTime end_Date, int tesYear = 0)
		{

			await context.Database.ExecuteSqlCommandAsync(StaticMethods.Generate_sql_string("TER_avg_permonth"
								, start_Date
								, end_Date
								, context: context
								, tesYear: tesYear));





		}
		public static bool FirstPeriodContainsPartOfSecondPeriod(DateTime firstPeriodStart, DateTime firstPeriodEnd, DateTime? secondPeriodStart, DateTime? secondPeriodEnd)
		{
			return
						  //содержит часть начала второго периода
						  firstPeriodStart <= secondPeriodStart && secondPeriodStart <= firstPeriodEnd
						  //содержит часть конца второго периода
						  || firstPeriodStart <= secondPeriodEnd && secondPeriodEnd <= firstPeriodEnd
						  //содержит первый период содержится во втором
						  || secondPeriodStart <= firstPeriodStart && firstPeriodEnd <= secondPeriodEnd;
		}
		public static string sumEnergyForPlaces(Dictionary<string, int[]> Places, IEnumerable<ConsumptionByManufactureByPeriod> records)
		{
			string ret_string = "";
			//Отдельно рисуем "БДМ-1 минус Мак. участок"
			ret_string += $@"<td>{records.Where(r => r.PlaceID == 1 || r.PlaceID == 99).Sum(r => r.EnergyConsumption)}</td>";
			foreach (var place in Places)
			{
				var EnergyConsumption = records.Where(r => place.Value.Contains(r.PlaceID)).Sum(r => r.EnergyConsumption);

				ret_string += $@"<td>{EnergyConsumption}</td>";

			}
			return ret_string;
		}
		public static string sumGasForPlaces(Dictionary<string, int[]> Places, IEnumerable<ConsumptionByManufactureByPeriod> records)
		{
			string ret_string = "";
			//Отдельно рисуем "БДМ-1 минус Мак. участок"
			ret_string += $@"<td>{records.Where(r => r.PlaceID == 1 || r.PlaceID == 99).Sum(r => r.GasConsumption)}</td>";

			foreach (var place in Places)
			{
				var GasConsumption = records.Where(r => place.Value.Contains(r.PlaceID)).Sum(r => r.GasConsumption);

				ret_string += $@"<td>{GasConsumption}</td>";

			}
			return ret_string;
		}

		public static string sumSteamForPlaces(Dictionary<string, int[]> Places, IEnumerable<ConsumptionByManufactureByPeriod> records)
		{
			string ret_string = "";
			//Отдельно рисуем "БДМ-1 минус Мак. участок"
			ret_string += $@"<td>{records.Where(r => r.PlaceID == 1 || r.PlaceID == 99).Sum(r => r.SteamConsumption)}</td>";

			foreach (var place in Places)
			{
				var SteamConsumption = records.Where(r => place.Value.Contains(r.PlaceID)).Sum(r => r.SteamConsumption);

				ret_string += $@"<td>{SteamConsumption}</td>";

			}
			return ret_string;
		}


		public static string PerfTime(DateTime startTime) => (DateTime.Now - startTime).TotalSeconds.ToString() + " s.";
		public static Image Base64StringToImage(string base64String)
		{
			var bitmapData = Convert.FromBase64String(FixBase64ForImage(base64String));
			var streamBitmap = new System.IO.MemoryStream(bitmapData);
			return Image.FromStream(streamBitmap);

		}

		private static string FixBase64ForImage(string base64String)
		{
			base64String = base64String.Replace(" ", "+");
			int mod4 = base64String.Length % 4;
			if (mod4 > 0)
			{
				base64String += new string('=', 4 - mod4);
			}
			var sbText = new System.Text.StringBuilder(base64String, base64String.Length);
			sbText.Replace("\r\n", String.Empty);
			sbText.Replace(" ", String.Empty);
			return sbText.ToString();
		}
		public static string gen_sku_cells(ITER_SKU_cells curr_record, int count_col_per_sku) {
			if (curr_record != null)
			{

				return $@"<td>"
					+ curr_record.ValueOfBO.ToString()
					+ "</td><td>"
						+ curr_record.AverageSpeed.ToString()
					+ "</td><td>"
					+ curr_record.EnegryConsumptionperTonne.ToString()
					+ "</td><td>"
					+ curr_record.GasConsumptionperTonne.ToString()
					+ "</td><td>"
					+ curr_record.SteamConsumptionperTonne.ToString()
					+ "</td><td > </td>".Replace(" ", "")
					.Replace("\t", "")
					.Replace("\r", "")
					.Replace("\n", "");
			}
			else
			{
				string temp_string = "";
				for (int i = 0; i < count_col_per_sku; i++)
				{
					temp_string += $@"<td>0</td>";
				}
				temp_string += $@"<td> </td>";
				return temp_string;
			}
		}

		public static void TERperMonthperTonne_min_max(List<TERperMonthperTonne> records
			, out List<double> mins, out List<double> maxs
			)
		{
			mins = new List<double>();
			maxs = new List<double>();
			//Берем поля для которых отбражаем графики
			var fInfo = typeof(TERperMonthperTonne)
				.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
				.Where(f => TERperMonthperTonne_graph_fields_list.Contains(GetFieldName(f)));

			if (records != null && records.Count > 0)
			{

				//Перечисляем поля для вывода, и считаем их минимумы, порядок в TERperMonthperTonne_graph_fields_list важнен, чтобы совпадал с порядком графиков
				foreach (var field_name in TERperMonthperTonne_graph_fields_list)
				{
					var field = fInfo.Where(r => GetFieldName(r) == field_name).FirstOrDefault();
					mins.Add(records.Select(r => (double)field.GetValue(r)).Min());
					maxs.Add(records.Select(r => (double)field.GetValue(r)).Max());
				}

			}
		}

		public static string append_data_string(string data_string, TERperMonthperTonne curr_record
			, int i, int count_col_per_sku, CultureInfo culture
			)
		{

			if (curr_record != null)
			{


				switch (i)
				{
					case 1:
						data_string += curr_record.ValueOfBO.ToString(culture) + ",";

						break;
					case 2:
						data_string += curr_record.AverageSpeed.ToString(culture) + ",";

						break;
					case 3:
						data_string += curr_record.EnegryConsumptionperTonne.ToString(culture) + ",";

						break;
					case 4:
						data_string += curr_record.GasConsumptionperTonne.ToString(culture) + ",";

						break;
					case 5:
						data_string += curr_record.SteamConsumptionperTonne.ToString(culture) + ",";

						break;
					default:
						break;
				}

			}
			else
			{

				data_string = data_string + "0,";


			}
			return data_string;
		}
		public static double[] Generate_sums(List<ConsumptionByCycleOfProduction> RecordsToShow_ConsumptionByCycleByBDM)
		{
			double[] sums = new double[7];
			RecordsToShow_ConsumptionByCycleByBDM.ToList().ForEach(r =>
			{
				int i = -1;
				sums[++i] += r.CapacityOfBO;
				sums[++i] += r.EnergyConsumption;
				sums[++i] += r.EnergyConsumptionPerTonne;
				sums[++i] += r.GasConsumption;
				sums[++i] += r.GasConsumptionPerTonne;
				sums[++i] += r.SteamConsumption;
				sums[++i] += r.SteamConsumptionPerTonne;

			});
			return sums;
		}
		public static double[] Sums_CycleOfProduction(List<ISums_CycleOfProduction> records)
			
		{
			return records.GroupBy(r => r.Place, (g, r) => new {

				CapacityOfBO = r.Sum(e => e.CapacityOfBO),
				EnergyConsumption = r.Sum(e => e.EnergyConsumption),
				GasConsumption = r.Sum(e => e.GasConsumption),
				SteamConsumption = r.Sum(e => e.SteamConsumption),
				AverageSpeed = r.Average(e => e.AverageSpeed)
			})
								  .Select(r => new double[] { r.CapacityOfBO, r.EnergyConsumption, r.EnergyConsumption / r.CapacityOfBO, r.GasConsumption, r.GasConsumption / r.CapacityOfBO, r.SteamConsumption, r.SteamConsumption / r.CapacityOfBO, r.AverageSpeed }).FirstOrDefault()
								  ;
		}
		//public static void CycleSpoolProduction_Correction_Full(ScaffoldContext context, DateTime start_Date, DateTime end_Date, bool isByMonth)
		//{

		//	if (isByMonth)
		//	{
		//		 CycleSpoolProduction_Correction_Cycle<StgCacheCycleSpoolProductionByMonth>(context, start_Date, end_Date, placeId: 1);
		//		 CycleSpoolProduction_Correction_Cycle<StgCacheCycleSpoolProductionByMonth>(context, start_Date, end_Date, placeId: 2);
		//	}
		//	else
		//	{
		//		CycleSpoolProduction_Correction_Cycle<StgCacheCycleSpoolProductionByDay>(context, start_Date, end_Date, placeId: 1);
		//		CycleSpoolProduction_Correction_Cycle<StgCacheCycleSpoolProductionByDay>(context, start_Date, end_Date, placeId: 2);
		//	}


		//}

		public static (DateTime begin, DateTime end) EvalPeriodBreak<T>(DateTime currPeriodEnd, DateTime nextPeriodBegin)
		{

			switch (typeof(T))
			{
				case var type when type == typeof(StgCacheCycleSpoolProductionByDay):
					//разрыв между периодами не больше t, так что это обычно меньше дня
										
					return ( 
						new DateTime(
							currPeriodEnd.Year,
							currPeriodEnd.Month,
							currPeriodEnd.Day
							).AddDays(1).AddSeconds(-1),//Конец дня
						new DateTime(// начало дня
						nextPeriodBegin.Year,
						nextPeriodBegin.Month,
						nextPeriodBegin.Day
						));
				case var type when type == typeof(StgCacheCycleSpoolProductionByMonth):
					return
						(StaticMethods.GetEndOfMonth(currPeriodEnd),//конец месяца
						new DateTime(nextPeriodBegin.Year,//начало месяца
						nextPeriodBegin.Month,
						1)
						);

				default:
					return (default,default);
			}
			
		}

		public static Func<DateTime, DateTime> NextDate<T>(DateTime periodStart, DateTime periodEnd)
		{

			 
			switch (typeof(T))
			{
				case var type when type == typeof(StgCacheCycleSpoolProductionByDay):					
					return (DateTime dt) => new DateTime(dt.Year,dt.Month,dt.Day).AddDays(1).AddSeconds(-1);
					
				case var type when type == typeof(StgCacheCycleSpoolProductionByMonth):
					return (DateTime dt) => new DateTime(dt.Year, dt.Month, 1).AddMonths(1).AddSeconds(-1);
					
				default:
					return null;
			}
			
		}

		//public static async Task<int> CycleSpoolProduction_Correction_Cycle<T>( ScaffoldContext dbContext
		//	//, List<T> records
		//	, DateTime start_Date
		//	, DateTime end_Date
		//	, int placeId
			
		//	)
		//	where T : class, IStgCacheCycleSpoolProduction, new()
		//{
		//	var tl = new List<Task<int>>();
			
		//	//Берём параметр из базы, который отвечает за время между циклами.
		//	//Если время больше пораметра то добавляется новый цикл с наименованием прочее, иначе циклы сшиваются
		//	int t = int.Parse(dbContext.StgConfigParams
		//								.Where(r => r.ParamName == "Delta_cycles")
		//								.FirstOrDefault()
		//								.ParamVal);
		//	var recsToAdd = new List<T>();
		
			
			
		//	List<T> records = dbContext.Set<T>()
		//			.Where(
		//		r => r.PlaceId == placeId
		//		&& r.Composition != "Прочее"
		//		//Берем записи которые пересекаются периодом для работы
		//		&& StaticMethods.FirstPeriodContainsPartOfSecondPeriod(start_Date, end_Date, r.CycleDateBegin, r.CycleDateEnd)
		//		).OrderBy(r => r.CycleDateBegin)
		//		.ToList();
		//		if (records.Count==0)
		//		{
		//			return default;
		//		}
		//		#region Замыкание с двух сторон записями
		//		//На нужно по одной зажимающей записи пред и после нашей выборки, так что пробуем её получить
		//		// поиск этой записи будем проводить в пределах среза периода, 
		//		// для StgCacheCycleSpoolProductionByDay это день
		//		// для StgCacheCycleSpoolProductionByMonth это месяц
		//		var lastRecDate = records.LastOrDefault().CycleDateEnd;
		//		//смотрим есть ли слева и справа записи в пределах разреза(день, месяц), если есть то добавляем по одному скаждого разреза 
		//		var right_rec = dbContext.Set<T>()
		//				.Where(
		//			r => r.PlaceId == placeId
		//			&& r.Composition != "Прочее"
		//			//Берем записи которые пересекаются периодом для работы
		//			&& StaticMethods.FirstPeriodContainsPartOfSecondPeriod(lastRecDate.Value.AddSeconds(1),
		//																	(typeof(T)==typeof(StgCacheCycleSpoolProductionByDay))?lastRecDate.Value.AddSeconds(1).AddDays(1): lastRecDate.Value.AddSeconds(1).AddMonths(1),
		//																	r.CycleDateBegin,
		//																	r.CycleDateEnd)
		//			).OrderBy(r => r.CycleDateBegin)
		//			.FirstOrDefault();
		//		var firstRecDate = records.FirstOrDefault().CycleDateBegin;
		//		var left_rec = dbContext.Set<T>()
		//				.Where(
		//			r => r.PlaceId == placeId
		//			&& r.Composition != "Прочее"
		//			//Берем записи которые пересекаются периодом для работы
		//			&& StaticMethods.FirstPeriodContainsPartOfSecondPeriod(firstRecDate.Value.AddSeconds(-1),
		//																	(typeof(T) == typeof(StgCacheCycleSpoolProductionByDay)) ? firstRecDate.Value.AddSeconds(-1).AddDays(-1) : firstRecDate.Value.AddSeconds(-1).AddMonths(-1),
		//																	r.CycleDateBegin,
		//																	r.CycleDateEnd)
		//			).OrderBy(r => r.CycleDateBegin)
		//			.LastOrDefault();
		//		if (left_rec != default)
		//		{ 
		//			records.Insert(0, left_rec); 
		//		}
		//		if (right_rec!= default)
		//		{
		//			records.Add(right_rec);
		//		}
		//		//---------------------------------------------------
		//		#endregion
		//		//удалим все добавленные ранее "Прочее" попадающие в период чтобы добавить их заново, если они изменились
		//		dbContext.RemoveRange(
		//	dbContext.Set<T>().Where(r =>
		//	StaticMethods.FirstPeriodContainsPartOfSecondPeriod(records.First().CycleDateBegin??default, records.Last().CycleDateEnd??default, r.CycleDateBegin, r.CycleDateEnd)
		//	&& r.PlaceId == placeId
		//	&& r.Composition == "Прочее"));
			

				
		//		for (int i = 0; i < records.Count-1; i++)
		//	{
					
		//		//Если нашли разрыв в циклах
		//		if (records.ElementAt(i).CycleDateEnd != records.ElementAt(i + 1).CycleDateBegin)
		//		{

		//			if ((records.ElementAt(i + 1).CycleDateBegin - records.ElementAt(i).CycleDateEnd).Value.TotalHours > t)
		//			{
		//					//прочее добавляется только при замыкании с обоих сторон циклами 
		//					var (isNeedBreak, nextDate) = NextDate<T>(records.ElementAt(i).CycleDateEnd??default, records.ElementAt(i + 1).CycleDateBegin??default);
		//					var finish = records.ElementAt(i + 1).CycleDateBegin ?? default;
		//					var begin = records.ElementAt(i).CycleDateEnd??default;
		//					var end = nextDate(begin);
		//					end = end< finish ? end: finish;
		//					//нарезаем "прочее в пределах ограничений на период"
		//					while (isNeedBreak)
		//					{


		//							recsToAdd.Add(new T()// день особо не нужен, так что можно пока не добавлять, 
		//							{

		//								Cycle = -1,
		//								CycleDateBegin = begin,
		//								CycleDateEnd = end,
		//								Weight = 0,
		//								Place = records.ElementAt(i).Place,
		//								PlaceId = records.ElementAt(i).PlaceId,
		//								Composition = "Прочее",
		//								Month = records.ElementAt(i).Month,
		//								Year = records.ElementAt(i).Year

		//							});
								
								
		//						begin = end.AddSeconds(1);
		//						end = nextDate(begin);
		//						end = end < finish ? end : finish;
		//						if (begin > finish)
		//						{
		//							isNeedBreak = false;
		//						}

		//					}



		//			}
		//			else {
		//					//получаем разрыв в зависимости от типа записей
		//					var periodChange = EvalPeriodBreak<T>(records.ElementAt(i).CycleDateEnd ?? default, records.ElementAt(i + 1).CycleDateBegin ?? default);
		//						  //выравниваем периоды
		//					//т.к. запись выравниваем по левому краю, то сдвиг происходит либо относительно левого края второй записи сдвигается к ближайшему из двух(разрыв и начало предыдущей записи), а у первой записи только выравнивание конца относительно разрыва
		//					records.ElementAt(i).CycleDateEnd = periodChange.begin < records.ElementAt(i+1).CycleDateBegin ? periodChange.begin : records.ElementAt(i).CycleDateEnd;
		//					records.ElementAt(i + 1).CycleDateBegin = records.ElementAt(i).CycleDateEnd<periodChange.end? periodChange.end : records.ElementAt(i).CycleDateEnd; 
		//				};

		//		}
			
				
		//	}
		//	dbContext.Set<T>().UpdateRange(records);
		//	dbContext.Set<T>().AddRange(recsToAdd);

		//	return await dbContext.SaveChangesAsync();
		//}

		public static async Task<int> CycleSpoolProduction_Correction_Cycle_ByShifts<T>(ScaffoldContext dbContext
			//, List<T> records
			, DateTime start_Date
			, DateTime end_Date
			, int placeId)
			where T : StgCacheCycleSpoolProductionByShifts, new()
		{
			//Сдвигаем время чтобы брались данные внутри сменны, иначе может получиться ситуация, 
			//после коррекции могу возникнуть пересекующиеся циклы, если в выборку для корректировки не попали циклы,
			//которые входят в смену. Например выборка с 00.00 по 00.00, и сюда не попал цикл  с 21.00 по 23.00.
			//А так как края расширяются то с этим циклом будте пересекатся первый циел который расширят до second_shift_start_hour
			start_Date = start_Date.Date.AddHours(first_shift_start_hour);
			end_Date = end_Date.Date.AddHours(first_shift_start_hour);


			List<T> records = dbContext.Set<T>().Where(
				r => r.PlaceId == placeId
				//&& r.Composition != "Прочее"
				&& StaticMethods.FirstPeriodContainsPartOfSecondPeriod(start_Date, end_Date, r.CycleDateBegin, r.CycleDateEnd)
				).OrderBy(r => r.CycleDateBegin)
				.ToList();




			////Если записей меньше чем нужно для алгоритма, то выходим
			//if (records.Count<3)
			//{
			//	return;
			//}
			//Будем подгонять циклы под периоды first_shift_start_hour.00 и second_shift_start_hour для первой смены и под периоды 20.00 и 8.00 для второй
			// так же если разрыв между циклами будет попадать в эти участки времени то будем сшивать их

			for (int i = 0; i < records.Count; i++)
			{

				//Чиним смены, за одно учитываем крайние точки i = 0 и  i = records.Count-1
				// Так как records[i] нельзя передовать в ref то содаём буферные переменые
				DateTime startPeriodToFix = records[i].CycleDateBegin;
				DateTime endPeriodToFix = records[i].CycleDateEnd;

				FixShiftsBeginTime(i != 0 ? records[i - 1].CycleDateEnd : new DateTime(), ref startPeriodToFix, ref endPeriodToFix, i == 0 ? true : false);
				records[i].CycleDateBegin = startPeriodToFix;

				//records[i].AverageSpeed += 1;

			}

			for (int i = 0; i < records.Count; i++)
			{

				//Чиним смены, за одно учитываем крайние точки i = 0 и  i = records.Count-1
				// Так как records[i] нельзя передовать в ref то содаём буферные переменые
				DateTime startPeriodToFix = records[i].CycleDateBegin;
				DateTime endPeriodToFix = records[i].CycleDateEnd;

				FixShiftsEndTime(ref startPeriodToFix, ref endPeriodToFix, i != (records.Count - 1) ? records[i + 1].CycleDateBegin : new DateTime(), i == (records.Count - 1) ? true : false);

				records[i].CycleDateEnd = endPeriodToFix;
				//records[i].AverageSpeed += 1;

			}

			return await dbContext.SaveChangesAsync();


		}


		public static void FixShiftsBeginTime(DateTime endCycleBefore, ref DateTime startCycleToFix, ref DateTime endCycleToFix, bool min_Index = false)
		{
			var ShiftBegin = new DateTime();
			var ShiftEnd = new DateTime();

			var currShiftStart = startCycleToFix;
			var currShiftEnd = endCycleToFix;
			//Проверяем чтобы циклы входили диапазон смен, если выходят за границы, то корректируем
			CheckShiftsLimits(ref currShiftStart, ref currShiftEnd);

			SetShiftsLimits(ref ShiftBegin, ref ShiftEnd, currShiftStart, currShiftEnd);
			//Сдвигаем начало цикла к наименьшему из времени до начала смены по стандарту и концом предыдущего цикла
			//Если первый цикл в списке от автоматом назанчаем ему начало как начало смены
			double diffTimeSec = min_Index ? (currShiftStart - ShiftBegin).TotalMilliseconds : Math.Min((currShiftStart - ShiftBegin).TotalMilliseconds, (currShiftStart - endCycleBefore).TotalMilliseconds);
			startCycleToFix = currShiftStart.AddMilliseconds(-diffTimeSec);

		}

		public static void FixShiftsEndTime(ref DateTime startCycleToFix, ref DateTime endCycleToFix, DateTime startCycleNext, bool max_Index = false)
		{
			var ShiftBegin = new DateTime();
			var ShiftEnd = new DateTime();

			var currShiftStart = startCycleToFix;
			var currShiftEnd = endCycleToFix;
			//Проверяем чтобы циклы входили диапазон смен, если выходят за границы, то корректируем
			CheckShiftsLimits(ref currShiftStart, ref currShiftEnd);

			SetShiftsLimits(ref ShiftBegin, ref ShiftEnd, currShiftStart, currShiftEnd);

			//Сдвигаем конец цикла к наименьшему из времени до конца смены по стандарту и началом следующего цикла
			//Если последний цикл в списке то назнаем ему время как конец смены
			double diffTimeSec = max_Index ? (ShiftEnd - currShiftEnd).TotalMilliseconds : Math.Min((ShiftEnd - currShiftEnd).TotalMilliseconds, (startCycleNext - currShiftEnd).TotalMilliseconds);
			endCycleToFix = currShiftEnd.AddMilliseconds(diffTimeSec);
		}

		public static void CheckShiftsLimits(ref DateTime currShiftStart, ref DateTime currShiftEnd)
		{
			//Сначала проверим не выходят ли за границы смен забитые циклы
			// если выходят слева в первой смене
			if (currShiftStart.Hour < first_shift_start_hour && (first_shift_start_hour < currShiftEnd.Hour && currShiftEnd.Hour < second_shift_start_hour))
			{
				currShiftStart = new DateTime(currShiftStart.Year, currShiftStart.Month, currShiftStart.Day, first_shift_start_hour, 0, 0);
			}
			// если выходят справа в первой смене
			if (first_shift_start_hour < currShiftStart.Hour && currShiftStart.Hour < second_shift_start_hour && currShiftEnd.Hour > second_shift_start_hour)
			{
				currShiftEnd = new DateTime(currShiftEnd.Year, currShiftEnd.Month, currShiftEnd.Day, second_shift_start_hour - 1, 59, 59);
			}

			// если выходят слева во второй смене
			if (currShiftStart.Day != currShiftEnd.Day && currShiftStart.Hour < second_shift_start_hour && (second_shift_start_hour < currShiftEnd.Hour || currShiftEnd.Hour < first_shift_start_hour))
			{
				currShiftStart = new DateTime(currShiftStart.Year, currShiftStart.Month, currShiftStart.Day, second_shift_start_hour, 0, 0);
			}

			// если выходят справа во второй смене смене
			if (currShiftStart.Day != currShiftEnd.Day && (second_shift_start_hour < currShiftStart.Hour || currShiftStart.Hour < first_shift_start_hour) && currShiftEnd.Hour > first_shift_start_hour)
			{
				currShiftEnd = new DateTime(currShiftEnd.Year, currShiftEnd.Month, currShiftEnd.Day, first_shift_start_hour - 1, 59, 59);
			}
		}
		public static void SetShiftsLimits(ref DateTime ShiftBegin, ref DateTime ShiftEnd, DateTime currShiftStart, DateTime currShiftEnd)
		{


			bool firstShift = (first_shift_start_hour <= currShiftStart.Hour && currShiftStart.Hour < second_shift_start_hour) ? true : false;

			if (firstShift)
			{

				ShiftBegin = new DateTime(currShiftStart.Year, currShiftStart.Month, currShiftStart.Day, first_shift_start_hour, 0, 0);
				ShiftEnd = new DateTime(currShiftEnd.Year, currShiftEnd.Month, currShiftEnd.Day, second_shift_start_hour - 1, 59, 59);

			}
			else
			{
				//если конец дня то начало смены в тот же день, если же нет то начало смены в предыдущий день и наоборот
				if (currShiftStart.Hour >= second_shift_start_hour)
				{
					ShiftBegin = new DateTime(currShiftStart.Year, currShiftStart.Month, currShiftStart.Day, second_shift_start_hour, 0, 0);
					ShiftEnd = new DateTime(currShiftStart.AddDays(1).Year, currShiftStart.AddDays(1).Month, currShiftStart.AddDays(1).Day, first_shift_start_hour - 1, 59, 59);
				}
				else
				{
					ShiftBegin = new DateTime(currShiftStart.AddDays(-1).Year, currShiftStart.AddDays(-1).Month, currShiftStart.AddDays(-1).Day, second_shift_start_hour, 0, 0);
					ShiftEnd = new DateTime(currShiftStart.Year, currShiftStart.Month, currShiftStart.Day, first_shift_start_hour - 1, 59, 59);
				}
			}
		}
		public static string RightTimeString(TimeSpan interval)// int timeValue, string firstString, string secondString)
		{
			string outputString = "";
			outputString += interval.Days.ToString() + " д. ";
			outputString += interval.Hours.ToString() + " ч. ";
			outputString += interval.Minutes.ToString() + " м. ";
			outputString += interval.Seconds.ToString() + " с.";

			return outputString;
		}
		public static List<TreeViewNode> BuildTree_Generic<T>(List<T> Records)
			where T : class
		{
			List<TreeViewNode> Tree = new List<TreeViewNode>();
			FieldInfo[] myFieldInfo;
			Type myType = typeof(T);

			////Собираем иноформацию о членов класса переданном в перечислителе
			myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

			var recordForCompare = Records.FirstOrDefault();
			bool firstRecord = true;
			bool addedUpperNode = false;

			int rootNodeIndex = 0;
			Records.ForEach(r =>
			{
				addedUpperNode = false;//Если добавлен начальный узел в ветке по текущей записи, то остальные язначения в этой запаси тоже можно добавить
				for (int i = 0; i < //3
			myFieldInfo.Length
			; i++)
				{


					var fieldValue = myFieldInfo[i].GetValue(r).ToString().Trim();
					var diffFieldValue = myFieldInfo[i].GetValue(recordForCompare).ToString().Trim();

					if (addedUpperNode || fieldValue != diffFieldValue || firstRecord)
					{
						if (i == 0)
						{
							rootNodeIndex++;
						}

						var parent = (i == 0 ? "#" : rootNodeIndex.ToString() + "-" + myFieldInfo[i - 1].GetValue(r).ToString().Trim());


						Tree.Add(new TreeViewNode
						{
							id = rootNodeIndex.ToString() + "-" + fieldValue,
							parent = parent,
							text = fieldValue
						});
						addedUpperNode = true;



					}

				}
				firstRecord = false;
				recordForCompare = r;
			}
			);

			return Tree;
		}
		public static List<TreeViewNode> BuildTree(List<PaRecordsByFilter> Records, List<string> places_with_wrong_rec = null, List<int> devices_with_wrong_rec = null, List<string> dates_with_wrong_rec = null, int device_id = 0)
		{
			List<TreeViewNode> Tree = new List<TreeViewNode>();
			//Строим корневые узлы
			int curr_ID = 1;
			int curr_root_ID = 1;
			int curr_2_level_node_ID = 0;
			int curr_3_level_node_ID = 0;
			string curr_root_node = "";
			int curr_2_level_node = 0;
			string curr_3_level_node = "";
			string curr_sort_date = "";
			bool changed_curr_root_node = false;
			bool changed_curr_2_level_node = false;
			bool has_wrong_rec = false;
			//string curr_parent = "";
			foreach (PaRecordsByFilter rec in Records)
			{
				changed_curr_root_node = false;
				changed_curr_2_level_node = false;
				if (device_id == 0)
				{


					if (rec.PlaceName != curr_root_node)
					{
						has_wrong_rec = places_with_wrong_rec.Contains(rec.PlaceName);
						//появился новый корневой узел, добавляем его
						Tree.Add(new TreeViewNode
						{
							id = curr_ID.ToString(),
							parent = "#",
							text = (has_wrong_rec ? @"<span style=""color:red"">" : "") + rec.PlaceName + (has_wrong_rec ? "</span>" : "")
						});
						curr_root_ID = curr_ID;
						curr_root_node = rec.PlaceName;
						changed_curr_root_node = true;
						curr_ID++;
					}
				}


				//появился новый второго уровня узел, добавляем его
				if (rec.DEVICE_ID != curr_2_level_node || changed_curr_root_node)
				{
					has_wrong_rec = devices_with_wrong_rec.Contains(rec.DEVICE_ID);
					Tree.Add(new TreeViewNode
					{
						id = curr_ID.ToString(),
						//parent = curr_root_ID.ToString(),
						//если поиск по устройству, не добавляем поиск по месту
						parent = (device_id > 0) ? "#" : curr_root_ID.ToString(),
						text = (has_wrong_rec ? @"<span style=""color:red"">" : "") + rec.DEVICE_NAME + (has_wrong_rec ? "</span>" : "")
					});
					curr_2_level_node = rec.DEVICE_ID;
					curr_2_level_node_ID = curr_ID;
					changed_curr_2_level_node = true;
					curr_ID++;
				}
				curr_sort_date = rec.RECORD_TIME.ToShortDateString();
				//проверяем условия добавки нового узла, а именно изменилась дата, изменился один из родительских узлов
				if (curr_sort_date != curr_3_level_node || changed_curr_root_node || changed_curr_2_level_node)
				{
					has_wrong_rec = dates_with_wrong_rec.Contains(curr_2_level_node + ";" + curr_sort_date);
					Tree.Add(new TreeViewNode
					{
						id = curr_ID.ToString(),
						parent = curr_2_level_node_ID.ToString(),
						text = (has_wrong_rec ? @"<span style=""color:red"">" : "") + rec.RECORD_TIME.ToShortDateString() + (has_wrong_rec ? "</span>" : "")
					});
					curr_3_level_node = curr_sort_date;
					curr_3_level_node_ID = curr_ID;
					curr_ID++;
				}

				has_wrong_rec = rec.RecordStatus == "Не походит по критерию";
				//Добавляем информацию о записи
				Tree.Add(new TreeViewNode
				{
					//добавляем "-" , чтобы не путать с ид корневого и втогоро уровня
					id = rec.ID.ToString() + "-" + rec.ID_RECORD.ToString(),
					parent = curr_3_level_node_ID.ToString(),
					text = (has_wrong_rec ? @"<span style=""color:red"">" : "") + "Время: " + rec.RECORD_TIME.ToString() + ", статус: " + rec.RecordStatus + ", Значение: " + rec.MEASURE_VALUE.ToString() + (has_wrong_rec ? "</span>" : "")
				});
				curr_ID++;

			}
			return Tree;

		}
		public static List<TreeViewNode> BuildTree(ScaffoldContext context)
		{
			List<TreeViewNode> Tree = new List<TreeViewNode>();

			//С учетом того что в таблице DevicePlaces не забиты имена переделов, скачиваем из vPlaces, чтобы можно было из получить по ID
			Dictionary<int, string> places = context.DevicePlaces.Select(r => new { r.PlaceId, r.PlaceName }).Distinct()
				//.GammaPlaces
				//				.FromSql(@"select ROW_NUMBER() OVER(ORDER BY  PlaceID ASC) as 'ID', * from(
				//select PlaceID, name as 'PlaceName' from vPlaces
				//union
				//select PlaceID, PlaceName from STG_PlacesNames
				//) a")
				.ToDictionary(p => p.PlaceId, p => p.PlaceName);

			foreach (int placeId in context.DevicePlaces.Select(d => d.PlaceId).Distinct().ToList())
			{
				Tree.Add(new TreeViewNode
				{
					id = placeId.ToString(),
					parent = "#",
					text = places[placeId]
				});


			}
			foreach (DevicePlacesT devicePlaces in context.DevicePlacesT.FromSql(@"select ROW_NUMBER() OVER(ORDER BY PlaceID ASC) AS 'ID',
[DEVICE_ID] as DeviceId,[PlaceID], [Multiplier] FROM[dbo].[DevicePlaces]").Include(d => d.Device))
			//foreach (DevicePlaces devicePlaces in context.DevicePlaces.FromSql("select * from DevicePlaces"))
			{
				//   Tree.Add(new TreeViewNode { id = devicePlaces.PlaceId.ToString() + "-" + devicePlaces.DeviceId.ToString(), parent = devicePlaces.PlaceId.ToString(), text = "1" });
				Tree.Add(new TreeViewNode { id = devicePlaces.PlaceId.ToString() + "-" + devicePlaces.DeviceId.ToString(), parent = devicePlaces.PlaceId.ToString(), text = devicePlaces.Device.DeviceName.ToString() });
			}
			return Tree;
		}

		public static void Set_period(ref DateTime start_Date, ref DateTime end_Date, int selectedPeriod)
		{
			//фильтр на период
			if (selectedPeriod != 0)
			{
				start_Date = DateTime.Now.Date.AddDays(-selectedPeriod);
				end_Date = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
			}
		}
		//процедура выводе всех возможный взаимо положений двух отрезков (два случая когда начало второго отрезка содержится в первом и конец второго отрезка в первом равназначны обратной ситуации так что отбрасываются)
		public static string Filter_by_time_byPeriod(DateTime start_date, DateTime end_date, string startPeriod, string endPeriod)
		{
			//RECORD_TIME
			return $@"(
				--Начало первого отрезка содержится во втором
				( {startPeriod}  <= CONVERT(Datetime, '{ start_date.Date.ToString("yyyy-MM-dd 00:00:00")}', 120) and {endPeriod} >= CONVERT(Datetime, '{start_date.Date.ToString("yyyy-MM-dd 00:00:00") }', 120))
				OR 
				--Конец первого отрезка содержится во втором
				( {startPeriod}  <= CONVERT(Datetime, '{end_date.Date.ToString("yyyy-MM-dd 23:59:59") }', 120) and {endPeriod}  >= CONVERT(Datetime, '{end_date.Date.ToString("yyyy-MM-dd 23:59:59")}', 120))
				OR 
				--Первый отрезок содержится во втором 
				({startPeriod}  <= CONVERT(Datetime, '{start_date.Date.ToString("yyyy-MM-dd 00:00:00")} ', 120) and    CONVERT(Datetime, '{end_date.Date.ToString("yyyy-MM-dd 23:59:59")}', 120)<= {endPeriod})
				 OR 
				--Второй отрезок содержится в первом
				(CONVERT(Datetime, '{start_date.Date.ToString("yyyy-MM-dd 00:00:00")}', 120) <={startPeriod}  and {endPeriod}  <= CONVERT(Datetime, '{end_date.Date.ToString("yyyy-MM-dd 23:59:59")}', 120))
				)";
			;
		}
		public static string Filter_by_time_byPeriod_withTime(DateTime start_date, DateTime end_date, string startPeriod, string endPeriod)
		{
			//RECORD_TIME
			return "("
				//Начало первого отрезка содержится во втором
				+ "(" + startPeriod + " <= CONVERT(Datetime, '" + start_date.ToString("yyyy-MM-dd HH:mm:ss") + "', 120) and " + endPeriod + " >= CONVERT(Datetime, '" + start_date.ToString("yyyy-MM-dd HH:mm:ss") + "', 120))"
				+ " OR "
				//Конец первого отрезка содержится во втором
				+ "(" + startPeriod + " <= CONVERT(Datetime, '" + end_date.ToString("yyyy-MM-dd HH:mm:ss") + "', 120) and " + endPeriod + " >= CONVERT(Datetime, '" + end_date.ToString("yyyy-MM-dd HH:mm:ss") + "', 120))"
				+ " OR "
				//Первый отрезок содержится во втором 
				+ "(" + startPeriod + " <= CONVERT(Datetime, '" + start_date.ToString("yyyy-MM-dd HH:mm:ss") + "', 120) and " + endPeriod + " >= CONVERT(Datetime, '" + end_date.ToString("yyyy-MM-dd HH:mm:ss") + "', 120))"
				+ " OR "
				//Второй отрезок содержится в первом
				+ "(" + startPeriod + " >= CONVERT(Datetime, '" + start_date.ToString("yyyy-MM-dd HH:mm:ss") + "', 120) and " + endPeriod + " <= CONVERT(Datetime, '" + end_date.ToString("yyyy-MM-dd HH:mm:ss") + "', 120))"
				+ ")";
			;
		}
		public static string Filter_by_time(DateTime start_date, DateTime end_date, string field_for_filter, bool isDoubleQuote = false)
		{
			//RECORD_TIME
			return field_for_filter + $" >= CONVERT(Datetime, {(isDoubleQuote ? "'" : "")}'  {start_date.Date.ToString("yyyy-MM-dd HH:mm:ss")} {(isDoubleQuote ? "'" : "")}', 120) and {field_for_filter}  <= CONVERT(Datetime, {(isDoubleQuote ? "'" : "")}' { end_date.Date.ToString("yyyy-MM-dd 23:59:59")} {(isDoubleQuote ? "'" : "")}', 120)";
		}
		public static string SQL_DateTime(DateTime date)
		{

			return " CONVERT(Datetime, '" + date.ToString("yyyy-MM-dd HH:mm:ss") + "', 120)";
		}

		public static string Filter_by_time_with_time(DateTime start_date, DateTime end_date, string field_for_filter)
		{
			//RECORD_TIME
			return field_for_filter + " >= CONVERT(Datetime, '" + start_date.ToString("yyyy-MM-dd HH:mm:ss") + "', 120) and " + field_for_filter + " <= CONVERT(Datetime, '" + end_date.ToString("yyyy-MM-dd 23:59:59") + "', 120)";
		}

		public static void Start_date_check(ref DateTime start_Date)
		{
			if (start_Date == DateTime.Parse("01.01.0001 0:00:00"))
			{
				start_Date = DateTime.Now.Date.AddDays(-1);
			}


		}

		//Если не ввели конечную дату
		public static void End_date_check(ref DateTime end_Date)
		{
			if (end_Date == DateTime.Parse("01.01.0001 0:00:00"))
			{
				end_Date = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
			}

		}

		public static int Sql_injection_check(string selectedItems)
		{

			//проверка на sql injection, на присутствие ";", чтобы не добавили лишнего кода
			Regex rx = new Regex(@"\;+",
			RegexOptions.Compiled | RegexOptions.IgnoreCase);
			MatchCollection matches = rx.Matches(selectedItems);
			//если нашли лишнее, то выкидываем на выбор, так как это похоже на проверку на sql injection
			return matches.Count;

		}

		public static void Check_dates(ref DateTime start_Date, ref DateTime end_Date, int selectedPeriod, int selectedMonth = 0, int selectedYear = 0, int lastDay = 0)
		{
			//если задан период, выставляем даты
			StaticMethods.Set_period(ref start_Date, ref end_Date, selectedPeriod);

			//Если не ввели начальную дату
			StaticMethods.Start_date_check(ref start_Date);

			//Если не ввели конечную дату
			StaticMethods.End_date_check(ref end_Date);

			//Если выбран отчет за месяц
			if (selectedMonth != 0)
			{
				//учитываем так же выбран ли год для отчета, если нет то ставим текущий
				start_Date = new DateTime(selectedYear != 0 ? selectedYear : DateTime.Now.Year, selectedMonth != 0 ? selectedMonth : DateTime.Now.Month, 1);
				end_Date = start_Date
					.AddMonths(1)
					.AddSeconds(-1);
			}
			else if (selectedYear != 0)//Выбран только год
			{
				start_Date = new DateTime(selectedYear, 1, 1);
				end_Date = new DateTime(selectedYear, 12, 31);
			}

			//Отчет за последние сутки 
			if (lastDay == 1)
			{
				end_Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddSeconds(-1);
				start_Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-1);
			}
		}
		public static string Generate_sql_string(string sqlName
			, DateTime start_Date
			 //Дата по которую будем собирать записи
			 , DateTime end_Date
			//Показывать ли записи за период

			, ScaffoldContext context = null
			, int placeId = 0
			, bool isByDay = false
			, int tesYear = 0
			, string sku = null

			)
		{
			#region Generate_sql_string
			string byPeriod = StrByPeriod(sqlName);

			//заводим соотвествие между Place и ID в таком стиле, а не берем из таблицы DevicePlaces,
			//потому что в отчете будут соберательные агригаторные места с несколькими ID

			Dictionary<string, string> placesAndID = new Dictionary<string, string> { { "BDM1", "1" }, { "BDM2", "2" }, { "Synhro4", "6" }, { "X5", "7" }, { "SDF", "13" } };

			string sql_query_string = "";
			string SKU_list = "";


			switch (sqlName)
			{

				case "ConsumptionByManufactureByPeriod":
					sql_query_string = @"--Концепция, сначала собираем суммарные данные по датчикам за нужные период, суммируя по каждому нужному параметру
--Но с учтом того что получается данные по месту только за один параметр типа, а выражения суммировать нельзя

-- По этому еще раз суммируем по с группировкой по месту
select ROW_NUMBER() OVER(ORDER BY PlaceID ASC) AS 'ID', PlaceID, round (sum(ISNULL( GasValue,0))/1000,3) as GasConsumption, round(sum(ISNULL(SteamValue,0)),3) as SteamConsumption, round( sum(ISNULL(Value,0))/1000,3) as EnergyConsumption  From
( select * from [dbo].[ElectricEnergyPlacesHourly] ee
		 where 
"
+ StaticMethods.Filter_by_time(start_Date, end_Date, "ee.Date") +
//	@" and ee.PlaceID in (";
////список мест гибкий, так что генирируем автоматически
//foreach (KeyValuePair<string, string> place in placesAndID)
//{
//	sql_query_string += place.Value + ",";
//}
////убираем лишнюю запятую
//sql_query_string = sql_query_string.Substring(0, sql_query_string.Length - 1);

//sql_query_string +=")";
//sql_query_string += 
@"


) t 

	group by PlaceID";


					break;

				case "SkuDataByShifts":

					sql_query_string = @"
--SkuDataByShifts
"
+ SqlCmdStrings.csp_by_shifts

+ @" and " + StaticMethods.Filter_by_time_byPeriod_withTime(start_Date, end_Date, "CycleDateBegin", "CycleDateEnd")
+ (sku != null ? $" AND Composition ='{sku}' " : "")
+ $" AND PlaceId ='{placeId}' "


+ $@"
declare @minCycleDate DateTime
set  @minCycleDate = (select min(CycleDateBegin) from @csp)

declare @maxCycleDate DateTime
set  @maxCycleDate = (select max(CycleDateEnd) from @csp)


select ROW_NUMBER() over ( order by SortofProduction asc) as ID,
SortofProduction as sku,
ShiftId as ShiftId,
CycleDateBegin,
CycleDateEnd,
FluidStream,
Wetness,
  sum(Weight) as ShiftProductivity,
	CASE sum(Weight)
     WHEN 0 THEN 0  
      ELSE    isnull( (SUM(EnergyConsumption)/(sum(Weight)/1000))/1000,0)
	  end AS EnergyConsumptionPerTonne,
	 CASE sum(Weight)
	  WHEN 0 THEN 0  
      ELSE   isnull((SUM(SteamConsumption)/(sum(Weight)/1000)),0)
	  end AS SteamConsumptionPerTonne,
	  
	 CASE sum(Weight)
	  WHEN 0 THEN 0  
      ELSE    isnull( (SUM(GasConsumption)/(sum(Weight)/1000)/1000),0)
	  end AS GasConsumptionPerTonne,
	 Machinist_name,
	   isnull(AVG(AverageSpeed),0) AS AverageSpeed
 from
(SELECT 
ROW_NUMBER() over ( order by b.Composition asc) as ID,	  

	  b.Composition AS SortofProduction,
	  b.CycleDateBegin,
	  b.CycleDateEnd,
	  shiftId,
      round(isnull([FluidStream],0),4) as FluidStream,
	  round(isnull([Wetness],0),4) as Wetness,
	  isnull(Weight,0) as weight,
	  SUM(isnull(a.Amount,0))  AS EnergyConsumption,
	  SUM(isnull(a.SteamValue,0))  AS SteamConsumption,	  
	  SUM(isnull(a.GasValue,0)) AS GasConsumption,
	  Machinist_name,
	  isnull(b.AverageSpeed,0) AS AverageSpeed	  
	FROM
	 @csp b 
	left JOIN ( 
	  SELECT a.Value AS Amount, a.GasValue, a.SteamValue, a.PlaceID, a.Date
	  FROM [ElectricEnergyPlacesHourly] a
	  --Это период месяц плюс день в начале чтобы если начала цикла чуть выходит наз границу месяца, то по нему данные бы попадали
	  
	  WHERE a.Date BETWEEN @minCycleDate and @maxCycleDate 

	  ) a ON b.PlaceID = a.PlaceID and a.Date between b.CycleDateBegin and b.CycleDateEnd
	

--	WHERE  --AND b.[Weight]/1000 > 20 --AND b.Sort LIKE '%код%'
	
  GROUP BY  ShiftId,b.CycleDateBegin,b.CycleDateEnd, b.Composition,weight,Machinist_name,AverageSpeed,FluidStream,Wetness
--,c.AvgSpeed
	HAVING  SUM(isnull(a.Amount,0))/1000 > 0) a
GROUP BY  ShiftId, CycleDateBegin, CycleDateEnd, Machinist_name,SortofProduction,FluidStream,Wetness

     ";
					break;

				case "ConsumptionByCycle":

					sql_query_string = @"
--ConsumptionByCycle
"
//+ (isByDay ? SqlCmdStrings.Csp_by_day_test(start_Date,end_Date,placeId) : SqlCmdStrings.csp_by_month)
//+SqlCmdStrings.avg_speed_over_csp_by_cycles_ao01
////+ $" and csp.PlaceID IN ({placeId}) "
////+ @" and " + StaticMethods.Filter_by_time_byPeriod(start_Date, end_Date, "CycleDateBegin", "CycleDateEnd")
+ (isByDay ? SqlCmdStrings.csp_by_day : SqlCmdStrings.csp_by_month)
+ $" and csp.PlaceID IN ({placeId}) "
+ @" and " + StaticMethods.Filter_by_time_byPeriod(start_Date, end_Date, "CycleDateBegin", "CycleDateEnd")





+ $@"
declare @minCycleDate DateTime
set  @minCycleDate = (select min(CycleDateBegin) from @csp)

declare @maxCycleDate DateTime
set  @maxCycleDate = (select max(CycleDateEnd) from @csp)

SELECT 
ROW_NUMBER() over ( order by b.Composition asc) as ID,	  
b.PlaceID , 
	  b.Place as Place ,
	  b.Composition AS SortofProduction,
	 
	  b.CycleDateBegin ,
	  b.CycleDateEnd ,
	  b.Weight/1000 as CapacityOfBO, 
	  
	  SUM(isnull(a.Amount,0))/1000 AS EnergyConsumption, 
	  SUM(isnull(a.GasValue,0))/1000 AS GasConsumption,
	  SUM(isnull(a.SteamValue,0)) AS SteamConsumption,

	  CASE b.Weight  
     WHEN 0 THEN 0  
      ELSE     (SUM(isnull(a.Amount,0))/(b.Weight/1000))/1000
	  end AS EnergyConsumptionPerTonne,
	  
	  CASE b.Weight
	  WHEN 0 THEN 0  
      ELSE     (SUM(isnull(a.GasValue,0))/(b.Weight/1000)/1000)
	  end AS GasConsumptionPerTonne,

	  CASE b.Weight
	  WHEN 0 THEN 0  
      ELSE   (SUM(isnull(a.SteamValue,0))/(b.Weight/1000))
	  end AS SteamConsumptionPerTonne,
	  
	  
	  
	  --c.AvgSpeed
	   AVG(isnull(b.AverageSpeed,0)) AS AverageSpeed	  
	FROM
	 @csp b 
	left JOIN ( 
	  SELECT a.Value AS Amount, a.GasValue, a.SteamValue, a.PlaceID, a.Date
	  FROM [ElectricEnergyPlacesHourly] a
	  --Это период месяц плюс день в начале чтобы если начала цикла чуть выходит наз границу месяца, то по нему данные бы попадали
	  
	  WHERE a.Date BETWEEN @minCycleDate and @maxCycleDate and a.PlaceId = {placeId} 

	  ) a ON b.PlaceID = a.PlaceID and a.Date between b.CycleDateBegin and b.CycleDateEnd
	

	WHERE b.PlaceID IN ({placeId}) --AND b.[Weight]/1000 > 20 --AND b.Sort LIKE '%код%'
	
  GROUP BY b.PlaceID,b.Place,  /*b.Sort,*/ b.CycleDateBegin, b.CycleDateEnd, b.Composition, Weight
--,c.AvgSpeed
	HAVING  SUM(isnull(a.Amount,0))/1000 > 0
order by b.CycleDateBegin
     ";

					break;
				case "TERperMonthperTonne":

					Generate_SKU_list(start_Date, end_Date, context);

					sql_query_string = @"select  * from [dbo].[STG_Cache_TERperMonths]
where SKU in  "
+ Generate_SKU_list(start_Date, end_Date, context)
+ @" and (month <= " + (tesYear == 0 ? DateTime.Now.Month : 12).ToString()
+ @" or month = 13 )"
+ " and year = " + (tesYear == 0 ? DateTime.Now.Year : tesYear).ToString();
					//>=1 выходит из контекста;
					break;

				case "TERperCycleperTonne":
					SKU_list = Generate_SKU_list(start_Date, end_Date, context);
					sql_query_string = @"
--TERperCycleperTonne"
+ SqlCmdStrings.csp_by_month
+ " and csp.PlaceID IN (1, 2) "
+ "and " + StaticMethods.Filter_by_time_byPeriod(start_Date, end_Date, "csp.CycleDateBegin", "csp.CycleDateEnd")//проверяем попадение фильтра по периоду в циклы производства
+ @" and csp.Composition in " + SKU_list


+ $@"
SELECT
ROW_NUMBER() over ( order by b.Composition asc) as ID,	  
b.PlaceID , 
	  b.Place as PlaceName ,
	  b.Composition AS SKU,
	 b.month,
	 b.year,
	  b.CycleDateBegin ,
	  b.CycleDateEnd ,
	  b.Weight/1000 as ValueOfBO, 
	  
	  CASE b.Composition   
     WHEN 'Прочее' THEN 0  
      ELSE     ROUND((SUM(isnull(a.Amount,0))/(b.Weight/1000))/1000, 3)
	  end AS EnegryConsumptionperTonne,
	  
	  CASE b.Composition
	  WHEN 'Прочее' THEN 0  
      ELSE     ROUND((SUM(isnull(a.GasValue,0))/(b.Weight/1000)/1000), 3)
	  end AS GasConsumptionperTonne,

	  CASE b.Composition
	  WHEN 'Прочее' THEN 0  
      ELSE    ROUND((SUM(isnull(a.SteamValue,0))/(b.Weight/1000)), 3)
	  end AS SteamConsumptionperTonne,
	  
	  
	  
	  --c.AvgSpeed
	   isnull(b.AverageSpeed,0) AS AverageSpeed	  
	FROM
	 @csp b 
	left JOIN ( 
	  SELECT a.Value AS Amount, a.GasValue, a.SteamValue, a.PlaceID, a.Date
	  FROM [ElectricEnergyPlacesHourly] a
	  --Это период месяц плюс день в начале чтобы если начала цикла чуть выходит наз границу месяца, то по нему данные бы попадали
	  
	  WHERE a.Date BETWEEN '{ start_Date.AddDays(-10).ToString("yyyyMMdd HH:mm:ss")}' and '{end_Date.AddDays(10).ToString("yyyyMMdd HH:mm:ss")}'"

						//+ new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1).ToString("yyyyMMdd HH:mm:ss") //20181231 00:00:00
						//+ @"' AND '"
						//+ new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1)
						//.AddMonths(-1)
						//.ToString("yyyyMMdd HH:mm:ss") //20190901 00:00:00
						+ @"

	  ) a ON b.PlaceID = a.PlaceID AND a.Date BETWEEN b.CycleDateBegin AND b.CycleDateEnd
	

	WHERE b.PlaceID IN (1, 2) --AND b.[Weight]/1000 > 20 --AND b.Sort LIKE '%код%'
	
  GROUP BY b.Place, b.PlaceID, /*b.Sort,*/ b.CycleDateBegin, b.CycleDateEnd, Weight, b.Composition, b.year,b.month
	, b.AverageSpeed
	HAVING  SUM(a.Amount)/1000 > 0
	--order by SKU";
					break;

				case "TER_Plan":
					sql_query_string = @"
--TER_Plan
SELECT [Id_STG_Plan_TER] as [Id]
      ,[SKU]
      ,[PlaceID]
      , '' as [PlaceName]
      ,[year]
      ,[month]
      ,BO_Value as [ValueOfBO]
      ,Energy_perTonne_norm as [EnegryConsumptionperTonne]
      ,Gas_perTonne_norm as [GasConsumptionperTonne]
      ,Steam_perTonne_norm as [SteamConsumptionperTonne]
      ,Avarage_speed_norm as [AverageSpeed]
  FROM [dbo].[STG_Plan_TER]

					where sku in "
					+ Generate_SKU_list(start_Date, end_Date, context)
					+ @" and (month <= " + (tesYear == 0 ? DateTime.Now.Month : 12).ToString()
					+ @" or month = 13 )"
					+ " and year = " + (tesYear == 0 ? DateTime.Now.Year : tesYear).ToString();

					break;

				case "Fill_Cache_CycleSpoolProduction_byMonth":
					sql_query_string = SqlCmdStrings.Fill_Cache_CycleSpoolProduction(start_Date, end_Date, isDay: false);
					break;
				case "Fill_Cache_CycleSpoolProduction_byDay":
					sql_query_string = SqlCmdStrings.Fill_Cache_CycleSpoolProduction(start_Date, end_Date, isDay: true);
					break;
				case "Fill_Cache_CycleSpoolProduction_byShifts":
					//Даты сдвигаем относительно начала смен
					sql_query_string = SqlCmdStrings.Fill_Cache_CycleSpoolProduction_byShifts(start_Date, end_Date);
					break;
				case "Update_avg_speed_Cache_CycleSpoolProduction_byShifts":
					sql_query_string = $@"{SqlCmdStrings.csp_by_shifts}
					 and csp.PlaceID IN(1, 2) 
--проверяем попадение фильтра по периоду в циклы производства
and  {StaticMethods.Filter_by_time_byPeriod(start_Date, end_Date, "csp.CycleDateBegin", "csp.CycleDateEnd")} 
{SqlCmdStrings.avg_speed_over_csp_by_shifts}
{SqlCmdStrings.Update_csp_byShifts_avg_speed}
";

					break;
				case "Update_avg_speed_Cache_CycleSpoolProduction_byDay":
				case "Update_avg_speed_Cache_CycleSpoolProduction_byMonth":

					sql_query_string = $@"{(sqlName == "Update_avg_speed_Cache_CycleSpoolProduction_byDay" ? SqlCmdStrings.csp_by_day : SqlCmdStrings.csp_by_month)}
					 and csp.PlaceID IN(1, 2) 
--проверяем попадение фильтра по периоду в циклы производства
and  {StaticMethods.Filter_by_time_byPeriod(start_Date, end_Date, "csp.CycleDateBegin", "csp.CycleDateEnd")} 
{SqlCmdStrings.avg_speed_over_csp}
{SqlCmdStrings.Update_csp_avg_speed($@"STG_Cache_CycleSpoolProduction_{byPeriod}")}
";
					break;
				case "TER_avg_permonth":
					bool isTotal = false;
					string year = (tesYear == 0 ? DateTime.Now.Year.ToString() : tesYear.ToString());
					string TERperMonths_filter =

						 //@" and ( month <= " + end_Date.Month + " or month = 13)"+
						 @" and year = "
						+ year;
					string csp_filter = " and PlaceID IN (1, 2) "
						+ @" and Composition not like '%Прочее%'"
						//						+ @" and month <= " + end_Date.Month
						+ @" and year = "
						+ year;
					sql_query_string =

						@"
						--TER_avg_permonth"
						+ SqlCmdStrings.clear_STG_Cache_TERperMonths + TERperMonths_filter
						+ SqlCmdStrings.csp_by_month + csp_filter
						+ " order by csp.PlaceID, year, month, Cycle"

					// сгенерируем строку для переданого периода(isTotal== false)
					+ TER_avg_sql_string(isTotal, start_Date, end_Date, context, tesYear: tesYear);
					isTotal = true;
					//сгенерируем строку для итого за период с начала месяца (isTotal==true)
					sql_query_string += TER_avg_sql_string(isTotal, start_Date, end_Date, context, tesYear: tesYear);


					break;

				case "fill_Cache_TagDump":
					sql_query_string += @"--fill_Cache_TagDump
                        "
						+ SqlCmdStrings.clear_Cache_TagDump
						+ " and " + StaticMethods.Filter_by_time(start_Date, end_Date, "Date")
						+ SqlCmdStrings.fill_Cache_TagDump
						+ " and " + StaticMethods.Filter_by_time(start_Date, end_Date, "Date");
					break;

				case "EnergyConsumptionByManufactureByHour":
					sql_query_string = $@"--Концепция, сначала собираем суммарные данные по датчикам за нужные период, суммируя по каждому нужному параметру
--Но с учтом того что получается данные по месту только за один параметр типа, а выражения суммировать нельзя

-- По этому еще раз суммируем по с группировкой по месту
select ROW_NUMBER() OVER(ORDER BY dateadd(HOUR,datediff(HOUR,0,t.Date),0) ASC) AS 'ID', 'ConsumptionByManufactureByPeriod' as Discriminator, PlaceID, dateadd(HOUR,datediff(HOUR,0,t.Date),0) as Measure_Date, CAST(0 as FLOAT) as GasConsumption, CAST(0 as FLOAT) as SteamConsumption, round( sum(ISNULL(Value,0)),3) as EnergyConsumption  From
( select * from [dbo].[ElectricEnergyPlacesHourly] ee
		 where 
{StaticMethods.Filter_by_time(start_Date, end_Date, "ee.Date")}


) t 

	group by PlaceID, dateadd(HOUR,datediff(HOUR,0,t.Date),0)
	having(sum(ISNULL(Value,0))<>0)
	";
					break;

				case "EnergyConsumptionByManufactureByHour_t":
					//Запрос стоится по принципу сначала агригируем все показания по дате считивания, и места считивания
					//затем берем список уникальных дат, и на каждую дату навешиваем названные соотвественно месту расположения суммарные данные по месту
					// как результат получаем список строк типа "дата, энергия_по_1_месту,энергия_по_2_месту, и т.д."
					sql_query_string = @"
declare @temp_EnergyConsumption table(ID int, EnergyConsumption float, PlaceID int, MEASURE_Date DATETIME);
declare @temp_Consumption_Dates table(MEASURE_Date DATETIME); 

--собираем агрегированные данные по электроэнергии в заданных местах
--везде используем просто join, потому что предполагаем что если не заведено место посчета приборов, то данные все равно бесполезные
insert into @temp_EnergyConsumption
SELECT  ROW_NUMBER() OVER(ORDER BY dateadd(HOUR,datediff(HOUR,0,a.Date),0) ASC) AS 'ID', SUM(a.Value )  AS EnergyConsumption,a.PlaceID , dateadd(HOUR,datediff(HOUR,0,a.Date),0)  as MEASURE_Date
						
                    FROM [ElectricEnergyPlacesHourly] a
  --       join DevicePlaces e on e.DEVICE_ID = a.DEVICE_ID and 
		----учитываем промежутки расположения приборов в указаных местах 
		--e.BeginDateOfLocation<= f.RECORD_TIME and
		-- e.EndDateOfLocation>= f.RECORD_TIME 
		 where
--f.RECORD_TIME >'2019-01-03'
" + StaticMethods.Filter_by_time(start_Date, end_Date, "a.Date")
+ @" and a.PlaceID in (";
					//список мест гибкий, так что генирируем автоматически
					foreach (KeyValuePair<string, int[]> place in Constants.places)
					{
						foreach (var place_Id in place.Value)
						{
							sql_query_string += place_Id.ToString() + ",";
						}

					}
					//убираем лишнюю запятую
					sql_query_string = sql_query_string.Substring(0, sql_query_string.Length - 1);
					sql_query_string += @") 
GROUP BY  a.PlaceID, dateadd(HOUR,datediff(HOUR,0,a.Date),0) 
--having  SUM(a.Value )>0
-- собираем список уникальных дат для того чтобы на каждую такую дату прикрепить
--имеющиеся данные по потреблению на соотвествующих местах
insert into @temp_Consumption_Dates		 
select distinct MEASURE_Date		 
		 from @temp_EnergyConsumption		 
		 order by MEASURE_Date

select distinct ROW_NUMBER() OVER(ORDER BY a.MEASURE_Date ASC) AS 'ID', a.MEASURE_Date,  ";
					int ind_place = 0;
					foreach (KeyValuePair<string, int[]> place in Constants.places)
					{

						sql_query_string += $"isnull(EnergyConsumption_byT{ind_place},0) as EnergyConsumption_byT{ind_place},";
						ind_place++;
					}
					//подсчитываем тотал
					ind_place = 12;
					sql_query_string += "round(";
					foreach (KeyValuePair<string, int[]> place in Constants.places_total)
					{
						sql_query_string += $"isnull(EnergyConsumption_byT{ind_place},0)+";
						ind_place++;
					}
					//Убираем лишний +
					sql_query_string = sql_query_string.Substring(0, sql_query_string.Length - 1);
					sql_query_string += ",3) ";



					sql_query_string += @"as 'EnergyConsumption_Total'
		from @temp_Consumption_Dates a";
					ind_place = 0;
					foreach (KeyValuePair<string, int[]> place in Constants.places)
					{
						string sPlaces_list = "";

						foreach (var place_Id in place.Value)
						{
							sPlaces_list += place_Id.ToString() + ',';
						}
						sPlaces_list = sPlaces_list.Substring(0, sPlaces_list.Length - 1);

						sql_query_string += @"
		--прикрепляем данные по " + place.Key + $@"
		left join (SELECT   sum(EnergyConsumption) AS EnergyConsumption_byT{ind_place}, dateadd(HOUR,datediff(HOUR,0,MEASURE_Date),0) as MEASURE_Date 

		FROM @temp_EnergyConsumption where PlaceID in ({ sPlaces_list })
		group by dateadd(HOUR,datediff(HOUR,0,MEASURE_Date),0) ) T{ind_place} on "
					   + $@"T{ind_place}.MEASURE_Date = a.MEASURE_Date
		";
						ind_place++;
					}
					break;
				case "ConsumptionByBDMByDay":
					sql_query_string = @" SELECT
				 ROW_NUMBER() OVER(ORDER BY dbo.TruncTime(b.CycleDateBegin) ASC) AS 'ID',

			b.Place ,
			b.CycleDateBegin,
			b.CycleDateEnd,
				  b.Composition AS [SortofProduction],


				 --CAST(b.Value as VARCHAR(5)) AS[Граммаж], 
	  
				  ROUND(SUM(a.Amount) / 1000, 2) AS [EnergyConsumption], 
				  b.Weight / 1000 AS[CapacityOfBO],	  
					ROUND((SUM(a.Amount) / (b.Weight / 1000)) / 1000, 3) AS[EnergyConsumptionPerTonne],      
					ROUND(SUM(a.GasValue) / 1000, 3) AS[GasConsumption],
				  SUM(a.SteamValue) AS[SteamConsumption],
	   
	  
				  ROUND((SUM(a.GasValue) / (b.Weight / 1000) / 1000), 3) AS[GasConsumptionPerTonne],
				  ROUND((SUM(a.SteamValue) / (b.Weight / 1000)), 3) AS[SteamConsumptionPerTonne]


				FROM

			  --  Gamma.GammaNew.dbo.CycleSpoolProduction 
			[dbo].[STG_Cache_CycleSpoolProduction_byDay] b
			--Добавляем правый доин, потому что цикла может и не быть
			  left  JOIN(
				  SELECT a.Value AS Amount, a.GasValue, a.SteamValue, a.PlaceID, a.Date

				  FROM PowerDB.dbo.[ElectricEnergyPlacesHourly] a where " +
	  StaticMethods.Filter_by_time(start_Date, end_Date, "a.Date")
		+ $@") a ON b.PlaceID = a.PlaceID AND a.Date BETWEEN b.CycleDateBegin AND b.CycleDateEnd 
				WHERE b.PlaceID IN({placeId}) --AND  b.[Weight] / 1000 > 20

				GROUP BY dbo.TruncTime(a.Date), b.Place,b.Composition,
			b.CycleDateBegin,
			b.CycleDateEnd,   Weight
				HAVING SUM(a.Amount) / 1000 > 0 

			   -- ORDER BY MEASURE_Date";
					break;
				case "EnergyConsumptionByDevicesByDay":
					sql_query_string = $@"SELECT  ROW_NUMBER() OVER(ORDER BY "
						+ (isByDay ? "dbo.TruncTime(f.RECORD_TIME)" : "dateadd(HOUR,datediff(HOUR,0,f.RECORD_TIME),0)")
						+ @" ASC) AS 'ID',

			  round(SUM(d.MEASURE_VALUE ),3) AS EnergyConsumption,"
				+ (isByDay ? "dbo.TruncTime(f.RECORD_TIME)" : "dateadd(HOUR,datediff(HOUR,0,f.RECORD_TIME),0)")
			   + @" AS Measure_Date, a.DEVICE_NAME as Device_Name
		FROM PA_DEVICES a
		JOIN PA_ADAPTERS b ON b.ID_DEVICE = a.DEVICE_ID AND b.ADAPTER_LOGICAL_ID = 0
		JOIN PA_ADAPTER_PARAMETERS c ON c.ID_ADAPTER = b.ID_ADAPTER AND a.DEVICE_ID = c.ID_DEVICE
		   AND (
		   (a.DEVICE_TYPE_NAME = 'Elster A1800 (Электросчетчик)' AND c.PARAMETER_NAME = 'Канал 1')
		   OR ((a.DEVICE_TYPE_NAME = 'СЭТ-4TM.03 (Электросчетчик)'  OR a.DEVICE_TYPE_NAME = 'Меркурий 230ART (Электросчетчик)') AND c.PARAMETER_NAME = 'A+ (Энергия активная +)'))
   
		JOIN PA_RECORDS f ON f.ID_ADAPTER = b.ID_ADAPTER AND f.ID_DEVICE = b.ID_DEVICE
		JOIN PA_DATA d ON d.ID_RECORD = f.ID_RECORD AND d.ID_PARAMETER = c.ID_PARAMETER
		--JOIN DevicePlaces g ON g.Device_ID = a.DEVICE_ID
		where 1=1 "
		+ " and " + StaticMethods.Filter_by_time(start_Date, end_Date, "f.RECORD_TIME")
		+ " and " + @"a.DEVICE_TYPE_NAME IN ('Elster A1800 (Электросчетчик)' 
			   , 'СЭТ-4TM.03 (Электросчетчик)'
			   , 'Меркурий 230ART (Электросчетчик)')"
		+ "-- and Multiplier>=0 "
		+ @"
		GROUP BY a.DEVICE_TYPE_NAME, c.PARAMETER_NAME, "
+ (isByDay ? "dbo.TruncTime(f.RECORD_TIME)" : "dateadd(HOUR,datediff(HOUR,0,f.RECORD_TIME),0)")
+ @" , a.DEVICE_NAME--, g.PlaceID
		--having SUM(d.MEASURE_VALUE * g.Multiplier)>0
		 ";

					break;

				//				case "fill_Cache_AverageSpeed":
				//					sql_query_string = $@"declare @Dates table(startPeriod DateTime, endPeriod DateTime)
				//declare @startDate DateTime
				//declare @endDate DateTime
				//declare @currDate DateTime
				//set @startDate='" + start_Date.ToString("yyyyMMdd HH:00:00") + @"'
				//set @endDate='" + end_Date.ToString("yyyyMMdd HH:00:00") + @"'
				//set @currDate=@startDate


				//while @currDate<@endDate
				//begin
				//insert into @Dates values(@currDate,dateadd(HOUR,1,@currDate))
				//set @currDate=dateadd(HOUR,1,@currDate)
				//end

				//delete from STG_Cache_AverageSpeed
				//where startPeriod>=@startDate and endPeriod<=@endDate

				//insert into STG_Cache_AverageSpeed
				//select 1 as placeID, d.startPeriod,d.endPeriod,round(AVG(td.Value),0) from @Dates d
				//join STG_Cache_TagDump td on td.Date between d.startPeriod and d.endPeriod and td.TagID = 29  
				//group by d.startPeriod,d.endPeriod
				//union
				//select 2 as placeID, d.startPeriod,d.endPeriod,round(AVG(td.Value),0) from @Dates d
				//join STG_Cache_TagDump td on td.Date between d.startPeriod and d.endPeriod and td.TagID = 30  
				//group by d.startPeriod,d.endPeriod";
				//					break;

				case "ConsumptionByBDMByHour":
					sql_query_string = $@"declare @Dates table(startPeriod DateTime, endPeriod DateTime)
declare @startDate DateTime
declare @endDate DateTime
declare @currDate DateTime
declare @TagId int
set @TagId = {(placeId == 1 ? 29 : 30)}
set @startDate='{start_Date.ToString("yyyyMMdd 00:00:00")}'
set @endDate='{end_Date.ToString("yyyyMMdd 23:59:59")}'
set @currDate=@startDate
--set @startDate=dateadd(minute,30,@startDate)

while @currDate<@endDate
begin
insert into @Dates values(@currDate,dateadd(HOUR,1,@currDate))
set @currDate=dateadd(HOUR,1,@currDate)
end
--select * from @Dates


{SqlCmdStrings.csp_by_day}
	--добавляем к начальной дате 5 часов т.к. после корректировки циклов, край может выступать за начальную дату
	and {StaticMethods.Filter_by_time_byPeriod(start_Date, end_Date, "csp.CycleDateBegin", "csp.CycleDateEnd")} --проверяем попадение фильтра по периоду в циклы производства
	and csp.PlaceID = {placeId}
{SqlCmdStrings.avg_speed_over_csp_by_hours}
--select * from @csp

declare @sku_list table (startPeriod Datetime, endPeriod Datetime, Place varchar(max),  Composition varchar(max))
insert into @sku_list
select distinct d.startPeriod,d.endPeriod,csp.Place, csp.Composition from
@Dates d
left join @csp  csp on 
	
	d.startPeriod between csp.CycleDateBegin and csp.CycleDateEnd
	or d.endPeriod between csp.CycleDateBegin and csp.CycleDateEnd

select
row_number() over (order by d.startPeriod asc) as ID,
	   d.startPeriod as StartPeriod,
	  d.endPeriod as EndPeriod,
	  sku_list.Place,
	  sku_list.Composition,
	  ROUND(SUM(a.Amount)/1000, 2) AS EnergyConsumption, 
	  ROUND(SUM(a.GasValue)/1000, 3) AS GasConsumption,
	  SUM(a.SteamValue) AS SteamConsumption,
	  avgS.AverageSpeed
	
	 	  
	FROM
	@Dates d
	 
	left JOIN ( 
	  SELECT a.Value AS Amount, a.GasValue, a.SteamValue, a.PlaceID, a.Date
	  FROM [ElectricEnergyPlacesHourly] a
	  WHERE a.Date BETWEEN @startDate and @endDate
	  and a.PlaceID = {placeId}
	  ) a ON a.Date >= d.startPeriod and a.Date< d.endPeriod
	left join @sku_list sku_list
	on sku_list.startPeriod = d.startPeriod and sku_list.endPeriod = d.endPeriod
	left join  @avgSpeed avgS on 
		 avgS.startPeriod = d.startPeriod and avgS.endPeriod = d.endPeriod 
	GROUP BY a.PlaceID, sku_list.Place , d.startPeriod,d.endPeriod, sku_list.Composition,avgS.AverageSpeed
	HAVING SUM(a.Amount)/1000 > 0";
					break;

				case "csp_shifts_add_fluid_stream_and_wetness"://задача разбить информацию по житкому потоку на промежутки по diff минут,
															   //в каждом промежутке посчитать произведение суммы массы на среднюю концентрацию= жидкий поток,
															   //затем для каждого цикла(смены) посчитать сумму житкого потока входящий в смену 
					sql_query_string = $@"
										SET NOCOUNT ON
										declare @startDate DateTime
										declare @endDate DateTime										
										declare @diff int 
										set @diff = 5 -- minute lenth of a period
										set @startDate='{start_Date.ToString("yyyyMMdd HH:mm:ss")}'
										set @endDate= '{end_Date.ToString("yyyyMMdd HH:mm:ss")}'
										"
										+ SqlCmdStrings.csp_by_shifts
										//условия
										+ @" and @startDate<=csp.CycleDateBegin and csp.CycleDateEnd<=@endDate
											order by PlaceID, CycleDateBegin"
										+ SqlCmdStrings.csp_shifts_fluid_stream_and_wetness;
					break;
				default:
					return "";

					//break;
			}
			#endregion
			return sql_query_string;
		}


		static public string TER_avg_sql_string(bool isTotal, DateTime start_Date, DateTime end_Date, ScaffoldContext context = null, int tesYear = 0)
		{



			return SqlCmdStrings.TER_avg_permonth_part1
			//Если итого, то заменяем месяц на 13 и убераем параметр месяц из группировки
			+ (isTotal ? "13" : "mc.month")
			+ SqlCmdStrings.TER_avg_permonth_part2
			//ограничиваем [ElectricEnergyPlacesHourly]
			+ " and "
			//если подсчитываем итого берем с начала года, если не итого то за указаный период,
			//можно брать с начала года в обоих случаях, это просто ускоряет посчет

			+ StaticMethods.Filter_by_time_with_time(new DateTime(tesYear == 0 ? DateTime.Now.Year : tesYear, 1, 1).AddHours(-4)
			//делаем отсуп за месяц потому что циклы могут иногда выступать на в пределах пары часов за края из-за коррекции
			//.AddHours(0)
			, new DateTime(tesYear == 0 ? DateTime.Now.Year : tesYear, 1, 1).AddYears(1), "a.Date")
			+ SqlCmdStrings.TER_avg_permonth_part3
			+ (isTotal ? "" : ",mc.month")
			+ SqlCmdStrings.TER_avg_permonth_part4;
		}






		static string Generate_SKU_list(DateTime start_Date, DateTime end_Date, ScaffoldContext context)
		{
			List<string> SKUs_to_show = context.StgCacheCycleSpoolProductionByMonth
									//будем показывать только те SKU которые появлялись в циклах в пределех дат для отчета
									.Where(r =>
									//конец цикла выпадает на заданный период
									(r.CycleDateEnd >= start_Date && r.CycleDateEnd <= end_Date)
									//весь заданный период попадает в  цикл 
									|| (start_Date >= r.CycleDateBegin && end_Date <= r.CycleDateEnd)
									//или начало цикла попадает в заданный период
									|| (r.CycleDateBegin >= start_Date && r.CycleDateBegin <= end_Date)
									)
									.Select(r => r.Composition).Distinct().ToList();
			//для ограничения SQl по SKU нужна строка вроде ('1',..,'n')

			string SKU_list = "(";
			if (SKUs_to_show.Count > 0)
			{
				foreach (var item in SKUs_to_show)
				{
					SKU_list += "'" + item + "'" + ",";
				}
				SKU_list = SKU_list.Remove(SKU_list.Length - 1);
			}
			//если записей нет, берем первую из словаря
			else
			{
				SKU_list += "'" + context.StgSkuDictionary.First().SkuName + "'";
			}

			return SKU_list = SKU_list + ")";
		}
		static public void AddNewTimer(TimerCallback WrapTimer, Timer_data timer_data, List<Timer> timers, ScaffoldContext dbContext)
		{
			//timer_data.StartDate = StaticMethods.CorrectionStartDay(timer_data.StartDate);
			timers.Add(new Timer(WrapTimer, timer_data
									//запуск в 7 часов 20 минут следующего дня, так что считаем задержку
									//, 0
									, timer_data.StartDate - DateTime.Now

									//Период каждые 24 часа
									, timer_data.Period
							));
			dbContext.StgBackgroundTasklogs.Add(
				new StgBackgroundTasklogs
				{
					LogDateTime = DateTime.Now
				,
					TypeId = 1
				,
					TimerName = timer_data.TimerName
				,
					Message = $"Task start date:{timer_data.StartDate}, period: {timer_data.Period.TotalMinutes} min."
				});
			try { dbContext.SaveChanges(); } catch (Exception e) { CustomExceptionHandler(e); }
		}
		//static public void AddNewTimer(TimerCallback method, DateTime startDate, TimeSpan period, string timerName, List<Timer> timers, ScaffoldContext dbContext)
		//{
		//	timers.Add(new Timer(method, null
		//							//запуск в 7 часов 20 минут следующего дня, так что считаем задержку
		//							//, 0
		//							, (startDate - DateTime.Now)

		//							//Период каждые 24 часа
		//							, period
		//					));
		//	dbContext.StgBackgroundTasklogs.Add(
		//		new StgBackgroundTasklogs
		//		{
		//			LogDateTime = DateTime.Now
		//		,
		//			TypeId = 1
		//		,
		//			TimerName = timerName
		//		, Message = $"Task start date:{startDate}, period: {period.TotalMinutes} min."
		//		});
		//	try	{dbContext.SaveChanges();} catch (Exception e){	}	
		//}


		static public DateTime CorrectionStartDay(DateTime datetime, TimeSpan cacheTasksPeriod)
		{
			while (datetime < DateTime.Now)
			{
				datetime = datetime.Add(cacheTasksPeriod);
			}
			return datetime;
		}
		static public List<string> Get_email_receivers(ScaffoldContext dbContext, string report_name)
		{
			var report_id = dbContext.StgEmailReportsNames.Where(r => r.ReportNameEng == report_name).Select(r => r.Id).FirstOrDefault();
			var recievers = dbContext.StgEmailReportsReceivers.Where(r => r.ReportNotificationNameId == report_id).Select(r => r.EmailName).ToList<string>();
			return recievers;
		}

		static public void SendEmailReport(ref List<DateTime> DateTimeSentMessages, List<string> receivers, string email_subject, string email_body, byte[] attachment_data = null, bool testing = false)
		{
			if (testing)
			{
				receivers = new List<string> { "thst@mail.ru" };
			}
			//Если мы еще не посылали сообщение в это время
			if (!DateTimeSentMessages.Contains(DateTime.Now))
			{

				foreach (var receiver in receivers)
				{

					Attachment attachment=null;
					if (email_subject == "Средневзвешенное ТЭР на тонну по циклам производства" && attachment_data != null)
					{
						attachment =(new Attachment(new MemoryStream(attachment_data), $"Средневзвешенное ТЭР на тонну по циклам производства_{DateTime.Now.ToShortDateString()}.xlsx"));
					}

					SendMail(receiver, email_subject, email_body, attachment, testing);

				}
				DateTimeSentMessages.Add(DateTime.Now);
			}

		}
		public static void SendMail(string receiver, string email_subject, string email_body, Attachment attachment=null, bool testing = false)
		{
			// настройки smtp-сервера, с которого мы и будем отправлять письмо

			
			SmtpClient smtp = new System.Net.Mail.SmtpClient("192.168.0.220", 25);
			MailAddress from = new MailAddress("service_mailer@sgbi.ru", "Report");
			smtp.EnableSsl = false;
			smtp.Credentials = new System.Net.NetworkCredential("service_mailer@sgbi.ru", "41%35b1Ga3");
			if (testing)// если ручное тестирование то переделываем параметры
			{
				var mailpwd = GetPwdFromConsole();
				smtp = new System.Net.Mail.SmtpClient("smtp.mail.ru", 587);
				from = new MailAddress("thst@mail.ru", "Test");
				smtp.EnableSsl = true;
				smtp.Credentials = new System.Net.NetworkCredential("thst@mail.ru", mailpwd);
				mailpwd.Dispose();
			}
			// наш email с заголовком письма


			// кому отправляем
			MailAddress to = new MailAddress(receiver);
			// создаем объект сообщения
			MailMessage m = new MailMessage(from, to);
			// тема письма
			m.Subject = email_subject;
			// текст письма
			m.Body = email_body;
			if (attachment != null)
			{
				m.Attachments.Add(attachment);
			}

			if (email_body.Contains("<html>") || email_body.Contains("<tr") || email_body.Contains("<table"))
				m.IsBodyHtml = true;
			try
			{
				smtp.Send(m);
			}
			catch (Exception e)
			{
				SmtpExceptionHandler(e);
				//throw;
			}
		}

		//формируем записи для отчетов
		static public List<T> RecordsToShow<T>(string reportName, DateTime start_Date, DateTime end_Date, ScaffoldContext context, int placeId = 0, bool isByDay = false, Dictionary<string, string> PerfTimes = null, Controller controller = null, int tesYear = 0, string sku = null, bool testing = false)
			where T : class
		{
			var startTime = DateTime.Now;
			string sql_cmd = Generate_sql_string(reportName, start_Date, end_Date, context, placeId, isByDay, tesYear: tesYear, sku: sku);
			var result = context.Set<T>().FromSql(
										// генерируем sql строку для вывода данных для отчета
										sql_cmd
										
										).ToList()
										;
			//Если нужно выводить производительность
			if (PerfTimes != null)
			{
				PerfTimes.Add(reportName, StaticMethods.PerfTime(startTime));
			}

			return result;

		}

		public static string FormPeriodString(DateTime t, DateTime endDate)
		{

			string period_string = "";
			while (t.Month <= endDate.Month)
			{
				period_string += t.ToString("MMMM") + ",";
				if (t.Month == 12) break;
				t = t.AddMonths(1);
			}
			//отрезаем последнюю запятую
			return period_string.Substring(0, period_string.Length - 1);
		}
		public static List<object> RoundList(List<object> items, int num)

		{
			if (items!=null&&items.Count>0)
			{

			
			var myFieldInfo = items.FirstOrDefault().GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

			foreach (var item in items)
			{
				for (int i = 0; i < myFieldInfo.Length; i++)
				{

					if (myFieldInfo[i].FieldType.ToString() == "System.Double")
					{
						myFieldInfo[i].SetValue(item, Math.Round((double)myFieldInfo[i].GetValue(item), num));
					}

				}

			}
			}
			return items;
		}
		public static string sqlString_by_searchOptions(int searchOption, string start_Date, string end_Date, int device_id, ScaffoldContext context, Controller curr_controller = null)
		{

			string sql_string = "";
			string where_device_string = "";
			if (device_id != 0)
			{
				where_device_string = @"
							and f.ID_DEVICE =" + device_id.ToString();
			};
			switch (searchOption)
			{
				//Поиск по критерию
				case 1:


					sql_string = SqlCmdStrings.ShowRecordsByStatusByCriteria_part1
					   + start_Date
					   + SqlCmdStrings.ShowRecordsByStatusByCriteria_part2
					   + end_Date
					   + SqlCmdStrings.ShowRecordsByStatusByCriteria_part3
					   + SqlCmdStrings.Creteria_filter
					   + SqlCmdStrings.ShowRecordsByStatusByCriteria_part4
					   + where_device_string;
					break;
				//поиск отсутсвующих записей
				case 2:
					string sql_part3 = SqlCmdStrings.ShowRecordsByStatus_absent_rec_part_device_all;
					if (device_id > 0)
					{
						string device_type = context.PaDevices.Find(device_id).DeviceTypeName;

						if (StaticMethods.DeviceTypesListwithHourlyIndication.Contains(device_type))
						{

							sql_part3 = SqlCmdStrings.ShowRecordsByStatus_absent_rec_part_device_selected_type1_1
									+ @" and  DEVICE_ID =" + device_id.ToString()
									+ SqlCmdStrings.ShowRecordsByStatus_absent_rec_part_device_selected_type1_2;

						};
						if (StaticMethods.DeviceTypesListwithHalfHourlyIndication.Contains(device_type))
						{

							sql_part3 = SqlCmdStrings.ShowRecordsByStatus_absent_rec_part_device_selected_type2_1
									+ @" and  DEVICE_ID =" + device_id.ToString()
							+ SqlCmdStrings.ShowRecordsByStatus_absent_rec_part_device_selected_type2_2;
						};

					};

					sql_string = SqlCmdStrings.ShowRecordsByStatus_absent_rec_part1
						+ start_Date
						+ SqlCmdStrings.ShowRecordsByStatus_absent_rec_part2
						+ end_Date
						+ sql_part3
						+ SqlCmdStrings.ShowRecordsByStatus_absent_rec_part4;
					if (curr_controller != null)
					{
						//Обозначаем что хотим добавлять отсутсвующие записи
						curr_controller.ViewBag.Current_action = "AddGroupRecordsByStatus";
					}

					break;
				//Показать все существующие записи за период
				case 3:
					sql_string = SqlCmdStrings.ShowRecordsByStatus_all_recs_part1
						+ start_Date
						+ SqlCmdStrings.ShowRecordsByStatus_all_recs_part2
						+ end_Date
						+ SqlCmdStrings.ShowRecordsByStatus_all_recs_part3
						+ where_device_string;
					break;

				default:
					break;
			}
			return sql_string;
		}

		//public static void Add_liquid_and_wetness_data(IServiceScopeFactory scopeFactory, DateTime start_Date, DateTime end_Date)
		//{

		//	DateTime currDate = start_Date.Date.AddHours(first_shift_start_hour);
		//	end_Date = end_Date.AddHours(first_shift_start_hour);
		//	MultiThread_execSqlAsync(scopeFactory, start_Date, end_Date, "csp_shifts_add_fluid_stream_and_wetness", TimeSpan.FromHours(shift_duration));
			

		//}

		//public static async Task<int> Fill_cache_csp_byMonth(IServiceScope scope, DateTime startDate, DateTime endDate)
		//{
		//	//Нам нужно чтобы начало периода обновления попадало на 1 число месяца, а конец на поледний день месяца, чтобы алгоритм поделил циклы среди месяцев
		//	var tl = new List<Task<int>>();
		//	int result = 0;
		//	startDate = StaticMethods.GetStartofMonth(startDate);
		//	endDate = StaticMethods.GetEndOfMonth(endDate);
		//	var currDate = startDate;

		//	while (currDate < endDate)
		//		{

		//			//обновляем кэш
		//			tl.Add(scope.ServiceProvider.GetRequiredService<ScaffoldContext>().Database.ExecuteSqlCommandAsync(Generate_sql_string("Fill_Cache_CycleSpoolProduction_byMonth"
		//		   , currDate
		//		   , currDate.AddMonths(1).AddSeconds(-1)))
		//				);


		//			currDate = currDate.AddMonths(1);

		//		}

		//	//корректируем его
		//	try {
		//		while (tl.Any())
		//		{
		//			var task = await Task.WhenAny(tl);
		//			tl.Remove(task);
		//			result += await task;
		//		}
		//	}
		//	catch (Exception e)
		//	{
		//		CustomExceptionHandler(e);
		//	}
		//	return result;
		//}

		//public static async Task<int> Fill_csp_byDay(ScaffoldContext dbContext, DateTime startDate, DateTime endDate)
		//{

		//	return await dbContext.Database.ExecuteSqlCommandAsync(StaticMethods.Generate_sql_string("Fill_Cache_CycleSpoolProduction_byDay"
		//			, startDate
		//			, endDate));






		//}

		//public static void Fill_csp_byShifts_all_stages(IServiceScopeFactory scopeFactory, DateTime startDate, DateTime endDate)
		//{

		//	List<Task> tl = new List<Task>();

		//	//разбиваем на более мелкие периоды чтобы быстрее соборать кэш
		//	DateTime currDate = startDate;
		//	TimeSpan diffPeriod = TimeSpan.FromDays(14);
		//	while (currDate < endDate)
		//	{

		//		tl.Add(Fill_csp_byShifts(scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>(), currDate, endDate)
		//					.ContinueWith(async (t) => await CycleSpoolProduction_Correction_Cycle_ByShifts<StgCacheCycleSpoolProductionByShifts>(scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>(),
		//																																			 currDate, endDate, placeId: 1))
		//					.ContinueWith(async (t) => await CycleSpoolProduction_Correction_Cycle_ByShifts<StgCacheCycleSpoolProductionByShifts>(scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>(),
		//																																			 currDate, endDate, placeId: 2))
		//					.ContinueWith((t) => Add_liquid_and_wetness_data(scopeFactory,
		//																				 currDate, endDate)));
		//		currDate.Add(diffPeriod);
		//	}
		//	try
		//	{
		//		Task.WaitAll(tl.ToArray());
		//	}
		//	catch (Exception e)
		//	{
		//		CustomExceptionHandler(e);
				
		//	}

		//}

		//public static async Task<int> Fill_csp_byShifts(ScaffoldContext dbContext, DateTime startDate, DateTime endDate)
		//{
		//	//Task result;
		//	startDate = startDate.Date;
		//	endDate = endDate.Date.AddDays(1);



		//	//Обновляем кэш
		//	return await dbContext.Database.ExecuteSqlCommandAsync(StaticMethods.Generate_sql_string("Fill_Cache_CycleSpoolProduction_byShifts"
		//	, startDate
		//	, endDate));
		//	//.ContinueWith(async (t) => await CycleSpoolProduction_Correction_Cycle_ByShifts<StgCacheCycleSpoolProductionByShifts>(dbContext, startDate, endDate, placeId: 1))				
		//	//.ContinueWith(async (t) => await CycleSpoolProduction_Correction_Cycle_ByShifts<StgCacheCycleSpoolProductionByShifts>(dbContext, startDate, endDate, placeId: 2))
		//	//.ContinueWith(async (t) => await Add_liquid_and_wetness_data(dbContext, startDate, endDate));







		//}
		//public static async Task Fill_Cache_AverageSpeed(ScaffoldContext dbContext, DateTime startDate, DateTime endDate) =>


		//	await dbContext.Database.ExecuteSqlCommandAsync(StaticMethods.Generate_sql_string("fill_Cache_AverageSpeed"
		//												, startDate
		//												, endDate
		//												));



		//public static async Task Fill_Cache_TagDump(ScaffoldContext dbContext, DateTime startDate, DateTime endDate) =>


		//	await dbContext.Database.ExecuteSqlCommandAsync(StaticMethods.Generate_sql_string("fill_Cache_TagDump"
		//											   , startDate
		//											   , endDate
		//											   ));
		//public static void Update_avg_speed_Cache_CycleSpoolProduction_byShifts(IServiceScopeFactory scopeFactory)
		public static async Task<int> MultiThread_execSqlAsync(IServiceScopeFactory scopeFactory, DateTime start_Date, DateTime end_Date, string sql_cmd, TimeSpan diffTime)
		{
			int affectedRowCount = 0;
			DateTime currDate = start_Date;
			var tl = new List<Task<int>>();
			var curr_DbContent = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>();
			while (currDate < end_Date)
			{

				//делим на кусочки и каждый кусочик пытаемся обработать в отдельном потоке
				tl.Add(curr_DbContent.Database.ExecuteSqlCommandAsync(StaticMethods.Generate_sql_string(sql_cmd, currDate, currDate.Add(diffTime))));
				currDate = currDate.Add(diffTime);
			}

			try//ловим ошибки исполнения
			{

				while (tl.Any())
				{
					Task<int> finishedTask = await Task.WhenAny(tl);
					tl.Remove(finishedTask);
					affectedRowCount += finishedTask.Result;
				}
				
				
				
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
			}
			return affectedRowCount;
		}




		public static void Save_report_usage(Controller controller, string reportName, ScaffoldContext context)
		{
			try
			{
				if (controller != null && controller.HttpContext != null && controller.HttpContext.Connection.RemoteIpAddress != null)
				{
					context.StgReportsUsage.Add(new StgReportsUsage { ReportName = reportName, AccessDate = DateTime.Now, RemoteIp = controller.HttpContext.Connection.RemoteIpAddress.ToString()});
					context.SaveChanges();
				}

			}
			catch (Exception e) { CustomExceptionHandler(e); }
		}

		//небольшой распоковщик класс, дает более менее надёжную структуру для сравнения класса,
		//можно также использовать для сохранения экземпляра класса для будущего тестирования
		public static string InstanceofClassToString<T>(T instance, string test_instance_name = "test_data")
		{
			string result = "";
			string name_of_class = typeof(T).Name;
			result += $"{name_of_class} {test_instance_name} = new {name_of_class}()\n{{\n\n";
			var fInfo = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
			for (int i = 0; i < fInfo.Length; i++)
			{

				var fieldValue = fInfo[i].GetValue(instance);
				string sFieldValue = "";

				//нам нужен double с точкй, для формирования экземляра класса
				switch (fieldValue.GetType().Name)
				{
					case "Double": sFieldValue = ((double)fieldValue).ToString(CultureInfo.CreateSpecificCulture("en-CA"));
						break;

					case "DateTime":
						sFieldValue = $@"DateTime.Parse(""{((DateTime)fieldValue).ToString("o")}"")";// ((DateTime)fieldValue).ToString(CultureInfo.CreateSpecificCulture("en-CA"));
						break;
					case "String":
						sFieldValue = $@"""{fieldValue}""";
						break;
					default:
						sFieldValue = fieldValue.ToString();
						break;
				}

				result += $"{GetFieldName(fInfo[i])} = {sFieldValue}{(i != fInfo.Length - 1 ? "," : "")}\n";
			}
			return result + "\n};";
		}

		//Пробуем распаковывать IEnumerable
		public static string ExpandEnumerable_forListFieldsValues(object instance)
		{
			string result = "";
			
			foreach (var item in ((IEnumerable)instance))
				{
					if (item != null)
					{
						result += ListFieldsValues(item);
					}

				}			
			return result;
		}

		public static bool CanBeUnPackedListOrArr(Type t) =>
			t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>) || t.IsArray;
		
		//распаковывает класс на строку состоящую из полей класса, 
		//удобно для тестирования и создания строки для дальшейшего тестирования
		// работает только если состоит из классов, строк, примитивов или (листов или массивов выше перечисленного)
		public static string ListFieldsValues<T>(T instance, string[] excludeFieldList =null)
		{
			if (excludeFieldList==null)
			{
				excludeFieldList = new string[] { "ID" };
			}
			string result = "";
			if (CanBeUnPackedListOrArr(typeof(T)))
			{
				result += ExpandEnumerable_forListFieldsValues(instance);
			}
			//пробуем распаковать IEnumerable
			else
			{
				if (!(instance.GetType().Name == "String")&&!instance.GetType().IsPrimitive)
				{
					var fInfo = instance.GetType()
									.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
									.Where(f => excludeFieldList == null || !excludeFieldList.Contains(GetFieldName(f).ToUpper()))
									.ToArray();
					for (int i = 0; i < fInfo.Length; i++)
					{
						var curValue = fInfo[i].GetValue(instance);
						if (curValue  != null)//&&!fInfo[i].GetValue(instance).GetType().IsPrimitive)//Если какой-то список то распаковываем дальше
						{
							if (CanBeUnPackedListOrArr(fInfo[i].FieldType))
							{
								result += ExpandEnumerable_forListFieldsValues(curValue);
							}
							else
							{
								// добрались до значений, добавляем в результат
								result += $"{GetFieldName(fInfo[i])}={curValue}\r\n";
							}
						}
					}
				}
				else
				{
					result += instance.ToString()+ "\r\n";
				}
			}
			return result;
		}

		
		public static string StrByPeriod(string inString)
		{
			if (inString.Contains("byDay")) return "byDay";
			if (inString.Contains("byMonth")) return "byMonth";
			if (inString.Contains("byShifts")) return "byShifts";

			return "";
		}
		public static void Forming_lists_timers(Controller controller,IServiceScopeFactory scopeFactory)
		{
			var backgroundTasks = scopeFactory.CreateScope().ServiceProvider.GetService<IHostedService>();

			var list_timers = new List<string>();
			var list_timers_names = new List<string>();
            foreach (var timer in ((TimedHostedService)backgroundTasks).List_of_timers_data)
			{
                list_timers.Add($"Name = {timer.TimerName}, время начала={timer.StartDate}, период ={timer.Period}");
                list_timers_names.Add(timer.TimerName);
            }
			controller.ViewData["list_timers"] = list_timers;
			controller.ViewData["list_timers_names"] = list_timers_names;
            
		}
		public static void CustomExceptionHandler(Exception e)
		{

			//Посылаем информацию о новом эксепшене			 
			string msg = $"Сообщение:= { e.Message}";	
						
			foreach (var frame in (new StackTrace(e)).GetFrames())
			{
				//добавляем информацию о методе
				if (frame.HasMethod()) {
					var method = frame.GetMethod();
					msg +=
					$@"Вызвавший метод: {method.Name}
					Декларируемый тип метода: {method.DeclaringType.FullName}
						";
				}
			}
			

			
			try
			{
				SendMail(admin_email, "Exception is cought!", msg.Replace("\t",""));
			}
			catch (Exception smtpEx)
			{
				SmtpExceptionHandler(smtpEx);
			}
		}

		public static void SmtpExceptionHandler(Exception e)
		{
			//Заглушка, не можем послать эксепшен, ничего не делаем, других оповещений пока нет
		}
		public static SecureString GetPwdFromConsole()
		{
			// Instantiate the secure string.
			SecureString securePwd = new SecureString();
			ConsoleKeyInfo key;

			Console.Write("Enter password: ");
			do
			{
				key = Console.ReadKey(true);

				// Ignore any key out of range.
				if (((int)key.Key) >= 65 && ((int)key.Key <= 90))
				{
					// Append the character to the password.
					securePwd.AppendChar(key.KeyChar);
					Console.Write("*");
				}
				// Exit if Enter key is pressed.
			} while (key.Key != ConsoleKey.Enter);
			Console.WriteLine();
			Console.WriteLine("Password excepted!");
			return securePwd;
		}

		//public static Task Fill_cache_csp_byMonth_full(IServiceScopeFactory scopeFactory, DateTime startDate_byMonth, DateTime endDate_byMonth, TimeSpan tSpanDiff) =>
		//	Task.Run(
		//				async () =>
		//				{
		//					Task<int> curTask;
		//					var curPreviousTimeStart = DateTime.Now;
		//					var rowAffected = 0;
		//					rowAffected = await Fill_cache_csp_byMonth(scopeFactory.CreateScope(), startDate_byMonth, endDate_byMonth);
							
		//					if (rowAffected > 0)
		//					{

		//						//с учтётом того что задача выполняется на стороне sql server, то приходится вводить допролнительную проверку для запуска следуюющей задачи
		//						CheckerSqlTableAvail checker = new CheckerSqlTableAvail("STG_Cache_CycleSpoolProduction_byMonth", scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>());
		//						//Пробывал упаковать задачи в какую то коллекцию для того чтобы в цикле применять чекер, но задачи тогда начинают выполняться паралльно, а нам нужно последовательно

		//						checker.WaitChanges(ref curPreviousTimeStart, rowAffected);//Если предыдущий запрос ничего неменял, то проверять нечего, иначе ждем изменений
		//						curTask = CycleSpoolProduction_Correction_Cycle<StgCacheCycleSpoolProductionByMonth>(scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>(), startDate_byMonth, endDate_byMonth,1);
		//						curTask.Wait();
		//						rowAffected = curTask.Result;
		//						checker.WaitChanges(ref curPreviousTimeStart, rowAffected);//Если предыдущий запрос ничего неменял, то проверять нечего, иначе ждем изменений
		//						curTask = MultiThread_execSqlAsync(scopeFactory, startDate_byMonth, endDate_byMonth, "Update_avg_speed_Cache_CycleSpoolProduction_byMonth", tSpanDiff);
		//						curTask.Wait();
		//						rowAffected = curTask.Result;
		//					}
		//				});

		
		//public static Task Fill_csp_byDay_full(IServiceScopeFactory scopeFactory, DateTime startDate, DateTime endDate, TimeSpan tSpanDiff) =>
		//	Task.Run(
		//				async () => 
		//				{
		//					var db = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>();
		//					var curPreviousTimeStart = DateTime.Now;
		//					int rowAffected = await Fill_csp_byDay(db, startDate, endDate).ConfigureAwait(false);
		//					if (rowAffected > 0)
		//					{

								
		//						//с учтётом того что задача выполняется на стороне sql server, то приходится вводить допролнительную проверку для запуска следуюющей задачи
		//						CheckerSqlTableAvail checker = new CheckerSqlTableAvail("STG_Cache_CycleSpoolProduction_byDay", scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>());
		//						//Пробывал упаковать задачи в какую то коллекцию для того чтобы в цикле применять чекер, но задачи тогда начинают выполняться паралльно, а нам нужно последовательно
								
		//						checker.WaitChanges(ref curPreviousTimeStart, rowAffected);//Если предыдущий запрос ничего неменял, то проверять нечего, иначе ждем изменений
		//						rowAffected = await CycleSpoolProduction_Correction_Cycle<StgCacheCycleSpoolProductionByDay>(db, startDate, endDate,1).ConfigureAwait(false);
								
		//						checker.WaitChanges(ref curPreviousTimeStart, rowAffected);//Если предыдущий запрос ничего неменял, то проверять нечего, иначе ждем изменений
		//						rowAffected = await MultiThread_execSqlAsync(scopeFactory, startDate, endDate, "Update_avg_speed_Cache_CycleSpoolProduction_byDay", tSpanDiff).ConfigureAwait(false);
		//					}
								
		//				});
		//public static Task Fill_csp_byShifts_full(IServiceScopeFactory scopeFactory, DateTime startDate, DateTime endDate, TimeSpan tSpanDiff) =>
		//	Task.Run(
		//				async () =>
		//				{
		//					await Fill_csp_byShifts(scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>(), startDate, endDate);
		//					await CycleSpoolProduction_Correction_Cycle_ByShifts<StgCacheCycleSpoolProductionByShifts>(scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>(), startDate, endDate, placeId: 1);
		//					await CycleSpoolProduction_Correction_Cycle_ByShifts<StgCacheCycleSpoolProductionByShifts>(scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>(), startDate, endDate, placeId: 2);
		//					Add_liquid_and_wetness_data(scopeFactory, startDate, endDate);
		//					await MultiThread_execSqlAsync(scopeFactory, startDate, endDate, "Update_avg_speed_Cache_CycleSpoolProduction_byShifts", tSpanDiff);
		//				});
		public static bool Test_linkedServer(ScaffoldContext dbContext, string serverName)
		{
			try
			{
				dbContext.Database.ExecuteSqlCommand($"sp_testlinkedserver {serverName}");
				return true;
			}
			catch
			{
				SendMail(admin_email, "error", $"Не доступен сервер {serverName}"); 
				return false;
				
			}


		}
	}

}
