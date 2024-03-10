using System;
using System.Collections.Generic;

namespace LeaveApp.Utilities
{
    public class EmployeeList
    {
        public int EmpId { get; set; }
        public string Name { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<EmployeeList>  GetEmployeeList(DateTime stateDate, DateTime endDate) { 
            return new List<EmployeeList>
            {
                new EmployeeList
                {
                    EmpId = 1, Name = "Name 1", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 2, Name = "Name 2", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 3, Name = "Name 3", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 4, Name = "Name 4", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 5, Name = "Name 5", StartDate = DateTime.Now, EndDate = endDate
                },
                 new EmployeeList
                {
                    EmpId = 6, Name = "Name 6", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 7, Name = "Name 7", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 8, Name = "Name 8", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 9, Name = "Name 9", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 10, Name = "Name 10", StartDate = DateTime.Now, EndDate = endDate
                },
                 new EmployeeList
                {
                    EmpId = 11, Name = "Name 11", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 12, Name = "Name 12", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 13, Name = "Name 13", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 14, Name = "Name 14", StartDate = DateTime.Now, EndDate = endDate
                },
                new EmployeeList
                {
                    EmpId = 15, Name = "Name 15", StartDate = DateTime.Now, EndDate = endDate
                }
            };
        }
    }

}
