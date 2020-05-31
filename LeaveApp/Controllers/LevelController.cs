using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveApp.Core.Entities;
using LeaveApp.Security;
using LeaveApp.Service.Abstract;
using LeaveApp.ViewModel.LevelViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace LeaveApp.Controllers
{
    public class LevelController : Controller
    {
        private readonly ILevelService levelService;
        private readonly IDataProtector protector;

        public LevelController(ILevelService levelService,
                                    IDataProtectionProvider dataProtectionProvider,
                                    DataProtecionPurposeStrings dataProtecionPurposeStrings)
        {
            this.levelService = levelService;
            protector = dataProtectionProvider.CreateProtector(dataProtecionPurposeStrings.EmployeeIdRouteValue);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Level> level = (await levelService.GetLevels()).Select(lvl =>
            {
                lvl.LevelEncryptedId = protector.Protect(lvl.Id.ToString());
                return lvl;
            });
            return View(level);
        }

        [HttpGet]
        //[Authorize(Policy = "CreateRolePolicy")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Policy = "CreateRolePolicy")]
        public async Task<IActionResult> Create(LevelCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Level level = new Level
                {
                    Name = model.Level.Name,
                    Description = model.Level.Description
                };

                var dept = await levelService.AddLevel(level);

                ViewBag.message = $"Employee {model.Level.Name} is successfullly saved to DataBase";

                return RedirectToAction("details", new { id = level.Id });
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
            var level = await levelService.GetLevel(decryptedId);
            if (level == null)
            {
                return View("NotFound", decryptedId);
            }
            if (decryptedId < 1)
            {
                return RedirectToAction("Index");
            }
            LevelDetailsViewModel levelDetailsViewModel = new LevelDetailsViewModel
            {
                Level = level,
                EncryptedId = Id,
                PageTitle = "DEPARTMENT DETAILS"
            };
            return View(levelDetailsViewModel);
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
            Level level = await levelService.GetLevel(decryptedId);
            LevelEditViewModel model = new LevelEditViewModel
            {
                Id = Id,
                Name = level.Name,
                DateCreated = level.DateCreated,
                Description = level.Description,
                DateModified = level.DateModified,
                LevelEncryptedId = Id
            };
            return View(model);
        }

        [HttpPost]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> Edit(LevelEditViewModel model)
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

                Level level = await levelService.GetLevel(decryptedId);
                level.Name = model.Name;
                level.Description = model.Description;
                level.DateCreated = model.DateCreated;
                level.DateModified = model.DateModified;
              
                try
                {
                    await levelService.UpdateLevel(level);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, " Ensure and level Id is Valid");
                }

                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(string Id)
        {
            int lvlDecryptedId = Convert.ToInt32(protector.Unprotect(Id));
            if (lvlDecryptedId < 1)
            {
                return RedirectToAction("Index");
            }
            var level = await levelService.GetLevel(lvlDecryptedId);
            return View(level);
        }

        [HttpPost]
        //[Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> Delete(string Id, int Ids = 0)
        {
            int lvlDecryptedId = Convert.ToInt32(protector.Unprotect(Id));

            try
            {
                await levelService.DeleteLevel(lvlDecryptedId);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = $"The Employee Level cannot be deleted becouse of reference constraint of his/her Id in another database";
                return View("CustomError");
            }
        }
    }
}