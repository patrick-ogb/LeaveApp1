using LeaveApp.Core.Entities;
using LeaveApp.Extensions;
using LeaveApp.Global;
using LeaveApp.ReportModel;
using LeaveApp.Security;
using LeaveApp.Service.Abstract;
using LeaveApp.Service.Concrete;
using LeaveApp.Services.IServices;
using LeaveApp.Utilities;
using LeaveApp.ViewModel.EmployeeViewModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using LeaveApp.ResponseViM;

namespace LeaveApp.Services
{
    public class ProcessorService : IProcessorService
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

        public string GenerateQRCode(QRCodeVM model)
        {
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                StringBuilder stringToCheck = new StringBuilder();
                stringToCheck.Append(model.Key1 + model.Value1);
                stringToCheck.AppendLine();
                stringToCheck.Append(model.Key2 + model.Value2);
                stringToCheck.AppendLine();
                stringToCheck.Append(model.Key3 + model.Value3);
                stringToCheck.AppendLine();
                stringToCheck.Append(model.Key4 + model.Value4);
                stringToCheck.AppendLine();
                stringToCheck.Append(model.Key5 + model.Value5);
                stringToCheck.AppendLine();
                stringToCheck.Append(model.Key6 + model.Value6);
                stringToCheck.AppendLine();
                stringToCheck.Append(model.Key7 + model.Value7);
                stringToCheck.AppendLine();
                stringToCheck.Append(Convert.ToString("For more information click : ") + "https://localhost:7038/ApplicationVerification");

                QRCodeData qrCodeData = qrGenerator.CreateQrCode(stringToCheck.ToString(), QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                string ImageUrl = "";
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    }
                }
                return ImageUrl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    public static ResponseObject EgolePay_CheckWallet()
    {
        try
        {
            string baseurl = "https://services.egolepay.com";
                string url = "/api/Account/AuthService";
                string key = "PlI34D1jFKHIwLYov9C9Fr2baO8LuoudAInoORo5pmlX0nSnc50k93V9ASDqMr+gTBHKnp9ve+z2ktZueGYHpnCCR+SzD1OxbIX8zsFiX/ZLl+cu49tGNVNogwO8/yCw";
            var client = new RestClient(baseurl);
            var request = new RestRequest(url, Method.Post);

            request.AddJsonBody(new { apikey = key });

            RestResponse response = client.Execute(request);
            var responseDada = response.Content;
                var data = JsonConvert.DeserializeObject<ResponseObject>(responseDada);

                return data;
        }
        catch (Exception ex)
        {
            return new ResponseObject();
        }
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
