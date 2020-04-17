namespace VacationFinder.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;
    using VacationFinder.Services.Mapping;

    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> _settingsRepository;

        public SettingsService(IDeletableEntityRepository<Setting> settingsRepository)
        {
            this._settingsRepository = settingsRepository;
        }

        public int GetCount()
        {
            return this._settingsRepository.All().Count();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return this._settingsRepository.All().To<T>().ToList();
        }
    }
}
