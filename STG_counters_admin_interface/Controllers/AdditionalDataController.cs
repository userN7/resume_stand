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
using System.Reflection;
using static STG_counters_admin_interface.Models.StaticMethods;
using Microsoft.Extensions.DependencyInjection;
using static STG_counters_admin_interface.Models.SqlCmdStrings;

namespace STG_counters_admin_interface.Controllers
{
	public class AdditionalDataController : Controller
	{
		private ScaffoldContext context;
		private readonly IServiceScopeFactory scopeFactory;
		public AdditionalDataController(ScaffoldContext ctx, IServiceScopeFactory scopeFactory) 
			{context = ctx;
			this.scopeFactory = scopeFactory;
		}

		public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult CopyPlan()
		{
			return View("Plan_TER_Copy",
					context.StgSkuDictionary.Select(r => r.SkuName));
		}
		[HttpPost]
		public async Task<IActionResult> CopyPlan(int year=0, string sku="", int PlaceId=0, int year_CopyTo=0, string sku_CopyTo="", int PlaceId_CopyTo=0, string copy_full_Year="")
		{
			bool copy_full_year = copy_full_Year == "on" ? true : false;
			//Проверка на введенные данные
			string msg = "";
			if (!copy_full_year)
			{
				msg += ((year == 0) ? "Нужно выбрать год внесения источника!<BR>" : "")
			+ ((sku == "") ? "Нужно выбрать SKU источника!<BR>" : "")
			+ ((PlaceId == 0) ? "Нужно выбрать БДМ источника!<br>" : "")
			+ ((year_CopyTo == 0) ? "Нужно выбрать год внесения назначения!<BR>" : "")
			+ ((sku_CopyTo == "") ? "Нужно выбрать SKU назначения!<BR>" : "")
			+ ((PlaceId_CopyTo == 0) ? "Нужно выбрать БДМ назначения!<br>" : "");
			}
			else //для копирования всего года нужна проверка только на прараметр год
			{
				msg += ((year == 0) ? "Нужно выбрать год внесения источника!<BR>" : "")
					+ ((year_CopyTo == 0) ? "Нужно выбрать год внесения назначения!<BR>" : "");
			}
			
			
			
			if (msg != "")
			{
				ViewBag.Message_text = msg;
				ViewBag.Message_color = "red";
				return View("Plan_TER_Copy",
				context.StgSkuDictionary.Select(r => r.SkuName)
				);
			}

			

				var records_ToCopy = new List<StgPlanTer>();


				if (!copy_full_year)
				{
					Copy_StgPlanTer(context,

			  context.StgPlanTer.Where(r =>
									   r.Year == year
									  && r.Sku == sku
									  && r.PlaceId == PlaceId).ToList(),
			   records_ToCopy,
			   year_CopyTo, sku_CopyTo, PlaceId_CopyTo);
					//Очищаем место куда должно быть копирование
					context.StgPlanTer.RemoveRange(context.StgPlanTer.Where(r =>
						 r.Year == year_CopyTo
						&& r.Sku == sku_CopyTo
						&& r.PlaceId == PlaceId_CopyTo));


				}

				//если копируем весь год, тогда делаем выборку sku которые есть в году для копирования,
				//для каждого SKU бером список мес на который забиты места
				// теперь у нас есть записи которые можно скопировать модифицировав только 
				if (copy_full_year)
				{

					Copy_StgPlanTer_full_Year(context,
																 context.StgPlanTer.Where(r => r.Year == year).ToList(),
																 records_ToCopy,
																 year_CopyTo,
																 typeof(StgPlanTer).GetTypeInfo().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic));


					//Очищаем место куда должно быть копирование
					context.StgPlanTer.RemoveRange(context.StgPlanTer.Where(r =>
						 r.Year == year_CopyTo
					));
				}

				ViewBag.Message_text = "Записи успешно скопированы";
				ViewBag.Message_color = "blue";
				//Таблицы чисто с именами у нас в базе нет, так что берем его из DevicePlaces
				ViewData["PlaceName"] = context.DevicePlaces.Where(r => r.PlaceId == PlaceId).First().PlaceName;
				try
				{

					context.StgPlanTer.AddRange(records_ToCopy);
					await context.SaveChangesAsync();

				}
				catch (Exception e)
				{
				CustomExceptionHandler(e);
				ViewBag.Message_text = $"При копирование произошла ошибка: {e.InnerException.Message}";
					ViewBag.Message_color = "red";
				}
			
			return   View("Plan_TER_Copy",context.StgSkuDictionary.Select(r => r.SkuName));
		}
		[HttpPost]
		public IActionResult Action_selector(int year, string sku, int PlaceId, string actionName)
		{
			switch (actionName)
			{
				case "editing":
					return Plan_TER_Editing(year, sku, PlaceId);
				case "showing":
			return Plan_TER_Show_All<StgPlanTer>(year, sku, PlaceId);
				
				default:
					return Index();
		}
		}

		[HttpPost]
		public IActionResult Plan_TER_Show_All<T>(int year, string sku, int PlaceId)
			where T: class, Isku_Identify
		{
			string viewName = "";
			switch (typeof(T).Name)
			{
				case "StgSkuPlanTer":
					viewName = "SKU_Plan_TER_Show_All";
					break;
				case "StgPlanTer":
					viewName = "Plan_TER_Show_All";
					break;
				default:
					break;
			}
			bool yearCondition = year == 0 ? false : true;
			bool skuCondition = sku == null || sku == "" ? false : true;
			bool PlaceCondition = PlaceId == 0 ? false : true;
			ViewData["list_place_names"] = context.DevicePlaces.Select(r => new { r.PlaceId, r.PlaceName }).Distinct().ToDictionary(r => r.PlaceId, r => r.PlaceName);
			return View(viewName
				, context.Set<T>()
				.Where(r => (r.Year == year || !yearCondition)
				&& (r.Sku == sku || !skuCondition)
				&& (r.PlaceId == PlaceId || !PlaceCondition))
				.OrderBy(r => r.PlaceId)
				.ThenBy(r => r.Year)
				.ThenBy(r => r.Sku)
				.ThenBy(r => r.Month)
				.ToList());
		}
				

		[HttpPost]
		public IActionResult Plan_TER_Show_All(int year, string sku, int PlaceId)
		{
			bool yearCondition = year == 0 ? false : true;
			bool skuCondition = sku == null || sku == "" ? false : true;
			bool PlaceCondition = PlaceId == 0 ? false : true;

			return View("Plan_TER_Show_All"
				, context.StgPlanTer
				.Where(r => (r.Year == year || !yearCondition)
				&& (r.Sku == sku || !skuCondition))
				.OrderBy(r => r.Year)				
				.ThenBy(r => r.Sku)
				.ThenBy(r => r.Month)
				.ToList());
		}
		
		public IActionResult Plan_TER(string actionName)
		{
			ViewData["actionName"] = actionName;

			return View(
				context.StgSkuDictionary.Select(r => r.SkuName)
				);
		}
		

		//Ввод и обновление плана по SKU
		[HttpPost]
		public IActionResult Plan_TER_Editing(int year, string sku, int PlaceId
			)
		{
			string msg =
				((year == 0) ? "Нужно выбрать год внесения!<BR>" : "")
				+ ((sku == "" || sku == null) ? "Нужно выбрать SKU!<BR>" : "")
				+ (!(PlaceId == 1 || PlaceId == 2) ? "Нужно выбрать БДМ!<br>" : "")
			;
			if (msg != "")
			{
				ViewBag.Message_text = msg;
				ViewBag.Message_color = "red";
				return View("Plan_TER",
				context.StgSkuDictionary.Select(r => r.SkuName)
				);
			}


			
			
			
			ViewBag.year = year;
			ViewBag.sku = sku;
			ViewData["PlaceId"] = PlaceId;
			ViewData["PlaceName"] = context.DevicePlaces.Where(r => r.PlaceId == PlaceId).FirstOrDefault().PlaceName;
			return View("Plan_TER_Editing", context.StgPlanTer.FromSql(
				Sql_cmd_Plan_TER_Editing(year, sku, PlaceId)
				)
				.ToList());

		}

		[HttpPost]
		public IActionResult Plan_TER_Update([FromForm] List<StgPlanTer> StgPlanTers)
		{
			var curr_record = new StgPlanTer();
			//Если есть записи то обновляем их либо добовляем новые
			foreach (var item in StgPlanTers)
			{
				//ID генерируется из месяцов из-за алгоритма так что ищем запись не по ID,
				// а по уникальним параметрам
				curr_record = context.StgPlanTer.Where(
					r => r.Month == item.Month
					&& r.Sku == item.Sku
					&& r.Year == item.Year
					&& r.PlaceId == item.PlaceId
					).FirstOrDefault();
				if (curr_record != null)
				{
					FieldInfo[] curr_FieldInfo = typeof(StgPlanTer).GetFields(BindingFlags.Public |BindingFlags.Instance | BindingFlags.NonPublic);
					//Копируем данные записи кроме ID
					for (int i = 1; i < curr_FieldInfo.Length; i++)
					{
						curr_FieldInfo[i].SetValue(curr_record, curr_FieldInfo[i].GetValue(item));
					}
					

				}
				else { context.StgPlanTer.Add(item); }


			}

			try
			{
				
				ViewBag.Message_text = "Данные успешно изменены!";
				ViewBag.Message_color = "blue";
				context.SaveChanges();
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
				ViewBag.Message_text = "Были ошибки при изменении! Проверьте все строчки с изменеными данными";
				ViewBag.Message_color = "red";
				//throw;
			}
			ViewBag.year = StgPlanTers.First().Year;
			ViewBag.sku = StgPlanTers.First().Sku;
			ViewData["PlaceId"] = StgPlanTers.First().PlaceId;
			ViewData["PlaceName"] = context.DevicePlaces.Where(r=>r.PlaceId== StgPlanTers.First().PlaceId).FirstOrDefault().PlaceName;

			return View("Plan_TER_Editing", StgPlanTers);
			
		}

	}
}