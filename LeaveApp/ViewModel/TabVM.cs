using LeaveApp.Global;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LeaveApp.ViewModel
{
    public class TabVM
    {
        public Tab ActiveTab { get; set; }
    }

    public enum Tab
    {
        Employee,
        Department,
        Level
    }


    public class UserViewModel
    {
        public string Name { get; set; }
        public List<string> SelectedRoles { get; set; }
        public List<SelectListItem> AvailableRoles { get; set; }
        public List<int> SelectMultiple { get; set; }

        public string  QRCode { get; set; }
        
    }


}
