﻿using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewModel.EmployeeViewModel
{
    public class EmployeeEditViewModel : Employee
    {
        public new string Id { get; set; }

        public List<Department> DepartmentList { get; set; }

        public List<Level> LevelList { get; set; }
    }
}
