using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewModel.LeaveTypeViewModel
{
    public class LeaveTypeCreateViewModel
    {
        [Required]
        public LeaveType LeaveType { get; set; }
        [Required]
        public string PageTitle { get; set; }
    }
}
