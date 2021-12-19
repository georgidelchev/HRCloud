using System.Collections.Generic;

namespace HRCloud.Services.Data.Interfaces
{
    public interface ISettingsService
    {
        int GetCount();

        IEnumerable<T> GetAll<T>();
    }
}
