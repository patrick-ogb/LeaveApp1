using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApp.Service.Abstract
{
    public interface ILeaveTypeService
    {
        Task<List<LeaveType>> GetLeaveTypes();
        Task<LeaveType> GetLeaveType(int Id);
        Task<LeaveType> AddLeaveType(LeaveType leaveType);
        Task<LeaveType> DeleteLeaveType(int Id);
        Task<LeaveType> UpdateLeaveType(LeaveType leaveChange);
    }
}
