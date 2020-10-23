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
using static STG_counters_admin_interface.Models.StaticMethods;
using static STG_counters_admin_interface.Models.SqlCmdStrings;

namespace STG_counters_admin_interface.Controllers
{
    public class ConfigurationsController : Controller
    {
		private ScaffoldContext context;
		public ConfigurationsController(ScaffoldContext ctx) => context = ctx;
		//страница по умолчанию
		public ViewResult Default_Index_action()
		{



			//Форматирование страниц(значения по умолчанию(10 строк на страницу))
			ViewBag.pages_Options = new QueryOptions();
			return View("Index");
		}
		[HttpPost]
		public IActionResult ChangeEtcParam(StgConfigParams param)
		{
		
				context.StgConfigParams.Update(param);
				TryWriteRecord("Запись успешна изменена", "При записи произошли ошибки!");
			
			
			
			return View("EtcParams", context.StgConfigParams);
		}
		public IActionResult EtcParams()
		{
			return View("EtcParams", context.StgConfigParams);
		}
		public IActionResult Index()
        {
            return View();
        }

		void TryWriteRecord(string success_msg,string fail_msg)
		{
			try
			{
				ViewBag.Message_text = success_msg;
				ViewBag.Message_color = "blue";
				context.SaveChanges();
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
				ViewBag.Message_text = fail_msg;
				ViewBag.Message_color = "red";
				//throw;
			}
		}

		public IActionResult SKUNames()
		{
			return View("SKUNames",context.StgSkuDictionary);
		}
		public IActionResult DeleteSKUName(int key)
		{
			var item = context.StgSkuDictionary.Find(key);

			if (item!=null)
			{
				context.StgSkuDictionary.Remove(item);
				TryWriteRecord("Запись успешна удалена", "При удалении записи произошли ошибки!");
				context.SaveChanges();
			};


			return View("SKUNames", context.StgSkuDictionary);
		}
		[HttpPost]
		public IActionResult SKUNames_Add(StgSkuDictionary record)
		{
			context.Update(record);
			TryWriteRecord("Запись успешно добавлена!","При записи были ошибки");
			
			
			return View("SKUNames", context.StgSkuDictionary);
		}
		[HttpPost]
		public IActionResult SKUNames_Update(StgSkuDictionary record)
		{
			
			
			var curr_record = context.StgSkuDictionary.Find(record.IdSkuDictionary);

			if (curr_record != null)
			{
				try
				{
					ViewBag.Message_text = "Запись успешно обновлена";
					ViewBag.Message_color = "blue";
					curr_record.SkuName = record.SkuName;
					curr_record.SkuNameFullName = record.SkuNameFullName;

					TryWriteRecord("Запись успешно обновлена", "При обновлении записи были ошибки");
				}
				catch (Exception e)
				{
					CustomExceptionHandler(e);
					ViewBag.Message_text = "При обновлении были ошибки"; ;
					ViewBag.Message_color = "red";
				}
				
			}
			return View("SKUNames", context.StgSkuDictionary);
		}
		public IActionResult SKUNames_Reload_from_Gamma()
		{
			//Выгружаем SKU из Gamma, и добавляем те sku которых нет в таблице STG_SKU_Dictionary
			context.Database.ExecuteSqlCommand(@"insert into STG_SKU_Dictionary
			select distinct Composition as SKU_Name, null as SKU_FullName from STG_Cache_CycleSpoolProduction 
			where Composition not in (select SKU_Name from STG_SKU_Dictionary )
			order by Composition");
			return View("SKUNames", context.StgSkuDictionary);
		}
		[HttpPost]
		public IActionResult UpdateDevicePlaces(DevicePlaces place)
		{
			//Сюда передается экземпляр DevicePlaces, содержащий свойства для изменения
			// ищем в базе запись для которой будем делать изменения, переносим изменения туда, и сохраняем
			//если в базе такой записи нет, тогда вычисляем новый индекс ключа,
			//и экземпляр с нужными свойствами пишем в базу

			DevicePlaces current_place = context.DevicePlaces.Find(place.DevicePlaceId);
			//с учетом того что ид мы выбираем из тега select имя места приходится получать отдельно
			place.PlaceName = context.vPlaces.FromSql(
				SqlCmdStrings.ConfigurationsController_Places_list)
				.Where(p=>p.PlaceID== place.PlaceId)
				.Select(p => p.PlaceName).FirstOrDefault();

			if (current_place == null|| place.DevicePlaceId==0)
			{
				
				place.DevicePlaceId =  context.DevicePlaces.Select(m => m.DevicePlaceId).Max() + 1;
				//корректируем день чтобы он был включительным
				place.EndDateOfLocation = place.EndDateOfLocation.AddHours(23).AddMinutes(59).AddSeconds(59);
				context.DevicePlaces.Add(place);
			}
			else
			{
				
				current_place.PlaceId = place.PlaceId;
				current_place.BeginDateOfLocation = place.BeginDateOfLocation;
				current_place.EndDateOfLocation = place.EndDateOfLocation.AddHours(23).AddMinutes(59).AddSeconds(59);
				current_place.PlaceName = place.PlaceName;
				current_place.Multiplier = place.Multiplier;
			}
			
			context.SaveChanges();

			//сохраняем список мест доступных для закрепления для устройства
			ViewBag.list_of_places = List_of_Places();

			ViewBag.Device_ID = place.DeviceId;
			return View("UpdateDevicePlaces", context.DevicePlaces.Where(p => p.DeviceId == place.DeviceId));

			


		}
		public IActionResult UpdateDevicePlaces(int key) {

			//сохраняем список мест доступных для закрепления для устройства
			ViewBag.list_of_places = List_of_Places();
			ViewBag.Device_ID = key;
			return View("UpdateDevicePlaces",context.DevicePlaces.Where(p=>p.DeviceId==key)

				);
		}

		[HttpGet]
		public IActionResult DevicesPlaces() {

			return View("DevicesPlaces", context.PaDevices
				//Теги навешиваются задом наперёд, так что это order by DeviceTypeName, DeviceName
				.OrderBy(d => d.DeviceName)
				.OrderBy(d => d.DeviceTypeName)
				
				.Include(d=>d.DevicePlaces)
				.AsEnumerable<PaDevices>());
		}
		[HttpGet]
		public IActionResult DeleteDevicePlace(int key)
		{
			try
			{
				context.DevicePlaces.Remove(context.DevicePlaces.Find(key));
				context.SaveChanges();
			}
			catch(Exception e) { CustomExceptionHandler(e); }

			return View("DevicesPlaces", context.PaDevices
				//Теги навешиваются задом наперёд, так что это order by DeviceTypeName, DeviceName
				.OrderBy(d => d.DeviceName)
				.OrderBy(d => d.DeviceTypeName)

				.Include(d => d.DevicePlaces)

				.AsEnumerable<PaDevices>());
		}




		public IActionResult UpdateCriteria(int key)
		{

			return View(context.StgCriteriaForDataFromDevices.Find(key));

		}

		[HttpPost]
		public IActionResult UpdateCriteria(StgCriteriaForDataFromDevices criteria)
		{
			//чтобы сохранить только нужные нам свойства, находи в базе такой же объект,
			//изменяем его свойства и делаем запрос на сохранение
			StgCriteriaForDataFromDevices current_criteria = context.StgCriteriaForDataFromDevices.Find(criteria.IdCriteria);
			current_criteria.MaxParam = criteria.MaxParam;
			current_criteria.MinParam = criteria.MinParam;
			context.SaveChanges();
			return RedirectToAction(nameof(CriteriaForDevices));
		}


		public IActionResult CriteriaForDevices()
		{
//Собираем список электрических устройств для которых нет критериев
			ViewBag.ListUnUsedDevices = context.PaDevices.FromSql(@"select d.* from PA_DEVICES d
			left
			join STG_CriteriaForDataFromDevices c on c.ID_DEVICE = d.DEVICE_ID
			where
			ID_Criteria is null 
			--and	DEVICE_TYPE_NAME in ('Elster A1800 (Электросчетчик)', 'СЭТ-4TM.03 (Электросчетчик)','Меркурий 230ART (Электросчетчик)')").ToDictionary(d=>d.DeviceId,d=>d.DeviceName);

			return View(context.StgCriteriaForDataFromDevices.FromSql(@"SELECT *
				FROM[dbo].[STG_CriteriaForDataFromDevices]").Include(c => c.IdDeviceNavigation));
		}

		[HttpGet]
		public IActionResult EmailReportReceivers()
		{
			return View(context.StgEmailReportsNames.Include(e=>e. StgEmailReportsReceivers));
		}

		[HttpPost]
		public IActionResult EmailReportReceivers(StgEmailReportsReceivers receiver)
		{
			StgEmailReportsReceivers receiver_to_Change = context.StgEmailReportsReceivers.Find(receiver.EmailReportsReceiversId);
			if (receiver_to_Change!=null)
			{
				receiver_to_Change.EmailName = receiver.EmailName;
				
			}
			else
			{
				try
				{
					context.StgEmailReportsReceivers.Add(receiver);
					context.SaveChanges();
				}
				catch (Exception e)
				{
					CustomExceptionHandler(e);
					ViewBag.Exception_Message = e.Message;
					//throw;
				}
				
			};
			
			

			return View("EmailReportReceivers", context.StgEmailReportsNames.Include(e => e.StgEmailReportsReceivers));
		}
		[HttpPost]
		public IActionResult AddEmailReceiver(int report_id, string receiver_email_name) 
{
			StgEmailReportsReceivers new_receiver = new StgEmailReportsReceivers();
			new_receiver.EmailName = receiver_email_name;
			//new_receiver.Id = null;
			new_receiver.ReportNotificationNameId = report_id;
			new_receiver.ReportNotificationName = context.StgEmailReportsNames.Find(report_id);
			try
			{
				context.StgEmailReportsReceivers.Add(new_receiver);
				context.SaveChanges();
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
				ViewBag.Exception_Message = e.InnerException.Message;
				//throw;
			}
			return View("EmailReportReceivers", context.StgEmailReportsNames.Include(e => e.StgEmailReportsReceivers));
		}
		[HttpPost]
		public IActionResult ChangeEmailReceiver(StgEmailReportsReceivers receiver)
		{
			StgEmailReportsReceivers receiver_to_change = context.StgEmailReportsReceivers.Find(receiver.EmailReportsReceiversId); ;
			receiver_to_change.EmailName = receiver.EmailName;
			//new_receiver.Id = null;
			receiver_to_change.EmailReportsReceiversId = receiver.EmailReportsReceiversId;
			receiver_to_change.ReportNotificationName = receiver.ReportNotificationName;
			try
			{
				
				context.SaveChanges();
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
				ViewBag.Exception_Message = e.InnerException.Message;
				//throw;
			}
			return View("EmailReportReceivers", context.StgEmailReportsNames.Include(e => e.StgEmailReportsReceivers));
		}

		[HttpPost]
		public IActionResult ChangeDelayTime(int report_id, int new_delay )
		{
			StgEmailReportsNames report_name_to_Change = context.StgEmailReportsNames.Find(report_id);
			if (report_name_to_Change != null)
			{
				report_name_to_Change.DelayTime = new_delay;

			};
			
			try
			{
				context.SaveChanges();
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
				ViewBag.Exception_Message = e.InnerException.Message; ;
				//throw;
			}


			return View("EmailReportReceivers", context.StgEmailReportsNames.Include(e => e.StgEmailReportsReceivers));
		}
		public IActionResult DeleteEmailReceiver(int key)
		{
			StgEmailReportsReceivers receiver = context.StgEmailReportsReceivers.Find(key);
			try
			{
				context.StgEmailReportsReceivers.Remove(receiver);
				context.SaveChanges();
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);

				ViewBag.Exception_Message = e.InnerException.Message; ;
				//throw;
			}
			return View("EmailReportReceivers", context.StgEmailReportsNames.Include(e => e.StgEmailReportsReceivers));
		}

		public IActionResult DevicePlacesNames()
		{
			
			return View("DevicePlacesNames", context.StgPlacesNames);
		}
		
		[HttpPost]
		public IActionResult UpdateDevicePlacesNames(StgPlacesNames place , StgPlacesNames new_place)
		{

			
			//Проверяем существование индификатора
			ViewBag.placeExists = 0;
			vPlaces existvPlace = null;
			try
			{
				if (new_place.PlaceId != 0)
				{
					existvPlace = context.vPlaces
						.FromSql(SqlCmdStrings.ConfigurationsController_Places_list)
						.Where(p => p.PlaceID == new_place.PlaceId).FirstOrDefault();
				}
				else
				{
					//делаем поиск во всех местах кроме текущей записи для изменения
					existvPlace = context.vPlaces.FromSql(
					Sql_cmd_UpdateDevicePlacesNames(place.PlacesNamesId.ToString()))
					.Where(p => p.PlaceID == place.PlaceId)
					.FirstOrDefault();
				}
				
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
				//throw;
			}
			
			if (existvPlace != null)
			{
				//место с таким ИД существует
				ViewBag.placeExists = 1;
				ViewBag.placeName = place.PlaceName;
				ViewBag.placeId = place.PlaceId;
				ViewBag.existPlaceName = existvPlace.PlaceName;
				//Для вывода используемых индификаторов
				ViewBag.ID_names_List = ExistPlacesIDandNames(place);

				return View("DevicePlacesNames", context.StgPlacesNames);
			}

			//В случае измения записи
			if (place.PlaceId!=0)
			{
				StgPlacesNames place_to_change = context.StgPlacesNames.Find(place.PlacesNamesId);
				place_to_change.PlaceId = place.PlaceId;
				place_to_change.PlaceName = place.PlaceName;
				
			}
			else if (new_place.PlaceId!=0)
			{
				context.StgPlacesNames.Add(new_place);

			}
			try
			{
				context.SaveChanges();
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
				//throw;
			}
			return View("DevicePlacesNames", context.StgPlacesNames);
		}

		
		public IActionResult DeleteDevicePlacesNames(int key)
		{
			try
			{
				context.StgPlacesNames.Remove(context.StgPlacesNames.Find(key));
				context.SaveChanges();
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
				//throw;
			}
			return View("DevicePlacesNames", context.StgPlacesNames);

		}
		public string ExistPlacesIDandNames(StgPlacesNames place)
		{
			IDictionary<int, string> IDPlacesNames = context.vPlaces
			.FromSql(Sql_cmd_ExistPlacesIDandNames(place.PlacesNamesId.ToString()))
			.OrderBy(p => p.PlaceID)
			.ToDictionary(p => p.PlaceID, p => p.PlaceName);

			string ID_names_List = "";
			foreach (KeyValuePair<int, string> item in IDPlacesNames)
			{
				ID_names_List += "ид: " + item.Key + ", место: " + item.Value + ";<br/>";
			}
			return ID_names_List;
		}
		[HttpPost]
		public IActionResult AddCriteriaForDevices(StgCriteriaForDataFromDevices criteria)
		{
			
			try
			{
				criteria.IdCriteria = context.StgCriteriaForDataFromDevices.Max(c => c.IdCriteria) + 1;
				context.StgCriteriaForDataFromDevices.Add(criteria);
				context.SaveChanges();
			}
			catch (Exception e)
			{
				CustomExceptionHandler(e);
				ViewBag.exception = e.Message;
				//throw;
			}
			return RedirectToAction(nameof(CriteriaForDevices));
		}
		//Возвращаем словарь соотвествия ID и имен мест собраных из вьшки vPlaces 
		//и долнительной таблицы STG_PlacesNames
		public Dictionary<int, string> List_of_Places()
		{

			return context.vPlaces
			.FromSql(SqlCmdStrings.ConfigurationsController_Places_list)
			.OrderBy(p => p.PlaceName)
			.ToDictionary(p => p.PlaceID, p => p.PlaceName);
		}
	}
	
	}