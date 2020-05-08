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
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly AppDbContext context;

        public LeaveRequestService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<LeaveRequest> AddLeaveRequest(LeaveRequest leaveRequest)
        {
            context.LeaveRequests.Add(leaveRequest);
            await context.SaveChangesAsync();
            return leaveRequest;
        }

        public async Task<LeaveRequest> DeleteLeaveRequest(int Id)
        {
            var leaveRequest = context.LeaveRequests.FirstOrDefault(emp => emp.Id == Id);
            context.LeaveRequests.Remove(leaveRequest);
            await context.SaveChangesAsync();
            return leaveRequest;
        }

        public async Task<List<LeaveRequest>> GetLeaveRequests()
        {
            return await context.LeaveRequests.ToListAsync();
        }

        public async Task<LeaveRequest> GetLeaveRequest(int Id)
        {
            return await context.LeaveRequests.FirstOrDefaultAsync(emp => emp.Id == Id);

        }

        public async Task<LeaveRequest> UpdateLeaveRequest(LeaveRequest leaveRequestChange)
        {
            context.LeaveRequests.Update(leaveRequestChange);
            await context.SaveChangesAsync();
            return leaveRequestChange;
        }
    }
}
