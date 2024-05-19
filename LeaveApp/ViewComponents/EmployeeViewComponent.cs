using LeaveApp.Core.Entities;
using LeaveApp.Security;
using LeaveApp.Service.Abstract;
using LeaveApp.Service.Concrete;
using LeaveApp.ViewModel.EmployeeViewModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewComponents
{
    public class EmployeeViewComponent: ViewComponent
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly ILevelService _levelService;
        private readonly IDataProtector protector;

        public EmployeeViewComponent(IEmployeeService employeeService, IDepartmentService departmentService, 
            ILevelService levelService, DataProtecionPurposeStrings dataProtecionPurposeStrings, IDataProtectionProvider dataProtectionProvider)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _levelService = levelService;
            protector = dataProtectionProvider.CreateProtector(dataProtecionPurposeStrings.EmployeeIdRouteValue);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<Employee> Employees = (await _employeeService.GetEmployees()).Select(emp =>
            {
                emp.EmployeeEncryptedId = protector.Protect(emp.Id.ToString());
                return emp;
            });

            var Departments = await _departmentService.GetDepartments();
            var Levels = await _levelService.GetLevels();

            EmployeeListViewModel model = new EmployeeListViewModel
            {
                Employees = Employees,
                Departments = Departments,
                Levels = Levels
            };

            return View(model);
        }
    }
}
