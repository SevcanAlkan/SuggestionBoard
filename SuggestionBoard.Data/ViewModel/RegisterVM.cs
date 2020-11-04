using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuggestionBoard.Data.ViewModel
{
    public sealed class RegisterVM
    {
        [Required(ErrorMessage = "E-Mail is required")]
        [DataType(DataType.Text)]
        [Display(Name = "E-Mail Address")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "E-Mail address cannot be longer than 100 characters and less than 5 characters")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Invalid password")]
        [Display(Name = "Password")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Password cannot be longer than 50 characters and less than 10 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password, ErrorMessage = "Invalid password")]
        [Compare("Password", ErrorMessage = "Confirm password is not equal to Password")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }
        public string GeneralError { get; set; }
    }
}
