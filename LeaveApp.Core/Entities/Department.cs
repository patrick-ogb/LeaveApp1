using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LeaveApp.Core.Entities
{
   public class Department
    {
        [Key]
        public int Id { get; set; }
        
        [NotMapped]
        public string DepartmentEncryptedId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [ForeignKey("DepartmentId")]
        public ICollection<Employee> Employees { get; set; }
    }
}
