using System.Collections.Generic;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using LeaveApp.ReportModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LeaveApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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


        public  string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            // System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get your key from config file to open the lock!
            string key = _configuration["EAString"];
            // Dim key As String = DirectCast("CBSCLIENT1", String)

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }


        public IActionResult CheckInOut(string SessionValue)
        {

            string[] sses = SessionValue.Split("!!!");
           

            return PartialView("_CheckInOut");
        }



    }
}
