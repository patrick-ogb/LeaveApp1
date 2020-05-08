using LeaveApp.Core.Entities;
using LeaveApp.Service.Abstract;
using LeaveApp.Service.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApp.Service.Concrete
{
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext context;

        public DepartmentService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Department> AddDepartment(Department department)
        {
            context.Departments.Add(department);
            await context.SaveChangesAsync();
            return department;
        }

        public async Task<Department> DeleteDepartment(int Id)
        {
            var department = context.Departments.FirstOrDefault(dept => dept.Id == Id);
            context.Departments.Remove(department);
            await context.SaveChangesAsync();
            return department;
        }

        public async Task<List<Department>> GetDepartments()
        {
            return await context.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartment(int Id)
        {
            return await context.Departments.FirstOrDefaultAsync(dept => dept.Id == Id);

        }

        public async Task<Department> UpdateDepartment(Department departmentChange)
        {
            context.Departments.Update(departmentChange);
            await context.SaveChangesAsync();
            return departmentChange;
        }
    }
}
