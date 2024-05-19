using LeaveApp.Core.Entities;
using LeaveApp.ReportModel;
using LeaveApp.Security;
using LeaveApp.Service.Abstract;
using LeaveApp.Services.IServices;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewComponents
{

    public class DepartmentViewComponent: ViewComponent
    {
        private readonly IProcessorService _processorService;

        public DepartmentViewComponent(IProcessorService processorService)
        {
            _processorService = processorService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? pageIndex, string? searchText, HoldDepartmentVM? departments = null)
        {

            HoldDepartmentVM holdDepartmentVM = new HoldDepartmentVM();
            if(departments != null)
            {
                holdDepartmentVM = departments;
            }
            else
            {
                holdDepartmentVM = await _processorService.ProcessDepartment(pageIndex, searchText);
            }
            return View(holdDepartmentVM);
        }
    }
    

}
