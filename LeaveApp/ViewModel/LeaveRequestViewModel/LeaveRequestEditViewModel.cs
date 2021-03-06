﻿using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewModel.LeaveRequestViewModel
{
    public class LeaveRequestEditViewModel : LeaveRequest
    {
        public new string Id { get; set; }
        public List<Employee> EmployeeList { get; set; }
        public List<LeaveType> LeaveTypeList { get; set; }
    }
}
