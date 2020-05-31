using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveApp.Core.Entities;
using LeaveApp.Security;
using LeaveApp.Service.Abstract;
using LeaveApp.ViewModel.LeaveTypeViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace LeaveApp.Controllers
{
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeService leaveTypeService;
        private readonly IDataProtector protector;

        public LeaveTypeController(ILeaveTypeService leaveTypeService,
                                    IDataProtectionProvider dataProtectionProvider,
                                    DataProtecionPurposeStrings dataProtecionPurposeStrings)
        {
            this.leaveTypeService = leaveTypeService;
            protector = dataProtectionProvider.CreateProtector(dataProtecionPurposeStrings.EmployeeIdRouteValue);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<LeaveType> leaveTypes = (await leaveTypeService.GetLeaveTypes()).Select(lvlType =>
            {
                lvlType.LeaveTypeEncryptedId = protector.Protect(lvlType.Id.ToString());
                return lvlType;
            });
            return View(leaveTypes);
        }

        [HttpGet]
        //[Authorize(Policy = "CreateRolePolicy")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Policy = "CreateRolePolicy")]
        public async Task<IActionResult> Create(LeaveTypeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                LeaveType leaveType = new LeaveType
                {
                    Name = model.LeaveType.Name,
                    NumberOfDays = model.LeaveType.NumberOfDays,
                    Discription = model.LeaveType.Discription,
                };

                var dept = await leaveTypeService.AddLeaveType(leaveType);

                ViewBag.message = $"Employee {model.LeaveType.Name} is successfullly saved to DataBase";

                return RedirectToAction("details", new { id = leaveType.Id });
            }
            return View();
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

            var leaveType = await leaveTypeService.GetLeaveType(decryptedId);
            if (leaveType == null)
            {
                return View("NotFound", decryptedId);
            }
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            LeaveTypeDetailsViewModel leaveTypeDetailsViewModel = new LeaveTypeDetailsViewModel
            {
                LeaveType = leaveType,
                EncryptedId = Id,
                PageTitle = "LEAVETYPE DETAILS"
            };
            return View(leaveTypeDetailsViewModel);
        }

        [HttpGet]
        //[Authorize(Policy = "EditRolePolicy")]
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

            LeaveType leaveType = await leaveTypeService.GetLeaveType(decryptedId);
            LeaveTypeEditViewModel model = new LeaveTypeEditViewModel
            {
                Id = Id,
                Name = leaveType.Name,
                NumberOfDays = leaveType.NumberOfDays,
                Discription = leaveType.Discription,
                DateCreated = leaveType.DateCreated,
                DateModified = leaveType.DateModified,
                LeaveTypeEncryptedId = Id
            };
            return View(model);
        }

        [HttpPost]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> Edit(LeaveTypeEditViewModel model)
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

                if (model.NumberOfDays < 1)
                {
                    ModelState.AddModelError(string.Empty, $"Invalid number of day(s). {model.NumberOfDays} is unaceptable");
                    return View(model);
                }

                LeaveType leaveType = await leaveTypeService.GetLeaveType(decryptedId);
                leaveType.Name = model.Name;
                leaveType.NumberOfDays = model.NumberOfDays;
                leaveType.LeaveRequests = model.LeaveRequests;
                leaveType.DateCreated = model.DateCreated;
                leaveType.DateModified = model.DateModified;
                leaveType.Discription = model.Discription;
                leaveType.LeaveTypeEncryptedId = model.LeaveTypeEncryptedId;

                try
                {
                    await leaveTypeService.UpdateLeaveType(leaveType);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, " Ensure LeaveType Id is Valid");
                }

                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(string Id)
        {
            int lveTypeDecryptedId = Convert.ToInt32(protector.Unprotect(Id));
            if (lveTypeDecryptedId < 1)
            {
                return RedirectToAction("Index");
            }
            var leaveType = await leaveTypeService.GetLeaveType(lveTypeDecryptedId);
            return View(leaveType);
        }

        [HttpPost]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(string Id, int Ids = 0)
        {
            int lveTypeDecryptedId = Convert.ToInt32(protector.Unprotect(Id));

            try
            {
                await leaveTypeService.DeleteLeaveType(lveTypeDecryptedId);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"The Department cannot be deleted becouse of reference constraint of his/her Id in another database";
                return View("CustomError");
            }
        }
    }
}