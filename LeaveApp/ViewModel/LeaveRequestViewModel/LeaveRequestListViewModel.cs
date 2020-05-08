using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewModel.LeaveRequestViewModel
{
    public class LeaveRequestListViewModel
    {
        public IEnumerable<LeaveRequest> LeaveRequests { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<LeaveType> LeaveTypes { get; set; }
    }
}
