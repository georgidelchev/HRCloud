using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HRCloud.Data.Common.Repositories;
using HRCloud.Data.Models;
using HRCloud.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace HRCloud.Services.Data
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> employeesRepository;

        public EmployeesService(
            IDeletableEntityRepository<ApplicationUser> employeesRepository)
        {
            this.employeesRepository = employeesRepository;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
            => await this.employeesRepository
                .All()
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> GetAllByDepartmentName<T>(string departmentName)
            => await this.employeesRepository
                .All()
                .Where(e => e.Department.Name == departmentName)
                .To<T>()
                .ToListAsync();
    }
}
