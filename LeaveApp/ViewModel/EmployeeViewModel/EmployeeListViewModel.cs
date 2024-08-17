using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewModel.EmployeeViewModel
{
    public class EmployeeListViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Department> Departments { get; set; }
        public IEnumerable<Level> Levels { get; set; }
        public EmpInfo empInfo { get; set; }
        public EmployeeListViewModel()
        {
            empInfo = new EmpInfo();
        }
    }

    public class EmpInfo
    {
        public string EmpInfoID { get; set; }
        public string EmpInfoName { get; set; }
    }
}
