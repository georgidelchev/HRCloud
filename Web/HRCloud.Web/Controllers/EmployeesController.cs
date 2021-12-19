using System.Threading.Tasks;

using HRCloud.Services.Data;
using HRCloud.Services.Data.Interfaces;
using HRCloud.Web.ViewModels.Employees;
using Microsoft.AspNetCore.Mvc;

namespace HRCloud.Web.Controllers
{
    public class EmployeesController : BaseController
    {
        private readonly IEmployeesService employeesService;
        private readonly IDepartmentsService departmentsService;
        private readonly IJobsService jobsService;

        public EmployeesController(
            IEmployeesService employeesService,
            IDepartmentsService departmentsService,
            IJobsService jobsService)
        {
            this.employeesService = employeesService;
            this.departmentsService = departmentsService;
            this.jobsService = jobsService;
        }

        [HttpGet]
        public async Task<IActionResult> All(string departmentName)
        {
            var viewModel = new ListEmployeesViewModel
            {
                Employees = await this.employeesService.GetAll<EmployeeViewModel>(departmentName),
                DepartmentName = departmentName,
            };
            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string departmentSource)
        {
            var viewModel = new CreateEmployeeInputModel()
            {
                Managers = await this.employeesService.GetAllAsKvp(),
                Departments = await this.departmentsService.GetAllAsKvp(),
                Jobs = await this.jobsService.GetAllAsKvp(),
                DepartmentId = this.departmentsService.GetIdByName(departmentSource),
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeInputModel input)
        {
            await this.employeesService.Create(input);
            var redirectUrl = $"/{this.ControllerContext.ActionDescriptor.ControllerName}/{nameof(this.All)}?departmentName={this.departmentsService.GetNameById(input.DepartmentId)}";

            return this.Redirect(redirectUrl);
        }
    }
}
