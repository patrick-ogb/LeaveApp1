using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LeaveApp.Core.Entities
{
   public class LeaveType
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public string LeaveTypeEncryptedId { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Number Of Days")]
        public int NumberOfDays { get; set; }

        [Required]
        public string Discription { get; set; }

        [Required(ErrorMessage ="The Date Created field is required")]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; } 

        [Required(ErrorMessage ="The Date Modified field is required")]
        [Display(Name ="Date Modified")]
        public DateTime DateModified { get; set; }

        [ForeignKey("LeaveTypeId")]
        public ICollection<LeaveRequest> LeaveRequests { get; set; }
    }
}
