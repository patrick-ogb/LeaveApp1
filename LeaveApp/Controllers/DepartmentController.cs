using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveApp.Core.Entities;
using LeaveApp.Security;
using LeaveApp.Service.Abstract;
using LeaveApp.ViewModel.DepartmentViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace LeaveApp.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly IDataProtector  protector;

        public DepartmentController(IDepartmentService departmentService,
                                        IDataProtectionProvider dataProtectionProvider,
                                    DataProtecionPurposeStrings dataProtecionPurposeStrings)
        {
            this.departmentService = departmentService;
            protector = dataProtectionProvider.CreateProtector(dataProtecionPurposeStrings.EmployeeIdRouteValue);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Department> departments = (await departmentService.GetDepartments()).Select(dept =>
            {
                dept.DepartmentEncryptedId = protector.Protect(dept.Id.ToString());
                return dept;
            });
            return View(departments);
        }

        [HttpGet]
        [Authorize(Policy = "CreateRolePolicy")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Policy = "CreateRolePolicy")]
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
        public async Task<IActionResult> Details(string Id)
        {
            int decryptedId;
            int y;
            if (int.TryParse(Id, out y))
            {
                decryptedId = y;
            }
            else
            {
                decryptedId = Convert.ToInt32(protector.Unprotect(Id));
            }

            var department = await departmentService.GetDepartment(decryptedId);
            
            if (department == null)
            {
                return View("NotFound", decryptedId);
            }
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            DepartmentDetailsViewModel departmentDetailsViewModel = new DepartmentDetailsViewModel
            {
                Department = department,
                EncryptedId = Id,
                PageTitle = "DEPARTMENT DETAILS"
            };
            return View(departmentDetailsViewModel);
        }


        [HttpGet]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> Edit(string Id)
        {
            int decryptedId;
            int y;
            if (int.TryParse(Id, out y))
            {
                decryptedId = y;
            }
            else
            {
                decryptedId = Convert.ToInt32(protector.Unprotect(Id));
            }

            if (decryptedId < 1)
            {
                return RedirectToAction("Index");
            }

            Department department = await departmentService.GetDepartment(decryptedId);
            DepartmentEditViewModel model = new DepartmentEditViewModel
            {
                Id = Id,
                Name = department.Name,
                DepartmentEncryptedId = Id
            };

            return View(model);
        }

        [HttpPost]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> Edit(DepartmentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                int decryptedId;
                int y;
                if (int.TryParse(model.Id, out y))
                {
                    decryptedId = y;
                }
                else
                {
                    decryptedId = Convert.ToInt32(protector.Unprotect(model.Id));
                }

                if (decryptedId < 1)
                {
                    ModelState.AddModelError(string.Empty, "Id can't be 0 or nagative number!");
                    return View(model);
                }

                Department department = await departmentService.GetDepartment(decryptedId);
                department.Name = model.Name;
                department.Description = model.Description;
                department.DateCreated = model.DateCreated;
                department.DateModified = model.DateModified;
                try
                {

                    await departmentService.UpdateDepartment(department);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Invalid input detected. please ensure the input data are correct");
                }

                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(string Id)
        {
            int deptDecryptedId = Convert.ToInt32(protector.Unprotect(Id));
            if (deptDecryptedId < 1)
            {
                return RedirectToAction("Index");
            }
            var department = await departmentService.GetDepartment(deptDecryptedId);
            return View(department);
        }

        [HttpPost]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(string Id, int Ids = 0)
        {
            int deptDecryptedId = Convert.ToInt32(protector.Unprotect(Id));

            try
            {
                await departmentService.DeleteDepartment(deptDecryptedId);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"The Department cannot be deleted becouse of reference constraint of his/her Id in another database";
                return View("CustomError");
            }
        }
    }
}