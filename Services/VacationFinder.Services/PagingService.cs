namespace VacationFinder.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using VacationFinder.Data.Common.Models;

    public class PagingService : IPagingService
    {
        public IEnumerable<BaseDeletableModel<int>> GetPage(IEnumerable<BaseDeletableModel<int>> list, int pageNumber, int pageSize = 10)
        {
            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public int GetPageCount(IEnumerable<BaseDeletableModel<int>> list, int pageSize = 10)
        {
            int count = list.Count();
            if (count == 0)
            {
                return 1;
            }


            if (count % pageSize == 0)
            {
                return count / pageSize;
            }

            return (count / pageSize) + 1;
        }
    }
}
