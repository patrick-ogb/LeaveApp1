using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System;
using LeaveApp.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LeaveApp.ViewModel.EmployeeViewModel;
using DocumentFormat.OpenXml.Bibliography;
using LeaveApp.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using LeaveApp.ViewModel;
using LeaveApp.Utilities;

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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }

    public class EmployeeEditMultiple
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
        public EmployeeEditMultiple EmployeeEditMultipleVM { get; set; }
        public EmployeeList EmployeeList { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }

    }



    public class LevelVM
    {
        public int Id { get; set; }
        public string LevelEncryptedId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

    }


    public class HoldLevelVM
    {
        public List<LevelVM> HoldLevelVMs { get; set; }
        public Pager Pager { get; set; }
        public int totalCount { get; set; }
        public string SearchText { get; set; }
        public FormData FormData { get; set; }
        public HoldLevelVM()
        {
            FormData = new FormData();
        }

    }

    public class FormData
    {
        public int FormID { get; set; }
        public string FormText { get; set; }

    }

    public class DepartmentVM
    {
        public int Id { get; set; }

        public string DepartmentEncryptedId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class HoldDepartmentVM
    {
        public List<DepartmentVM> HoldDepartmentVMs { get; set; }
        public Pager Pager { get; set; }
        public int totalCount { get; set; }
        public string SearchText { get; set; }

    }
    public class HoldEmployeeVM
    {
        public EmployeeListViewModel EmployeeListVMs { get; set; }
        public Pager Pager { get; set; }
        public int totalCount { get; set; }
        public string SearchText { get; set; }

    }

    public class HoldApplicationVM
    {
        public HoldLevelVM holdLevelVM { get; set; }
        public HoldDepartmentVM holdDepartmentVM { get; set; }
        public TabVM TabVM { get; set; }
        public HoldApplicationVM()
        {
            TabVM = new TabVM();   
        }

    }

}
