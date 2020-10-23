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


namespace STG_counters_admin_interface.Controllers
{
    public class ElectricEnergyPlacesController : Controller
    {
		private ScaffoldContext context;
		public ElectricEnergyPlacesController(ScaffoldContext ctx) => context = ctx;

		string filter_by_devices_sql_script;
		string sql_string;
		string filter_by_time;
		string paged_sql_script_part;
		//int count_of_records;

		//страница по умолчанию
		public ViewResult Default_Index_action()
		{

			//Строим дерево устройств и мест
			ViewBag.Json = JsonConvert.SerializeObject(StaticMethods.BuildTree(context));
			//Форматирование страниц(значения по умолчанию(10 строк на страницу))
			ViewBag.pages_Options = new QueryOptions();
			return View("Index");
		}


		[HttpGet]
		public ViewResult Index()
		{

			return Default_Index_action();
		}

		[HttpPost]
		public ActionResult FilteredRecords(
			//Получаем список узлов(устройств) где стоят галочки
			string selectedItems,
			//Получаем номер страницы где мы сейчас, и количестов строк на страницу, и пр. параметры косающиеся страницы
			QueryOptions pages_Options,
			//Дата с которой будем собирать записи
			DateTime start_Date,
			//Дата по которую будем собирать записи
			DateTime end_Date,
			//Показывать ли записи за период
			int selectedPeriod

			)
		{

			//Если выраны галочки то делаем фильтры
			if (selectedItems != null && selectedItems.Length > 1)
			{
				if (StaticMethods.Sql_injection_check(selectedItems) > 0)
				{
					return RedirectToAction("Index");
				}

				//если задан период, выставляем даты
				StaticMethods.Set_period(ref start_Date, ref end_Date, selectedPeriod);

				//Если не ввели начальную дату
				StaticMethods.Start_date_check(ref start_Date);

				//Если не ввели конечную дату
				StaticMethods.End_date_check(ref end_Date);

				//Сохраняем парметры фильтров
				ViewBag.start_Date = start_Date;
				ViewBag.end_Date = end_Date;
				
				//Сохраняем списков выбраных устройств в ViewBag
				ViewBag.Selected_Items = selectedItems;



				//фильтр по устройства где стояли галочки
				filter_by_devices_sql_script = " WHERE b.ID_DEVICE IN " + selectedItems;


				filter_by_time = " and "+StaticMethods.Filter_by_time(start_Date, end_Date, "RECORD_TIME");

				paged_sql_script_part = " order by Date  OFFSET " + ((pages_Options.CurrentPage - 1) * pages_Options.PageSize).ToString() + " ROWS FETCH NEXT " + pages_Options.PageSize + " ROWS ONLY";
				
				sql_string = "SELECT ROW_NUMBER() OVER(ORDER BY a.DEVICE_NAME ASC) AS 'ID', " +
  "CASE WHEN " +

	"a.DEVICE_TYPE_NAME = 'СПГ761 (газовый корректор)' " +
	 " THEN SUM(d.MEASURE_VALUE * g.Multiplier)  END AS GasValue, " +
  "CASE WHEN" +
	"(a.DEVICE_TYPE_NAME IN('СПТ961.1(.2) (тепловычислитель)') AND c.PARAMETER_NAME = 'Тепловая энергия, тп1') " +
	 " THEN SUM(d.MEASURE_VALUE * g.Multiplier) END AS SteamValue, " +
   "CASE WHEN " +
	"(a.DEVICE_TYPE_NAME IN('СПТ961.1(.2) (тепловычислитель)') AND c.PARAMETER_NAME = 'Масса теплоносителя, тп1') " +
	 " THEN SUM(d.MEASURE_VALUE * g.Multiplier) END AS SteamMassValue, " +
  "CASE WHEN " +
	" a.DEVICE_TYPE_NAME IN('Elster A1800 (Электросчетчик)' " +
	 "  , 'СЭТ-4TM.03 (Электросчетчик)','Меркурий 230ART (Электросчетчик)') " +
	  "THEN SUM(d.MEASURE_VALUE * g.Multiplier) END AS Value, " +
	 "g.PlaceID, " +
	 "dbo.TruncTime(f.RECORD_TIME) AS Date, a.DEVICE_NAME " +
"FROM PA_DEVICES a " +
"JOIN PA_ADAPTERS b ON b.ID_DEVICE = a.DEVICE_ID AND b.ADAPTER_LOGICAL_ID = 0 " +
"JOIN PA_ADAPTER_PARAMETERS c ON c.ID_ADAPTER = b.ID_ADAPTER AND a.DEVICE_ID = c.ID_DEVICE " +
 "  AND( " +
  " (a.DEVICE_TYPE_NAME = 'Elster A1800 (Электросчетчик)' AND c.PARAMETER_NAME = 'Канал 1') " +
  " OR((a.DEVICE_TYPE_NAME = 'СЭТ-4TM.03 (Электросчетчик)' OR a.DEVICE_TYPE_NAME = 'Меркурий 230ART (Электросчетчик)') AND c.PARAMETER_NAME = 'A+ (Энергия активная +)') " +
  " OR(a.DEVICE_TYPE_NAME = 'СПГ761 (газовый корректор)' AND c.PARAMETER_NAME = 'Объем газа при ст. усл., тп1') " +
  " OR(a.DEVICE_TYPE_NAME = 'СПТ961.1(.2) (тепловычислитель)' " +

	" AND c.PARAMETER_NAME IN('Масса теплоносителя, тп1', 'Тепловая энергия, тп1'))) " +
"JOIN PA_RECORDS f ON f.ID_ADAPTER = b.ID_ADAPTER AND f.ID_DEVICE = b.ID_DEVICE " +
"JOIN PA_DATA d ON d.ID_RECORD = f.ID_RECORD AND d.ID_PARAMETER = c.ID_PARAMETER " +
"JOIN DevicePlaces g ON g.Device_ID = a.DEVICE_ID " +
filter_by_devices_sql_script +
filter_by_time+
"GROUP BY a.DEVICE_TYPE_NAME, c.PARAMETER_NAME, dbo.TruncTime(f.RECORD_TIME), a.DEVICE_NAME, g.PlaceID ";
				
				PagedList<ElectricEnergyPlaces> chunck_of_data_ElectricEnergyPlaces = new PagedList<ElectricEnergyPlaces>(context.ElectricEnergyPlaces.FromSql(sql_string + paged_sql_script_part), context.ElectricEnergyPlaces.FromSql(sql_string).Count(), pages_Options);

				return View(//возвращаем в модель отфильтрованный список записей Records, с присоединными используемыми таблицами
						chunck_of_data_ElectricEnergyPlaces

					);
			}
			//если нет выбранных устройств, то просто возвращаем фильтр
			else
			{
				ViewBag.Show_warning_unselected_devices = 1;
				return Default_Index_action();
			}



		}

	}
}
