using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.ViewModel.AccountViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage ="The email field is required")]
        [EmailAddress]
        [Display(Name ="Enter Your Email")]
        public string Email { get; set; }
    }
}
