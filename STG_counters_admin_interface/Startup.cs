using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using STG_counters_admin_interface.Models;
using STG_counters_admin_interface.Models.PowerDB_data_classes;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using System.IO;
using Microsoft.Extensions.Hosting;
//using Microsoft.AspNetCore.Hosting;

namespace STG_counters_admin_interface
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

		public IConfiguration Configuration { get; }

		public Startup(IConfiguration config) => Configuration = config;

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
            
            //Используем свою строку для подключения к sql
            string conString = Configuration["ConnectionStrings:DefaultConnection"];
			
			services.AddDbContext<ScaffoldContext>(options => options.UseSqlServer(conString, 
                //увеличиваем таймаут для запроса до 90 сек
                sqlServerOptions => sqlServerOptions.CommandTimeout(90))
            );


			//Добавляем фоновые задачи(рассылка почты, обновление кэшей и прочее)
			//Стандартное добавление фоновой задачи services.AddHostedService<TimedHostedService>()
			// не подходит для наших целей так как это синоним services.AddTransient<IHostedService, TimedHostedService>()
			// т.е для каждого обращения браузера создается свой экзепляр TimedHostedService
			//но для наших целей нам нужен  доступ к фоновой задачи чтобы перезапускать под задачи, а так же получать текущее состояние фоновых задач
			// так что запускаем его через синглтон, чтобы был едиственный главный экзепляр.

			services.AddSingleton<IHostedService, TimedHostedService>();
			//кэш для экселя и для фильтрации по выведенным данным
			services.AddSingleton<ReportInfo>();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
		{
			app.UseDeveloperExceptionPage();			
			app.UseMvcWithDefaultRoute();			
			app.UseStaticFiles();
		}
	}
}
