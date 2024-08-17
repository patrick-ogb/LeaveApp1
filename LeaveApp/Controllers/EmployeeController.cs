using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveApp.Core.Entities;
using LeaveApp.Security;
using LeaveApp.Service.Abstract;
using LeaveApp.ViewModel.EmployeeViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;

namespace LeaveApp.Controllers
{
    public class EmployeeController : Controller
    {
        public IEmployeeService employeeService { get; }
        public IDepartmentService departmentService { get; }
        public ILevelService levelService { get; }
        private readonly IDataProtector protector;

        public EmployeeController(IEmployeeService employeeService,
                                    IDepartmentService departmentService,
                                    ILevelService levelService,
                                    IDataProtectionProvider dataProtectionProvider,
                                    DataProtecionPurposeStrings dataProtecionPurposeStrings)
        {
            this.employeeService = employeeService;
            this.departmentService = departmentService;
            this.levelService = levelService;
            protector = dataProtectionProvider.CreateProtector(dataProtecionPurposeStrings.EmployeeIdRouteValue);
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(EmployeeListViewModel? param)
        {
            try
            {
                IEnumerable<Employee> Employees = (await employeeService.GetEmployees()).Select(emp =>
                {
                    emp.EmployeeEncryptedId = protector.Protect(emp.Id.ToString());
                    return emp;
                });

                var Departments = await departmentService.GetDepartments();
                var Levels = await levelService.GetLevels();
                
                EmployeeListViewModel model = new EmployeeListViewModel
                {
                    Employees = Employees,
                    Departments = Departments,
                    Levels = Levels,
                    
                };
                foreach (var item in Employees)
                {
                    model.empInfo = new EmpInfo { EmpInfoID = item.Id.ToString(), EmpInfoName = item.FirstName + " " + item.LastName };
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorTitle = "Resource not available:";
                ViewBag.ErrorMessage = ex.Message;
                return View("CustomError");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(EmployeeListViewModel? param, int? msg = 0)
        {
            if (param != null)
                return RedirectToAction(nameof(Index), new TestNullableClass { ClassId = param.empInfo.EmpInfoID, ClassName = param.empInfo.EmpInfoName });
            return View(param);
        }

        public async Task<IActionResult> Print(int id)
        {

            return View();
        }


        [HttpGet("GeneratePdf")]
        public IActionResult GeneratePdf()
        {
            try
            {
                // Create a memory stream to hold the PDF data
                using (var memoryStream = new MemoryStream())
                {
                    // Initialize the PDF writer and document
                    PdfWriter writer = new PdfWriter(memoryStream);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);

                    // Add a paragraph to the PDF
                    document.Add(new Paragraph("Hello, this is a PDF generated in ASP.NET Core!"));

                    // Add another paragraph with more styling
                    document.Add(new Paragraph("This is another paragraph with bold text.")
                        .SetBold());

                    // Close the document
                    document.Close();

                    // Convert the MemoryStream to a byte array and return as a file
                    var bytes = memoryStream.ToArray();
                    //return File(bytes, "application/pdf", "GeneratedPdf.pdf");
                    var result = File(bytes, "application/pdf", "GeneratedPdf.pdf");
                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }







        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Edit(EmployeeListViewModel? param, int? msg = 0)
        {
            if (param != null)
                return RedirectToAction(nameof(Edit), new TestNullableClass { ClassId = param.empInfo.EmpInfoID, ClassName = param.empInfo.EmpInfoName });
            return View(param);
        }

        [HttpGet]
        //[Authorize(Policy = "CreateRolePolicy")]
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
        //[Authorize(Policy = "CreateRolePolicy")]
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
            var employee = await employeeService.GetEmployee(decryptedId);
            var deptName = await departmentService.GetDepartment(employee.DepartmentId);
            var level = await levelService.GetLevel(employee.LevelId);
            if (employee == null)
            {
                return View("NotFound", decryptedId);
            }
            if (decryptedId < 1)
            {
                return RedirectToAction("Index");
            }
            EmployeeDetailsViewModel employeeDetailsViewModel = new EmployeeDetailsViewModel
            {
                Employee = employee,
                DeptName = deptName,
                Level = level,
                EncryptedId = Id,
                PageTitle = "EMPLOYEE DETAILS"
            };
            return View(employeeDetailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id = null, EmployeeCreateViewModel param = null)
        {
            int decryptedId;
            
            int y;
            

            var departments = await departmentService.GetDepartments();
            var level = await levelService.GetLevels();
            Employee employee = new Employee();
            if (Id == null)
            {
                //employee = (await employeeService.GetEmployees()).FirstOrDefault();

                //if (int.TryParse(param.empInfo., out y))
                //{
                //    decryptedId = y;
                //}
                //else
                //{
                //    decryptedId = Convert.ToInt32(protector.Unprotect(Id));
                //}
            }
            else
            {
                if (int.TryParse(Id, out y))
                {
                    decryptedId = y;
                }
                else
                {
                    decryptedId = Convert.ToInt32(protector.Unprotect(Id));
                }
                employee = await employeeService.GetEmployee(decryptedId);
            }
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                LeaveRequests = employee.LeaveRequests,
                LevelId = employee.LevelId,
                DateCreated = employee.DateCreated,
                DepartmentId = employee.DepartmentId,
                DateModified = employee.DateModified,
                EmployeeEncryptedId = Id,
                DepartmentList = departments,
                LevelList = level
            };
            
            //if (decryptedId < 1)
            //{
            //    return RedirectToAction("Index");
            //}
            return View(employeeEditViewModel);
        }

        

        [HttpPost]
        public async Task<IActionResult> Edit2(EmployeeEditViewModel model)
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
                Employee employee = await employeeService.GetEmployee(decryptedId);
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.Phone = model.Phone;
                employee.LevelId = model.LevelId;
                employee.LeaveRequests = model.LeaveRequests;
                employee.DepartmentId = model.DepartmentId;
                employee.DateCreated = model.DateCreated;
                employee.DateModified = model.DateModified;
                employee.Email = model.Email;

                try
                {
                    await employeeService.UpdateEmployee(employee);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message + "\n" + "  Ensure both department and level Ids are correct");
                }
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(string Id)
        {
            int empDecryptedId = Convert.ToInt32(protector.Unprotect(Id));
            if (empDecryptedId < 1)
            {
                return RedirectToAction("Index");
            }
            var employee = await employeeService.GetEmployee(empDecryptedId);
            return View(employee);
        }

        [HttpPost]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(string Id, int Ids = 0)
        {
            int empDecryptedId = Convert.ToInt32(protector.Unprotect(Id));

            try
            {
                await employeeService.DeleteEmployee(empDecryptedId);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"The Emplyee cannot be deleted becouse of reference constraint of his/her Id in another database";
                return View("CustomError");
            }
        }
    }

    public class TestNullableClass
    {
        public string? ClassId { get; set; }
        public string? ClassName { get; set; }
    }
}