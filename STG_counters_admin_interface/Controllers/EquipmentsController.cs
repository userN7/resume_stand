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

namespace STG_counters_admin_interface.Controllers
{
    public class EquipmentsController : Controller
    {

        private ScaffoldContext context;
        public EquipmentsController(ScaffoldContext ctx) => context = ctx;


        public IActionResult Index()
        {
           
            return View(context.PaDevices
                .Include(d => d.PaAdapters)
                .OrderBy(d => d.DeviceType));
        }
    }
}