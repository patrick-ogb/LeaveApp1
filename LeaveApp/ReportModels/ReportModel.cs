using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System;
using LeaveApp.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LeaveApp.ReportModel
{
    public class EmployeeListVM
    {
        public int EmpId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }

    }
    public class EmployeeListVMList
    {
        public List<EmployeeListVM> EmployeeListVMs { get; set; }

    }
    public class EmployeeListVMSearchParams
    {
        [Required, DataType(DataType.Date), DisplayName(displayName: "Start Date")]

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date), DisplayName(displayName: "End Date")]

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }

    public class EmployeeCreate
    {
        public string SelectedList { get; set; }
    }

    public class HoldEmployeeListVM
    {
        public List<EmployeeListVM> HoldEmployeeListVMs { get; set; }
        public EmployeeListVMSearchParams EmployeeListVMSearchParams { get; set; }
        public Pager Pager { get; set; }
        public int totalCount { get; set; }
        public string SearchText { get; set; }
        public EmployeeCreate EmployeeCreateVM { get; set; }

    }
}
