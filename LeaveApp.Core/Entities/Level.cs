using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LeaveApp.Core.Entities
{
   public class Level
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        public string LevelEncryptedId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Display(Name="Date Created")]
        [DisplayFormat(DataFormatString ="{0:dd-mm-yyyy}", ApplyFormatInEditMode =true)]
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        [ForeignKey("LevelId")]
        public ICollection<Employee> Employees { get; set; }
    }
}
