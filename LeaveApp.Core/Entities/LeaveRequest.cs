using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LeaveApp.Core.Entities
{
   public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }
        [Required] //FOREIGN KEY
        public int EmployeeId { get; set; }

        [Required] //FOREIGN KEY
        public int LeaveTypeId { get; set; }

        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; } = DateTime.Now.ToUniversalTime();

        [Display(Name = "Approved Date")]
        [Required]
        public DateTime ApprovalDate { get; set; }

        [Required]
        public string ApprovedBy { get; set; }
    }
}
