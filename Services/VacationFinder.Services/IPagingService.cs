namespace VacationFinder.Services
{
    using System.Collections.Generic;

    using VacationFinder.Data.Common.Models;

    public interface IPagingService
    {
        IEnumerable<BaseDeletableModel<int>> GetPage(IEnumerable<BaseDeletableModel<int>> list, int pageNumber, int pageSize = 10);

        int GetPageCount(IEnumerable<BaseDeletableModel<int>> list, int pageSize = 10);
    }
}
