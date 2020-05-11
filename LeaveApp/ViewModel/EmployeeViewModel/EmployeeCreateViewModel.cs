using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewModel.EmployeeViewModel
{
    public class EmployeeCreateViewModel
    {
        [Required]
        public Employee Employee { get; set; }
        [Required]
        public List<Department> DepartmentList { get; set; }
        [Required]
        public List<Level> LevelList { get; set; }
    }
}
