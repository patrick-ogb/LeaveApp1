using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApp.Service.Abstract
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequest>> GetLeaveRequests();
        Task<LeaveRequest> GetLeaveRequest(int Id);
        Task<LeaveRequest> AddLeaveRequest(LeaveRequest level);
        Task<LeaveRequest> DeleteLeaveRequest(int Id);
        Task<LeaveRequest> UpdateLeaveRequest(LeaveRequest leaveRequestChange);
    }
}
