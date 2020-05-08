using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveApp.Core.Entities;
using LeaveApp.Service.Abstract;
using LeaveApp.ViewModel.LeaveRequestViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveApp.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestService leaveRequestService;
        private readonly ILeaveTypeService leaveTypeService;
        private readonly IEmployeeService employeeService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService,
                                        ILeaveTypeService leaveTypeService,
                                        IEmployeeService employeeService)
        {
            this.leaveRequestService = leaveRequestService;
            this.leaveTypeService = leaveTypeService;
            this.employeeService = employeeService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                LeaveRequestListViewModel leaveRequestListViewModel = new LeaveRequestListViewModel
                {
                    Employees = await employeeService.GetEmployees(),
                    LeaveTypes = await leaveTypeService.GetLeaveTypes(),
                    LeaveRequests = await leaveRequestService.GetLeaveRequests()
                };
                return View(leaveRequestListViewModel);
            }
            catch (Exception)
            {

                return View("NotFound");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            LeaveRequestCreateViewModel leaveRequestCreateViewModel = new LeaveRequestCreateViewModel
            {
                LeaveTypeList = await leaveTypeService.GetLeaveTypes(),
                EmployeeList = await employeeService.GetEmployees()
            };
            return View(leaveRequestCreateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequestCreateViewModel model)
        {
            LeaveRequest leaveRequest = new LeaveRequest
            {
                EmployeeId = model.LeaveRequest.EmployeeId,
                LeaveTypeId = model.LeaveRequest.LeaveTypeId,
                ApprovalDate = model.LeaveRequest.ApprovalDate,
                ApprovedBy = model.LeaveRequest.ApprovedBy
            };

            var leaveReq = await leaveRequestService.AddLeaveRequest(leaveRequest);
            return RedirectToAction("details", new { id = leaveReq.Id });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? Id)
        {
            var leaveRequest = await leaveRequestService.GetLeaveRequest(Id.Value);
            if (leaveRequest == null)
            {
                return View("NotFound", Id.Value);
            }
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            LeaveRequestDetailsViewModel leaveRequestDetailsViewModel = new LeaveRequestDetailsViewModel
            {
                LeaveRequest = leaveRequest,
                PageTitle = "LEAVEREQUEST DETAILS"
            };
            return View(leaveRequestDetailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var leaveRequest = await leaveRequestService.GetLeaveRequest(Id.Value);
            return View(leaveRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LeaveRequest leaveRequestChange)
        {
            if (ModelState.IsValid)
            {
                await leaveRequestService.UpdateLeaveRequest(leaveRequestChange);

                return RedirectToAction("Index");
            }
            return View(leaveRequestChange);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var leaveRequest = await leaveRequestService.GetLeaveRequest(Id.Value);
            return View(leaveRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            await leaveRequestService.DeleteLeaveRequest(Id);
            return RedirectToAction("Index");
        }
    }
}