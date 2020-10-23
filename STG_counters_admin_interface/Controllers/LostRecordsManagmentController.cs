using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using STG_counters_admin_interface.Models.PowerDB_data_classes;
using Microsoft.EntityFrameworkCore;

namespace STG_counters_admin_interface.Controllers
{
    public class LostRecordsManagmentController : Controller
    {

		private ScaffoldContext context;
		public LostRecordsManagmentController(ScaffoldContext ctx) => context = ctx;

		//public IActionResult Index()
		//{
		//	//return View(context.StgRecordsLost.Where(r=> r.RecordTime > (DateTime.Today.AddMonths(-6))).Include(r => r.IdDeviceNavigation).Include(r=>r.StgDataLost));
		//}
	}

	
}