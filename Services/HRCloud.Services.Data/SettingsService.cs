using System.Collections.Generic;
using System.Linq;

using HRCloud.Data.Common.Repositories;
using HRCloud.Data.Models;
using HRCloud.Services.Data.Interfaces;
using HRCloud.Services.Mapping;

namespace HRCloud.Services.Data
{
    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> settingsRepository;

        public SettingsService(
            IDeletableEntityRepository<Setting> settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public int GetCount()
            => this.settingsRepository.AllAsNoTracking().Count();

        public IEnumerable<T> GetAll<T>()
            => this.settingsRepository.All().To<T>().ToList();
    }
}
