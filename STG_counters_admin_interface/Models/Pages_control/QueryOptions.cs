using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG_counters_admin_interface.Models.Pages_control
{
	public class QueryOptions
	{
		public int CurrentPage { get; set; } = 1;
		public int PageSize { get; set; } = 10;
	}
}
