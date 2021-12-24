using System.Threading.Tasks;

using HRCloud.Services.Data.Interfaces;
using HRCloud.Web.ViewModels.Employees;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HRCloud.Web.Controllers
{
    public class EmployeesController : BaseController
    {
        private readonly IEmployeesService employeesService;
        private readonly IDepartmentsService departmentsService;
        private readonly IJobsService jobsService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EmployeesController(
            IEmployeesService employeesService,
            IDepartmentsService departmentsService,
            IJobsService jobsService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.employeesService = employeesService;
            this.departmentsService = departmentsService;
            this.jobsService = jobsService;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> All(string departmentName)
        {
            var viewModel = new ListEmployeesViewModel
            {
                Employees = await this.employeesService.GetAllAsync<EmployeeViewModel>(departmentName),
                DepartmentName = departmentName,
            };
            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string departmentSource)
        {
            var viewModel = new CreateEmployeeInputModel
            {
                Mentors = await this.employeesService.GetAllAsKvpAsync(departmentSource),
                Jobs = await this.jobsService.GetAllAsKvp(),
                DepartmentName = departmentSource,
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeInputModel input)
        {
            if (!this.ModelState.IsValid || input.Image == null)
            {
                var viewModel = new CreateEmployeeInputModel
                {
                    Mentors = await this.employeesService.GetAllAsKvpAsync(input.DepartmentName),
                    Jobs = await this.jobsService.GetAllAsKvp(),
                    DepartmentName = input.DepartmentName,
                };
                return this.View(viewModel);
            }

            await this.employeesService.CreateAsync(input, this.webHostEnvironment.WebRootPath);

            var redirectUrl = $"/{this.ControllerContext.ActionDescriptor.ControllerName}/{nameof(this.All)}?departmentName={input.DepartmentName}";

            return this.Redirect(redirectUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (!this.employeesService.IsEmployeeExistById(id))
            {
                // TODO: Add error!
            }

            var viewModel = await this.employeesService
                .GetDetailsAsync<EmployeeDetailsViewModel>(id);

            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (this.employeesService.IsEmployeeExistById(id))
            {
                // TODO: Add validation
            }

            await this.employeesService.DeleteAsync(id);

            return this.Redirect("/");
        }

        // Used in Remote Validation
        [HttpGet]
        public IActionResult DoesEmailExists(string email)
        {
            var result = this.employeesService
                .IsEmailExist(email);
            return this.Json(!result);
        }

        [HttpGet]
        public IActionResult IsSalaryValid(int jobId, decimal salary)
        {
            var result = this.jobsService
                .IsJobSalaryValid(jobId, salary);
            return this.Json(result);
        }
    }
}
