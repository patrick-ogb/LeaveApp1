using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewModel.LeaveTypeViewModel
{
    public class LeaveTypeDetailsViewModel
    {
        public LeaveType LeaveType { get; set; }
        public string PageTitle { get; set; }
        public string EncryptedId { get; set; }
    }
}
