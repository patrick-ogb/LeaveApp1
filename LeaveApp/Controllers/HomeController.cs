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
using QRCoder;
using static QRCoder.PayloadGenerator;
using LeaveApp.Services.IServices;
using LeaveApp.Services;

namespace LeaveApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IProcessorService _processorService;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IProcessorService processorService)
        {
            _logger = logger;
            _configuration = configuration;
            _processorService = processorService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {


            try
            {

                //var result = _processorService.EgolePay_CheckWallet();

                int number = 74856363;
                string formattedNumber = number.ToString("N0");

                //var result = ProcessorService.EgolePay_CheckWallet();
                //var cip = "BdkeBE2qGapkeruyfGVwav0RizJg==";
                //var result =  Decrypt(cip, true);
                //int first = 20, sec = 0, div = 0;
                //div = first / sec;


                //TempData["value"] = "someValueForNexOlabisi2015tRequest";
                //object value = TempData.Peek("value");


                //TempData.Peek("Td").ToString();

                var model = new IndexModel { Id = 1, Name = "Joy"};
                //var divid = 0 / 30;

                return View(model);
            }
            catch (System.Exception ex)
            {
                Log.Logger.Error("Error => {@error}", $"Message: {ex.Message}. Source: {ex.Source}. StackTrace: {ex.StackTrace}");
               //return RedirectToAction(nameof(Privacy));
                throw;
            }
            finally
            {
                //RedirectToAction(nameof(Index));
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
            //string key = _configuration["EAString"];
            string key = "CBSCLIENT1";
            // Dim key As String = DirectCast("CBSCLIENT1", String)  /// 3189481084

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




    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

   public class IndexModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}




  //Update [ReservationDb].[dbo].[Reservations]
  //Set [ReservationDb].[dbo].[Reservations].RoomText = [NewLeaveAppDb].[dbo].[Employees].FirstName + ' ' + [NewLeaveAppDb].[dbo].[Employees].LastName
  //From [ReservationDb].[dbo].[Reservations]
  //Inner Join [NewLeaveAppDb].[dbo].[Employees]
  //On [ReservationDb].[dbo].[Reservations].ID = [NewLeaveAppDb].[dbo].[Employees].ID



  //Update [AdventureWorks2019].[Person].[AddressNew]
  //Set [AdventureWorks2019].[Person].[AddressNew].AddressLine2 = [AdventureWorks2022].[Person].[Address].AddressLine1 + '_' + '2022'
  //From [AdventureWorks2019].[Person].[AddressNew]
  //Inner Join [AdventureWorks2022].[Person].[Address]
  //On [AdventureWorks2019].[Person].[AddressNew].AddressID = [AdventureWorks2022].[Person].[Address].AddressID

  //Update [ReservationDb].[dbo].[Reservations]
  //Set [ReservationDb].[dbo].[Reservations].RoomText = [NewLeaveAppDb].[dbo].[Employees].FirstName + ' ' + [NewLeaveAppDb].[dbo].[Employees].LastName
  //From [ReservationDb].[dbo].[Reservations]
  //Inner Join [NewLeaveAppDb].[dbo].[Employees]
  //On [ReservationDb].[dbo].[Reservations].ID = [NewLeaveAppDb].[dbo].[Employees].ID



  //Update [AdventureWorks2019].[Person].[AddressNew]
  //Set [AdventureWorks2019].[Person].[AddressNew].AddressLine2 = [AdventureWorks2022].[Person].[Address].AddressLine1 + '_' + '2022'
  //From [AdventureWorks2019].[Person].[AddressNew]
  //Inner Join [AdventureWorks2022].[Person].[Address]
  //On [AdventureWorks2019].[Person].[AddressNew].AddressID = [AdventureWorks2022].[Person].[Address].AddressID

