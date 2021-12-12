using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCloud.Services.Data
{
    public interface IDepartmentsService
    {
        Task<IEnumerable<T>> GetAll<T>();
    }
}
