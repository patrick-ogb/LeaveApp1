using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LeaveApp.Core.Entities
{
   public class Employee
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address Reuied")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number required")]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please select department")] //FOREIGN KEY
        [Display(Name = "Department Id")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please select level")] //FOREIGN KEY
        [Display(Name = "Level Id")]
        public int LevelId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;

        [ForeignKey("EmployeeId")]
        public ICollection<LeaveRequest> LeaveRequests { get; set; }
    }
}
