//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using STG_counters_admin_interface.Models.PowerDB_data_classes;
//using STG_counters_admin_interface.Models;
//using Microsoft.EntityFrameworkCore;
//using STG_counters_admin_interface.Models.Pages_control;
//using Newtonsoft.Json;
//using OfficeOpenXml.Drawing;
//using System.Drawing;
//using Microsoft.Extensions.DependencyInjection;
//using System.Threading;
//using static STG_counters_admin_interface.Models.StaticMethods;

//namespace STG_counters_admin_interface.Controllers
//{

	
//	public class CorrectionRecordsController : Controller
//    {
//		private readonly IServiceScopeFactory scopeFactory;
//		private ScaffoldContext context;
//		public CorrectionRecordsController(ScaffoldContext ctx, IServiceScopeFactory scopeFactory) { context = ctx; this.scopeFactory = scopeFactory; }
//	public IActionResult Index()
//        {
//            return View("index");
//        }

//		public async Task<IActionResult> Action_handler(DateTime start_Date, DateTime end_Date, int action_number, int selectedPeriod=0, int tesYear=0)
//		{
			
			
			
//			StaticMethods.Check_dates(ref start_Date, ref end_Date, selectedPeriod);
//			//т.к. даты передаются без часов, делаем конец дня		
//			end_Date = end_Date.Date.AddDays(1).AddSeconds(-1);
//			try
//			{
//				switch (action_number)
//				{
//					case 1://"fill_Cache_TagDump"
//						StaticMethods.ExecSql(context, "fill_Cache_TagDump", start_Date, end_Date);
						
//						break;
//					case 2://"fill_Cache_AverageSpeed"
//						StaticMethods.ExecSql(context, "fill_Cache_AverageSpeed", start_Date, end_Date);						

//						break;
//					case 3://"Fill_Cache_CycleSpoolProduction_byDay"
//					case 4://"Fill_Cache_CycleSpoolProduction_byMonth"
//					//заполняем помесячно	
					
//						//end_Date = StaticMethods.GetEndOfMonth(end_Date);
//						//while (start_Date < end_Date)
//						//{

//						//		StaticMethods.ExecSql(context, action_number == 3 ? "Fill_Cache_CycleSpoolProduction_byDay" : "Fill_Cache_CycleSpoolProduction_byMonth"
//						//				, start_Date
//						//				, StaticMethods.GetEndOfMonth(start_Date));
//						//		start_Date = start_Date.AddMonths(1);

//						//}
//					//	await  Fill_Cache;

//						break;
//					//case 5://CycleSpoolProduction_Correction_Full day
//					//case 6://CycleSpoolProduction_Correction_Full month
//					//	StaticMethods.CycleSpoolProduction_Correction_Full(context, start_Date, end_Date, isByMonth: action_number==5?true:false);
						
//						//break;
//					case 7://TER_avg_permonth

						
//							await StaticMethods.TER_avg_permonth(context, start_Date, end_Date,tesYear: tesYear);
						
						
//						break;
//					default:
//						break;
//				}
//			}
//			catch (Exception e)
//			{
//				CustomExceptionHandler(e);

//			}
			
//			return View("index");
//		}



//	}
//}