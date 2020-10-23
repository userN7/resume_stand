using STG_counters_admin_interface.Models.PowerDB_data_classes;
using STG_counters_admin_interface.Controllers;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static STG_counters_admin_interface.Models.StaticMethods;
using static STG_counters_admin_interface.Models.Constants;
using STG_counters_admin_interface.Models;
using System.Collections;

namespace STG_counters_admin_interface.Tests
{
	public class DataTests
	{
		DateTime startDate = new DateTime(2020, 6, 1);
		DateTime endDate = new DateTime(2020, 6, 1, 23, 59, 59);
		public static List<object> PackReportInfoData(object data)
		{
			var data_to_test = new List<object>();
			if (((ReportInfo)data).Records.CanTakeRecs())
			{
				data_to_test.AddRange(((ReportInfo)data).Records);
			};

			foreach (var recs in ((ReportInfo)data).Records_List.Values)
			{
				if (recs.CanTakeRecs())
				{
					data_to_test.AddRange(recs);
				}
			}
			return data_to_test;
		}
		public static string CleanSpecialSymbols(string str) =>
			str.Replace("\t", "").Replace("\n", "").Replace("\r", "");
		


		[Theory]
		[ClassData(typeof(TestDataGenerator_VerifyModelData))]
		public void VerifyModelData(string report_name, string test_data)
		{
			
			var mock = new Mock<ScaffoldContext>();
			
			var controller = new ReportsController(new ScaffoldContext((new DbContextOptionsBuilder<ScaffoldContext>())
					.UseSqlServer(@"Server=127.0.0.1;Database=PowerDB;Integrated Security=SSPI;Trusted_connection=True;MultipleActiveResultSets=true")
					.Options));
			
			ViewResult result = (ViewResult)controller.ShowReport(startDate, endDate, report_name);
			
			List<object> data_to_test = PackReportInfoData(result.Model);			

			string[] excludeFieldList = new string[] {"ID"};
			var strData_to_test = ListFieldsValues(data_to_test, excludeFieldList);
			try
			{
				Assert.Equal( CleanSpecialSymbols(test_data), CleanSpecialSymbols(strData_to_test));
			}
			catch (Exception e)
			{

				throw e;
			}



		}
		//[Fact]
		//пока не настроил вывод данных для тестирования то класс можно увидеть в переменной test_class, если сделать дебаг
		 void FillModelData()
		{
			//Arrange
			var mock = new Mock<ScaffoldContext>();
			
			//В test_class будет хранится класс для тестирования, можно получить через дебаг, или написать выгрузку куда нибудь, возможно в *.cs для чтобы не копировать лишний раз
			var test_class = $@"public class TestDataGenerator_VerifyModelData : IEnumerable<object[]>
	{{
		#pragma warning disable CS0414// these fields used for information only
		private readonly string creationDate = ""{DateTime.Now}"";
		private readonly string startDate =""{startDate}"";
		private readonly string endDate =""{endDate}"";
		#pragma warning restore CS0414
		private readonly List<object[]> _data= new List<object[]>
    {{
";
			var task_list = new List<Task>();
			foreach (var report in List_report_fullname_for_tests()
										//.Where(o=>o[0].ToString() == "EnergyConsumptionByManufactureByHour;Часовой расход электроэнергии по производству")
										)
			{
				task_list.Add(Task.Run(() =>
				{
					//Создаем новый экземпляр контроллера для получения Model, потому что если использовать один то с контекстом базы происходит какой то глюк и для dbContext.Set<T>().fromSql выдает первый сет для тех же T, вне зависимости от sql_cmd
					
					ViewResult result = (ViewResult)(new ReportsController(new ScaffoldContext((new DbContextOptionsBuilder<ScaffoldContext>())
						.UseSqlServer(@"Server=127.0.0.1;Database=PowerDB;Integrated Security=SSPI;Trusted_connection=True;MultipleActiveResultSets=true")
						.Options))).ShowReport(startDate, endDate, report[0].ToString());

					var data_to_test = new List<object>();
					if (((ReportInfo)result.Model).Records.CanTakeRecs())
					{
						data_to_test.AddRange(((ReportInfo)result.Model).Records);
					};

					foreach (var recs in ((ReportInfo)result.Model).Records_List.Values)
					{
						if (recs.CanTakeRecs())
						{
							data_to_test.AddRange(recs);
						}
					}
					

					test_class += $@" new object[]  {{""{report[0]}"",
@""{ListFieldsValues(data_to_test)}""
}},
";
				}));
			}
			Task.WaitAll(task_list.ToArray());
			test_class = test_class.Substring(0, test_class.Length - 3);//срезаем запятую
			test_class += @"
		}; 

		public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}";



		}

		
		
	}
}
