using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LeaveApp.Core.Entities
{
   public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public string LeveRequestEncryptedId { get; set; }

        [Required(ErrorMessage ="Employee Id is Required")] //FOREIGN KEY
        public int EmployeeId { get; set; }

        [Required] //FOREIGN KEY
        public int LeaveTypeId { get; set; }

        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; } 

        [Display(Name = "Approved Date")]
        [Required]
        public DateTime ApprovalDate { get; set; }

        [Required(ErrorMessage ="Name of approval is required")]
        public string ApprovedBy { get; set; }
    }
}
