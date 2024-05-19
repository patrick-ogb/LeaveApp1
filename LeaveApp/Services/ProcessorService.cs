using LeaveApp.Core.Entities;
using LeaveApp.Extensions;
using LeaveApp.ReportModel;
using LeaveApp.Security;
using LeaveApp.Service.Abstract;
using LeaveApp.Service.Concrete;
using LeaveApp.Services.IServices;
using LeaveApp.Utilities;
using LeaveApp.ViewModel.EmployeeViewModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.Services
{
    public class ProcessorService: IProcessorService
    {
        private readonly ILevelService _levelService;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IDataProtector protector;

        public ProcessorService(ILevelService levelService,
                IEmployeeService employeeService,
                IDepartmentService departmentService,
                                     IDataProtectionProvider dataProtectionProvider,
                                     DataProtecionPurposeStrings dataProtecionPurposeStrings)
        {
            _levelService = levelService;
            _employeeService = employeeService;
            _departmentService = departmentService;
            protector = dataProtectionProvider.CreateProtector(dataProtecionPurposeStrings.EmployeeIdRouteValue);
        }


        public async Task<HoldLevelVM> Processlevel(int? pageIndex, string? SearchText)
        {
            HoldLevelVM model = new HoldLevelVM();
            List<LevelVM> list = new List<LevelVM>();
            Pager pager = new Pager();

            //EmployeeList emp = new EmployeeList();
            var levels = await _levelService.GetLevels();

            foreach (var item in levels)
            {
                list.Add(new LevelVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    DateCreated = item.DateModified,
                    Description = item.Description,
                    LevelEncryptedId = protector.Protect(item.Id.ToString())
                });
            }

            model.HoldLevelVMs = list;

            pager = new Pager(model.HoldLevelVMs.Count(), pageIndex, 5);

            model.Pager = pager;
            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchText = SearchText.Trim();
                model.HoldLevelVMs = model.HoldLevelVMs.Where(c => c.Name.Contains(SearchText)).ToList();
            }
            else
            {
                model.HoldLevelVMs = model.HoldLevelVMs.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();
            }


            model.totalCount = list.Count();


            return model;
        }

        public async Task<HoldDepartmentVM> ProcessDepartment(int? pageIndex, string? SearchText)
        {
            HoldDepartmentVM model = new HoldDepartmentVM();
            List<DepartmentVM> list = new List<DepartmentVM>();
            Pager pager = new Pager();

            //EmployeeList emp = new EmployeeList();
            var dpts = await _departmentService.GetDepartments();

            foreach (var item in dpts)
            {
                list.Add(new DepartmentVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    DateCreated = item.DateModified,
                    Description = item.Description,
                    DepartmentEncryptedId = protector.Protect(item.Id.ToString())
                });
            }

            model.HoldDepartmentVMs = list;

            pager = new Pager(model.HoldDepartmentVMs.Count(), pageIndex, 5);

            model.Pager = pager;
            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchText = SearchText.Trim();
                model.HoldDepartmentVMs = model.HoldDepartmentVMs.Where(c => c.Name.Contains(SearchText)).ToList();
            }
            else
            {
                model.HoldDepartmentVMs = model.HoldDepartmentVMs.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();
            }


            model.totalCount = list.Count();


            return model;
        }

        //public async Task<HoldEmployeeVM> ProcessEmployee(int? pageIndex, string? SearchText)
        //{
        //    HoldEmployeeVM model = new HoldEmployeeVM();
        //    List<EmployeeListVM> list = new List<EmployeeListVM>();
        //    Pager pager = new Pager();

        //    //EmployeeList emp = new EmployeeList();
        //    var employees = await _employeeService.GetEmployees();

        //    IEnumerable<Employee> Employees = (await _employeeService.GetEmployees()).Select(emp =>
        //    {
        //        emp.EmployeeEncryptedId = protector.Protect(emp.Id.ToString());
        //        return emp;
        //    });

        //    var Departments = await _departmentService.GetDepartments();
        //    var Levels = await _levelService.GetLevels();



        //    model.EmployeeListVMs = Employees;

        //    pager = new Pager(model.EmployeeListVMs.Employees.Count(), pageIndex, 5);

        //    model.Pager = pager;
        //    if (!string.IsNullOrEmpty(SearchText))
        //    {
        //        SearchText = SearchText.Trim();
        //        model.HoldLevelVMs = model.HoldLevelVMs.Where(c => c.Name.Contains(SearchText)).ToList();
        //    }
        //    else
        //    {
        //        model.HoldLevelVMs = model.HoldLevelVMs.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();
        //    }

        //    //EmployeeListViewModel empList = new EmployeeListViewModel
        //    //{
        //    //    Employees = Employees,
        //    //    Departments = Departments,
        //    //    Levels = Levels
        //    //};

        //    model.totalCount = list.Count();


        //    return model;
        //}
    }
}
