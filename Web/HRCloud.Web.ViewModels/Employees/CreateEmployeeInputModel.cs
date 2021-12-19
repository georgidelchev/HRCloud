using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HRCloud.Web.ViewModels.Employees
{
    public class CreateEmployeeInputModel
    {
        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password
            => Guid.NewGuid().ToString();

        public string PhoneNumber { get; set; }

        public decimal Salary { get; set; }

        public IFormFile Image { get; set; }

        [FileExtensions(Extensions = ".pdf")]
        public IFormFile WelcomeCard { get; set; }

        public string DepartmentName { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Jobs { get; set; }

        public int JobId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Mentors { get; set; }

        public string MentorId { get; set; }
    }
}
