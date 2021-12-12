using System.Collections.Generic;
using System.Threading.Tasks;

using HRCloud.Data.Common.Repositories;
using HRCloud.Data.Models;
using HRCloud.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HRCloud.Services.Data
{
    public class DepartmentsService : IDepartmentsService
    {
        private readonly IDeletableEntityRepository<Department> departmentsRepository;

        public DepartmentsService(IDeletableEntityRepository<Department> departmentsRepository)
        {
            this.departmentsRepository = departmentsRepository;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
            => await this.departmentsRepository
                .All()
                .Include(d => d.Employees)
                .To<T>()
                .ToListAsync();
    }
}
