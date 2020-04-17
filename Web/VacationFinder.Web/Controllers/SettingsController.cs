namespace VacationFinder.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using VacationFinder.Data.Common.Repositories;
    using VacationFinder.Data.Models;
    using VacationFinder.Services.Data;
    using VacationFinder.Web.ViewModels.Settings;

    public class SettingsController : BaseController
    {
        private readonly ISettingsService _settingsService;

        private readonly IDeletableEntityRepository<Setting> _repository;

        public SettingsController(ISettingsService settingsService, IDeletableEntityRepository<Setting> repository)
        {
            this._settingsService = settingsService;
            this._repository = repository;
        }

        public IActionResult Index()
        {
            var settings = this._settingsService.GetAll<SettingViewModel>();
            var model = new SettingsListViewModel { Settings = settings };
            return this.View(model);
        }

        public async Task<IActionResult> InsertSetting()
        {
            var random = new Random();
            var setting = new Setting { Name = $"Name_{random.Next()}", Value = $"Value_{random.Next()}" };

            await this._repository.AddAsync(setting);
            await this._repository.SaveChangesAsync();

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
