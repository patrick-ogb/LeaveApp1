using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewModel.LeaveRequestViewModel
{
    public class LeaveRequestCreateViewModel
    {
        [Required]
        public LeaveRequest LeaveRequest { get; set; }
        [Required]
        public List<LeaveType> LeaveTypeList { get; set; }
        [Required]
        public List<Employee> EmployeeList { get; set; }
    }
}
