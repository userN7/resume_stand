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
using static STG_counters_admin_interface.Models.StaticMethods;
namespace STG_counters_admin_interface.Controllers
{
    public class ShowRecordsByStatusController : Controller
    {
		private ScaffoldContext context;
		
		public ShowRecordsByStatusController(ScaffoldContext ctx) => context = ctx;
		public IActionResult Index()
		{
			//string[] device_types =   {  "Elster A1800 (Электросчетчик)"
			//		, "СЭТ-4TM.03 (Электросчетчик)"
			//,"Меркурий 230ART (Электросчетчик)"};
			return View(context.PaDevices
				//Выводим устройств с известным типом
				.Where(d=>StaticMethods.DeviceTypesListwithHalfHourlyIndication.Contains(d.DeviceTypeName)
				||StaticMethods.DeviceTypesListwithHourlyIndication.Contains(d.DeviceTypeName))
				.OrderBy(d=>d.DeviceName)
			);
        }
		public void AddRecords(int DEVICE_ID, double MEASURE_VALUE, DateTime RECORD_TIME)
		{

			//int new_ID_RECORD = 0;
			int currIdAdapt = context.PaAdapters.Where(a => a.AdapterLogicalId == 0 && a.IdDevice == DEVICE_ID).First().IdAdapter;
			string deviceType = context.PaDevices.Where(r => r.DeviceId == DEVICE_ID).FirstOrDefault().DeviceTypeName;
			int currIdParam = context.PaAdapterParameters.Where(p => p.IdAdapter == currIdAdapt && (p.ParameterName == DeviceParam(deviceType))).First().IdParameter;
			//int currIdParam = context.PaAdapterParameters.Where(p => p.IdAdapter == currIdAdapt && (p.ParameterName == "Канал 1" || p.ParameterName == "A+ (Энергия активная +)")).First().IdParameter;
			PaData data = new PaData();
			//сохраняем чтобы записать потом что изменилось в записях, и старые значения чтобы востановить
			double old_measure_value = 0;
			double new_measure_value = 0;
			double old_param_value = 0;
			double new_param_value = 0;
			PaRecords paRecord = new PaRecords();
			
			

				paRecord.IdDevice = DEVICE_ID;
				paRecord.IdAdapter = currIdAdapt;
				paRecord.RecordTime = RECORD_TIME;
				//paRecord.IdRecord = context.PaRecords.Max(r => r.IdRecord) + 1;
				paRecord.Status = 1;
				paRecord.RecordIndex = 0;
				paRecord.MethodType = 0;



				data.IdParameter = currIdParam;
				data.MeasureValue = MEASURE_VALUE;
				new_measure_value = MEASURE_VALUE;
				//Значение параметра зависит о значения Measure/MulKoeff= paramValue
				// получить уникальное значение MulKoeff
				data.ParamValue = MEASURE_VALUE / (context.PaAdapterParameters.Where(p => p.IdParameter == currIdParam).First().MulKoeff);
				new_param_value = data.ParamValue;
				paRecord.PaData.Add(data);


				context.PaRecords.Add(paRecord);

			//context.PaData.Add(data);


			//context.Database.ExecuteSqlCommand(@"SET IDENTITY_INSERT [PowerDB].[dbo].[PA_RECORDS] ON;");

			AddLogRecord(paRecord.IdRecord
			, 2 //1 for changed, 2 for added
			, old_measure_value
			, new_measure_value
			, old_param_value
			, new_param_value
			);
				

			
		}
		public IActionResult AddGroupRecordsByStatus(double avarageValue, string selectedItemsToAdd, string serializedModel
			, string start_Date
			, string end_Date
			, int searchOption
			, int device_id=0
			)
		{
			//Принцип получаем объекты из модели, список индификаторов выбраных записей для изменения,
			//и список новых значений для них, а так же среднее значение. Для каждого индификатора выбираем запись, 
			//и на её основе делаем изменения в PA_RECORDS и PA_DATA
			//if (selectedItems==null|| selectedItems =="")
			//{
			//	return View("PaRecordsByFilterByStatus",ViewData.Model);
			//}
			//Убираем последнюю запятую
			//checkedIds = checkedIds.Substring(0, checkedIds.Length - 1);
			string[] listIds = selectedItemsToAdd.Split(',');
			//Убираем последнюю запятую
			//checkedMeasureValues = checkedMeasureValues.Substring(0, checkedMeasureValues.Length - 1);
			//string[] listMeasureValues = checkedMeasureValues.Split(';');

			//	double measureValue;

			IEnumerable<PaRecordsByFilter> recordsList = JsonConvert.DeserializeObject<IEnumerable<PaRecordsByFilter>>(serializedModel);


			for (int i = 0; i < listIds.Length; i++)
			{
				PaRecordsByFilter record = recordsList.Single(r => r.ID.ToString() == listIds[i]);
				//	measureValue = 0;

				//пробуем среднее значение
				//если оно не нулевое то пишем его
				if (avarageValue > 0)
				{
					//делаем изменения в PA_RECORDS и PA_DATA
					AddRecords(record.DEVICE_ID, avarageValue, record.RECORD_TIME);
				}
				



			}

				try
				{
				ViewBag.result_add= context.SaveChanges();
				}
				catch (Exception e)
				{
				CustomExceptionHandler(e);
				//throw;
			}

			return Prepare_View_PaRecordsByFilterByStatus(searchOption, start_Date, end_Date, device_id); 
		}
		public IActionResult EditGroupRecordsByStatus(double avarageValue, double multiplier, string selectedItems
			, string start_Date
			, string end_Date
			, int searchOption
			, int device_id=0
			) {
			//Принцип получаем объекты из модели, список индификаторов выбраных записей для изменения,
			//и список новых значений для них, а так же среднее значение. Для каждого индификатора выбираем запись, 
			//и на её основе делаем изменения в PA_RECORDS и PA_DATA
			//if (selectedItems==null|| selectedItems =="")
			//{
			//	return View("PaRecordsByFilterByStatus",ViewData.Model);
			//}
			//Убираем последнюю запятую
			//checkedIds = checkedIds.Substring(0, checkedIds.Length - 1);
			string[] listIds = selectedItems.Split(',');
			//Убираем последнюю запятую
			//checkedMeasureValues = checkedMeasureValues.Substring(0, checkedMeasureValues.Length - 1);
			//string[] listMeasureValues = checkedMeasureValues.Split(';');

		//	double measureValue;

			IEnumerable<PaRecords> recordsList = context.PaRecords.Where(r=>
			r.RecordTime >= DateTime.Parse(start_Date)
			&&r.RecordTime<= DateTime.Parse(end_Date) && listIds.Contains(r.IdRecord.ToString())).AsEnumerable();
			

			foreach (PaRecords record in recordsList)
			{
			//	PaRecords record = recordsList.Single(r => r.IdRecord.ToString() == listIds[i]);
			//	measureValue = 0;
			
				//пробуем среднее значение
				//если оно не нулевое то пишем его
				
					EditRecords(record, avarageValue, multiplier);
				



			}
			try
			{
			ViewBag.result_edit = context.SaveChanges();
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
				//throw;
			}

			return Prepare_View_PaRecordsByFilterByStatus(searchOption, start_Date, end_Date, device_id); ;
		}

		public string DeviceParam(string deviceType)
		{
			string paramName = "";
			switch (deviceType)
			{
				case "Elster A1800 (Электросчетчик)":
					paramName = "Канал 1";
					break;
				case "Меркурий 230ART (Электросчетчик)":
				case "СЭТ-4TM.03 (Электросчетчик)":
					paramName = "A+ (Энергия активная +)";
					break;
				case "СПГ761 (газовый корректор)":
					paramName = "Объем газа при ст. усл., тп1";
					break;
				case "СПТ961.1(.2) (тепловычислитель)":
					paramName = "Тепловая энергия, тп1";
					break;
				default:
					break;
			}
			return paramName;
		}

		

		public void EditRecords(PaRecords record, double avarageValue, double multiplier ) {
			double MEASURE_VALUE;
			
			int currIdAdapt = record.IdAdapter;
			string deviceType = context.PaDevices.Where(r => r.DeviceId == record.IdDevice).FirstOrDefault().DeviceTypeName;						
			int currIdParam = context.PaAdapterParameters.Where(p => p.IdAdapter == currIdAdapt && (p.ParameterName == DeviceParam(deviceType))).First().IdParameter;
			
			//сохраняем чтобы записать потом что изменилось в записях, и старые значения чтобы востановить
			double old_measure_value=0;
			double new_measure_value = 0;
			double old_param_value = 0;
			double new_param_value = 0;
			
			//если записи нет
			
				//PaRecords paRecord = context.PaRecords.Where(a => a.IdDevice == DEVICE_ID).First();

				PaData paData = context.PaData
				.Where(d => d.IdRecord == record.IdRecord)

					.Where(d => d.IdParameter == currIdParam)
					//получаем индификатор адаптера для данного устройства


					.First();
				MEASURE_VALUE = paData.MeasureValue;
				if (avarageValue>0)
				{
					MEASURE_VALUE = avarageValue;
				}
				else
					if (multiplier>0)
				{
					MEASURE_VALUE = paData.MeasureValue * multiplier;
				}
				old_measure_value = paData.MeasureValue;
				paData.MeasureValue = MEASURE_VALUE;
				new_measure_value = MEASURE_VALUE; ;
				//Значение параметра зависит о значения Measure/MulKoeff= paramValue
				// получить уникальное значение MulKoeff
				old_param_value = paData.ParamValue;
				paData.ParamValue = MEASURE_VALUE / (context.PaAdapterParameters.Where(p => p.IdParameter == currIdParam).First().MulKoeff);
				new_param_value = paData.ParamValue;
			    context.PaData.Update(paData);

			AddLogRecord(record.IdRecord
			, 1 //1 for changed, 2 for added
			, old_measure_value
			, new_measure_value
			, old_param_value
			, new_param_value
			);

		
	
		}
		
			public ViewResult ShowRecordsByStatus(DateTime start_Date,
			//Дата по которую будем собирать записи
			DateTime end_Date,
			//Показывать ли записи за период
			int selectedPeriod
			, int device_id =0
			, int export_excel = 0
			, string onlyLastHour = null
			, int searchOption =1
			)
		{
			StaticMethods.Check_dates(ref start_Date, ref end_Date,selectedPeriod);
			////Если по текущую дату то делаем отскок на два часа, потому что данные могли еще не попасть в систему
			//if (end_Date.Date==DateTime.Now.Date)
			//{
			//	end_Date.AddHours(-2);
			//}
			//end_Date. .Minute = 0;
			//string end_Date_pattern = "";
			if (onlyLastHour!=null)
			{
				start_Date = DateTime.Now.AddHours(-1);
				end_Date = DateTime.Now;
			
			}

			//string sql_string = "";
			//Если крайняя дата сегодня, то режем лишние часы 
			if (end_Date.ToShortDateString() == DateTime.Now.ToShortDateString())
			{
				return Prepare_View_PaRecordsByFilterByStatus(searchOption, start_Date.ToString("yyyy-MM-dd HH:00:00"), DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00"), device_id);
			}
			//иначе просто загоняем даты в рамки 00:00:00 и 23:59:59
			else
			{
				return Prepare_View_PaRecordsByFilterByStatus(searchOption, start_Date.ToString("yyyy-MM-dd 00:00:00"), end_Date.ToString("yyyy-MM-dd 23:59:59"), device_id);
			}
			//return Prepare_View_PaRecordsByFilterByStatus(searchOption, start_Date.ToString("yyyy-MM-dd HH:00:00"), end_Date.ToString("yyyy-MM-dd HH:00:00"), device_id);

		}

		public ViewResult Default_Index_action()
		{

			
		
			//Форматирование страниц(значения по умолчанию(10 строк на страницу))
			ViewBag.pages_Options = new QueryOptions();
			return View("Index");
		}
		

		public ViewResult Prepare_View_PaRecordsByFilterByStatus(int searchOption, string start_Date, string end_Date, int device_id)
		{
			ViewBag.Current_action = "EditGroupRecordsByStatus";
			ViewBag.searchOption = searchOption;
			string sql_string = StaticMethods.sqlString_by_searchOptions(searchOption, start_Date, end_Date, device_id,context,this);


			// по умолчанию сортировка e.PlaceName, a.DEVICE_NAME,  t.RECORD_TIME

			List<PaRecordsByFilter> records_set = context.PaRecordsByFilter.FromSql(sql_string).OrderBy(r=>r.PlaceName).ThenBy(r=>r.DEVICE_NAME).ThenBy(r=>r.RECORD_TIME).ToList();
				
			
			List<string> places_with_wrong_rec = records_set.Where(r => r.RecordStatus == "Не походит по критерию").Select(r => r.PlaceName).Distinct().ToList();
			List<int> devices_with_wrong_rec = records_set.Where(r => r.RecordStatus == "Не походит по критерию").Select(r => r.DEVICE_ID).Distinct().ToList();
			List<string> dates_with_wrong_rec = records_set.Where(r => r.RecordStatus == "Не походит по критерию").Select(r=> r.DEVICE_ID.ToString()+";"+ r.RECORD_TIME.ToShortDateString()).Distinct().ToList();
			
			//Передаем дерево из узлов для постоения JSTree
			ViewBag.Json = JsonConvert.SerializeObject(StaticMethods.BuildTree(records_set, places_with_wrong_rec, devices_with_wrong_rec, dates_with_wrong_rec, device_id));
			//return View("PaRecordsByFilterByStatus", records_set.AsEnumerable()
			//	);
			ViewBag.start_Date = start_Date;
			ViewBag.end_Date = end_Date;
			ViewBag.device_id = device_id;
			return View("PaRecordsByFilterByStatus", records_set
				);
		}

		public void AddLogRecord(int idRecord_to_log
			,int status_to_log //1 for changed, 2 for added
			,double old_measure_value
			,double new_measure_value
			,double old_param_value
			,double new_param_value
			)
		{


			StgPaRecordsChangesLog log_record = new StgPaRecordsChangesLog();

			log_record.IdRecord = idRecord_to_log;
			log_record.IdChangesStatus = status_to_log;
			log_record.DateChange = DateTime.Now;
			log_record.OldMeasureValue = old_measure_value;
			log_record.NewMeasureValue = new_measure_value;
			log_record.OldParamValue = old_param_value;
			log_record.NewParamValue = new_param_value;

			context.StgPaRecordsChangesLog.Add(log_record);
		}
	}
}