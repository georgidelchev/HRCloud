using System.Threading.Tasks;

using HRCloud.Services.Data;
using HRCloud.Web.ViewModels.Employees;
using Microsoft.AspNetCore.Mvc;

namespace HRCloud.Web.Controllers
{
    public class EmployeesController : BaseController
    {
        private readonly IEmployeesService employeesService;

        public EmployeesController(
            IEmployeesService employeesService)
        {
            this.employeesService = employeesService;
        }

        public async Task<IActionResult> All()
        {
            var viewModel = await this.employeesService
                .GetAll<EmployeeViewModel>();

            return this.View(viewModel);
        }
    }
}
