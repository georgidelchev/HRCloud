﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using HRCloud.Data.Common.Repositories;
using HRCloud.Data.Models;
using HRCloud.Services.Data.Interfaces;
using HRCloud.Services.Mapping;
using HRCloud.Services.Messaging;
using HRCloud.Web.ViewModels.Employees;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HRCloud.Services.Data
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmailSender emailSender;
        private readonly IServiceProvider serviceProvider;
        private readonly IDepartmentsService departmentsService;
        private readonly IFileProcessingService fileProcessingService;
        private readonly IDeletableEntityRepository<ApplicationUser> employeesRepository;

        public EmployeesService(
            IEmailSender emailSender,
            IServiceProvider serviceProvider,
            IDepartmentsService departmentsService,
            IFileProcessingService fileProcessingService,
            IDeletableEntityRepository<ApplicationUser> employeesRepository)
        {
            this.emailSender = emailSender;
            this.serviceProvider = serviceProvider;
            this.employeesRepository = employeesRepository;
            this.fileProcessingService = fileProcessingService;
            this.departmentsService = departmentsService;
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
                DepartmentId = this.departmentsService.GetIdByName(input.DepartmentName),
                JobId = input.JobId,
                Salary = input.Salary,
                PhoneNumber = input.PhoneNumber,
                ApplicationUserId = input.MentorId,
            };

            var employeeFolderPath = $"/img/Employees/{user.Id.Split('-').FirstOrDefault()}_{user.FirstName} {user.Surname} {user.LastName}/";
            user.ImageUrl = employeeFolderPath + await this.fileProcessingService.SaveImageLocallyAsync(input.Image, webRoot + employeeFolderPath);
            user.WelcomeCardUrl = employeeFolderPath + await this.fileProcessingService.SaveWelcomeCardAsync(input.WelcomeCard, webRoot + employeeFolderPath, $"{user.FirstName}{user.Surname}{user.LastName}");

            await userManager.CreateAsync(user, input.Password);

            if (!string.IsNullOrEmpty(input.MentorId))
            {
                await this.SetMentorAsync(input.MentorId);
            }

            //await this.SendMailWithWelcomeCardAttachmentToAll(input.WelcomeCard);

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

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKvp(string departmentName)
            => await this.employeesRepository
                .All()
                .Where(e => e.ApplicationUserId == null && e.IsMentor == false && e.Department.Name == departmentName)
                .Select(kvp => new KeyValuePair<string, string>(kvp.Id, $"{kvp.FirstName} {kvp.Surname} {kvp.LastName}"))
                .ToListAsync();

        public bool IsEmailExists(string email)
            => this.employeesRepository
                .AllWithDeleted()
                .Any(e => e.Email == email);

        private async Task SetMentorAsync(string id)
        {
            var employee = this.employeesRepository
                .All()
                .FirstOrDefault(e => e.Id == id);

            employee.IsMentor = true;

            this.employeesRepository.Update(employee);
            await this.employeesRepository.SaveChangesAsync();
        }

        private async Task SendMailWithWelcomeCardAttachmentToAll(IFormFile welcomeCard)
        {
            await using var ms = new MemoryStream();

            await welcomeCard.CopyToAsync(ms);
            var welcomeCardBytes = ms.ToArray();

            var attachments = new List<EmailAttachment>
            {
                new()
                {
                    Content = welcomeCardBytes,
                    FileName = welcomeCard.FileName,
                },
            };

            foreach (var employee in this.employeesRepository.All())
            {
                await this.emailSender.SendEmailAsync(
                    from: "allyouneedplatform@gmail.com",
                    fromName: "HRCloud",
                    to: employee.Email,
                    subject: $"New Teammate - {employee.FirstName} {employee.Surname} {employee.LastName}",
                    htmlContent: "Welcome ;).",
                    attachments: attachments);
            }
        }
    }
}
