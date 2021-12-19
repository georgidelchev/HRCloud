using System;

using HRCloud.Data.Models;
using HRCloud.Services.Mapping;

namespace HRCloud.Web.ViewModels.Employees
{
    public class EmployeeViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public DateTime HireDate { get; set; }

        public string HireDateAsString
            => this.HireDate.ToString("g");

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string DepartmentName { get; set; }

        public string FirstName { get; set; }

        public string SurName { get; set; }

        public string LastName { get; set; }

        public string FullName
            => $"{this.FirstName} {this.SurName} {this.LastName}";

        public string ImageUrl { get; set; }

        public string JobName { get; set; }

        public string MentorName { get; set; }

        public string Salary { get; set; }
    }
}
