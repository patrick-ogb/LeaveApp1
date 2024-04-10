using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using LeaveApp.ReportModel;
using System.Linq;

namespace LeaveApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {


            try
            {
                //int first = 20, sec = 0, div = 0;
                //div = first / sec;


                //TempData["value"] = "someValueForNextRequest";
                //object value = TempData.Peek("value");


                //TempData.Peek("Td").ToString();

                return View();
            }
            catch (System.Exception ex)
            {
                Log.Logger.Error("Error => {@error}", $"Message: {ex.Message}. Source: {ex.Source}. StackTrace: {ex.StackTrace}");
               return RedirectToAction(nameof(Privacy));
                throw;
            }
            

        }

        public IActionResult Privacy()
        {
            return View();
        }



        
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]




    }
}
