using System.Collections.Generic;

namespace HRCloud.Services.Data
{
    public interface ISettingsService
    {
        int GetCount();

        IEnumerable<T> GetAll<T>();
    }
}
