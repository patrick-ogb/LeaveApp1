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

namespace LeaveApp.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployee(int? pageIndex, DateTime startDate, DateTime endDate, int stateId, string? SearchText)
        {
            //if (!Microsoft.AspNetCore.Session.IsUserLoggedIn(HttpContext))
            //{
            //    return RedirectToAction("Login", "User", new { area = "Account" });
            //}

            //HolAllReconciliationVM model = new HolAllReconciliationVM();
            HoldEmployeeListVM model = new HoldEmployeeListVM();


            Pager pager = new Pager();

            EmployeeList emp  = new EmployeeList();
            var empList =  emp.GetEmployeeList(startDate, endDate);
            List< EmployeeListVM > list = new List< EmployeeListVM >();
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

            var searchItems = new EmployeeListVMSearchParams()
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

            //var Reconcilist = await GetReconciliationLists(model.ReconciliationSearchParams.StartDate, model.ReconciliationSearchParams.EndDate, model.ReconciliationSearchParams.StateId);


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
