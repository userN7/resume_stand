using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STG_counters_admin_interface.Models
{
	public static class Extensions
	{
	 public static bool CanTakeRecs<T>(this List<T> recs)
		{
			if (recs != null && recs.Count > 0)
				return true;
			else
			return false;
		}
	 public static  List<T> UnPack<T>(this List<object> recs) => recs.Select(r => (T)r).ToList();
	}
}
