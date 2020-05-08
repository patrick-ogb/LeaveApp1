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
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext context;

        public EmployeeService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> DeleteEmployee(int Id)
        {
            var employee = context.Employees.FirstOrDefault(emp => emp.Id == Id);
            context.Employees.Remove(employee);
            await context.SaveChangesAsync();
            return employee;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployee(int Id)
        {
            return await context.Employees.FirstOrDefaultAsync(emp => emp.Id == Id);
        }

        public async Task<Employee> UpdateEmployee(Employee employeeChange)
        {
            context.Employees.Update(employeeChange);
            await context.SaveChangesAsync();
            return employeeChange;
        }
    }
}
