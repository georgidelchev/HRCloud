using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HRCloud.Data.Common.Repositories;
using HRCloud.Data.Models;
using HRCloud.Services.Data.Interfaces;
using HRCloud.Services.Mapping;
using HRCloud.Web.ViewModels.Employees;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HRCloud.Services.Data
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDeletableEntityRepository<ApplicationUser> employeesRepository;
        private readonly IImageProcessingService imageProcessingService;

        public EmployeesService(
            IServiceProvider serviceProvider,
            IDeletableEntityRepository<ApplicationUser> employeesRepository,
            IImageProcessingService imageProcessingService)
        {
            this.serviceProvider = serviceProvider;
            this.employeesRepository = employeesRepository;
            this.imageProcessingService = imageProcessingService;
        }

        public async Task<bool> Create(CreateEmployeeInputModel input, string webRoot)
        {

            var userManager = this.serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var user = new ApplicationUser
            {
                Email = input.Email,
                CreatedOn = DateTime.UtcNow,
                UserName = input.Email,
                FirstName = input.FirstName,
                Surname = input.SurName,
                LastName = input.LastName,
                HireDate = DateTime.UtcNow,
                DepartmentId = input.DepartmentId,
                JobId = input.JobId,
                Salary = input.Salary,
                PhoneNumber = input.PhoneNumber,
            };

            var imageUrl = $"/img/Employees/{user.Id.Split('-').FirstOrDefault()}_{user.FirstName} {user.Surname} {user.LastName}/";
            user.ImageUrl = imageUrl + await this.imageProcessingService.SaveImageLocallyAsync(input.Image, webRoot + imageUrl);

            await userManager.CreateAsync(user, input.Password);

            return true;
        }

        public async Task<IEnumerable<T>> GetAll<T>(string departmentName)
        {
            var allEmployees = this.employeesRepository
                .All();

            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                allEmployees = allEmployees
                    .Where(e => e.Department.Name == departmentName);
            }

            return await allEmployees
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByDepartmentName<T>(string departmentName)
            => await this.employeesRepository
                .All()
                .Where(e => e.Department.Name == departmentName)
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKvp()
            => await this.employeesRepository
                .All()
                .Select(kvp => new KeyValuePair<string, string>(kvp.Id, $"{kvp.FirstName} {kvp.Surname} {kvp.LastName}"))
                .ToListAsync();

        public string GetFullNameById(string id)
            => this.employeesRepository
                .All()
                .Where(e => e.Id == id)
                .Select(e => $"{e.FirstName} {e.Surname} {e.LastName}")
                .FirstOrDefault();
    }
}
