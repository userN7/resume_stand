using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STG_counters_admin_interface.Models.PowerDB_data_classes;
using Microsoft.AspNetCore.Mvc;
using STG_counters_admin_interface.Models;
using Newtonsoft.Json;
using STG_counters_admin_interface.Models.Pages_control;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using STG_counters_admin_interface.Controllers;
using static STG_counters_admin_interface.Models.StaticMethods;
using static STG_counters_admin_interface.Models.Constants;
using System.Diagnostics;
using System.Threading;

using System.Collections;
using Microsoft.Extensions.Hosting;
using System.Security;

using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;

namespace STG_counters_admin_interface.Models
{
	public class Fill_Cache
	{
		private readonly IServiceScopeFactory scopeFactory;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		//Для упрощения берем две смены, получется с сутки со сдвигом
		public DateTime ShiftsStartDate { get=>StartDate.Date.AddHours(first_shift_start_hour);}
		public DateTime ShiftsEndDate { get=>EndDate.Date.AddHours(first_shift_start_hour); }
		
		public DateTime MonthStartDate { get => GetStartofMonth(StartDate); }
		public DateTime MonthEndDate { get => GetEndOfMonth(EndDate); }
		public TimeSpan PeriodToCutSql { get; set; } = TimeSpan.FromDays(3);//переменная отвечающая на какие переиоды будут резатся таблица для обновления ср. скорости, для распредеелния между потоками
		
		delegate int FillingCache();
		delegate int CacheCorrection<T>(int PlaceId);
		
		private volatile ScaffoldContext dbContext;
		Dictionary<Type, (DateTime curStartDate, DateTime curEndDate)> curDates; 

		// проверяем доступность сервера кэшей 
		public bool Is_AO01_available () => Test_linkedServer(dbContext, "AO01");		
		public bool Is_Gamma_available() => Test_linkedServer(dbContext, "Gamma");
		public Fill_Cache(IServiceScopeFactory scopeFactory, DateTime startDate, DateTime endDate)
		{
			this.scopeFactory = scopeFactory;
			this.StartDate = startDate;
			this.EndDate = endDate;
			this.dbContext = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ScaffoldContext>();
			this.curDates = new Dictionary<Type, (DateTime curStartDate, DateTime curEndDate)>()
			{
				[typeof(StgCacheCycleSpoolProductionByMonth)] = (MonthStartDate, MonthEndDate),
				[typeof(StgCacheCycleSpoolProductionByDay)] = (StartDate, EndDate),
				[typeof(StgCacheCycleSpoolProductionByShifts)] = (ShiftsStartDate, ShiftsEndDate)
			}; ;
		}

		
		public async Task<int> Fill_Cache_Full()
		{
			var result = 0;
			var tasks = new List<Task<int>>();
			

			if (this.Is_AO01_available())//если тэги доступны
				try
				{//перед тем как продолжать, нужно заполнить кеши для tagDump 
					 this.Fill_Cache_TagDump();
				}
				catch (Exception e)
				{
					CustomExceptionHandler(e);

				}

			if (this.Is_Gamma_available())
			{
				//так как выгрузка идёт в разные таблицы, мы можем разбить на разные потоки, и пыполнять паралельно
				tasks.Add((new Fill_Cache(scopeFactory, StartDate, EndDate)).Fill_cache_csp_full<StgCacheCycleSpoolProductionByDay>());
				tasks.Add((new Fill_Cache(scopeFactory, StartDate, EndDate)).Fill_cache_csp_full<StgCacheCycleSpoolProductionByShifts>());
				tasks.Add((new Fill_Cache(scopeFactory, StartDate, EndDate)).Fill_cache_csp_full<StgCacheCycleSpoolProductionByMonth>());
			}
			try//ждем все задачи, чтобы отловить исключения
			{
				while (tasks.Any())
				{					
					var task = await Task.WhenAny(tasks.ToArray());
					tasks.Remove(task);
					result += await task;
					//results.Add(result.taskName, result.rowAffected);					
				}
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
			}
			return result;
		}
		public int Fill_Cache_TagDump()	=>
		dbContext.Database.ExecuteSqlCommand(StaticMethods.Generate_sql_string("fill_Cache_TagDump"
													  , StartDate
													  , EndDate
													  ));		
		public int Fill_cache_csp_byMonth()
		{
			//Важно выгружать циклы чтобы они делились внутри месяца
			int result = 0;			
			////Если конец лежит в заданом промежутке, то делим его			
			var curStartDate = MonthStartDate;
			var curEndDate = MonthEndDate;
			//делим на месяцы
			while (curStartDate < curEndDate)
			{

				//обновляем кэш
				result+=dbContext.Database.ExecuteSqlCommand(Generate_sql_string("Fill_Cache_CycleSpoolProduction_byMonth"
					,curStartDate
					,curStartDate.AddMonths(1).AddSeconds(-1)
							)
					);
				curStartDate = curStartDate.AddMonths(1);

			}
			return result;
		}		
		public int Fill_cache_csp_byDay()=>
			dbContext.Database.ExecuteSqlCommand(StaticMethods.Generate_sql_string("Fill_Cache_CycleSpoolProduction_byDay"
					, StartDate
					, EndDate));
		public int Fill_cache_csp_byShifts()
		{
			
			//Обновляем кэш
			return  dbContext.Database.ExecuteSqlCommand(StaticMethods.Generate_sql_string("Fill_Cache_CycleSpoolProduction_byShifts"
			, ShiftsStartDate
			, ShiftsEndDate));
			}
		public async Task<int> Fill_cache_csp_full<T>()

		{
			#region init_var
			//Исключения ловятся выше, так что не проверяем доступность
			var fillingCacheDict = new Dictionary<Type, FillingCache>()
			{
				[typeof(StgCacheCycleSpoolProductionByMonth)] = Fill_cache_csp_byMonth,
				[typeof(StgCacheCycleSpoolProductionByDay)] = Fill_cache_csp_byDay,
				[typeof(StgCacheCycleSpoolProductionByShifts)] = Fill_cache_csp_byShifts
			};
			

			var cacheCorrectionDict = new Dictionary<Type, CacheCorrection<T>>()
			{
				[typeof(StgCacheCycleSpoolProductionByMonth)] = CycleSpoolProduction_Correction_Cycle<StgCacheCycleSpoolProductionByMonth>,
				[typeof(StgCacheCycleSpoolProductionByDay)] = CycleSpoolProduction_Correction_Cycle<StgCacheCycleSpoolProductionByDay>,
				[typeof(StgCacheCycleSpoolProductionByShifts)] = CycleSpoolProduction_Correction_Cycle_ByShifts<StgCacheCycleSpoolProductionByShifts>
			};

			

			FillingCache fillingCache = fillingCacheDict[typeof(T)];

			CacheCorrection<T> cacheCorrection = cacheCorrectionDict[typeof(T)];
			var (curStartDate, curEndDate) = curDates[typeof(T)];
			string sqlUpdateAvgSpeedStr = Constants.sqlUpdateAvgSpeedStr[typeof(T)];


			
			#endregion
			return await Task.Run(
						async () =>
						{
							var rowAffected = fillingCache();
							if (rowAffected > 0)//если кэш изменился, его нужно обработать
							{
								foreach (var placeId in Constants.placeIdsForCorrection)
								{
									rowAffected += cacheCorrection(placeId);
								}

								rowAffected += await MultiThread_execSqlAsync(scopeFactory, curStartDate, curEndDate, sqlUpdateAvgSpeedStr, PeriodToCutSql);

								//Для сменн дополнительно обновляем данные
								if (typeof(T) == typeof(StgCacheCycleSpoolProductionByShifts))
								{
									rowAffected += await Add_liquid_and_wetness_data();
								}
							}
							return rowAffected;
						});
		}

		public int CycleSpoolProduction_Correction_Cycle<T>(int placeId)
			where T : class, IStgCacheCycleSpoolProduction, new()
		{
			var minSpanBetweanCycles = TimeSpan.FromSeconds(1);
			var SpanForSearchEnclosingRec = TimeSpan.FromDays(30);
			//var tl = new List<Task<int>>();
			//Для коррекции не важно как распределились периоды для деления(день и месяц), а важны только концы заданного периода
			var (start_Date, end_Date) = curDates[typeof(T)];			
			//Берём параметр из базы, который отвечает за время между циклами.
			//Если время больше пораметра то добавляется новый цикл с наименованием прочее, иначе циклы сшиваются
			int t = int.Parse(dbContext.StgConfigParams
										.Where(r => r.ParamName == "Delta_cycles")
										.FirstOrDefault()
										.ParamVal);
			var recsToAdd = new List<T>();



			List<T> records = dbContext.Set<T>()
					.Where(
				r => r.PlaceId == placeId
				&& r.Composition != "Прочее"
				//Берем записи которые пересекаются периодом для работы
				&& StaticMethods.FirstPeriodContainsPartOfSecondPeriod(start_Date, end_Date, r.CycleDateBegin, r.CycleDateEnd)
				).OrderBy(r => r.CycleDateBegin)
				.ToList();
			if (records.Count == 0)
			{
				return default;
			}
			//удалим все добавленные ранее "Прочее" попадающие в период чтобы добавить их заново, если они изменились
			dbContext.RemoveRange(
		dbContext.Set<T>().Where(r =>
		StaticMethods.FirstPeriodContainsPartOfSecondPeriod(records.First().CycleDateBegin ?? default, records.Last().CycleDateEnd ?? default, r.CycleDateBegin, r.CycleDateEnd)
		&& r.PlaceId == placeId
		&& r.Composition == "Прочее"));

			#region Замыкание с двух сторон записями
			//На нужно по одной зажимающей записи пред и после нашей выборки, так что пробуем её получить
			// поиск этой записи будем проводить в пределах среза периода, 
			// для StgCacheCycleSpoolProductionByDay это день
			// для StgCacheCycleSpoolProductionByMonth это месяц
			var lastRecDate = records.LastOrDefault().CycleDateEnd;
			//смотрим есть ли слева и справа записи в пределах разреза(день, месяц), если есть то добавляем по одному скаждого разреза 
			var right_rec = dbContext.Set<T>()
					.Where(
				r => r.PlaceId == placeId
				&& r.Composition != "Прочее"
				//Берем записи которые пересекаются периодом для работы
				&& StaticMethods.FirstPeriodContainsPartOfSecondPeriod(lastRecDate.Value.AddSeconds(1),
																		lastRecDate.Value.AddSeconds(1).Add(SpanForSearchEnclosingRec),
																		r.CycleDateBegin,
																		r.CycleDateEnd)
				).OrderBy(r => r.CycleDateBegin)
				.FirstOrDefault();
			var firstRecDate = records.FirstOrDefault().CycleDateBegin;
			var left_rec = dbContext.Set<T>()
					.Where(
				r => r.PlaceId == placeId
				&& r.Composition != "Прочее"
				//Берем записи которые пересекаются периодом для работы
				&& StaticMethods.FirstPeriodContainsPartOfSecondPeriod(firstRecDate.Value.AddSeconds(-1),
																		firstRecDate.Value.AddSeconds(-1).Add(-SpanForSearchEnclosingRec),
																		r.CycleDateBegin,
																		r.CycleDateEnd)
				).OrderBy(r => r.CycleDateBegin)
				.LastOrDefault();
			if (left_rec != default)
			{
				records.Insert(0, left_rec);
			}
			if (right_rec != default)
			{
				records.Add(right_rec);
			}
			//---------------------------------------------------
			#endregion
			


			for (int i = 0; i < records.Count - 1; i++)
			{

				//Если нашли разрыв в циклах
				if (records.ElementAt(i + 1).CycleDateBegin - records.ElementAt(i).CycleDateEnd >minSpanBetweanCycles)
				{
					//
					if ((records.ElementAt(i + 1).CycleDateBegin - records.ElementAt(i).CycleDateEnd).Value.TotalHours > t)
					{

						//В зависимости от какого периода идет выравнивания нужно выястить, 
						//нужен ли разрыв, и получить функцию получения следующего разрыва
						var  nextDate = NextDate<T>(records.ElementAt(i).CycleDateEnd ?? default, records.ElementAt(i + 1).CycleDateBegin ?? default);

						var finish = records.ElementAt(i + 1).CycleDateBegin ?? default;
						var begin = records.ElementAt(i).CycleDateEnd ?? default;
						var nextBreak = nextDate(begin);
						nextBreak = nextBreak < finish ? nextBreak : finish;
						//нарезаем пока есть разрыв
						while (begin < finish)
						{
							recsToAdd.Add(new T()// день особо не нужен, так что можно пока не добавлять, 
							{

								Cycle = -1,
								CycleDateBegin = begin,
								CycleDateEnd = nextBreak,
								Weight = 0,
								Place = records.ElementAt(i).Place,
								PlaceId = records.ElementAt(i).PlaceId,
								Composition = "Прочее",
								Month = records.ElementAt(i).Month,
								Year = records.ElementAt(i).Year

							});


							begin = nextBreak.AddSeconds(1);
							nextBreak = nextDate(begin);
							nextBreak = nextBreak < finish ? nextBreak : finish;
							

						}



					}
					else
					{
						//получаем разрыв в зависимости от типа записей
						var periodChange = EvalPeriodBreak<T>(records.ElementAt(i).CycleDateEnd ?? default, records.ElementAt(i + 1).CycleDateBegin ?? default);
						//выравниваем периоды
						//т.к. запись выравниваем по левому краю, то сдвиг происходит либо относительно левого края второй записи сдвигается к ближайшему из двух(разрыв и начало предыдущей записи), а у первой записи только выравнивание конца относительно разрыва
						records.ElementAt(i).CycleDateEnd = periodChange.begin < records.ElementAt(i + 1).CycleDateBegin ? periodChange.begin : records.ElementAt(i).CycleDateEnd;
						records.ElementAt(i + 1).CycleDateBegin = records.ElementAt(i).CycleDateEnd < periodChange.end ? periodChange.end : records.ElementAt(i).CycleDateEnd;
					};

				}


			}
			dbContext.Set<T>().UpdateRange(records);
			dbContext.Set<T>().AddRange(recsToAdd);

			return dbContext.SaveChanges();
		}
		public int CycleSpoolProduction_Correction_Cycle_ByShifts<T>(int placeId)
		where T : StgCacheCycleSpoolProductionByShifts, new()
		{
			//Сдвигаем время чтобы брались данные внутри сменны, иначе может получиться ситуация, 
			//после коррекции могу возникнуть пересекующиеся циклы, если в выборку для корректировки не попали циклы,
			//которые входят в смену. Например выборка с 00.00 по 00.00, и сюда не попал цикл  с 21.00 по 23.00.
			//А так как края расширяются то с этим циклом будте пересекатся первый циел который расширят до second_shift_start_hour
			var (start_Date, end_Date) = curDates[typeof(T)];

			List<T> records = dbContext.Set<T>().Where(
				r => r.PlaceId == placeId
				//&& r.Composition != "Прочее"
				&& StaticMethods.FirstPeriodContainsPartOfSecondPeriod(start_Date, end_Date, r.CycleDateBegin, r.CycleDateEnd)
				).OrderBy(r => r.CycleDateBegin)
				.ToList();


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

			}

			for (int i = 0; i < records.Count; i++)
			{

				//Чиним смены, за одно учитываем крайние точки i = 0 и  i = records.Count-1
				// Так как records[i] нельзя передовать в ref то содаём буферные переменые
				DateTime startPeriodToFix = records[i].CycleDateBegin;
				DateTime endPeriodToFix = records[i].CycleDateEnd;

				FixShiftsEndTime(ref startPeriodToFix, ref endPeriodToFix, i != (records.Count - 1) ? records[i + 1].CycleDateBegin : new DateTime(), i == (records.Count - 1) ? true : false);

				records[i].CycleDateEnd = endPeriodToFix;
				

			}

			return dbContext.SaveChanges();

		}

		public async Task<int> Add_liquid_and_wetness_data() =>
			await MultiThread_execSqlAsync(scopeFactory, ShiftsStartDate, ShiftsEndDate, "csp_shifts_add_fluid_stream_and_wetness", TimeSpan.FromHours(shift_duration));

		
	}
	public class CheckerSqlTableAvail
	{
		string colNameToCheckTime = "TimeStamp";
		string colNameToCheckTable = "Tablename";
		string tableNameWhereCheck = "STG_BackgroundTaskTimeStamps";
		TimeSpan periodBetweenCheck = TimeSpan.FromMilliseconds(500);
		TimeSpan timeout = TimeSpan.FromSeconds(90);

		string tableNameToCheck;
		
		ScaffoldContext dbContext;
		public CheckerSqlTableAvail(string tableNameToCheck,
		
		ScaffoldContext dbContext){
			this.tableNameToCheck = tableNameToCheck;
			
			this.dbContext = dbContext;
		}

		//Суть в том чтобы подождать пока изменятся данные на сервер sql, это характеризуется тем что время изменения таблицы позже старта операции
		public void WaitChanges(ref DateTime timeToCheck, int affectedRows)
		{
			if (affectedRows==0)//Если предыдущая операция не затронула данные, ждать нечего
			{
				return;
			}
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			
			
			while (stopWatch.ElapsedMilliseconds < timeout.TotalMilliseconds)//делаем проверки пока не выйдет таймаут
			{
				var sqlCmd = $@"
SELECT [{colNameToCheckTime}]
FROM [dbo].[{tableNameWhereCheck}]
WHERE {colNameToCheckTable} = '{tableNameToCheck}'";
				var dr = dbContext.Database.ExecuteSqlQuery(sqlCmd).DbDataReader;
				dr.Read();
			
				if (dr.GetDateTime(0).Ticks > timeToCheck.Ticks)
				{
					timeToCheck = DateTime.Now;

					return;					
				}
				Thread.Sleep(periodBetweenCheck);
			}
			throw new Exception("Sql connection timeout!"); ;
		}
		
	}
}
