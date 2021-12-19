using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRCloud.Services.Data.Interfaces
{
    public interface IJobsService
    {
        Task<IEnumerable<KeyValuePair<int, string>>> GetAllAsKvp();
    }
}
