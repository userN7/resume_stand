using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG_counters_admin_interface.Models.Pages_control
{
    
    public class PagedList<T> : List<T>
	{
		public QueryOptions Options { get; set; }
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public int TotalPages { get; set; }
		public int Total_count_of_records { get; set; }

		public bool HasPreviousPage => CurrentPage > 1;
		public bool HasNextPage => CurrentPage < TotalPages;

		public PagedList(IQueryable<T> query, int total_count_of_records, QueryOptions options = null)
		{
			CurrentPage = options.CurrentPage;
			PageSize = options.PageSize;
			Options = options;
			Total_count_of_records = total_count_of_records;
			//количество страниц это количество строк поделеное на размер страницы плюс 1 если остаток не нулевой
			TotalPages = (Total_count_of_records) / PageSize + ((Total_count_of_records % PageSize == 0) ? 0 : 1);
			//AddRange(query.Skip((CurrentPage - 1) * PageSize).Take(PageSize));
			AddRange(query);
		}
	}
}
