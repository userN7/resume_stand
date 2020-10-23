using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STG_counters_admin_interface.Models.PowerDB_data_classes;
using STG_counters_admin_interface.Models;

using Newtonsoft.Json;
using STG_counters_admin_interface.Models.Pages_control;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using static STG_counters_admin_interface.Models.StaticMethods;
using Microsoft.Extensions.Hosting;
using static STG_counters_admin_interface.Models.Constants;

namespace STG_counters_admin_interface.Controllers
{
    public class AdminController : Controller
    {
        private ScaffoldContext context;
        private readonly IServiceScopeFactory scopeFactory;

        public AdminController(ScaffoldContext ctx, IServiceScopeFactory scopeFactory) { context = ctx; this.scopeFactory = scopeFactory; }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Logs()
        {
            return View("Logs");
        }
        [HttpPost]
        public IActionResult Logs(
            DateTime start_Date            
            , DateTime end_Date
            , string log_name
            , int selectedPeriod = 0
            )
        {
            
            StaticMethods.Check_dates(ref start_Date, ref end_Date, selectedPeriod);
            ViewBag.start_Date = start_Date;
            ViewBag.end_Date = end_Date;
            string tree="";
            switch (log_name)
            {
            case "ReportsRunHistory":
                    var records = context.StgReportsUsage.Select(r => new { RemoteIp=r.RemoteIp.Trim()==""? "Background" : r.RemoteIp, r.ReportName, r.AccessDate })
                .Where(r => r.AccessDate <= end_Date && r.AccessDate >= start_Date)
                .OrderBy(r => r.RemoteIp).ThenBy(r => r.ReportName).ThenBy(r => r.AccessDate)
                .ToList();
               
                    tree = JsonConvert.SerializeObject(StaticMethods.BuildTree_Generic(records));
            
            break;
                case "BackgroundTasks":
                     tree = JsonConvert.SerializeObject(StaticMethods.BuildTree_Generic(context.StgBackgroundTasklogs.Select(r => new { r.TimerName,r.Type.Typename, r.LogDateTime
                         , Message=r.Message!=null?r.Message:"" 
                     })
               .Where(r => r.LogDateTime <= end_Date && r.LogDateTime >= start_Date)
               .OrderBy(r => r.TimerName)
               .ThenBy(r => r.Typename)
               .ThenBy(r => r.LogDateTime)
               .ThenBy(r => r.LogDateTime)
               .ToList()));
                  
                    break;
            default:
            break;
            };

            ViewBag.Json = tree;
            ViewBag.JsTreePlugins = @"[""wholerow"", ""search""]";
            return View("Logs");
        }
        [HttpPost]
        public IActionResult Background_tasks_handler(string taskName, string actionName)
        {
            var backgroundTasks = (TimedHostedService)(scopeFactory.CreateScope().ServiceProvider.GetService<IHostedService>());
            //var List_of_timers_data =backgroundTasks.List_of_timers_data;
            
            //MethodInfo methodInfo = backgroundTasks.GetType().GetMethod(taskName);
            
            switch (actionName)
            {
               case "task_start":
                    backgroundTasks.GetType().GetMethod(taskName,BindingFlags.Public|BindingFlags.Instance|BindingFlags.NonPublic).Invoke(backgroundTasks,new Timer_data[] { new Timer_data() { TimerName = $"Admin_Handler_{taskName}" } });



                    break;
                default:
                    break;
            }
            Forming_lists_timers(this, scopeFactory);
            return View(
                );
        }

		public IActionResult Background_tasks_handler()
		{

			Forming_lists_timers(this, scopeFactory);
			return View(
				);
		}
		//[HttpGet]
		//public IActionResult Action_handler()
		//{
		//	return View("CorrectionRecords", -2);
		//}
		//[HttpPost]
		public async Task<IActionResult> Action_handler(DateTime start_Date, DateTime end_Date, int action_number, int selectedPeriod = 0, int tesYear = 0)
        {

            var result = -2;

            StaticMethods.Check_dates(ref start_Date, ref end_Date, selectedPeriod);
            //т.к. даты передаются без часов, делаем конец дня		
            end_Date = end_Date.Date.AddDays(1).AddSeconds(-1);
            try
            {
                var fillCache = new Fill_Cache(scopeFactory, start_Date, end_Date);
                switch (action_number)
                {
                 
                    case 1://"fill_Cache_TagDump"
                        result = fillCache.Fill_cache_csp_byShifts();
                        break;    
                    case 3:

                        result = await fillCache.Fill_cache_csp_full<StgCacheCycleSpoolProductionByDay>();
                        
                        break;
                    case 4:
                        result = await fillCache.Fill_cache_csp_full<StgCacheCycleSpoolProductionByMonth>();
                        
                        break;
                    case 5://Уточнить таблицу CSP_ByDay с распределением пустого времени в зависимости от параметра
                        foreach (var placeId in placeIdsForCorrection)
						{
                            result += fillCache.CycleSpoolProduction_Correction_Cycle<StgCacheCycleSpoolProductionByDay>(placeId);
                        }
                        break;
                    case 6://Уточнить таблицу CSP_ByMonth с распределением пустого времени в зависимости от параметра
                        foreach (var placeId in placeIdsForCorrection)
                        {
                            result = fillCache.CycleSpoolProduction_Correction_Cycle<StgCacheCycleSpoolProductionByMonth>(placeId);
                        }
                        break;
                    case 7://TER_avg_permonth
                        //перекешируем месячные отчёты в году для пересчёта
                        start_Date = new DateTime(tesYear, 1, 1);
                        end_Date = tesYear == DateTime.Now.Year ? DateTime.Now : new DateTime(tesYear, 12, 31, 23, 59, 59);                        
                        TER_avg_permonth(context, start_Date, end_Date, tesYear: tesYear).Wait(); ;
                        break;
                    case 8://Заполнить таблицу CSP_ByShift
                        result = await fillCache.Fill_cache_csp_full<StgCacheCycleSpoolProductionByShifts>();
                        
                        break;
                    case 9://Уточнить таблицу CSP_ByShifts в пределах сменн
                        foreach (var placeId in placeIdsForCorrection)
                        {
                            result = fillCache.CycleSpoolProduction_Correction_Cycle_ByShifts<StgCacheCycleSpoolProductionByShifts>(placeId);
                        }
                        break;                    
                    case 11://Заполнить влажность и жидкий поток для CSP_ByShifts
                        result = await fillCache.Add_liquid_and_wetness_data();                       
                        break;                    
                    case 13:
                        result = await MultiThread_execSqlAsync(scopeFactory, start_Date, end_Date, "Update_avg_speed_Cache_CycleSpoolProduction_byShifts", TimeSpan.FromDays(3));                       
                        break;
                    case 14:
                        result = await MultiThread_execSqlAsync(scopeFactory, start_Date, end_Date, "Update_avg_speed_Cache_CycleSpoolProduction_byDay", TimeSpan.FromDays(3));
                        break;
                    case 15:
                        result = await MultiThread_execSqlAsync(scopeFactory, start_Date, end_Date, "Update_avg_speed_Cache_CycleSpoolProduction_byMonth", TimeSpan.FromDays(3));
                        break;
                    case 16://Заполнить таблицы CSP_ByDay, CSP_ByMonth, CSP_ByShifts
                        result = await fillCache.Fill_Cache_Full();
						                       
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                CustomExceptionHandler(e);

            }

            return View("CorrectionRecords",result);
        }


    }
}