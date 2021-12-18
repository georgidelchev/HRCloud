using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCloud.Services.Data
{
    public interface IEmployeesService
    {
        Task<IEnumerable<T>> GetAll<T>();

        Task<IEnumerable<T>> GetAllByDepartmentName<T>(string departmentName);
    }
}
