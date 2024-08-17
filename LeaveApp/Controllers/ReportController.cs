using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using LeaveApp.ReportModel;
using LeaveApp.Extensions;
using System.Linq;
using LeaveApp.Utilities;
using System.Collections.Generic;
using System.IO;
using ClosedXML.Excel;
using System.Globalization;
using System.Text.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office.CustomUI;

namespace LeaveApp.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
         public IActionResult IndexXX(HoldEmployeeListVM model)
        {
            var result = model;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployee(int? pageIndex, DateTime startDate, DateTime endDate, int stateId, string? SearchText)
        {
            //if (!Microsoft.AspNetCore.Session.IsUserLoggedIn(HttpContext))
            //{
            //    return RedirectToAction("Login", "User", new { area = "Account" }); 36483
            //}

            int number1 = 33330;
            ViewBag.Number1 = number1.ToString("N0"); // "N0" format specifier adds commas for thousands separator
           

           double number = 3330;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            ViewBag.Number = number.ToString("N", culture);

            HoldEmployeeListVM model = new HoldEmployeeListVM();
            EmployeeListVMSearchParams searchItems = new EmployeeListVMSearchParams();
            List< EmployeeListVM > list = new List< EmployeeListVM >();
            Pager pager = new Pager();

            if (startDate == Convert.ToDateTime("01/01/0001 00:00:00") && endDate == Convert.ToDateTime("01/01/0001 00:00:00"))
            {
                model.EmployeeListVMSearchParams = searchItems;
                model.Pager = pager;
                model.HoldEmployeeListVMs = list;
                return View(model);
            }

            EmployeeList emp  = new EmployeeList();
            var empList =  emp.GetEmployeeList(startDate, endDate);

            foreach (var item in empList)
            {
                list.Add(new EmployeeListVM
                {
                    EmpId = item.EmpId,
                    Name = item.Name,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                });
            }

            model.HoldEmployeeListVMs = list;

            

            pager = new Pager(model.HoldEmployeeListVMs.Count(), pageIndex, 5);

            model.Pager = pager;
            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchText = SearchText.Trim();
                model.HoldEmployeeListVMs = model.HoldEmployeeListVMs.Where(c => c.Name.Contains(SearchText)).ToList();
            }
            else
            {
                model.HoldEmployeeListVMs = model.HoldEmployeeListVMs.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();
            }


            model.totalCount = empList.Count();

             searchItems = new EmployeeListVMSearchParams()
            {
                StartDate = startDate,
                EndDate = endDate,
            };
            model.EmployeeListVMSearchParams = searchItems;

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> GetAllEmployee(HoldEmployeeListVM model, int? pageIndex)
        {

            EmployeeList emp = new EmployeeList();
            var empList = emp.GetEmployeeList(model.EmployeeListVMSearchParams.StartDate, model.EmployeeListVMSearchParams.EndDate);

            if (empList == null)
            {
                ViewBag.MessageX = "No data to be displayed for the selected period...";
                return View();
            }

            List<EmployeeListVM> list = new List<EmployeeListVM>();
            foreach (var item in empList)
            {
                list.Add(new EmployeeListVM
                {
                    EmpId = item.EmpId,
                    Name = item.Name,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                });
            }

            model.HoldEmployeeListVMs = list;


            var pager = new Pager(model.HoldEmployeeListVMs.Count(), pageIndex, 5);

            model.Pager = pager;

            model.HoldEmployeeListVMs = model.HoldEmployeeListVMs.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();

            model.totalCount = empList.Count();

            return View(model);
        }

        [HttpPost]
        public IActionResult EmployeeCreate(HoldEmployeeListVM model)
        {
            if (model.EmployeeEditMultipleVM.SelectedList != null)
            {
                var id = 0;
                var results = model.EmployeeEditMultipleVM.SelectedList.Split(',');
                foreach (var item in results)
                {
                    id = Convert.ToInt32(item.Split('_')[1]);
                }
            }
            return View();
        }

        public JsonResult PostEmployee(HoldEmployeeListVM model)
        {
            var formData = model;
            var serializer = JsonSerializer.Serialize(formData);
            return Json(model);
        }

        public JsonResult PostEmployeeNew(string emp)
        {
            var formData = "";
            if(emp != null)
            {
                formData = emp;
                //var serializer = JsonSerializer.Serialize(formData);
            }
            return Json(formData);
        }

        public IActionResult ViewEmployeeDetails(int EmployeeId)
        {
            EmployeeList emp = new EmployeeList();
            var empList = emp.GetEmployeeList(DateTime.Now, DateTime.Now).FirstOrDefault(x => x.EmpId == EmployeeId);
            HoldEmployeeListVM model = new HoldEmployeeListVM();
            model.EmployeeList = empList;
            return PartialView("_EmployeeDetail", model);
        }

        public async Task<IActionResult> ExportEmployeeReporToExcell(DateTime startDate, DateTime endDate)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Employees.xlsx";
            try
            {
                EmployeeList emp = new EmployeeList();
                var empList = emp.GetEmployeeList(startDate, endDate);

                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Employees");
                    worksheet.Cell(1, 1).Value = "No";
                    worksheet.Cell(1, 2).Value = "Name";
                    worksheet.Cell(1, 3).Value = "Start Date";
                    worksheet.Cell(1, 4).Value = "End Date";

                    for (int index = 1; index <= empList.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = empList[index - 1].EmpId;
                        worksheet.Cell(index + 1, 2).Value = empList[index - 1].Name;
                        worksheet.Cell(index + 1, 3).Value = empList[index - 1].StartDate;
                        worksheet.Cell(index + 1, 4).Value = empList[index - 1].EndDate;

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return RedirectToAction("HttpStatusCodeHandler", "Error");
            }
        }

    }
}
