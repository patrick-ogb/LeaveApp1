using Microsoft.AspNetCore.Mvc;

namespace LeaveApp.Controllers
{
    using LeaveApp.Global;
    using LeaveApp.Services.IServices;
    using LeaveApp.ViewModel;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;

    public class UserController : Controller
    {
        private readonly IProcessorService _processorService;

        public UserController(IProcessorService processorService)
        {
            _processorService = processorService;
        }
        public IActionResult Create()
        {
            var model = new UserViewModel
            {
                AvailableRoles = GetAvailableRoles()
            };
            QRCodeVM qRCodeVM = new QRCodeVM()
            {
                Key1 = "Remita Reference Number: ",
                Value1 = "RMT5324243fgfh34443",
                Key2 = "ReferenceNo: ",
                Value2 = "DDF4363747373",
                Key3 = "Invoice Date: ",
                Value3 = DateTime.Now.ToString(),
                Key4 = "Total Amount: ",
                Value4 = "6463653",
                Key5 = "Applicant Name: ",
                Value5 = "Sir Noble",
                Key6 = "Applicant Phone: ",
                Value6 = "08105793993",
                Key7 = "Product Name: ",
                Value7 = "Balister Rocket",
            };       
            model.QRCode = _processorService.GenerateQRCode(qRCodeVM);
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle the form submission, e.g., save data to the database
            }

            // Repopulate the AvailableRoles in case of validation errors
            model.AvailableRoles = GetAvailableRoles();
            return View(model);
        }

        private List<SelectListItem> GetAvailableRoles()
        {
            // This would typically come from a database or other data source
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Admin" },
            new SelectListItem { Value = "2", Text = "User" },
            new SelectListItem { Value = "3", Text = "Guest" },
            new SelectListItem { Value = "4", Text = "New" },
            new SelectListItem { Value = "5", Text = "Best" },
            new SelectListItem { Value = "6", Text = "You" },
            new SelectListItem { Value = "7", Text = "Roar" },
        };
        }
    }

    

}
