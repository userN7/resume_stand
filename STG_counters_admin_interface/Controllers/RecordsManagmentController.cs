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


//using STG_counters_admin_interface.Models


namespace STG_counters_admin_interface.Controllers
{
	public class RecordsManagmentController : Controller
	{
		private ScaffoldContext context;
		public RecordsManagmentController(ScaffoldContext ctx) => context = ctx;
		//public PagedList<PaRecords> Filtred_records;

		string wrong_data_sql_script_part1;
		string wrong_data_sql_script_part2;
		string filter_by_devices_sql_script;
		string sql_string;
		string filter_by_time;
		string paged_sql_script_part;
		//int count_of_records;
		

		
	//страница по умолчанию
	public ViewResult Default_Index_action() {
			
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
			int selectedPeriod,
			//Показывать только записи с подозрением на неправильные данные
			string showWrongRecords,
            int export_excel=0

            )
		{
			
			//Если выраны галочки то делаем фильтры
			if (selectedItems != null && selectedItems.Length>1)
			{
				if (StaticMethods.Sql_injection_check(selectedItems)>0)
				{
					return RedirectToAction("Index");
				}

				//если задан период, выставляем даты
				StaticMethods.Set_period(ref start_Date,ref end_Date, selectedPeriod);

				//Если не ввели начальную дату
				StaticMethods.Start_date_check(ref start_Date);

				//Если не ввели конечную дату
				StaticMethods.End_date_check(ref end_Date);

				//Сохраняем парметры фильтров
				ViewBag.start_Date = start_Date;
				ViewBag.end_Date = end_Date;
				ViewBag.showWrongRecords = showWrongRecords;
				//Сохраняем списков выбраных устройств в ViewBag
				ViewBag.Selected_Items = selectedItems;
				
				

				//фильтр по устройства где стояли галочки
				filter_by_devices_sql_script = " WHERE ID_DEVICE IN " + selectedItems;
				//filter_by_devices_sql_script = " WHERE a.ID_DEVICE IN " + selectedItems+ " and b.MEASURE_VALUE <> 0";

				filter_by_time = " and "+StaticMethods.Filter_by_time(start_Date, end_Date.AddDays(1), "RECORD_TIME");

				paged_sql_script_part = " order by ID_RECORD OFFSET " + ((pages_Options.CurrentPage - 1) * pages_Options.PageSize).ToString() + " ROWS FETCH NEXT " + pages_Options.PageSize + " ROWS ONLY";


				
						if (showWrongRecords == "on")
						{
							wrong_data_sql_script_part1 = " join PA_DATA b on a.ID_RECORD = b.ID_RECORD";
							wrong_data_sql_script_part2 = " group by a.ID_RECORD, a.ID_DEVICE, a.ID_ADAPTER, a.METHOD_TYPE, a.RECORD_TIME, a.STATUS,a.RECORD_INDEX having sum(b.MEASURE_VALUE) = 0 or sum(b.MEASURE_VALUE)> 1000000 ";

						}
						else
						{
							wrong_data_sql_script_part1 = "";
							wrong_data_sql_script_part2 = "";
						}

						


						//формируем итоговый запрос для основной таблицы PaRecords, без нарезки на страницу
						sql_string = "select a.* from PA_RECORDS a " +
							//sql_string = "select a.*,b.ID_PARAMETER, b.MEASURE_VALUE,b.PARAM_VALUE from PA_RECORDS a join PA_DATA b on a.ID_RECORD = b.ID_RECORD " + 
							wrong_data_sql_script_part1 +
							filter_by_devices_sql_script +
							filter_by_time +
							wrong_data_sql_script_part2
							;

						//получаем кусок данных из таблицы для вывода
						PagedList<PaRecords> chunck_of_data_paRecords = new PagedList<PaRecords>(
									context.PaRecords

							.FromSql(sql_string + paged_sql_script_part
							)
							//фильтр по времени
							//.Where(r =>

							// //AdapterLogicalId==0 означает часовой архив срезов, то что используется для посчетов данных с счетчика
							// //&&
							// r.Id.AdapterLogicalId == 0)

						

							//Влючаем таблицу PaAdapters, там хранится референс на таблицу PaDevice, и из этой таблицы забираем имя устройства
							.Include(r => r.Id)
							//Влючаем таблицу PaDevices, чтобы получить имя устройства
							.ThenInclude(a => a.IdDeviceNavigation)
							////Влючаем таблицу PaDevices, чтобы получить ограничения на показания датчиков
							//.ThenInclude(d=>d.StgDevsApproprMeasuresRange)

							//Влючаем таблицу PaAdapterParameters, там хранится названия счетчиков по которым выводится показания
							.Include(r => r.Id.PaAdapterParameters)
							//Влючаем таблицу PaData, там хранятся записи данных со счетчика
							.Include(r => r.PaData)
							.OrderBy(r=>r.RecordTime),
							//передаем итоговое количество записей для посчета количества страниц
									context.PaRecords.FromSql(sql_string).Count()
							
							, pages_Options);



                if (export_excel==1)
                {
                    

                    
                    //Добавить информацию для вывода в случае успеха, пути куда выгрузили фалй, либо сообщение об ошибке
                    //ExcelExport<PaRecords> export = new ExcelExport<PaRecords>(
                    ////Строки на вывод
                    //new Dictionary<int, string> { { 0, "ИД Записи" }, { 4, "Время записи" } },
                    ////Список обектов для экспорта
                    //context.PaRecords.FromSql(sql_string),
                    ////название файла
                    //"Записи с устройств");
                    //ViewBag.Excel_export_message_list = export.MessageList;
                }
						return View(//возвращаем в модель отфильтрованный список записей Records, с присоединными используемыми таблицами
								chunck_of_data_paRecords

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