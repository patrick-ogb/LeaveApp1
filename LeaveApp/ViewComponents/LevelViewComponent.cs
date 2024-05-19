using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using LeaveApp.Core.Entities;
using LeaveApp.Extensions;
using LeaveApp.ReportModel;
using LeaveApp.Security;
using LeaveApp.Service.Abstract;
using LeaveApp.Service.Concrete;
using LeaveApp.Services.IServices;
using LeaveApp.Utilities;
using MailKit.Search;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewComponents
{
    public class LevelViewComponent: ViewComponent
    {
        private readonly IProcessorService _processorService;

        public LevelViewComponent(IProcessorService processorService)
        {
            _processorService = processorService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? pageIndex, string? SearchText, HoldLevelVM? levels = null)
        {
            HoldLevelVM holdLevelVM = new HoldLevelVM();
            if(levels != null)
            {
                holdLevelVM = levels;
            }
            else
            {
                holdLevelVM = await _processorService.Processlevel(pageIndex, SearchText);
            }
            return View(holdLevelVM);
        }
    }
}
