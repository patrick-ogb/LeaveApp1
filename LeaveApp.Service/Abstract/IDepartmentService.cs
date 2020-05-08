using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApp.Service.Abstract
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetDepartments();
        Task<Department> GetDepartment(int Id);
        Task<Department> AddDepartment(Department department);
        Task<Department> DeleteDepartment(int Id);
        Task<Department> UpdateDepartment(Department departmentChange);
    }
}
