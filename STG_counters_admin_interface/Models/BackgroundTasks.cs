using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using STG_counters_admin_interface.Models.PowerDB_data_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static STG_counters_admin_interface.Models.StaticMethods;
using static STG_counters_admin_interface.Models.Constants;
//Механизм с фоновыми задачами реализован через таймеры, и пока не очень, так что я дописал свой логер в ДБ и метод добаления таймеров. 


namespace STG_counters_admin_interface.Models
{
	//Класс для хранения данных по таймерам
	public class Timer_data {
		public TimerCallback method;
		public string TimerName {get;set;}
		public DateTime StartDate { get; set; }
		public TimeSpan Period { get; set; }
	}
	internal class TimedHostedService : IHostedService, IDisposable
	{
		private readonly IServiceScopeFactory scopeFactory;
		public bool IsBackgroundProcess = false;
		//private  ScaffoldContext dbContext;
		//Добовляем scope для доступа к базе данных черзе ScaffoldContext
		public TimedHostedService(IServiceScopeFactory scopeFactory)
		{
			this.scopeFactory = scopeFactory;
			Timers = new List<Timer>();
			List_of_timers_data = new List<Timer_data>();
			using ( var scope = scopeFactory.CreateScope())
			{
				IsBackgroundProcess = scope.ServiceProvider.GetRequiredService<WebHostBuilderContext>().Configuration.GetSection("USERNAME").Value == "ascue_IIS";
				//IsBackgroundProcess = true;
			}
			


		}


		//private readonly ILogger _logger;
		string serverUrl = "http://192.168.0.147:8080/";
		public List<Timer> Timers { get; }
		public List<Timer_data> List_of_timers_data { get; }
		public DateTime TimeToCacheTasks { get; set; }
		public DateTime TimeToSendDaily_TER_avg_permonth { get; set; }
		public DateTime TimeToSendDaily_Shifts { get; set; }
		public DateTime TimeTo_TER_avg_permonth { get; set; }
		//Здесь сохраняем список временных меток когда отправляли пиьсма
		List<DateTime> SentMsges_daily_TER_avg_permonth = new List<DateTime>();
		List<DateTime> SentMsges_Shifts = new List<DateTime>();
		List<DateTime> SentMsges_CheckPaRecordByStatus = new List<DateTime>();

		public Task StartAsync(CancellationToken cancellationToken)
		{
			//Если процесс для обработки бегграундных задач
			if (//true
					IsBackgroundProcess
					)
			{
				//_logger.LogInformation("Timed Background Service is starting.");
				int hourToStartDailyTasks = 8;
				//11;//debug
			int minuteToStartDailyTasks = 25;
			//27;//debug
			int hourToSendDaily_TER_avg_permonth = 8;
			//1;//debug
			int minuteToSendDaily_TER_avg_permonth = 0;
			//25;//debug
			TimeSpan dayPeriod = new TimeSpan(24, 0, 0);
			TimeSpan halfDayPeriod = new TimeSpan(12, 0, 0);
			TimeSpan hourPeriod = new TimeSpan(1, 0, 0);
			TimeSpan cacheTasksPeriod = hourPeriod;
			TimeSpan TER_avg_permonth = cacheTasksPeriod;//hourPeriod;

			TimeToCacheTasks = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hourToStartDailyTasks, minuteToStartDailyTasks, 0);
			//DateTime.Now.AddSeconds(10);//debug
			TimeToCacheTasks = StaticMethods.CorrectionStartDay(TimeToCacheTasks, cacheTasksPeriod);

			TimeTo_TER_avg_permonth = TimeToCacheTasks;
				//new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour: 7, minute: 55, second: 0);
				//DateTime.Now.AddSeconds(10);//debug
			TimeTo_TER_avg_permonth = StaticMethods.CorrectionStartDay(TimeTo_TER_avg_permonth, TER_avg_permonth);

			TimeToSendDaily_Shifts = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour: 8, minute: 30, second: 0);
			TimeToSendDaily_Shifts = StaticMethods.CorrectionStartDay(TimeToSendDaily_Shifts, halfDayPeriod);

			TimeToSendDaily_TER_avg_permonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hourToSendDaily_TER_avg_permonth, minuteToSendDaily_TER_avg_permonth, 0);
			TimeToSendDaily_TER_avg_permonth = StaticMethods.CorrectionStartDay(TimeToSendDaily_TER_avg_permonth, dayPeriod);

			using (var scope = scopeFactory.CreateScope())
			{

				//у нас отдельно висит процесс запущенный для фоновых задач из под сервиса,
				//так что проверяем чтобы пользователь был из под которого запущена служба
				// Важный момент, под IIS процессы быстро убиваются так что там нельзя запускать в фоне ничего долго играющего
				

				var dbContext = scope.ServiceProvider.GetRequiredService<ScaffoldContext>();
				//dbContext.StgBackgroundTasklogs.Add(new StgBackgroundTasklogs { LogDateTime = DateTime.Now, Message = $"{scope.ServiceProvider.GetRequiredService<WebHostBuilderContext>().Configuration.GetSection("SESSIONNAME").Value}", TimerName = "debug",TypeId=2 });
				//dbContext.StgBackgroundTasklogs.Add(new StgBackgroundTasklogs { LogDateTime = DateTime.Now, Message = $"{scope.ServiceProvider.GetRequiredService<WebHostBuilderContext>().Configuration.GetSection("USERNAME").Value}", TimerName = "debug", TypeId = 2 });
				//Чтобы запускалось в круглое время вычисляем ближайшее время в формате HH:00:00 

				DateTime roundTime = DateTime.Now.AddHours(1).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second);

					List_of_timers_data.Add(new Timer_data { method = new TimerCallback(Email_report_CheckPaRecordByStatus)
						, StartDate = roundTime
						, Period = TimeSpan.FromMinutes(
					//ищем отчет "Отчет о некорректных записях" и получаем периодичность для задания
					dbContext.StgEmailReportsNames.Where(r => r.ReportNameEng == "CheckPaRecordByStatus").FirstOrDefault().DelayTime)
						, TimerName = "email_report_CheckPaRecordByStatus"
					});
				
					List_of_timers_data.Add(new Timer_data { method = new TimerCallback(Email_report_Daily_TER_avg_permonth)
						, StartDate =  TimeToSendDaily_TER_avg_permonth
						//,StartDate = DateTime.Now.AddSeconds(5)
						,Period = TimeSpan.FromMinutes(
								//ищем отчет "Средневзвешенное ТЭР на тонну по циклам производства" и получаем периодичность для задания
								dbContext.StgEmailReportsNames.Where(r => r.ReportNameEng == "Daily_TER_avg_permonth").FirstOrDefault().DelayTime)
						, TimerName = "email_report_Daily_TER_avg_permonth"
				});
					//для управления разностью времени между задачами
					int diff_minutes = 0;

					List_of_timers_data.Add(new Timer_data
					{
						method = new TimerCallback(Fill_Caches)
						,StartDate = TimeToCacheTasks.AddMinutes(diff_minutes) 
						//DateTime.Now.AddSeconds(5)//debug
						,
						Period = cacheTasksPeriod
						,
						TimerName = "Fill_Caches"
					});

					List_of_timers_data.Add(new Timer_data { method = new TimerCallback(Fill_TER_avg_permonth)
						, StartDate = //DateTime.Now.AddSeconds(5) 
						TimeTo_TER_avg_permonth
						, Period = TER_avg_permonth
						, TimerName = "Fill_TER_avg_permonth"
					});
					diff_minutes += 1;
					List_of_timers_data.Add(new Timer_data
					{
						method = new TimerCallback(Email_report_SkuDataByShifts_BDM)
						,
						StartDate = //DateTime.Now.AddSeconds(5) 
						TimeToSendDaily_Shifts
						,
						Period = halfDayPeriod
						,
						TimerName = "email_report_SkuDataByShifts_BDM"
					});

					foreach (var timer_data in List_of_timers_data)
					{
						StaticMethods.AddNewTimer(new TimerCallback(WrapTimer), timer_data, Timers, dbContext);
					}
				}	
			}
			
			return Task.CompletedTask;
		}

		private void Email_report_CheckPaRecordByStatus(object obj)
		{
			var timer_data = (Timer_data)obj;
			string start_Date = DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:00:00");//берём промежуток за 24 часа от текущей даты за вычетом минут и секунд	
			string end_Date = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00");
			using (var scope = scopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<ScaffoldContext>();
				string msg = "";
				string sql_string = StaticMethods.sqlString_by_searchOptions(2,//Отсуствующие записи
					 start_Date, end_Date, device_id: 0,//все устройства
					context: dbContext);
					
					//сохраняем количество записей
				int count_of_missing_records = dbContext.PaRecordsByFilter.FromSql(sql_string).Count(); ;

				//берём промежуток за 24 часа
				sql_string = StaticMethods.sqlString_by_searchOptions(1,//неподходящие по критерию
					start_Date, end_Date, 0,//все устройства
					context: dbContext);

				int count_of_out_of_creteria_records = dbContext.PaRecordsByFilter.FromSql(sql_string).Count(); ;
				//int count_of_records = reader.DbDataReader.RecordsAffected;

				if (count_of_missing_records > 0 || count_of_out_of_creteria_records > 0)
				{
					
			    if (count_of_out_of_creteria_records > 0)
					{
						msg += "Есть не подходящие под критерий записи. Кол-во : " + count_of_out_of_creteria_records.ToString() + "\n";
					}
					if (count_of_missing_records > 0)
					{
						msg += "Есть отсутствующие записи. Кол-во : " + count_of_missing_records.ToString() + "\n";
					}
					try
					{
						StaticMethods.SendEmailReport(ref SentMsges_CheckPaRecordByStatus
						, StaticMethods.Get_email_receivers(dbContext, "CheckPaRecordByStatus")
						, "Оповещение системы АСТУЭ"
						, msg);
					}
					catch (Exception e)
					{
						CustomExceptionHandler(e);						
					}
					
				}
				
				
			}
		}
		private void Email_report_Daily_TER_avg_permonth(object obj)
		{
			var timer_data = (Timer_data)obj;
			using (var scope = scopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<ScaffoldContext>();
				DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-1);
				DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddSeconds(-1);
				var reportName = new ReportName("TERperMonthperTonne;Средневзвешенное ТЭР на тонну по циклам производства");
				var reportDates = new ReportDates(startDate, endDate, tesYear: DateTime.Now.Year);
				var reportInfo = new ReportInfo();

				reportInfo.EvalReportInfo(reportName,
					dbContext,
					reportDates
					) ;
				var excel_file = Excel_methods.Export_to_Excel_TERperMonthperTonne(reportInfo);


				string serverUrl = "http://192.168.0.147:8080/";
				
				
				string msg = $@"<html><body><h2>Здравствуйте. Отчет средневзвешенное ТЭР на тонну по циклам производства во вложении.</h2> <h4>Версию с графиками можно посмотреть и выгрузить здесь: <a href= ""{serverUrl}Reports/ShowReport?lastDay=1&report_name=TERperMonthperTonne%3B%D0%A1%D1%80%D0%B5%D0%B4%D0%BD%D0%B5%D0%B2%D0%B7%D0%B2%D0%B5%D1%88%D0%B5%D0%BD%D0%BD%D0%BE%D0%B5+%D0%A2%D0%AD%D0%A0+%D0%BD%D0%B0+%D1%82%D0%BE%D0%BD%D0%BD%D1%83+%D0%BF%D0%BE+%D1%86%D0%B8%D0%BA%D0%BB%D0%B0%D0%BC+%D0%BF%D1%80%D0%BE%D0%B8%D0%B7%D0%B2%D0%BE%D0%B4%D1%81%D1%82%D0%B2%D0%B0"">Дневной отчет средневзвешенное ТЭР на тонну по циклам производства</a><h4></body></html>";
				try
				{
					StaticMethods.SendEmailReport(ref SentMsges_daily_TER_avg_permonth
					, StaticMethods.Get_email_receivers(dbContext, "Daily_TER_avg_permonth")
					//new List<string> {"thst@mail.ru"} //debug
					, "Средневзвешенное ТЭР на тонну по циклам производства"
					, msg
					,attachment_data: excel_file.FileContents);
				}
				catch (Exception e)
				{
					CustomExceptionHandler(e);					

				}
				
				
				
			}
		}

		private void Email_report_SkuDataByShifts_BDM(object obj)
		{
			var timer_data = (Timer_data)obj;
			using (var scope = scopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<ScaffoldContext>();

				DateTime startDate = DateTime.Now.AddDays(-1);
				DateTime endDate = DateTime.Now;


				string msg = $@"<html><body><h2>Здравствуйте. Отчет по сменам.</h2> 
												<h4>Версию с графиками можно посмотреть и выгрузить здесь: 
												<br>
												<a href= ""{serverUrl}Reports/ShowReport?start_Date={startDate.ToString("yyyy-MM-dd")}&end_Date={endDate.ToString("yyyy-MM-dd")}&report_name=SkuDataByShifts_BDM1%3BПо+сменный+отчет+БДМ-1"">БДМ-1</a><br>
												<a href= ""{serverUrl}Reports/ShowReport?start_Date={startDate.ToString("yyyy-MM-dd")}&end_Date={endDate.ToString("yyyy-MM-dd")}&report_name=SkuDataByShifts_BDM2%3BПо+сменный+отчет+БДМ-2"">БДМ-2</a><br>
												<h4>
										</body></html>";
				try
				{
					StaticMethods.SendEmailReport(ref SentMsges_Shifts
					, StaticMethods.Get_email_receivers(dbContext, "SkuDataByShifts_BDM")
					//new List<string> {"thst@mail.ru"} //debug
					, "По сменный отчет БДМ"
					, msg);
				}
				catch (Exception e)
				{
					CustomExceptionHandler(e);

				}
			}
		}

		private void Fill_TER_avg_permonth(object obj)
		{

			var timer_data = (Timer_data)obj;
			using (var scope = scopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<ScaffoldContext>();
				var is_Gamma_available = Test_linkedServer(dbContext, "Gamma");
				DateTime startDate = new DateTime(DateTime.Now.Year,1,1);
				DateTime endDate = DateTime.Now;
				if (is_Gamma_available)
				
				try
				{
					//если прошел день с начала года то ещё считаем итог и за прошлый год
					if ((endDate - startDate).TotalDays == 1)
					{
						//Fill_cache_csp_byMonth(scopeFactory.CreateScope(), startDate, endDate).Wait();//сначала перекешируем csp за месяц, который участвет в отчёте
						TER_avg_permonth(scope.ServiceProvider.GetRequiredService<ScaffoldContext>(), startDate, endDate, DateTime.Now.AddYears(-1).Year).Wait();//считем за текущий год (startDate, endDate, - не имеют значение, остались от старой реализации, важен только год)
					}

					//Fill_cache_csp_byMonth(scopeFactory.CreateScope(), startDate, endDate).Wait();//сначала перекешируем csp за месяц, который участвет в отчёте
					TER_avg_permonth(scope.ServiceProvider.GetRequiredService<ScaffoldContext>(), startDate, endDate, DateTime.Now.Year).Wait();//считем за текущий год (startDate, endDate, - не имеют значение, остались от старой реализации, важен только год)

				}
				
				catch (Exception e)	{
					CustomExceptionHandler(e);
				}
				
			}
		}
	
		private void Fill_Caches(object obj)
		{
			//dbContext некорректно передается следующим потокам, так что каждому потоку создаем свой контекст
			var timer_data = (Timer_data)obj;
			
			var startDate = DateTime.Now.Date.AddDays(-2);
			var endDate = DateTime.Now.Date.AddDays(1);
			
			var task = (new Fill_Cache(scopeFactory, startDate, endDate)).Fill_Cache_Full();
			task.Wait();//ограничение по времени, это таймаут sql опериций, которые выбростят исключение			
			//проверяем были ли пустые загрузки, если были то уведовляем админа
			//CheckResults(task.Result);			
			
		}
		
		public Task StopAsync(CancellationToken cancellationToken)
		{
			//_logger.LogInformation("Timed Background Service is stopping.");
			//Stop all timers
			foreach (var timer in Timers)
			{
				timer?.Change(Timeout.Infinite, 0);
			}
			

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			//Dispose all timers
			foreach (var timer in Timers)
			{
				timer?.Dispose();
			}
		}

		void WrapTimer(object obj)
		{
			//для каждого таймера нужен свой скоуп и дбКонтекст
			using (var scope = scopeFactory.CreateScope())
			{
				var dbContext = scope.ServiceProvider.GetRequiredService<ScaffoldContext>();
				var timer_Data = (Timer_data)obj;
				dbContext.StgBackgroundTasklogs.Add(new StgBackgroundTasklogs { LogDateTime = DateTime.Now, TimerName = timer_Data.TimerName, TypeId = 4 });
				
				timer_Data.method.Invoke(timer_Data);
				dbContext.StgBackgroundTasklogs.Add(new StgBackgroundTasklogs { LogDateTime = DateTime.Now, TimerName = timer_Data.TimerName, TypeId = 5 });
				try	{dbContext.SaveChanges();}	catch (Exception e){ CustomExceptionHandler(e);}
			}


		}

		

	}
}
