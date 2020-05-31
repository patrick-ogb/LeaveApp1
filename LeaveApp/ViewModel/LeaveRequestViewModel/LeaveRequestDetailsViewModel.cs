using LeaveApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewModel.LeaveRequestViewModel
{
    public class LeaveRequestDetailsViewModel
    {
        public LeaveRequest LeaveRequest { get; set; }
        public string PageTitle { get; set; }
        public string EncryptedId { get; set; }
    }
}
