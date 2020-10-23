using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using STG_counters_admin_interface.Models.PowerDB_data_classes;
using STG_counters_admin_interface.Models;
using Microsoft.EntityFrameworkCore;
using STG_counters_admin_interface.Models.Pages_control;
using Newtonsoft.Json;
using OfficeOpenXml.Drawing;
using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using static STG_counters_admin_interface.Models.StaticMethods;


//using STG_counters_admin_interface.Models.Additional_Data_clases;

namespace STG_counters_admin_interface.Controllers
{
	
	public class TestController : Controller
	{
		
		private readonly IServiceScopeFactory scopeFactory;
		private ScaffoldContext context;
		public TestController(ScaffoldContext ctx, IServiceScopeFactory scopeFactory) { context = ctx; this.scopeFactory = scopeFactory; }
		//string sqlstring =

		public IActionResult Error() {
			//var result  = context.Database.ExecuteSqlCommand("select");
			return View("index");
		}

		
		
		
		[HttpGet]
		public IActionResult Index()
		{
			
			return View("index"
				//,context.PaRecords.Where(r=>r.RecordTime>DateTime.Now.AddDays(-100)&& r.RecordTime <= DateTime.Now.AddDays(-89))
				);
			
		}



		public IActionResult Test()
		{
			string serverUrl = "http://192.168.0.147:8080/";

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
					var t = new List<DateTime>() { DateTime.Now };
					StaticMethods.SendEmailReport(ref t
					, StaticMethods.Get_email_receivers(dbContext, "SkuDataByShifts_BDM")
					
					, "По сменный отчет БДМ"
					, msg
					,testing:true);
				}
				catch (Exception e)
				{
					CustomExceptionHandler(e);
					//dbContext.StgBackgroundTasklogs.Add(new StgBackgroundTasklogs { LogDateTime = DateTime.Now, TimerName = timer_data.TimerName, TypeId = 3 });

				}



			}
			return View("index");
		}

	}
}