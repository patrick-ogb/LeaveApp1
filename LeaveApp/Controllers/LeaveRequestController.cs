using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveApp.Core.Entities;
using LeaveApp.Security;
using LeaveApp.Service.Abstract;
using LeaveApp.ViewModel.LeaveRequestViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace LeaveApp.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestService leaveRequestService;
        private readonly ILeaveTypeService leaveTypeService;
        private readonly IEmployeeService employeeService;
        private readonly IDataProtector protector;

        public LeaveRequestController(ILeaveRequestService leaveRequestService,
                                        ILeaveTypeService leaveTypeService,
                                        IEmployeeService employeeService,
                                        IDataProtectionProvider dataProtectionProvider,
                                        DataProtecionPurposeStrings dataProtecionPurposeStrings)
        {
            this.leaveRequestService = leaveRequestService;
            this.leaveTypeService = leaveTypeService;
            this.employeeService = employeeService;
            protector = dataProtectionProvider.CreateProtector(dataProtecionPurposeStrings.EmployeeIdRouteValue);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<LeaveRequest> leaveRequests = (await leaveRequestService.GetLeaveRequests()).Select(lvlRequst =>
                {
                    lvlRequst.LeveRequestEncryptedId = protector.Protect(lvlRequst.Id.ToString());
                    return lvlRequst;
                });

                var leaveTypes = await leaveTypeService.GetLeaveTypes();
                var employees = await employeeService.GetEmployees();

                LeaveRequestListViewModel model = new LeaveRequestListViewModel
                {
                    Employees = employees,
                    LeaveTypes = leaveTypes,
                    LeaveRequests = leaveRequests
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorTitle = "Resource not available:";
                ViewBag.ErrorMessage = ex.Message;
                return View("CustomError");
            }
        }

        [HttpGet]
        //[Authorize(Policy = "CreateRolePolicy")]
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
        //[Authorize(Policy = "CreateRolePolicy")]
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
        public async Task<IActionResult> Details(string Id)
        {
            int decryptedId;
            int y;
            if (int.TryParse(Id, out y))
            {
                decryptedId = y;
            }
            else
            {
                decryptedId = Convert.ToInt32(protector.Unprotect(Id));
            }
            var leaveRequest = await leaveRequestService.GetLeaveRequest(decryptedId);
            if (leaveRequest == null)
            {
                return View("NotFound", decryptedId);
            }
            if (decryptedId < 1)
            {
                return RedirectToAction("Index");
            }
            LeaveRequestDetailsViewModel leaveRequestDetailsViewModel = new LeaveRequestDetailsViewModel
            {
                LeaveRequest = leaveRequest,
                EncryptedId = Id,
                PageTitle = "LEAVEREQUEST DETAILS"
            };
            return View(leaveRequestDetailsViewModel);
        }

        [HttpGet]
       // [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> Edit(string Id)
        {
            int decryptedId;
            int y;
            if (int.TryParse(Id, out y))
            {
                decryptedId = y;
            }
            else
            {
                decryptedId = Convert.ToInt32(protector.Unprotect(Id));
            }

            if (decryptedId < 1)
            {
                return RedirectToAction("Index");
            }

            var employees = await employeeService.GetEmployees();
            var leaveTyes = await leaveTypeService.GetLeaveTypes();
            LeaveRequest leaveRequest = await leaveRequestService.GetLeaveRequest(decryptedId);
            LeaveRequestEditViewModel model = new LeaveRequestEditViewModel
            {
                Id = Id,
                ApprovalDate = leaveRequest.ApprovalDate,
                EmployeeId = leaveRequest.EmployeeId,
                RequestDate = leaveRequest.RequestDate,
                ApprovedBy = leaveRequest.ApprovedBy,
                LeaveTypeId = leaveRequest.LeaveTypeId,
                LeveRequestEncryptedId = Id,
                EmployeeList = employees,
                LeaveTypeList = leaveTyes
            };
            return View(model);
        }

        [HttpPost]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> Edit(LeaveRequestEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                int decryptedId;
                int y;
                if (int.TryParse(model.Id, out y))
                {
                    decryptedId = y;
                }
                else
                {
                    decryptedId = Convert.ToInt32(protector.Unprotect(model.Id));
                }

                LeaveRequest leaveReqiuest = await leaveRequestService.GetLeaveRequest(decryptedId);
                leaveReqiuest.LeaveTypeId = model.LeaveTypeId;
                leaveReqiuest.ApprovalDate = model.ApprovalDate;
                leaveReqiuest.ApprovedBy = model.ApprovedBy;
                leaveReqiuest.EmployeeId = model.EmployeeId;
                leaveReqiuest.RequestDate = model.RequestDate;
                leaveReqiuest.LeveRequestEncryptedId = model.LeveRequestEncryptedId;
                try
                {
                await leaveRequestService.UpdateLeaveRequest(leaveReqiuest);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, " Ensure Id, EmployeeId and LeaveTypeId are Valid");
                }

                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(string Id)
        {
            int lvlRequestDecryptedId = Convert.ToInt32(protector.Unprotect(Id));
            if (lvlRequestDecryptedId < 1)
            {
                return RedirectToAction("Index");
            }
            var leaveRequest = await leaveRequestService.GetLeaveRequest(lvlRequestDecryptedId);
            return View(leaveRequest);
        }

        [HttpPost]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(string Id, int ids = 0)
        {
            int lvlRequestDecryptedId = Convert.ToInt32(protector.Unprotect(Id));

            try
            {
                await leaveRequestService.DeleteLeaveRequest(lvlRequestDecryptedId);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"The Leave Request cannot be deleted becouse of reference constraint of his/her Id in another database";
                return View("CustomError");
            }
        }
    }
}