using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Helpers
{
    public class ConvertHelper
    {

        public static string NullToString(object obj)
        {
            return obj == null || obj == DBNull.Value ? string.Empty : obj.ToString() ?? string.Empty;
        } 


    }
}
