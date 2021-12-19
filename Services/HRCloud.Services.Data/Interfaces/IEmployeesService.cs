using System.Collections.Generic;
using System.Threading.Tasks;

using HRCloud.Web.ViewModels.Employees;

namespace HRCloud.Services.Data.Interfaces
{
    public interface IEmployeesService
    {
        Task<bool> Create(CreateEmployeeInputModel input);

        Task<IEnumerable<T>> GetAll<T>(string departmentName);

        Task<IEnumerable<T>> GetAllByDepartmentName<T>(string departmentName);

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKvp();
    }
}
