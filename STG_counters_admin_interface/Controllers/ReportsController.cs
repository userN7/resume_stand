using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using STG_counters_admin_interface.Models.PowerDB_data_classes;
using STG_counters_admin_interface.Models;
using STG_counters_admin_interface.Models.Pages_control;
using System.Reflection;
using static STG_counters_admin_interface.Models.StaticMethods;
using static STG_counters_admin_interface.Models.Constants;
using Microsoft.AspNetCore.Authorization;

namespace STG_counters_admin_interface.Controllers
{
	public class ReportsController : Controller
	{
		private ScaffoldContext context;
		public Dictionary<string, string> PerfTimes = new Dictionary<string, string>();
		public ReportInfo reportInfo_contr { get; set; }
		public ReportsController(ScaffoldContext ctx, ReportInfo reportInfo_contr=null) 
		{ 
			this.context = ctx;
			this.reportInfo_contr = reportInfo_contr?? new ReportInfo();
		}
		
		public ViewResult Default_Index_action()
		{
			//Форматирование страниц(значения по умолчанию(10 строк на страницу))
			ViewBag.pages_Options = new QueryOptions();
			return View("Index", context.StgSkuDictionary.Select(r => r.SkuName).ToList());
		}
		//[HttpGet]
		public IActionResult Index()
		{
			//return FilteredRecords();
			return Default_Index_action();
		}

		public IActionResult ShowReport(
			DateTime start_Date			
			, DateTime end_Date
			
			, string report_name
			//Показывать ли записи за период
			, int selectedPeriod = 0
			//используется для фильтра по сорту продукции в отчетах
			, string selectedSortOfProduction = null
			, int export_excel = 0
			, string[] baseStrings = null//используется для экспорта рисункову диаграм в эксель
			, int lastDay = 0 // нужен вывод за последний день
			, int tesYear = 0//для отчёта по ТЭС нужен отдельно год отчёта
			, int selectedMonth = 0
			, int selectedYear = 0
			, int currShiftIndex = -1)//индекс смены для отчёта по сменам
		{
			//Формируем инфу по проекту
			var reportName = new ReportName(report_name);
			var reportDates = new ReportDates(start_Date, end_Date, selectedPeriod, lastDay,
				tesYear,//для отчёта по ТЭС нужен отдельно год отчёта
			selectedMonth, selectedYear);
			
			//Сохраняем использование отчёта
			Save_report_usage(this, reportName.Rus, context);
			
			//берём закешированные данные если они есть, иначе формируем новые данные
			var reportInfo = reportInfo_contr;
			reportInfo.EvalReportInfo(reportName, context, reportDates, currShiftIndex: currShiftIndex, export_excel: export_excel, selectedSortOfProduction, baseStrings, curr_controller: this);
			
			if (export_excel == 1)
				return Excel_methods.Export_to_Excel(reportName.Eng, reportInfo, controller: this);

			#region viewbag filling
			//Подготавливаем параметры для отчёта
			//в основном данные берутся из reportInfo, но где то может понадобится
			ViewBag.start_Date = reportInfo.ReportDates.StartDate;
			ViewBag.end_Date = reportInfo.ReportDates.StartDate;
			ViewBag.report_name = reportName.Full;
			ViewData["tesYear"] = tesYear;
			#endregion
			return View(reportInfo.ReportName.ViewName, reportInfo);
		}
	}
}