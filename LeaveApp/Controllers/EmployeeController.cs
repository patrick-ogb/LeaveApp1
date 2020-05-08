using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveApp.Core.Entities;
using LeaveApp.Service.Abstract;
using LeaveApp.ViewModel.EmployeeViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveApp.Controllers
{
    public class EmployeeController : Controller
    {
        public IEmployeeService employeeService { get; }
        public IDepartmentService departmentService { get; }
        public ILevelService levelService { get; }

        public EmployeeController(IEmployeeService employeeService,
                                    IDepartmentService departmentService,
                                    ILevelService levelService)
        {
            this.employeeService = employeeService;
            this.departmentService = departmentService;
            this.levelService = levelService;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                EmployeeListViewModel model = new EmployeeListViewModel
                {
                    Employees = await employeeService.GetEmployees(),
                    Departments = await departmentService.GetDepartments(),
                    Levels = await levelService.GetLevels()
                };

                return View(model);
            }
            catch (Exception)
            {
                return View("NotFound");
            }
        }

        public async Task<IActionResult> Create()
        {
            EmployeeCreateViewModel employeeCreateViewModel = new EmployeeCreateViewModel
            {
                DepartmentList = await departmentService.GetDepartments(),
                LevelList = await levelService.GetLevels()
            };
            return View(employeeCreateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            Employee employee = new Employee
            {
                FirstName = model.Employee.FirstName,
                LastName = model.Employee.LastName,
                Email = model.Employee.Email,
                Phone = model.Employee.Phone,
                DepartmentId = model.Employee.DepartmentId,
                LevelId = model.Employee.LevelId
            };

            var emp = await employeeService.AddEmployee(employee);
            return RedirectToAction("details", new { id = emp.Id });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? Id)
        {
            try
            {
                var employee = await employeeService.GetEmployee(Id.Value);
                var deptName = await departmentService.GetDepartment(employee.DepartmentId);
                if (employee == null || deptName == null)
                {
                    return View("EmployeeNotFound", Id.Value);
                }
                if (Id == null)
                {
                    return RedirectToAction("Index");
                }
                EmployeeDetailsViewModel employeeDetailsViewModel = new EmployeeDetailsViewModel
                {
                    Employee = employee,
                    DeptName = deptName,
                    PageTitle = "EMPLOYEE DETAILS"
                };
            return View(employeeDetailsViewModel);
            }
            catch (Exception)
            {

                return View("EmployeeNotFound", Id.Value);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var employee = await employeeService.GetEmployee(Id.Value);
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employeeChange)
        {
            if (ModelState.IsValid)
            {
                await employeeService.UpdateEmployee(employeeChange);

                return RedirectToAction("Index");
            }
            return View(employeeChange);
        }

        [HttpGet]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var employee = await employeeService.GetEmployee(Id.Value);
            return View(employee);
        }
        [HttpPost]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(int Id)
        {
            await employeeService.DeleteEmployee(Id);
            return RedirectToAction("Index");
        }

    }
}