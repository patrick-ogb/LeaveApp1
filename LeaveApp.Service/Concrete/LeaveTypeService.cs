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
   public class LeaveTypeService : ILeaveTypeService
    {
        private readonly AppDbContext context;

        public LeaveTypeService(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<LeaveType> AddLeaveType(LeaveType leaveType)
        {
            context.LeaveTypes.Add(leaveType);
            await context.SaveChangesAsync();
            return leaveType;
        }

        public async Task<LeaveType> DeleteLeaveType(int Id)
        {
            var leaveType = context.LeaveTypes.FirstOrDefault(levlType => levlType.Id == Id);
            context.LeaveTypes.Remove(leaveType);
            await context.SaveChangesAsync();
            return leaveType;
        }

        public async Task<List<LeaveType>> GetLeaveTypes()
        {
            return await context.LeaveTypes.ToListAsync();
        }

        public async Task<LeaveType> GetLeaveType(int Id)
        {
            return await context.LeaveTypes.FirstOrDefaultAsync(lvType => lvType.Id == Id);

        }

        public async Task<LeaveType> UpdateLeaveType(LeaveType leaveChange)
        {
            context.LeaveTypes.Update(leaveChange);
            await context.SaveChangesAsync();
            return leaveChange;
        }
    }
}
