using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewModel.EmployeeViewModel
{
    public class EmployeeDetailsViewModel
    {
        public Employee Employee { get; set; }
        public Department DeptName { get; set; }
        public Level Level { get; set; }
        public string PageTitle { get; set; }
        public string EncryptedId { get; set; }
    }
}
