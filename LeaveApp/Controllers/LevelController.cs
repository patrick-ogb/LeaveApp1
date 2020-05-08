using System.Threading.Tasks;
using LeaveApp.Core.Entities;
using LeaveApp.Service.Abstract;
using LeaveApp.ViewModel.LevelViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveApp.Controllers
{
    public class LevelController : Controller
    {
        private readonly ILevelService levelService;

        public LevelController(ILevelService levelService)
        {
            this.levelService = levelService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var level = await levelService.GetLevels();
            return View(level);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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
        public async Task<IActionResult> Details(int? Id)
        {
            var level = await levelService.GetLevel(Id.Value);
            if (level == null)
            {
                return View("NotFound", Id.Value);
            }
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            LevelDetailsViewModel levelDetailsViewModel = new LevelDetailsViewModel
            {
                Level = level,
                PageTitle = "DEPARTMENT DETAILS"
            };
            return View(levelDetailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var level = await levelService.GetLevel(Id.Value);
            return View(level);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Level levelChange)
        {
            if (ModelState.IsValid)
            {
                await levelService.UpdateLevel(levelChange);

                return RedirectToAction("Index");
            }
            return View(levelChange);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }
            var level = await levelService.GetLevel(Id.Value);
            return View(level);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            await levelService.DeleteLevel(Id);
            return RedirectToAction("Index");
        }
    }
}