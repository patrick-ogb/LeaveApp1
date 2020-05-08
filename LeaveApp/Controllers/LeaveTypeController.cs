using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveApp.Core.Entities;
using LeaveApp.Service.Abstract;
using LeaveApp.ViewModel.LeaveTypeViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveApp.Controllers
{
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeService leaveTypeService;

        public LeaveTypeController(ILeaveTypeService leaveTypeService)
        {
            this.leaveTypeService = leaveTypeService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var leaveType = await leaveTypeService.GetLeaveTypes();
            return View(leaveType);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LeaveTypeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                LeaveType leaveType = new LeaveType
                {
                    Name = model.LeaveType.Name,
                    NumberOfDays = model.LeaveType.NumberOfDays,
                    Discription = model.LeaveType.Discription
                };

                var dept = await leaveTypeService.AddLeaveType(leaveType);

                ViewBag.message = $"Employee {model.LeaveType.Name} is successfullly saved to DataBase";

                return RedirectToAction("details", new { id = leaveType.Id });
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? Id)
        {
            var leaveType = await leaveTypeService.GetLeaveType(Id.Value);
            if (leaveType == null)
            {
                return View("NotFound", Id.Value);
            }
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            LeaveTypeCreateViewModel leaveTypeCreateViewModel = new LeaveTypeCreateViewModel
            {
                LeaveType = leaveType,
                PageTitle = "LEAVETYPE DETAILS"
            };
            return View(leaveTypeCreateViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var leaveType = await leaveTypeService.GetLeaveType(Id.Value);
            return View(leaveType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LeaveType leaveTypeChange)
        {
            if (ModelState.IsValid)
            {
                await leaveTypeService.UpdateLeaveType(leaveTypeChange);

                return RedirectToAction("Index");
            }
            return View(leaveTypeChange);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var leaveType = await leaveTypeService.GetLeaveType(Id.Value);
            return View(leaveType);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            await leaveTypeService.DeleteLeaveType(Id);
            return RedirectToAction("Index");
        }
    }
}