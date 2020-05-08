using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveApp.Core.Entities;
using LeaveApp.Service.Abstract;
using LeaveApp.ViewModel.DepartmentViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveApp.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var department = await departmentService.GetDepartments();
            return View(department);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Department department = new Department
                {
                    Name = model.Department.Name,
                    Description = model.Department.Description
                };

                await departmentService.AddDepartment(department);

                ViewBag.message = $"Employee {model.Department.Name} is successfullly saved to DataBase";

                return RedirectToAction("details", new { id = department.Id });
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? Id)
        {
            var department = await departmentService.GetDepartment(Id.Value);
            if (department == null)
            {
                return View("NotFound", Id.Value);
            }
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            DepartmentDetailsViewModel departmentDetailsViewModel = new DepartmentDetailsViewModel
            {
                Department = department,
                PageTitle = "DEPARTMENT DETAILS"
            };
            return View(departmentDetailsViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var department = await departmentService.GetDepartment(Id.Value);
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department departmentChange)
        {
            if (ModelState.IsValid)
            {
                await departmentService.UpdateDepartment(departmentChange);

                return RedirectToAction("Index");
            }
            return View(departmentChange);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var department = await departmentService.GetDepartment(Id.Value);
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            await departmentService.DeleteDepartment(Id);
            return RedirectToAction("Index");
        }
    }
}