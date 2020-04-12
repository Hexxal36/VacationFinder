using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TableExtensions.Paging
{
    public class Paging
    {
        public static IList<int> GetPage(IList<int> list, int pageNumber, int pageSize = 10)
        {
            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public static int GetPageCount(IList<int> list, int pageSize = 10)
        {
            if (list.Count % pageSize == 0)
            {
                return list.Count / pageSize;
            }

            return list.Count / pageSize + 1;
        }
    }
}
