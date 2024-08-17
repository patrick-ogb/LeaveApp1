using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace LeaveApp.Controllers
{
    public class ItemsController : Controller
    {

        private static readonly List<Item> AllItems = new List<Item>
        {
            new Item { Id = 1, Name = "Item 1" },
            new Item { Id = 2, Name = "Item 2" },
            new Item { Id = 3, Name = "Item 3" }
        };
        public IActionResult Index()
        {
            var model = new ItemViewModel
            {
                Items = AllItems,
                SelectedItemIds = new List<int>()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle selected items
                var selectedItems = AllItems.Where(item => model.SelectedItemIds.Contains(item.Id)).ToList();

                // For demo purpose, we can just return a view with the selected items
                return View("SelectedItems", selectedItems);
            }

            model.Items = AllItems;
            return View(model);
        }
    }


    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ItemViewModel
    {
        public List<Item> Items { get; set; }
        public List<int> SelectedItemIds { get; set; }
    }


}
