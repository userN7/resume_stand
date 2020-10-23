using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using STG_counters_admin_interface.Models.PowerDB_data_classes;
using STG_counters_admin_interface.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using OfficeOpenXml.Drawing;
using System.Drawing;
using System.Collections;
using static STG_counters_admin_interface.Models.Constants;
using static STG_counters_admin_interface.Models.StaticMethods;

namespace STG_counters_admin_interface.Models
{

	class ExcelExport
	{
		//Куда будем писать файл
		public MemoryStream mStream;
		private int row = 1;
		private int col = 1;
		public void New_line(int col = 1, int row = 1)
		{
			this.col = col;
			this.row = this.row + row;
		}
		public void IncreaseCol(int inc)
		{
			col = col + inc;
		}
		public void IncreaseRow(int inc)
		{
			row = row + inc;
		}
		public int Col
		{
			get
			{ return col; }
			set { col = value; }
		}
		public DateTime StartTime { get; }
		public int Row
		{
			get
			{ return row; }
			set { row = value; }
		}
		ExcelPackage eP { get; set; }
		public ExcelWorksheet Sheet { get; set; }
		public List<string> MessageList = new List<string> { };
		public void AddExcelCellList<T>(List<T> list)
		{
			foreach (var item in list)
			{
				AddExcelCell(item);
			}
		}
		public FileContentResult SaveToBrowser(string name="")
			{
			this.Save_excel_file_to_Stream();
			return new FileContentResult(this.mStream.ToArray(),
					"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
		{
			FileDownloadName = $"{name}_{DateTime.UtcNow.ToShortDateString()}.xlsx"
			}; }
		public  void AddImageToExcel(Image image, int width, int height)
		{
			ExcelPicture picture = null;
			if (image != null)
			{
				picture = this.Sheet.Drawings.AddPicture("pic"+ this.Row.ToString()+ this.Col.ToString(), image);
				picture.From.Column = this.Col;
				picture.From.Row = this.Row;
				picture.SetSize(width, height);
			}
		}
		public void Add_head_reportName(string Report_name, DateTime start_Date, DateTime end_Date)
		{
			Sheet.Cells[row, col].Value = Report_name;
			Sheet.Cells[row, col].Style.Font.Size = 22;
			row++;
			row++;
			Sheet.Cells[row, col].Value = "Начало периода: " + start_Date.ToShortDateString();
			row++;
			Sheet.Cells[row, col].Value = "Конец периода: " + end_Date.ToShortDateString();
			row++;
			row++;}
		void merge_cells(ExcelExport exportToExcel, int count_to_merge)
		{
			exportToExcel.Sheet.Cells[exportToExcel.Row, exportToExcel.Col, exportToExcel.Row, exportToExcel.Col + count_to_merge].Merge = true;
		}
		public void Add_head(string[] Excel_Head, int height = 0, int width = 0, int col_adder =0) {



			//Переходим переменные для управления позицией в листе

			merge_cells(this, col_adder);
			//в Excel_Head передается пара, номер поля для вывода и его название для вывода
			//сначала заполняем шапку
			foreach (string field in Excel_Head)
			{
				AddExcelCell(field, col_adder, height, width, bold:true);
				Sheet.Cells[this.Row, this.Col-1].AutoFitColumns();
			}



			//row++;
		}

		public void AddExcelCell<T>(T cell_value, int col_adder = 0, int height = 0, int width = 0, string format = null, int col_to_merge = 0, bool outline = true, bool horizAlign = true, bool bold = false)
		{
			

			if (height > 0)
			{
				Sheet.Row(Row).Height = height;
			}
			Sheet.Cells[row, col].Style.WrapText = true;
			Sheet.Cells[row, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
			if (horizAlign)
			{
				Sheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
			}
			
			Sheet.Cells[row, col].Value = cell_value;
			if (col_to_merge!=0)
			{
				Sheet.Cells[row, col+col_to_merge].Merge = true;
			}
			if (outline)
			{
				OutlineExcelCell(col_adder);
			}
			if (bold)
			{
				Sheet.Cells[row, col, row, col + col_adder].Style.Font.Bold = true;
			}
			
			Sheet.Cells[row, col].Style.Numberformat.Format = format;
			if (width > 0)
			{
				Sheet.Column(Col).Width = width;
			}
			col++;
			col += col_adder;
		}

		public void OutlineExcelCell(int col_adder=0)
		{

			Sheet.Cells[row, col,row,col + col_adder].Style.Border.Top.Style = ExcelBorderStyle.Thin;
			Sheet.Cells[row, col, row, col + col_adder].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
			Sheet.Cells[row, col + col_adder].Style.Border.Right.Style = ExcelBorderStyle.Thin;
			Sheet.Cells[row, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
			


		}

		public void OutlineExcelCells()
		{
			using (var cells = Sheet.Cells[Sheet.Dimension.Address])
			{
				cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
				cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
				cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
//				cells.AutoFitColumns();
			}
		}

		public void Save_excel_file_to_Stream() {
			//создаем поток для записи excel
			mStream = new MemoryStream();
			//сохраняем файл
			eP.SaveAs(mStream);
		}
		public ExcelExport()
		{

			

			//создаем болванку для экселя
			StartTime = DateTime.Now;
			eP = new ExcelPackage();
			Sheet = eP.Workbook.Worksheets.Add("Выгрузка");
			Sheet.DefaultColWidth = 15;
			//eP.Workbook.Properties.Author = "";
			//eP.Workbook.Properties.Title = "";
			//eP.Workbook.Properties.Company = "";

			//добавляем страницу


			//foreach (T item in Excel_body)
			//{


			//	col = 1;

			//	////Вывод всех полей в классе
			//	//foreach (FieldInfo field in myFieldInfo)
			//	//{
			//	//    try
			//	//    {
			//	//        sheet.Cells[row, col].Value = field.GetValue(item)?.ToString();
			//	//    }
			//	//    catch (Exception e)
			//	//    {
			//	//        MessageList.Add(e.Message);
			//	//    }
			//	//    col++;
			//	//}

			//	//выводим только те колонки, и в той последовательности, номера которых указали в Excel_Head
			//	for (int i=0; i<Excel_Head.Length;i++)
			//	{

			//		try
			//		{
			//			StaticMethods.AddExcelCell(ref sheet, ref col, row, myFieldInfo[i].GetValue(item));
			//		}
			//		catch (Exception e)
			//		{
			//			MessageList.Add(e.Message);
			//		}
			//		col++;
			//	}



			//	// указываем что в этой ячейке число
			//	//sheet.Cells[row, col].Style.Numberformat.Format = @"#,##0.00_ ;\-#,##0.00_ ;0.00_ ;";

			//	row++;
			//}
			// добавим всем ячейкам рамку




			//StaticMethods.WriteExcelFile(eP, ref MessageList, Report_name);

			//StaticMethods.FormatExcelExportMessageList(ref MessageList);
		}

		
	}

	static class Excel_methods {
		public static IActionResult Export_to_Excel(string reportName, ReportInfo reportInfo, Controller controller)
		{
			switch (reportName)
			{
				case "ConsumptionByCycleByBDM1"://БДМ-1 по видам продукции
				case "ConsumptionByCycleByBDM2"://БДМ-2 по видам продукции
				case "ConsumptionByBDM1ByDay"://БДМ-1(Суточный)
				case "ConsumptionByBDM2ByDay"://БДМ-2(Суточный)
					return Excel_methods.Export_to_Excel_ConsumptionByCycleByBDM(reportInfo);
					
				case "TERperMonthperTonne"://Средневзвешенное ТЭР на тонну по циклам производства
										   //В данном отчете ограничение по датам используется для ограничения SKU по которым выводить годовой отчет, год передается отдельно
					return Excel_methods.Export_to_Excel_TERperMonthperTonne(reportInfo);
				case "EnergyConsumptionByManufactureByHour"://Часовой расход электроэнергии по производству
					var sList_places = new List<string> {"Дата"
								,"Период" };
					//Добавляем заранее сформированный список мест
					sList_places.AddRange(Constants.places.Keys.ToList());
					sList_places.Add("Итого:");
					return Excel_methods.Export_to_Excel_Generic(reportInfo: reportInfo, excel_head: sList_places.ToArray(), sums_position_shift: 2);
				case "EnergyConsumptionByDevicesByDay"://Суточный расход электроэнергии по учётам
				case "EnergyConsumptionByDevicesByHour"://Часовой расход электроэнергии по учётам
														//отличия только в группировках между этими отчетами, так что формируем признак
					return Excel_methods.Export_to_Excel_EnergyConsumptionByDevices(reportInfo);
				case "ConsumptionByManufactureByPeriod"://общая по производству
					return Excel_methods.Export_to_Excel_ConsumptionByManufactureByPeriod(reportInfo);
				case "ConsumptionByBDM1ByHour"://БДМ-1(Часовой)
				case "ConsumptionByBDM2ByHour"://БДМ-2(Часовой)
					return Excel_methods.Export_to_Excel_Generic(reportInfo,
						     excel_head: Constants.Excel_export_head_ConsumptionByBDMByHour 
							

							, propsNameToModify: new string []{ "ID","StartPeriod","EndPeriod"} 
							, buttom_sums_name: "ConsumptionByBDMByHour"
							, sums_position_shift:3
							);
				case "SkuDataByShifts_BDM1":
				case "SkuDataByShifts_BDM2":
					return Excel_methods.Export_to_Excel_SkuDataByShifts(reportInfo);
				default:
					return null;
			}
			
		}
		static void AddRow_Body<T>(ExcelExport exportToExcel, List<T> Row, int col_adder=0,int height=0,int width=0, string format=null)
		{
			foreach (var item in Row)
			{
				exportToExcel.AddExcelCell(item, col_adder, height, width, format);
			}
		}

		static void AddRow_ConsumptionByManufactureByPeriod(ExcelExport exportToExcel, int start_Col, DateTime start_Date, IEnumerable<ConsumptionByManufactureByPeriod> RecordsToShow, List<string> first_part, List<double> second_part,  double total)
		{
			exportToExcel.Col = start_Col;
			
			AddRow_Body(exportToExcel, first_part);
			//метод формирует ячейки для таблицы, нам нужны только данные
			AddRow_Body(exportToExcel, Row: second_part);
			exportToExcel.AddExcelCell(total);
			exportToExcel.Row++;
		}
		//вырезаем данные из тега <td>
		static List<double> ParseTD_double(string input)
		{
			return input
						  .Substring(0, input.Length - 5)
						  .Replace("<td>", "", StringComparison.OrdinalIgnoreCase)
						  //отрезаем последний "</td>" чтобы не было пустого элемента
						  .Split("</td>")						  
						  .Select(r => Double.Parse(r))
						  .ToList();
		}
		static public FileContentResult Export_to_Excel_ConsumptionByManufactureByPeriod(ReportInfo reportInfo)
		{
			ExcelExport exportToExcel = new ExcelExport();
			var data = reportInfo.Records_List;
			string rep_name = reportInfo.ReportName.Rus;
			DateTime start_Date = reportInfo.ReportDates.StartDate;
			DateTime end_Date = reportInfo.ReportDates.EndDate;			
			Controller curr_controller = reportInfo.Curr_controller;
			
			int start_Col = 1;
			//добавляем в файл выгрузки головную часть файла
			exportToExcel.Add_head_reportName(

				 rep_name
				, start_Date
				, end_Date);
			#region Add header
			exportToExcel.Col = start_Col;
			

			exportToExcel.Add_head(ConsumptionByManufactureByPeriod_header.ToArray());

			exportToExcel.Add_head(Constants.places.Keys.ToArray());

			exportToExcel.Add_head(new string[] { "Всего:"});		

			#endregion




			var additional_data = Constants.ConsumptionByManufactureByPeriod_additional_data;
			for (int i = 0; i < data.Count; i++)
			{
				exportToExcel.Col = start_Col;
				exportToExcel.Row++;

				foreach (var s in additional_data[i])
				{
					exportToExcel.AddExcelCell(s);
				};

				exportToExcel.AddExcelCell(StaticMethods.FormPeriodString(start_Date, end_Date));

				for (int j = 0; j < data.Values.ElementAt(i).Count; j++)
				{
					exportToExcel.AddExcelCell(data.Values.ElementAt(i)[j], bold: j== data.Values.ElementAt(i).Count-1);
				}
				 
				
			
			}

			

			//Сохраняем эксель в поток
			exportToExcel.Save_excel_file_to_Stream();

			curr_controller.ViewBag.Excel_export_message_list = exportToExcel.MessageList;

			//mStream.Flush();


			return Excel_file_result(exportToExcel, rep_name);
		}

		static public FileContentResult Export_to_Excel_EnergyConsumptionByDevices(ReportInfo reportInfo)
		{
			ExcelExport exportToExcel = new ExcelExport();
			var RecordsToShow = reportInfo.Records.Select(r=>(EnergyConsumptionByDevices)r);
			string rep_name = reportInfo.ReportName.Rus;
			DateTime start_Date = reportInfo.ReportDates.StartDate;
			DateTime end_Date = reportInfo.ReportDates.EndDate;
			bool isByDay = reportInfo.IsByDay;
			List<double> sums = reportInfo.Sums["EnergyConsumptionByDevices"].ToList();
			double[] sums_EnergyConsumptionByDevices_col_production = reportInfo.Sums["EnergyConsumptionByDevices_col_production"];
			Controller curr_controller = reportInfo.Curr_controller;
			int start_Col = 1;
			//добавляем в файл выгрузки головную часть файла
			exportToExcel.Add_head_reportName(

				 rep_name
				, start_Date
				, end_Date);
			
			exportToExcel.Col = start_Col;
			exportToExcel.Add_head(new string[]{ "Дата" });
			
			var devices_list = RecordsToShow.Select(r => r.Device_Name).Distinct().ToList();
			exportToExcel.Add_head(devices_list.ToArray());
			//exportToExcel.Add_head(new string[] { "Итого:" });
			exportToExcel.Add_head(new string[] { "Итого по производству:" });
			int devices_count = devices_list.Count();
			
			

			exportToExcel.Row++;
			exportToExcel.Col = start_Col;



			

			//собираем списко всех дат по которым будем выводить данные
			var list_of_dates = RecordsToShow.Select(r => r.Measure_Date).Distinct().ToList();

			//Для каждой даты генерируем словарь (имя устройста, дата), по которму будем искать данные для каждого устройства
			//Принцип такой в каждом ряду нужны данные по дате, пробегаемя по списку всех устройств выгруженных для данного периода
			// если по нему есть данные в словаре, то пишим их иначе пишем нуль
			int index = 0;
			foreach (var curr_date in list_of_dates)
			{
				exportToExcel.Col = start_Col;
				exportToExcel.AddExcelCell(curr_date.ToShortDateString() + (isByDay?" ":" "+curr_date.ToShortTimeString()));

				var data_for_curr_date = RecordsToShow.Where(r => r.Measure_Date == curr_date).ToDictionary(r => r.Device_Name, r => r.EnergyConsumption);
				for (int i = 0; i < devices_count; i++)
				{
					exportToExcel.AddExcelCell(data_for_curr_date.ContainsKey(devices_list[i])?data_for_curr_date[devices_list[i]]:0.0);
				}
				//exportToExcel.AddExcelCell(sums_EnergyConsumptionByDevices_col[index]);
				exportToExcel.AddExcelCell(sums_EnergyConsumptionByDevices_col_production[index]);
				exportToExcel.Row++;
				index++;

			}
			//Дорисуем итоги:
			exportToExcel.Col = start_Col;
			exportToExcel.AddExcelCell("Итого:", outline:true, bold:true);
			foreach (var sum in sums)
			{
				exportToExcel.AddExcelCell(sum, outline: true, bold: true);
			}


			//Сохраняем эксель в поток
			exportToExcel.Save_excel_file_to_Stream();

			curr_controller.ViewBag.Excel_export_message_list = exportToExcel.MessageList;

			//mStream.Flush();


			return Excel_file_result(exportToExcel, rep_name);
		}

		static public FileContentResult Export_to_Excel_ConsumptionByCycleByBDM( ReportInfo reportInfo)
		{
			 
			var RecordsToShow = reportInfo.Records.Select(r => (ConsumptionByCycleOfProduction)r);
			var rep_name = reportInfo.ReportName.Rus;
			var sums = reportInfo.Sums.FirstOrDefault().Value;
			var start_Date = reportInfo.ReportDates.StartDate;
			var end_Date = reportInfo.ReportDates.EndDate;
			var curr_controller = reportInfo.Curr_controller;
			var exportToExcel = new ExcelExport();
			//добавляем в файл выгрузки головную часть файла
			exportToExcel.Add_head_reportName(

				 rep_name
				, start_Date
				, end_Date);

			exportToExcel.Add_head(Constants.Excel_export_head_ConsumptionByCycleByBDM);
			//добавляем в файл выгрузки тело файла
			foreach (ConsumptionByCycleOfProduction record in RecordsToShow)
			{
				exportToExcel.Col = 1;
				exportToExcel.Row++;
				//row++;
				exportToExcel.AddExcelCell(record.CycleDateBegin.ToString(), format: "dd.mm.YYYY HH:mm:ss");
				exportToExcel.AddExcelCell(record.CycleDateEnd.ToString(), format: "dd.mm.YYYY HH:mm:ss");
				exportToExcel.AddExcelCell(record.Place);
				exportToExcel.AddExcelCell(Math.Round((record.CycleDateEnd - record.CycleDateBegin).TotalMinutes, 2));
				exportToExcel.AddExcelCell(record.SortofProduction);
				exportToExcel.AddExcelCell(record.CapacityOfBO,format: "0.000");
				exportToExcel.AddExcelCell(record.EnergyConsumption, format: "0.000");
				exportToExcel.AddExcelCell(record.EnergyConsumptionPerTonne, format: "0.000");
				exportToExcel.AddExcelCell(record.GasConsumption, format: "0.000");
				exportToExcel.AddExcelCell(record.GasConsumptionPerTonne, format: "0.000");
				exportToExcel.AddExcelCell(record.SteamConsumption, format: "0.000");
				exportToExcel.AddExcelCell(record.SteamConsumptionPerTonne, format: "0.000");
				exportToExcel.AddExcelCell(record.AverageSpeed);

			}
			exportToExcel.Row++;
			exportToExcel.Col = 3;


			exportToExcel.AddExcelCell("Итого:");
			//Считаем и записывыем  длину периода отдельно
			exportToExcel.AddExcelCell(StaticMethods.RightTimeString(RecordsToShow.Select(m => (m.CycleDateEnd - m.CycleDateBegin)).Aggregate(TimeSpan.Zero,
			(sumSoFar, nextMyObject) => sumSoFar + nextMyObject)));

			exportToExcel.AddExcelCell("");


			if (sums!=null&&sums.Length>0)
			{
				//записываем в эксель зарание посчитаные суммы для соответсвующих колонок
				foreach (double sum in sums)
				{
					exportToExcel.AddExcelCell(sum, format: "0.000");
				}
			}
			


			//выделяем итоги
			exportToExcel.Sheet.Cells[exportToExcel.Row, 2, exportToExcel.Row, exportToExcel.Col - 1].Style.Font.Bold = true;

			//Сохраняем эксель в поток
			exportToExcel.Save_excel_file_to_Stream();

			curr_controller.ViewBag.Excel_export_message_list = exportToExcel.MessageList;

			//mStream.Flush();


			return Excel_file_result(exportToExcel, rep_name);
		}

		static public FileContentResult Export_to_Excel_SkuDataByShifts(ReportInfo reportInfo)
		{

		
			ExcelExport exportToExcel = new ExcelExport();
			string rep_name = reportInfo.ReportName.Rus;
			DateTime start_Date = reportInfo.ReportDates.StartDate;
			DateTime end_Date = reportInfo.ReportDates.EndDate;
			string placeName = reportInfo.PlaceName;
			Controller curr_controller = reportInfo.Curr_controller;
			string[] baseStrings = reportInfo.BaseStrings;
			List<SkuDataByShifts> records = reportInfo.Records_List["SkuDataByShifts"]
									.Select(r => (SkuDataByShifts)r)
									.Where(r => reportInfo.ReportDates.CurrShiftStart <= r.CycleDateBegin && r.CycleDateEnd <= reportInfo.ReportDates.CurrShiftEnd)
									.ToList();
			List<string> ter_list_shifts = Constants.ter_list_shifts;
			int shiftId = records.FirstOrDefault().ShiftId;
			string machinist_name = records.FirstOrDefault().Machinist_name; 
			var head_report_lines = new List<string> {$"Отчёт за смену № {shiftId.ToString()}",
				$"Дата: {start_Date.ToShortDateString()} - {end_Date.ToShortDateString()}",
				$"Время: с {start_Date.ToShortTimeString()} до {end_Date.ToShortTimeString()}",				
				$"Передел: {placeName}",
				$"Машинист: {machinist_name}",
				$""};

			//exportToExcel.Sheet.Row(exportToExcel.Row).Height = 22;
			exportToExcel.Sheet.Column(1).Width = 50;//Размер первой колонки
													 //Фиксируем первые колонки
			exportToExcel.Sheet.View.FreezePanes(1, 2);
			foreach (var rl in head_report_lines)
			{
				exportToExcel.AddExcelCell(rl,outline:false,horizAlign:false);
				exportToExcel.New_line();
			}
			
			int curr_string_index = 0;
			int ter_start_col = 1;

			foreach (var data in records)
			{
				
				exportToExcel.Col = ter_start_col;
				exportToExcel.AddExcelCell($"Наименование продукции: {data.Sku}");
				exportToExcel.Row++;
				exportToExcel.Col--;
				var row_shift = 0;
				exportToExcel.AddExcelCell($"Жидкий поток: {data.FluidStream}");
				exportToExcel.Row++;
				exportToExcel.Col--;
				row_shift--;
				exportToExcel.AddExcelCell($"Средняя влажность: {data.Wetness}");
				exportToExcel.Row++;
				exportToExcel.Col--;
				row_shift--;
				exportToExcel.AddExcelCell($"Начало цикла: {data.CycleDateBegin}");
				exportToExcel.Row++;
				exportToExcel.Col--;
				row_shift--;
				exportToExcel.AddExcelCell($"Конец цикла: {data.CycleDateEnd}");
				row_shift--;
				exportToExcel.IncreaseRow(row_shift);//смещение графика относительно данных по циклу
				//Рисуем название графика и сам график

				int image_incr_row = 12;
				//int image_incr_col = 9;

				if (baseStrings!=null&& baseStrings.Length>0)//если есть рисунки то выгружаем их
				{

				
					foreach (var ter in ter_list_shifts)
					{
					
					exportToExcel.AddExcelCell(ter, width:50);
					exportToExcel.Row++;
					exportToExcel.IncreaseCol(-2);
					exportToExcel.AddImageToExcel(StaticMethods.Base64StringToImage(baseStrings[curr_string_index].Split(",")[1]), 300, 200);
					exportToExcel.Row--;
					exportToExcel.IncreaseCol(2);
					curr_string_index++;
					}
				}
				exportToExcel.IncreaseRow(image_incr_row);
				
			}
			//Сохраняем эксель в поток
			exportToExcel.Save_excel_file_to_Stream();
			//Сохраняем ошибки
			if (curr_controller != null)
			{
				curr_controller.ViewBag.Excel_export_message_list = exportToExcel.MessageList;
				//Сохраняем производительность
				//((Dictionary<string, string>)curr_controller.ViewData["PerfTimes"]).Add("excel_export", StaticMethods.PerfTime(exportToExcel.StartTime));
			}

			//Возвращаем экселевский файл браузеру
			return Excel_file_result(exportToExcel, rep_name);

		}
			static public FileContentResult Export_to_Excel_TERperMonthperTonne(ReportInfo reportInfo)
		{
			var exportToExcel = new ExcelExport();
			var RecordsToShow_perMonths = reportInfo.Records_List["TERperMonthperTonne"].Select(r => (TERperMonthperTonne)r);
			var RecordsToShow_cycles = reportInfo.Records_List["TERperCycleperTonne"].Select(r => (TERperCycleperTonne)r);
			var RecordsToShow_perMonths_Plan = reportInfo.Records_List["TER_Plan"].Select(r => (TERperMonthperTonne)r);
			string rep_name = reportInfo.ReportName.Rus;
			var start_Date = reportInfo.ReportDates.StartDate;
			var end_Date = reportInfo.ReportDates.EndDate;
			int tesYear = reportInfo.ReportDates.TesYear;
			var baseStrings = reportInfo.BaseStrings;
			bool isCurrYear = tesYear == DateTime.Now.Year;
			//добавляем в файл выгрузки головную часть файла
			exportToExcel.Add_head_reportName(

				 rep_name
				, start_Date
				, end_Date);

			string[] list_months = new string[] { };


			exportToExcel.Add_head(new string[] { " ", " " });

			exportToExcel.Sheet.Row(exportToExcel.Row).Height = 22;
			exportToExcel.Add_head(new string[] { "Текущие показатели" }, col_adder: 6);

			exportToExcel.Add_head(new string[] { isCurrYear?"Показатели с начала месяца": @CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(12) }, col_adder: 4);


			int monthToShow = isCurrYear ? DateTime.Now.Month :  12;
			for (int i = monthToShow - 1; i > 0; i--)
			{


				exportToExcel.Add_head(new string[] { @CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i) }, col_adder: 4);

			}

			exportToExcel.Add_head(new string[] { "Итого средняя  за год(истекший период)" }, col_adder: 4);
			exportToExcel.Add_head(new string[] { $"Бюджет {tesYear} года (исходные табличные данные)" }, col_adder: 4);
			exportToExcel.Add_head(new string[] { "Отклонение от бюджета" }, col_adder: 4);

			exportToExcel.Col = 1;
			exportToExcel.Row++;
			exportToExcel.Sheet.Row(exportToExcel.Row).Height = 48.5;
			exportToExcel.Add_head(new string[] { "Передел" }, width: 13);
			exportToExcel.Add_head(new string[] { "Номенклатура" }, width: 55);
			exportToExcel.Add_head(new string[] { "Начало цикла производства" }, width: 19);
			exportToExcel.Add_head(new string[] { "Конец цикла производства" }, width: 19);

			for (int i = monthToShow + 4; i > 0; i--)
			{

				exportToExcel.Add_head(new string[] { "Объем БО тонн" }, width: 15);
				exportToExcel.Add_head(new string[] { "Средняя скорость м/мин" }, width: 15);
				exportToExcel.Add_head(new string[] { "Энергоемкость МВт*ч/тонна" }, width: 15);
				exportToExcel.Add_head(new string[] { "Потребление газа тыс м3 /тонна" }, width: 15);
				exportToExcel.Add_head(new string[] { "Потребление пара ГКал/тонна" }, width: 15);


			}

			int report_body_start_col = 1;
			int report_body_months_start_col = 10;
			int report_body_start_row = 7;
			int count_of_col_per_sku = 5;
			int curr_sku_row = report_body_start_row;
			int curr_max_row = report_body_start_row;
			int month_for_total = 13;

			exportToExcel.Row = report_body_start_row;
			foreach (var item in RecordsToShow_perMonths.Select(r => new { r.SKU, r.PlaceName, r.PlaceID }).Distinct())
			{
				exportToExcel.Col = report_body_start_col;
				exportToExcel.Row = curr_max_row;
				exportToExcel.Row++;

				exportToExcel.AddExcelCell(item.PlaceName, width: 13);
				exportToExcel.AddExcelCell(item.SKU, width: 55);
				curr_sku_row = exportToExcel.Row;
				exportToExcel.Row--;

				var cycles_per_sku = RecordsToShow_cycles.Where(r => r.SKU == item.SKU && r.PlaceID == item.PlaceID);
				if (cycles_per_sku.Count() > 0)
				{
					foreach (var curr_cycle in cycles_per_sku)
					{
						exportToExcel.Row++;
						exportToExcel.Col = report_body_start_col + 2;

						curr_max_row++;
						Add_excel_data_per_sku_per_cycles(exportToExcel, curr_cycle);



					}
				}
				else
				{
					exportToExcel.Row++;
					exportToExcel.Col = report_body_start_col + 2;
					curr_max_row++;
					for (int i = 0; i < count_of_col_per_sku + 2; i++)
					{

						exportToExcel.AddExcelCell("Нет данных", width: 15);
					}
				}







				//Возвращаемся на позицию для прорисовки данных по месяцам и итогам
				exportToExcel.Col = report_body_months_start_col;
				exportToExcel.Row = curr_sku_row;

				for (int curr_month = monthToShow; curr_month > 0; curr_month--)
				{
					//Получем строку с параметрами за нужный нам месяц по SKU
					var curr_record = RecordsToShow_perMonths.Where(r => r.SKU == item.SKU
					  && r.PlaceID == item.PlaceID
					  && r.Month == curr_month).FirstOrDefault();
					Add_excel_data_per_sku_per_month(exportToExcel, curr_record,count_of_col_per_sku);
					
				}
				//итог хранится в 13 месяце
				//Получем строку с параметрами за нужный нам месяц по SKU


				var for_total_record = RecordsToShow_perMonths.Where(r => r.SKU == item.SKU
				  && r.PlaceID == item.PlaceID
				  && r.Month == month_for_total).FirstOrDefault();
				Add_excel_data_per_sku_per_month(exportToExcel, for_total_record,count_of_col_per_sku);


				//итог хранится в 13 месяце
				//Получем строку с параметрами за нужный нам месяц по Plan

				var for_total_record_plan = RecordsToShow_perMonths_Plan.Where(r => r.SKU == item.SKU
					&& r.PlaceID == item.PlaceID
					&& r.Month <= month_for_total).GroupBy(r => new { r.PlaceID, r.SKU, r.PlaceName }, (g, r) => new TER_SKU_cells
					{
						ValueOfBO = r.Sum(e => e.ValueOfBO),
						AverageSpeed = r.Average(e => e.AverageSpeed),
						EnegryConsumptionperTonne = r.Average(e => e.EnegryConsumptionperTonne),
						GasConsumptionperTonne = r.Average(e => e.GasConsumptionperTonne),
						PlaceName = g.PlaceName,
						SteamConsumptionperTonne = r.Average(e => e.SteamConsumptionperTonne)
					}).FirstOrDefault();
				
				Add_excel_data_per_sku_per_month(exportToExcel, for_total_record_plan,count_of_col_per_sku);

				if (for_total_record_plan!=null)
				{
					var diff_total_period_record = new TERperMonthperTonne();
					diff_total_period_record.ValueOfBO = Math.Round(for_total_record_plan.ValueOfBO - for_total_record.ValueOfBO, 3);
					diff_total_period_record.AverageSpeed = Math.Round(for_total_record_plan.AverageSpeed - for_total_record.AverageSpeed, 3);
					diff_total_period_record.EnegryConsumptionperTonne = Math.Round(for_total_record_plan.EnegryConsumptionperTonne - for_total_record.EnegryConsumptionperTonne, 3);
					diff_total_period_record.GasConsumptionperTonne = Math.Round(for_total_record_plan.GasConsumptionperTonne - for_total_record.GasConsumptionperTonne, 3);
					diff_total_period_record.SteamConsumptionperTonne = Math.Round(for_total_record_plan.SteamConsumptionperTonne - for_total_record.SteamConsumptionperTonne, 3);
					Add_excel_data_per_sku_per_month(exportToExcel, diff_total_period_record, count_of_col_per_sku);
				}
				
			}
			//Фиксируем первые колонки
			exportToExcel.Sheet.View.FreezePanes(1, 3);
			//Добавляем графики
			if (baseStrings != null)
			{
			int curr_string_index = 0;
			int Images_part_start_row = exportToExcel.Row+5;
			int image_incr_row = 15;
			int image_incr_col = 4;

			exportToExcel.Row = Images_part_start_row;
			exportToExcel.Row = exportToExcel.Row + image_incr_row % 2 + 1;
			foreach (var item in RecordsToShow_perMonths.Select(r => new { r.SKU, r.PlaceName }).Distinct())
			{
				exportToExcel.Col = report_body_start_col;
				
				exportToExcel.AddExcelCell(item.PlaceName, width: 13);
				exportToExcel.AddExcelCell(item.SKU, width: 55);
				exportToExcel.Row = exportToExcel.Row + image_incr_row;
			}

			exportToExcel.Row = Images_part_start_row;
			exportToExcel.Col = 2;
			

			
			foreach (var item in baseStrings)
			{
				
				
				
				exportToExcel.AddImageToExcel(StaticMethods.Base64StringToImage(baseStrings[curr_string_index].Split(",")[1]), 300, 300);
				
				exportToExcel.Col = exportToExcel.Col + image_incr_col;
				curr_string_index++;
				if ((curr_string_index) % count_of_col_per_sku == 0)
				{
					exportToExcel.Col = 2;
					exportToExcel.Row = exportToExcel.Row + image_incr_row;
				}
			}
			}
			//Сохраняем эксель в поток
			exportToExcel.Save_excel_file_to_Stream();
			//Сохраняем ошибки
			if (reportInfo.Curr_controller!=null)
			{
				reportInfo.Curr_controller.ViewBag.Excel_export_message_list = exportToExcel.MessageList;
				//Сохраняем производительность
				//((Dictionary<string, string>)curr_controller.ViewData["PerfTimes"]).Add("excel_export", StaticMethods.PerfTime(exportToExcel.StartTime));
			}
            
			//Возвращаем экселевский файл браузеру
			return Excel_file_result(exportToExcel,rep_name);

		}

		static public FileContentResult Export_to_Excel_Generic(ReportInfo reportInfo, string[] excel_head, List<object> optional_data=null, string buttom_sums_name = null, string right_sums_name = null,  int[] sums_positions=null, int? sums_position_shift =null, string[] propsNameToModify=null)
		{
			var RecordsToShow = optional_data != null ? optional_data : reportInfo.Records;
			
			
			string rep_name = reportInfo.ReportName.Rus;
			DateTime start_Date = reportInfo.ReportDates.StartDate;
			DateTime end_Date = reportInfo.ReportDates.EndDate;
			double[] buttom_sums = buttom_sums_name!=null?reportInfo.Sums[buttom_sums_name]:new double[0];
			double[] right_sums = right_sums_name!=null?reportInfo.Sums[right_sums_name]:new double[0];
			bool is_buttom_sums = buttom_sums.Length > 0;
			bool is_right_sums = right_sums.Length > 0; 
			var curr_controller = reportInfo.Curr_controller;
			var exportToExcel = new ExcelExport();
			int first_col = 1;
			//добавляем в файл выгрузки головную часть файла
			exportToExcel.Add_head_reportName(

				 rep_name
				, start_Date
				, end_Date);

			exportToExcel.Col = first_col;

			exportToExcel.Add_head(excel_head);

			

			////Получаем список полей для выгрузки в эксель
			FieldInfo[] myFieldInfo;
			Type myType = reportInfo.Records.FirstOrDefault().GetType();

			////Собираем иноформацию о членов класса переданном в перечислителе
			myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			int col_count = 1;
			foreach (var item in RecordsToShow)
			{
				exportToExcel.Col = first_col;
				exportToExcel.Row++;
				//для каждого элемента в переданном классе T выводим ячейку в эксель
				for (int i = 0; i < myFieldInfo.Length; i++)
				{
					if (myFieldInfo[i].FieldType.Name.Contains("List"))
					{
						exportToExcel.AddExcelCellList<double>((List<double>)myFieldInfo[i].GetValue(item));
					}
					else
					{
							//Если нужна модификация ячейки то проводим её, иначе просто выводим её
							if (propsNameToModify!=null
								&&propsNameToModify!=null
								&&propsNameToModify.Contains(GetFieldName(myFieldInfo[i])))
							{
								var temp = ChangeOutput(myFieldInfo[i].GetValue(item), GetFieldName(myFieldInfo[i]));
								if (temp!=null)
								{
									exportToExcel.AddExcelCell(temp);
								}
								
							}
							else
							{						
								exportToExcel.AddExcelCell(myFieldInfo[i].GetValue(item));							
							}
							
						
						
					}
				}
				//Так же добавляем итоги справа
				if (is_right_sums)
				{
					exportToExcel.AddExcelCell(right_sums[col_count], outline:true, bold: true);	
				}
				col_count++;
			}
			//Итоги снизу
			if (is_buttom_sums&&(sums_positions!=null|| sums_position_shift != null))
			{
				exportToExcel.Row++;
				
				exportToExcel.Col = first_col;

				//Два способа нарисовать итого, один то в определенные колонки, позиции которых заданы массивом,
				// второй это просто начиная с какой то позиции рисуем все итого
				if (sums_positions != null)//рисуем итого на позиции которые записаные в sums_positions[]
				{
					exportToExcel.Col = sums_positions[0] + first_col;
					exportToExcel.AddExcelCell("Итого:", bold: true);
					for (int i = 0; i < sums_positions.Length; i++)
					{
						exportToExcel.Col = sums_positions[i] + first_col + 1;
						exportToExcel.AddExcelCell(buttom_sums[i], bold: true);
					}
				}
				if (sums_position_shift!=null)
				{
					exportToExcel.Col = first_col+sums_position_shift ?? 0;
					exportToExcel.AddExcelCell("Итого:", bold: true);
					foreach (var sum in buttom_sums)
					{
						exportToExcel.AddExcelCell(sum,bold:true);
					}
					
				}

				
				//выделяем итоги
				exportToExcel.Sheet.Cells[exportToExcel.Row, 1, exportToExcel.Row, exportToExcel.Col - 1].Style.Font.Bold = true;
			}
			
			


			//Сохраняем эксель в поток
			exportToExcel.Save_excel_file_to_Stream();
			//Сохраняем ошибки
			curr_controller.ViewBag.Excel_export_message_list = exportToExcel.MessageList;
			//Возвращаем экселевский файл браузеру
			return Excel_file_result(exportToExcel, rep_name);

		}

		
		static void  Add_excel_data_per_sku_per_cycles(ExcelExport exportToExcel, TERperCycleperTonne curr_cycle)

		{

			exportToExcel.AddExcelCell(curr_cycle.CycleDateBegin, width: 19, format: "dd.mm.YYYY");

			exportToExcel.AddExcelCell(curr_cycle.CycleDateEnd, width: 19, format: "dd.mm.YYYY");


			exportToExcel.AddExcelCell(curr_cycle.ValueOfBO, width: 15);


			exportToExcel.AddExcelCell(curr_cycle.AverageSpeed, width: 15);


			exportToExcel.AddExcelCell(curr_cycle.EnegryConsumptionperTonne, width: 15);


			exportToExcel.AddExcelCell(curr_cycle.GasConsumptionperTonne, width: 15);


			exportToExcel.AddExcelCell(curr_cycle.SteamConsumptionperTonne, width: 15);
		}

		static void Add_excel_data_per_sku_per_month(ExcelExport exportToExcel, ITER_SKU_cells curr_record, int count_of_col_per_sku)

		{

			if (curr_record != null)
			{
				exportToExcel.AddExcelCell(curr_record.ValueOfBO, width: 15);


				exportToExcel.AddExcelCell(curr_record.AverageSpeed, width: 15);


				exportToExcel.AddExcelCell(curr_record.EnegryConsumptionperTonne, width: 15);


				exportToExcel.AddExcelCell(curr_record.GasConsumptionperTonne, width: 15);


				exportToExcel.AddExcelCell(curr_record.SteamConsumptionperTonne, width: 15);
			}
			//если данныех по месяце нет то добаляем нули
			else
			{
				for (int i = 0; i < count_of_col_per_sku; i++)
				{
					exportToExcel.AddExcelCell(0, width: 15);
				}
			}


			
		}

		static  FileContentResult Excel_file_result(ExcelExport exportToExcel, string report_name)
		{
			return new FileContentResult(exportToExcel.mStream.ToArray(),
					"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
			{
				FileDownloadName = report_name + $"_{DateTime.UtcNow.ToShortDateString()}.xlsx"
			};
		}
	}
}
